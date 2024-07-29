using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Model.System;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.DataCenter
{
     /// <summary>
     /// 产品信息->品名对应关系维护
     /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("prodMnt/prodModel/")]
    [ApiController]
    public class MntnErpMesController : BaseController
    {

        readonly IMntnErpMesService service;

        public MntnErpMesController(IMntnErpMesService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 品名对应关系-->查询
        /// </summary>
        [HttpGet("ErpMesllist")]
        public Task<IActionResult> ErpMeslist(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.ErpMeslist(enaBled, optionData, textData, pageNum, pageSize, HttpContext.GetSite())));
        }

        /// <summary>
        /// 品名对应关系导出
        /// </summary>
        /// <param name="enaBled"></param>
        /// <param name="optionData"></param>
        /// <param name="textData"></param>
        /// <param name="pageSize"></param>
        /// <param name="user"></param>
        [HttpGet("ErpExport")]
        [Log(Title = "品名对应关系导出", BusinessType = BusinessType.EXPORT)]
        //[ActionPermissionFilter(Permission = "system:user:export")]
        public IActionResult UserExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.ErpMeslist(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());
            Console.WriteLine("导出的list："+list);
            var result = ExportExcelMini(list.Result, "Model", "品名对应关系数据");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 品名对应关系导入
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        /// <returns></returns>
        [HttpPost("ErpImportData")]
        [Log(Title = "品名对应关系导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        //[ActionPermissionFilter(Permission = "system:user:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<ImesMpartSpecErpMesMapping> imes = new();
            using (var stream = formFile.OpenReadStream())
            {
                imes = stream.Query<ImesMpartSpecErpMesMapping>(startCell: "A1").ToList();
            }
            return SUCCESS(service.ImportErp(imes, HttpContext.GetSite(), HttpContext.GetName()));
        }

        /// <summary>
        /// 品名对应关系-->新增->机种查询
        /// </summary>
        [HttpGet("ErpMeslModellist")]
        public Task<IActionResult> ErpMeslModellist()
        {
            return Task.FromResult<IActionResult>(Ok(service.ErpMeslModellist(HttpContext.GetSite())));
        }

        /// <summary>
        /// 品名对应关系-->新增->StageName查询
        /// </summary>
        [HttpGet("ErpMeslStageNamelist")]
        public Task<IActionResult> ErpMeslStageNamelist()
        {
            return Task.FromResult<IActionResult>(Ok(service.ErpMeslStageNamelist(HttpContext.GetSite())));
        }

        /// <summary>
        /// 品名对应关系-->新增
        /// </summary>
        [HttpPost("ErpMesInsert")]
        public Task<IActionResult> ErpMesInsert(ImesMpartSpecErpMesMapping imesMpart)
        {
            imesMpart.updateEmpno = HttpContext.GetName();
            imesMpart.createEmpno = HttpContext.GetName();
            imesMpart.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.ErpMesInsert(imesMpart)));
        }

        /// <summary>
        /// 品名对应关系-->修改
        /// </summary>
        [HttpPut("ErpMesUpdate")]
        public Task<IActionResult> ErpMesUpdate( ImesMpartSpecErpMesMapping imesMpart)
        {
            imesMpart.updateEmpno = HttpContext.GetName();
            imesMpart.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.ErpMesUpdate(imesMpart)));
        }

        /// <summary>
        /// 品名对应关系-->删除
        /// </summary>
        [HttpDelete("ErpMesDelet")]
        public Task<IActionResult> ErpMesDelet(ImesMpartSpecErpMesMapping imesMpart)
        {
            imesMpart.updateEmpno = HttpContext.GetName();
            imesMpart.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.ErpMesDelet(imesMpart)));
        }

        /// <summary>
        /// 品名对应关系-->历史查询
        /// </summary>
        [HttpGet("ErpMeslistHt")]
        public Task<IActionResult> ErpMeslistHt( int id)
        {
            return Task.FromResult<IActionResult>(Ok(service.ErpMeslistHt(id, HttpContext.GetSite())));
        }

        //-----------------分割线-------------------------------------------------------------------------------------


        /// <summary>
        /// 站点材料绑定-->分页查询
        /// </summary>
        [HttpGet("Stationlist")]
        public Task<IActionResult> Stationlist(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.Stationlist(enaBled, optionData, textData, HttpContext.GetSite(), pageNum, pageSize)));
        }

        /// <summary>
        /// 站点材料绑定导出
        /// </summary>
        /// <param name="enaBled"></param>
        /// <param name="optionData"></param>
        /// <param name="textData"></param>
        /// <param name="pageSize"></param>
        /// <param name="user"></param>
        [HttpGet("StationExport")]
        [Log(Title = "站点材料导出", BusinessType = BusinessType.EXPORT)]
        //[ActionPermissionFilter(Permission = "system:user:export")]
        public IActionResult StationExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.Stationlist(enaBled, optionData, textData, HttpContext.GetSite(), 1, pageSize);

            var result = ExportExcelMini(list.Result, "Model", "站点材料导出");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 站点材料绑定导入
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        [HttpPost("StationImportData")]
        [Log(Title = "品名对应关系导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        //[ActionPermissionFilter(Permission = "system:user:import")]
        public IActionResult StationImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<ImesMstationTypePartSpec> imes = new();
            using (var stream = formFile.OpenReadStream())
            {
                imes = stream.Query<ImesMstationTypePartSpec>(startCell: "A1").ToList();
            }
            return SUCCESS(service.StationImportData(imes, HttpContext.GetSite(), HttpContext.GetName()));
        }

        /// <summary>
        /// 站点材料绑定-->查询-->工段查询
        /// </summary>
        [HttpGet("StationStagelist")]
        public Task<IActionResult> StationStagelist()
        {
            return Task.FromResult<IActionResult>(Ok(service.StationStagelist(HttpContext.GetSite())));
        }

        /// <summary>
        /// 站点材料绑定-->查询-->站点类型(stage工段必须有数据,无数据不能触发此接口)
        /// </summary>
        [HttpGet("StationTypelist")]
        public Task<IActionResult> StationTypelist(string stage, string? stationType)
        {
            return Task.FromResult<IActionResult>(Ok(service.StationTypelist(stage, stationType, HttpContext.GetSite())));
        }

        /// <summary>
        /// 站点材料绑定-->查询-->机种查询
        /// </summary>
        [HttpGet("StationModellist")]
        public Task<IActionResult> StationModellist()
        {
            return Task.FromResult<IActionResult>(Ok(service.StationModellist(HttpContext.GetSite())));
        }

        /// <summary>
        /// 站点材料绑定-->查询-->品名
        /// </summary>
        [HttpGet("StationBrandlist")]
        public Task<IActionResult> StationBrandlist(string? mesSpec, string? stageName)
        {
            return Task.FromResult<IActionResult>(Ok(service.StationBrandlist(mesSpec, stageName, HttpContext.GetSite())));
        }

        /// <summary>
        /// 站点材料绑定-->新增
        /// </summary>
        [HttpPost("StationInsert")]
        public Task<IActionResult> StationInsert(ImesMstationTypePartSpec imesMstation)
        {
            imesMstation.site = HttpContext.GetSite();
            string name =  HttpContext.GetName();
            imesMstation.createEmpno = name;
            imesMstation.updateEmpno = name;
            return Task.FromResult<IActionResult>(Ok(service.StationInsert(imesMstation)));
        }

        /// <summary>
        /// 站点材料绑定-->删除
        /// </summary>
        [HttpDelete("StationDelete")]
        public Task<IActionResult> StationDelete( ImesMstationTypePartSpec imesMpart)
        {
            imesMpart.updateEmpno = HttpContext.GetName();
            imesMpart.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.StationDelete(imesMpart)));
        }


        /// <summary>
        /// 站点材料绑定-->修改
        /// </summary>
        [HttpPut("StationtUpdate")]
        public Task<IActionResult> StationtUpdate(ImesMstationTypePartSpec imesMpart)
        {
            imesMpart.updateEmpno = HttpContext.GetName();
            imesMpart.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.StationtUpdate(imesMpart)));
        }

        /// <summary>
        /// 站点材料绑定-->复制(传ID和输入框里的参数)
        /// </summary>
        [HttpPost("StationCopy")]
        public Task<IActionResult> StationCopy(ImesMstationTypePartSpec imesMstation)
        {
            imesMstation.site = HttpContext.GetSite();
            string name = HttpContext.GetName();
            imesMstation.createEmpno = name;
            imesMstation.updateEmpno = name;
            return Task.FromResult<IActionResult>(Ok(service.StationCopy(imesMstation)));
        }

        /// <summary>
        /// 站点材料绑定-->历史查询
        /// </summary>
        [HttpGet("StationtlistHt")]
        public Task<IActionResult> StationtlistHt(int id)
        {
            return Task.FromResult<IActionResult>(Ok(service.StationtlistHt(id, HttpContext.GetSite())));
        }

        /// <summary>
        /// 站点材料绑定-->修改调解耦
        /// </summary>
        [HttpGet("StationtlistStage")]
        public Task<IActionResult> StationtlistStage(string stationType)
        {
            return Task.FromResult<IActionResult>(Ok(service.StationtlistStage( stationType, HttpContext.GetSite())));
        }
    }
}
