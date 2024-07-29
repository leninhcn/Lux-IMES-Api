using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZR.Infrastructure.Model;
using ZR.Service.Repair;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;
using static ZR.ServiceCore.Model.Dto.ErrorMaterial;

namespace ZR.Admin.WebApi.Controllers.Repair
{
    /// <summary>
    /// 防错料
    /// </summary>
    //[Verify]
    [Route("repair/errorproof")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class ErrorProofMaterialController : BaseController
    {
        public IErrorProofMaterialService repairService;
        public IRepairExeService repExeService;
        public ErrorProofMaterialController(IErrorProofMaterialService _repairService, IRepairExeService _repExeService)
        {
            repairService = _repairService;
            repExeService = _repExeService;
        }

        /// <summary>
        /// 获取SN状态
        /// </summary>
        /// <param name="gSn">sn</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:errorproof:GetSnStatusInfo")]
        [HttpGet("GetSnStatusInfo")]
        public async Task<IActionResult> GetSnStatusInfo(string gSn)
        {
            if (string.IsNullOrEmpty(gSn))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "SN不能为空");
            }
            DataTable dtSnStatusInfo =await repairService.GetSnStatusInfo(gSn);

            snStatusInfo snStaus = new snStatusInfo();
            if (dtSnStatusInfo.Rows.Count > 0)
            {
                snStaus.panelNo = dtSnStatusInfo.Rows[0]["PANEL_NO"].ToString();
                snStaus.snCounter = dtSnStatusInfo.Rows[0]["SN_COUNTER"].ToString() == null ? "" : dtSnStatusInfo.Rows[0]["SN_COUNTER"].ToString();

                return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", snStaus));
            }

            return ToResponse(ResultCode.PARAM_ERROR, "未找到sn状态信息");
        }

        /// <summary>
        /// OldPartNo Check
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="partNo"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:errorproof:ChkOldPartNo")]
        [HttpGet("ChkOldPartNo")]
        public async Task<IActionResult> ChkOldPartNo(string wo,string partNo)
        {
            if (string.IsNullOrEmpty(partNo))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "partNo不能为空");
            }

            ExecuteResult exeRes = await repairService.CheckWoBomItemIpn(wo, partNo);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", ""));
        }

        /// <summary>
        /// ChkNewPartNo
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="oldPartNo"></param>
        /// <param name="newPartNo"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:errorproof:ChkNewPartNo")]
        [HttpGet("ChkNewPartNo")]
        public async Task<IActionResult> ChkNewPartNo(string wo, string oldPartNo,string newPartNo)
        {
            if (string.IsNullOrEmpty(newPartNo))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "partNo不能为空");
            }

            ExecuteResult exeRes = await repairService.CheckWoBomItemIpn(wo, newPartNo);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
            exeRes = await repairService.CheckNewReelPn(wo, oldPartNo, newPartNo);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", ""));

        }

        /// <summary>
        /// NewReelNo  CHECK
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="oldPartNo"></param>
        /// <param name="newPartNo"></param>
        /// <param name="newreel"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:errorproof:ChkNewReelNo")]
        [HttpGet("ChkNewReelNo")]
        public async Task<IActionResult> ChkNewReelNo(string wo, string oldPartNo,string newPartNo, string newreel)
        {
            if (string.IsNullOrEmpty(newreel))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "New ReelNo不能为空");
            }

            //获取reelno 的料号 datacode lotcode

            ExecuteResult exeRes =await repairService.CheckReelNo(newreel);
            if (exeRes.Status)
            {
                DataTable dtTemp = (DataTable)exeRes.Anything;
                if (dtTemp.Rows.Count > 0)
                {
                    string reelpartno = dtTemp.Rows[0]["IPN"].ToString();
                    // 检查new PCN 和 odl CPN 是否一致或者替代料关系
                    exeRes =await repairService.CheckNewReelPn(wo, oldPartNo, reelpartno);
                    if (!exeRes.Status)
                    {
                        return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
                    }
                    // 赋值给前端
                    if (reelpartno != newPartNo)
                    {
                        return ToResponse(ResultCode.PARAM_ERROR, "New partno 和 New Reelno 的料号不一致 " + Environment.NewLine
                            + "New partno:" + newPartNo + Environment.NewLine
                            + "New reel partno:" + reelpartno);
                       
                    }
                    errorPM erpm = new errorPM();
                    erpm.dateCode = dtTemp.Rows[0]["DATECODE"].ToString();
                    erpm.lotCode = dtTemp.Rows[0]["LOT"].ToString();
                    erpm.newReelNo = newreel;

                    return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", erpm));
                }
                else
                {
                    return ToResponse(ResultCode.PARAM_ERROR, "未获取到reelno 的料号 datacode lotcode");
                }
            }

            return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);

        }

        /// <summary>
        /// ErrorMaterialOK
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairexe:ErrorMaterialOK")]
        [HttpPost("ErrorMaterialOK")]
        public async Task<IActionResult> ErrorMaterialOK([FromBody] RepairDataDto repDto)
        {
            resRepairInfo resRepair = new resRepairInfo();

            DataTable dt = await repExeService.CheckReason(repDto.reasonCode);
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

                repairDetail.Old_Cpn = rowRepair.oldcpn;
                repairDetail.New_Cpn = rowRepair.newCPN;
                repairDetail.Old_Reel = rowRepair.oldreel;
                repairDetail.Datecode1 = rowRepair.dateCode1;
                repairDetail.Lotcode1 = rowRepair.lotCode1;

                repairdetails.Add(repairDetail);
            }

            repairInfo.RepairDetails = repairdetails;
            string kpempno = HttpContext.GetName();
            string kptime = DateTime.Now.ToString();
            string g_sDutyID = "0";//責任類別
            string userNo = HttpContext.GetName();
            string kpstation = string.Empty;
            // 保存替换 reel
            await repairService.SaveReplaceReelDetail(repairInfo, repDto.repairRes.defectRecID, kpempno, kptime);

            //====SAJET.SJ_REPAIR_REASON // 维修原因记录
            ExecuteResult exe = await repExeService.RepairSnReason(repDto.gSn, repDto.repairRes.wo, repDto.repairRes.partNo, repDto.repairRes.defectRecID, repDto.reasonCode, repDto.reasonType, g_sDutyID, repDto.repairRes.stationName, userNo);

            if (!exe.Status)
            {
                return ToResponse(ResultCode.FAIL, exe.Message);
            }
            ExecuteResult exe1 = await repExeService.SaveRepairDetail1(repairInfo, repDto.repairRes.defectRecID, kpempno, kptime, kpstation, userNo);
            if (!exe1.Status)
            {
                return ToResponse(ResultCode.FAIL, exe.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", resRepair));
        }

    }
}
