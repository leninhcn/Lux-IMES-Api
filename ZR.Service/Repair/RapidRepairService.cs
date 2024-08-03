using Infrastructure.Attribute;
using JinianNet.JNTemplate;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;
using static System.Collections.Specialized.BitVector32;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class RapidRepairService : BaseService<SnStatus>, IRapidRepairService
    {
        public async Task<DataTable> checkstatus(string sn)
        {
            string getHold = string.Format(@" SELECT * FROM SAJET.p_sn_status WHERE serial_number= '{0}'  AND CURRENT_STATUS = '1' ", sn);
            return await Context.Ado.GetDataTableAsync(getHold);
        }

        public async Task<DataTable> getDetail(string snno)
        {
            string sSQL = @"select a.serial_number, a.work_order, a.version,a.ipn, a.line, a.station_type, a.station_name, a.out_stationtype_time,a.wip_station_type,a.route_name FROM SAJET.p_sn_status A
                             
                             WHERE A.CURRENT_STATUS = '1'
                             and a.serial_number='" + snno + "'";

            return await Context.Ado.GetDataTableAsync(sSQL);

        }
        public async Task<DataTable> getDefect1(string sNInfo)
        {
            string sSQL = "";
            sSQL = @"SELECT A.RECID,A.Location, A.RP_STATUS,NVL(A.DEFECT_CODE||'|'||B.DEFECT_DESC ,'N/A') DEFECT_CODE ,B.DEFECT_CODE DEFECT_CODE1, B.DEFECT_DESC2,a.LINE,a.station_name,a.station_type                               
                            FROM SAJET.p_SN_DEFECT A,imes.m_DEFECT   B                                                            
                           WHERE  A.SERIAL_NUMBER = '" + sNInfo + @"' AND NVL (rp_status, 1) <> '0' AND  A.DEFECT_code = B.DEFECT_code(+)  ORDER BY A.REC_TIME";
            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<ExecuteResult> CheckIsErrorCode(string errorcode)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlstr = "SELECT DEFECT_CODE,DEFECT_DESC FROM SAJET.M_DEFECT WHERE DEFECT_CODE='" + errorcode + "' AND ENABLED='Y' AND ROWNUM=1";
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlstr);

                if (dtTemp.Rows.Count > 0)
                {
                    string defectcode = dtTemp.Rows[0]["DEFECT_CODE"].ToString();
                    string defectdesc = dtTemp.Rows[0]["DEFECT_DESC"].ToString();
                    exeRes.Message = "Defect Code Input OK. 此不良描述为:" + defectdesc;
                    exeRes.Status = true;
                }
                else
                {
                    exeRes.Message = "不良代码不存在，请重新选择";
                    exeRes.Status = false;
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }

            return exeRes;
        }

        public async Task<ExecuteResult> RapidRepairGo(string station, string sn, string nstation,string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var sRes = new SugarParameter("TRES", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_SPIRepair_GO_CHECK",
                new SugarParameter[]
                    {
                    new SugarParameter("T_STATION_NAME", station),
                    new SugarParameter("TSN", sn),
                    new SugarParameter("TEMPNO", userNo),
                    new SugarParameter("T_NEXT_STATION_TYPE", nstation),
                    sRes
                    });
                if (sRes.Value.ToString() != "SPI Repair OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = "批量维修不成功";
                }
                else
                {
                    exeRes.Status = true;
                    exeRes.Message = sRes.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }

            return exeRes;
        }

        public async Task<bool> AddRapidRepair(RapidRequst rapid,string userNo)
        {
            string ID = await GetMaxID();
            string sSQL = @"INSERT INTO SAJET.P_SPI_REPAIR ( ID,SERIAL_NUMBER,WORK_ORDER, IPN,LINE, STATION_TYPE,STATION_NAME, DEFECT_CODE, REPAIR_EMPNO, REMARK ) 
                           VALUES(@ID,@SERIAL_NUMBER,@WORK_ORDER,@IPN,@LINE,@STATION_TYPE,@STATION_NAME,@DEFECT_CODE,@REPAIR_EMPNO,@REMARK)";
           
            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@ID", ID),
                new SugarParameter("@SERIAL_NUMBER", rapid.sn),
                new SugarParameter("@WORK_ORDER",rapid.wo),
                new SugarParameter("@IPN", rapid.parnNo),
                new SugarParameter("@LINE",rapid.line),
                new SugarParameter("@STATION_TYPE", rapid.stationType),
                new SugarParameter("@STATION_NAME", rapid.stationName),
                new SugarParameter("@DEFECT_CODE", rapid.defectCode),
                new SugarParameter("@REPAIR_EMPNO",userNo),
                new SugarParameter("@REMARK",rapid.memo)
            });

            return affected > 0;
        }

        private async Task<string> GetMaxID()
        {
            string sMaxID = "0";
            try
            {
                var sRes = new SugarParameter("TRES", null, true);
                var sMax = new SugarParameter("T_MAXID", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_GET_MAXID",
                new SugarParameter[]
                    {
                    new SugarParameter("TFIELD", "ID"),
                    new SugarParameter("TTABLE", "SAJET.P_SPI_REPAIR"),
                    new SugarParameter("TNUM", 8),
                    sRes,
                    sMax
                    });

                if (sRes.Value.ToString() != "OK")
                {
                    return "0";
                }
                sMaxID = sMax.Value.ToString();
            }
            catch (Exception)
            {
                return "0";
            }
            return sMaxID;
        }

    }
}
