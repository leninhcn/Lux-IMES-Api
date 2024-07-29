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
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Quality;
using ZR.Model.Repair.Dto;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;
using static NLog.LayoutRenderers.Wrappers.ReplaceLayoutRendererWrapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZR.Admin.WebApi.Controllers.Repair
{
    /// <summary>
    /// 维修执行
    /// </summary>
    //[Verify]
    [Route("repair/repairexe")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class RepairExeController : BaseController
    {
        public IRepairExeService repairService;
        public RepairExeController(IRepairExeService _repairService)
        {
            repairService = _repairService;
        }

        /// <summary>
        /// 获取维修人员姓名
        /// </summary>
        /// <param name="empNo">工号</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:getEmpName")]
        [HttpGet("getEmpName")]
        public async Task<IActionResult> getEmpName(string empNo)
        {
            if (string.IsNullOrEmpty(empNo))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "维修人不能为空，请输入工号");
            }
            DataTable dtTemp = new DataTable();
            dtTemp = await repairService.getEmpName(empNo);

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", dtTemp));
        }

        /// <summary>
        /// get SN信息
        /// </summary>
        /// <param name="repExe">repExe</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:GetInfoBySn")]
        [HttpPost("GetInfoBySn")]
        public async Task<IActionResult> GetInfoBySn([FromBody]RepairInDto repExe)
        {
            string sn = repExe.sn;
            if (string.IsNullOrEmpty(sn))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "请扫入SN");
            }

            DataTable white =await repairService.checkstatus(sn);
            if (white.Rows.Count > 0)
            {
                string status = white.Rows[0]["CURRENT_STATUS"].ToString();

                if (status == "0")
                {
                    return ToResponse(ResultCode.PARAM_ERROR, sn + "状态正常，请确认!!");
                }
                if (status == "1")
                {
                    return ToResponse(ResultCode.PARAM_ERROR, sn + " 状态错误, 请先进行 CheckIn");
                }
                //if (status == "5")
                //{
                //    ToResponse(new ApiResult((int)ResultCode.SUCCESS, sn + " 状态错误, 请先进行 CheckIn", null));
                //}
            }
            else
            {
                return ToResponse(ResultCode.PARAM_ERROR, sn + "不存在");
            }
            var g_sRepairTime = DateTime.Now.ToString();

            ExecuteResult exeRet = await CheckSNNo(repExe);

            if (!exeRet.Status)
            {
                return ToResponse(new ApiResult((int)ResultCode.FAIL, exeRet.Message, exeRet));
            }

            repairExe reP = (repairExe) exeRet.Anything;

            DataTable dt = await repairService.getDefect(reP.ipn);
            if (dt.Rows.Count == 0)
            {
                return ToResponse(new ApiResult((int)ResultCode.FAIL, exeRet.Message, exeRet));
            }

            reP.lineName = dt.Rows[0]["LINE"].ToString();
            reP.stationType = dt.Rows[0]["STATION_TYPE"].ToString();
            reP.stationName = dt.Rows[0]["STATION_NAME"].ToString();
            reP.sRepairTime = g_sRepairTime;

            string S = "";
            string L = "";
            List<LVDefect> liLv = new List<LVDefect>();
            //显示 不良 ShowDefect();
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                string sDefectCode = dt.Rows[i]["DEFECT_CODE"].ToString();
                string sLocation = dt.Rows[i]["LOCATION"].ToString();
                if (S != sDefectCode || L != sLocation)
                {
                    LVDefect lvDef = new LVDefect();
                    lvDef.DefectCode = sDefectCode;
                    lvDef.DEFECT_DESC = dt.Rows[i]["DEFECT_DESC"].ToString();
                    lvDef.DEFECT_DESC2 = dt.Rows[i]["DEFECT_DESC2"].ToString();
                    lvDef.LOCATION = dt.Rows[i]["LOCATION"].ToString();
                    lvDef.RECID = dt.Rows[i]["RECID"].ToString();
                    lvDef.STATION_TYPE = dt.Rows[i]["STATION_TYPE"].ToString();

                    if (dt.Rows[i]["REASON_CODE"].ToString() == "N/A")
                    {
                        lvDef.ImageIndex = 1;
                    }
                    else
                    {
                        lvDef.ImageIndex = 0;
                    }

                    S = dt.Rows[i]["DEFECT_CODE"].ToString();
                    L = dt.Rows[i]["LOCATION"].ToString();
                    liLv.Add(lvDef);
                }
            }

            reP.lvdef = liLv;

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", reP));
            /***
            if (liLv.Count() > 0)
            {
                for (int i = 0; i <= liLv.Count() - 1; i++)
                {
                    if (LVDefect.Items[i].Checked == false)
                    {
                        string sdefectcode = LVDefect.Items[i].Text;
                        string sid = LVDefect.Items[i].SubItems[4].Text;
                        DataTable dtlocation = repairService.GetRepair1(g_sSN, sdefectcode, sid);

                        if (dtlocation.Rows.Count > 0)
                        {
                            string status = dtlocation.Rows[0]["ENABLED"].ToString();
                            string rpstatus = dtlocation.Rows[0]["RP_STATUS"].ToString();
                            if (status == "Y" && rpstatus == "0")
                            {
                                SuccessMSG(sdefectcode + "不良代码已维修完成，无需重复维修");
                                LVDefect.Items[i].Selected = false;
                                LVDefect.Items[i].BackColor = Color.Green;
                                LVDefect.Items[i].ForeColor = Color.Black;
                                ShowReason(LVDefect.Items[i].SubItems[4].Text);
                                btnRemove.Enabled = false;
                                btnRepair.Enabled = false;
                                butReplace.Enabled = false;
                                return;
                            }
                            else if (status == "Y" && rpstatus == "")
                            {
                                SuccessMSG(sdefectcode + "不良代码已维修，请确认!!");
                                LVDefect.Items[i].Selected = false;
                                LVDefect.Items[i].BackColor = Color.Green;
                                LVDefect.Items[i].ForeColor = Color.Black;
                                ShowReason(LVDefect.Items[i].SubItems[4].Text);
                                btnRemove.Enabled = false;
                                btnRepair.Enabled = false;
                                butReplace.Enabled = false;
                                //return;
                            }
                            else if (status == "Y" && rpstatus == "1")
                            {
                                SuccessMSG(sdefectcode + "不良代码已维修，请确认!!");
                                LVDefect.Items[i].BackColor = Color.Green;
                                LVDefect.Items[i].ForeColor = Color.Black;
                                ShowReason(LVDefect.Items[i].SubItems[4].Text);
                                btnRemove.Enabled = false;
                                btnRepair.Enabled = false;
                                butReplace.Enabled = false;
                            }
                            else
                            {
                                ErrorMSG(sdefectcode + "不良代码未维修,请进行维修执行");
                                LVDefect.Items[i].Selected = true;
                                LVDefect.Items[i].BackColor = Color.Red;
                                LVDefect.Items[i].ForeColor = Color.Black;
                                ShowReason(LVDefect.Items[i].SubItems[4].Text);
                                btnRemove.Enabled = true;
                                btnRepair.Enabled = true;
                                butReplace.Enabled = true;
                                butErrorProofMaterial.Enabled = true;

                            }
                        }
                        else
                        {
                            ErrorMSG(sdefectcode + "不良代码未维修,请进行维修执行");
                            LVDefect.Items[i].Selected = true;
                            btnRemove.Enabled = true;
                            btnRepair.Enabled = true;
                            butReplace.Enabled = true;
                            butErrorProofMaterial.Enabled = true;

                        }

                    }
                }
            }
            **/
        }

        private async Task<ExecuteResult> CheckSNNo(RepairInDto repExe)
        {
            ExecuteResult exeRes =await repairService.CheckSN(repExe.sn);
           
            repairExe repairExe = new repairExe();
            if (!exeRes.Status)
            {
                return exeRes;
            }
            else
            {
                repairExe.ipn = exeRes.Anything.ToString();
            }
            // check route
            exeRes =await repairService.CheckRoute(repExe.stationName, repairExe.ipn);
            if (!exeRes.Status)
            {
                return exeRes;
            }

            DataTable dtTemp =await repairService.getDetail(repairExe.ipn);
            repairExe.stationType = repExe.stationType;
            if (dtTemp.Rows.Count > 0)
            {
                repairExe.wo = dtTemp.Rows[0]["WORK_ORDER"].ToString();
                // version = dtTemp.Rows[0]["VERSION"].ToString();
                repairExe.partNo = dtTemp.Rows[0]["IPN"].ToString();
                // g_sPartNo = dtTemp.Rows[0]["IPN"].ToString();
                repairExe.sOutTime = dtTemp.Rows[0]["OUT_PROCESS_TIME"].ToString();
                repairExe.CurrentStationType = dtTemp.Rows[0]["STATION_TYPE"].ToString();
                repairExe.routName = dtTemp.Rows[0]["ROUTE_NAME"].ToString();
            }

            DataTable dt =await repairService.getStep(repairExe);

            if (dt.Rows.Count == 0)
            {
                exeRes.Status = false;
                exeRes.Message = "站点设置错误，请配置站点";
                return exeRes;
            }
            repairExe.sRouteStep = dt.Rows[0]["Step"].ToString();
            exeRes.Status = true;
            exeRes.Anything = repairExe;

            return exeRes;
        }

        /// <summary>
        /// 维修--check
        /// </summary>
        /// <param name="repExe">sn</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:CheckRepairOP")]
        [HttpPost("CheckRepairOP")]
        public async Task<IActionResult> CheckRepairOP([FromBody] repariDto repExe)
        {
            string sn = repExe.sn;
            if (string.IsNullOrEmpty(sn))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "请扫入SN");
            }

            string repaier = repExe.repairEmpNo.Trim();
            if (string.IsNullOrEmpty(repaier))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "请输入员工工号");
            }
            else
            {
                DataTable emp1 =await repairService.getEmpName(repaier);
                if (emp1.Rows.Count == 0)
                {
                    return ToResponse(ResultCode.PARAM_ERROR, "此工号" + repaier + "不存在，请确认并重新输入");
                }
            }

            if (repExe.lvdef.Count() == 0)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "请选择不良信息");
            }

            List<LVDefect> lVDefect = repExe.lvdef;

            responseLV resLvDef = new responseLV();
            List <failDetail> liFail = new List<failDetail>();
            List<string> liRecid = new List<string>();

            for (int i = 0; i < lVDefect.Count() -1; i++)
            {
                //检查SN多次同一个位置和代码                               
                string sdefectcode1 = lVDefect[i].DefectCode.ToString();
                string slocation1 = lVDefect[i].LOCATION.ToString();
                string sid1 = lVDefect[i].RECID.ToString();
                DataTable dtlocation2 =await repairService.GetRepair1(repExe.gsSN, sdefectcode1, sid1);
                failDetail failItem = new failDetail();
                if (dtlocation2.Rows.Count > 0)
                {
                    string status = dtlocation2.Rows[0]["ENABLED"].ToString();
                    string rpstatus = dtlocation2.Rows[0]["RP_STATUS"].ToString();
                    if (status == "Y" && rpstatus == "0")
                    {
                        failItem.defCode = sdefectcode1;
                        failItem.failMsg = sdefectcode1 + "不良代码已维修完成，无需重复维修";
                        liFail.Add(failItem);
                      //  return ToResponse(ResultCode.FAIL, sdefectcode1 + "不良代码已维修完成，无需重复维修");

                        //LVDefect.Items[i].Selected = false;
                        //btnRemove.Enabled = false;
                        //btnRepair.Enabled = false;
                        //butReplace.Enabled = false;
                    }
                    else if (status == "Y" && rpstatus == "")
                    {
                        failItem.defCode = sdefectcode1;
                        failItem.failMsg = sdefectcode1 + "不良代码已维修完成，请确认";
                        liFail.Add(failItem);
                        //return ToResponse(ResultCode.FAIL, sdefectcode1 + "不良代码已维修，请确认!!");

                        //btnRemove.Enabled = false;
                        //btnRepair.Enabled = false;
                        //butReplace.Enabled = false;

                    }
                    else if (status == "Y" && rpstatus == "1")
                    {
                        failItem.defCode = sdefectcode1;
                        failItem.failMsg = sdefectcode1 + "不良代码已维修完成，请确认";
                        liFail.Add(failItem);
                       // return ToResponse(ResultCode.FAIL, sdefectcode1 + "不良代码已维修，请确认!!");
                        //LVDefect.Items[i].Selected = false;
                        //btnRemove.Enabled = false;
                        //btnRepair.Enabled = false;
                        //butReplace.Enabled = false;
                    }
                    else
                    {
                        liRecid.Add(sid1);
                        // return ToResponse(ResultCode.FAIL, sdefectcode1 + "不良代码未维修完成，请进行维修执行");
                        //btnRemove.Enabled = true;
                        //btnRepair.Enabled = true;
                        //butReplace.Enabled = true;
                    }
                }
                else
                {
                    liRecid.Add(sid1);
                    //  return ToResponse(ResultCode.FAIL, sdefectcode1 + "不良代码未维修完成，请进行维修执行");
                    //btnRemove.Enabled = true;
                    //btnRepair.Enabled = true;
                    //butReplace.Enabled = true;
                }
            }
            resLvDef.failItem = liFail;
            resLvDef.selectRecid = liRecid;
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "不良代码未维修完成，请进行维修执行", resLvDef));
          //  return ToResponse(ResultCode.SUCCESS, "不良代码未维修完成，请进行维修执行");
        }

        /// <summary>
        /// 不良原因类型
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:repairDefReasonType")]
        [HttpGet("repairDefReasonType")]
        public async Task<IActionResult> repairDefReasonType()
        {
            DataTable dt =await repairService.getDefectReasonType();

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", dt));
        }

        /// <summary>
        /// 不良原因
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:repairDefReason")]
        [HttpGet("repairDefReason")]
        public async Task<IActionResult> repairDefReason(string reasonType)
        {
            if (string.IsNullOrWhiteSpace(reasonType) || string.IsNullOrEmpty(reasonType))
            {
                return ToResponse(ResultCode.FAIL, "不良原因类型不能为空,请选择不良原因类型!!!");
            }

            DataTable dt = await repairService.getDefectReason(reasonType);

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", dt));
        }

        /// <summary>
        /// 查询SN不良原因
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="recid"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:getReason")]
        [HttpGet("getReason")]
        public async Task<IActionResult> getReason(string sn,string recid)
        {
            DataTable dt = await repairService.getReason(sn, recid);

            if (dt == null || dt.Rows.Count ==0)
            {
                return ToResponse(ResultCode.FAIL, "不良原因为空");
            }
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", dt));
        }

        /// <summary>
        /// Location
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:getLocation")]
        [HttpGet("getLocation")]
        public async Task<IActionResult> getLocation(string gSN,string defCode)
        {
            DataTable dt = await repairService.getLocation(gSN, defCode);

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", dt));
        }

        /// <summary>
        /// Action Validate
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:getACTValidate")]
        [HttpGet("getACTValidate")]
        public async Task<IActionResult> getACTValidate()
        {
            List<string> liAct = new List<string>();

            liAct.Add("仅重新测试");
            liAct.Add("交叉验证");
            liAct.Add("重新焊接");
            liAct.Add("报废");
            liAct.Add("SMT物料更换");
            liAct.Add("KEYPART SN替换");

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", liAct));
        }

        /// <summary>
        /// GetKPReplaceInfo
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:GetKPReplaceInfo")]
        [HttpGet("GetKPReplaceInfo")]
        public async Task<IActionResult> GetKPReplaceInfo(string gRecid, string gSN)
        {
            DataTable dt = await repairService.GetKPReplaceInfo(gRecid, gSN);

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", dt));
        }

        /// <summary>
        /// CheckOldCSN
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:CheckOldCSN")]
        [HttpGet("CheckOldCSN")]
        public async Task<IActionResult> CheckOldCSN(string gSN, string oldsn)
        {
            if (string.IsNullOrEmpty(oldsn))
            {
                return ToResponse(ResultCode.FAIL, "OLD KEYPART SN is null");
            }
            DataTable dt = await repairService.Checkoldsn(gSN, oldsn);

            if (dt.Rows.Count == 0)
            {
                return ToResponse(ResultCode.FAIL, oldsn + "   " + "OLD KEYPART SN NOT FOUND");
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", dt));
        }

        /// <summary>
        /// CheckNewCSN
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:CheckNewCSN")]
        [HttpGet("CheckNewCSN")]
        public async Task<IActionResult> CheckNewCSN(string gSN, string newSn, string oldSn)
        {
            if (string.IsNullOrEmpty(newSn))
            {
                return ToResponse(ResultCode.FAIL, "New KEYPART SN is null");
            }
            DataTable Checktype = await repairService.Checkparttype(gSN, oldSn);
            string sInStatus = string.Empty;

            if (Checktype.Rows.Count == 0)
            {
                return ToResponse(ResultCode.FAIL, "New Keypart SN has finished");
            }
            else
            {
               string sRuleType = Checktype.Rows[0]["PART_TYPE"].ToString();

                ExecuteResult exeR =await repairService.CheckNewKeypart(gSN, oldSn, sRuleType, newSn);
                if (!exeR.Status)
                {
                    return ToResponse(ResultCode.FAIL, exeR.Message);
                }
                else
                {
                    sInStatus = exeR.Message;
                }
            }
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", sInStatus));
        }

        /// <summary>
        /// CheckNewCPN
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:CheckNewCPN")]
        [HttpGet("CheckNewCPN")]
        public async Task<IActionResult> CheckNewCPN(string newCPN)
        {
            if (string.IsNullOrEmpty(newCPN))
            {
                return ToResponse(ResultCode.FAIL, "新料号的输入不能为空，请确认");
            }
            ExecuteResult exeRes = new ExecuteResult();
            RepairData repData = new  RepairData();

            if (newCPN.Contains("|"))
            {
                int num = Tools.SubstringCount(newCPN, "|");
                string[] list = newCPN.Split('|').ToArray();
                string userNo = HttpContext.GetName();

                if (num < 11)
                {
                    string reelno = list[5];
                  
                    exeRes =await repairService.CheckReelNo(reelno);

                    if (exeRes.Status)
                    {
                        DataTable dtTemp = (DataTable)exeRes.Anything;
                        if (dtTemp.Rows.Count > 0)
                        {
                            repData.newCPN = dtTemp.Rows[0]["IPN"].ToString();
                            repData.dateCode1 = dtTemp.Rows[0]["DATECODE"].ToString();
                            repData.lotCode1 = dtTemp.Rows[0]["LOT"].ToString();
                            repData.vendor1 = dtTemp.Rows[0]["VENDOR"].ToString();
                            repData.newReel = reelno;
                        }
                        else
                        {
                            repData.newCPN = list[0];
                            repData.dateCode1 = list[1];
                            string qty1 = list[2];
                            repData.vendor1 = list[3];
                            repData.lotCode1 = list[4];
                            await repairService.InsertReelNo(reelno, repData.newCPN.Trim(), repData.dateCode1.Trim(), qty1, repData.vendor1.Trim(), repData.lotCode1.Trim(), userNo);
                            await repairService.InsertReelNo_HT(reelno, repData.newCPN.Trim(), repData.dateCode1.Trim(), qty1, repData.vendor1.Trim(), repData.lotCode1.Trim(), userNo);
                            await repairService.InsertReelNoSMT(reelno, repData.newCPN.Trim(), repData.dateCode1.Trim(), qty1, repData.vendor1.Trim(), repData.lotCode1.Trim(), userNo);
                        }
                    }
                }
                else
                {
                    string reelno1 = list[6];
                    exeRes = await repairService.CheckReelNo(reelno1);
                    if (exeRes.Status)
                    {
                        DataTable dtTemp = (DataTable)exeRes.Anything;
                        if (dtTemp.Rows.Count > 0)
                        {
                            repData.newCPN = dtTemp.Rows[0]["IPN"].ToString();
                            repData.dateCode1 = dtTemp.Rows[0]["DATECODE"].ToString();
                            repData.lotCode1 = dtTemp.Rows[0]["LOT"].ToString();
                            repData.vendor1= dtTemp.Rows[0]["VENDOR"].ToString();
                            repData.newReel = reelno1;
                        }
                        else
                        {
                            repData.newCPN = list[0];
                            repData.dateCode1 = list[1];
                            string qty1 = list[2];
                            repData.vendor1 = list[3];
                            repData.lotCode1 = list[5];
                            string txtmsd1 = list[11];
                            txtmsd1 = txtmsd1.Replace("MSD", " ");
                            await repairService.InsertReelNo1(reelno1, repData.newCPN.Trim(), repData.dateCode1.Trim(), qty1, repData.vendor1.Trim(), repData.lotCode1.Trim(), txtmsd1,userNo);
                            await repairService.InsertReelNo1_HT(reelno1, repData.newCPN.Trim(), repData.dateCode1.Trim(), qty1, repData.vendor1.Trim(), repData.lotCode1.Trim(), txtmsd1, userNo);
                            await repairService.InsertReelNoSMT1(reelno1, repData.newCPN.Trim(), repData.dateCode1.Trim(), qty1, repData.vendor1.Trim(), repData.lotCode1.Trim(), txtmsd1, userNo);
                        }
                    }                     
                }
            }
            else
            {
                exeRes = await repairService.CheckReelNo(newCPN);
                if (exeRes.Status)
                {
                    DataTable dtTemp = (DataTable)exeRes.Anything;
                    if (dtTemp.Rows.Count > 0)
                    {
                        repData.newCPN = dtTemp.Rows[0]["IPN"].ToString();
                        repData.dateCode1 = dtTemp.Rows[0]["DATECODE"].ToString();
                        repData.lotCode1 = dtTemp.Rows[0]["LOT"].ToString();
                        repData.vendor1 = dtTemp.Rows[0]["VENDOR"].ToString();
                        repData.newReel = newCPN;
                    }
                    else
                    {
                        return ToResponse(ResultCode.FAIL, "没有找到新料盘ID的料号，请确认");
                    }
                }
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", repData));
        }

        /// <summary>
        /// CheckOldCPN
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:CheckOldCPN")]
        [HttpGet("CheckOldCPN")]
        public async Task<IActionResult> CheckOldCPN(string oldCPN)
        {
            if (string.IsNullOrEmpty(oldCPN))
            {
                return ToResponse(ResultCode.FAIL, "旧料号的输入不能为空，请确认");
            }
            ExecuteResult exeRes = new ExecuteResult();
            RepairData repData = new RepairData();

            if (oldCPN.Contains("|"))
            {
                int num = Tools.SubstringCount(oldCPN, "|");
                string[] list = oldCPN.Split('|').ToArray();
                string userNo = HttpContext.GetName();

                if (num < 11)
                {
                    string reelno = list[5];

                    exeRes = await repairService.CheckReelNo(reelno);

                    if (exeRes.Status)
                    {
                        DataTable dtTemp = (DataTable)exeRes.Anything;
                        if (dtTemp.Rows.Count > 0)
                        {
                            repData.newCPN = dtTemp.Rows[0]["IPN"].ToString();
                            repData.dateCode1 = dtTemp.Rows[0]["DATECODE"].ToString();
                            repData.lotCode1 = dtTemp.Rows[0]["LOT"].ToString();
                            repData.vendor1 = dtTemp.Rows[0]["VENDOR"].ToString();
                            repData.newReel = reelno;
                        }
                        else
                        {
                            repData.newCPN = list[0];
                            repData.dateCode1 = list[1];
                            string qty1 = list[2];
                            repData.vendor1 = list[3];
                            repData.lotCode1 = list[4];
                            await repairService.InsertReelNo(reelno, repData.newCPN.Trim(), repData.dateCode1.Trim(), qty1, repData.vendor1.Trim(), repData.lotCode1.Trim(), userNo);
                            await repairService.InsertReelNo_HT(reelno, repData.newCPN.Trim(), repData.dateCode1.Trim(), qty1, repData.vendor1.Trim(), repData.lotCode1.Trim(), userNo);
                            await repairService.InsertReelNoSMT(reelno, repData.newCPN.Trim(), repData.dateCode1.Trim(), qty1, repData.vendor1.Trim(), repData.lotCode1.Trim(), userNo);
                        }
                    }
                }
                else
                {
                    string reelno1 = list[6];
                    exeRes = await repairService.CheckReelNo(reelno1);
                    if (exeRes.Status)
                    {
                        DataTable dtTemp = (DataTable)exeRes.Anything;
                        if (dtTemp.Rows.Count > 0)
                        {
                            repData.newCPN = dtTemp.Rows[0]["IPN"].ToString();
                            repData.dateCode1 = dtTemp.Rows[0]["DATECODE"].ToString();
                            repData.lotCode1 = dtTemp.Rows[0]["LOT"].ToString();
                            repData.vendor1 = dtTemp.Rows[0]["VENDOR"].ToString();
                            repData.newReel = reelno1;
                        }
                        else
                        {
                            repData.newCPN = list[0];
                            repData.dateCode1 = list[1];
                            string qty1 = list[2];
                            repData.vendor1 = list[3];
                            repData.lotCode1 = list[5];
                            string txtmsd1 = list[11];
                            txtmsd1 = txtmsd1.Replace("MSD", " ");
                            await repairService.InsertReelNo1(reelno1, repData.newCPN.Trim(), repData.dateCode1.Trim(), qty1, repData.vendor1.Trim(), repData.lotCode1.Trim(), txtmsd1, userNo);
                            await repairService.InsertReelNo1_HT(reelno1, repData.newCPN.Trim(), repData.dateCode1.Trim(), qty1, repData.vendor1.Trim(), repData.lotCode1.Trim(), txtmsd1, userNo);
                            await repairService.InsertReelNoSMT1(reelno1, repData.newCPN.Trim(), repData.dateCode1.Trim(), qty1, repData.vendor1.Trim(), repData.lotCode1.Trim(), txtmsd1, userNo);
                        }
                    }
                }
            }
            else
            {
                exeRes = await repairService.CheckReelNo(oldCPN);
                if (exeRes.Status)
                {
                    DataTable dtTemp = (DataTable)exeRes.Anything;
                    if (dtTemp.Rows.Count > 0)
                    {
                        repData.newCPN = dtTemp.Rows[0]["IPN"].ToString();
                        repData.dateCode1 = dtTemp.Rows[0]["DATECODE"].ToString();
                        repData.lotCode1 = dtTemp.Rows[0]["LOT"].ToString();
                        repData.vendor1 = dtTemp.Rows[0]["VENDOR"].ToString();
                        repData.newReel = oldCPN;
                    }
                    else
                    {
                        return ToResponse(ResultCode.FAIL, "没有找到新料盘ID的料号，请确认");
                    }
                }
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", repData));
        }

        /// <summary>
        /// RepairDataOK
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:RepairDataOK")]
        [HttpPost("RepairDataOK")]
        public async Task<IActionResult> RepairDataOK([FromBody] RepairDataDto repDto)
        {
            resRepairInfo resRepair = new resRepairInfo();

            DataTable dt =await repairService.CheckReason(repDto.reasonCode);
            if (dt.Rows.Count == 0)
            {
                return ToResponse(ResultCode.FAIL, "Reason Code Error" + Environment.NewLine + "Reason Code" + " : " + repDto.reasonCode);
            }
            resRepair.reasonCode = dt.Rows[0]["REASON_CODE"].ToString();
            resRepair.reasonDesc = dt.Rows[0]["Reason_Desc"].ToString();
            resRepair.reasonDesc1 = dt.Rows[0]["REASON_DESC2"].ToString();

            if (string.IsNullOrWhiteSpace(repDto.remark) || string.IsNullOrEmpty(repDto.remark))
            {
                return ToResponse(ResultCode.FAIL, " Remark不能为空,请输入!");
            }

            if (repDto.casetype == "")
            {
                return ToResponse(ResultCode.FAIL, "请选择任一问题类型Case Type!!!");
            }

            if (repDto.repLiData.Count == 0)
            {
                return ToResponse(ResultCode.FAIL, "维修的Location不能为空,请输入!");
            }

            RepairInfo repairInfo = new RepairInfo();
            repairInfo.Sn = repDto.gSn;
            repairInfo.Casetype = repDto.casetype;
            repairInfo.Station = repDto.comStation;
            repairInfo.Remark = repDto.remark;
            repairInfo.DefectsSym = repDto.defectsSym;
            List<RepairDetail> repairdetails = new List<RepairDetail>();

            foreach (RepDetailData rowRepair in repDto.repLiData)
            {
                RepairDetail repairDetail = new RepairDetail();
                repairDetail.Action = rowRepair.action;
                repairDetail.Location = rowRepair.location;
                repairDetail.Old_Csn = rowRepair.oldcsn;
                repairDetail.New_Csn = rowRepair.newcsn;

                repairDetail.Old_Cpn = rowRepair.oldcpn;
                repairDetail.Old_Reel = rowRepair.oldreel;
                repairDetail.Vendor = rowRepair.vendor;
                repairDetail.Datecode = rowRepair.dateCode;
                repairDetail.Lotcode = rowRepair.lotCode;

                repairDetail.New_Cpn = rowRepair.newCPN;
                repairDetail.New_Reel = rowRepair.newReel;
                repairDetail.Vendor1 = rowRepair.vendor1;
                repairDetail.Datecode1 = rowRepair.dateCode1;
                repairDetail.Lotcode1 = rowRepair.lotCode1;
                repairInfo.Remark = rowRepair.remark;
                repairDetail.Remark1 = rowRepair.remark1;
                repairdetails.Add(repairDetail);
            }

            repairInfo.RepairDetails = repairdetails;
            string g_sDutyID = "0";//責任類別
            string userNo = HttpContext.GetName();
            //====SAJET.SJ_REPAIR_REASON // 维修原因记录
            ExecuteResult exe =await repairService.RepairSnReason(repDto.gSn, repDto.repairRes.wo, repDto.repairRes.partNo, repDto.repairRes.defectRecID, repDto.reasonCode, repDto.reasonType, g_sDutyID, repDto.repairRes.stationName, userNo);

            if (!exe.Status)
            {
                return ToResponse(ResultCode.FAIL, exe.Message);
            }

            DataTable dtKP =await repairService.GetKPReplaceInfo(repDto.repairRes.defectRecID, repDto.gSn);
            string kpipn, kpoldsn, kpnewsn;
            string kpempno = string.Empty;
            string kptime = string.Empty;
            string kpstation = string.Empty;

            if (dtKP.Rows.Count > 0)
            {
                for (int i = 0; i <= dtKP.Rows.Count - 1; i++)
                {
                    kpipn = dtKP.Rows[i]["ITEM_IPN"].ToString();
                    kpoldsn = dtKP.Rows[i]["OLD_IPN_SN"].ToString();
                    kpnewsn = dtKP.Rows[i]["NEW_IPN_SN"].ToString();

                    kpempno = dtKP.Rows[i]["REPLACE_EMPNO"].ToString();
                    kptime = dtKP.Rows[i]["REPLACE_TIME"].ToString();
                    kpstation = dtKP.Rows[i]["NEW_IPN"].ToString();

                    ExecuteResult exe1 = await repairService.SaveRepairDetail(repairInfo, repDto.repairRes.defectRecID, kpipn, kpoldsn, kpnewsn, kpempno, kptime, kpstation, userNo);
                    if (!exe1.Status)
                    {
                        return ToResponse(ResultCode.FAIL, exe1.Message);
                    }
                }
                //save data to repair details 维修详情记录
            }
            else
            {
                ExecuteResult exe1 = await repairService.SaveRepairDetail1(repairInfo, repDto.repairRes.defectRecID, kpempno, kptime, kpstation, userNo);
                if (!exe1.Status)
                {
                    return ToResponse(ResultCode.FAIL, exe1.Message);
                }
            }
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", resRepair));
        }


        /// <summary>
        /// 维修--移除Check
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:CheckRepairRemove")]
        [HttpPost("CheckRepairRemove")]
        public async Task<IActionResult> CheckRepairRemove([FromBody] RepairRemoveCHK repRemoveChk)
        {
            if (string.IsNullOrEmpty(repRemoveChk.gSn))
            {
                return ToResponse(ResultCode.FAIL, "请输入SN");
            }

            if (string.IsNullOrEmpty(repRemoveChk.repairEmpNo))
            {
                return ToResponse(ResultCode.FAIL, "请输入员工工号");
            }
            else
            {
                DataTable emp1 =await repairService.getEmpName(repRemoveChk.repairEmpNo);
                if (emp1.Rows.Count == 0)
                {
                    return ToResponse(ResultCode.FAIL, "此工号" + repRemoveChk.repairEmpNo + "不存在，请确认并重新输入");
                }
            }
            if (repRemoveChk.lvDef.Count() == 0)
            {
                return ToResponse(ResultCode.FAIL, "请选择不良信息");
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", null));
        }


        /// <summary>
        /// 维修--add不良信息
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:AddDefect")]
        [HttpPost("AddDefect")]
        public async Task<IActionResult> AddDefect([FromBody] repairDef repDef)
        {
            if (string.IsNullOrEmpty(repDef.sn))
            {
                return ToResponse(ResultCode.FAIL, "请输入SN");
            }

            DataTable  dtTemp =await repairService.GetDefectCodeInfo(repDef.sDefCode);
            if (dtTemp.Rows.Count == 0)
            {
                return ToResponse(ResultCode.FAIL, "不良代码错误：" + repDef.sDefCode);
            }
            string sDefectDesc = dtTemp.Rows[0]["Defect_Desc"].ToString();
            string sDefectDesc2 = dtTemp.Rows[0]["Defect_Desc2"].ToString();
            string sRecID = await repairService.GetDefectRECID();
            if (sRecID == "0")
            {
                return ToResponse(ResultCode.FAIL, "Get Defect RECID Error");
            }
            string userNo = HttpContext.GetName();
            await repairService.AddDefectInfo(sRecID, userNo, repDef);

            LVDefect lVDefect = new LVDefect();
            lVDefect.DefectCode = repDef.sDefCode;
            lVDefect.DEFECT_DESC = sDefectDesc;
            lVDefect.DEFECT_DESC2 = sDefectDesc2;
            lVDefect.LOCATION = repDef.sLocation;
            lVDefect.RECID = sRecID;
            lVDefect.STATION_TYPE = repDef.stationType;
            lVDefect.ImageIndex = 1;

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", lVDefect));
        }

        /// <summary>
        /// 维修--删除不良信息
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:DelDefect")]
        [HttpPost("DelDefect")]
        public async Task<IActionResult> DelDefect([FromBody] repariDto repDef)
        {
            if (string.IsNullOrEmpty(repDef.sn))
            {
                return ToResponse(ResultCode.FAIL, "请输入SN");
            }
            if (repDef.lvdef.Count() == 0)
            {
                return ToResponse(ResultCode.FAIL, "不良信息为空");
            }

            if (repDef.lvdef[0].STATION_TYPE != repDef.stationType)
            {
                return ToResponse(ResultCode.FAIL, "Can't Delete this Defect Code");
            }

            string sDefectCode = repDef.lvdef[0].DefectCode;
            string sRECID = repDef.lvdef[0].RECID;

            //删除记录 delete  by sRECID
              await repairService.DeleteDefectByRecid(sRECID);

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "删除不良信息成功", ""));
        }

        /// <summary>
        /// 获取回流站点
        /// </summary>
        /// <param name="gSN"></param>
        /// <param name="gStation"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:GetReturnStation")]
        [HttpGet("GetReturnStation")]
        public async Task<IActionResult> GetReturnStation(string gSN,string gStation)
        {
            if (string.IsNullOrEmpty(gStation))
            {
                return ToResponse(ResultCode.FAIL, "请维护站点信息");
            }

            DataTable dt = await repairService.GetReturnStation(gSN, gStation);

            if (dt.Rows.Count > 0)
            {
                return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", dt));
            }
            else
            {
                return ToResponse(ResultCode.FAIL, "未维护回流站点");
            }
        }

        /// <summary>
        /// 维修完成
        /// </summary>
        /// <param name="gSN">sn</param>
        /// <param name="gStation">当前站点</param>
        /// <param name="reStation">回流站点</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:RepairFinish")]
        [HttpGet("RepairFinish")]
        public async Task<IActionResult> RepairFinish(string gSN, string gStation,string reStation)
        {
            if (string.IsNullOrEmpty(reStation))
            {
                return ToResponse(ResultCode.FAIL, "请维护站点信息");
            }
            string userNo = HttpContext.GetName();
            ExecuteResult exeRes =await repairService.RepairGo(gStation, gSN, reStation, userNo);

            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.FAIL, exeRes.Message);
            }
            else
            {
                return ToResponse(new ApiResult((int)ResultCode.SUCCESS, exeRes.Message, ""));
            }
        }

        [ActionPermissionFilter(Permission = "repair:repairexe:Scrap")]
        [HttpPost("Scrap")]
        public async Task<IActionResult> Scrap([FromBody] ScrapDto scrapDto)
        {
            string userNo = HttpContext.GetName();
           
            await repairService.AddScrapInfo(scrapDto,userNo);
            await repairService.UpdateSnStatus(scrapDto.sn,userNo);
            await repairService.AddSnTravel(scrapDto.sn);
           
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "序列号报废成功", ""));
        }

    }
}