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
using static System.Collections.Specialized.BitVector32;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class RepairINService : BaseService<SnStatus>, IRepairINService
    {
        DataTable dtTemp = new DataTable();
       
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
                        FROM imes.p_SN_DEFECT A,
                             imes.m_DEFECT   B,
                             imes.p_SN_REPAIR F,
                             imes.m_REASON   G,
                             imes.p_wo_base h
                           WHERE     A.SERIAL_NUMBER = '" + sn + "'AND a.work_order=h.work_order AND NVL (rp_status, 1) <> '0' AND A.DEFECT_code = B.DEFECT_code(+) AND A.RECID = F.RECID(+) AND F.REASON_CODE = G.REASON_CODE(+) ORDER BY A.REC_TIME ";
            return await Context.Ado.GetDataTableAsync(sSQL);
            

        }

        public async Task<DataTable> getRepaired(string sn)
        {
            string getRepaired = string.Format(@"SELECT NVL(MAX(COUNT(*)),0) COUNT FROM  (SELECT T.REC_TIME,T.STATION_TYPE    FROM IMES.P_SN_DEFECT T  WHERE T.SERIAL_NUMBER = '{0}' AND T.RP_STATUS = '0' GROUP BY T.REC_TIME,T.STATION_TYPE) AA  GROUP BY AA.STATION_TYPE", sn);

            return await Context.Ado.GetDataTableAsync(getRepaired);
        }

        public async Task<DataTable> getRepair(string sn)
        {
            string getRepair = string.Format(@"SELECT * FROM imes.p_repair_in WHERE SERIAL_NUMBER = '{0}' ORDER BY CREATE_TIME DESC", sn);

            return await Context.Ado.GetDataTableAsync(getRepair);
        }

        public async Task<DataTable> getHold(string sn)
        {
            string getHold = string.Format(@"SELECT * FROM imes.p_hold_sn A WHERE A.SN = '{0}' and a.station_type='*' and a.enabled = 'Y' and a.unhold_empno is null", sn);
            return await Context.Ado.GetDataTableAsync(getHold);
        }

        public async Task<DataTable> getSPI(string sn, string stationtype)
        {
            string getHold = string.Format(@"select * from imes.p_sn_status A  WHERE A.SERIAL_NUMBER= '{0}' and  instr('" + stationtype + "', 'SPI',-1,1)>0  AND A.CURRENT_STATUS = '1'", sn);
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
               return  "Error:" + ex.Message;
            }
        }

        public async Task<string> RepairSn(RepairInDto typeSN, string sn,string _userNo)
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

    }
}
