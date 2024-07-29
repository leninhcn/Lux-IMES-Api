using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Service.ToolingManagement.IService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Service.ToolingManagement
{
    [AppService(ServiceType = typeof(IToolingRepairService), ServiceLifetime = LifeTime.Transient)]
    public class ToolingRepairServiceImpl :BaseService<MToolingSnDefect>, IToolingRepairService
    {
        ExecuteResult exeRes = new ExecuteResult();

        public ExecuteResult GetEmp(string empNo, string site)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sqlStr =$@"SELECT A.EMP_NO FROM IMES.M_EMP A WHERE A.EMP_NO='{empNo}' and a.site = '{site}' AND A.ENABLED='Y'";
                exeRes.Anything = Context.Ado.GetDataTable(sqlStr);
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult GetToolingSnInfo(string toolingSn, string site)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sqlStr = $@"SELECT DISTINCT f.TOOLING_SN,
                                    f.TOOLING_SN_DESC,
                                    f.tooling_status,
                                    b.tooling_type,
                                    TO_CHAR (f.LAST_MAINTAIN_TIME, 'yyyymmdd') AS dt,
                                    f.enabled
                      FROM ismt.M_tooling_sn_defect a,
                           IMES.M_tooling_sn f,
                           IMES.M_tooling b
                     WHERE     a.rp_status = '1'
                           AND a.tooling_sn = f.tooling_sn
                           AND f.tooling_ID = b.ID
                            AND b.site = '{site}'
                           AND a.tooling_sn = '{toolingSn}'
                    UNION ALL
                    SELECT DISTINCT f.TOOLING_SN,
                                    f.TOOLING_SN_DESC,
                                    f.tooling_status,
                                    b.tooling_type,
                                    TO_CHAR(f.LAST_MAINTAIN_TIME, 'yyyymmdd') AS dt,
                                   f.enabled
                     FROM IMES.M_tooling_sn f, IMES.M_tooling b
                     WHERE f.TOOLING_ID = b.ID
                           AND(f.tooling_status = 'NG'
                                OR f.tooling_status = 'D'
                                OR f.tooling_status = 'I'
                                OR f.tooling_status = 'R'
                                OR f.tooling_status = 'P')
                           AND f.tooling_sn = '{toolingSn}' and b.site = '{site}'";
                exeRes.Anything = Context.Ado.GetDataTable(sqlStr);
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult GetToolingDefectInfo(string toolingSn, string site)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sqlStr = $@"SELECT DISTINCT b.defect_code,
                                        b.defect_desc2,
                                        a.rec_id,
                                        a.rp_status
                                    FROM ismt.M_tooling_sn_defect a, IMES.M_defect b
                                    WHERE     a.tooling_sn = '{toolingSn}'
                                        AND a.defect_code = b.defect_code
                                        AND a.rec_time >= (SELECT update_time1
                                                            FROM IMES.M_tooling_sn
                                                            WHERE tooling_sn = '{toolingSn}' and site = '{site}')";
                exeRes.Anything = Context.Ado.GetDataTable(sqlStr);
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult GetDefect(string DefectCode, string site)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sqlStr = $@"SELECT defect_code,DEFECT_DESC2
                                  FROM IMES.M_defect a
                                 WHERE defect_code = '{DefectCode}' and site = '{site}' AND enabled = 'Y'";
                exeRes.Anything = Context.Ado.GetDataTable(sqlStr);
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult InsertToolingSnDefect(string toolingSn, string DefectCode, string empNo, string site)
        {
            exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                Context.Ado.UseStoredProcedure().ExecuteCommandAsync("ismt.SP_tooling_defect",
                new SugarParameter[]
                  {
                    new SugarParameter("ToolingSN", toolingSn),
                    new SugarParameter("DefectCode", DefectCode),
                    new SugarParameter("Temp", empNo),
                    tRes,
                    new SugarParameter("SITE", site)
                  });
                string sRes = tRes.Value.ToString();
                if (sRes != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = sRes;
                }
                else
                {
                    exeRes.Status = true;
                    exeRes.Anything = "OK";
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult GetReson(string reason, string site)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sqlStr = $@" Select REASON_CODE, Reason_Desc From IMES.M_REASON Where Enabled = 'Y'and REASON_CODE = '{reason}'  and site = '{site}' ";
                exeRes.Anything = Context.Ado.GetDataTable(sqlStr);
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult GetToolingSnRepairData(string toolingSn, string reason, string site)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sqlStr = $@"select TOOLING_SN from ismt.M_tooling_sn_repair where  tooling_sn='{toolingSn}' and reason_CODE = '{reason}' and site = '{site}' and repair_time>=(select update_time from IMES.M_tooling_sn where tooling_sn='{toolingSn}' and site = '{site}') ";
                exeRes.Anything = Context.Ado.GetDataTable(sqlStr);
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult ToolingSnRepair(string toolingSn, string reason, string site, string remark, string empNo)
        {
            exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.sp_tooling_repair",
                    new SugarParameter[]
                    {
                        new SugarParameter("TToolingSN", toolingSn),
                        new SugarParameter("TReasonCode", reason),
                        new SugarParameter("Temp", empNo),
                        new SugarParameter("Tremark", remark),
                        tRes,
                        new SugarParameter("SITE", site)
                    });
                string sRes = tRes.Value.ToString();
                if (sRes != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = sRes;
                }
                else
                {
                    exeRes.Status = true;
                    exeRes.Anything = "OK";
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult DeleteToolingSnDefectCode(string toolingSn, string defectCode)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sql = @$"DELETE FROM ismt.M_tooling_sn_defect WHERE TOOLING_SN = '{toolingSn}' AND DEFECT_CODE = '{defectCode}'";
                Context.Ado.SqlQuery<string>(sql);
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult ToolingSnComplete(string toolingSn, string site, string empNo)
        {
            exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_TOOLING_REPAIR_COMPLETE",
                    new SugarParameter[]
                    {
                        new SugarParameter("TToolingSN", toolingSn),
                        new SugarParameter("Temp", empNo),
                        tRes,
                        new SugarParameter("SITE", site)
                    });
                string sRes = tRes.Value.ToString();
                if (sRes != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = sRes;
                }
                else
                {
                    exeRes.Status = true;
                    exeRes.Anything = "OK";
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult CheckToolingSnScrap(string toolingSn, string site)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sqlStr = $@"SELECT A.TOOLING_SN FROM IMES.M_TOOLING_SN A WHERE A.TOOLING_SN = '{toolingSn}' AND A.TOOLING_STATUS = 'S' and site = '{site}' ";
                exeRes.Anything = Context.Ado.GetDataTable(sqlStr);
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult ToolingSnScrap(string toolingSn, string empNo, string site)
        {
            exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_tooling_scrap",
                    new SugarParameter[]
                    {
                        new SugarParameter("TToolingSN", toolingSn),
                        new SugarParameter("Temp", empNo),
                        tRes,
                        new SugarParameter("SITE", site)
                    });
                string sRes = tRes.Value.ToString();
                if (sRes != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = sRes;
                }
                else
                {
                    exeRes.Status = true;
                    exeRes.Anything = "OK";
                }
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
