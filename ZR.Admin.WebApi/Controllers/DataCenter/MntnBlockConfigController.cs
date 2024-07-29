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

namespace ZR.Admin.WebApi.Controllers.System
{
    /// <summary>
    /// 不良原因
    /// </summary>
    [Verify]
    [Route("datacenter/mblockconfig")]
    [ApiExplorerSettings(GroupName = "DataCenter")]
    public class MntnBlockConfigController : BaseController
    {
        private readonly IMntnBlockConfigService _MntnBlockConfigService;
        private readonly ISysUserService _UserService;
        public MntnBlockConfigController(IMntnBlockConfigService MntnBlockConfigService
            , ISysUserService userService)
        {
            _MntnBlockConfigService = MntnBlockConfigService;
            _UserService = userService;
        }
        /// <summary>
        /// 获取route所有信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_route")]
        public IActionResult ListRoute(string parm)
        {
            var site=HttpContext.GetSite();
            return SUCCESS(_MntnBlockConfigService.GetListRoute(parm,site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取line所有信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_line")]
        public IActionResult ListLine(string parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MntnBlockConfigService.GetListLine(parm, site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取站点类型所有信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_station_type")]
        public IActionResult ListStaionType(string parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MntnBlockConfigService.GetListStationType(parm,site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取站点所有信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_station")]
        public IActionResult ListStaion(string parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MntnBlockConfigService.GetListStation(parm, site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取type所有信息分页
        /// </summary>
        /// <returns></returns>
       // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_typefenye")]
        public IActionResult ListTypefenye([FromQuery] MBlockConfigTypeQueryDto parm)
        {
            parm.site = HttpContext.GetSite();
            return SUCCESS(_MntnBlockConfigService.GetListType(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取type所有信息不分页
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_type")]
        public IActionResult ListType([FromQuery] MBlockConfigTypeQueryDto parm)
        {
            parm.site = HttpContext.GetSite();
            return SUCCESS(_MntnBlockConfigService.GetListType(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取sqltype所有信息不分页
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_sqltype")]
        public IActionResult ListSqlType([FromQuery] MBlockConfigTypeQueryDto parm)
        {
            parm.site = HttpContext.GetSite();
            return SUCCESS(_MntnBlockConfigService.GetListSqlType(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取value所有信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_value")]
        public IActionResult ListValue([FromQuery] MBlockConfigValueQueryDto parm)
        {
            parm.site = HttpContext.GetSite();
            return SUCCESS(_MntnBlockConfigService.GetListValue(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取sql所有信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_sql")]
        public IActionResult ListSql([FromQuery] MBlockConfigProsqlQueryDto parm)
        {
            parm.site = HttpContext.GetSite();
            return SUCCESS(_MntnBlockConfigService.GetListSql(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 查询type详细信息
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "datacenter:mntnduty:query")]
        [HttpGet("query_type")]
        public IActionResult Query(string id)
        {
            return SUCCESS(_MntnBlockConfigService.GetInfoType(id), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 查询value详细信息
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "datacenter:mntnduty:query")]
        [HttpGet("query_value")]
        public IActionResult QueryValue(string id)
        {
            return SUCCESS(_MntnBlockConfigService.GetInfoValue(id), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 查询sql详细信息
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "datacenter:mntnduty:query")]
        [HttpGet("query_sql")]
        public IActionResult QuerySql(string id)
        {
            return SUCCESS(_MntnBlockConfigService.GetInfoSql(id), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 新增type
        /// </summary>
        /// <returns></returns>
        [HttpPost("add_type")]
        [Log(Title = "新增", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnduty:add")]
        public IActionResult AddType([FromBody] MBlockConfigType parm)
        {
            parm.Site = HttpContext.GetSite();
            parm.CreateEmpno = HttpContext.GetName();
            string result = _MntnBlockConfigService.AddType(parm);
            return  ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS:(int)ResultCode.CUSTOM_ERROR,result=="OK"?"sucess": result, result));
        }
        /// <summary>
        /// 新增value
        /// </summary>
        /// <returns></returns>
        [HttpPost("add_value")]
        [Log(Title = "新增", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnduty:add")]
        public IActionResult AddValue([FromBody] MBlockConfigValue parm)
        {
            parm.Site= HttpContext.GetSite();
            parm.CreateEmpno = HttpContext.GetName();
            string result = _MntnBlockConfigService.AddValue(parm);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 新增sql
        /// </summary>
        /// <returns></returns>
        [HttpPost("add_sql")]
        [Log(Title = "新增", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnduty:add")]
        public IActionResult AddSql([FromBody] MBlockConfigProsql parm)
        {
            parm.Site = HttpContext.GetSite();
            parm.CreateEmpno = HttpContext.GetName();
            string result = _MntnBlockConfigService.AddSql(parm);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }

        /// <summary>
        /// 修改type
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("update_type")]
        [Log(Title = "修改", BusinessType = BusinessType.UPDATE)]
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:update")]
        public IActionResult Update([FromBody] MBlockConfigType param)
        {
            param.Site = HttpContext.GetSite();
            param.UpdateEmpno = HttpContext.GetName();
            string result = _MntnBlockConfigService.UpdateType(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 修改value
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("update_value")]
        [Log(Title = "修改", BusinessType = BusinessType.UPDATE)]
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:update")]
        public IActionResult UpdateValue([FromBody] MBlockConfigValue param)
        {
            param.Site = HttpContext.GetSite();
            param.UpdateEmpno = HttpContext.GetName();
            string result = _MntnBlockConfigService.UpdateValue(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 修改sql
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("update_sql")]
        [Log(Title = "修改", BusinessType = BusinessType.UPDATE)]
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:update")]
        public IActionResult UpdateSql([FromBody] MBlockConfigProsql param)
        {
            param.Site = HttpContext.GetSite();
            param.UpdateEmpno = HttpContext.GetName();
            string result = _MntnBlockConfigService.UpdateSql(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }

         //<summary>
         //删除type
         //</summary>
         //<returns></returns>
        [HttpDelete("type/{delete}")]
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:remove")]
        [Log(Title = "不良原因维护", BusinessType = BusinessType.DELETE)]
        public IActionResult RemoveType(string delete)
        {
            MBlockConfigTypeDto param = new MBlockConfigTypeDto();
            param.UpdateEmpno = HttpContext.GetName();
            param.Enabled = "N";
            param.UpdateTime = DateTime.Now;
            string result = _MntnBlockConfigService.DeleteType(param, delete);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        //<summary>
        //删除value
        //</summary>
        //<returns></returns>
        [HttpDelete("value/{delete}")]
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:remove")]
        [Log(Title = "不良原因维护", BusinessType = BusinessType.DELETE)]
        public IActionResult RemoveValue(string delete)
        {
            MBlockConfigValueDto param = new MBlockConfigValueDto();
            param.UpdateEmpno = HttpContext.GetName();
            param.Enabled = "N";
            param.UpdateTime = DateTime.Now;
            string result = _MntnBlockConfigService.DeleteValue(param, delete);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        //<summary>
        //删除sql
        //</summary>
        //<returns></returns>
        [HttpDelete("sql/{delete}")]
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:remove")]
        [Log(Title = "停用SQL", BusinessType = BusinessType.DELETE)]
        public IActionResult RemoveSql(string delete)
        {
            MBlockConfigProsqlDto param = new MBlockConfigProsqlDto();
            param.UpdateEmpno = HttpContext.GetName();
            param.Enabled = "N";
            param.UpdateTime = DateTime.Now;
            string result = _MntnBlockConfigService.DeleteSql(param, delete);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
    }
}
