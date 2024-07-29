using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using ZR.Model.Repair.Dto;
using ZR.Service.Repair.IRepairService;

namespace ZR.Admin.WebApi.Controllers.Repair
{
    /// <summary>
    /// 出维修
    /// </summary>
    //[Verify]
    [Route("repair/repairout")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class RepairOutController : BaseController
    {
        public IRepairOutService repairService;
        public RepairOutController(IRepairOutService _repairService)
        {
            repairService = _repairService;
        }

        /// <summary>
        /// 出维修
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairout:list")]
        [HttpPost("List")]
        public async Task<IActionResult> List([FromBody] RepairInDto typeSN)
        {
            if (string.IsNullOrEmpty(typeSN.sn))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "序列号输入不能为空");
            }

            DataTable dtTemp = new DataTable();
            var dt = await repairService.selectSN(typeSN.sn, typeSN.lblType);
            string snno = "";
            string status1 = "";

            RepairBaseInfo repInfo = new RepairBaseInfo();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    snno = dr[0].ToString();
                    status1 = dr[1].ToString();
                    if (status1 == "0")
                    {
                        //  return ToResponse(ResultCode.FAIL, "序列号状态错误");
                        continue;
                    }
                    dtTemp = await repairService.getDefect(snno);
                    if (dtTemp.Rows.Count > 0)
                    {
                        repInfo.wo = dtTemp.Rows[0]["WORK_ORDER"].ToString();
                        repInfo.ipn = dtTemp.Rows[0]["IPN"].ToString();
                        repInfo.lineName = dtTemp.Rows[0]["LINE"].ToString();
                        repInfo.stationType = dtTemp.Rows[0]["STATION_TYPE"].ToString();   //process
                        repInfo.stationName = dtTemp.Rows[0]["STATION_NAME"].ToString();    //lbTerminal
                        repInfo.defectDesc2 = dtTemp.Rows[0]["DEFECT_DESC2"].ToString();   //lbldefectdesc
                        repInfo.woType = dtTemp.Rows[0]["WO_TYPE"].ToString();
                        repInfo.rpStatus = dtTemp.Rows[0]["RP_STATUS"].ToString();   //lblStatus

                    }
                    else
                    {
                        return ToResponse(ResultCode.NO_DATA, snno + " 没有不良记录，请确认!!");

                        /*continue*/
                        ;
                    }
                    DataTable black2 = await repairService.getRepaired(snno);
                    if (black2.Rows.Count > 0)
                    {
                        int sum = Convert.ToInt32(black2.Rows[0]["COUNT"].ToString());
                        if (sum > 3)
                            return ToResponse(ResultCode.SUCCESS, snno + "已维修过" + sum + "次，请确认是否需要报废");
                    }

                    DataTable black = await repairService.getHold(snno);
                    if (black.Rows.Count > 0)
                    {
                        return ToResponse(ResultCode.FAIL, snno + " 所有站点已被HOLD，请联系QA解HOLD");
                        /*continue*/
                        ;
                    }
                    DataTable white = await repairService.checkstatus(snno);
                    if (white.Rows.Count > 0)
                    {
                        string status = white.Rows[0]["CURRENT_STATUS"].ToString();
                        if (status == "0")
                        {
                            return ToResponse(ResultCode.FAIL, snno + " 状态正常，请确认!!");
                            //continue;
                        }
                        if (status == "1")
                        {
                            return ToResponse(ResultCode.FAIL, snno + " 没有进行 CheckIn");
                           // continue;
                        }
                        if (status == "5")
                        {
                            return ToResponse(ResultCode.FAIL, snno + " 进入维修执行");
                        }
                    }
                    else
                    {
                        return ToResponse(ResultCode.FAIL, snno + " 不存在");
                    }

                    string eRes = await repairService.CheckSN(snno);

                    if (eRes != "OK")
                    {
                        return ToResponse(ResultCode.FAIL, eRes);
                        // continue;
                    }
                    else
                    {
                        var _userNo = HttpContext.GetName();
                        eRes =await repairService.RepairSnOut(typeSN, _userNo, repInfo.stationType);

                        eRes = await repairService.RepairSn(typeSN, snno, _userNo);

                        if (eRes != "OK")
                        {
                            return ToResponse(ResultCode.FAIL, eRes);
                        }
                        else
                        {
                            return SUCCESS(snno + " 维修完成");
                        }
                    }
                }
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", repInfo));
        }

    }
}
