using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using ZR.Model.Repair.Dto;
using ZR.Service.Repair.IRepairService;

namespace ZR.Admin.WebApi.Controllers.Repair
{
    /// <summary>
    /// 进维修
    /// </summary>
    //[Verify]
    [Route("repair/repairin")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class RepairINController : BaseController
    {
        public IRepairINService repairService;
        public RepairINController(IRepairINService _repairService)
        {
            repairService = _repairService;
        }

        /// <summary>
        /// 进维修
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairin:list")]
        [HttpPost("List")]
        public async Task<IActionResult> List([FromBody] RepairInDto typeSN)
        {
            if (string.IsNullOrEmpty(typeSN.sn))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "序列号输入不能为空");
            }

            DataTable dtTemp = new DataTable();
            var dt =await repairService.selectSN(typeSN.sn, typeSN.lblType);
            string snno = "";
            string status1 = "";

            RepairBaseInfo repInfo = new  RepairBaseInfo();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    snno = dr[0].ToString();
                    status1 = dr[1].ToString();
                    if (status1 == "0")
                    {
                        return ToResponse(ResultCode.FAIL, "序列号状态错误");
                        /*continue*/
                        ;
                    }
                    dtTemp = await repairService.getDefect(snno);
                    if (dtTemp.Rows.Count > 0)
                    {
                        string id = dtTemp.Rows[0]["RECID"].ToString();
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

                        /*continue*/;
                    }
                    DataTable black2 = await repairService.getRepaired(snno);
                    if (black2.Rows.Count > 0)
                    {
                        int sum = Convert.ToInt32(black2.Rows[0]["COUNT"].ToString());
                        if (sum > 3)
                            return ToResponse(ResultCode.SUCCESS, snno + "已维修过" + sum + "次，请确认是否需要报废");
                    }

                    DataTable black1 = await repairService.getRepair(snno);
                    if (black1.Rows.Count > 0)
                    {
                        string status = black1.Rows[0]["REPAIR_FLAG"].ToString();

                        if (status == "N")
                        {
                            return ToResponse(ResultCode.FAIL, snno + " 已经进入待维修状态 Check In，请进行维修执行");
                            /*continue*/;
                        }
                    }
                    else
                    {
                         ToResponse(ResultCode.FAIL, snno + " 未进入维修区");
                    }

                    DataTable black = await repairService.getHold(snno);
                    if (black.Rows.Count > 0)
                    {
                        return ToResponse(ResultCode.FAIL, snno + " 所有站点已被HOLD，请联系QA解HOLD");
                        /*continue*/;
                    }
                    string stationtype = repInfo.stationType.ToString().Trim();
                    DataTable spimanager = await repairService.getSPI(snno, stationtype);
                    if (spimanager.Rows.Count > 0)
                    {
                        return ToResponse(ResultCode.FAIL, snno + " 是SMT SPI FAIL的主板，请走SPIRepair维修");
                       // continue;
                    }
                    string eRes = await repairService.CheckSN(snno);

                    if (eRes != "OK")
                    {
                        return ToResponse(ResultCode.FAIL, eRes);
                       // continue;
                    }
                    else
                    {
                        var _userNo =  HttpContext.GetName();
                        eRes = await repairService.RepairSn(typeSN, snno, _userNo);

                        if (eRes != "OK")
                        {
                            return ToResponse(ResultCode.FAIL, eRes);
                        }
                        else
                        {
                          return  SUCCESS(snno + "进入维修区 CheckIn OK，请进行维修执行");
                        }
                    }
                }
            }
            else
            {
               return ToResponse(ResultCode.FAIL, typeSN.lblType + " : " + typeSN.sn + " 数据不存在");
            }

             return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", repInfo));
        }

    }
}
