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
    /// 云监控
    /// </summary>
    //[Verify]
    [Route("cloudmonitor")]
    //[ApiExplorerSettings(GroupName = "DataCenter")]
    public class MCloudMonitorController : BaseController
    {
        private readonly ICloudMonitorService _CloudMonitor;
        public MCloudMonitorController(ICloudMonitorService CloudMonitor)
        {
            _CloudMonitor = CloudMonitor;
        }

        /// <summary>
        /// 获取所有单据
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:listwarehouse")]
        [HttpGet("listticket")]
        public IActionResult ListTicket([FromQuery] PTicketStatusQueryDto parm)
        {
            parm.site = HttpContext.GetSite();
            return SUCCESS(_CloudMonitor.GetLsit(parm), TIME_FORMAT_FULL);
        }

        /// <summary>
        /// 获取单据明细
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:querywarehouse")]
        [HttpGet("queryticketdetail")]
        public IActionResult QueryTicketDetail(string ID)
        {
            return SUCCESS(_CloudMonitor.GetInfo(ID), TIME_FORMAT_FULL);
        }

        /// <summary>
        /// 获取单据流程
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:querywarehouse")]
        [HttpGet("querytickettravel")]
        public IActionResult QueryTicketTravel(string ID)
        {
            return SUCCESS(_CloudMonitor.GetTravel(ID), TIME_FORMAT_FULL);
        }

        /// <summary>
        /// 新增单据,暂时未使用
        /// </summary>
        /// <returns></returns>
        [HttpPost("addticket")]
        [Log(Title = "新增", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:addwarehouse")]
        public IActionResult AddTicket([FromBody] PTicketStatus parm)
        {
            parm.Site = HttpContext.GetSite();
            parm.CreateEmpno = HttpContext.GetName();
            parm.CreateTime = DateTime.Now;
            string result = _CloudMonitor.AddTicket(parm);
            return  ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS:(int)ResultCode.CUSTOM_ERROR,result=="OK"?"sucess": result, result));
        }

        /// <summary>
        /// 签核单据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updateticket")]
        [Log(Title = "修改", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:updatewarehouse")]
        public async Task< IActionResult> Update([FromBody] PTicketStatus param)
        {
            param.Site = HttpContext.GetSite();
            param.UpdateEmpno = HttpContext.GetName();
            param.CreateTime = DateTime.Now;
            var result = _CloudMonitor.UpdateTicket(param);
            result.Lange=HttpContext.GetLang();
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            // return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
            return await ToResponseByErrorcode(result);
        }
        /// <summary>
        /// 指派单据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("assignticket")]
        [Log(Title = "修改", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:updatewarehouse")]
        public async Task<IActionResult> AssignTicket([FromBody] PTicketStatus param)
        {
            param.Site = HttpContext.GetSite();
            param.UpdateEmpno = HttpContext.GetName();
            param.CreateTime = DateTime.Now;
            var result = _CloudMonitor.AssignTicket(param);
            result.Lange = HttpContext.GetLang();
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            //return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
            return await ToResponseByErrorcode(result);
        }
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updateticketstatus")]
        [Log(Title = "修改", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:updatewarehouse")]
        public async Task<IActionResult> UpdateStatus([FromBody] PTicketStatus param)
        {
            param.Site = HttpContext.GetSite();
            param.UpdateEmpno = HttpContext.GetName();
            param.CreateTime = DateTime.Now;
            var result = _CloudMonitor.UpdateTicketStatus(param);
            result.Lange=HttpContext.GetLang();
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            //return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
            return await ToResponseByErrorcode(result);
        }

        /// <summary>
        /// 报表获取
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("getreport")]
        //[ActionPermissionFilter(Permission = "warehousemanagement:mwarehouse:updatewarehouse")]
        public async Task<IActionResult> GetReport([FromQuery] PTicketReportQueryDto param)
        {
            param.Site = HttpContext.GetSite();
            var result = _CloudMonitor.GetReport(param);
            result.Lange = HttpContext.GetLang();
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            //return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
            return await ToResponseByErrorcode(result);
        }

    }
}
