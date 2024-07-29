using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;
using ZR.Admin.WebApi.Filters;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Service.System.IService;
using ZR.Model.Business;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ZR.Model.Dto;
using ZR.Service.IService;
using ZR.Model;
using Aliyun.OSS;

namespace ZR.Admin.WebApi.Controllers.System
{
    /// <summary>
    /// 入库程式
    /// </summary>
    [Verify]
    [Route("warehousemanagement/warehousing")]
    //[ApiExplorerSettings(GroupName = "DataCenter")]
    public class WareHousingController : BaseController
    {
        private readonly IWareHousingService _WareHousing;
        public WareHousingController(IWareHousingService WareHousing)
        {
            _WareHousing = WareHousing;
        }

        /// <summary>
        /// 入库操作
        /// </summary>
        /// <returns></returns>
        [HttpPost("action")]
        [Log(Title = "操作", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:addwarehouse")]
        public async Task<IActionResult> Action([FromBody] WareHousingDto parm)
        {
            parm.Site = HttpContext.GetSite();
            parm.UpdateEmpno = HttpContext.GetName();
            parm.UpdateTime = DateTime.Now;
            var (bs,msg,result) = await _WareHousing.WareHousingInData(parm);
            return ToResponse(new ApiResult(bs ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR,msg, result ==null?"":result));
        }

    }
}
