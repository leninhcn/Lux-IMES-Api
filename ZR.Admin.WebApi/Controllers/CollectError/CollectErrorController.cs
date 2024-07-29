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
using Microsoft.IdentityModel.Tokens;
using System.Drawing;

namespace ZR.Admin.WebApi.Controllers.System
{
    /// <summary>
    /// 收集不良日志
    /// </summary>
    //[Verify]
    [Route("collecterror")]
    //[ApiExplorerSettings(GroupName = "CollectError")]
    public class CollectErrorController : BaseController
    {
        private readonly ICollectErrorService _CollectErrorService;
        private readonly ISysUserService _UserService;
        public CollectErrorController(ICollectErrorService CollectErrorService
            , ISysUserService userService)
        {
            _CollectErrorService = CollectErrorService;
            _UserService = userService;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        [Log(Title = "新增", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "datacenter:mntnduty:add")]
        public IActionResult Add([FromBody] PCollecterrorLog parm)
        {
            parm.Site = HttpContext.GetSite();
            parm.EmpNo = HttpContext.GetName();
            if(!parm.ResponseResults.IsNullOrEmpty())
            {
                parm.ResponseResults = parm.ResponseResults.Length>=2000 ? parm.ResponseResults.Substring(0,1999) : parm.ResponseResults;
            }
            string result = _CollectErrorService.Add(parm);
            return  ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS:(int)ResultCode.CUSTOM_ERROR,result=="OK"?"sucess": result, result));
        }

    }
}
