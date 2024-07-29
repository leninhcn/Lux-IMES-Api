using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business;
using ZR.Model;
using ZR.Service.ToolingManagement.IService;
using ZR.Model.System;
using ZR.Model.Dto.Tooling;
using ZR.Model.Dto;

namespace ZR.Admin.WebApi.Controllers.ToolingManagement
{
    /// <summary>
    /// 治具SN管理
    /// </summary>
    [Route("toolingmanagement/toolingsn/[action]")]
    public class ToolingSnController : BaseController
    {
        public IToolingSnService toolingSnService;

        public ToolingSnController(IToolingSnService toolingSnService) 
        {
            this.toolingSnService = toolingSnService;
        }

        /// <summary>
        /// 获取治具SN清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult list([FromQuery] ToolingSnQuery query)
        {
            var site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            var response = toolingSnService.GetInfoList(query, site);
            return SUCCESS(response);
        }

        [HttpGet]
        public async Task<IActionResult> SwitchStatus(long id, string status)
        {
            await toolingSnService.SwitchStatus(id, status, HttpContext.GetName());
            return SUCCESS(null);
        }

        /// <summary>
        /// 根据ID获取治具SN信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("toolingSnId={id}")]
        public IActionResult GetInfoByToolingSnId(long id)
        {
            var response = toolingSnService.GetInfoByToolingSnId(id);
            return SUCCESS(response);
        }

        /// <summary>
        /// 新增载治具SN
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToolingSn([FromBody] MToolingSnDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.ToolingSn) || dto.ToolingId <= 0 )
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            MToolingSn toolingSn = dto.Adapt<MToolingSn>();
            if (UserConstants.NOT_UNIQUE.Equals(toolingSnService.CheckInfoNameUnique(toolingSn)))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"新增载治具SN '{toolingSn.ToolingSn}'失败，载治具SN已存在"));
            }
            toolingSn.Site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            toolingSn.Enabled = "Y";
            toolingSn.CreateEmpNo ??= HttpContext.GetName();
            long Id = toolingSnService.AddInfo(toolingSn);
            return SUCCESS(Id);
        }

        /// <summary>
        /// 修改载治具SN
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateToolingSn([FromBody] MToolingSnDto dto)
        {
            if (dto == null || dto.ToolingId <= 0 || dto.ToolingSnId <= 0)
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }

            MToolingSn toolingSn = dto.Adapt<MToolingSn>();
            var info = toolingSnService.GetInfoByToolingSnId(toolingSn.ToolingSnId);
            if (info != null && info.ToolingSn != toolingSn.ToolingSn)
            {
                if (UserConstants.NOT_UNIQUE.Equals(toolingSnService.CheckInfoNameUnique(toolingSn)))
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"编辑载治具SN '{toolingSn.ToolingSn}'失败，载治具SN已存在"));
                }
            }

            toolingSn.UpdateEmpNo ??= HttpContext.GetName();
            toolingSn.UpdateTime = DateTime.Now;
            int upResult = toolingSnService.UpdateInfo(toolingSn);
            if (upResult > 0)
            {
                return SUCCESS(upResult);
            }
            return ToResponse(ApiResult.Error($"编辑载治具SN '{toolingSn.ToolingSn} '失败，载治具SN已存在"));
        }

        [HttpGet]
        public IActionResult Export([FromQuery] ToolingSnQuery query)
        {
            var site = HttpContext.GetSite();
            var list = toolingSnService.GetInfoList(query, site);

            var result = ExportExcelMini(list.Result, "Tooling SN", "治具SN");
            return ExportExcel(result.Item2, result.Item1);
        }
    }
}
