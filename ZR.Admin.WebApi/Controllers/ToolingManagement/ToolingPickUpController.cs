using Microsoft.AspNetCore.Mvc;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Service.ToolingManagement.IService;

namespace ZR.Admin.WebApi.Controllers.ToolingManagement
{
    /// <summary>
    /// 治具领用归还
    /// </summary>
    [Route("toolingmanagement/toolingpickup/[action]")]
    public class ToolingPickUpController : BaseController
    {
        public IToolingPickUpService toolingPickUpService;
        public ToolingPickUpController(IToolingPickUpService toolingPickUpService)
        {
            this.toolingPickUpService = toolingPickUpService;
        }

        /// <summary>
        /// 检查工号 
        /// </summary>
        /// <param name="empno"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SelectEmpByNo(string empno)
        {
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            var resp = toolingPickUpService.SelectEmpByNo(empno, site);
            if (resp == null)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"用户不存在"));
            }
            return SUCCESS(resp);
        }

        /// <summary>
        /// 治具领用
        /// </summary>
        /// <param name="empno"></param>
        /// <param name="toolingSn"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PickUpToolingSn([FromQuery] string empno, [FromQuery] string toolingSn)
        {
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();

            var toolPickUpVo = toolingPickUpService.selectToolingByToolingSn( toolingSn,site);

            if (toolPickUpVo == null)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具不存在"));
            }
            if (toolPickUpVo.totalUsedCount >= toolPickUpVo.MaxUseTimes)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具总使用次数超过最大使用次数"));
            }
            if (toolPickUpVo.usedCount >= toolPickUpVo.WarnUsedTimes && toolPickUpVo.usedCount < toolPickUpVo.MaxUseTimes)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具使用次数超过警告使用次数"));
            }



            ExecuteResult exeRes =  toolingPickUpService.ToolingPickUp(toolingSn, empno, site);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.CUSTOM_ERROR, exeRes.Message);
            }

            ApiResult result = ApiResult.Success("Tooling Pick Up OK", toolPickUpVo);
            return ToResponse(result);

            //string sRes = exeRes.Message.ToString();
            //var sss = sRes.Substring(0 , 2);
            //if (sRes.Substring(0, 2) == "OK" || sRes.Substring(1, 2) == "Wa")
            //{
            //    if (sRes.Substring(0, 2) == "Wa")
            //    {
            //        return ToResponse(ResultCode.CUSTOM_ERROR, exeRes.Message);
            //    }
            //    else
            //    {
            //        ApiResult result = ApiResult.Success("Tooling Pick Up OK", toolPickUpVo);
            //        return ToResponse(result);
            //    }
            //}
            //else
            //{
            //    return ToResponse(ResultCode.CUSTOM_ERROR, exeRes.Message);
            //}

        }

        /// <summary>
        /// 治具归还
        /// </summary>
        /// <param name="empno"></param>
        /// <param name="toolingSn"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ReturnToolingSn([FromQuery] string empno, [FromQuery] string toolingSn)
        {
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();

            var toolPickUpVo = toolingPickUpService.selectToolingByToolingSn(toolingSn, site);

            if (toolPickUpVo == null)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具不存在"));
            }
            if (toolPickUpVo.totalUsedCount >= toolPickUpVo.MaxUseTimes)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具总使用次数超过最大使用次数"));
            }
            if (toolPickUpVo.usedCount >= toolPickUpVo.WarnUsedTimes && toolPickUpVo.usedCount < toolPickUpVo.MaxUseTimes)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具使用次数超过警告使用次数"));
            }



            ExecuteResult exeRes =  toolingPickUpService.ToolingReturn(toolingSn, empno, site);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.CUSTOM_ERROR, exeRes.Message);
            }

            ApiResult result = ApiResult.Success("Tooling Return OK", toolPickUpVo);
            return ToResponse(result);

            //string sRes = exeRes.Message;
            //if (sRes.Substring(0, 2) == "OK")
            //{
            //    ApiResult result = ApiResult.Success("Tooling Return OK", toolPickUpVo);
            //    return ToResponse(result);
            //}
            //else
            //{
            //    return ToResponse(ResultCode.CUSTOM_ERROR, exeRes.Message);
            //}

        }

        [HttpGet]
        public async Task<IActionResult> GetToolingInfo(string tooling)
        {
            return SUCCESS(await toolingPickUpService.GetToolingInfo(tooling));
        }

    }
}
