using Microsoft.AspNetCore.Mvc;
using ZR.Model.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;

//创建时间：2024-04-27
namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// 辅材上下线
    /// </summary>
    [Verify]
    [Route("business/PMaterialsStatus")]
    public class PMaterialsStatusController : BaseController
    {
        /// <summary>
        /// 辅材上下线接口
        /// </summary>
        private readonly IPMaterialsStatusService _PMaterialsStatusService;

        public PMaterialsStatusController(IPMaterialsStatusService PMaterialsStatusService)
        {
            _PMaterialsStatusService = PMaterialsStatusService;
        }

        /// <summary>
        /// 查询辅材上下线列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "business:pmaterialsstatus:list")]
        public IActionResult QueryPMaterialsStatus([FromQuery] PMaterialsStatusQueryDto parm)
        {
            parm.Site= HttpContext.GetSite();   
            var response = _PMaterialsStatusService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询辅材上下线详情
        /// </summary>
        /// <param name="MaterialsId"></param>
        /// <returns></returns>
        [HttpGet("{MaterialsId}")]
        [ActionPermissionFilter(Permission = "business:pmaterialsstatus:query")]
        public IActionResult GetPMaterialsStatus(long MaterialsId)
        {
            var response = _PMaterialsStatusService.GetInfo(MaterialsId);

            var info = response.Adapt<PMaterialsStatus>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加辅材上下线
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        // [ActionPermissionFilter(Permission = "business:pmaterialsstatus:add")]
        [HttpPost("AddPMaterialsStatus")]
        [Log(Title = "辅材上下线", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPMaterialsStatus([FromBody] PMaterialsStatus parm)
        {
            var modal = parm.Adapt<PMaterialsStatus>().ToCreate(HttpContext);
            modal.Site = HttpContext.GetSite();
            modal.CreateEmpno = HttpContext.GetName();
            modal.Id = Tools.GenerateLongUUID();
            modal.Enabled = "Y";
            var response = _PMaterialsStatusService.AddPMaterialsStatus(modal);
            return SUCCESS(response);
        }

        /// <summary>
        /// 更新辅材上下线
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "business:pmaterialsstatus:edit")]
        [Log(Title = "辅材上下线", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePMaterialsStatus([FromBody] PMaterialsStatusDto parm)
        {
            var modal = parm.Adapt<PMaterialsStatus>().ToUpdate(HttpContext);
            var response = _PMaterialsStatusService.UpdatePMaterialsStatus(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除辅材上下线
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:pmaterialsstatus:delete")]
        [Log(Title = "辅材上下线", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePMaterialsStatus(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _PMaterialsStatusService.Delete(idsArr);

            return ToResponse(response);
        }




    }
}