using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;
using ZR.Admin.WebApi.Filters;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Service.System.IService;
using ZR.Model.Business;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ZR.Model.Dto;
using ZR.Service.IService;
using static System.Collections.Specialized.BitVector32;
using ZR.Model.Dto.ProdDto;
using ZR.Service;

namespace ZR.Admin.WebApi.Controllers.System
{
    /// <summary>
    /// MES共用
    /// </summary>
    //[Verify]
    [Route("mescommon/mesget")]
    [ApiExplorerSettings(GroupName = "sys")]
    public class MesGetController : BaseController
    {
        private readonly IMesGetService _MesGetService;
        private readonly ISysUserService _UserService;
        public MesGetController(IMesGetService MnesGetService
            , ISysUserService userService)
        {
            _MesGetService = MnesGetService;
            _UserService = userService;
        }
        /// <summary>
        /// 获取model机种信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_model")]
        public IActionResult ListModel(MesGetModel? parm)
        {
            parm.Site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListModel(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取route所有信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_route")]
        public IActionResult ListRoute(string? parm)
        {
            var site=HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListRoute(parm,site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取line所有信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_line")]
        public IActionResult ListLine(string? parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListLine(parm, site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取站点类型所有信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_station_type")]
        public IActionResult ListStaionType(string? parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListStationType(parm,site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 标签信息维护获取站点类型
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_labelstationtype")]
        public IActionResult ListLabelStaionType(MesGetLabelStationType? parm)
        {
            parm.Site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListLabelStationType(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取站点所有信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_station")]
        public IActionResult ListStaion(string? parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListStation(parm, site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取包装规则信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_pkspec")]
        public IActionResult ListPKSpec(string? parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListPKSPEC(parm, site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取规则条码信息
        /// </summary>
        /// <returns></returns>
        // [ActionPermissionFilter(Permission = "datacenter:mntnduty:list")]
        [HttpGet("list_rule")]
        public IActionResult ListRule(string? parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListRule(parm, site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_dept")]
        public IActionResult ListDept(string? parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListDept(parm, site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取工单类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_wotype")]
        public IActionResult ListWoType(string? parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListWoType(parm, site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取料号
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_part")]
        public IActionResult ListPart(string? parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListPart(parm, site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 判断料号是否存在
        /// </summary>
        /// <returns></returns>
        [HttpGet("check_part")]
        public IActionResult GetPart(string parm)
        {
            var site = HttpContext.GetSite();
            var res = _MesGetService.GetPart(parm, site);
            return ToResponse(new ApiResult(res.Count >0 ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, res.Count >0 ? "part NO check OK" : "Part no Error", null));
        }
        /// <summary>
        /// 判断库位-储位是否存在
        /// </summary>
        /// <returns></returns>
        [HttpGet("check_warehouse")]
        public async Task<IActionResult> CheckWareHouse(MesCheckWareHouse parm)
        {
            parm.Site = HttpContext.GetSite();
            var res = await _MesGetService.CheckWareHouse(parm);
            return ToResponse(new ApiResult(res=="OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, res == "OK" ? "sucess" : res));
        }
        /// <summary>
        /// 获取库位
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_warehouse")]
        public async Task<IActionResult> GetWareHouse(MesCheckWareHouse? parm)
        {
            parm.Site = HttpContext.GetSite();
            var res = await _MesGetService.GetWareHouse(parm);
            return SUCCESS(res, TIME_FORMAT_FULL);
            //return ToResponse(new ApiResult(res == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, res == "OK" ? "sucess" : res));
        }
        /// <summary>
        /// 获取工号
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_emp")]
        public async Task<IActionResult> GetEmp(MesGetEMP? parm)
        {
            parm.Site = HttpContext.GetSite();
            var res = await _MesGetService.GetEMP(parm);
            return SUCCESS(res, TIME_FORMAT_FULL);
            //return ToResponse(new ApiResult(res == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, res == "OK" ? "sucess" : res));
        }
        /// <summary>
        /// 标签维护程式使用，根据model获取料号
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_partlabeltype")]
        public IActionResult ListPartLabelType(MesGetPart? parm)
        {
            parm.Site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListPartlabeltype(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取投入制程
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_inprocess")]
        public IActionResult ListInProcess(string? parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetInProcess(parm, site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取产出制程
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_outprocess")]
        public IActionResult ListOutProcess(string? parm)
        {
            var site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetOutProcess(parm, site), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取imes下的procedure
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_procedure")]
        public IActionResult ListProcedures(string? parm)
        {
            return SUCCESS(_MesGetService.GetListProcedures(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取imes的Label_type_base内容
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_labeltypebase")]
        public IActionResult ListLabelTypeBase(MesGetLabelTypeBase? parm)
        {
            parm.Site=HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListLabelTypeBase(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 标签信息维护配置有station信息时获取imes的Label_type
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_labeltype_bystation")]
        public IActionResult ListLabelTypeByStation(MesGetLabelType? parm)
        {
            parm.Site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListLabelTypeByStation(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取stationtype生成树
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_stationtypetree")]
        public IActionResult ListStationTypeTree()
        {
            var site = HttpContext.GetSite();
            var stationtypetree = _MesGetService.GetStationTypeTree(site);

            string preStage = "";
            string preStationType = "";
            string preStationTypeDesc = "";

            var treeNodes = new List<ElTreeNodeStation>();

            foreach (var stationtype in stationtypetree)
            {
                var stage = stationtype.STAGE;
                var stationType = stationtype.STATION_TYPE;
                var stationTypeDesc = stationtype.STATION_TYPE_DESC;
                var clentType =stationtype.CLIENT_TYPE;

                if (preStage != stage)
                {
                    var lineNode = new ElTreeNodeStation { Label = stage, IconIndex = 0 };
                    treeNodes.Add(lineNode);

                    lineNode?.AddChild(new() { Label = stationTypeDesc,StationType = stationType, IconIndex = 1,ClientType=clentType });
                }
                else if (preStationTypeDesc != stationTypeDesc)
                {
                    var stageNode = treeNodes.LastOrDefault();

                    stageNode?.AddChild(new() { Label = stationTypeDesc, StationType = stationType, IconIndex = 1,ClientType = clentType });
                }
            
                preStage = stage;
                preStationTypeDesc = stationType;
            }
            return SUCCESS(treeNodes, TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取stationt生成树
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_stationtree")]
        public IActionResult ListStationTree()
        {
            var site = HttpContext.GetSite();
            var stationtree = _MesGetService.GetStationTree(site);

            string preLine = "";
            string preStage = "";
            string preStationType = "";
            string preStationTypeDesc = "";

            var treeNodes = new List<ElTreeNodeStation>();

            foreach (var station in stationtree)
            {
                var line = station.LINE;
                var stage = station.STAGE;
                var stationType = station.STATION_TYPE;
                var stationTypeDesc = station.STATION_TYPE_DESC;
                var stationName = station.STATION_NAME;

                if (preLine != line)
                {
                    var lineNode = new ElTreeNodeStation { Label = line, IconIndex = 0 };
                    treeNodes.Add(lineNode);

                    lineNode.AddChild(new() { Label = stage, IconIndex = 1 })
                        .AddChild(new() { Label = stationTypeDesc, IconIndex = 2 })
                        .AddChild(new() { Label = stationName, IconIndex = 3 ,StationType=stationType});
                }
                else if (preStage != stage)
                {
                    var lineNode = treeNodes.LastOrDefault();

                    lineNode?.AddChild(new() { Label = stage, IconIndex = 1 })
                        .AddChild(new() { Label = stationTypeDesc, IconIndex = 2 })
                        .AddChild(new() { Label = stationName, IconIndex = 3, StationType = stationType });
                }
                else if (preStationTypeDesc != stationTypeDesc)
                {
                    var stageNode = treeNodes.LastOrDefault()?.Last;

                    stageNode?.AddChild(new() { Label = stationTypeDesc, IconIndex = 2 })
                        .AddChild(new() { Label = stationName, IconIndex = 3, StationType = stationType });
                }
                else
                {
                    var stationTypeNode = treeNodes.LastOrDefault()?.Last?.Last;
                    stationTypeNode?.AddChild(new() { Label = stationName, IconIndex = 3 , StationType = stationType });
                }
                preLine = line;
                preStage = stage;
                preStationTypeDesc = stationTypeDesc;
            }
            return SUCCESS(treeNodes, TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取label的Printfield参数信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_printfield")]
        public IActionResult ListLabelType(MesGetPrintField? parm)
        {
            parm.Site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetListPrintField(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取工单内的sn信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_snbywo")]
        public IActionResult ListSNByWO(MesGetSNInfo? parm)
        {
            parm.Site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetSNInfo(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取工单内生产的Panel信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("list_Panelbywo")]
        public IActionResult ListPanelByWO(MesGetSNInfo? parm)
        {
            parm.Site = HttpContext.GetSite();
            return SUCCESS(_MesGetService.GetWoPanelInfo(parm), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 用户导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "用户模板", BusinessType = BusinessType.EXPORT, IsSaveRequestData = true, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel(string param )
        {
            (string, string) result = DownloadImportTemplate(param);
            return ExportExcel(result.Item2, result.Item1);
        }
    }
}
