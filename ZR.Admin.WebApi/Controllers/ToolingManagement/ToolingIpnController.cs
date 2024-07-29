using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business;
using ZR.Model;
using ZR.Service.ToolingManagement.IService;
using ZR.Model.System;
using ZR.Model.Dto.Tooling;

namespace ZR.Admin.WebApi.Controllers.ToolingManagement
{
    /// <summary>
    /// 治具料号管理
    /// </summary>
    [Route("toolingmanagement/toolingipn")]
    public class ToolingIpnController : BaseController
    {
        public IToolingIpnService toolingIpnService { get; set; }
        public IToolingService toolingService { get; set; }

        public ToolingIpnController(IToolingIpnService toolingIpnService, IToolingService toolingService)
        {
            this.toolingIpnService = toolingIpnService;
            this.toolingService = toolingService;
        }

        /// <summary>
        /// 获取载治具清单
        /// </summary>
        /// <param name="toolingid"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public IActionResult list([FromQuery] long toolingid)
        {
            var site = HttpContext.GetSite();
            if (toolingid <= 0)
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            var response = toolingIpnService.GetInfoList(site, toolingid);
            return SUCCESS(response);
        }


        /// <summary>
        /// 新增载治具型号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToolingIpn([FromBody] MToolingIpnDto dto)
        {
            if (dto == null || dto.toolingId <= 0)
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }

            if(toolingService.GetInfoById(dto.toolingId) == null)
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }


            toolingIpnService.DeleteInfo(dto.toolingId);

            foreach (var item in dto.ipnAssy)
            {
                MToolingIpn toolingIpn = new MToolingIpn();
                toolingIpn.ToolingId = dto.toolingId;
                toolingIpn.Ipn = item;
                toolingIpn.Site = HttpContext.GetSite();
                if (string.IsNullOrEmpty(toolingIpn.Site))
                {
                    toolingIpn.Site = "DEF";
                }
                toolingIpn.Enabled = 'Y';
                toolingIpn.CreateEmpNo ??= HttpContext.GetName();
                toolingIpnService.AddInfo(toolingIpn);
            }
            long Id = toolingIpnService.AddHistory(dto.toolingId);
            return SUCCESS(Id);
        }
    }
}
