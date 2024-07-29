using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Service.Repair.IRepairService;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class RepPrintlabService : BaseService<SnStatus>, IRepPrintlabService
    {

        public async Task<ExecuteResult> checksn(string sn)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                exeRes = new ExecuteResult();
                string sqlStr = @"  SELECT * FROM IMES.p_sn_status A WHERE A.serial_number = @sn ";

                DataTable dt = await Context.Ado.GetDataTableAsync(sqlStr, new List<SugarParameter>
                    { new SugarParameter("@sn", sn)
                    });

                if (dt.Rows.Count == 0)
                {
                    exeRes.Status = false;
                }
                else
                {
                    exeRes.Anything = dt;
                    exeRes.Status = true;
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> checkprintlog(string sn, string labeltype)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlStr = @"  SELECT * FROM IMES.P_SN_PRINT_LOG A WHERE A.serial_number = @sn and A.LABEL_TYPE = @labeltype";

                DataTable dt = await Context.Ado.GetDataTableAsync(sqlStr, new List<SugarParameter>
                 {
                     new SugarParameter("@sn", sn),
                     new SugarParameter("@labeltype", labeltype)
                    });

                if (dt.Rows.Count == 0)
                {
                    exeRes.Status = false;
                }
                else
                {
                    exeRes.Anything = dt;
                    exeRes.Status = true;
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> checkrepairin(string sn)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlStr = @"  SELECT *
                                      FROM IMES.P_REPAIR_IN A
                                     WHERE A.SERIAL_NUMBER = @sn
                                     ORDER BY A.CREATE_TIME DESC ";

                DataTable dt = await Context.Ado.GetDataTableAsync(sqlStr, new List<SugarParameter>
                 {
                     new SugarParameter("@sn", sn)
                  });
                exeRes.Anything = dt;
                exeRes.Status = true;
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["REPAIR_FLAG"].ToString() != "N")
                    {
                        exeRes.Message = sn + ",SN 没有Repair In,请检查";
                        exeRes.Status = false;
                    }
                }
                else
                {
                    exeRes.Message = sn + ",SN 没有Repair In,请检查";
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

        public async Task<ExecuteResult> chekprintrole(string empno,string pwd)
        {
            ExecuteResult exeRes = new ExecuteResult();
            string sSQL = $@"Select ID,trim(imes.password.decrypt(PASSWD)) PWD from imes.M_EMP  
                          where emp_no = '{empno}' ";
            DataTable dt = await Context.Ado.GetDataTableAsync(sSQL);
            if (dt.Rows.Count == 0)
            {
                exeRes.Message = "Employee Error";
                exeRes.Status = false;
            }else if ( pwd != dt.Rows[0]["PWD"].ToString())
            {
                exeRes.Message = "Passwd Error";
                exeRes.Status = false;
            }else
            {
                exeRes.Anything = dt.Rows[0]["ID"].ToString();
                exeRes.Status = true;
            }
          
            return exeRes;
        }

        public async Task<ExecuteResult> GetLabelInfo(string SN, string StationType)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlStr = @"  SELECT a.IPN,B.LABEL_TYPE, B.LABEL_NAME, B.LABEL_SRV_IP, B.LABEL_DL_URL
                                      FROM IMES.P_SN_STATUS A, IMES.M_STATIONTYPE_LABEL B
                                     WHERE A.IPN = B.IPN
                                       AND A.SERIAL_NUMBER = @SN
                                       AND B.STATION_TYPE =@StationType ";
                object[] para = new object[] { SN, StationType };

                DataTable dt = await Context.Ado.GetDataTableAsync(sqlStr, new List<SugarParameter>
                 {
                     new SugarParameter("@SN", SN),
                     new SugarParameter("@StationType", StationType)
                  });

                exeRes.Anything = dt;
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> GetLabelInfo(string StationType)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlStr = @"  SELECT A.IPN, A.LABEL_TYPE, A.LABEL_NAME, A.LABEL_SRV_IP, A.LABEL_DL_URL
                                      FROM IMES.M_STATIONTYPE_LABEL A
                                     WHERE A.STATION_TYPE = @StationType
                                       AND A.ENABLED = 'Y'
                                       AND A.IPN = '*' ";
                DataTable dt = await Context.Ado.GetDataTableAsync(sqlStr, new List<SugarParameter>
                 {
                     new SugarParameter("@StationType", StationType)
                  });
                exeRes.Anything = dt;
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

    }
}
