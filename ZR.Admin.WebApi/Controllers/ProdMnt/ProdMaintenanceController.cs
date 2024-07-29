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
    [Route("ProdMnt/ProdMaintenance")]
    [ApiController]
    public class ProdMaintenanceController : BaseController
    {
        readonly IProdMaintenanceService service;
        public ProdMaintenanceController(IProdMaintenanceService service)
        {
            this.service = service;
        }

        //public Task<IActionResult> 
        [HttpGet("ShowData")]
        public Task<IActionResult> ShowData(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            string site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.ShowData(enaBled, optionData, textData, pageNum, pageSize, site)));
        }

        /// <summary>
        /// 碑文数据导出
        /// </summary>
        /// <param name="enaBled"></param>
        /// <param name="optionData"></param>
        /// <param name="textData"></param>
        /// <param name="pageSize"></param>
        /// <param name="user"></param>
        [HttpGet("ShowExport")]
        [Log(Title = "碑文数据导出", BusinessType = BusinessType.EXPORT)]
        public IActionResult ShowExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.ShowData(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());
            var result = ExportExcelMini(list.Result, "ImesMpartInscription", "碑文数据导出");
            return ExportExcel(result.Item2, result.Item1);
        }
        [HttpGet("MaintenanceList")]
        public Task<IActionResult>MaintenanceList(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            string site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.MaintenanceList(enaBled, optionData, textData, pageNum, pageSize, site)));
        }
        [HttpGet("Maintenancepart")]
        public Task<IActionResult> Maintenancepart(string?ipn)
        {
            return Task.FromResult<IActionResult>(Ok(service.Maintenancepart(ipn, HttpContext.GetSite())));
        }
        /// <summary>
        /// REELNO带出料号
        /// </summary>
        [HttpGet("Verification")]
        public Task<IActionResult> Verification(string? reelNo)
        {
            return Task.FromResult<IActionResult>(Ok(service.Verification( reelNo, HttpContext.GetSite())));
        }
        [HttpGet("Validate")]
        public Task<IActionResult> Validate( string ipn, string reelNo, string? inscription)
        {
            Console.WriteLine(ipn,reelNo,inscription);
            return Task.FromResult<IActionResult>(Ok(service.Validate(ipn, reelNo, inscription, HttpContext.GetSite(), HttpContext.GetUId())));
        }
        /// <summary>
        /// 碑文数据新增
        /// </summary>
        [HttpPost("MaintenanceInsert")]
        public Task<IActionResult> MaintenanceInsert(ImesMpartInscription ImesMpartInscription)
        {
            ImesMpartInscription.site = HttpContext.GetSite();
            ImesMpartInscription.createEmpno = HttpContext.GetName();
            ImesMpartInscription.updateEmpno = HttpContext.GetName();
            return Task.FromResult<IActionResult>(Ok(service.MaintenanceInsert(ImesMpartInscription)));
        }
        /// <summary>
        /// 碑文数据修改
        /// </summary>
        [HttpPut("MaintenanceUpdate")]
        public Task<IActionResult> MaintenanceUpdate(ImesMpartInscription ImesMpartInscription)
        {

            ImesMpartInscription.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.MaintenanceUpdate(ImesMpartInscription)));

        }
        /// <summary>
        /// 删除数据
        /// </summary>
        [HttpPut("MaintenanceDelete")]
        public  Task<IActionResult> MaintenanceDelete(ImesMpartInscription ImesMpartInscription)
        {
            string site = "DEF";
            return Task.FromResult<IActionResult>(Ok(service.MaintenanceDelete(ImesMpartInscription)));
        }
        /// <summary>
        /// 历史记录获取
        /// </summary>
        [HttpGet("MaintenanceHistory")]
        public async Task<IActionResult> History(string? id)
        {
            string site = "DEF";
            return Ok(service.History(id, site));
        }

    }
}
