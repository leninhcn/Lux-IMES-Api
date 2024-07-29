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
using Newtonsoft.Json;

namespace ZR.Admin.WebApi.Controllers.System
{
    /// <summary>
    /// 不良原因
    /// </summary>
    [Verify]
    [Route("datacenter/mntnreason")]
    [ApiExplorerSettings(GroupName = "DataCenter")]
    public class MntnReasonController : BaseController
    {
        private readonly IMReasonService _MReasonService;
        private readonly ISysUserService _UserService;
        private readonly ICollectErrorService _CollectErrorService;
        public MntnReasonController(IMReasonService mreasonService
            , ISysUserService userService,ICollectErrorService CollectErrorService)
        {
            _MReasonService = mreasonService;
            _UserService = userService;
            _CollectErrorService = CollectErrorService;
        }
        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "datacenter:mntnreason:list")]
        [HttpGet("list")]
        public IActionResult List([FromQuery] MReasonQueryDto parm)
        {
            parm.Site=HttpContext.GetSite();
            return SUCCESS(_MReasonService.GetList(parm), TIME_FORMAT_FULL);
        }

        /// <summary>
        /// 查询详细信息
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "datacenter:mntnreason:query")]
        [HttpGet("query")]
        public IActionResult Query(long id)
        {
            return SUCCESS(_MReasonService.GetInfo(id), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        [Log(Title = "不良原因维护", BusinessType = BusinessType.INSERT)]
        [ActionPermissionFilter(Permission = "datacenter:mntnreason:add")]
        public IActionResult Add([FromBody] MReason parm)
        {
            parm.CreateEmpno = HttpContext.GetName();
            parm.Site = HttpContext.GetSite();
            var resultinfo = _MReasonService.AddMReason(parm);
            //记录异常信息         
            if(_CollectErrorService.Status()=="1")
            {
            if(resultinfo.Result != "OK")
              {
                PCollecterrorLog pCollecterror = new PCollecterrorLog();
                pCollecterror.Site= HttpContext.GetSite();
                pCollecterror.ProgramMent = HttpContext.GetReferer();
                pCollecterror.EmpNo= HttpContext.GetName();
                pCollecterror.ClentIp = HttpContext.GetClientUserIp();
                pCollecterror.ClientType = "Backend";
                pCollecterror.Errcode = "MntnReasonController:Add:"+resultinfo.Errcode;
                pCollecterror.RequestParam = JsonConvert.SerializeObject(parm);
                pCollecterror.ResponseResults = resultinfo.Result;
                _CollectErrorService.Add(pCollecterror);
               }
            }
            return  ToResponse(new ApiResult(resultinfo.Result == "OK" ? (int)ResultCode.SUCCESS:(int)ResultCode.CUSTOM_ERROR, resultinfo.Result=="OK"?"sucess": resultinfo.Result, resultinfo.Result));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Log(Title = "不良原因维护", BusinessType = BusinessType.UPDATE)]
        [ActionPermissionFilter(Permission = "datacenter:mntnreason:update")]
        public IActionResult Update([FromBody] MReason param)
        {
            param.UpdateEmpno = HttpContext.GetName();
            string result = _MReasonService.UpdateMReason(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updatestatus")]
        [Log(Title = "不良原因维护", BusinessType = BusinessType.UPDATE)]
        [ActionPermissionFilter(Permission = "datacenter:mntnreason:updatestatus")]
        public IActionResult Updatestatus([FromBody] MReason param)
        {
            param.UpdateEmpno = HttpContext.GetName();
            string result = _MReasonService.UpdateMReasonStatus(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }

        // <summary>
        // 删除
        // </summary>
        // <returns></returns>
        [HttpDelete("{delete}")]
        [ActionPermissionFilter(Permission = "datacenter:mntnreason:remove")]
        [Log(Title = "不良原因维护", BusinessType = BusinessType.DELETE)]
        public IActionResult Remove(string delete)
        {
                MReasonDto param = new MReasonDto();
                param.UpdateEmpno = HttpContext.GetName();
                param.Enabled = "N";
                param.UpdateTime = DateTime.Now;
                string result = _MReasonService.DeleteMReason(param, delete);
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
        [ActionPermissionFilter(Permission = "datacenter:mntnreason:export")]
        public IActionResult MntndutyExport([FromQuery] MReasonQueryDto parm)
        {
            parm.Site = HttpContext.GetSite();
            var list = _MReasonService.GetList(parm);

            var result = ExportExcelMini(list.Result, "mntnreason", "不良原因");
            return ExportExcel(result.Item2, result.Item1);
        }
    }
}
