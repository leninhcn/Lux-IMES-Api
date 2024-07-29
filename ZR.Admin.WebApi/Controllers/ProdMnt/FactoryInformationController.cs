using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using ZR.Model.Dto;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers
{
 /// <summary>
 /// 基础信息 -> 厂区信息
 /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("prodMnt/FactoryInformation/")]
    [ApiController]
    public class FactoryInformationController : BaseController
    {

        readonly IFactoryInformationService service;

        public FactoryInformationController(IFactoryInformationService service) {
        
           this.service = service;
        }
        /// <summary>
        /// 删除厂区数据
        /// </summary>
        [HttpDelete("DeleteSiteByID")]
        public async Task<IActionResult> DeleteSiteByID(SiteInfo siteInfo)
        {
            var name = HttpContext.GetName();
            siteInfo.updateEmpno = name;
            siteInfo.updateTime = DateTime.Now;
            return Ok(service.DeleteSiteByID(siteInfo));
        }

        /// <summary>
        /// 厂区状态修改
        /// </summary>
        [HttpPut("UpdateSite")]
        public async Task<IActionResult> UpdateSite(SiteInfo siteInfo)
        {
            var name = HttpContext.GetName();
            siteInfo.updateEmpno = name;
            siteInfo.updateTime = DateTime.Now;
            return Ok(service.UpdateSite(siteInfo));
        }

        /// <summary>
        /// 新增厂区信息
        /// </summary>
        [HttpPut("InsertSiteInfo")]
        public async Task<IActionResult> InsertSiteInfo(SiteInfo siteInfo)
        {
            var name = HttpContext.GetName();
            siteInfo.updateEmpno = name;
            siteInfo.updateTime = DateTime.Now;
            siteInfo.createEmpno = name;
            siteInfo.createTime = DateTime.Now;
            siteInfo.enabled = "Y";
            return Ok(service.InsertSiteInfo(siteInfo));
        }

        /// <summary>
        /// 厂区信息获取
        /// </summary>
        [HttpGet("ShowSiteInfo")]
        public Task<IActionResult> ShowSiteInfo(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {

            return Task.FromResult<IActionResult>(Ok(service.ShowSiteInfo(enaBled, optionData, textData, pageNum, pageSize)));
        }
        /// <summary>
        /// 厂区数据导出
        /// </summary>
        /// <param name="enaBled"></param>
        /// <param name="optionData"></param>
        /// <param name="textData"></param>
        /// <param name="pageSize"></param>
        [HttpGet("ShowSiteExport")]
        [Log(Title = "厂区数据导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowSiteExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.ShowSiteInfo(enaBled, optionData, textData, 1, pageSize);
            var result = ExportExcelMini(list.Result, "SiteInfo", "厂区数据导出");
            return ExportExcel(result.Item2, result.Item1);
        }
        /// <summary>
        /// 厂区历史记录获取
        /// </summary>
        [HttpGet("SiteHistory")]
        public async Task<IActionResult> SiteHistory(string id)
        {
            return Ok(service.SiteHistory(id));

        }
        //----------------------------------------厂区分割线-----------------------------------------------------------------------------------
        /// <summary>
        /// 获取厂区
        /// </summary>
        [HttpGet("GetSiteCode")]
        public Task<IActionResult> GetSiteCode()
        {

            return Task.FromResult<IActionResult>(Ok(service.GetSiteCode()));
        }
        /// <summary>
        /// 获取线体
        /// </summary>
        [HttpGet("GetLineType")]
        public Task<IActionResult> GetLineType()
        {

            return Task.FromResult<IActionResult>(Ok(service.GetLineType()));
        }

        /// <summary>
        /// 获取线体等级
        /// </summary>
        [HttpGet("GetLineLevel")]
        public Task<IActionResult> GetLineLevel()
        {

            return Task.FromResult<IActionResult>(Ok(service.GetLineLevel())); 
        }
        /// <summary>
        /// 获取工作中心
        /// </summary>
        [HttpGet("GetWorkCenter")]
        public Task<IActionResult> GetWorkCenter()
        {

            return Task.FromResult<IActionResult>(Ok(service.GetWorkCenter())); 
        }
        /// <summary>
        /// 删除线体数据
        /// </summary>
        [HttpDelete("DeleteLineByID")]
        public async Task<IActionResult> DeleteLineByID(LineInfo lineInfo)
        {
            var name = HttpContext.GetName();
            lineInfo.updateEmpno = name;
            lineInfo.updateTime = DateTime.Now;
            return Ok(service.DeleteLineByID(lineInfo));
        }

        /// <summary>
        /// 线体状态修改
        /// </summary>
        [HttpPut("UpdateLine")]
        public async Task<IActionResult> UpdateLine(LineInfo lineInfo)
        {
            var name = HttpContext.GetName();
            lineInfo.updateEmpno = name;
            lineInfo.updateTime = DateTime.Now;
            return Ok(service.UpdateLine(lineInfo));
        }
        /// <summary>
        /// 新增线体信息
        /// </summary>
        [HttpPut("InsertLineInfo")]
        public async Task<IActionResult> InsertLineInfo(LineInfo lineInfo)
        {
            var name = HttpContext.GetName();
            lineInfo.updateEmpno = name;
            lineInfo.updateTime = DateTime.Now;
            lineInfo.createEmpno = name;
            lineInfo.createTime = DateTime.Now;
            lineInfo.enabled = "Y";
            return Ok(service.InsertLineInfo(lineInfo));
        }

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
        [HttpGet("ShowLineExport")]
        [Log(Title = "线体数据导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowLineExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.ShowLineInfo(enaBled, optionData, textData, 1, pageSize);
            var result = ExportExcelMini(list.Result, "LineInfo", "线体数据导出");
            return ExportExcel(result.Item2, result.Item1);
        }
        /// <summary>
        /// 线体历史记录获取
        /// </summary>
        [HttpGet("LineHistory")]
        public async Task<IActionResult> LineHistory(string id)
        {
            return Ok(service.LineHistory(id));

        }
        /// <summary>
        /// 线体信息导入
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        /// <returns></returns>
        [HttpPost("LineImportData")]
        [Log(Title = "线体导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        //[ActionPermissionFilter(Permission = "system:user:import")]
        public IActionResult LineImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<LineInfo> Line  = new();
            using (var stream = formFile.OpenReadStream())
            {
                Line = stream.Query<LineInfo>(startCell: "A1").ToList();
            }
            return SUCCESS(service.LineImportData(Line, HttpContext.GetSite(), HttpContext.GetName()));
        }
 //----------------------------------------线别维护分割线-----------------------------------------------------------------------------------

        /// <summary>
        /// 获取工站维护线别
        /// </summary>
        [HttpGet("GetLine")]
        public Task<IActionResult> GetLine(string ? line)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.GetLine(line, site)));
        }
        /// <summary>
        /// 获取工站维护站点信息（右边）
        /// </summary>
        [HttpGet("GetLineStation")]
        public async Task<IActionResult> GetLineStation(string line)
        {
            var site = HttpContext.GetSite();
            return Ok(service.GetLineStation(line,site));
        }

        /// <summary>
        /// 获取工站站点信息（左边）
        /// </summary>
        [HttpGet("GetStageStationtype")]
        public async Task<IActionResult> GetStageStationtype(string ? stage)
        {
            var site = HttpContext.GetSite();
            return Ok(service.GetStageStationtype(stage, site));
        }
        /// <summary>
        /// 站点数据导出
        /// </summary>
        /// <param name="line"></param>
        [HttpGet("GetLineStationExport")]
        [Log(Title = "站点数据导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult GetLineStationExport(string line)
        {
            var site = HttpContext.GetSite();
            var list = service.GetLineStationOutExport(line,site);
            var result = ExportExcelMini(list, "StationVo", "站点数据导出");
            return ExportExcel(result.Item2, result.Item1);
        }  
        
        /// <summary>
           /// 工站信息导入
           /// </summary>
           /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
           /// <returns></returns>
        [HttpPost("StationImportData")]
        [Log(Title = "工站导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        //[ActionPermissionFilter(Permission = "system:user:import")]
        public IActionResult StationImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<StationInfo> stationInfo = new();
            using (var stream = formFile.OpenReadStream())
            {
                stationInfo = stream.Query<StationInfo>(startCell: "A1").ToList();
            }
            return SUCCESS(service.StationImportData(stationInfo, HttpContext.GetSite(), HttpContext.GetName()));
        }
     
        /// <summary>
        /// 新增站点信息
        /// </summary>
        [HttpPut("InsertStationInfo")]
        public async Task<IActionResult> InsertStationInfo(StationInfoVo stationInfoVo)
        {
            var name = HttpContext.GetName();
            var site= HttpContext.GetSite();
            stationInfoVo.site = site;
            stationInfoVo.updateEmpno = name;
            stationInfoVo.createEmpno = name;
            return Ok(service.InsertStationInfo(stationInfoVo));
        }
        /// <summary>
        /// 删除工站数据
        /// </summary>
        [HttpDelete("DeleteStation")]
        public async Task<IActionResult> DeleteStation(StationInfoVo stationInfoVo)
        {
            var site = HttpContext.GetSite();
            stationInfoVo.site = site;
            var name = HttpContext.GetName();
            stationInfoVo.updateEmpno = name;
            return Ok(service.DeleteStation(stationInfoVo));
        }

    }
}
