using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Service;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.ProdMnt
{

    /// <summary>
    /// 基础信息 -> 其他信息
    /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("prodMnt/StationPassQty/")]
    [ApiController]
    public class StationPassQtyController : BaseController
    {
        readonly IStationPassQtyService service;

        public StationPassQtyController(IStationPassQtyService service)
        {
            this.service = service;
        }
        /// <summary>
        /// 线体获取
        /// </summary>
        [HttpGet("GetLineInfo")]
        public async Task<IActionResult> GetLineInfo()
        {
            string site = HttpContext.GetSite();
            return Ok(service.GetLineInfo(site));
        }
        /// <summary>
        /// 站点类型获取
        /// </summary>
        [HttpGet("GetStationTypeInfo")]
        public async Task<IActionResult> GetStationTypeInfo(string line)
        {

            string site = HttpContext.GetSite();
            return Ok(service.GetStationTypeInfo(line,site));

        }
        /// <summary>
        /// 站点名获取
        /// </summary>
        [HttpGet("GetStationNameInfo")]
        public async Task<IActionResult> GetStationNameInfo(string line, string stationType)
        {
            
            string site = HttpContext.GetSite();
            return Ok(service.GetStationNameInfo(line, stationType, site));

        }
        /// <summary>
        /// 站点信息获取
        /// </summary>
        [HttpGet("GetStationInfo")]
        public async Task<IActionResult> GetStationInfo(string ? line, string ? stationType, string ? stationName,int pageNum,int pageSize)
        {
      
            string site = HttpContext.GetSite();
            return Ok(service.GetStationInfo(line, stationType, stationName, site, pageNum, pageSize));

        }

        /// <summary>
        /// 过站次数维护
        /// </summary>
        [HttpPut("UpdateStationInfo")]
        public async Task<IActionResult> UpdateStationInfo(StationInfos stationInfos)
        {

            string site = HttpContext.GetSite();
            var name = HttpContext.GetName();
            stationInfos.updateTime = DateTime.Now;
            stationInfos.updateEmpno = name;
            stationInfos.site = site;
            return Ok(service.UpdateStationInfo(stationInfos));

        }

        //----------------------------------------分割线---------------------------------------------------------------

        /// <summary>
        /// 重量数据获取
        /// </summary>
        [HttpGet("ShowData")] 
        public  Task<IActionResult> ShowData(string ? enaBled,string ? optionData,string ? textData, int pageNum, int pageSize)
        {
            string site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.ShowData(enaBled, optionData, textData, pageNum, pageSize, site)));
        }

        /// <summary>
        /// 重量数据导出
        /// </summary>
        /// <param name="enaBled"></param>
        /// <param name="optionData"></param>
        /// <param name="textData"></param>
        /// <param name="pageSize"></param>
        /// <param name="user"></param>
        [HttpGet("ShowExport")]
        [Log(Title = "重量数据导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.ShowData(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());
            var result = ExportExcelMini(list.Result, "WeightFaiInfo", "重量数据导出");

            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 重量数据新增
        /// </summary>
        [HttpPut("InsertWeightFai")]
        public async Task<IActionResult> InsertWeightFai(WeightFaiInfo weightFaiInfo)
        {
            string site = HttpContext.GetSite();
            var name =  HttpContext.GetName();
            weightFaiInfo.createEmpno = name;
            weightFaiInfo.updateEmpno = name;
            return Ok(service.InsertWeightFai(weightFaiInfo, site));

        }
        /// <summary>
        /// 检查ICT是否存在
        /// </summary>
        [HttpGet("ExistIPN")]
        public async Task<IActionResult> ExistIPN(string ipn)
        {


            return Ok(service.ExistIPN(ipn));

        }
        /// <summary>
        /// 重量数据修改
        /// </summary>
        [HttpPut("UpdateWeightFai")]
        public async Task<IActionResult> UpdateWeightFai(WeightFaiInfo weightFaiInfo)
        {

            string site = HttpContext.GetSite();
            return Ok(service.UpdateWeightFai(weightFaiInfo,site));

        }
        /// <summary>
        /// 状态修改
        /// </summary>
        [HttpPut("UpdateWeightFaiState")]
        public async Task<IActionResult> UpdateWeightFaiState(WeightFaiInfo weightFaiInfo)
        {

            string site = HttpContext.GetSite();
            return Ok(service.UpdateWeightFaiState(weightFaiInfo, site));
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        [HttpDelete("DeleteWeightFai")]
        public async Task<IActionResult> DeleteWeightFai(WeightFaiInfo weightFai)
        {
            string site=HttpContext.GetSite();
            return Ok(service.DeleteWeightFai(weightFai.id, site));
        }
        /// <summary>
        /// 历史记录获取
        /// </summary>
        [HttpGet("History")]
        public async Task<IActionResult> History(string id)
        {

            string site = HttpContext.GetSite();
            return Ok(service.History(id, site));

        }

    }
}