using Aliyun.OSS;
using Infrastructure.Attribute;
using JinianNet.JNTemplate;
using Mapster;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Model.Dto.Tooling;
using ZR.Model.System;
using ZR.Model.System.ZR.Model.Business;
using ZR.Service.ToolingManagement.IService;

namespace ZR.Service.ToolingManagement
{
    [AppService(ServiceType = typeof(IToolingPickUpService),ServiceLifetime = LifeTime.Transient)]
    public class ToolingPickUpServiceImpl  : BaseService<MToolingToolingSnVo>, IToolingPickUpService
    {

        public ImesMemp SelectEmpByNo(string empno, string site)
        {

            var exp = Expressionable.Create<ImesMemp>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.And(it => it.empNo == empno);
            return Context.Queryable<ImesMemp>().Where(exp.ToExpression()).First();
        }



        public MToolingToolingSnVo selectToolingByToolingSn(string toolingsn, string site)
        {
            MToolingToolingSnVo mToolingToolingSnVo = new MToolingToolingSnVo();
            var exp = Context.Queryable<MTooling, MToolingSn, ImesMemp>((a, b, c) => new JoinQueryInfos(
                JoinType.Left, a.Id == b.ToolingId,
                JoinType.Left, c.empNo == b.UpdateEmpNo1))
                .Select((a, b, c) => new MToolingToolingSnVo
                {
                    Site = a.Site,
                    toolingId = a.Id,
                    toolingSnId = b.ToolingSnId,
                    toolingType = a.ToolingType,
                    WarnUsedTimes = a.WarnUsedTimes,
                    MaxUseTimes = a.MaxUseTimes,
                    WarnUsedDay = a.WarnUsedDay,
                    MaxUseDay = a.MaxUseDay,
                    toolingSn = b.ToolingSn,
                    totalUsedCount = b.TotalUsedCount,
                    usedCount = b.UsedCount,
                    toolingStatus = b.ToolingStatus,
                    updateStatusTime = b.UpdateTime1,
                    updateStatusEmpName = c.empName,
                    MaintainTime = a.MaintainTime,
                })
                .Distinct()
                .Where("a.SITE = '" + site + "'")
                .Where("b.tooling_sn = '" + toolingsn + "'")
                .Where("b.enabled = b.enabled and a.enabled = 'Y'");
            if (exp != null)
            {
                mToolingToolingSnVo = exp.First();
            }
           
            return mToolingToolingSnVo;

        }


        public  ExecuteResult  ToolingPickUp(string toolingsn, string empno, string site)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                  Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_TOOLING_PICKUP",
                new SugarParameter[]
                    {
                    new SugarParameter("TTOOLING_SN", toolingsn),
                    new SugarParameter("TEMPNO", empno),
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

        public ExecuteResult  ToolingReturn(string toolingsn, string empno, string site)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                  Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_TOOLING_RETURN",
                new SugarParameter[]
                    {
                    new SugarParameter("TTOOLING_SN", toolingsn),
                    new SugarParameter("TEMPNO", empno),
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

        public async Task<List<ToolingPickupResultVo>> GetToolingInfo(string toolingSn)
        {
            return await Context.Queryable<MTooling, MToolingSn, MEmp>(
                (a, b, c) => a.Id == b.ToolingId && c.EmpNo == b.UpdateEmpNo1
            )
            .Where((a, b, c) => b.ToolingSn == b.ToolingSn)
            .Select((a, b, c) => new ToolingPickupResultVo
            {
                ToolingType = a.ToolingType,
                ToolingNo = a.ToolingNo,
                ToolingSn = b.ToolingSn,
                ToolingSnDesc = b.ToolingSnDesc,
                MaxUseTimes = a.MaxUseTimes,
                TotalUsedCount = b.TotalUsedCount,
                ToolingStatus = b.ToolingStatus,
                EmpName = c.EmpName,
                UpdateTime = b.UpdateTime1,
            })
            .Mapper(x => x.ToolingStatus = x.ToolingStatus switch
            {
                "I" => "入库",
                "P" => "领用",
                "L" => "上线",
                "D" => "下线",
                "R" => "维修",
                "M" => "保养",
                "S" => "报废",
                "T" => "生产",
                "F" => "归还",
                _ => x.ToolingStatus,
            })
            .ToListAsync();
        }

        public List<MLine> SelectLine(string site)
        {
            List<MLine> resList = Context.Queryable<MLine>()
                .Where("ENABLED = 'Y' AND SITE = '" + site + "'")
                .ToList();
            return resList;

            //string strSql = @"SELECT TOOLING_TYPE FROM IMES.M_LINE WHERE SITE = @SITE AND ENABLED = 'Y'";
            //var TypeList = Context.Ado.SqlQuery<dynamic>(strSql, new SugarParameter("@SITE", site));
            //List<string> resList = new List<string>();
            //foreach (var item in TypeList)
            //{
            //    resList.Add(item.TOOLING_TYPE.ToString());
            //}
            //return resList;
        }

        public bool CheckLine(string site, string line)
        {
            var response = Context.Queryable<MLine>()
                .Where(a => a.Enabled == "Y")
                .Where(a => a.Site == site)
                 .Where(a => a.Line == line).ToList();
            if (response.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public List<MPart> SelectPart(string ipn, string site)
        {
            var response = Context.Queryable<MPart>()
                .Where(a => a.Ipn == ipn)
                .Where(a => a.Enabled == "Y")
                .Where(a => a.Site == site)
                .Select(a => new MPart { Ipn = a.Ipn, SPEC2 = a.SPEC2, Site = a.Site }).ToList();
            return response;
        }


        public ExecuteResult ToolingPickLoad(string toolingsn, string line, string ipn ,string empno, string site)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_TOOLING_PICKLOAD",
                new SugarParameter[]
                    {
                    new SugarParameter("TTOOLING_SN", toolingsn),
                    new SugarParameter("TIPN", ipn),
                    new SugarParameter("TPDLINE", line),
                    new SugarParameter("TEMPNO", empno),
                    tRes,
                    new SugarParameter("SITE", site)
                    });
                string sRes = tRes.Value.ToString();
                exeRes.Status = true;
                exeRes.Message = sRes;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult  ToolingPickUnload(string toolingsn,string line, string empno, string site)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                 Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_TOOLING_PC_UNLOAD",
                new SugarParameter[]
                    {
                    new SugarParameter("TTOOLINGSN", toolingsn),
                    new SugarParameter("TLINE", line),
                    new SugarParameter("TEMP", empno),
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
                    exeRes.Message = "Tooling UnLoad OK";
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }


        public ToolingPickLoadVo SelectToolingPickLoadIn(string line, string toolingSn, string site)
        {
            var resp = Context.Queryable<MToolingLoad, MToolingSn, MTooling, MEmp>((a, b, c,d) => new JoinQueryInfos(
                JoinType.Left, a.ToolingSn == b.ToolingSn,
                JoinType.Left, b.ToolingId == c.Id,
                JoinType.Left, a.UpdateEmpNo == d.EmpNo
                ))
                .Select((a, b, c, d) => new ToolingPickLoadVo
                {
                    ToolingType = c.ToolingType,
                    ToolingNo = c.ToolingNo,
                    ToolingSn = b.ToolingSn,
                    Line = a.Line,
                    Status = a.Status,
                    TotalUsedCount = a.TotalusedCount,
                    MaxUseTimes = c.MaxUseTimes,
                    EmpName = d.EmpName,
                    UpdateTime = a.UpdateTime
                }).OrderBy("A.UPDATE_TIME DESC")
                .Where("A.LINE = '"+ line + "' ")
                .Where("A.SITE = '" + site + "' ")
                .Where("B.TOOLING_SN = '"+ toolingSn + "'")
                .First();
            return resp;

        }

        public ToolingPickLoadVo SelectToolingPickLoadOut(string line, string toolingSn, string site)
        {
            var resp = Context.Queryable<MHtToolingLoad, MToolingSn, MTooling, MEmp>((a, b, c, d) => new JoinQueryInfos(
                JoinType.Left, a.ToolingSn == b.ToolingSn,
                JoinType.Left, b.ToolingId == c.Id,
                JoinType.Left, a.UpdateEmpNo == d.EmpNo
                ))
                .Select((a, b, c, d) => new ToolingPickLoadVo
                {
                    ToolingType = c.ToolingType,
                    ToolingNo = c.ToolingNo,
                    ToolingSn = b.ToolingSn,
                    Line = a.Line,
                    Status = a.Status,
                    TotalUsedCount = a.TotalusedCount,
                    MaxUseTimes = c.MaxUseTimes,
                    EmpName = d.EmpName,
                    UpdateTime = a.UpdateTime
                }).OrderBy("A.UPDATE_TIME DESC")
                .Where("A.PDLINE_NAME = '" + line + "' ")
                .Where("A.SITE = '" + site + "' ")
                .Where("B.TOOLING_SN = '" + toolingSn + "'")
                .First();
            return resp;

        }

    }


}

