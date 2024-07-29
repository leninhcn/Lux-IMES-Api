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
namespace ZR.Admin.WebApi.Controllers.ProdMnt
{
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("ProdMnt/ProdSamplingPlan")]
    [ApiController]
    public class ProdSamplingPlanController : BaseController
    {
        readonly IProdSamplingPlanService service;
        public ProdSamplingPlanController(IProdSamplingPlanService service)
        {
            this.service = service;
        }
        [HttpGet("ShowExport")]
        [Log(Title = "抽验计划导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.SamplingList(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());
            var result = ExportExcelMini(list.Result, "ImesMqcSamplingPlan", "抽验计划");
            return ExportExcel(result.Item2, result.Item1);
        }
        [HttpGet("SamplingList")]
        public Task<IActionResult> SamplingList(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            string site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.SamplingList(enaBled, optionData, textData, pageNum, pageSize, site)));
        }
        [HttpGet("ShowDetaitExport")]
        [Log(Title = "抽验计划规则导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowDetaitExport(string? level, string samplingId, string? optionData, string? textData, int pageSize)
        {
            var list = service.SamplingDetaitList(level, samplingId, optionData, textData, 1, pageSize, HttpContext.GetSite());
            var result = ExportExcelMini(list.Result, "ImesMqcSamplingPlanDefault", "抽验计划规则");
            return ExportExcel(result.Item2, result.Item1);
        }
        [HttpGet("SamplingDetaitList")]
        public Task<IActionResult> SamplingDetaitList(string? level, string samplingId, string? optionData, string? textData, int pageNum, int pageSize)
        {
            string site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.SamplingDetaitList(level, samplingId, optionData, textData, pageNum, pageSize, site)));
        }
        //[HttpGet("GetSamplingInfo")]
        //public async Task<IActionResult> GetSamplingInfo(string? samplingType, string? samplingDesc)
        //{
        //    string site = HttpContext.GetSite();
        //    return Ok(service.GetSamplingInfo(samplingType, samplingDesc, site));
        //}
        [HttpPut("SamplingInsert")]
        public Task<IActionResult> InsertSamplingPlan(ImesMqcSamplingPlan imesMqcSamplingPlan)
        {
            var site = HttpContext.GetSite();
            var name = HttpContext.GetName();
            return Task.FromResult<IActionResult>(Ok(service.InsertSamplingPlan(imesMqcSamplingPlan,site,name)));
        }
        [HttpPut("SamplingUpdate")]
        public Task<IActionResult> UpdateSamplingPlan(ImesMqcSamplingPlan imesMqcSamplingPlan)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.UpdateSamplingPlan(imesMqcSamplingPlan, site)));
        }
        [HttpPut("Samplingabled")]
        public Task<IActionResult> SamplingPlanabled(ImesMqcSamplingPlan imesMqcSamplingPlan)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.SamplingPlanabled(imesMqcSamplingPlan, site)));
        }
        [HttpPut("SamplingDelete")]
        public Task<IActionResult> DeleteSamplingPlan(ImesMqcSamplingPlan imesMqcSamplingPlan)
        {
            imesMqcSamplingPlan.site = HttpContext.GetSite();
            imesMqcSamplingPlan.updateEmp = HttpContext.GetName();
            
            return Task.FromResult<IActionResult>(Ok(service.DeleteSamplingPlan(imesMqcSamplingPlan)));
        }
        [HttpPut("SamplingDefaultInsert")]
        public Task<IActionResult> InsertSamplingDefault(ImesMqcSamplingPlanDefault imesMqcSamplingPlanDefault)
        {
            var site = HttpContext.GetSite();
            var name = HttpContext.GetName();
            return Task.FromResult<IActionResult>(Ok(service.InsertSamplingDefault(imesMqcSamplingPlanDefault, site, name)));
        }
        [HttpPut("SamplingDefaultUpdate")]
        public Task<IActionResult> UpdateSamplingDefault(ImesMqcSamplingPlanDefault imesMqcSamplingPlanDefault)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.UpdateSamplingDefault(imesMqcSamplingPlanDefault, site)));
        }
        [HttpPut("SamplingDefaultDelete")]
        public Task<IActionResult> DeleteSamplingDefault(ImesMqcSamplingPlanDefault imesMqcSamplingPlanDefault)
        {
            imesMqcSamplingPlanDefault.site = HttpContext.GetSite();
            imesMqcSamplingPlanDefault.updateEmp = HttpContext.GetName();
            return Task.FromResult<IActionResult>(Ok(service.DeleteSamplingDefault(imesMqcSamplingPlanDefault)));
        }
    }
}
