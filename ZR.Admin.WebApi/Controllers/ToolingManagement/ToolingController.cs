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
    /// 治具管理
    /// </summary>
    [Route("toolingmanagement/tooling/[action]")]
    public class ToolingController : BaseController
    {
        public IToolingTypeService toolingTypeService { get; set; }

        public IToolingService toolingService { get; set; }

        public ToolingController(IToolingTypeService toolingTypeService, IToolingService toolingService)
        {
            this.toolingTypeService = toolingTypeService;
            this.toolingService = toolingService;
        }

        /// <summary>
        /// 获取治具清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult listAllType()
        {
            var site = HttpContext.GetSite();
            site = site == "" ? "DEF" : HttpContext.GetSite();

            var response = toolingTypeService.GetAllTypeList(site);
            return SUCCESS(response);
        }


        /// <summary>
        /// 获取治具清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult list([FromQuery] MesCommonQuery query)
        {
            var site = HttpContext.GetSite();
            site = site == "" ? "DEF" : HttpContext.GetSite();

            var response = toolingService.GetInfoList(query, site);
            return SUCCESS(response);
        }

        [HttpGet]
        public async Task<IActionResult> SwitchStatus(long id, string status)
        {
            await toolingService.SwitchStatus(id, status, HttpContext.GetName());
            return SUCCESS(null);
        }

        /// <summary>
        /// 根据ID获取治具信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetInfo(long id = 0)
        {
            if (id <= 0)
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            var response = toolingService.GetInfoById(id);
            return SUCCESS(response);
        }

        /// <summary>
        /// 新增治具
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddTooling([FromBody] MToolingDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.ToolingNo))
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            MTooling tooling = dto.Adapt<MTooling>();
            if (UserConstants.NOT_UNIQUE.Equals(toolingService.CheckInfoNameUnique(tooling)))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"新增载治具型号'{tooling.ToolingNo}'失败，载治具型号已存在"));
            }
            tooling.Site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            tooling.Enabled = "Y";
            tooling.CreateEmpNo ??= HttpContext.GetName();
            long Id = toolingService.AddInfo(tooling);
            return SUCCESS(Id);
        }
        /// <summary>
        /// 修改治具
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateTooling([FromBody] MToolingDto dto)
        {
            if (dto == null || dto.Id <= 0 || string.IsNullOrEmpty(dto.ToolingNo))
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }

            MTooling tooling = dto.Adapt<MTooling>();
            var info = toolingService.GetInfoById(tooling.Id);
            if (info != null && info.ToolingNo != tooling.ToolingNo)
            {
                if (UserConstants.NOT_UNIQUE.Equals(toolingService.CheckInfoNameUnique(tooling)))
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"编辑载治具型号'{tooling.ToolingNo}'失败，载治具型号已存在"));
                }
            }
            //var data = toolingService.GetInfoById(tooling.Id);
            //if (data == null)
            //{
            //    return ToResponse(ApiResult.Error(101, "请求参数错误"));
            //}
            tooling.UpdateEmpNo ??= HttpContext.GetName();
            tooling.UpdateTime = DateTime.Now;
            int upResult = toolingService.UpdateInfo(tooling);
            if (upResult > 0)
            {
                return SUCCESS(upResult);
            }
            return ToResponse(ApiResult.Error($"编辑载治具型号'{tooling.ToolingNo} '失败，载治具型号已存在"));
        }

        [HttpGet]
        public IActionResult Export([FromQuery] MesCommonQuery query)
        {
            var site = HttpContext.GetSite();
            var list = toolingService.GetInfoList(query, site);

            var result = ExportExcelMini(list.Result, "Tooling", "治具");
            return ExportExcel(result.Item2, result.Item1);
        }
    }
}
