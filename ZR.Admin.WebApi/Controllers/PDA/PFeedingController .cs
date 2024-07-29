using Microsoft.AspNetCore.Mvc;
using ZR.Model.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;
using ZR.Model.Dto.ProdDto;

//创建时间：2024-04-27
namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// 上下料
    /// </summary>
    [Verify]
    [Route("business/PFeeding")]
    public class PFeedingController : BaseController
    {
        /// <summary>
        /// 上下料
        /// </summary>
        private readonly IPFeedingService _PFeedingService;

        public PFeedingController(IPFeedingService PFeedingService)
        {
            _PFeedingService = PFeedingService;
        }

        /// <summary>
        /// 查询上下料
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "business:pfeeding:list")]
        public IActionResult QueryPFeeding([FromQuery] PFeedingQueryDto parm)
        {
            parm.Site = HttpContext.GetSite();
            var response = _PFeedingService.GetList(parm);
            return SUCCESS(response);
        }
        /// <summary>
        /// 查询上下料
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "business:pfeeding:query")]
        public IActionResult GetPFeeding(long Id)
        {
            var response = _PFeedingService.GetInfo(Id);

            var info = response.Adapt<PFeeding>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 查询上下料
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("orderupdate")]
        [AllowAnonymousAttribute]
        public IActionResult GetPFeedingupdateorder(string Id)
        {
            var response = _PFeedingService.GetPFeedingupdateorder(long.Parse(Id));
            return SUCCESS(response);
        }

         //PDA订单完结
        [HttpGet("order")]
        public IActionResult QueryPFeedingorder()
        {
            var response = _PFeedingService.QueryPFeedingorder();
            return SUCCESS(response);
        }



        /// <summary>
        /// 添加上下料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddPFeeding")]
        [Log(Title = "上下料", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPFeeding([FromBody] PFeedingDto parm)
        {
            var modal = parm.Adapt<PFeeding>().ToCreate(HttpContext);
            modal.Site = HttpContext.GetSite();
            modal.CreateEmpno = HttpContext.GetName();
            modal.Enabled = "Y";
            var response = _PFeedingService.AddPFeeding(modal);
            return SUCCESS(response);
        }




        ///// <summary>
        ///// 上料判断
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("AddPFeedingPd")]
        //[Log(Title = "上料判断", BusinessType = BusinessType.INSERT)]
        //public IActionResult AddPFeedingPd([FromBody] PFeedingDto parm)
        //{
        //    var modal = parm.Adapt<PFeeding>().ToCreate(HttpContext);
        //    modal.Site = HttpContext.GetSite();
        //    modal.CreateEmpno = HttpContext.GetName();
        //    modal.Enabled = "Y";
        //    var response = _PFeedingService.AddPFeedingPd(modal);
        //    return SUCCESS(response);
        //}


        /// <summary>
        /// 投入产出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("stationInout")]
        [AllowAnonymousAttribute]
        [Log(Title = "投入产出", BusinessType = BusinessType.INSERT)]
        public IActionResult stationInout([FromBody] stationInout parm)
        {
            if (parm.OutPutQty <= 0)
            {
                parm.OutPutQty = 0;
            }
            parm.Site = HttpContext.GetSite();
            parm.CreateEmpno = HttpContext.GetName();
            var response = _PFeedingService.stationInout(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// Agv投入产出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Agv")]
        [AllowAnonymousAttribute]
        [Log(Title = "Agv", BusinessType = BusinessType.INSERT)]
        public IActionResult stationInoutAgv([FromBody] stationInout parm)
        {
            if (parm.OutPutQty <= 0)
            {
                parm.OutPutQty = 0;
            }
            var response = _PFeedingService.stationInoutAgv(parm);
            return SUCCESS(response);
        }

        

        /// <summary>
        /// 更新上下料
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "business:pfeeding:edit")]
        [Log(Title = "上下料", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePFeeding([FromBody] PFeedingDto parm)
        {
            var modal = parm.Adapt<PFeeding>().ToUpdate(HttpContext);
            var response = _PFeedingService.UpdatePFeeding(modal);

            return ToResponse(response);
        }






        /// <summary>
        /// 删除上下料
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:pfeeding:delete")]
        [Log(Title = "上下料", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePFeeding(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _PFeedingService.Delete(idsArr);

            return ToResponse(response);
        }
    }
}