using IPTools.Core;
using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Tsp;
using System.Data;
using ZR.Infrastructure.Model;
using ZR.Model.Dto;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Admin.WebApi.Controllers.Repair
{
    /// <summary>
    /// FAE借入借出
    /// </summary>
    [Route("repair/faetransfer")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class FAETransferController : BaseController
    {
        public IFAETransferService repairService;
        public FAETransferController(IFAETransferService _repairService)
        {
            repairService = _repairService;
        }
        SNTInfo snInfo = new SNTInfo();
        WOTInfo woInfo = new WOTInfo();

        /// <summary>
        /// check SN借出信息
        /// </summary>
        /// <param name="sn">sn</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:faetransfer:CheckOutSn")]
        [HttpGet("CheckOutSn")]
        public async Task<IActionResult> CheckOutSn(string sn)
        {
            if (string.IsNullOrEmpty(sn))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "SN不能为空");
            }
            SNTInfo snInfo = new SNTInfo();
            ExecuteResult exeRes = await repairService.GetValues(sn, "SN", snInfo);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
            snInfo = (SNTInfo)exeRes.Anything;

            //检查Repair CheckIn
            exeRes = await repairService.CheckRepairIn(sn);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", snInfo));
        }

        /// <summary>
        /// Check 工号（借出）
        /// </summary>
        /// <param name="userNo"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:faetransfer:CheckEmpNo")]
        [HttpGet("CheckEmpNo")]
        public async Task<IActionResult> CheckEmpNo(string userNo)
        {
            if (string.IsNullOrEmpty(userNo))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "SN不能为空");
            }

            ExecuteResult exeRes = await repairService.CheckOutUser(userNo);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "工号" + userNo + "无效！");
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", exeRes.Anything));
        }

        /// <summary>
        /// Check 工号（归还）
        /// </summary>
        /// <param name="empno"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:faetransfer:CheckBackUser")]
        [HttpGet("CheckBackUser")]
        public async Task<IActionResult> CheckBackUser(string empno)
        {
            if (string.IsNullOrEmpty(empno))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "SN不能为空");
            }

            ExecuteResult exeRes = await repairService.CheckBackUser(empno);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "工号" + empno + "无效！");
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", ""));
        }

        /// <summary>
        /// 借出
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:faetransfer:SaveTransferOut")]
        [HttpGet("SaveTransferOut")]
        public async Task<IActionResult> SaveTransferOut(string sn)
        {
            faeTransInfo snInfo = new faeTransInfo();
            //检查是否已有记录
            ExecuteResult exeRes = await repairService.FAESNOUTCheck(sn);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
            DataTable dtTemp = (DataTable)exeRes.Anything;
            if (dtTemp.Rows.Count > 0)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "SN:" + sn + "已借出!");
            }
            exeRes = await repairService.GetRepairInRecid(snInfo.SN);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
            dtTemp = (DataTable)exeRes.Anything;
            Decimal recid = Convert.ToDecimal(dtTemp.Rows[0]["RECID"].ToString());
            FAETransferInfo transferInfo = new FAETransferInfo()
            {
                CREATE_EMPNO = HttpContext.GetName(),
                IPN = snInfo.IPN,
                SN = snInfo.SN,
                LINE = snInfo.LINE,
                WORK_ORDER = woInfo.WO,
                Model = snInfo.MODEL,

                OUTFROMUSERID = snInfo.outFromUserNo,
                OUTTOUSERID = snInfo.outToUserNo,
                OUTTOUSERPHONE = snInfo.outToUserPhone,

                STATUS = 0,
                LAB = snInfo.outLab,
                RECID = recid
            };

            exeRes = await repairService.InsertTransfer(transferInfo);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "资料插入失败！msg:" + exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "借出成功!", exeRes.Anything));
        }


        /// <summary>
        /// check SN归还信息
        /// </summary>
        /// <param name="sn">sn</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:faetransfer:CheckInSn")]
        [HttpGet("CheckInSn")]
        public async Task<IActionResult> CheckInSn(string sn)
        {
            if (string.IsNullOrEmpty(sn))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "SN不能为空");
            }
          
            SNTInfo snInfo = new SNTInfo();
            ExecuteResult exeRes = await repairService.GetValues(sn, "SN", snInfo);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
            snInfo = (SNTInfo)exeRes.Anything;

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", snInfo));
        }

        /// <summary>
        /// 归还
        /// </summary>
        /// <param name="snInfo"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:faetransfer:SaveTransferIn")]
        [HttpGet("SaveTransferIn")]
        public async Task<IActionResult> SaveTransferIn(faeTransInfo snInfo)
        {
            if (snInfo.SN == "")
            {
                return ToResponse(ResultCode.PARAM_ERROR, "SN不能为空");
               
            }
            if (snInfo.outFromUserNo == "")
            {
                return ToResponse(ResultCode.PARAM_ERROR, "经受人不能为空");

            }
            if (snInfo.outToUserNo == "")
            {
                return ToResponse(ResultCode.PARAM_ERROR, "归还人不能为空");

            }
            if (string.IsNullOrEmpty(snInfo.IPN)|| string.IsNullOrEmpty(snInfo.WO)|| string.IsNullOrEmpty(snInfo.MODEL))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "SN:"+ snInfo.SN+"没有相关信息！");

            }

            //检查是否已借出记录
            ExecuteResult exeRes = await repairService.GetFAESNOUT(snInfo.SN);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
            DataTable dtTemp = (DataTable)exeRes.Anything;
            if (dtTemp.Rows.Count == 0)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "SN:" + snInfo.SN + "没有借出数据!");
            }

            FAETransferInfo transferInfo = ParseTransferInfo(dtTemp, snInfo);
            transferInfo.STATUS = 1;

            exeRes =await repairService.UpdateTransfer(transferInfo);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "资料更新失败！msg:" + exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "归还成功!请继续作业！", exeRes.Anything));
        }

        private FAETransferInfo ParseTransferInfo(DataTable dt, faeTransInfo sninfo)
        {
            DataRow dr = dt.Rows[0];
            FAETransferInfo transferInfo = new FAETransferInfo();
            if (dr["ID"] != null && dr["ID"].ToString() != "")
                transferInfo.ID = int.Parse(dr["ID"].ToString());
            transferInfo.UPDATE_EMPNO = HttpContext.GetName();
            transferInfo.BACKFROMUSERID = sninfo.outToUserNo;
            transferInfo.BACKTOUSERID = sninfo.outFromUserNo;
            if (dr["ID"] != null)
                transferInfo.LAB = dr["LAB"].ToString();
            return transferInfo;
        }
    }
}
