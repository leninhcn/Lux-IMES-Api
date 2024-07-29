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
    /// 站点行为配置
    /// </summary>
    [Verify]
    [Route("datacenter/mntnactionbase")]
    //[ApiExplorerSettings(GroupName = "DataCenter")]
    public class MntnActionBaseController : BaseController
    {
        public IMntnActionBaseService ActionBaseService;
        public ISysUserService UserService;
        public MntnActionBaseController(IMntnActionBaseService actionbaseService
            , ISysUserService userService)
        {
            ActionBaseService = actionbaseService;
            UserService = userService;
        }
        /// <summary>
        /// 获取jobtype
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:queryjobtype")]
        [HttpGet("queryjobtype")]
        public IActionResult JobTypeList([FromQuery] MActionJobTypeBaseQueryDto param)
        {
            var site = HttpContext.GetSite();
            param.Site = site;
            return SUCCESS(ActionBaseService.GetJobType(param), TIME_FORMAT_FULL);
        }

        /// <summary>
        /// 查询jobid
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:queryjobid")]
        [HttpGet("queryjobid")]
        public IActionResult JobIdQuery([FromQuery] MActionJobBaseQueryDto param)
        {
            var site = HttpContext.GetSite();
            param.Site = site;
            return SUCCESS(ActionBaseService.GetJobId(param), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 查询joblink
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:queryjoblink")]
        [HttpGet("queryjoblink")]
        public IActionResult JobLinkQuery([FromQuery] MActionJobLinkQueryDto param)
        {
            var site = HttpContext.GetSite();
            param.Site = site;
            return SUCCESS(ActionBaseService.GetJobLink(param), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 查询jobgroup
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:queryjobgroup")]
        [HttpGet("queryjobgroup")]
        public IActionResult JobGroupQuery([FromQuery] MActionGroupBaseQueryDto param)
        {
            var site = HttpContext.GetSite();
            param.Site = site;
            return SUCCESS(ActionBaseService.GetJobGroup(param), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 查询jobgroupdetail
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:queryjobgroupdetail")]
        [HttpGet("queryjobgrouplinkdetail")]
        public IActionResult JobGroupLinkDetailQuery([FromQuery] MActionGroupBaseQueryDto param)
        {
            //var site = HttpContext.GetSite();
            //param.Site = site;
            return SUCCESS(ActionBaseService.GetJobGroupLinkDetail(param), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 查询stationaction，查询stationname,条件为stationtype=stationtype and StationName=stationname；查询stationtype,条件为stationtype=stationtype and stationname='0'
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:querystationaction")]
        [HttpGet("querystationaction")]
        public IActionResult StationActionQuery([FromQuery] MStationActionQueryDto param)
        {
            var site = HttpContext.GetSite();
            param.Site = site;
            return SUCCESS(ActionBaseService.GetStationAction(param), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 新增jobtype
        /// </summary>
        /// <returns></returns>
        [HttpPost("addjobtype")]
        [Log(Title = "addjobtype", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:addjobtype")]
        public IActionResult AddJobType([FromBody] MActionJobTypeBase param)
        {
            param.CreateTime= DateTime.Now;
            param.CreateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = ActionBaseService.InsertJobType(param);
            return  ToResponse(new ApiResult(result == "OK"?(int)ResultCode.SUCCESS:(int)ResultCode.CUSTOM_ERROR,result=="OK"?"sucess": result, result));
        }

        /// <summary>
        /// 新增jobid
        /// </summary>
        /// <returns></returns>
        [HttpPost("addjobid")]
        [Log(Title = "addjobid", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:addjobid")]
        public IActionResult AddJobId([FromBody] MActionJobBase param)
        {
            param.CreateTime = DateTime.Now;
            param.CreateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = ActionBaseService.InsertJobId(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }

        /// <summary>
        /// 新增joblink
        /// </summary>
        /// <returns></returns>
        [HttpPost("addjoblink")]
        [Log(Title = "addjoblink", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:addjoblink")]
        public IActionResult AddJobLink([FromBody] MActionJobLink param)
        {
            param.CreateTime = DateTime.Now;
            param.CreateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = ActionBaseService.InsertJobLink(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 修改jobtype
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updatejobtype")]
        [Log(Title = "updatejobtype", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:updatejobtype")]
        public IActionResult UpdateJobType([FromBody] MActionJobTypeBase param)
        {
            param.UpdateTime = DateTime.Now;
            param.UpdateEmpno = HttpContext.GetName();
            string result = ActionBaseService.UpdateJobType(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 修改jobid
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updatejobid")]
        [Log(Title = "updatejobid", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:updatejobid")]
        public IActionResult UpdateJobId([FromBody] MActionJobBase param)
        {
            param.UpdateTime = DateTime.Now;
            param.UpdateEmpno = HttpContext.GetName();
            string result = ActionBaseService.UpdateJobId(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 修改jobtype
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updatejoblink")]
        [Log(Title = "updatejoblink", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:updatejoblink")]
        public IActionResult UpdateJobLink([FromBody] MActionJobLink param)
        {
            param.UpdateTime = DateTime.Now;
            param.UpdateEmpno = HttpContext.GetName();
            string result = ActionBaseService.UpdateJobLink(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 删除jobtype
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete("deletejobtype")]
        [Log(Title = "deletejobtype", BusinessType = BusinessType.DELETE)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:deletejobgrouplink")]
        public IActionResult DeleteJobtype([FromBody] MActionJobTypeBase param)
        {
            param.UpdateTime = DateTime.Now;
            param.UpdateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = ActionBaseService.DeleteJobType(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS:(int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 删除jobid
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete("deletejobid")]
        [Log(Title = "deletejobid", BusinessType = BusinessType.DELETE)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:deletejobgrouplink")]
        public IActionResult DeleteJobId([FromBody] MActionJobBase param)
        {
            param.UpdateTime = DateTime.Now;
            param.UpdateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = ActionBaseService.DeleteJobID(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 删除joblink
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete("deletejoblink")]
        [Log(Title = "deletejoblink", BusinessType = BusinessType.DELETE)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:deletejobgrouplink")]
        public IActionResult DeleteJobLink([FromBody] MActionJobLink param)
        {
            param.UpdateTime = DateTime.Now;
            param.UpdateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = ActionBaseService.DeleteJobLink(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 删除jobgroup
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete("deletejobgroup")]
        [Log(Title = "deletejobgroup", BusinessType = BusinessType.DELETE)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:deletejobgrouplink")]
        public IActionResult DeleteJobGroup([FromBody] MActionGroupBase param)
        {
            param.UpdateTime = DateTime.Now;
            param.UpdateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = ActionBaseService.DeleteJobGroup(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 新增或者修改MStationAction
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("insertorupdatestationaction")]
        [Log(Title = "insertorupdatestationaction", BusinessType = BusinessType.UPDATE)]
       // [ActionPermissionFilter(Permission = "datacenter:mntnactionbase:insertorupdatestationaction")]
        public IActionResult InsertOrUpdateStationAction([FromBody] MStationAction param)
        {
            param.UpdateTime = DateTime.Now;
            param.UpdateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = ActionBaseService.InsertOrUpdateStationAction(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 新增jobgrouplink
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("insertjobgrouplink")]
        [Log(Title = "insertjobgrouplink", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:insertjobgrouplink")]
        public IActionResult InsertGroupLink([FromBody] MActionGroupLink param)
        {
            param.CreateTime = DateTime.Now;
            //param.UpdateEmpno = HttpContext.GetName();
            param.CreateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = ActionBaseService.InsertJobGroupLink(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 新增jobgrouplink
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updatejobgrouplink")]
        [Log(Title = "updatejobgrouplink", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:updatejobgrouplink")]
        public IActionResult UpdateGroupLink([FromBody] MActionGroupLink param)
        {
            // param.CreateTime = DateTime.Now;
            //param.CreateEmpno = HttpContext.GetName();
            param.UpdateTime = DateTime.Now;
            param.UpdateEmpno = HttpContext.GetName();
            //param.Site = HttpContext.GetSite();
            string result = ActionBaseService.UpdateJobGroupLink(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 删除jobgrouplink
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("deletejobgrouplink")]
        [Log(Title = "deletejobgrouplink", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:deletejobgrouplink")]
        public IActionResult DeleteGroupLink([FromBody] MActionGroupLink param)
        {
            // param.CreateTime = DateTime.Now;
            //param.CreateEmpno = HttpContext.GetName();
            param.UpdateTime = DateTime.Now;
            param.UpdateEmpno = HttpContext.GetName();
            //param.Site = HttpContext.GetSite();
            string result = ActionBaseService.DeleteJobGroupLink(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 新增jobgroup
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("insertjobgroup")]
        [Log(Title = "insertjobgroup", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:insertjobgroup")]
        public IActionResult InsertJobGroup([FromBody] MActionGroupBase param)
        {
            param.CreateTime = DateTime.Now;
            //param.UpdateEmpno = HttpContext.GetName();
            param.CreateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = ActionBaseService.InsertJobGroup(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 更新jobgroup
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updatejobgroup")]
        [Log(Title = "updatejobgroup", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnactionbase:updatejobgroup")]
        public IActionResult UpdateJobGroup([FromBody] MActionGroupBase param)
        {
            // param.CreateTime = DateTime.Now;
            //param.CreateEmpno = HttpContext.GetName();
            param.UpdateTime = DateTime.Now;
            param.UpdateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = ActionBaseService.UpdateJobGroup(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
    }
}
