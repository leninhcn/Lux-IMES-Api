//using Microsoft.AspNetCore.Mvc;
//using System.Data;
//using ZR.Infrastructure.Model;
//using ZR.Model.Business;
//using ZR.Service.Repair.IRepairService;
//using ZR.ServiceCore.Model.Dto;

using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO.Pipelines;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using ZR.Infrastructure.Model;
using ZR.Model.Dto;
using ZR.Model.Quality;
using ZR.Model.Repair.Dto;
using ZR.Service.Repair.IRepairService;
using static NLog.LayoutRenderers.Wrappers.ReplaceLayoutRendererWrapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZR.Admin.WebApi.Controllers.Repair
{
    /// <summary>
    /// 维修打印
    /// </summary>
    [Route("repair/printlabel")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class RepPrintlabController : BaseController
    {
        public IRepPrintlabService repairService;
        public RepPrintlabController(IRepPrintlabService _repairService)
        {
            repairService = _repairService;
        }

        /// <summary>
        /// checkPrintSN 
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:printlabel:checkPrintSN")]
        [HttpGet("checkPrintSN")]
        public async Task<IActionResult> checkPrintSN(string sn)
        {
            //配合 chekprintrole 接口使用，checkPrintSN 为true，在继续使用chekprintrole 验证
            ExecuteResult exeRes = await repairService.checksn(sn);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR,sn + "SN 错误,请检查  " + exeRes.Message);
            }
            string labeltype = "5";
            exeRes = await repairService.checkprintlog(sn, labeltype);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, sn + "SN 错误,请检查  " + exeRes.Message);
            }
           
            //  检查流程等基本信息
            exeRes = await repairService.checkrepairin(sn);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// chekprintrole  
        /// 检查打印角色 密码验证
        /// 配合checkPrintSN 接口使用，checkPrintSN 为true，在继续使用此接口
        /// </summary>
        /// <param name="empno"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:printlabel:chekprintrole")]
        [HttpGet("chekprintrole")]
        public async Task<IActionResult> chekprintrole(string empno,string pwd)
        {
            ExecuteResult exeRes = await repairService.chekprintrole(empno,pwd);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR,exeRes.Message);
            }
           
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (string)exeRes.Anything));
        }

        /// <summary>
        /// 获取打印Label
        /// </summary>
        /// <param name="SN"></param>
        /// <param name="pStationType">SR01</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:printlabel:GetLabelInfo")]
        [HttpGet("GetLabelInfo")]
        public async Task<IActionResult> GetLabelInfo(string SN, string pStationType)
        {
            ExecuteResult exeRes = await repairService.GetLabelInfo(SN, pStationType);
            if (exeRes.Status)
            {
                DataTable dtTemp = (DataTable)exeRes.Anything;
                if (dtTemp.Rows.Count == 0)
                {
                    exeRes = await repairService.GetLabelInfo(pStationType);
                    //if (exeRes.Status)
                    //{
                    //    dtTemp = (DataTable)exeRes.Anything;
                    //}
                }
            }

            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "获取Label信息失败：" + exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }
    }
}
