using Infrastructure.Attribute;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Service.Repair.IRepairService;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class FAMaintainMService : BaseService<SnStatus>, IFAMaintainMService
    {
        public async Task<ExecuteResult> getLine()
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlstr = "SELECT ID,LINE  FROM  imes.m_line where enabled = 'Y'";
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlstr);

                exeRes.Status = true;
                exeRes.Anything = dtTemp;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> getDefectType()
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlstr = "SELECT DISTINCT DEFECT_CODE,DEFECT_DESC,DEFECT_DESC2 FROM  IMES.M_DEFECT";
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlstr);

                exeRes.Status = true;
                exeRes.Anything = dtTemp;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> getShowData()
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlstr = $@"
SELECT A.*,B.EMP_NAME,G.EMP_NAME AS FA_EMP_NAME,C.DEFECT_DESC,D.PDLINE_NAME,E.STATION_TYPE,F.STATION_NAME,  
  I.EMP_NAME AS OUT_EMP_NAME ,S.WORK_ORDER,NVL(WO.WO_OPTION5,'') AS WORKMSG    
FROM   IMES.P_MAINTAIN_SN A 
   left join IMES.M_EMP  B on A.EMP_NO = B.EMP_NO  AND A.SITE = B.SITE  AND A.SITE = 'DEF'
   LEFT JOIN IMES.M_EMP G ON A.FA_EMP_NO = G.EMP_NO AND G.SITE = 'DEF'
    LEFT JOIN IMES.M_DEFECT C ON A.DEFECT_CODE = C.DEFECT_CODE 
    LEFT JOIN IMES.M_LINE D ON A.PDLINE = D.LINE
    LEFT JOIN IMES.M_STATION_TYPE E ON A.STATION_TYPE = E.STATION_TYPE 
    LEFT JOIN IMES.M_STATION F ON A.STATION_NAME = F.STATION_NAME    
    LEFT JOIN IMES.M_EMP  I ON A.MAINTAIN_EMP_NO = I.EMP_NO  
    LEFT JOIN IMES.P_SN_STATUS S ON (S.SERIAL_NUMBER=A.SERIAL_NUMBER OR S.CUSTOMER_SN=A.SERIAL_NUMBER ) 
    LEFT JOIN IMES.P_WO_BASE WO ON WO.WORK_ORDER=S.WORK_ORDER
     WHERE A.ENABLED_FLAG = 'Y'  
";

     //           --AND DATA_TYPE = '{_dataType}'
     //AND A.IN_TIME BETWEEN TO_DATE('{dtStart}', 'yyyy-mm-dd hh24:mi:ss')
     //AND TO_DATE('{dtEnd}', 'yyyy-mm-dd hh24:mi:ss')

                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlstr);

                exeRes.Status = true;
                exeRes.Anything = dtTemp;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> getModel()
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlstr = @" SELECT C.STATION_TYPE  
                FROM  imes.M_ROUTE A
                  JOIN imes.M_ROUTE_DETAIL B ON A.ROUTE_NAME = B.ROUTE_NAME AND B.STEP= B.SEQ AND A.SITE = B.SITE AND A.SITE = 'DEF'
                  JOIN imes.M_STATION_TYPE C ON B.NEXT_STATION_TYPE = C.STATION_TYPE AND B.SITE = C.SITE
                  WHERE A.ROUTE_DESC = 'FA Analysis' AND A.ROUTE_NAME LIKE '%FA%'
                  ORDER BY B.SEQ ";
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlstr);

                exeRes.Status = true;
                exeRes.Anything = dtTemp;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> GetEmpInfoByNo(string empno)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlstr = $@" SELECT ID,EMP_NO,EMP_NAME,SITE  FROM  IMES.M_EMP WHERE ENABLED = 'Y' AND SITE = 'DEF' AND EMP_NO='{empno}' ";
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlstr);

                exeRes.Status = true;
                exeRes.Anything = dtTemp;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }
        public async Task<ExecuteResult> GetDefectByCode(string defCode)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlstr = $@" SELECT ID,DEFECT_CODE,DEFECT_DESC,DEFECT_DESC2  FROM  IMES.M_DEFECT where enabled= 'Y' AND SITE = 'DEF' AND DEFECT_CODE = '{defCode}' ";
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlstr);

                exeRes.Status = true;
                exeRes.Anything = dtTemp;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        //public async Task<ExecuteResult> appendData(string sn,string defCode)
        //{
        //    string sMaxID = SajetCommon.GetMaxID("SAJET.G_MAINTAIN_SN", "MAINTAIN_ID", 5);
        //}


        //public async Task<string> GetSN(string trev, out string sn)
        //{
        //    try
        //    {
        //        object[][] procParams = new object[3][];
        //        procParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", trev };
        //        procParams[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
        //        procParams[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "PSN", "" };
        //        DataTable dt = ClientUtils.ExecuteProc("SAJET.SJ_CKRT_SN_PSN", procParams).Tables[0];
        //        string msg = dt.Rows[0]["TRES"].ToString();
        //        sn = dt.Rows[0]["PSN"].ToString();
        //        if (dt.Rows.Count > 0 && msg.Substring(0, 2) == "OK")
        //        {
        //            return true;
        //        }

        //        return false;
        //    }
        //    catch (Exception ex) { sn = string.Empty; return false; }
        //}

    }
}
