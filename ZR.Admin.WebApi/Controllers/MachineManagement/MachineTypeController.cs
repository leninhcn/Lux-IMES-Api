using Microsoft.AspNetCore.Mvc;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.System;
using ZR.Service.MachineManagement.IMachineManagementService;

namespace ZR.Admin.WebApi.Controllers.MachineManagement
{
    /// <summary>
    /// 设备类型管理
    /// </summary>
    [Route("machinemanagement/machinetype/[action]")]
    public class MachineTypeController : BaseController
    {

        public IMachineTypeService _MachineTypeService;

        public MachineTypeController(IMachineTypeService machineTypeService)
        {
            this._MachineTypeService = machineTypeService;
        }

        /// <summary>
        /// 获取设备类型清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult list([FromQuery] MntnMachineCommonQuery query)
        {
            var site = HttpContext.GetSite();

            if (site == null)
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            var response = _MachineTypeService.GetMachineTypeList(query, site);
            //return SUCCESS(response, TIME_FORMAT_FULL);
            return SUCCESS(response);
        }

        [HttpGet]
        public async Task<IActionResult> SwitchStatus(long id, string status)
        {
            await _MachineTypeService.SwitchStatus(id, status, HttpContext.GetName());
            return SUCCESS(null);
        }

        /// <summary>
        /// 根据ID获取设备类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetInfo(long id = 0)
        {
            var response = _MachineTypeService.GetMachineTypeById(id);
            return SUCCESS(response);
        }

        /// <summary>
        /// 新增设备类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddMachineType([FromBody] MMachineType dto)
        {
            if (dto == null)
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            MMachineType machineTypeDto = dto.Adapt<MMachineType>();
            if (UserConstants.NOT_UNIQUE.Equals(_MachineTypeService.CheckMachineTypeNameUnique(machineTypeDto)))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"新增设备类型'{machineTypeDto.MachineTypeName}'失败，设备类型已存在"));
            }
            machineTypeDto.Site = HttpContext.GetSite();
            machineTypeDto.Enabled = "Y";
            machineTypeDto.CreateEmpNo ??= HttpContext.GetName();
            long Id = _MachineTypeService.AddMachineType(machineTypeDto);
            return SUCCESS(Id);
        }

        /// <summary>
        /// 修改设备类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateMachineType([FromBody] MMachineType dto)
        {
            if (dto == null || dto.Id <= 0 || string.IsNullOrEmpty(dto.MachineTypeName))
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }

            MMachineType machineTypeDto = dto.Adapt<MMachineType>();
            var info = _MachineTypeService.GetMachineTypeById(machineTypeDto.Id);
            if (info != null && info.MachineTypeName != machineTypeDto.MachineTypeName)
            {
                if (UserConstants.NOT_UNIQUE.Equals(_MachineTypeService.CheckMachineTypeNameUnique(machineTypeDto)))
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"编辑设备类型'{machineTypeDto.MachineTypeName}'失败，设备类型已存在"));
                }
            }
            var data = _MachineTypeService.GetMachineTypeById(machineTypeDto.Id);
            if (data == null)
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            machineTypeDto.UpdateEmpNo ??= HttpContext.GetName();
            machineTypeDto.UpdateTime = DateTime.Now;
            int upResult = _MachineTypeService.UpdateMachineType(machineTypeDto);
            if (upResult > 0)
            {
                return SUCCESS(upResult);
            }
            return ToResponse(ApiResult.Error($"编辑设备类型'{machineTypeDto.MachineTypeName}'失败，请联系管理员"));
        }

        [HttpGet]
        public IActionResult Export([FromQuery] MntnMachineCommonQuery query)
        {
            var site = HttpContext.GetSite();
            var list = _MachineTypeService.GetMachineTypeList(query, site);

            var result = ExportExcelMini(list.Result, "Machine Type", "设备类型");
            return ExportExcel(result.Item2, result.Item1);
        }
    }
}
