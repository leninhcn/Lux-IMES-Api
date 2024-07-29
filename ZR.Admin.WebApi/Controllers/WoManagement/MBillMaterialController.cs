using Microsoft.AspNetCore.Mvc;
using ZR.Model.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;

//创建时间：2024-05-16
namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Verify]
    [Route("business/MBillMaterial")]
    public class MBillMaterialController : BaseController
    {
        /// <summary>
        /// 接口
        /// </summary>
        private readonly IMBillMaterialService _MBillMaterialService;

        public MBillMaterialController(IMBillMaterialService MBillMaterialService)
        {
            _MBillMaterialService = MBillMaterialService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "womanagement:bomworkorder:list")]
        public IActionResult QueryMBillMaterial([FromQuery] MBillMaterialQueryDto parm)
        {
            var response = _MBillMaterialService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "womanagement:bomworkorder:query")]
        public IActionResult GetMBillMaterial(string Id)
        {
            var response = _MBillMaterialService.GetInfo(Id);
            
            var info = response.Adapt<MBillMaterial>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "womanagement:bomworkorder:add")]
        [Log(Title = "", BusinessType = BusinessType.INSERT)]
        public IActionResult AddMBillMaterial([FromBody] MBillMaterialDto parm)
        {
            var modal = parm.Adapt<MBillMaterial>().ToCreate(HttpContext);

            var response = _MBillMaterialService.AddMBillMaterial(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "womanagement:bomworkorder:edit")]
        [Log(Title = "", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateMBillMaterial([FromBody] MBillMaterialDto parm)
        {
            var modal = parm.Adapt<MBillMaterial>().ToUpdate(HttpContext);
            var response = _MBillMaterialService.UpdateMBillMaterial(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "womanagement:bomworkorder:delete")]
        [Log(Title = "", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteMBillMaterial(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _MBillMaterialService.Delete(idsArr);

            return ToResponse(response);
        }




    }
}