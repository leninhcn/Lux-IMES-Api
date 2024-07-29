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
using MapsterMapper;
using ZR.Model.Dto;
using static ZR.Model.Dto.ProdDto.ImesMmodel;
using ZR.Model;
using SqlSugar;
using JinianNet.JNTemplate;
using ZR.Model.System;
using System.Collections.Generic;
using ZR.Model.System.Dto;
using System.Drawing.Printing;
using MiniExcelLibs;

namespace ZR.Admin.WebApi.Controllers.ProdMnt
{
    /// <summary>
    /// 基础信息 -> 产品信息
    /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("prodMnt/prodModel/")]
    [ApiController]
    public class ProdModelController : BaseController
    {
        readonly IProdModelService service;

        public ProdModelController(IProdModelService service) 
        {
            this.service = service;
        }

        /*/// <summary>
        /// 基础信息 -> 产品信息 -> 机种维护查询
        /// </summary>
        [HttpGet("PordModellist")]
        public Task<IActionResult> PordModellist(string? enaBled, string? optionData, string? textData)
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.getModelData(enaBled,optionData,textData).ToList()));
        }*/

        /// <summary>
        /// 基础信息 -> 产品信息 -> 机种维护查询
        /// </summary>
        [HttpGet("PordModellist")]
        public IActionResult PordModellist(string? enaBled, string? optionData, string? textData,int pageNum,int pageSize)
        {
            return SUCCESS(service.getModelData(enaBled, optionData, textData, pageNum, pageSize, HttpContext.GetSite()));
        }

        /// <summary>
        /// 机种导出
        /// </summary>
        /// <param name="enaBled"></param>
        /// <param name="optionData"></param>
        /// <param name="textData"></param>
        /// <param name="pageSize"></param>
        /// <param name="user"></param>
        [HttpGet("ModelExport")]
        [Log(Title = "机种导出", BusinessType = BusinessType.EXPORT)]
        //[ActionPermissionFilter(Permission = "system:user:export")]
        public IActionResult UserExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.getModelData(enaBled, optionData, textData, 1 ,pageSize, HttpContext.GetSite());

            var result = ExportExcelMini(list.Result, "Model", "机种数据");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 基础信息 -> 产品信息 -> 机种维护新增
        /// </summary>
        [HttpPost("PordModelInsert")]
        public Task<IActionResult> PordModelInsert(ImesMmodel mmodel)
        {
            var name = HttpContext.GetName();
            mmodel.UpdateEmpno = name;
            mmodel.CreateEmpno = name;
            mmodel.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.getModelInsert(mmodel)));
        }

        /// <summary>
        /// 基础信息 -> 产品信息 -> 机种维护修改
        /// </summary>
        [HttpPut("PordModelUpdate")]
        public Task<IActionResult> PordModelUpdate( ImesMmodel mmodel)
        {
            var name = HttpContext.GetName();
            mmodel.UpdateEmpno = name;
            mmodel.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.getModelUpdate(mmodel)));
        }

        /// <summary>
        /// 基础信息 -> 产品信息 -> 机种删除
        /// </summary>
        [HttpDelete("PordModelDelete")]
        public Task<IActionResult> PordModelDelete( ImesMmodel mmodel)
        {
            mmodel.site = HttpContext.GetSite();
            mmodel.UpdateEmpno= HttpContext.GetName() ;
            return Task.FromResult<IActionResult>(Ok(service.PordModelDelete(mmodel)));
        }

        /// <summary>
        /// 基础信息 -> 产品信息 -> 机种备份查询
        /// </summary>
        [HttpGet("SAJETMmodelHtlist")]
        public Task<IActionResult> ImesMmodelHtlist(int id, string model)
        {
            return Task.FromResult<IActionResult>(Ok(service.ImesMmodelHtlist(id,  model, HttpContext.GetSite())));
        }



        //---------------------------分割线-----------------------------------------------------------------------------
        

        /// <summary>
        /// 基础信息 -> 产品信息 -> 料号查询
        /// </summary>
        [HttpGet("PordPartList")]
        public IActionResult PordPartList(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            return SUCCESS(service.PordPartList(enaBled, optionData, textData, pageNum, pageSize, HttpContext.GetSite()));
        }


        /// <summary>
        /// 料号查询导出
        /// </summary>
        /// <param name="enaBled"></param>
        /// <param name="optionData"></param>
        /// <param name="textData"></param>
        /// <param name="pageSize"></param>
        /// <param name="user"></param>
        [HttpGet("PordPartExport")]
        [Log(Title = "料号查询导出", BusinessType = BusinessType.EXPORT)]
        //[ActionPermissionFilter(Permission = "system:user:export")]
        public IActionResult PordPartExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.PordPartList(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());
            var result = ExportExcelMini(list.Result, "Model", "料号查询导出");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 料号导入
        /// </summary>
        [HttpPost("PordPartImportData")]
        [Log(Title = "料号导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        //[ActionPermissionFilter(Permission = "system:user:import")]
        public IActionResult PordPartImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List< MPartHtData> mPartHts = new();
            using (var stream = formFile.OpenReadStream())
            {
                mPartHts = stream.Query<MPartHtData>(startCell: "A1").ToList();
            }
            return SUCCESS(service.PordPartImportData(mPartHts, HttpContext.GetSite(), HttpContext.GetName()));
        }

        /// <summary>
        /// 基础信息 -> 产品信息 -> 料号抽验计划查询
        /// </summary>
        [HttpGet("PlanList")]
        public Task<IActionResult> PlanList()
        {
            var site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.PlanList(site)));
        }

        /// <summary>
        /// 基础信息 -> 产品信息 -> 料号新增---
        /// </summary>
        [HttpPost("PordPartInsert")]
        public IActionResult PordPartInsert([FromBody] MPartHtData? mPartHt)
        {
            mPartHt.site = HttpContext.GetSite();
            mPartHt.createEmpno = HttpContext.GetName();
            mPartHt.updateEmpno = HttpContext.GetName();
            return SUCCESS(service.PordPartInsert(mPartHt));
        }

        /// <summary>
        /// 基础信息 -> 产品信息 -> 料号修改---id和ipn都需要传
        /// </summary>
        [HttpPut("PordPartUpdate")]
        public IActionResult PordPartUpdate(MPartHtData mPartHt)
        {
            mPartHt.site = HttpContext.GetSite();
            mPartHt.createEmpno = HttpContext.GetName();
            mPartHt.updateEmpno = HttpContext.GetName();
            return SUCCESS(service.PordPartUpdate(mPartHt));
           // return Task.FromResult<IActionResult>(Ok(service.PordPartUpdate(mPartHt)));
        }

        /// <summary>
        /// 基础信息 -> 产品信息 -> 料号删除
        /// </summary>
        [HttpDelete("PordPartDelete")]
        public Task<IActionResult> PordPartDelete(MPartHtData mPartH)
        {
            return Task.FromResult<IActionResult>(Ok(service.PordPartDelete(mPartH.id, HttpContext.GetSite(),HttpContext.GetName())));
        }

        /// <summary>
        /// 基础信息 -> 产品信息 -> 料号备份查询
        /// </summary>
        [HttpGet("PordPartHtlist")]
        public Task<IActionResult> PordPartHtlist(int id)
        {
            return Task.FromResult<IActionResult>(Ok(service.PordPartHtlist(id, HttpContext.GetSite())));
        }


    }

    

}
