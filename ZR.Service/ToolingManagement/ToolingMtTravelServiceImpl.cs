using Infrastructure.Attribute;
using JinianNet.JNTemplate;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Model.Dto.Tooling;
using ZR.Service.ToolingManagement.IService;

namespace ZR.Service.ToolingManagement
{
    [AppService(ServiceType = typeof(IToolingMtTravelService), ServiceLifetime = LifeTime.Transient)]
    public class ToolingMtTravelServiceImpl : BaseService<PToolingMtTravel>, IToolingMtTravelService
    {
        ExecuteResult exeRes;
        public ImesMemp SelectEmpByNo(string empno, string site)
        {
            var exp = Expressionable.Create<ImesMemp>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.And(it => it.empNo == empno);
            return Context.Queryable<ImesMemp>().Where(exp.ToExpression()).First();
        }



        public ExecuteResult GetDateATime(string Tooling)
        {
            exeRes = new ExecuteResult();
            try
            {

                string sqlStr = @"SELECT TO_CHAR (MAX (A.UPDATE_TIME1), 'yyyy-mm-dd hh24:mi:ss')       update_time,
                                 A.TOOLING_SN,
                                 B.MAINTAIN_TIME
                            FROM imes.m_TOOLING_SN_ht a
                                 LEFT JOIN IMES.M_TOOLING B ON A.TOOLING_ID = B.ID
                           WHERE A.TOOLING_SN = '" + Tooling + @"' AND A.TOOLING_STATUS IN ( 'I','P','D','M')
                        GROUP BY A.TOOLING_SN, B.MAINTAIN_TIME";
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

        public ExecuteResult GetDay(string time)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sqlStr = @"SELECT trunc((sysdate- to_date('" + time + "','YYYY/MM/DD HH24:mi:ss'))*24) OVER_TIME from dual";
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

        public ExecuteResult CgToolingDEC(string Tooling)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sql = "UPDATE IMES.M_TOOLING_SN SET TOOLING_SN_DESC = '超时' WHERE TOOLING_SN ='" + Tooling + "'";
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

        public ExecuteResult GetUsedCount(string Tooling)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sqlStr = @"SELECT TOTAL_USED_COUNT FROM IMES.M_TOOLING_SN WHERE TOOLING_SN = '" + Tooling + "'";
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

        public ExecuteResult GetMAXCount(string Tooling)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sqlStr = @"SELECT NVL (B.WARN_UESD_TIMES, 0), NVL (B.MAX_USE_TIMES, 0), NVL (B.MAX_USE_DAY, 0),  NVL (B.WARN_USED_DAY, 0)  
                                FROM IMES.M_TOOLING_SN A, IMES.M_TOOLING B  WHERE A.TOOLING_SN ='" + Tooling + "'  AND A.TOOLING_ID = B.ID  AND ROWNUM = 1";
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

        public ExecuteResult ChangeLStatus(string toolingsn, string status,string empno ,string site)
        {
            exeRes = new ExecuteResult();

            try
            {

                string sql = "UPDATE IMES.M_TOOLING_LOAD SET STATUS = '" + status + "' , UPDATE_EMPNO = '" + empno + "',UPDATE_TIME = SYSDATE WHERE TOOLING_SN = '" + toolingsn + "' AND SITE = '" + site + "'";
                Context.Ado.SqlQuery<string>(sql);


                sql = "INSERT INTO IMES.M_HT_TOOLING_LOAD (SELECT * FROM IMES.M_TOOLING_LOAD  WHERE TOOLING_SN = '" + toolingsn + "' AND SITE = '" + site + "')";
                Context.Ado.SqlQuery<string>(sql);
                exeRes.Status= true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public int UpdateInfo(MToolingSn toolingsn)
        {
            int updateType = Context.Updateable<MToolingSn>()
                .SetColumns(_ => new MToolingSn
                {
                    LastMaintainTime = toolingsn.LastMaintainTime,
                    UsedCount = toolingsn.UsedCount,
                    ToolingStatus = toolingsn.ToolingStatus,
                    UpdateEmpNo1 = toolingsn.UpdateEmpNo1,
                    UpdateTime1 = toolingsn.UpdateTime1
                }).Where(l => l.ToolingSnId == toolingsn.ToolingSnId).ExecuteCommand();

            if (updateType > 0)
            {
                string sqlStr = $"INSERT INTO IMES.M_TOOLING_SN_HT(SELECT * FROM IMES.M_TOOLING_SN WHERE TOOLING_SN_ID =  " + toolingsn.ToolingSnId + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return 0;
        }

        public ExecuteResult INSERTDATA( ToolingMtTravelDto dto)
        {
            try
            {
                exeRes = new ExecuteResult();
                string sql = @"INSERT INTO IMES.P_TOOLING_MT_TRAVEL (TOOLING_SN,
                                       UPDATE_EMPNO, 
                                       UPDATE_TIME, 
                                       PRIOR_MAINTAIN_TIME,
                                       USED_COUNT,
                                       TOTAL_USED_COUNT,
                                       MAINTAIN_ITEM,
                                       MAINTAIN_RESULT, 
                                       DAMAGE_DEGREE,
                                       CLEAN_DEGREE,
                                       ROUGH,
                                       SCRAPE, 
                                       VIEWCHECK,SITE)
                                   SELECT TOOLING_SN,
                                          '" + dto.UpdateEmpNo + @"',
                                          SYSDATE AS UPDATE_TIME, 
                                          LAST_MAINTAIN_TIME,
                                          USED_COUNT,
                                          TOTAL_USED_COUNT, 
                                          '目檢', 
                                            '" + dto.MaintainResult + @"', 
                                            '" + dto.DamageDegree + @"', 
                                            '" + dto.CleanDegree + @"', 
                                            '" + dto.Rough + @"', 
                                            '" + dto.Scrape + @"', 
                                            '" + dto.ViewCheck + @"',
                                            '" + dto.Site + @"'
                                     FROM IMES.M_TOOLING_SN
                                    WHERE TOOLING_SN = '" + dto.ToolingSn + "' AND SITE = '"+ dto.Site+ "'";
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

        public async Task<List<ToolingMtVo>> GetMaintainResult(string toolingSn)
        {
            return await Context.Queryable<MToolingSn, MTooling>((a, b) => new JoinQueryInfos(
                JoinType.Left, a.ToolingId == b.Id 
                ))
                .Where((a, b) => a.Enabled == "Y" && a.ToolingSn == toolingSn)
                .Select((a, b) => new ToolingMtVo
                {
                    ToolingType = b.ToolingType,
                    ToolingSn = a.ToolingSn,
                    ToolingDesc = b.ToolingDesc,
                    Face = a.Face,
                    Length = a.Length,
                    Width = a.Width,
                    Height = a.Height,
                    DamageDegree = a.DamageDegree,
                    CleanDegree = a.CleanDegree,
                    Rough = a.Rough,
                    Scrape = a.Scrape,
                    LastMaintainTime = a.LastMaintainTime,
                })
                .ToListAsync();
        }
    }
}
