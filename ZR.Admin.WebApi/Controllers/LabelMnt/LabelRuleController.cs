using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using ZR.Admin.WebApi.Filters;
using ZR.Service.IService;
using ZR.Model.Dto;

namespace ZR.Admin.WebApi.Controllers.Label
{

    [Route("labelMnt/labelRule/[action]")]
    [ApiController]
    public class LabelRuleController : BaseController
    {
        readonly ILabelRuleService labelRuleService;

        public LabelRuleController(ILabelRuleService labelRuleService)
        {
            this.labelRuleService = labelRuleService;
        }

        [HttpGet]
        public async Task<IActionResult> ListRuleNames([FromQuery] LabelRuleDto.Query query)
        {
            //rule_name,rule_type,rule_desc,group_qty,safety_stock,update_empno,update_time, id, enabled
            var list = await labelRuleService.GetAllRuleName(query.Status, query.RuleField, query.FilterText);
            return Ok(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetRuleParamByName(string ruleName)
        {
            var list = await labelRuleService.GetRuleParamByName(ruleName);
            return Ok(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetFunName(string sFix)
        {
            var List = await labelRuleService.GetFunName(sFix);
            return Ok(List);
        }

        [HttpGet]
        public async Task<IActionResult> GetSeqName(string ruleName)
        {
            var seqName = await labelRuleService.GetSeqName(ruleName);
            return Ok(new { seqName });
        }

        [HttpGet]
        public async Task<IActionResult> GetSeq(string seqName, string ruleName)
        {
            var val = await labelRuleService.GetSeq(seqName, ruleName);
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(val));
        }
    }
}
