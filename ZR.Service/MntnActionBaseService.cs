using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using ZR.Common;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Model.System.Vo;
using ZR.Repository;
using ZR.Service.IService;
using ZR.Service.System.IService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Service
{
    /// <summary>
    /// 站点行为配置
    /// </summary>
    [AppService(ServiceType = typeof(IMntnActionBaseService), ServiceLifetime = LifeTime.Transient)]
    public class MntnActionBaseService : BaseService<MActionJobTypeBase>, IMntnActionBaseService
    {
        /// <summary>
        /// //获取所有JobType
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<MActionJobTypeBaseDto> GetJobType(MActionJobTypeBaseQueryDto param)
        {
            if(param.Enabled=="ALL")
            {
                param.Enabled = null;
            }
            //var response = Queryable().WhereIF(!param.Enabled.IsNullOrEmpty(), it => it.Enabled == param.Enabled).WhereIF(!param.TypeName.IsNullOrEmpty(), it => it.TypeName == param.TypeName).WhereIF(!param.TypeId.IsNullOrZero(), it => it.TypeId == param.TypeId).Where(it => it.Site == param.Site).OrderBy(s => s.CreateTime, OrderByType.Desc).ToDto<MActionJobTypeBase, MActionJobTypeBaseDto>();
            var response = Queryable().WhereIF(!param.Enabled.IsNullOrEmpty(), it => it.Enabled == param.Enabled).WhereIF(!param.TypeName.IsNullOrEmpty(), it => it.TypeName.StartsWith(param.TypeName)).WhereIF(!param.TypeId.IsNullOrZero(), it => it.TypeId == param.TypeId).Where(it => it.Site == param.Site).OrderBy(s => s.CreateTime, OrderByType.Desc).ToList().Adapt<List<MActionJobTypeBaseDto>>();
            //var response = Queryable().WhereIF(!param.Enabled.IsNullOrEmpty(), it => it.Enabled == param.Enabled).WhereIF(!param.TypeName.IsNullOrEmpty(), it => it.TypeName == param.TypeName).WhereIF(!param.TypeId.IsNullOrZero(), it => it.TypeId == param.TypeId).Where(it => it.Site == param.Site).OrderBy(s => s.CreateTime, OrderByType.Desc).ToList();
            return response;
        }

        /// <summary>
        /// //获取所有JobId
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<MActionJobBaseDto> GetJobId(MActionJobBaseQueryDto param)
        {
            if (param.Enabled == "ALL")
            {
                param.Enabled = null;
            }
            var response = Context.Queryable<MActionJobBase>().WhereIF(!param.Enabled.IsNullOrEmpty(), it => it.Enabled == param.Enabled).WhereIF(!param.JobName.IsNullOrEmpty(),it=>it.JobName.StartsWith(param.JobName)).WhereIF(!param.TypeId.IsNullOrZero(),it=>it.TypeId==param.TypeId).WhereIF(!param.JobId.IsNullOrZero(),it=>it.JobId==param.JobId).Where(it => it.Site == param.Site).OrderBy(s=>s.CreateTime, OrderByType.Desc).ToList().Adapt<List<MActionJobBaseDto>>();
            return response;
        }

        /// <summary>
        /// //获取所有JobLink
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<MActionJobLinkDto> GetJobLink(MActionJobLinkQueryDto param)
        {
            if (param.Enabled == "ALL")
            {
                param.Enabled = null;
            }
            var response = Context.Queryable<MActionJobLink>().WhereIF(!param.Enabled.IsNullOrEmpty(), it => it.Enabled == param.Enabled).Where(it => it.Site == param.Site && it.JobId==param.JobId).OrderBy(s => s.JobSeq, OrderByType.Desc).ToList().Adapt<List<MActionJobLinkDto>>();
            return response;
        }

        /// <summary>
        /// //获取所有JobGroup
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<MActionGroupBaseDto> GetJobGroup(MActionGroupBaseQueryDto param)
        {
            if (param.Enabled == "ALL")
            {
                param.Enabled = null;
            }
            var response = Context.Queryable<MActionGroupBase>().WhereIF(!param.Enabled.IsNullOrEmpty(), it => it.Enabled == param.Enabled).WhereIF(!param.GroupName.IsNullOrEmpty(),it=>it.GroupName.StartsWith(param.GroupName)).WhereIF(!param.GroupId.IsNullOrZero(),it=>it.GroupId==param.GroupId).Where(it => it.Site == param.Site ).OrderBy(s => s.CreateTime, OrderByType.Desc).ToList().Adapt<List<MActionGroupBaseDto>>();
            return response;
        }
        /// <summary>
        /// //获取JobGroupDetail
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<dynamic> GetJobGroupLinkDetail(MActionGroupBaseQueryDto param)
        {
            var sql = @"SELECT C.TYPE_NAME,'['||C.PROC_PARAM||']' PROC_PARAM,A.GROUP_SEQ,B.JOB_NAME,A.VALUE_KIND,A.VALUE_TRANSFORMATION,A.LOOP_COUNT,A.GROUP_ID,B.JOB_ID
                                   FROM SAJET.M_ACTION_GROUP_LINK A,IMES.M_ACTION_JOB_BASE B,IMES.M_ACTION_JOB_TYPE_BASE C 
                                   WHERE A.JOB_ID = B.JOB_ID AND B.TYPE_ID = C.TYPE_ID
                                    AND A.GROUP_ID=@groupid ORDER BY A.GROUP_SEQ";
            var response = Context.Ado.SqlQuery<dynamic>(sql, new List<SugarParameter> { new SugarParameter("@groupid", param.GroupId) });
            return response;
        }

        /// <summary>
        /// //获取JobAGroupDetail
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetJobTypelist(string site)
        {
            var sql = @"SELECT  TYPE_ID, LPAD(TYPE_ID, 3, '0') || ': ' || TYPE_NAME || '(' || TYPE_DESC || ')' TYPE_DESC FROM SAJET.M_ACTION_JOB_TYPE_BASE WHERE 1=1 AND ENABLED='Y' AND SITE=@site ORDER BY TYPE_NAME";
            var response = Context.Ado.SqlQuery<dynamic>(sql,new List<SugarParameter> { new SugarParameter("@site", site) });
            return response;
        }

        /// <summary>
        /// //获取站点配置
        /// </summary>
        /// <returns></returns>
        public List<MStationActionDto> GetStationAction(MStationActionQueryDto param)
        {
            var response=Context.Queryable<MStationAction>().WhereIF(!param.StationName.IsNullOrEmpty(),it=>it.StationName==param.StationName).WhereIF(!param.StationType.IsNullOrEmpty(),it=>it.StationType==param.StationType).Where(it=>it.Site==param.Site).ToList().Adapt<List<MStationActionDto>>();
            return response;
        }

        /// <summary>
        /// 新增jobtype
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string InsertJobType(MActionJobTypeBase param)
        {
            try
            {
                //检查是否有重复
                var response = Queryable().Where(it => it.TypeName == param.TypeName).ToList();
            if(response.Count()>0)
            {
                return "TypeName 重复，请重新检查";
            }
            //获取ID
            //var oData = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);
            var tMax = new SugarParameter("T_MAXID",null,true);

             Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_GET_MAXID",
            new SugarParameter[]
            {
                    new SugarParameter("TFIELD", "TYPE_ID"),
                    new SugarParameter("TTABLE", "SAJET.M_ACTION_JOB_TYPE_BASE"),
                new SugarParameter("TNUM", 8),
                     tRes,tMax
                });
            if(tRes.Value.ToString()=="OK")
            {
                param.TypeId = Convert.ToInt64( tMax.Value.ToString());
            }
            else
            {
                return  "获取ID最大值失败，请检查 "+tRes.Value.ToString();
            }
           int i=  Add(param);
            return i==1?"OK":"插入失败";
            }
            catch(Exception ex)
            {
                return "新增失败，请检查 "+ex.ToString();
            }
        }
        /// <summary>
        /// 修改jobtype
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string UpdateJobType(MActionJobTypeBase param)
        {
            param.UpdateTime = DateTime.Now;
            int result = Context.Updateable(param).ExecuteCommand();
            return result == 1 ? "OK" : "更新失败";
        }
        /// <summary>
        /// 删除Jobtype
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string DeleteJobType(MActionJobTypeBase param)
        {
            var resut = UseTran2(() =>
            {
                int result = Context.Deleteable<MActionJobTypeBase>().Where(it => it.TypeId == param.TypeId && it.Site==param.Site).ExecuteCommand();
            });
            return resut == true ? "OK" : "删除失败";
        }
        /// <summary>
        /// 新增JobId
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string InsertJobId(MActionJobBase param)
        {
            try
            {
                //检查是否有重复
                var response = Context.Queryable<MActionJobBase>().Where(it => it.JobName == param.JobName).ToList();
                if (response.Count() > 0)
                {
                    return "JobName 重复，请重新检查";
                }
                //获取ID
                //var oData = new SugarParameter("O_MSG", null, true);
                var tRes = new SugarParameter("TRES", null, true);
                var tMax = new SugarParameter("T_MAXID", null, true);

                Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_GET_MAXID",
               new SugarParameter[]
               {
                    new SugarParameter("TFIELD", "JOB_ID"),
                    new SugarParameter("TTABLE", "SAJET.M_ACTION_JOB_BASE"),
                new SugarParameter("TNUM", 8),
                     tRes,tMax
                   });
                if (tRes.Value.ToString() == "OK")
                {
                    param.JobId = Convert.ToInt64(tMax.Value.ToString());
                }
                else
                {
                    return "获取ID最大值失败，请检查 " + tRes.Value.ToString();
                }
                int i = Context.Insertable(param).IgnoreColumns(ignoreNullColumn:true).ExecuteCommand();
                return i == 1 ? "OK" : "插入失败";
            }
            catch (Exception ex)
            {
                return "新增失败，请检查 " + ex.ToString();
            }
        }
        /// <summary>
        /// 修改JobId
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string UpdateJobId(MActionJobBase param)
        {
            param.UpdateTime = DateTime.Now;
            int result = Context.Updateable(param).ExecuteCommand();
            return result == 1 ? "OK" : "更新失败";
        }
        /// <summary>
        /// 删除JobID
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string DeleteJobID(MActionJobBase param)
        {
            var resut = UseTran2(() =>
            {
                int result = Context.Deleteable<MActionJobBase>().Where(it => it.TypeId == param.TypeId && it.JobId == param.JobId && it.Site==param.Site).ExecuteCommand();
            });
            return resut == true ? "OK" : "删除失败";
        }
        /// <summary>
        /// 新增Joblink
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string InsertJobLink(MActionJobLink param)
        {
            try
            {
                //获取最大jobseq
                var sql = "select nvl(max(job_seq),0)+1 FROM SAJET.M_ACTION_JOB_LINK  where job_id= @jobid ";
                DataTable dt= Context.Ado.GetDataTable(sql, new List<SugarParameter> { new SugarParameter("@jobid", param.JobId) });
                param.JobSeq = Convert.ToInt32(dt.Rows[0][0].ToString());
                //检查是否有重复
                var response = Context.Queryable<MActionJobLink>().Where(it => it.JobId == param.JobId && it.JobSeq==param.JobSeq).ToList();
                if (response.Count() > 0)
                {
                    return "JobSeq 重复，请重新检查";
                }
                int i = Context.Insertable(param).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                return i == 1 ? "OK" : "插入失败";
            }
            catch (Exception ex)
            {
                return "新增失败，请检查 " + ex.ToString();
            }
        }
        /// <summary>
        /// 修改Joblink
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string UpdateJobLink(MActionJobLink param)
        {
            param.UpdateTime = DateTime.Now;
            int result = Context.Updateable(param).WhereColumns(a => new {a.JobSeq,a.JobId}).ExecuteCommand();
            return result == 1 ? "OK" : "更新失败";
        }
        /// <summary>
        /// 删除JobLink
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string DeleteJobLink(MActionJobLink param)
        {
            try
            {
                var resut = UseTran(() =>
            {
                var u1sql = "update SAJET.M_ACTION_JOB_LINK set JOB_SEQ=JOB_SEQ-1   where JOB_ID=@jobid  and JOB_SEQ >=@jobseq";
                int result = Context.Deleteable<MActionJobLink>().Where(it => it.JobSeq == param.JobSeq && it.JobId == param.JobId && it.Site == param.Site).ExecuteCommand();
                Context.Ado.ExecuteCommand(u1sql, new List<SugarParameter> { new SugarParameter("@jobid", param.JobId), new SugarParameter("@jobseq", param.JobSeq) });
            });
            return resut.IsSuccess == true ? "OK" : resut.ErrorMessage;
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// 新增jobgroup
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string InsertJobGroup(MActionGroupBase param)
        {
            try
            {
                //检查是否有重复
                var response = Context.Queryable<MActionGroupBase>().Where(it => it.GroupName == param.GroupName).ToList();
                if (response.Count() > 0)
                {
                    return "GroupName 重复，请重新检查";
                }
                //获取ID
                //var oData = new SugarParameter("O_MSG", null, true);
                var tRes = new SugarParameter("TRES", null, true);
                var tMax = new SugarParameter("T_MAXID", null, true);

                Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_GET_MAXID",
               new SugarParameter[]
               {
                    new SugarParameter("TFIELD", "GROUP_ID"),
                    new SugarParameter("TTABLE", "SAJET.M_ACTION_GROUP_BASE"),
                new SugarParameter("TNUM", 8),
                     tRes,tMax
                   });
                if (tRes.Value.ToString() == "OK")
                {
                    param.GroupId = Convert.ToInt64(tMax.Value.ToString());
                }
                else
                {
                    return "获取ID最大值失败，请检查 " + tRes.Value.ToString();
                }
                int i = Context.Insertable(param).IgnoreColumns(ignoreNullColumn:true).ExecuteCommand();
                return i == 1 ? "OK" : "插入失败";
            }
            catch (Exception ex)
            {
                return "新增失败，请检查 " + ex.ToString();
            }
        }
        /// <summary>
        /// 修改jobgroup
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string UpdateJobGroup(MActionGroupBase param)
        {
            param.UpdateTime = DateTime.Now;
            int result = Context.Updateable(param).IgnoreColumns(it=>new {it.CreateTime,it.CreateEmpno,it.Site}).ExecuteCommand();
            return result == 1 ? "OK" : "更新失败";
        }
        /// <summary>
        /// 删除JobGroup
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string DeleteJobGroup(MActionGroupBase param)
        {
            var resut = UseTran2(() =>
            {
                int result = Context.Deleteable<MActionGroupBase>().Where(it => it.GroupId == param.GroupId && it.Site == param.Site).ExecuteCommand();
            });
            return resut == true ? "OK" : "删除失败";
        }
        /// <summary>
        /// 新增OR修改stationaction
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string InsertOrUpdateStationAction(MStationAction param)
        {
            int resulti;
           // param.UpdateTime = DateTime.Now;
            var result = Context.Queryable<MStationAction>().Where(it => it.StationType == param.StationType && it.StationName == param.StationName && it.Site==param.Site).ToList();
            //有值更新，无值新增
            if (result.Count > 0)
            {//更新
                //groupid 为空删除数据
                if (param.GroupId.IsEmpty())
                {
                    var sqldel = @"DELETE IMES.M_STATION_ACTION  C WHERE  C.STATION_TYPE=@StationType AND C.STATION_NAME=@StationName AND C.SITE=@site";
                    resulti= Context.Ado.ExecuteCommand(sqldel, new List<SugarParameter> { new SugarParameter("@StationType", param.StationType), new SugarParameter("@StationName", param.StationName),new SugarParameter("@site",param.Site) });
                }
                else
                {
                    resulti= Context.Updateable(param).UpdateColumns(it => new { it.GroupId, it.ShowBom, it.CheckLine, it.PrintFlag, it.AutoReadsn, it.AutoReadPath, it.CheckFont, it.PrintQty, it.UpdateEmpno, it.UpdateTime }).WhereColumns(it => new { it.StationName, it.StationType }).ExecuteCommand();
                }
            }
            else
            {//新增
                resulti= Context.Insertable(param).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            }
            return resulti == 1 ? "OK" : "操作失败";
        }
        /// <summary>
        /// 新增Jobgrouplink
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string InsertJobGroupLink(MActionGroupLink param)
        {
         var resut=   UseTran2(() =>
            {
                var u1sql = "update SAJET.M_ACTION_GROUP_LINK set GROUP_SEQ=GROUP_SEQ+1   where GROUP_ID=@groupid  and GROUP_SEQ >=@groupseq";
                var u2sql = "update SAJET.M_ACTION_GROUP_BASE  set UPDATE_EMPNO=@userno ,UPDATE_TIME = sysdate  where GROUP_ID=@groupid and rownum=1";
                Context.Ado.ExecuteCommand(u1sql, new List<SugarParameter> { new SugarParameter("@groupid", param.GroupId), new SugarParameter("@groupseq", param.GroupSeq) });
                Context.Insertable(param).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                Context.Ado.ExecuteCommand(u2sql, new List<SugarParameter> { new SugarParameter("@userno", param.CreateEmpno), new SugarParameter("@groupid", param.GroupId) });
            });    
                return resut==true ? "OK" : "插入失败";
        }
        /// <summary>
        /// 删除Jobgrouplink
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string DeleteJobGroupLink(MActionGroupLink param)
        {
            var resut = UseTran2(() =>
            {
                var u1sql = "update SAJET.M_ACTION_GROUP_LINK set GROUP_SEQ=GROUP_SEQ-1   where GROUP_ID=@groupid  and GROUP_SEQ > @groupseq";
                var u2sql = "update SAJET.M_ACTION_GROUP_BASE  set UPDATE_EMPNO=@userno ,UPDATE_TIME = sysdate  where GROUP_ID=@groupid and rownum=1";
                int result = Context.Deleteable<MActionGroupLink>().Where(it => it.GroupId == param.GroupId && it.GroupSeq == param.GroupSeq).ExecuteCommand();
                Context.Ado.ExecuteCommand(u1sql, new List<SugarParameter> { new SugarParameter("@groupid", param.GroupId), new SugarParameter("@groupseq", param.GroupSeq) });
                Context.Ado.ExecuteCommand(u2sql, new List<SugarParameter> { new SugarParameter("@userno", param.UpdateEmpno), new SugarParameter("@groupid", param.GroupId) });
            });
            return resut == true ? "OK" : "删除失败";
        }
        /// <summary>
        /// 更新Jobgrouplink
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string UpdateJobGroupLink(MActionGroupLink param)
        {
            int result = Context.Updateable(param).UpdateColumns(it=> new  {it.ValueKind,it.ValueTransformation,it.LoopCount, it.Enabled,it.UpdateEmpno,it.UpdateTime }).WhereColumns(it =>new {  it.GroupId ,it.GroupSeq }).ExecuteCommand();
            return result == 1 ? "OK" : "失败";
        }
    }
}
