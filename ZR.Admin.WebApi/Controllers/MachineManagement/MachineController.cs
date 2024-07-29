using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business;
using ZR.Model.System;
using ZR.Model;
using ZR.Service.IService;
using ZR.Service.MachineManagement.IMachineManagementService;
using ZR.Model.Dto;

namespace ZR.Admin.WebApi.Controllers.MachineManagement
{
    /// <summary>
    /// 设备管理
    /// </summary>
    
    [Route("machinemanagement/machine/[action]")]
    public class MachineController : BaseController
    {
        public IMachineService _MachineService;
        public IMesGetService _MesGetService;
        public IMachineGroupService _MachineGroupService;

        public MachineController(IMachineService machineService, IMesGetService mesGetService, IMachineGroupService machineGroupService )
        {
            this._MachineService = machineService;
            this._MesGetService = mesGetService;
            this._MachineGroupService = machineGroupService;
        }


        /// <summary>
        /// 获取设备清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult list([FromQuery] MntnMachineQuery query)
        {
            var site = HttpContext.GetSite();
            if (site == null)
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }

            var response = _MachineService.GetMachineList(query, site);
            return SUCCESS(response);
        }

        [HttpGet]
        public async Task<IActionResult> SwitchStatus(long id, string status)
        {
            await _MachineService.SwitchStatus(id, status, HttpContext.GetName());
            return SUCCESS(null);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMachineLocs()
        {
            var data = await _MachineService.GetAllMachineLocs();
            return SUCCESS(data);
        }

        /// <summary>
        /// 获取设备线别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetLineList()
        {
            string site = HttpContext.GetSite();
            var response = _MesGetService.GetListLine(null, site);
            return SUCCESS(response);
        }

        /// <summary>
        /// 根据ID获取设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetInfo(long id = 0)
        {
            var response = _MachineService.GetMachineCodeById(id);
            return SUCCESS(response);
        }

        /// <summary>
        /// 新增设备
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddMachine([FromBody] MMachine dto)
        {
            if (dto == null ||  dto.GroupId <= 0 ||string.IsNullOrEmpty(dto.MachineType))
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            MMachine machine = dto.Adapt<MMachine>();
            if (UserConstants.NOT_UNIQUE.Equals(_MachineService.CheckMachineCodeUnique(machine)))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"新增设备'{machine.MachineCode}'失败，设备已存在"));
            }
            machine.Enabled = "Y";
            machine.CreateEmpNo ??= HttpContext.GetName();
            machine.CreateTime = DateTime.Now;
            long Id = _MachineService.AddMachine(machine);
            return SUCCESS(Id);
        }

        /// <summary>
        /// 修改设备
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateMachine([FromBody] MMachine dto)
        {
            if (dto == null || dto.Id <= 0 || dto.GroupId <= 0)
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            MMachine machine = dto.Adapt<MMachine>();
            var info = _MachineService.GetMachineCodeById(machine.Id);
            if (info != null && info.MachineCode != machine.MachineCode)
            {
                if (UserConstants.NOT_UNIQUE.Equals(_MachineService.CheckMachineCodeUnique(machine)))
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"编辑设备'{machine.MachineCode}'失败，设备已存在"));
                }
            }
            var group = _MachineGroupService.GetMachineGroupById(machine.GroupId);
            if (group == null)
            {
                return ToResponse(ApiResult.Error($"编辑设备'{machine.MachineCode}'失败，请联系管理员"));
            }
            machine.UpdateEmpNo ??= HttpContext.GetName();
            machine.UpdateTime = DateTime.Now;
            int upResult = _MachineService.UpdateMachine(machine);
            if (upResult > 0)
            {
                return SUCCESS(upResult);
            }
            return ToResponse(ApiResult.Error($"编辑设备'{machine.MachineCode}'失败，请联系管理员"));
        }

        [HttpGet]
        public IActionResult Export([FromQuery] MntnMachineQuery query)
        {
            var site = HttpContext.GetSite();
            var list = _MachineService.GetMachineList(query, site);

            var result = ExportExcelMini(list.Result, "Machine", "设备");
            return ExportExcel(result.Item2, result.Item1);
        }
    }
}
