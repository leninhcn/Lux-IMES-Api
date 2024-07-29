using Infrastructure;
using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.Protocols;
using System.Runtime.Intrinsics.X86;
using ZR.Admin.WebApi.Filters;
using ZR.Infrastructure.Model;
using ZR.Model.Dto;
using ZR.Model.Repair.Dto;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Service.Repair.IRepairService;
using ZR.Service.System.IService;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ResultCode = Infrastructure.ResultCode;

namespace ZR.Admin.WebApi.Controllers.Repair
{
    /// <summary>
    /// 维修delink
    /// </summary>
    //[Verify]
    [Route("repair/repairdelink")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class RepairDelinkController : BaseController
    {
        public IRepairDelinkService repairService;
        public RepairDelinkController(IRepairDelinkService _repairService)
        {
            repairService = _repairService;
        }

        /// <summary>
        /// 维修Delink 
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairdelink:list")]
        [HttpPost("List")]
        public async Task<IActionResult> List([FromBody] RepairInDto typeSN)
        {
            if (string.IsNullOrEmpty(typeSN.sn))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "序列号输入不能为空");
            }

            ExecuteResult exeRes = new ExecuteResult();
            DataTable dtTemp = new DataTable();
            repariDel repBase = new repariDel();
            exeRes =await repairService.CheckSN(typeSN.sn);

            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.FAIL, exeRes.Message);
            }
            else
            {
                string sSn = exeRes.Anything.ToString();

                dtTemp =await repairService.getDetail(sSn);
                if (dtTemp.Rows.Count > 0)
                {
                    repBase.wo = dtTemp.Rows[0]["WORK_ORDER"].ToString();
                    repBase.partNo = dtTemp.Rows[0]["IPN"].ToString();
                    repBase.processTime = dtTemp.Rows[0]["OUT_PROCESS_TIME"].ToString();
                }
            }

            dtTemp =await repairService.getDefect(typeSN.sn);
            if (dtTemp.Rows.Count == 0)
            {
                return ToResponse(new ApiResult((int)ResultCode.FAIL, "此SN未出现不良", repBase));
            }
            else
            {
                repBase.lineName = dtTemp.Rows[0]["LINE"].ToString();
                repBase.stationType = dtTemp.Rows[0]["STATION_TYPE"].ToString();
                repBase.stationName = dtTemp.Rows[0]["STATION_NAME"].ToString();
            }

            repBase.lvkps = await repairService.Show_KP(typeSN.sn);
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", repBase));

        }

        /// <summary>
        /// 维修Delink Remove
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairdelink:remove")]
        [HttpPost("remove")]
        public async Task<IActionResult> removeKP([FromBody] repariDel repariDel)
        {
            if (repariDel.lvkps.Count == 0)
            {
                return ToResponse(ResultCode.PARAM_ERROR, "Please select a Keypart to replace");
            }

            var liKps = repariDel.lvkps;
            string sKPSN = "";
            string sKPNO = "";
            List<string> kpsnlist = new List<string>();
            List<string> kpnolist = new List<string>();
            bool bTransOK = false;
            for (int j = 0; j < liKps.Count; j++)
            {
                sKPSN = liKps[j].ITEM_SN.ToString();           
                sKPNO = liKps[j].ITEM_SN_CUSTOMER.ToString();
                kpsnlist.Add(sKPSN);
                kpnolist.Add(sKPNO);
            }

            string[] kpsnArray = kpsnlist.ToArray();
            string[] kpnoArray = kpnolist.ToArray();
            string strkpsn = string.Join(",", kpsnArray);
            string strkpno = string.Join(",", kpnoArray);
            var userNo = HttpContext.GetName();
            bTransOK =await repairService.Remove_KP(repariDel, userNo);

            if (bTransOK)
            {
                ToResponse(new ApiResult((int)ResultCode.SUCCESS, "移除OK", null));
            }
            else
            {
                return ToResponse(new ApiResult((int)ResultCode.FAIL, "移除失败", null));
            }

            repariDel repBase = new repariDel();
            repBase.lvkps = await repairService.Show_KP(repariDel.ipn);
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", repBase));
        }

    }
}
