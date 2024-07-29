using Aliyun.OSS;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;
using System.Xml;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Service.IService;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MapsterMapper;
using JinianNet.JNTemplate;
using System.Security.Policy;
using System.Data.SqlTypes;
using System.Diagnostics;
using ZR.Model;
namespace ZR.Admin.WebApi.Controllers.ProdMnt
{
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("ProdMnt/ProdTestitem")]
    [ApiController]
    public class ProdTestitemController : BaseController
    {
        readonly IPordTestitemService service;
        public ProdTestitemController(IPordTestitemService service)
        {
            this.service = service;
        }
        [HttpGet("ShowExport")]
        [Log(Title = "测试大项导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.TestList(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());
            var result = ExportExcelMini(list.Result, "ImesMTestItemType", "测试大项");
            return ExportExcel(result.Item2, result.Item1);
        }
        [HttpGet("Testlist")]
        public Task<IActionResult> TestList(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            string site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.TestList(enaBled, optionData, textData, pageNum, pageSize, site)));
        }
        [HttpPut("TestInsert")]
        public Task<IActionResult> InsertTestItemType(ImesMTestItemType imesMTestItemType)
        {
            var site = HttpContext.GetSite();
            var name = HttpContext.GetName();
            return Task.FromResult<IActionResult>(Ok(service.InsertTestItemType(imesMTestItemType, site, name)));
        }
        [HttpPut("TestUpdate")]
        public Task<IActionResult> UpdateTestItemType(ImesMTestItemType imesMTestItemType)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.UpdateTestItemType(imesMTestItemType, site)));
        }
        [HttpPut("Testabled")]
        public Task<IActionResult> TestItemTypeabled(ImesMTestItemType imesMTestItemType)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.TestItemTypeabled(imesMTestItemType, site)));
        }
        [HttpPut("TestDelete")]
        public Task<IActionResult> DeleteTestItemType(ImesMTestItemType imesMTestItemType)
        {
            imesMTestItemType.site = HttpContext.GetSite();
            imesMTestItemType.updateEmp = HttpContext.GetName();
            return Task.FromResult<IActionResult>(Ok(service.DeleteTestItemType(imesMTestItemType)));
        }
        [HttpGet("TestitemList")]
        public Task<IActionResult> TestitemList(string? enaBled, string? itemTypeid, string? optionData, string? textData, int pageNum, int pageSize)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.TestitemList(enaBled, itemTypeid,optionData, textData, pageNum, pageSize, site)));
        }
        [HttpGet("ShowItemExport")]
        [Log(Title = "测试小项导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowItemExport(string? enaBled, string? itemTypeid, string? optionData, string? textData, int pageSize)
        {
            var list = service.TestitemList(enaBled, itemTypeid, optionData, textData, 1, pageSize, HttpContext.GetSite());
            var result = ExportExcelMini(list.Result, "ImesMTestItem", "测试小项");
            return ExportExcel(result.Item2, result.Item1);
        }
        [HttpPut("ItemInsert")]
        public Task<IActionResult> InsertTestItem(ImesMTestItem imesMTestItem)
        {
            var site = HttpContext.GetSite();
            var name = HttpContext.GetName();
            return Task.FromResult<IActionResult>(Ok(service.InsertTestItem(imesMTestItem, site, name)));
        }
        [HttpPut("Itemabled")]
        public Task<IActionResult> TestItemabled(ImesMTestItem imesMTestItem)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.TestItemabled(imesMTestItem, site)));
        }
        [HttpPut("ItemUpdate")]
        public Task<IActionResult> UpdateTestItem(ImesMTestItem imesMTestItem)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.UpdateTestItem(imesMTestItem, site)));
        }
        [HttpPut("ItemDelete")]
        public Task<IActionResult> DeleteTestItem(ImesMTestItem imesMTestItem)
        {
            imesMTestItem.site = HttpContext.GetSite();
            imesMTestItem.updateEmp = HttpContext.GetName();
            return Task.FromResult<IActionResult>(Ok(service.DeleteTestItem(imesMTestItem)));
        }
    }
}
