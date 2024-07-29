using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.DataCenter
{
    /// <summary>
    /// 产品信息->标准工时维护
    /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("MntnlStandard/StandardTime/")]
    [ApiController]
    public class MntnlStandardTimeController : BaseController
    {

        readonly IMntnlStandardTimeService service;

        public MntnlStandardTimeController(IMntnlStandardTimeService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 标准工时维护-->分页查询
        /// </summary>
        [HttpGet("StandardTimelist")]
        public Task<IActionResult> StandardTimelist(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.StandardTimelist(enaBled, optionData, textData, pageNum, pageSize, HttpContext.GetSite())));
        }

        /// <summary>
        /// 标准工时维护导出
        /// </summary>
        [HttpGet("StandardTimeExport")]
        [Log(Title = "标准工时维护导出", BusinessType = BusinessType.EXPORT)]
        //[ActionPermissionFilter(Permission = "system:user:export")]
        public IActionResult UserExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.StandardTimelist(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());

            var result = ExportExcelMini(list.Result, "StandardTimeExport", "标准工时维护导出");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 标准工时维护导入
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        /// <returns></returns>
        [HttpPost(" StandardTimeImport")]
        [Log(Title = "标准工时维护导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        public IActionResult StandardTimeImport([FromForm(Name = "file")] IFormFile formFile)
        {
            List<imesMstandardtime> imes = new();
            using (var stream = formFile.OpenReadStream())
            {
                imes = stream.Query<imesMstandardtime>(startCell: "A1").ToList();
            }
            return SUCCESS(service.StandardTimeImport(imes, HttpContext.GetSite(), HttpContext.GetName()));
        }

        /// <summary>
        /// 标准工时维护-->历史查询
        /// </summary>
        [HttpGet("StandardTimelistHt")]
        public Task<IActionResult> StandardTimelistHt(int id)
        {
            return Task.FromResult<IActionResult>(Ok(service.StandardTimelistHt(id, HttpContext.GetSite())));
        }

        /// <summary>
        /// 标准工时维护-->修改
        /// </summary>
        [HttpPut("StandardTimeUpdate")]
        public Task<IActionResult> StandardTimeUpdate(imesMstandardtime imesMsn)
        {
            imesMsn.updateEmpno = HttpContext.GetName();
            imesMsn.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.StandardTimeUpdate(imesMsn)));
        }

        /// <summary>
        /// 标准工时维护-->删除
        /// </summary>
        [HttpDelete("StandardTimeDelet")]
        public Task<IActionResult> StandardTimeDelet( imesMstandardtime imesMsn)
        {
            imesMsn.updateEmpno = HttpContext.GetName();
            imesMsn.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.StandardTimeDelet(imesMsn)));
        }

        /// <summary>
        /// 标准工时维护-->复制(传ID和输入框里的参数)
        /// </summary>
        [HttpPost("StationCopy")]
        public Task<IActionResult> StandardTimeCopy(imesMstandardtime imes)
        {
            imes.site = HttpContext.GetSite();
            string name = HttpContext.GetName();
            imes.createEmpno = name;
            imes.updateEmpno = name;
            return Task.FromResult<IActionResult>(Ok(service.StandardTimeCopy(imes)));
        }

        /// <summary>
        /// 标准工时维护-->新增(厂内料号/机种/机种负责人/备注信息/线别/CT/人力 不能为空)
        /// </summary>
        [HttpPost("StandardTimeInsert")]
        public Task<IActionResult> StandardTimeInsert(imesMstandardtime imes)
        {
            imes.site = HttpContext.GetSite();
            string name = HttpContext.GetName();
            imes.createEmpno = name;
            imes.updateEmpno = name;
            return Task.FromResult<IActionResult>(Ok(service.StandardTimeInsert(imes)));
        }

        /// <summary>
        /// 标准工时维护-->厂内料号(长度要大于4个字符，才触发这个接口)
        /// </summary>
        [HttpGet("FactoryStandardTimelist")]
        public Task<IActionResult> FactoryStandardTimelist(string? ipn)
        {
            return Task.FromResult<IActionResult>(Ok(service.FactoryStandardTimelist(ipn, HttpContext.GetSite())));
        }

        /// <summary>
        /// 标准工时维护-->客户(长度要大于3个字符，才触发这个接口)
        /// </summary>
        [HttpGet("ModelStandardTimelist")]
        public Task<IActionResult> ModelStandardTimelist(string? model)
        {
            return Task.FromResult<IActionResult>(Ok(service.ModelStandardTimelist(model, HttpContext.GetSite())));
        }

        /// <summary>
        /// 标准工时维护-->MES站点(长度要大于2个字符，才触发这个接口)
        /// </summary>
        [HttpGet("StationtypeStandardTimelist")]
        public Task<IActionResult> StationtypeStandardTimelist(string? stationtype, string? line)
        {
            return Task.FromResult<IActionResult>(Ok(service.StationtypeStandardTimelist(stationtype, line, HttpContext.GetSite())));
        }

        /// <summary>
        /// 标准工时维护-->获取线别
        /// </summary>
        [HttpGet("LineStandardTimelist")]
        public Task<IActionResult> LineStandardTimelist()
        {
            return Task.FromResult<IActionResult>(Ok(service.LineStandardTimelist(HttpContext.GetSite())));
        }

        /// <summary>
        /// 标准工时维护-->获取机种负责人
        /// </summary>
        [HttpGet("ModelNameStandardTimeList")]
        public Task<IActionResult> ModelNameStandardTimeList()
        {
            return Task.FromResult<IActionResult>(Ok(service.ModelNameStandardTimeList(HttpContext.GetSite())));
        }


    }
}
