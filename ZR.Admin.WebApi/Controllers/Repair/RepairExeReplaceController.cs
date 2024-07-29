using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using ZR.Infrastructure.Model;
using ZR.Model.Repair.Dto;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Admin.WebApi.Controllers.Repair
{
    /// <summary>
    /// 维修替换
    /// </summary>
    //[Verify]
    [Route("repair/repairexereplace")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class RepairExeReplaceController : BaseController
    {
        public IRepairExeReplaceService repairService;
        public RepairExeReplaceController(IRepairExeReplaceService _repairService)
        {
            repairService = _repairService;
        }

        /// <summary>
        /// 获取关键料信息
        /// </summary>
        /// <param name="sSN">sn</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexereplace:getKpsn")]
        [HttpGet("getKpsn")]
        public async Task<IActionResult> getKpsn(string sSN)
        {
            if (string.IsNullOrEmpty(sSN))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "SN不能为空，请输入SN");
            }
            DataTable dtTemp = new DataTable();
            dtTemp = await repairService.getKpsn(sSN);

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", dtTemp));
        }

        /// <summary>
        /// Check  NewKPSN
        /// </summary>
        /// <param name="repReplaceDto">repReplaceDto</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexereplace:CheckNewKPSN")]
        [HttpPost("CheckNewKPSN")]
        public async Task<IActionResult> CheckNewKPSN([FromBody]RepairReplace repReplaceDto)
        {
            if (string.IsNullOrEmpty(repReplaceDto.kpsn))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "New Keypart SN is null");
            }
            string sRuleType = "";

            if (repReplaceDto.liKpNew.Count() == 0)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "New Keypart is null");
            }

            var liKP = repReplaceDto.liKpNew;

            for (int i = 0; i < liKP.Count(); i++)
            {
                if (string.IsNullOrEmpty(liKP[i].inputValue))
                {
                    sRuleType = liKP[i].partType;
                    break;
                }
            }
            if (string.IsNullOrEmpty(sRuleType))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "New Keypart SN has finished");
            }
            else
            {
                string sKPSN = liKP[0].inputValue;      
                string sInStatus = "";
                ExecuteResult exe =await repairService.CheckNewKeypart(repReplaceDto.sn, sKPSN, sRuleType, repReplaceDto.kpsn);
                if (!exe.Status)
                {
                    return ToResponse(ResultCode.PARAM_ERROR, exe.Message);
                }
                else
                {
                    sInStatus = exe.Message;
                }
                /**
                //for (int i = 0; i < liKP.Count(); i++)
                //{
                //    if (liKP[i].partType == sRuleType)
                //    {
                //        liKP[i].inputValue = repReplaceDto.kpsn;
                //    }
                //}

                //if (sRuleType == "KPSN" && sInStatus == "Y")
                //{
                //    this.rdbtnYes.Checked = true;
                //    this.rdbtnNo.Enabled = false;
                //    this.rdbtnYes.Enabled = false;
                //}
                //else if (sRuleType == "KPSN" && sInStatus == "N")
                //{
                //    this.rdbtnNo.Checked = true;
                //    this.rdbtnNo.Enabled = false;
                //    this.rdbtnYes.Enabled = false;
                //    this.editDefect.Enabled = false;
                //    this.btnSearchDefect.Enabled = false;
                //    this.RichTextRemark.Focus();
                //    this.RichTextRemark.SelectAll();
                //}
                //this.editNewKPSN.Text = "";
                //this.txtRID.Focus();
                //this.txtRID.SelectAll();
                */
                return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", sInStatus));
            }
        }

        /// <summary>
        /// editDefect
        /// </summary>
        /// <param name="repDef">repDef</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexereplace:editDefect")]
        [HttpPost("editDefect")]
        public async Task<IActionResult> editDefect([FromBody] RepReplaceDef repDef)
        {
            if (string.IsNullOrEmpty(repDef.defectCode))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "Defect Code is null");
            }

            DataTable dt =await repairService.getDefect(repDef.defectCode);

            if (dt.Rows.Count == 0)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "Defect Code Error");
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", dt));
            //if (LVEC.FindItemWithText(sDefectCode) != null)
            //{
            //    utility.ShowMessage("Defect Code Duplicate", 0);
            //    editDefect.Focus();
            //    editDefect.SelectAll();
            //    return;
            //}
            //LVEC.Items.Add(dsTemp.Tables[0].Rows[0]["DEFECT_CODE"].ToString());
            //LVEC.Items[LVEC.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["DEFECT_DESC"].ToString());

        }

        /// <summary>
        /// defReplaceKP
        /// </summary>
        /// <param name="repDef">repDef</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexereplace:defReplaceKP")]
        [HttpPost("defReplaceKP")]
        public async Task<IActionResult> defReplaceKP([FromBody] defReplaceKp repDef)
        {
            if (repDef.lvkp.Count()==0)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "Please select a Keypart to replace");
            }
            var lvNewKP = repDef.liKpNew;
            if (lvNewKP.Count() > 0)
            {
                string sNewPartSN = "";
                string sNewPartSNCustomer = "";
                string sNewPartSNMac = "";
                bool bIsFinish = false;
                for (int i = 0; i < lvNewKP.Count(); i++)
                {
                    if (string.IsNullOrEmpty(lvNewKP[i].inputValue))
                    {
                        bIsFinish = true;
                        break;
                    }
                    else
                    {
                        sNewPartSN = lvNewKP[i].inputValue.Trim();
                    }
                }

                if (bIsFinish)
                {
                    return ToResponse(ResultCode.PARAM_ERROR, "New KeyPart SN not finish");
                }

               // return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", sNewPartSN));

                if (sNewPartSN != "")
                {
                   await ReplaceKP("", "0", sNewPartSN, sNewPartSNCustomer, sNewPartSNMac, repDef);
                }
            }
            return ToResponse(ResultCode.PARAM_ERROR, "Replace  KeyPart SN Error");
            // return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", dt));
        }

        private async Task<bool> ReplaceKP(string sNewPartID, string sNewItem, string sNewPartSN, string sNewPartSNCustomer, string sNewPartSNMac, defReplaceKp repDef)
        {
            string sOldKPNO = repDef.lvkp[0].kpsn;   
            string sOldKPSN = repDef.lvkp[0].sn;
            string sOldPartID = repDef.lvkp[0].partType;

            string rid = repDef.liKpNew[0].rid;   
            string lotcode = repDef.liKpNew[0].lc;
            string datecode = repDef.liKpNew[0].dc; 

            string sKPFlag = "N";
            string sKPDefectData = "";
            if (repDef.rdbtnYes)
            {
                sKPFlag = "Y";
                var LVEC = repDef.LVEC;
                for (int i = 0; i <= LVEC.Count()-1; i++)
                {
                    sKPDefectData = sKPDefectData + LVEC[i].defDes + "@"
                                                 + LVEC[i].defCode + "@";
                }
            }
            if (sKPDefectData == "")
                sKPDefectData = "N/A";

            var g_sRTerminalID = repDef.stationName;
            var userNo = HttpContext.GetName();
            ExecuteResult exe =await repairService.ReplaceKpsn(g_sRTerminalID, repDef.sn, repDef.defRecid, sOldKPSN, sOldKPNO, sNewPartSN, sNewPartID, sNewItem, sKPFlag, sKPDefectData, repDef.remark, userNo, "", "", lotcode, datecode);
            if (!exe.Status)
            {
               // utility.ShowMessage(exe.Message, 0);
                return false;
            }
            await repairService.InsertReplaceKP(repDef.defRecid, g_sRTerminalID, repDef.sn, sOldKPSN, sOldKPNO, sNewPartSN, sNewPartID, repDef.remark, userNo, lotcode, datecode, rid);

            return true;
        }

        /// <summary>
        /// defRemoveKP
        /// </summary>
        /// <param name="repDef">repDef</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexereplace:defRemoveKP")]
        [HttpPost("defRemoveKP")]
        public async Task<IActionResult> defRemoveKP([FromBody] defReplaceKp repDef)
        {
            if (repDef.lvkp.Count() == 0)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "Please select a Keypart to remove");
            }
            var lvkp = repDef.lvkp;
            string sKPSN = "";
            string sKPNO = "";
            string sPartNo = "";
           
            for (int j = 0; j < lvkp.Count(); j++)
            {
                sKPSN = lvkp[j].sn;
                sKPNO = lvkp[j].sn;
                sPartNo = lvkp[j].kpsn;
              
                string sKPFlag = "N";
                string sKPDefectData = "";
                if (repDef.rdbtnYes)
                {
                    sKPFlag = "Y";
                    var LVEC = repDef.LVEC;
                    for (int i = 0; i <= LVEC.Count() - 1; i++)
                    {
                        sKPDefectData = sKPDefectData + LVEC[i].defDes + "@"
                                                     + LVEC[i].defCode + "@";
                    }
                }
                if (sKPDefectData == "")
                    sKPDefectData = "N/A";
                var g_sRTerminalID = repDef.stationName;
                var userNo = HttpContext.GetName();

                ExecuteResult exe = await repairService.RemoveKp(g_sRTerminalID, repDef.sn, repDef.defRecid, sKPSN, sPartNo, sKPFlag, sKPDefectData, userNo);

                if (!exe.Status)
                {
                    return ToResponse(ResultCode.PARAM_ERROR, exe.Message);
                }
                return ToResponse(ResultCode.SUCCESS);
            }
            return ToResponse(ResultCode.PARAM_ERROR, "Please select a Keypart");
        }
    }
}
