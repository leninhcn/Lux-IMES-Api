using Aliyun.OSS;
using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Infrastructure.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
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

namespace ZR.Service
{
    /// <summary>
    /// 不良原因业务处理层
    /// </summary>
    [AppService(ServiceType = typeof(IMntnBlockConfigService), ServiceLifetime = LifeTime.Transient)]
    public class MntnBlockConfigService : BaseService<MBlockConfigType>,IMntnBlockConfigService
    {
        /// <summary>
        /// //获取所有route信息
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<Route> GetListRoute(string parm,string site)
        { 
          var response=  Context.Queryable<Route>().WhereIF(!parm.IsNullOrEmpty(), it => it.RouteName.ToLower().StartsWith(parm.ToLower())).WhereIF(true, it => it.Enabled == "Y").Where(it=>it.Site==site).ToList();
            return response;
        }
        /// <summary>
        /// //获取所有line信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<MLine> GetListLine(string parm, string site)
        {
            var response= Context.Queryable<MLine>().WhereIF(!parm.IsNullOrEmpty(), it => it.Line.ToLower().StartsWith(parm.ToLower())).WhereIF(true, it => it.Enabled == "Y").Where(it=>it.Site==site).
               ToList();
            return response;
        }
        /// <summary>
        /// //获取所有StationType信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<MStationType> GetListStationType(string parm,string site)
        {
            var response = Context.Queryable<MStationType>().WhereIF(!parm.IsNullOrEmpty(), it => it.StationType.ToLower().StartsWith(parm.ToLower())).WhereIF(true, it => it.Enabled == "Y").Where(it=>it.Site==site).ToList();
            return response;
        }
        /// <summary>
        /// //获取所有Station信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<Station> GetListStation(string parm,string site)
        {
            var response = Context.Queryable<Station>().WhereIF(!parm.IsNullOrEmpty(), it => it.StationName.ToLower().StartsWith(parm.ToLower())).WhereIF(true, it => it.Enabled == "Y").Where(it=>it.Site==site).ToList();
            return response;
        }
        /// <summary>
        /// //获取所有type信息分页
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MBlockConfigTypeDto> GetListTypeFenye(MBlockConfigTypeQueryDto parm)
        {
            var predicate = Expressionable.Create<MBlockConfigType>();
            predicate = predicate.AndIF (parm.Enabled.IsNotEmpty(),it => it.Enabled == parm.Enabled);
            predicate = predicate.AndIF(parm.ConfigTypeName != null, it => it.ConfigTypeName.Contains(parm.ConfigTypeName));
            predicate = predicate.And(it => it.Site == parm.site);
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            // var response1 = GetPages(predicate.ToExpression(), parm, s => new { s.CreateTime });
            // var response = GetPages((predicate.ToExpression());
            //排序
            parm.Sort = "CreateTime";
            parm.SortType= "desc";
            var response = Queryable().Where(predicate.ToExpression()).ToPage<MBlockConfigType, MBlockConfigTypeDto>(parm);
            return response;
        }
        /// <summary>
        /// //获取所有type信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<MBlockConfigType> GetListType(MBlockConfigTypeQueryDto parm)
        {
            if (parm.Enabled == "ALL")
            {
                parm.Enabled = "";
            }
            var predicate = Expressionable.Create<MBlockConfigType>();
            predicate = predicate.AndIF(!parm.Enabled.IsNullOrEmpty(), it => it.Enabled == parm.Enabled);
            predicate = predicate.AndIF(parm.ConfigTypeName != null, it => it.ConfigTypeName.Contains(parm.ConfigTypeName));
            predicate = predicate.And(it=>it.Site == parm.site);
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            // var response1 = GetPages(predicate.ToExpression(), parm, s => new { s.CreateTime });
            // var response = GetPages((predicate.ToExpression());
            //排序
            //parm.Sort = "CreateTime";
            //parm.SortType = "desc";
            var response = Queryable().Where(predicate.ToExpression()).OrderBy(f=>f.CreateTime,OrderByType.Desc).ToList();
            return response;
        }
        /// <summary>
        /// //获取sqltype信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<MBlockConfigType> GetListSqlType(MBlockConfigTypeQueryDto parm)
        {
            if (parm.Enabled == "ALL")
            {
                parm.Enabled = "";
            }
            parm.ConfigTypeName = "PROCESS_BLOCK_PROSQL";
            var predicate = Expressionable.Create<MBlockConfigType>();
            predicate = predicate.AndIF(!parm.Enabled.IsNullOrEmpty(), it => it.Enabled == parm.Enabled);
            predicate = predicate.AndIF(parm.ConfigTypeName != null, it => it.ConfigTypeName.Contains(parm.ConfigTypeName));
            predicate = predicate.And(it => it.Site == parm.site);
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            // var response1 = GetPages(predicate.ToExpression(), parm, s => new { s.CreateTime });
            // var response = GetPages((predicate.ToExpression());
            //排序
            //parm.Sort = "CreateTime";
            //parm.SortType = "desc";
            var response = Queryable().Where(predicate.ToExpression()).OrderBy(f => f.CreateTime, OrderByType.Desc).ToList();
            return response;
        }
        /// <summary>
        /// //获取所有value信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<MBlockConfigValue> GetListValue(MBlockConfigValueQueryDto parm)
        {
            if (parm.Enabled == "ALL")
            {
                parm.Enabled = "";
            }
            return Context.Queryable<MBlockConfigValue>().WhereIF(!parm.Enabled.IsNullOrEmpty(), it => it.Enabled == parm.Enabled).WhereIF(parm.ConfigTypeId != null, it => it.ConfigTypeId == parm.ConfigTypeId).WhereIF(parm.ConfigName != null, it => it.ConfigName.Contains(parm.ConfigName)).Where(it=>it.Site==parm.site).OrderBy(it=>it.CreateTime,OrderByType.Desc).ToList();
        }
        /// <summary>
        /// //获取所有sql信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<MBlockConfigProsql> GetListSql(MBlockConfigProsqlQueryDto parm)
        {
            if (parm.Enabled == "ALL")
            {
                parm.Enabled = "";
            }
            return Context.Queryable<MBlockConfigProsql>().WhereIF(!parm.Enabled.IsNullOrEmpty(), it => it.Enabled == parm.Enabled).WhereIF(parm.ConfigTypeId != null, it => it.ConfigTypeId == parm.ConfigTypeId).WhereIF(parm.ConfigName != null, it => it.ConfigName.Contains(parm.ConfigName)).Where(it=>it.Site==parm.site).OrderBy(it=>it.CreateTime,OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 获取type详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MBlockConfigType GetInfoType(string ID)
        {
            var response = Queryable().Where(x => x.ConfigTypeId == ID).First();
            return response;
        }

        /// <summary>
        /// 获取value详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MBlockConfigValue GetInfoValue(string ID)
        {
            var response = Context.Queryable<MBlockConfigValue>().Where( it => it.ConfigId == ID).First();
            return response;
        }

        /// <summary>
        /// 获取sql详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public MBlockConfigProsql GetInfoSql(string ID)
        {
            var response = Context.Queryable<MBlockConfigProsql>().Where(it => it.ConfigId == ID).First();
            return response;
        }
        /// <summary>
        /// 新增type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddType(MBlockConfigType model)
        {
            try
            {
                //检查是否有重复
                var predicate = Expressionable.Create<MBlockConfigType>();
                predicate.AndIF(model.ConfigTypeName.IsNotEmpty(), it => it.ConfigTypeName == model.ConfigTypeName).And(it=>it.Site==model.Site);
                var response = GetList(predicate.ToExpression());
                if(response.Count()>0)
                {
                    return "ConfigTypeName 重复，请重新检查";
                }
                //获取ID
                //var oData = new SugarParameter("O_MSG", null, true);
                model.ConfigTypeId=Guid.NewGuid().ToString("D");
                int i = Context.Insertable(model).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                //备份备份代码取消,数据库trigger
                //Context.Ado.ExecuteCommand("insert into IMES.M_BLOCK_CONFIG_TYPE_HT select * from IMES.M_BLOCK_CONFIG_TYPE where CONFIG_TYPE_ID = @ID", new List<SugarParameter>{ new SugarParameter("@ID", model.ConfigTypeId ) });
            return i==1?"OK":"插入失败";
            }
            catch(Exception ex)
            {
                return "新增失败，请检查 "+ex.ToString();
            }
        }
        /// <summary>
        /// 新增value
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddValue(MBlockConfigValue model)
        {
            try
            {
                //获取ID
                //var oData = new SugarParameter("O_MSG", null, true);
                model.ConfigId = Guid.NewGuid().ToString();
                int i = Context.Insertable(model).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                //备份备份代码取消,数据库trigger
                // Context.Ado.ExecuteCommand("insert into IMES.M_BLOCK_CONFIG_VALUE_HT select * from IMES.M_BLOCK_CONFIG_VALUE where CONFIG_ID = @ID", new List<SugarParameter>{ new SugarParameter("@ID", model.ConfigId ) });
                return i == 1 ? "OK" : "插入失败";
            }
            catch (Exception ex)
            {
                return "新增失败，请检查 " + ex.ToString();
            }
        }
        /// <summary>
        /// 新增sql
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddSql(MBlockConfigProsql model)
        {
            try
            {
                //获取ID
                //var oData = new SugarParameter("O_MSG", null, true);
                model.ConfigId = Guid.NewGuid().ToString();
                int i = Context.Insertable(model).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                //备份备份代码取消,数据库trigger
                //Context.Ado.ExecuteCommand("insert into IMES.M_BLOCK_CONFIG_PROSQL_HT select * from IMES.M_BLOCK_CONFIG_PROSQL where CONFIG_ID = @ID", new List<SugarParameter>{ new SugarParameter("@ID", model.ConfigId ) });
                return i == 1 ? "OK" : "插入失败";
            }
            catch (Exception ex)
            {
                return "新增失败，请检查 " + ex.ToString();
            }
        }
        /// <summary>
        /// 修改type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UpdateType(MBlockConfigType model)
        {   
            model.UpdateTime = DateTime.Now;
            int result = Context.Updateable(model).ExecuteCommand();
            //备份备份代码取消,数据库trigger
            //Context.Ado.ExecuteCommand("insert into IMES.M_BLOCK_CONFIG_TYPE_HT select * from IMES.M_BLOCK_CONFIG_TYPE where CONFIG_TYPE_ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", model.ConfigTypeId) });
            return "OK";
        }
        /// <summary>
        /// 修改value
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UpdateValue(MBlockConfigValue model)
        {
            model.UpdateTime = DateTime.Now;
            int result = Context.Updateable(model).ExecuteCommand();
            //备份备份代码取消,数据库trigger
            //Context.Ado.ExecuteCommand("insert into IMES.M_BLOCK_CONFIG_VALUE_HT select * from IMES.M_BLOCK_CONFIG_VALUE where CONFIG_ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", model.ConfigId) });
            return "OK";
        }
        /// <summary>
        /// 修改sql
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UpdateSql(MBlockConfigProsql model)
        {
            model.UpdateTime = DateTime.Now;
            int result = Context.Updateable(model).ExecuteCommand();
            //备份代码取消,数据库trigger
           // Context.Ado.ExecuteCommand("insert into IMES.M_BLOCK_CONFIG_PROSQL_HT select * from IMES.M_BLOCK_CONFIG_PROSQL where CONFIG_ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", model.ConfigId) });
            return "OK";
        }
        /// <summary>
        /// 删除类型
        /// </summary>
        /// <param name = "model" ></param >
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DeleteType(MBlockConfigTypeDto model, string ids)
        {
            // long[] idsArr = Tools.SpitLongArrary(ids);
            string[] idsArr = ids.Split(',');
            if (idsArr.Length <= 0)
            {
                return "删除失败Id 不能为空";
            }
            string sqlparm="";
            foreach (string id in idsArr)
            {
                sqlparm=sqlparm+"'"+id+"'"+",";
            }         
            ids = "(" + sqlparm.TrimEnd(',') + ")";
            //int result = Context.Updateable(defect).ExecuteCommand();
            Context.Ado.ExecuteCommand($" update imes.M_BLOCK_CONFIG_TYPE set ENABLED='N',UPDATE_TIME=SYSDATE,UPDATE_EMPNO=@EMPNO WHERE CONFIG_TYPE_ID in {ids}", new List<SugarParameter> { new SugarParameter("@EMPNO", model.UpdateEmpno) });
            //备份代码取消,数据库trigger
            //Context.Ado.ExecuteCommand($"insert into IMES.M_BLOCK_CONFIG_TYPE_HT select * from IMES.M_BLOCK_CONFIG_TYPE where CONFIG_TYPE_ID in {ids}");
            return "OK";
        }
        /// <summary>
        /// 删除配置项
        /// </summary>
        /// <param name = "model" ></param >
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DeleteValue(MBlockConfigValueDto model, string ids)
        {
           // long[] idsArr = Tools.SpitLongArrary(ids);
            string[] idsArr = ids.Split(',');
            if (idsArr.Length <= 0)
            {
                return "删除失败Id 不能为空";
            }
            string sqlparm = "";
            foreach (string id in idsArr)
            {
                sqlparm = sqlparm + "'" + id + "'" + ",";
            }
            ids = "(" + sqlparm.TrimEnd(',') + ")";
            //int result = Context.Updateable(defect).ExecuteCommand();
            Context.Ado.ExecuteCommand($" update imes.M_BLOCK_CONFIG_VALUE set ENABLED='N',UPDATE_TIME=SYSDATE,UPDATE_EMPNO=@EMPNO WHERE CONFIG_ID in {ids}", new List<SugarParameter> {  new SugarParameter("@EMPNO",model.UpdateEmpno)} );
            //备份代码取消,数据库trigger
            //Context.Ado.ExecuteCommand($"insert into IMES.M_BLOCK_CONFIG_VALUE_HT select * from IMES.M_BLOCK_CONFIG_VALUE where CONFIG_ID in {ids}");
            return "OK";
        }
        /// <summary>
        /// 删除sql
        /// </summary>
        /// <param name = "model" ></param >
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DeleteSql(MBlockConfigProsqlDto model, string ids)
        {
            string[] idsArr = ids.Split(',');
            if (idsArr.Length <= 0)
            {
                return "删除失败Id 不能为空";
            }
            string sqlparm = "";
            foreach (string id in idsArr)
            {
                sqlparm = sqlparm + "'" + id + "'" + ",";
            }
            ids = "(" + sqlparm.TrimEnd(',') + ")";
            //int result = Context.Updateable(defect).ExecuteCommand();
            Context.Ado.ExecuteCommand($" update imes.M_BLOCK_CONFIG_PROSQL set ENABLED='N',UPDATE_TIME=SYSDATE,UPDATE_EMPNO=@EMPNO WHERE CONFIG_ID in {ids}", new List<SugarParameter> { new SugarParameter("@EMPNO", model.UpdateEmpno) });
            //备份代码取消,数据库trigger
            //Context.Ado.ExecuteCommand($"insert into IMES.M_BLOCK_CONFIG_VALUE_HT select * from IMES.M_BLOCK_CONFIG_VALUE where CONFIG_ID in {ids}");
            return "OK";
        }
    }
}
