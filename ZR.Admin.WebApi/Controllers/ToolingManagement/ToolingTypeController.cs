using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business;
using ZR.Model.System;
using ZR.Model;
using ZR.Service.ToolingManagement.IService;
using ZR.Model.Dto.Tooling;
using ZR.Model.Dto;
using ZR.Service.MachineManagement.IMachineManagementService;

namespace ZR.Admin.WebApi.Controllers.ToolingManagement
{
    /// <summary>
    ///治具类型管理
    /// </summary>
    [Route("toolingmanagement/toolingtype/[action]")]
    public class ToolingTypeController : BaseController
    {
        public IToolingTypeService _ToolingTypeService { get; set; }

        public ToolingTypeController(IToolingTypeService toolingTypeService)
        {
            this._ToolingTypeService = toolingTypeService;
        }

        /// <summary>
        /// 获取治具类型清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult list([FromQuery] MesCommonQuery query)
        {
            var site = HttpContext.GetSite();
            var response = _ToolingTypeService.GetInfoList(query, site);
            //return SUCCESS(response, TIME_FORMAT_FULL);
            return SUCCESS(response);
        }

        [HttpGet]
        public async Task<IActionResult> SwitchStatus(long id, string status)
        {
            await _ToolingTypeService.SwitchStatus(id, status, HttpContext.GetName());
            return SUCCESS(null);
        }

        /// <summary>
        /// 根据ID获取治具类型
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
            var response = _ToolingTypeService.GetInfoById(id);
            return SUCCESS(response);
        }

        /// <summary>
        /// 新增治具类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToolingType([FromBody] MToolingTypeDto dto)
        {
            if (dto == null)
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            MToolingType toolingType = dto.Adapt<MToolingType>();
            if (UserConstants.NOT_UNIQUE.Equals(_ToolingTypeService.CheckInfoNameUnique(toolingType)))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"新增治具类型'{toolingType.ToolingType}'失败，治具类型已存在"));
            }
            toolingType.Site = HttpContext.GetSite() == "" ? "DEF": HttpContext.GetSite();
            toolingType.Enabled = "Y";
            toolingType.CreateEmpNo ??= HttpContext.GetName();
            long Id = _ToolingTypeService.AddInfo(toolingType);
            return SUCCESS(Id);
        }

        /// <summary>
        /// 修改治具类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateToolingType([FromBody] MToolingTypeDto dto)
        {
            if (dto == null || dto.Id <= 0 || string.IsNullOrEmpty(dto.ToolingType))
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }

            MToolingType toolingType = dto.Adapt<MToolingType>();
            var info = _ToolingTypeService.GetInfoById(toolingType.Id);
            if (info != null && info.ToolingType != toolingType.ToolingType)
            {
                if (UserConstants.NOT_UNIQUE.Equals(_ToolingTypeService.CheckInfoNameUnique(toolingType)))
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"编辑治具类型'{toolingType.ToolingType}'失败，治具类型已存在"));
                }
            }
            //var data = _ToolingTypeService.GetInfoById(toolingType.Id);
            //if (data == null)
            //{
            //    return ToResponse(ApiResult.Error(101, "请求参数错误"));
            //}
            toolingType.UpdateEmpNo ??= HttpContext.GetName();
            toolingType.UpdateTime = DateTime.Now;
            int upResult = _ToolingTypeService.UpdateInfo(toolingType);
            if (upResult > 0)
            {
                return SUCCESS(upResult);
            }
            return ToResponse(ApiResult.Error($"编辑治具类型'{toolingType.ToolingType} '失败，治具类型已存在"));
        }

        [HttpGet]
        public IActionResult Export([FromQuery] MesCommonQuery query)
        {
            var site = HttpContext.GetSite();
            var list = _ToolingTypeService.GetInfoList(query, site);

            var result = ExportExcelMini(list.Result, "Tooling Type", "治具类型");
            return ExportExcel(result.Item2, result.Item1);
        }
    }
}
