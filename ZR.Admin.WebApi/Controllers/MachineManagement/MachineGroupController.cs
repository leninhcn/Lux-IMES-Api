using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business;
using ZR.Model;
using ZR.Service.MachineManagement.IMachineManagementService;
using ZR.Model.System;
using ZR.Service.IService;
using ZR.Model.Dto;
using Aliyun.OSS;

namespace ZR.Admin.WebApi.Controllers.MachineManagement
{
    /// <summary>
    /// 设备组管理
    /// </summary>
    [Route("machinemanagement/machinegroup/[action]")]
    public class MachineGroupController : BaseController
    {
        public IMachineGroupService _MachineGroupService;
        public IMesGetService _MesGetService;

        public MachineGroupController(IMachineGroupService machineGroupService, IMesGetService mesGetService)
        {
            this._MachineGroupService = machineGroupService;
            this._MesGetService = mesGetService;
        }

        /// <summary>
        /// 获取设备组类型
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
            var response = _MachineGroupService.GetMachineGroupList(query, site);
            return SUCCESS(response);
        }

        [HttpGet]
        public async Task<IActionResult> SwitchStatus(long id, string status)
        {
            await _MachineGroupService.SwitchStatus(id, status, HttpContext.GetName());
            return SUCCESS(null);
        }

        /// <summary>
        /// 获取制程信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStationTypeList()
        {
            string site = HttpContext.GetSite();
            var response = _MesGetService.GetListStationType(null, site);
            return SUCCESS(response);
        }

        /// <summary>
        /// 根据ID获取设备类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetInfo(long id = 0)
        {
            var response = _MachineGroupService.GetMachineGroupById(id);
            return SUCCESS(response);
        }

        /// <summary>
        /// 新增设备类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddMachineGroup([FromBody] MMachineGroup dto)
        {
            if (dto == null )
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            MMachineGroup group = dto.Adapt<MMachineGroup>();
            if (UserConstants.NOT_UNIQUE.Equals(_MachineGroupService.CheckMachineGroupUnique(group)))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"新增设备组'{group.Name}'失败，设备组已存在"));
            }
            group.Site = HttpContext.GetSite();
            group.Enabled = "Y";
            group.CreateEmpNo ??= HttpContext.GetName();
            long Id = _MachineGroupService.AddMachineGroup(group);
            return SUCCESS(Id);
        }

        /// <summary>
        /// 修改设备类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateMachineGroup([FromBody] MMachineGroup dto)
        {
            if (dto == null || dto.Id <= 0 || string.IsNullOrEmpty(dto.Name))
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }

            MMachineGroup group = dto.Adapt<MMachineGroup>();
            var info = _MachineGroupService.GetMachineGroupById(group.Id);
            if (info != null && info.Name != group.Name)
            {
                if (UserConstants.NOT_UNIQUE.Equals(_MachineGroupService.CheckMachineGroupUnique(group)))
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"编辑设备组'{group.Name}'失败，设备组已存在"));
                }
            }
            var data = _MachineGroupService.GetMachineGroupById(group.Id);
            if (data == null)
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }

            group.UpdateEmpNo ??= HttpContext.GetName();
            group.UpdateTime = DateTime.Now;
            int upResult = _MachineGroupService.UpdateMachineGroup(group);
            if (upResult > 0)
            {
                return SUCCESS(upResult);
            }
            return ToResponse(ApiResult.Error($"编辑设备组'{group.Name}'失败，请联系管理员"));
        }

        [HttpGet]
        public IActionResult Export([FromQuery] MntnMachineCommonQuery query)
        {
            var site = HttpContext.GetSite();
            var list = _MachineGroupService.GetMachineGroupList(query, site);

            var result = ExportExcelMini(list.Result, "Machine Group", "设备组");
            return ExportExcel(result.Item2, result.Item1);
        }
    }
}
