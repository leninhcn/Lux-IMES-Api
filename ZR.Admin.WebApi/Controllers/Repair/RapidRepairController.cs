using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Text.RegularExpressions;
using ZR.Infrastructure.Model;
using ZR.Model.Dto;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Admin.WebApi.Controllers.Repair
{
    /// <summary>
    /// 批量维修
    /// </summary>
    [Route("repair/rapidrepair")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class RapidRepairController : BaseController
    {
        public IRapidRepairService repairService;
        public IRepairDelinkService delinkService;
        public IRepairExeService exeService;
        public RapidRepairController(IRapidRepairService _repairService, IRepairDelinkService _delinkService, IRepairExeService _exeService)
        {
            repairService = _repairService;
            delinkService = _delinkService;
            exeService = _exeService;
        }

        /// <summary>
        /// 批量维修
        /// </summary>
        /// <param name="strSN">sn</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:rapidrepair:getRapidSNInfo")]
        [HttpGet("getRapidSNInfo")]
        public async Task<IActionResult> getRapidSNInfo(string strSN)
        {
            if (string.IsNullOrEmpty(strSN))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "请输入SN信息");
            }

            string snInfo = Regex.Replace(strSN, @"^\s*$(\n|\r|\r\n)", "", RegexOptions.Multiline);//去除文本里的空行
            snInfo = snInfo.TrimEnd('\n');
            
            var arrSn = snInfo.Split("\n");
            int lines = arrSn.Length;

            ExecuteResult exeRes = new ExecuteResult();
            RapidRepair rapidRepair = new RapidRepair();
            for (int i = 0; i < arrSn.Length; i++)
            {
                string valueSn = arrSn[i];

                exeRes =await delinkService.CheckSN(valueSn);
                if (!exeRes.Status)
                {
                    return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message + ":" + valueSn);
                }
                else
                {
                    string reSN = exeRes.Anything.ToString();
               
                    DataTable white = await repairService.checkstatus(valueSn);
                    if (white.Rows.Count == 0)
                    {
                        return ToResponse(ResultCode.PARAM_ERROR, valueSn + " 状态正常");
                    }
                    DataTable dtTemp = await repairService.getDetail(valueSn);
                    if (dtTemp.Rows.Count > 0)
                    {
                        rapidRepair.sn = valueSn;
                        rapidRepair.wo =  dtTemp.Rows[0]["WORK_ORDER"].ToString();
                        rapidRepair.version = dtTemp.Rows[0]["VERSION"].ToString();
                        rapidRepair.parnNo = dtTemp.Rows[0]["IPN"].ToString();
                        rapidRepair.line = dtTemp.Rows[0]["LINE"].ToString();
                        rapidRepair.route = dtTemp.Rows[0]["ROUTE_NAME"].ToString();
                        rapidRepair.stationType = dtTemp.Rows[0]["STATION_TYPE"].ToString();
                        rapidRepair.stationName = dtTemp.Rows[0]["STATION_NAME"].ToString();
                        rapidRepair.wipStationType = dtTemp.Rows[0]["WIP_STATION_TYPE"].ToString();
                    }
                    else
                    {
                        return ToResponse(ResultCode.PARAM_ERROR, valueSn + " 状态正常");
                    }

                    DataTable dt =await repairService.getDefect1(valueSn);
                    if (dt.Rows.Count > 0)
                    {
                        rapidRepair.defectCode = dt.Rows[0]["DEFECT_CODE"].ToString();
                        rapidRepair.defectCode1 = dt.Rows[0]["DEFECT_CODE1"].ToString();
                        rapidRepair.location = dt.Rows[0]["LOCATION"].ToString();
                        rapidRepair.repairTime = DateTime.Now.ToString();
                    }
                    else
                    {
                        return ToResponse(ResultCode.PARAM_ERROR, valueSn + " 已维修完成");
                    }

                    //  Show_SNData1();
                }
              
            }
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", rapidRepair));
        }

        /// <summary>
        /// 不良代码Check
        /// </summary>
        /// <param name="defect"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:rapidrepair:checkDefect")]
        [HttpGet("checkDefect")]
        public async Task<IActionResult> checkDefect(string defect)
        {
            if (string.IsNullOrEmpty(defect))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "不良代码不能为空,请选择不良代码!!!");
            }

            ExecuteResult exeRes =await repairService.CheckIsErrorCode(defect);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
            else
            {
                return ToResponse(new ApiResult((int)ResultCode.SUCCESS, exeRes.Message, ""));
            }
        }

        /// <summary>
        /// 维修接口  （单sn逻辑，需要前端处理批量逻辑）
        /// </summary>
        /// <param name="rapidRequst"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:rapidrepair:repairFinish")]
        [HttpPost("repairFinish")]
        public async Task<IActionResult> repairFinish([FromBody]RapidRequst rapidRequst)
        {
            //string[] lines = sSN.Split('\n');
            //string snn1 = sSN.ToString().Trim();
            //string[] slines = snn1.Split('\n');
         
            if (string.IsNullOrEmpty(rapidRequst.sn))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "请输入批量序列号");
            }
            if (string.IsNullOrEmpty(rapidRequst.memo))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "请输入备注信息");
            }
            if (string.IsNullOrEmpty(rapidRequst.nStation))
            {
                return ToResponse(ResultCode.PARAM_ERROR, rapidRequst.sn + " 维修站没有设置返回站点");
            }
            var userNo = HttpContext.GetName();
            ExecuteResult exeRes = await repairService.RapidRepairGo(rapidRequst.stationName, rapidRequst.sn, rapidRequst.nStation, userNo);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
            else
            {
                await repairService.AddRapidRepair(rapidRequst,userNo);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, rapidRequst.sn + ":批量维修完成", ""));
        }

        /// <summary>
        /// 报废   （单sn逻辑，需要前端处理批量逻辑）
        /// </summary>
        /// <param name="scrapDto"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:rapidrepair:Scrap")]
        [HttpPost("Scrap")]
        public async Task<IActionResult> Scrap([FromBody] ScrapDto scrapDto)
        {
            string userNo = HttpContext.GetName();

            await exeService.AddScrapInfo(scrapDto, userNo);
            await exeService.UpdateSnStatus(scrapDto.sn, userNo);
            await exeService.AddSnTravel(scrapDto.sn);

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "序列号报废成功", ""));
        }
    }
}
