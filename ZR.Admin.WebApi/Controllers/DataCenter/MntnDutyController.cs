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
    /// 不良原因
    /// </summary>
    [Verify]
    [Route("datacenter/mduty")]
    [ApiExplorerSettings(GroupName = "DataCenter")]
    public class MDutyController : BaseController
    {
        private readonly IMDutyService _MDutyService;
        private readonly ISysUserService _UserService;
        public MDutyController(IMDutyService MDutyService
            , ISysUserService userService)
        {
            _MDutyService = MDutyService;
            _UserService = userService;
        }
        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list")]
        public IActionResult List([FromQuery] MDutyQueryDto parm)
        {
            parm.Site = HttpContext.GetSite();
            return SUCCESS(_MDutyService.GetList(parm), TIME_FORMAT_FULL);
        }

        /// <summary>
        /// 查询详细信息
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:query")]
        [HttpGet("query")]
        public IActionResult Query(long id)
        {
            return SUCCESS(_MDutyService.GetInfo(id), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        [Log(Title = "新增", BusinessType = BusinessType.INSERT)]
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:add")]
        public IActionResult Add([FromBody] MDuty parm)
        {
            parm.Site = HttpContext.GetSite();
            parm.CreateEmpno = HttpContext.GetName();
            string result = _MDutyService.AddMReason(parm);
            return  ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS:(int)ResultCode.CUSTOM_ERROR,result=="OK"?"sucess": result, result));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Log(Title = "修改", BusinessType = BusinessType.UPDATE)]
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:update")]
        public IActionResult Update([FromBody] MDuty param)
        {
            param.Site = HttpContext.GetSite();
            param.UpdateEmpno = HttpContext.GetName();
            string result = _MDutyService.UpdateMReason(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updatestatus")]
        [Log(Title = "修改", BusinessType = BusinessType.UPDATE)]
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:updatestatus")]
        public IActionResult UpdateStatus([FromBody] MDuty param)
        {
            param.Site = HttpContext.GetSite();
            param.UpdateEmpno = HttpContext.GetName();
            string result = _MDutyService.UpdateMReasonStatus(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }

        // <summary>
        // 删除
        // </summary>
        // <returns></returns>
        [HttpDelete("{delete}")]
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:remove")]
        [Log(Title = "不良原因维护", BusinessType = BusinessType.DELETE)]
        public IActionResult Remove(string delete)
        {
                MDutyDto param = new MDutyDto();
                param.UpdateEmpno = HttpContext.GetName();
                param.Enabled = "N";
                param.UpdateTime = DateTime.Now;
                string result = _MDutyService.DeleteMReason(param, delete);
                // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
                return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));   
        }
        /// <summary>
        /// 用户导出
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("export")]
        [Log(Title = "用户导出", BusinessType = BusinessType.EXPORT)]
        [ActionPermissionFilter(Permission = "datacenter:mntnduty:export")]
        public IActionResult MntndutyExport([FromQuery] MDutyQueryDto parm)
        {
            parm.Site = HttpContext.GetSite();
            var list = _MDutyService.GetList(parm);

            var result = ExportExcelMini(list.Result, "mntnduty", "责任类型");
            return ExportExcel(result.Item2, result.Item1);
        }
    }
}
