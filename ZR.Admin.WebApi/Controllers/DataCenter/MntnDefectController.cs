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
using ZR.Service;

namespace ZR.Admin.WebApi.Controllers.System
{
    /// <summary>
    /// 不良维护
    /// </summary>
    [Verify]
    [Route("datacenter/mntndefect")]
    //[ApiExplorerSettings(GroupName = "DataCenter")]
    public class MntnDefectController : BaseController
    {
        public IMntnDefectService DefectService;
        public ISysUserService UserService;
        public MntnDefectController(IMntnDefectService defectService
            , ISysUserService userService)
        {
            DefectService = defectService;
            UserService = userService;
        }
        /// <summary>
        /// 获取所有不良code信息
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "datacenter:mntndefect:list")]
        [HttpGet("list")]
        public IActionResult List([FromQuery] MntnDefect defect)
        {
            var site = HttpContext.GetSite();
            defect.Site = site;
            return SUCCESS(DefectService.GetDefect(defect), TIME_FORMAT_FULL);
        }

        /// <summary>
        /// 查询不良code信息
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "datacenter:mntndefect:list")]
        [HttpGet("query")]
        public IActionResult Query([FromQuery] MntnDefect defect)
        {
            var site = HttpContext.GetSite();
            defect.Site = site;
            return SUCCESS(DefectService.QueryDefect(defect), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 查询所有的机种信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("model")]
        public DataTable GetModel()
        {
            var site = HttpContext.GetSite();
            return DefectService.GetModel();
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        [Log(Title = "不良维护", BusinessType = BusinessType.INSERT)]
        [ActionPermissionFilter(Permission = "datacenter:mntndefect:add")]
        public IActionResult Add([FromBody] MDefect defect)
        {
            defect.CreateEmpno = HttpContext.GetName();
            defect.Site = HttpContext.GetSite();
            string result = DefectService.Insert(defect);
            return  ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS:(int)ResultCode.CUSTOM_ERROR,result=="OK"?"sucess": result, result));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="defect"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Log(Title = "不良维护", BusinessType = BusinessType.UPDATE)]
        [ActionPermissionFilter(Permission = "datacenter:mntndefect:update")]
        public IActionResult Update([FromBody] MDefect defect)
        {
            defect.UpdateEmpno = HttpContext.GetName();
            string result = DefectService.Update(defect);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="defect"></param>
        /// <returns></returns>
        [HttpPost("updatestatus")]
        [Log(Title = "不良维护", BusinessType = BusinessType.UPDATE)]
        [ActionPermissionFilter(Permission = "datacenter:mntndefect:updatestatus")]
        public IActionResult UpdateStatus([FromBody] MDefect defect)
        {
            defect.UpdateEmpno = HttpContext.GetName();
            string result = DefectService.UpdateStatus(defect);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        // <summary>
        // 删除
        // </summary>
        // <returns></returns>
        [HttpDelete("{delete}")]
        [ActionPermissionFilter(Permission = "datacenter:mntndefect:remove")]
        [Log(Title = "不良维护", BusinessType = BusinessType.DELETE)]
        public IActionResult Remove(string delete)
        {
            MDefect defect = new MDefect();
            defect.UpdateEmpno = HttpContext.GetName();
            defect.Enabled = "N";
            defect.UpdateTime = DateTime.Now;
            string result = DefectService.Delete(defect, delete);
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
        [ActionPermissionFilter(Permission = "datacenter:mntndefect:export")]
        public IActionResult MntndutyExport([FromQuery] MntnDefect parm)
        {
            parm.Site = HttpContext.GetSite();
            var list = DefectService.GetDefect(parm);

            var result = ExportExcelMini(list.Result, "mntndefect", "不良");
            return ExportExcel(result.Item2, result.Item1);
        }
    }
}
