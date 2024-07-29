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
    [Route("ProdMnt/ProdSamplingRule")]
    [ApiController]
    public class ProdSamplingRuleController : BaseController
    {
        readonly IProdSamplingRuleService service;
        public ProdSamplingRuleController(IProdSamplingRuleService service)
        {
            this.service = service;
        }
        [HttpGet("RuleShowExport")]
        [Log(Title = "抽验规则导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.SamplingRuleList(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());
            var result = ExportExcelMini(list.Result, "ImesMqcSamplingPlan", "抽验规则");
            return ExportExcel(result.Item2, result.Item1);
        }
        [HttpGet("SamplingRuleList")]
        public Task<IActionResult> SamplinRuleList(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            string site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.SamplingRuleList(enaBled, optionData, textData, pageNum, pageSize, site)));
        }
        [HttpGet("RuleShowDetaitExport")]
        [Log(Title = "抽验规则明细导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowDetaitExport(int samplingRuleid,int pageSize)
        {
            var list = service.SamplingDetailList(samplingRuleid, 1, pageSize, HttpContext.GetSite());
            var result = ExportExcelMini(list.Result, "ImesMqcSamplingPlanDefault", "抽验规则明细");
            return ExportExcel(result.Item2, result.Item1);
        }
        [HttpGet("SamplingDetaitList")]
        public Task<IActionResult> SamplingDetailList(int samplingRuleid, int pageNum, int pageSize)
        {
            string site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.SamplingDetailList(samplingRuleid,pageNum, pageSize,HttpContext.GetSite())));
        }

        [HttpPut("SamplingRuleInsert")]
        public Task<IActionResult> InsertSamplingRule(IMesMqcSamplingRule iMesMqcSamplingRule)
        {
            var site = HttpContext.GetSite();
            var name = HttpContext.GetName();
            return Task.FromResult<IActionResult>(Ok(service.InsertSamplingRule(iMesMqcSamplingRule, site,name)));
        }
        [HttpPut("SamplingRuleUpdate")]
        public Task<IActionResult> UpdateSamplingRule(IMesMqcSamplingRule iMesMqcSamplingRule)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.UpdateSamplingRule(iMesMqcSamplingRule, site)));
        }
        [HttpPut("SamplingRuleabled")]
        public Task<IActionResult> SamplingRuleabled(IMesMqcSamplingRule iMesMqcSamplingRule)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.SamplingRuleabled(iMesMqcSamplingRule, HttpContext.GetSite())));
        }
        [HttpGet("SamplingRulePreset")]
        public Task<IActionResult> SamplingRulePreset(string? samplingRuleid)
        {
            return Task.FromResult<IActionResult>(Ok(service.SamplingRulePreset(samplingRuleid, HttpContext.GetUId(), HttpContext.GetSite())));
        }
        [HttpPut("SamplingRuleDelete")]
        public Task<IActionResult> DeleteSamplingRule(IMesMqcSamplingRule iMesMqcSamplingRule)
        {
            iMesMqcSamplingRule.site = HttpContext.GetSite();   
            return Task.FromResult<IActionResult>(Ok(service.DeleteSamplingRule(iMesMqcSamplingRule)));
        }
        [HttpPut("RuleDefaultInsert")]
        public Task<IActionResult> InsertSamplingRuleDetait(IMesMqcSamplingRuleDetail iMesMqcSamplingRuleDetait)
        {
            var site = HttpContext.GetSite();
            var name = HttpContext.GetName();
            return Task.FromResult<IActionResult>(Ok(service.InsertSamplingRuleDetait(iMesMqcSamplingRuleDetait, site, name)));
        }
        [HttpPut("RuleDefaultUpdate")]
        public Task<IActionResult> UpdateSamplingRuleDetait(IMesMqcSamplingRuleDetail iMesMqcSamplingRuleDetait)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.UpdateSamplingRuleDetait(iMesMqcSamplingRuleDetait, site)));
        }
        [HttpDelete("RuleDefaultDelete")]
        public Task<IActionResult> DeleteSamplingDefault(IMesMqcSamplingRuleDetail iMesMqcSamplingRuleDetail)
        {
            iMesMqcSamplingRuleDetail.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.DeleteSamplingDefault(iMesMqcSamplingRuleDetail)));
        }
    }
}
