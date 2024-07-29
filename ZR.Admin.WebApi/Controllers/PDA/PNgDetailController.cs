using Microsoft.AspNetCore.Mvc;
using ZR.Model.Dto;
using ZR.Model.Business;
using ZR.Admin.WebApi.Filters;
using ZR.Service.Business.IBusinessService;
using JinianNet.JNTemplate;
using System.Security.Policy;
using ZR.Service.Business;

//创建时间：2024-04-20
namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// 不良品统计
    /// </summary>
    [Verify]
    [Route("ng/PNgDetail")]
    public class PNgDetailController : BaseController
    {
        /// <summary>
        /// 不良品统计接口
        /// </summary>
        private readonly IPNgDetailService _PNgDetailService;

        public PNgDetailController(IPNgDetailService PNgDetailService)
        {
            _PNgDetailService = PNgDetailService;
        }

    
        
        /// <summary>
        /// 查询STATION_TYPE 下拉框
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("SnTravel")]
        public IActionResult QuerySnTravel(string WorkOrder)
        {
            var response = _PNgDetailService.QuerySnTravel(WorkOrder);
            return SUCCESS(response);
        }

        /// <summary>
        /// 根据STATION_TYPE查询设备
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("SnTravelMacine")]
        public IActionResult SnTravelMacine(string SnTravelstr)
        {
            var response = _PNgDetailService.SnTravelMacine(SnTravelstr);
            return SUCCESS(response);
        }




        /// <summary>
        /// 查询不良品统计列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "ng:pngdetail:list")]
        public IActionResult QueryPNgDetail([FromQuery] PNgDetailQueryDto parm)
        {
            parm.Site = HttpContext.GetSite();
            var response = _PNgDetailService.GetList(parm);
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询不良品统计
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("statistics")]
        //  [ActionPermissionFilter(Permission = "ng:statistics:list")]
        public IActionResult QueryPNgDetailstatistics([FromQuery] PNgDetailQueryDto parm)
        {
            var response = _PNgDetailService.GetListstatistics(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询设备
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("ListMaChine")]
        [ActionPermissionFilter(Permission = "ng:pngdetail:list")]
        public IActionResult ListMaChine()
        {
            var site = HttpContext.GetSite();
            var response = _PNgDetailService.ListMaChine(site);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询不良品统计详情
        /// </summary>
        /// <param name="DetailId"></param>
        /// <returns></returns>
        [HttpGet("Id")]
        [ActionPermissionFilter(Permission = "ng:pngdetail:query")]
        public IActionResult GetPNgDetail(long Id)
        {
            var response = _PNgDetailService.GetInfo(Id);
            var info = response.Adapt<PNgDetail>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加不良品统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "ng:pngdetail:add")]
        [Log(Title = "不良品统计", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPNgDetail([FromBody] PNgDetailDto parm)
        {
            var modal = parm.Adapt<PNgDetail>().ToCreate(HttpContext);
            modal.Site = HttpContext.GetSite();
            modal.CreateEmpno = HttpContext.GetName();
            modal.CreateTime = DateTime.Now;
            var response = _PNgDetailService.AddPNgDetail(modal, parm.DataList);
            return SUCCESS(response);
        }

        /// <summary>
        /// 更新不良品统计
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "ng:pngdetail:edit")]
        [Log(Title = "不良品统计", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePNgDetail([FromBody] PNgDetailDto parm)
        {
            var modal = parm.Adapt<PNgDetail>().ToUpdate(HttpContext);
            modal.UpdateEmpno= HttpContext.GetName();
            modal.UpdateTime = DateTime.Now;
            var response = _PNgDetailService.UpdatePNgDetail(modal);
            return ToResponse(response);
        }

        /// <summary>
        /// 删除不良品统计
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "ng:pngdetail:delete")]
        [Log(Title = "不良品统计", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePNgDetail(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }
            int restlt=0;
            foreach (var item in idsArr)
            {
                PNgDetail pNgDetail = _PNgDetailService.GetInfo(item);
                pNgDetail.Enabled = "N";
                pNgDetail.UpdateEmpno = HttpContext.GetName();
                pNgDetail.UpdateTime = DateTime.Now;
                restlt = _PNgDetailService.DelPNgDetail(pNgDetail);
            }
            return ToResponse(restlt);
        }




    }
}