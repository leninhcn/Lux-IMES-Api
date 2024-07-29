using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.WoManagement
{
    [Route("womanagement/mntnwostatus")]
    [ApiController]
    public class MntnWoStatusController : BaseController
    {
        private readonly IMntnWoStatusService _mntnWoStatusService;
        public MntnWoStatusController(IMntnWoStatusService mntnWoStatusService)
        {
            _mntnWoStatusService = mntnWoStatusService;
        }
        [HttpGet("getdetail")]
        public async Task<IActionResult> GetWoBase(string workOrder)
        {
            var site=HttpContext.GetSite();
            var wobase = await _mntnWoStatusService.GetWoBase(workOrder, site);
            return ToResponse(new ApiResult(wobase.Count != 0?(int)ResultCode.SUCCESS:(int)ResultCode.CUSTOM_ERROR, wobase.Count != 0 ? "sucess" : "工单不存在", wobase));
        }
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] MntnWoStatusDto parm)
        {
            string result = "";
            parm.Site=HttpContext.GetSite();
            parm.UpdateEmpno = HttpContext.GetName();
            var wobase = await _mntnWoStatusService.GetWoBase(parm.WorkOrder, parm.Site);
            if(wobase.Count == 0)
            {
                return ToResponse(new ApiResult(wobase.Count != 0 ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, wobase.Count != 0 ? "sucess" : "工单不存在", wobase));
            }      
            result = await _mntnWoStatusService.CheckWoBase(parm);
            if(result !="OK")
            {
                // return Ok(new { ok=result=="OK", result });
                return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
            }
            result=  await _mntnWoStatusService.UpdateWoBase(parm);
            // return Ok(new { Ok = result == "OK", result });
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
    }
}
