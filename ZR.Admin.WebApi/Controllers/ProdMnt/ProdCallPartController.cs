using Microsoft.AspNetCore.Mvc;
using ZR.Model.Dto.ProdDto;
using ZR.Model;
using ZR.Service.IService;
using System.Drawing.Printing;
using MiniExcelLibs;
using Microsoft.AspNetCore.Http;

namespace ZR.Admin.WebApi.Controllers.ProdMnt
{
    [Route("ProdMnt/CallPart")]
    [ApiController]
    public class ProdCallPartController : BaseController
    {
        readonly IProdCallPartService service;
        public ProdCallPartController(IProdCallPartService service)
        {
            this.service = service;
        }
        [HttpGet("CallPartList")]
        public Task<IActionResult> CallPartList(string? enabled, string? optionData, string? textData,int pageNum, int pageSize)
        {
            string site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.CallPartList(enabled, optionData, textData, pageNum, pageSize, site)));
        }
        [HttpGet("ShowExport")]
        [Log(Title = "锡膏胶水维护数据导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult CallPartList(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.CallPartList(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());
            var result = ExportExcelMini(list.Result, "imesMCallPart", "锡膏胶水维护数据导出");
            return ExportExcel(result.Item2, result.Item1);
        }
        [HttpGet("History")]
        public Task<IActionResult> History(string id)
        {
            return Task.FromResult<IActionResult>(Ok(service.History(id,HttpContext.GetSite())));
        }
        [HttpGet("LineList")]
        public Task<IActionResult> LineList()
        {
            return Task.FromResult<IActionResult>(Ok(service.LineList( HttpContext.GetSite())));
        }
        [HttpGet("IpnList")]
        public Task<IActionResult> Ipnlist(string? type, string? enabled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.Ipnlist(type,enabled, optionData, textData, pageNum, pageSize)));
        }
        [HttpPost("CallPartImport")]
        [Log(Title = "锡膏胶水维护导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        public IActionResult CallPartImport([FromForm(Name = "file")] IFormFile formFile)
        {
            List<ImesMCallPart> imes = new();
            using (var stream = formFile.OpenReadStream())
            {
                imes = stream.Query<ImesMCallPart>(startCell: "A1").ToList();
            }
            return SUCCESS(service.CallPartImport(imes, HttpContext.GetSite(), HttpContext.GetName()));
        }
        [HttpGet("WorkOrder")]
        public Task<IActionResult> WorkOrder(string? optionData, string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.WorkOrder(optionData, textData, pageNum, pageSize,HttpContext.GetSite())));
        }
        [HttpPut("CallPartabled")]
        public Task<IActionResult> CallPartabled(ImesMCallPart imesMCallPart)
        {
            return Task.FromResult<IActionResult>(Ok(service.CallPartabled(imesMCallPart, HttpContext.GetSite())));
        }
        [HttpPut("InsertCallPart")]
        public Task<IActionResult> InsertCallPart(ImesMCallPart imesMCallPart)
        {
            imesMCallPart.updateEmp = HttpContext.GetName();
            imesMCallPart.createEmp = HttpContext.GetName();
            imesMCallPart.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.InsertCallPart(imesMCallPart)));
        }
        [HttpPut("UpdateCallPart")]
        public Task<IActionResult> UpdateCallPart(ImesMCallPart imesMCallPart)
        {
            return Task.FromResult<IActionResult>(Ok(service.UpdateCallPart(imesMCallPart, HttpContext.GetSite())));
        }
        [HttpPut("DeleteCallPart")]
        public Task<IActionResult> DeleteCallPart(ImesMCallPart imesMCallPart)
        {
            return Task.FromResult<IActionResult>(Ok(service.DeleteCallPart(imesMCallPart, HttpContext.GetSite())));
        }
    }
}
