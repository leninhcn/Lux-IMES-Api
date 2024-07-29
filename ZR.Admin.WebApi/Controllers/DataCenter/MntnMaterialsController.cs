using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using ZR.Model.Dto.ProdDto;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.DataCenter
{
     /// <summary>
     /// 产品信息->料号特征管理
     /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("prodMnt/MntnMaterials/")]
    [ApiController]
    public class MntnMaterialsController : BaseController
    {


        readonly IMntnMaterialsService service;

        public MntnMaterialsController(IMntnMaterialsService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 料号特征管理-->分页查询
        /// </summary>
        [HttpGet("Materialsllist")]
        public Task<IActionResult> Materialsllist(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.Materialsllist(enaBled, optionData, textData, pageNum, pageSize, HttpContext.GetSite())));
        }

        /// <summary>
        /// 料号特征管理导出
        /// </summary>
        [HttpGet("MaterialsExport")]
        [Log(Title = "料号特征导出", BusinessType = BusinessType.EXPORT)]
        //[ActionPermissionFilter(Permission = "system:user:export")]
        public IActionResult UserExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.Materialsllist(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());

            var result = ExportExcelMini(list.Result, "Model", "料号特征数据导出");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 料号特征管理导入
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        [HttpPost("MaterialsImportData")]
        [Log(Title = "料号特征管理导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        public IActionResult MaterialsImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<ImesMsnFeature> imes = new();
            using (var stream = formFile.OpenReadStream())
            {
                imes = stream.Query<ImesMsnFeature>(startCell: "A1").ToList();
            }
            return SUCCESS(service.MaterialsImportData(imes, HttpContext.GetSite(), HttpContext.GetName()));
        }


        /// <summary>
        /// 料号特征管理-->新增(料号特征码不允许出现#！0该数据已存在/1成功/2失败，请检查数据)
        /// </summary>
        [HttpPost("MaterialInsert")]
        public Task<IActionResult> MaterialInsert(ImesMsnFeature imesMsn)
        {
            imesMsn.updateEmpno = HttpContext.GetName();
            imesMsn.createEmpno = HttpContext.GetName();
            imesMsn.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.MaterialInsert(imesMsn)));
        }


        /// <summary>
        /// 料号特征管理-->修改(Ipn/SnFeature/Enabled/id不能为空--0该数据已存在/1成功/2失败，请检查数据)
        /// </summary>
        [HttpPut("MaterialsUpdate")]
        public Task<IActionResult> MaterialsUpdate(ImesMsnFeature imesMsn)
        {
            imesMsn.updateEmpno = HttpContext.GetName();
            imesMsn.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.MaterialsUpdate(imesMsn)));
        }


        /// <summary>
        /// 料号特征管理-->删除
        /// </summary>
        [HttpDelete("MaterialsDelet")]
        public Task<IActionResult> MaterialsDelet(ImesMsnFeature imesMsn)
        {
            imesMsn.updateEmpno = HttpContext.GetName();
            imesMsn.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.MaterialsDelet(imesMsn)));
        }


        /// <summary>
        /// 料号特征管理-->历史查询
        /// </summary>
        [HttpGet("MaterialsllistHt")]
        public Task<IActionResult> MaterialsllistHt([FromQuery] ImesMsnFeature imesMsn)
        {
            return Task.FromResult<IActionResult>(Ok(service.MaterialsllistHt( imesMsn.id, HttpContext.GetSite())));
        }


        /// <summary>
        /// 料号特征管理-->料号下拉框获取(料号文本框无数据，提示在文本框数数据/传值(Ipn、NoIpn--input框返回的数据不一样))
        /// </summary>
        [HttpGet("MaterialslIpnlist")]
        public Task<IActionResult> MaterialslIpnlist(string dateIpn , string? dateIpnText)
        {
            return Task.FromResult<IActionResult>(Ok(service.MaterialslIpnlist(dateIpn, dateIpnText, HttpContext.GetSite())));
        }

        /// <summary>
        /// 料号特征管理-->材料品名赋值(未查询到IPN对应的SPEC1不存在或者未维护料号对应关系,请确认！)
        /// </summary>
        [HttpGet("MaterialslDescriptionlist")]
        public Task<IActionResult> MaterialslDescriptionlist(string dateIpn, string? dateIpnText)
        {
            return Task.FromResult<IActionResult>(Ok(service.MaterialslDescriptionlist(dateIpn, dateIpnText, HttpContext.GetSite())));
        }

        //-------------------------------分隔---------------------------------------------------------------------------

        /// <summary>
        /// 料号工艺流程维护-->分页查询
        /// </summary>
        [HttpGet("MntnPartRoutelist")]
        public Task<IActionResult> MntnPartRoutelist(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.MntnPartRoutelist(enaBled, optionData, textData, pageNum, pageSize, HttpContext.GetSite())));
        }

        /// <summary>
        /// 料号工艺流程导出
        /// </summary>
        [HttpGet("MntnPartRouteExport")]
        [Log(Title = "料号工艺流程导出", BusinessType = BusinessType.EXPORT)]
        //[ActionPermissionFilter(Permission = "system:user:export")]
        public IActionResult MntnPartRouteExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.MntnPartRoutelist(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());

            var result = ExportExcelMini(list.Result, "Model", "料号工艺流程数据导出");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 料号工艺流程导入
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        [HttpPost("MntnPartRouteImportData")]
        [Log(Title = "料号工艺流程导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        public IActionResult MntnPartRouteImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<ImesMpartRoute> imes = new();
            using (var stream = formFile.OpenReadStream())
            {
                imes = stream.Query<ImesMpartRoute>(startCell: "A1").ToList();
            }
            return SUCCESS(service.MntnPartRouteImportData(imes, HttpContext.GetSite(), HttpContext.GetName()));
        }

        /// <summary>
        /// 料号工艺流程维护-->修改()
        /// </summary>
        [HttpPut("MntnPartRouteUpdate")]
        public Task<IActionResult> MntnPartRouteUpdate(ImesMpartRoute imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.MntnPartRouteUpdate(imes)));
        }

        /// <summary>
        /// 料号工艺流程维护-->删除
        /// </summary>
        [HttpDelete("MntnPartRouteDelet")]
        public Task<IActionResult> MntnPartRouteDelet(ImesMpartRoute imesMsn)
        {
            imesMsn.updateEmpno = HttpContext.GetName();
            imesMsn.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.MntnPartRouteDelet(imesMsn)));
        }


        /// <summary>
        /// 料号工艺流程维护-->历史查询
        /// </summary>
        [HttpGet("MntnPartRoutelistHt")]
        public Task<IActionResult> MntnPartRoutelistHt([FromQuery] ImesMpartRoute imesMsn)
        {
            return Task.FromResult<IActionResult>(Ok(service.MntnPartRoutelistHt(imesMsn.id, HttpContext.GetSite())));
        }

        /// <summary>
        /// 料号工艺流程维护-->IPN获取非必须
        /// </summary>
        [HttpGet("MntnPartRouteIpnlist")]
        public Task<IActionResult> MntnPartRouteIpnlist(string? ipn)
        {
            return Task.FromResult<IActionResult>(Ok(service.MntnPartRouteIpnlist(ipn, HttpContext.GetSite())));
        }

        /// <summary>
        /// 料号工艺流程维护-->途程名称 routeName非必须
        /// </summary>
        [HttpGet("MntnPartRouteRoadNamelist")]
        public Task<IActionResult> MntnPartRouteRoadNamelist(string? routeName)
        {
            return Task.FromResult<IActionResult>(Ok(service.MntnPartRouteRoadNamelist(routeName, HttpContext.GetSite())));
        }

        /// <summary>
        /// 料号工艺流程维护-->规则组合ruleSetName非必须
        /// </summary>
        [HttpGet("MntnPartRuleSetNameRulelist")]
        public Task<IActionResult> MntnPartRuleSetNameRulelist(string? ruleSetName)
        {
            return Task.FromResult<IActionResult>(Ok(service.MntnPartRuleSetNameRulelist(ruleSetName, HttpContext.GetSite())));
        }

        /// <summary>
        /// 料号工艺流程维护-->包规名称 pkspecName非必须
        /// </summary>
        [HttpGet("MntnPartPkspecNamelist")]
        public Task<IActionResult> MntnPartPkspecNamelist(string? pkspecName)
        {
            return Task.FromResult<IActionResult>(Ok(service.MntnPartPkspecNamelist(pkspecName, HttpContext.GetSite())));
        }

        /// <summary>
        /// 料号工艺流程维护-->新增()
        /// </summary>
        [HttpPost("MntnPartRouteInsert")]
        public Task<IActionResult> MntnPartRouteInsert1(ImesMpartRoute imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.createEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.MntnPartRouteInsert(imes)));
        }


    }
}
