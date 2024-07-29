using Microsoft.AspNetCore.Mvc;
using ZR.Model.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;

//创建时间：2024-05-21
namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Verify]
    [Route("business/PSnTravel")]
    public class PSnTravelController : BaseController
    {
        /// <summary>
        /// 接口
        /// </summary>
        private readonly IPSnTravelService _PSnTravelService;

        public PSnTravelController(IPSnTravelService PSnTravelService)
        {
            _PSnTravelService = PSnTravelService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "business:psntravel:list")]
        public IActionResult QueryPSnTravel([FromQuery] PSnTravelQueryDto parm)
        {
            var response = _PSnTravelService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "business:psntravel:query")]
        public IActionResult GetPSnTravel(int Id)
        {
            var response = _PSnTravelService.GetInfo(Id);
            
            var info = response.Adapt<PSnTravel>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "business:psntravel:add")]
        [Log(Title = "", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPSnTravel([FromBody] PSnTravelDto parm)
        {
            var modal = parm.Adapt<PSnTravel>().ToCreate(HttpContext);

            var response = _PSnTravelService.AddPSnTravel(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "business:psntravel:edit")]
        [Log(Title = "", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePSnTravel([FromBody] PSnTravelDto parm)
        {
            var modal = parm.Adapt<PSnTravel>().ToUpdate(HttpContext);
            var response = _PSnTravelService.UpdatePSnTravel(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:psntravel:delete")]
        [Log(Title = "", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePSnTravel(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _PSnTravelService.Delete(idsArr);

            return ToResponse(response);
        }




    }
}