using Aliyun.OSS;
using Infrastructure;
using Infrastructure.Attribute;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;
using ZR.Model.Repair.Dto;
using ZR.Service.IService;
using ZR.Service.Repair.IRepairService;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class RepairOutService : BaseService<SnStatus>, IRepairOutService
    {
        DataTable dtTemp = new DataTable();
        //public async Task<string> repairIn(RepairInDto typeSN)
        //{
        //    if (string.IsNullOrEmpty(typeSN.sn))
        //    {
        //        return "序列号输入不能为空";
        //    }

        //    var dt = await selectSN(typeSN.sn, typeSN.lblType);
        //    string snno = "";
        //    string status1 = "";

        //    if (dt.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            snno = dr[0].ToString();
        //            status1 = dr[1].ToString();
        //            if (status1 == "0")
        //            {
        //                continue;
        //            }
        //            dtTemp = await getDefect(snno);
        //            if (dtTemp.Rows.Count > 0)
        //            {
        //                string id = dtTemp.Rows[0]["RECID"].ToString();
        //                lbWo.Text = dtTemp.Rows[0]["WORK_ORDER"].ToString();
        //                lbPart.Text = dtTemp.Rows[0]["IPN"].ToString();
        //                lbPdline.Text = dtTemp.Rows[0]["LINE"].ToString();
        //                lbProcess.Text = dtTemp.Rows[0]["STATION_TYPE"].ToString();
        //                lbTerminal.Text = dtTemp.Rows[0]["STATION_NAME"].ToString();
        //                lbldefectdesc.Text = dtTemp.Rows[0]["DEFECT_DESC2"].ToString();
        //                lblMoType.Text = dtTemp.Rows[0]["WO_TYPE"].ToString();
        //                lblStatus.Text = dtTemp.Rows[0]["RP_STATUS"].ToString();

        //            }
        //            else
        //            {
        //                ToResponse(ResultCode.NO_DATA, snno + " 没有不良记录，请确认!!");

        //                ErrorMSG(snno + " 没有不良记录，请确认!!");
        //                txtSN.Clear();
        //                txtSN.Focus();
        //                txtSN.SelectAll();
        //                continue;
        //            }
        //            DataTable black2 = await getRepaired(snno);
        //            if (black2.Rows.Count > 0)
        //            {
        //                int sum = Convert.ToInt32(black2.Rows[0]["COUNT"].ToString());
        //                if (sum > 3)
        //                    MessageBox.Show(snno + "已维修过" + sum + "次，请确认是否需要报废");
        //            }

        //            DataTable black1 = await getRepair(snno);
        //            if (black1.Rows.Count > 0)
        //            {
        //                string status = black1.Rows[0]["REPAIR_FLAG"].ToString();

        //                if (status == "N")
        //                {
        //                    ErrorMSG(snno + " 已经进入待维修状态 Check In，请进行维修执行");

        //                    txtSN.Clear();
        //                    txtSN.Focus();
        //                    txtSN.SelectAll();
        //                    continue;
        //                }
        //            }
        //            else
        //            {
        //                ErrorMSG(snno + " 未进入维修区");
        //            }

        //            DataTable black = await getHold(snno);
        //            if (black.Rows.Count > 0)
        //            {
        //                ErrorMSG(snno + " 所有站点已被HOLD，请联系QA解HOLD");
        //                txtSN.Clear();
        //                txtSN.Focus();
        //                txtSN.SelectAll();
        //                continue;
        //            }
        //            string stationtype = lbProcess.Text.ToString().Trim();
        //            DataTable spimanager = await getSPI(snno, stationtype);
        //            if (spimanager.Rows.Count > 0)
        //            {
        //                ErrorMSG(snno + " 是SMT SPI FAIL的主板，请走SPIRepair维修");
        //                txtSN.Clear();
        //                txtSN.Focus();
        //                txtSN.SelectAll();
        //                continue;
        //            }
        //            exeRes =await CheckSN(snno);
        //            if (!exeRes.Status)
        //            {
        //                ErrorMSG(exeRes.Message);
        //                txtSN.Clear();
        //                txtSN.Focus();
        //                txtSN.SelectAll();
        //                continue;
        //            }
        //            else
        //            {
        //                exeRes = await RepairSn(baseInfo, snno);
        //                if (!exeRes.Status)
        //                {
        //                    txtSN.Clear();
        //                    txtSN.Focus();
        //                    txtSN.SelectAll();
        //                    ErrorMSG(snno + exeRes.Message);
        //                    continue;
        //                }
        //                else
        //                {
        //                    txtSN.Clear();
        //                    txtSN.Focus();
        //                    txtSN.SelectAll();
        //                    SuccessMSG(snno + "进入维修区 CheckIn OK，请进行维修执行");
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ErrorMSG(label1.Text + " : " + txtSN.Text + " 数据不存在");
        //        return;
        //    }
        //}

        public async Task<DataTable> selectSN(string sn, string type)
        {
            var ret = await Context.Queryable<SnStatus>()
                            .WhereIF(type == "SN", x => x.SerialNumber == sn)
                            .WhereIF(type != "SN", x => x.PanelNo == sn)
                            .Select(x => new { x.SerialNumber, x.CurrentStatus }).ToDataTableAsync();

            return ret;
        }

        public async Task<DataTable> getDefect(string sn)
        {
            string sSQL = @" SELECT A.RECID,a.work_order,a.ipn,
                             A.Location,
                             decode(A.RP_STATUS,'0','维修完成','','待维修') RP_STATUS,
                             B.DEFECT_CODE,
                             B.DEFECT_DESC,
                             B.DEFECT_DESC2,
                             a.LINE,
                             a.station_name,
                             a.station_type,h.wo_type,
                             NVL (G.REASON_CODE, 'N/A') REASON_CODE
                        FROM SAJET.p_SN_DEFECT A,
                             imes.m_DEFECT   B,
                             imes.p_SN_REPAIR F,
                             imes.m_REASON   G,
                             imes.p_wo_base h
                           WHERE     A.SERIAL_NUMBER = '" + sn + "'AND a.work_order=h.work_order AND NVL (rp_status, 1) <> '0' AND A.DEFECT_code = B.DEFECT_code(+) AND A.RECID = F.RECID(+) AND F.REASON_CODE = G.REASON_CODE(+) ORDER BY A.REC_TIME ";
            return await Context.Ado.GetDataTableAsync(sSQL);


        }

        public async Task<DataTable> getRepaired(string sn)
        {
            string getRepaired = string.Format(@"SELECT NVL(MAX(COUNT(*)),0) COUNT FROM  (SELECT T.REC_TIME,T.STATION_TYPE    FROM SAJET.P_SN_DEFECT T  WHERE T.SERIAL_NUMBER = '{0}' AND T.RP_STATUS = '0' GROUP BY T.REC_TIME,T.STATION_TYPE) AA  GROUP BY AA.STATION_TYPE", sn);

            return await Context.Ado.GetDataTableAsync(getRepaired);
        }

        public async Task<DataTable> getHold(string sn)
        {
            string getHold = string.Format(@"SELECT * FROM SAJET.p_hold_sn A WHERE A.SN = '{0}' and a.station_type='*' and a.enabled = 'Y' and a.unhold_empno is null", sn);
            return await Context.Ado.GetDataTableAsync(getHold);
        }

        public async Task<string> CheckSN(string sn)
        {
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_SN_PSN",
                new SugarParameter[]
                    {
                    new SugarParameter("TREV", sn),
                    new SugarParameter("PSN", null,true),
                    tRes
                    });

                return tRes.Value.ToString();
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        public async Task<string> RepairSn(RepairInDto typeSN, string sn, string _userNo)
        {
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                var rDt = await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_REPAIR_SN_IN_TIME",
                     new SugarParameter[]
                     {
                    new SugarParameter("TREV", sn),
                    new SugarParameter("TEMPNO", _userNo),
                    new SugarParameter("T_STATIONNAME", typeSN.stationName),
                    new SugarParameter("T_STATIONTYPE", typeSN.stationType),
                    new SugarParameter("T_PDLINE", typeSN.lineName),
                    new SugarParameter("T_STAGE", typeSN.stationName),
                    tRes
                     });

                return tRes.Value.ToString();
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        public async Task<DataTable> checkstatus(string sn)
        {
            string getHold = string.Format(@" SELECT  *  FROM SAJET.p_sn_status WHERE serial_number= '{0}' ", sn);
            return await Context.Ado.GetDataTableAsync(getHold);
        }

        public async Task<string> RepairSnOut(RepairInDto repOut,string _userNo,string strProcess)
        {
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_REPAIR_SN_OUT_TIME",
                new SugarParameter[]
                    {
                    new SugarParameter("TREV", repOut.sn),
                    new SugarParameter("T_DEFECT_STATION", strProcess),
                    new SugarParameter("TEMPNO", _userNo),
                    new SugarParameter("T_STATIONNAME", repOut.stationName),
                    new SugarParameter("T_STATIONTYPE", repOut.stationType),
                    new SugarParameter("T_PDLINE", repOut.lineName),
                    new SugarParameter("T_STAGE", repOut.stageName),
                    tRes
                    });

                return tRes.Value.ToString();
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }
    }
}