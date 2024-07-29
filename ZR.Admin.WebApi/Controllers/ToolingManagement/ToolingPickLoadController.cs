using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.ActiveDirectory;
using System;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Service.ToolingManagement.IService;
using ZR.Infrastructure.Model;
using ZR.Model.Dto.Tooling;

namespace ZR.Admin.WebApi.Controllers.ToolingManagement
{
    /// <summary>
    /// 治具上下线
    /// </summary>
    [Route("toolingmanagement/toolingpickload/[action]")]
    public class ToolingPickLoadController : BaseController
    {
        IToolingPickUpService toolingPickUpService;
        IToolingSnService toolingSnService;
        public ToolingPickLoadController(IToolingPickUpService toolingPickUpService, IToolingSnService toolingSnService )
        {
            this.toolingPickUpService = toolingPickUpService;
            this.toolingSnService = toolingSnService;
        }

        /// <summary>
        /// 获取line
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SelectLine()
        {
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            List<MLine> list = toolingPickUpService.SelectLine(site);
            return SUCCESS(list);
        }


        /// <summary>
        /// 获取料号
        /// </summary>
        /// <param name="partno"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SelectPart([FromQuery] string partno)
        {
            if (string.IsNullOrEmpty(partno))
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            var data = toolingPickUpService.SelectPart(partno, site);
            if (data.Count<=0)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"料号不存在"));
            }
            return SUCCESS(data);
        }
        
        /// <summary>
        /// 检查工号
        /// </summary>
        /// <param name="empno"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SelectEmpNo([FromQuery] string empno)
        {
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            ImesMemp imesMemp = toolingPickUpService.SelectEmpByNo(empno, site);
            if (imesMemp == null || string.IsNullOrWhiteSpace(imesMemp.empNo))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"用户不存在"));
            }
            return SUCCESS(imesMemp);
        }

        /// <summary>
        /// 上下线
        /// </summary>
        /// <param name="upOrDown"></param>
        /// <param name="line"></param>
        /// <param name="partNo">成品料号</param>
        /// <param name="toolingsn"></param>
        /// <param name="empno"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ToolingSnPickLoad([FromQuery] string upOrDown, [FromQuery] string line, [FromQuery] string? partNo, [FromQuery] string toolingsn, [FromQuery] string empno)
        {
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            if (upOrDown != "up" && upOrDown != "down")
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请选择上线或下线"));
            }

            if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请选择线别"));
            }
            else
            {
                if (!toolingPickUpService.CheckLine(site, line))
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"线别不可用，请刷新后重试"));
                }
            }

            if (string.IsNullOrEmpty(empno) || string.IsNullOrWhiteSpace(empno))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请输入工号"));
            }
            else
            {
                ImesMemp imesMemp = toolingPickUpService.SelectEmpByNo(empno, site);
                if (imesMemp == null)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"用户不存在，请刷新后重试"));
                }
            }

            MToolingSn toolsn;
            if (string.IsNullOrEmpty(toolingsn) || string.IsNullOrWhiteSpace(toolingsn))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请输入治具SN"));
            }
            else
            {
                toolsn = new MToolingSn();
                toolsn = toolingSnService.GetInfoByToolingSn(site, toolingsn);
                if (toolsn == null)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具SN不存在，请刷新后重试"));
                }

                if (toolsn.ToolingStatus == "S")
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具SN已经报废，请使用其他治具SN"));
                }
            }


            if (upOrDown == "up")
            {

                ExecuteResult exeRes = toolingPickUpService.ToolingPickLoad(toolingsn, line, partNo, empno, site);
                if (exeRes.Status)
                {
                    string RES = exeRes.Message.ToString();
                    if (RES.Substring(0, 2) == "OK" || RES.Substring(0, 2) == "Wa")
                    {


                        if (RES.Substring(0, 2) == "Wa")
                        {
                            return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具SN超出告警次数"));
                        }

                        ToolingPickLoadVo toolingPickLoadVoIn = toolingPickUpService.SelectToolingPickLoadIn(line, toolingsn, site);
                        return SUCCESS(toolingPickLoadVoIn);

                    }
                    else if (RES.Substring(0, 2) == "Ov")
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具SN超出最大使用次数"));
                    }
                    else
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, RES));
                    }
                }
                else
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
                }

            }
            else //(upOrDown != "down")
            {
                ExecuteResult exeRes = toolingPickUpService.ToolingPickUnload(toolingsn, line, empno, site);
                if (exeRes.Status)
                {
                    ToolingPickLoadVo toolingPickLoadVoOut = toolingPickUpService.SelectToolingPickLoadOut(line, toolingsn, site);

                    return SUCCESS(toolingPickLoadVoOut);
                }
                else
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
                }
            }

        }
    }
}
