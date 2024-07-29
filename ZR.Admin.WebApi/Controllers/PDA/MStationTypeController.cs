using Microsoft.AspNetCore.Mvc;
using ZR.Model.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;

//创建时间：2024-05-18
namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Verify]
    [Route("business/MStationType")]
    public class MStationTypeController : BaseController
    {
        /// <summary>
        /// 接口
        /// </summary>
        private readonly IMStationTypeService _MStationTypeService;

        public MStationTypeController(IMStationTypeService MStationTypeService)
        {
            _MStationTypeService = MStationTypeService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "business:mstationtype:list")]
        public IActionResult QueryMStationType([FromQuery] MStationTypeQueryDto parm)
        {
            var response = _MStationTypeService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "business:mstationtype:query")]
        public IActionResult GetMStationType(int Id)
        {
            var response = _MStationTypeService.GetInfo(Id);
            
            var info = response.Adapt<MStationType>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "business:mstationtype:add")]
        [Log(Title = "", BusinessType = BusinessType.INSERT)]
        public IActionResult AddMStationType([FromBody] MStationTypeDto parm)
        {
            var modal = parm.Adapt<MStationType>().ToCreate(HttpContext);

            var response = _MStationTypeService.AddMStationType(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "business:mstationtype:edit")]
        [Log(Title = "", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateMStationType([FromBody] MStationTypeDto parm)
        {
            var modal = parm.Adapt<MStationType>().ToUpdate(HttpContext);
            var response = _MStationTypeService.UpdateMStationType(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:mstationtype:delete")]
        [Log(Title = "", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteMStationType(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _MStationTypeService.Delete(idsArr);

            return ToResponse(response);
        }




    }
}