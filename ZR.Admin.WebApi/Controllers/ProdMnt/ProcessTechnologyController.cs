using Microsoft.AspNetCore.Mvc;
using ZR.Model.Dto;
using ZR.Model.Business;
using ZR.Service.IService;
using MiniExcelLibs;
using SkiaSharp;
using System.Xml.Linq;
using Microsoft.AspNetCore.Routing;
using System.Security.Policy;


namespace ZR.Admin.WebApi.Controllers.ProdMnt
{/// <summary>
 /// 基础信息 -> 制程工艺
 /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("prodMnt/ProcessTechnology/")]
    [ApiController]
    public class ProcessTechnologyController : BaseController
    {
        readonly IProcessTechnologyService service;
        public ProcessTechnologyController(IProcessTechnologyService service) {
            this.service = service;
        }
        /// <summary>
        /// 删除站点工艺数据
        /// </summary>
        [HttpDelete("DeleteStationByID")]
        public async Task<IActionResult> DeleteStationByID(StationTechInfo stationTechInfo)
        {
            var site = HttpContext.GetSite();
            stationTechInfo.site = site;
            var name = HttpContext.GetName();
            stationTechInfo.updateEmpno = name;
            stationTechInfo.updateTime = DateTime.Now;
            return Ok(service.DeleteStationByID(stationTechInfo));
        }
        /// <summary>
        /// 站点工艺状态修改
        /// </summary>
        [HttpPut("UpdateStation")]
        public async Task<IActionResult> UpdateStation(StationTechInfo stationTechInfo)
        {
            var name = HttpContext.GetName();
            stationTechInfo.updateEmpno = name;
            stationTechInfo.updateTime = DateTime.Now;
            return Ok(service.UpdateStation(stationTechInfo));
        }
        /// <summary>
        /// 新增站点工艺信息
        /// </summary>
        [HttpPut("InsertStationInfo")]
        public async Task<IActionResult> InsertStationInfo(StationTechInfo stationTechInfo)
        {
            var name = HttpContext.GetName();
            stationTechInfo.updateEmpno = name;
            stationTechInfo.updateTime = DateTime.Now;
            stationTechInfo.createEmpno = name;
            stationTechInfo.createTime = DateTime.Now;
            stationTechInfo.enabled = "Y";
            return Ok(service.InsertStatinoInfo(stationTechInfo));
        }
        [HttpGet("ShowStationType")]
        public Task<IActionResult> ShowStationType(string? textData)
        {

            return Task.FromResult<IActionResult>(Ok(service.ShowStationType(textData)));
        }
        /// <summary>
        ///  Copy站点工艺信息
        /// </summary>
        [HttpPut("CopyStationInfo")]
        public async Task<IActionResult> CopyStationInfo(StationTechInfo stationTechInfo)
        {
            var name = HttpContext.GetName();
            stationTechInfo.createEmpno = name;
            stationTechInfo.updateEmpno = name;
            return Ok(service.CopyStationInfo(stationTechInfo));
        }
        /// <summary>
        /// 站点工艺信息获取
        /// </summary>
        [HttpGet("ShowStationInfo")]
        public Task<IActionResult> ShowStationInfo(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            
            return Task.FromResult<IActionResult>(Ok(service.ShowStationInfo(enaBled, optionData, textData, pageNum, pageSize)));
        }
        /// <summary>
        /// 站点工艺数据导出
        /// </summary>
        /// <param name="enaBled"></param>
        /// <param name="optionData"></param>
        /// <param name="textData"></param>
        /// <param name="pageSize"></param>
        /// <param name="user"></param>
        [HttpGet("ShowStationExport")]
        [Log(Title = "站点工艺数据导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowStationExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.ShowStationInfo(enaBled, optionData, textData, 1, pageSize);
            var result = ExportExcelMini(list.Result, "StationTechInfo", "站点工艺数据导出");
            return  ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 站点工艺导入
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        /// <returns></returns>
        [HttpPost("StationImportData")]
        [Log(Title = "站点导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        //[ActionPermissionFilter(Permission = "system:user:import")]
        public IActionResult StationImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<StationTechInfo> Station = new();
            using (var stream = formFile.OpenReadStream())
            {
                Station = stream.Query<StationTechInfo>(startCell: "A1").ToList();
            }
            return SUCCESS(service.StationImportData(Station, HttpContext.GetSite(), HttpContext.GetName()));
        }


        /// <summary>
        /// 站点工艺历史记录获取
        /// </summary>
        [HttpGet("StationHistory")]
        public async Task<IActionResult> StationHistory(string id)
        {
            return Ok(service.StationHistory(id));

        }


 //----------------------------------------站点分割线-----------------------------------------------------------------------------------

        /// <summary>
        /// 线体信息获取
        /// </summary>
        [HttpGet("ShowLineInfo")]
        public Task<IActionResult> ShowLineInfo(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {

            return Task.FromResult<IActionResult>(Ok(service.ShowLineInfo(enaBled, optionData, textData, pageNum, pageSize)));
        }

        /// <summary>
        /// 线体数据导出
        /// </summary>
        /// <param name="enaBled"></param>
        /// <param name="optionData"></param>
        /// <param name="textData"></param>
        /// <param name="pageSize"></param>
        /// <param name="user"></param>
        [HttpGet("ShowLineExport")]
        [Log(Title = "线体数据导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowLineExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.ShowLineInfo(enaBled, optionData, textData, 1, pageSize);
            var result = ExportExcelMini(list.Result, "LineTypeInfo", "站点数据导出");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 新增线体信息
        /// </summary>
        [HttpPut("InsertLineInfo")]
        public async Task<IActionResult> InsertLineInfo(LineTypeInfo LineTypeInfo)
        {
            return Ok(service.InsertLineInfo(LineTypeInfo));
        }
        /// <summary>
        /// 站点修改
        /// </summary>
        [HttpPut("UpdateLine")]
        public async Task<IActionResult> UpdateLine(LineTypeInfo lineTypeInfo)
        {
            var name = HttpContext.GetName();
            lineTypeInfo.updateEmpno = name;
            lineTypeInfo.updateTime = DateTime.Now;
            return Ok(service.UpdateLine(lineTypeInfo));
        }

        /// <summary>
        /// 删除线体数据
        /// </summary>
        [HttpDelete("DeleteLineByID")]
        public async Task<IActionResult> DeleteLineByID(LineTypeInfo lineTypeInfo)
        {
            var name = HttpContext.GetName();
            lineTypeInfo.updateEmpno = name;
            lineTypeInfo.updateTime = DateTime.Now;
            return Ok(service.DeleteLineByID(lineTypeInfo));
        }
        /// <summary>
        /// 站点类型导入
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        /// <returns></returns>
        [HttpPost("LineImportData")]
        [Log(Title = "站点类型导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        //[ActionPermissionFilter(Permission = "system:user:import")]
        public IActionResult LineImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<LineTypeInfo> lineTypeInfo = new();
            using (var stream = formFile.OpenReadStream())
            {
                lineTypeInfo = stream.Query<LineTypeInfo>(startCell: "A1").ToList();
            }
            return SUCCESS(service.LineImportData(lineTypeInfo, HttpContext.GetSite(), HttpContext.GetName()));
        }

//----------------------------------------线体分割线-----------------------------------------------------------------------------------
        /// <summary>
        /// 区段信息获取
        /// </summary>
        [HttpGet("ShowStageInfo")]
        public Task<IActionResult> ShowStageInfo(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {

            return Task.FromResult<IActionResult>(Ok(service.ShowStageInfo(enaBled, optionData, textData, pageNum, pageSize)));
        }
        /// <summary>
        /// 区段信息导出
        /// </summary>
        /// <param name="enaBled"></param>
        /// <param name="optionData"></param>
        /// <param name="textData"></param>
        /// <param name="pageSize"></param>
        /// <param name="user"></param>
        [HttpGet("ShowStageExport")]
        [Log(Title = "区段信息导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowStageExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.ShowStageInfo(enaBled, optionData, textData, 1, pageSize);
            var result = ExportExcelMini(list.Result, "StageInfo", "区段信息导出");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 新增区段信息
        /// </summary>
        [HttpPut("InsertStageInfo")]
        public async Task<IActionResult> InsertStageInfo(StageInfo stageInfo)
        {
            var site =HttpContext.GetSite();
            var name = HttpContext.GetName();

            stageInfo.createEmpno = name;
            stageInfo.createTime = DateTime.Now;
            stageInfo.updateEmpno = name;
            stageInfo.updateTime = DateTime.Now;
            stageInfo.site = site;

            return Ok(service.InsertStageInfo(stageInfo));
        }
        /// <summary>
        /// 区段信息修改
        /// </summary>
        [HttpPut("UpdateStage")]
        public async Task<IActionResult> UpdateStage(StageInfo stageInfo)
        {
            var name = HttpContext.GetName();
            stageInfo.updateEmpno = name;
            stageInfo.updateTime = DateTime.Now;
            return Ok(service.UpdateStage(stageInfo));
        }
        /// <summary>
        /// 删除区段数据
        /// </summary>
        [HttpDelete("DeleteStageByID")]
        public async Task<IActionResult> DeleteStageByID(StageInfo stageInfo)
        {
            var name = HttpContext.GetName();
            stageInfo.updateEmpno = name;
            stageInfo.updateTime = DateTime.Now;
            return Ok(service.DeleteStageByID(stageInfo));
        }
        /// <summary>
        /// 区段历史记录获取
        /// </summary>
        [HttpGet("StageHistory")]
        public async Task<IActionResult> StageHistory(string id)
        {
            return Ok(service.StageHistory(id));

        }
//----------------------------------------区段分割线-----------------------------------------------------------------------------------
        /// <summary>
        /// 站点类型信息获取
        /// </summary>
        [HttpGet("ShowStationTypeInfo")]
        public Task<IActionResult> ShowStationTypeInfo(string? stage, string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {

            return Task.FromResult<IActionResult>(Ok(service.ShowStationTypeInfo(stage,enaBled, optionData, textData, pageNum, pageSize)));
        }
        /// <summary>
        /// 站点类型信息导出
        /// </summary>
        /// <param name="enaBled"></param>
        /// <param name="optionData"></param>
        /// <param name="textData"></param>
        /// <param name="pageSize"></param>
        [HttpGet("ShowStationTypeExport")]
        [Log(Title = "站点类型信息导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowStationTypeExport(string? stage, string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.ShowStationTypeInfo(stage,enaBled, optionData, textData, 1, pageSize);
            var result = ExportExcelMini(list.Result, "StationTypeInfo", "站点类型信息导出");
            return ExportExcel(result.Item2, result.Item1);
        }
        /// <summary>
        /// 新增站点类型信息
        /// </summary>
        [HttpPut("InsertStationTypeInfo")]
        public async Task<IActionResult> InsertStationTypeInfo(StationTypeInfo stationTypeInfo)
        {
            var site = HttpContext.GetSite();
            var name = HttpContext.GetName();

            stationTypeInfo.createEmpno = name;
            stationTypeInfo.createTime = DateTime.Now;
            stationTypeInfo.updateEmpno = name;
            stationTypeInfo.updateTime = DateTime.Now;
            stationTypeInfo.site = site;

            return Ok(service.InsertStationTypeInfo(stationTypeInfo));
        }
        /// <summary>
        /// 站点类型信息修改
        /// </summary>
        [HttpPut("UpdateStationType")]
        public async Task<IActionResult> UpdateStationType(StationTypeInfo stationTypeInfo)
        {
            var name = HttpContext.GetName();
            stationTypeInfo.updateEmpno = name;
            stationTypeInfo.updateTime = DateTime.Now;
            return Ok(service.UpdateStationType(stationTypeInfo));
        }

        /// <summary>
        /// 删除站点类型数据
        /// </summary>
        [HttpDelete("DeleteStationTypeByID")]
        public async Task<IActionResult> DeleteStationTypeByID(StationTypeInfo stationTypeInfo)
        {
            var name = HttpContext.GetName();
            stationTypeInfo.updateEmpno = name;
            stationTypeInfo.updateTime = DateTime.Now;

            return Ok(service.DeleteStationTypeByID(stationTypeInfo));
        }

        /// <summary>
        /// 站点类型历史记录获取
        /// </summary>
        [HttpGet("StationTypeHistory")]
        public async Task<IActionResult> StationTypeHistory(string id)
        {
            return Ok(service.StationTypeHistory(id));

        }

        /// <summary>
        /// 站点类型导入
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        /// <returns></returns>
        [HttpPost("StationTypeImportData")]
        [Log(Title = "站点类型导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        //[ActionPermissionFilter(Permission = "system:user:import")]
        public IActionResult StationTypeImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<StationTypeInfo> stationTypes = new();
            using (var stream = formFile.OpenReadStream())
            {
                stationTypes = stream.Query<StationTypeInfo>(startCell: "A1").ToList();
            }
            return SUCCESS(service.StationTypeImportData(stationTypes, HttpContext.GetSite(), HttpContext.GetName()));
        }

        /// <summary>
        /// 获取站点类型
        /// </summary>
        [HttpGet("GetStationType")]
        public Task<IActionResult> GetStationType()
        {

            return Task.FromResult<IActionResult>(Ok(service.GetStationType()));
        }
        /// <summary>
        /// 获取客户端类型
        /// </summary>
        [HttpGet("GetClientType")]
        public Task<IActionResult> GetClientType()
        {

            return Task.FromResult<IActionResult>(Ok(service.GetClientType()));
        }

//----------------------------------------站点分割线-----------------------------------------------------------------------------------
        /// <summary>
        /// 获取流程名
        /// </summary>
        [HttpGet("GetRouteName")]
        public Task<IActionResult> GetRouteName(string? routeName)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.GetRouteName(routeName, site)));
        }
        /// <summary>
        /// 获取工艺流程（左边）
        /// </summary>
        [HttpGet("GetStationTypeInfo")]
        public Task<IActionResult> GetStationTypeInfo(string? stage)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.GetStationTypeInfo(stage, site)));
        }
        /// <summary>
        /// 获取工艺流程（右边）
        /// </summary>
        [HttpGet("GetRouteDetail")]
        public Task<IActionResult> GetRouteDetail(string? routeName)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.GetRouteDetail(routeName, site)));
        }
        /// <summary>
        /// 站点类型信息导出
        /// </summary>
        /// <param name="routeName"></param>
        /// <param name="site"></param>
        [HttpGet("ShowRouteDetailExport")]
        [Log(Title = "流程信息导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowRouteDetailExport(string routeName)
        {
            var site = HttpContext.GetSite();
            var list = service.GetRouteDetail(routeName, site);
            var result = ExportExcelMini(list, "RouteMaintenanceInfoVo", "流程信息导出");
            return ExportExcel(result.Item2, result.Item1);
        }
        /// <summary>
        /// 获取流程WIP
        /// </summary>
        [HttpGet("CheckRouteWIP")]
        public Task<IActionResult> CheckRouteWIP(string? RouteName)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.CheckRouteWIP(RouteName, site)));
        }
        /// <summary>
        /// 删除流程
        /// </summary>
        [HttpPut("UpdateRoute")]
        public async Task<IActionResult> UpdateRoute(string routeName)
        {
            RouteInfo routeInfo = new RouteInfo();
            var name = HttpContext.GetName();
            routeInfo.updateEmpno = name;
            routeInfo.updateTime = DateTime.Now;
            routeInfo.routeName = routeName;
            return Ok(service.UpdateRoute(routeInfo));
        }
        /// <summary>
        /// 修改工艺是非为必要站
        /// </summary>
        [HttpPut(" UpdateMustStation")]
        public async Task<IActionResult> UpdateMustStation(string routeName, string stationType, string enaBled)
        {
            RouteDetailInfo routeDetailInfo = new RouteDetailInfo();
            routeDetailInfo.site =HttpContext.GetSite();
            routeDetailInfo.updateEmpno = HttpContext.GetName();
            routeDetailInfo.updateTime = DateTime.Now;
            routeDetailInfo.routeName = routeName;
            routeDetailInfo.stationType = stationType;
            routeDetailInfo.enaBled = enaBled;  

            return Ok(service.UpdateMustStation(routeDetailInfo));
        }
        /// <summary>
        ///新增流程
        /// </summary>
        [HttpPost("CheckRouteName")]
        public Task<IActionResult> CheckRouteName(List<RouteDetailVo> routeDetailVo)
        {

            Console.WriteLine("-----------"+routeDetailVo[0].routeName);
            var name = HttpContext.GetName();
            var site = HttpContext.GetSite();

            return Task.FromResult<IActionResult>(Ok(service.CheckRouteName(routeDetailVo, site,name)));
        }
    }

}
