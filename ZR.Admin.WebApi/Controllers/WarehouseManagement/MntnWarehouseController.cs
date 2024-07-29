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
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;
using System.Security.Policy;

namespace ZR.Admin.WebApi.Controllers.System
{
    /// <summary>
    /// 仓储信息
    /// </summary>
    //[Verify]
    [Route("warehousemanagement/mwarehouse")]
    //[ApiExplorerSettings(GroupName = "DataCenter")]
    public class MntnWarehouseController : BaseController
    {
        private readonly IMntnWarehouseService _MWareHouse;
        public MntnWarehouseController(IMntnWarehouseService MWareHouse)
        {
            _MWareHouse = MWareHouse;
        }
        /// <summary>
        /// 获取所有仓储信息
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:listwarehouse")]
        [HttpGet("listwarehouse")]
        public IActionResult ListWareHouse([FromQuery] MWarehouseQueryDto parm)
        {
            parm.site = HttpContext.GetSite();
            return SUCCESS(_MWareHouse.GetListWareHouse(parm), TIME_FORMAT_FULL);
        }

        /// <summary>
        /// 查询详细仓储信息
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:querywarehouse")]
        [HttpGet("querywarehouse")]
        public IActionResult QueryWareHouse(string warehouseCode)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MWareHouse.GetInfoWareHouse(warehouseCode, site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 新增仓储
        /// </summary>
        /// <returns></returns>
        [HttpPost("addwarehouse")]
        [Log(Title = "新增", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:addwarehouse")]
        public async Task< IActionResult> AddWareHouse([FromBody] MWarehouse parm)
        {
            parm.Site = HttpContext.GetSite();
            parm.CreateEmp = HttpContext.GetName();
            parm.CreateTime = DateTime.Now;
            var result = _MWareHouse.AddWareHouse(parm);
            errorcode = result.ErrCode;
            result.Lange=HttpContext.GetLang();
            return await ToResponseByErrorcode(result);
            // return await ToResponseByErrorcode(new ResponseErrorCodeDto{ Lang = HttpContext.GetLang() }, new ApiResult(result.Result == "OK" ? (int)ResultCode.SUCCESS:(int)ResultCode.CUSTOM_ERROR,result.Result=="OK"?"sucess": result.GetResMSG, result.Data));
        }

        /// <summary>
        /// 修改仓储
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updatewarehouse")]
        [Log(Title = "修改", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:updatewarehouse")]
        public async Task< IActionResult> Update([FromBody] MWarehouse param)
        {
            param.Site = HttpContext.GetSite();
            param.UpdateEmp = HttpContext.GetName();
            param.CreateTime = DateTime.Now;
            var  result = _MWareHouse.UpdateWareHouse(param);
            errorcode = result.ErrCode;
            result.Lange = HttpContext.GetLang();
            return await ToResponseByErrorcode(result);
            //return await ToResponseByErrorcode(new ResponseErrorCodeDto { Lang=HttpContext.GetLang()},new ApiResult(result.Result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result.Result == "OK" ? "sucess" : result.GetResMSG, result.Data));
        }
        /// <summary>
        /// 获取储位信息
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:listlocal")]
        [HttpGet("listlocal")]
        public async Task< IActionResult> ListLocal([FromQuery] MLocationQueryDto parm)
        {
            parm.site = HttpContext.GetSite();
            var result = _MWareHouse.GetListLocation(parm);
            result.Lange=HttpContext.GetLang();
            return await ToResponseByErrorcode(result);
            // return SUCCESS(_MWareHouse.GetListLocation(parm), TIME_FORMAT_FULL);
            //return await ToResponseByErrorcode(new ResponseErrorCodeDto { Lang = HttpContext.GetLang() }, new ApiResult(result.Result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result.GetResMSG, result.Data));
        }

        /// <summary>
        /// 查询详细储位信息
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:querylocal")]
        [HttpGet("querylocal")]
        public IActionResult QueryLocal(MLocationQueryDto param)
        {
            param.site = HttpContext.GetSite();
            return SUCCESS(_MWareHouse.GetInfoLocation(param), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 新增储位
        /// </summary>
        /// <returns></returns>
        [HttpPost("addlocal")]
        [Log(Title = "新增", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:addlocal")]
        public async Task<IActionResult> AddLocal([FromBody] MLocation parm)
        {
            parm.Site = HttpContext.GetSite();
            parm.CreateEmp = HttpContext.GetName();
            parm.CreateTime = DateTime.Now;
            var result = _MWareHouse.AddLocation(parm);
            result.Lange = HttpContext.GetLang();
            return await ToResponseByErrorcode(result);
           // return ToResponse(new ApiResult(result.Result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result.Result == "OK" ? "sucess" : result.GetResMSG, result.Data));
        }

        /// <summary>
        /// 修改储位
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updatelocal")]
        [Log(Title = "修改", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:updatelocal")]
        public async Task<IActionResult> UpdateLocal([FromBody] MLocation param)
        {
            param.Site = HttpContext.GetSite();
            param.UpdateEmp = HttpContext.GetName();
            param.CreateTime = DateTime.Now;
            var  result = _MWareHouse.UpdateLocal(param);
            result.Lange = HttpContext.GetLang();
            return await ToResponseByErrorcode(result);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            //return await ToResponseByErrorcode(new ResponseErrorCodeDto {Lang=HttpContext.GetLang() },new ApiResult(result.Result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result.GetResMSG, result.Data));
        }
    }
}
