using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Repository;
using ZR.Service.IService;

namespace ZR.Service.ProdMnt
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class ProcessTechnologyService : BaseService<SnStatus>, IProcessTechnologyService

    {
        public int DeleteStationByID(StationTechInfo stationTechInfo)
        {


            int updateStation = Context.Updateable(stationTechInfo).UpdateColumns(it => new { it.updateEmpno, it.updateTime }).WhereColumns(it => new { it.id }).ExecuteCommand();
            if (updateStation > 0)
            {
                string sqlStr = $"insert into  IMES.M_STATIONTECH_ht(select * FROM SAJET.M_STATIONTECH   where id = " + stationTechInfo.id + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return Context.Deleteable<StationTechInfo>().Where(it => it.id == stationTechInfo.id).ExecuteCommand();
            }
            return 0;

        }

        public int UpdateStation(StationTechInfo stationTechInfo)
        {
            int updateStation = Context.Updateable(stationTechInfo).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.id).ExecuteCommand();
            if (updateStation > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_STATIONTECH_ht(select * FROM SAJET.M_STATIONTECH  where id = '" + stationTechInfo.id + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return 0;
        }

        public int InsertStatinoInfo(StationTechInfo stationTechInfo)
        {

            int MaxId = Context.Queryable<StationTechInfo>().Max(it => it.id) + 1;
            stationTechInfo.id = MaxId;


            int insertErp = Context.Insertable(stationTechInfo).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_STATIONTECH_ht (select * FROM SAJET.M_STATIONTECH where id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }

        public int CopyStationInfo(StationTechInfo stationTechInfo)
        {
            int MaxId = Context.Queryable<StationTechInfo>().Max(it => it.id) + 1;
            stationTechInfo.id = MaxId;
            stationTechInfo.enabled = "Y";
            stationTechInfo.createTime = DateTime.Now;
            stationTechInfo.updateTime = DateTime.Now;
            int insertErp = Context.Insertable(stationTechInfo).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_STATIONTECH_ht (select * FROM SAJET.M_STATIONTECH where id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }
        public List<StationTypeInfo> ShowStationType(string textData)
        {
            string enaBled = "Y";
            string optionData = "STATION_TYPE";

            var exp = Expressionable.Create<StationTypeInfo>();
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            exp.AndIF(optionData != "" && optionData != null, it => it.stationType.Contains(textData));

            List<StationTypeInfo> resultList = Context.Queryable<StationTypeInfo>()
                                                  .Where(exp.ToExpression())
                                                  .ToList();

            return resultList;

        }

        public PagedInfo<StationTechInfo> ShowStationInfo(string enaBled, string optionData, string textData, int pageNum, int pageSize)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<StationTechInfo>();
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "id", it => it.id.Equals(textData));
                exp.AndIF(optionData == "stationType", it => it.stationType.Contains(textData));
                exp.AndIF(optionData == "processType", it => it.processType.Contains(textData));
                exp.AndIF(optionData == "value", it => it.value.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
                exp.AndIF(optionData == "updateTime", it => it.updateTime.Equals(textData));
                exp.AndIF(optionData == "createTime", it => it.createTime.Equals(textData));
                exp.AndIF(optionData == "createEmpno", it => it.createEmpno.Contains(textData));
                exp.AndIF(optionData == "enabled", it => it.enabled.Equals(textData));
            }
            var expPagedInfo = Context.Queryable<StationTechInfo>().Where(exp.ToExpression()).OrderBy(it => it.createTime);
            return expPagedInfo.ToPage(pager);
        }

        public (string, object, object) StationImportData(List<StationTechInfo> station, string site, string name)
        {
            try
            {
                int add = 0;
                int up = 0;
                station.ForEach(x =>
                {
                    x.createTime = DateTime.Now;
                    x.updateTime = DateTime.Now;
                    x.createEmpno = name;
                    x.updateEmpno = name;
                    x.site = site;
                    if (x.id == 0)
                    {
                        x.id = Context.Queryable<StationTechInfo>().Max(it => it.id) + 1;
                        int Insertable = Context.Insertable(x).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                        if (Insertable > 0)
                        {
                            add++;
                        }
                    }
                    else
                    {
                        int UpdateStage = Context.Updateable(x).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.id).ExecuteCommand();
                        up++;
                    }
                    Context.Ado.SqlQuery<Object>($"insert INTO SAJET.M_STATIONTECH_ht (select * from  IMES.M_STATIONTECH   where id = " + x.id + ")");

                });
                return ("数量：" + station.Count, " 添加;" + add, " 修改;" + up);
            }
            catch (Exception e)
            {
                return ("站点/站点类型重复，请检查！", " 添加;0", " 修改;0");
            }
        }
        public List<StationTechInfo> StationHistory(string id)

        {
            String sqlStr = $"select *  from  IMES.M_STATIONTECH_ht  b  where 1=1 ";

            if (id != null && id != "")
            {
                sqlStr += " and b.id ='" + id + "'";
            }
            List<StationTechInfo> list = Context.Ado.SqlQuery<StationTechInfo>(sqlStr);

            return list;
        }

        public PagedInfo<LineTypeInfo> ShowLineInfo(string enaBled, string optionData, string textData, int pageNum, int pageSize)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<LineTypeInfo>();
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {

                exp.AndIF(optionData == "lineType", it => it.lineType.Contains(textData));
                exp.AndIF(optionData == "lineTypeDesc", it => it.lineTypeDesc.Contains(textData));
                exp.AndIF(optionData == "lineOn", it => it.lineOn.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
                exp.AndIF(optionData == "updateTime", it => it.updateTime.Equals(textData));
            }
            var expPagedInfo = Context.Queryable<LineTypeInfo>().Where(exp.ToExpression()).OrderBy(it => it.updateTime).ToPage(pager);
            return expPagedInfo;
        }

        public int InsertLineInfo(LineTypeInfo lineTypeInfo)
        {
            int MaxId = Context.Queryable<LineTypeInfo>().Max(it => it.id) + 1;
            lineTypeInfo.id = MaxId;

            lineTypeInfo.enabled = "Y";
            lineTypeInfo.createTime = DateTime.Now;
            lineTypeInfo.updateTime = DateTime.Now;
            int insertErp = Context.Insertable(lineTypeInfo).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_LINE_TYPE_ht (select * FROM SAJET.M_LINE_TYPE where id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }

        public int UpdateLine(LineTypeInfo lineTypeInfo)
        {
            int UpdateLine = Context.Updateable(lineTypeInfo).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.id).ExecuteCommand();
            if (UpdateLine > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_LINE_TYPE_ht(select * FROM SAJET.M_LINE_TYPE   where id = '" + lineTypeInfo.id + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return UpdateLine;
        }

        public int DeleteLineByID(LineTypeInfo lineTypeInfo)
        {
            {
                int updateLine = Context.Updateable(lineTypeInfo).UpdateColumns(it => new { it.updateEmpno, it.updateTime }).WhereColumns(it => new { it.id }).ExecuteCommand();
                if (updateLine > 0)
                {
                    string sqlStr = $"insert INTO SAJET.M_LINE_TYPE_ht(select * FROM SAJET.M_LINE_TYPE   where id = " + lineTypeInfo.id + ")";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return Context.Deleteable<LineTypeInfo>().Where(it => it.id == lineTypeInfo.id).ExecuteCommand();
                }

                return 0;
            }
        }
        /// <summary>
        /// 导入
        /// </summary>
        public (string, object, object) LineImportData(List<LineTypeInfo> stationTypes, string site, string name)
        {
            try
            {
                int add = 0;
                int up = 0;
                stationTypes.ForEach(x =>
                {
                    x.createTime = DateTime.Now;
                    x.updateTime = DateTime.Now;
                    x.createEmpno = name;
                    x.updateEmpno = name;
                    x.site = site;
                    if (x.id == 0)
                    {
                        x.id = Context.Queryable<LineTypeInfo>().Max(it => it.id) + 1;
                        int Insertable = Context.Insertable(x).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                        if (Insertable > 0)
                        {
                            add++;
                        }
                    }
                    else
                    {
                        int UpdateStage = Context.Updateable(x).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.id).ExecuteCommand();
                        up++;
                    }
                    Context.Ado.SqlQuery<Object>($"insert INTO SAJET.M_LINE_TYPE_ht (select * from  IMES.M_LINE_TYPE   where id = " + x.id + ")");

                });
                return ("数量：" + stationTypes.Count, " 添加;" + add, " 修改;" + up);
            }
            catch (Exception e)
            {
                return ("lineType/线别类型重复，请检查！", " 添加;0", " 修改;0");
            }
        }

        public PagedInfo<StageInfo> ShowStageInfo(string enaBled, string optionData, string textData, int pageNum, int pageSize)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<StageInfo>();
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "stage", it => it.stage.Contains(textData));
                exp.AndIF(optionData == "stageDesc", it => it.stageDesc.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
                exp.AndIF(optionData == "updateTime", it => it.updateTime.Equals(textData));
            }
            var expPagedInfo = Context.Queryable<StageInfo>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
            return expPagedInfo;
        }

        public int InsertStageInfo(StageInfo stageInfo)
        {
            int MaxId = Context.Queryable<StageInfo>().Max(it => it.id) + 1;
            stageInfo.id = MaxId;
            stageInfo.enabled = "Y";
            int insertErp = Context.Insertable(stageInfo).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_STAGE_ht (select * FROM SAJET.M_STAGE where id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }

        public int UpdateStage(StageInfo stageInfo)
        {
            int UpdateStage = Context.Updateable(stageInfo).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.id).ExecuteCommand();
            if (UpdateStage > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_STAGE_ht(select * from  IMES.M_STAGE  where id = '" + stageInfo.id + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return UpdateStage;
        }

        public int DeleteStageByID(StageInfo stageInfo)
        {

            int updateLine = Context.Updateable(stageInfo).UpdateColumns(it => new { it.updateEmpno, it.updateTime }).WhereColumns(it => new { it.id }).ExecuteCommand();
            if (updateLine > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_STAGE_ht(select * from  IMES.M_STAGE   where id = " + stageInfo.id + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return Context.Deleteable<StageInfo>().Where(it => it.id == stageInfo.id).ExecuteCommand();
            }

            return 0;

        }
        public List<StageInfo> StageHistory(string id)
        {
            String sqlStr = $"select *  FROM SAJET.M_STAGE_ht  b  where 1=1 ";

            if (id != null && id != "")
            {
                sqlStr += " and b.id ='" + id + "'";
            }

            List<StageInfo> list = Context.Ado.SqlQuery<StageInfo>(sqlStr);

            return list;
        }


        public PagedInfo<StationTypeInfo> ShowStationTypeInfo(string stage, string enaBled, string optionData, string textData, int pageNum, int pageSize)
        {

            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<StationTypeInfo>();
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            exp.AndIF(stage != "" && stage != null, it => it.stage == stage);

            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "stage", it => it.stage.Contains(textData));
                exp.AndIF(optionData == "stageDesc", it => it.stationType.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.stationTypeCustomer.Contains(textData));
                exp.AndIF(optionData == "updateTime", it => it.operateType.Equals(textData));
                exp.AndIF(optionData == "updateTime", it => it.clientType.Equals(textData));
                exp.AndIF(optionData == "updateTime", it => it.stationTypeSeq.Equals(textData));
                exp.AndIF(optionData == "updateTime", it => it.customerStationDesc.Equals(textData));
                exp.AndIF(optionData == "updateTime", it => it.customerStationDesc.Equals(textData));
            }
            var expPagedInfo = Context.Queryable<StationTypeInfo>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
            return expPagedInfo;
        }

        public int InsertStationTypeInfo(StationTypeInfo stationTypeInfo)   
        {

            int MaxId = Context.Queryable<StationTypeInfo>().Max(it => it.id) + 1;
            stationTypeInfo.id = MaxId;

            stationTypeInfo.enabled = "Y";

            int insertErp = Context.Insertable(stationTypeInfo).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_STATION_TYPE_ht (select *  from  IMES.M_STATION_TYPE  where id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }

        public int UpdateStationType(StationTypeInfo stationTypeInfo)
        {
            int UpdateStage = Context.Updateable(stationTypeInfo).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.id).ExecuteCommand();
            if (UpdateStage > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_STATION_TYPE_ht(select * from  IMES.M_STATION_TYPE  where id = '" + stationTypeInfo.id + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return UpdateStage;
        }

        public int DeleteStationTypeByID(StationTypeInfo stationTypeInfo)
        {
            {
                int updateLine = Context.Updateable(stationTypeInfo).UpdateColumns(it => new { it.updateEmpno, it.updateTime }).WhereColumns(it => new { it.id }).ExecuteCommand();
                if (updateLine > 0)
                {
                    string sqlStr = $"insert INTO SAJET.M_STATION_TYPE_ht(select * FROM SAJET.M_STATION_TYPE   where id = " + stationTypeInfo.id + ")";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return Context.Deleteable<StationTypeInfo>().Where(it => it.id == stationTypeInfo.id).ExecuteCommand();
                }

                return 0;
            }
        }
        public List<StationTypeInfo> StationTypeHistory(string id)
        {
            String sqlStr = $"select *  FROM SAJET.M_STATION_TYPE_ht  b  where 1=1 ";

            if (id != null && id != "")
            {
                sqlStr += " and b.id ='" + id + "'";
            }

            List<StationTypeInfo> list = Context.Ado.SqlQuery<StationTypeInfo>(sqlStr);

            return list;
        }

        /// <summary>
        /// 导入
        /// </summary>
        public (string, object, object) StationTypeImportData(List<StationTypeInfo> stationTypes, string site, string name)
        {
            try
            {
                int add = 0;
                int up = 0;
                stationTypes.ForEach(x =>
                {
                    x.createTime = DateTime.Now;
                    x.updateTime = DateTime.Now;
                    x.createEmpno = name;
                    x.updateEmpno = name;
                    x.site = site;
                    if (x.id == 0)
                    {
                        x.id = Context.Queryable<StationTypeInfo>().Max(it => it.id) + 1;
                        int Insertable = Context.Insertable(x).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                        if (Insertable > 0)
                        {
                            add++;
                        }
                    }
                    else
                    {
                        int UpdateStage = Context.Updateable(x).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.id).ExecuteCommand();
                        up++;
                    }
                    Context.Ado.SqlQuery<Object>($"insert INTO SAJET.M_STATION_TYPE_ht (select * from  IMES.M_STATION_TYPE   where id = " + x.id + ")");

                });
                return ("数量：" + stationTypes.Count, " 添加;" + add, " 修改;" + up);
            }
            catch (Exception e)
            {
                return ("站点/站点类型重复，请检查！", " 添加;0", " 修改;0");
            }
        }

        public List<string> GetStationType()
        {
            String sqlStr = $"  Select SCAN_TYPE FROM SAJET.M_OPERATE_TYPE order by SCAN_TYPE ";

            List<string> list = Context.Ado.SqlQuery<string>(sqlStr);

            return list;

        }

        public List<string> GetClientType()
        {
            //知道这么写不对，但是不管从之前代码还是提供表都找不到维护CLIENT_TYPE表名
            String sqlStr = $"Select  distinct(CLIENT_TYPE)  from  IMES.M_STATION_TYPE  ";

            List<string> list = Context.Ado.SqlQuery<string>(sqlStr);

            return list;
        }

        public List<RouteInfo> GetRouteName(string routeName, string site)

        {
            string enaBled = "Y";
            var exp = Expressionable.Create<RouteInfo>();
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enaBled == enaBled);
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(routeName != "" && routeName != null, it => it.routeName == routeName);

            return Context.Queryable<RouteInfo>().Where(exp.ToExpression()).ToList();

        }

        public List<RouteMaintenanceInfoVo> GetStationTypeInfo(string stage, string site)
        {
            string enaBled = "Y";
            var exp = Context.Queryable<StationTypeInfo>()
            .LeftJoin<StageInfo>((a, b) => a.stage == b.stage)
            .RightJoin<OperateTypeInfo>((a, b, c) => a.operateType == c.scanType)
            .WhereIF(enaBled != null && enaBled != "", (a, b, c) => a.enabled == enaBled && b.enabled == enaBled)
            .WhereIF(site != null && site != "", a => a.site == site);
            if (stage != null && stage != "")
            {
                exp.Where((a, b, c) => b.stage.Contains(stage));
            }
            var result = exp.OrderBy((a, b, c) => b.id).OrderBy((a, b, c) => a.id)
              .Select((a, b, c) =>
               new RouteMaintenanceInfoVo
               {
                   stageId = b.id,
                   stage = b.stage,
                   stationTypeId = a.id,
                   stationType = a.stationType,
                   scanType = c.scanType.ToUpper(),
                   stationTypeDesc = a.stationTypeDesc,
                   clientType = a.clientType,

               });
            return result.ToList();
        }

        public List<RouteMaintenanceInfoVo> GetRouteDetail(string routeName, string site)
        {
            string enaBled = "Y";
            var exp = Context.Queryable<RouteInfo>()
            .LeftJoin<RouteDetailInfo>((a, b) => a.routeName == b.routeName)
            .LeftJoin<StationTypeInfo>((a, b, c) => b.stationType == c.stationType)
            .LeftJoin<StationTypeInfo>((a, b, c, d) => b.nextStationType == d.stationType)
            .LeftJoin<OperateTypeInfo>((a, b, c, d, e) => d.operateType == e.scanType)
            .WhereIF(enaBled != null && enaBled != "", (a, b, c, d, e) => a.enaBled == enaBled)
            .WhereIF(site != null && site != "", a => a.site == site);
            if (routeName != null && routeName != "")
            {
                exp.Where((a, b, c, d, e) => a.routeName.Contains(routeName));
            }
            var result = exp.OrderBy((a, b, c, d, e) => b.step).OrderBy((a, b, c, d, e) => b.seq)
              .Select((a, b, c, d, e) =>
               new RouteMaintenanceInfoVo
               {
                   routeName = a.routeName,
                   stationType = b.stationType,
                   stationTypeId = c.id,
                   stationTypeDesc = c.stationTypeDesc,
                   nextStationType = b.nextStationType,
                   nextstationTypeDesc = d.stationTypeDesc,
                   scanType = e.scanType,
                   necessary = b.necessary,
                   result = b.result,
                   seq = b.seq,
                   step = b.step,
                   enabled = d.enabled
               });
            return result.ToList();
        }
        public List<string> CheckRouteWIP(string routeName, string site)
        {
            String sqlStr = $"SELECT  work_order  FROM SAJET.P_WO_BASE  where ROUTE_NAME = '" + routeName + "'   AND WO_STATUS IN ('2','3')";

            List<string> list = Context.Ado.SqlQuery<string>(sqlStr);

            return list;

        }

        public int UpdateRoute(RouteInfo routeInfo)
        {
            routeInfo.enaBled = "N";
            int UpdateDisable = Context.Updateable(routeInfo).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.routeName).WhereColumns(it => it.enaBled == "Y").ExecuteCommand();
            if (UpdateDisable > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_ROUTE_ht(select * from  IMES.M_ROUTE  where routeName = '" + routeInfo.routeName + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return UpdateDisable;
        }

        public int UpdateMustStation(RouteDetailInfo routeDetailInfo)
        {
            int updateStation = Context.Updateable(routeDetailInfo).UpdateColumns(it => it.enaBled).WhereColumns(it => it.routeName).WhereColumns(it => it.stationType).WhereColumns(it => it.result == 1).ExecuteCommand();
            if (updateStation > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_ROUTE_DETAIL_ht(select * from  IMES.M_ROUTE_DETAIL  where routeName = '" + routeDetailInfo.routeName + "' and stationType = '" + routeDetailInfo.stationType + "' and stationType = '" + 1 + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;

            }
            return updateStation;
        }

        public int CheckRouteName(List<RouteDetailVo> routeDetailVo, string site, string name)

        {
            string routeName = routeDetailVo[0].routeName;
            string enaBled = "Y";
            int insertRouteDetail = 0;
            RouteDetailInfo routeDetailInfos = new RouteDetailInfo();
            var exp = Expressionable.Create<RouteInfo>();
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enaBled == enaBled);
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(routeName != "" && routeName != null, it => it.routeName == routeName);

            int queryCount = Context.Queryable<RouteInfo>().Where(exp.ToExpression()).Count();
            if (queryCount <= 0)
            {
                RouteInfo routeInfo = new RouteInfo();
                int MaxId = Context.Queryable<RouteInfo>().Max(it => it.id) + 1;
                routeInfo.id = MaxId; routeInfo.routeName = routeName; routeInfo.enaBled = enaBled;
                routeInfo.createTime = DateTime.Now; routeInfo.createEmpno = name;
                routeInfo.updateTime = DateTime.Now; routeInfo.updateEmpno = name; routeInfo.site = site;
                Context.Insertable(routeInfo).ExecuteCommand();
            }
            List<RouteDetailInfo> routeDetailInfo = Context.Queryable<RouteDetailInfo>().Where(it => it.routeName == routeName).ToList();
            if (routeDetailInfo.Count > 0)
            {
                int insertRouteDetailHt = Context.Insertable(routeDetailInfo).AS("SAJET.M_ROUTE_DETAIL_HT").ExecuteCommand();
                if (insertRouteDetailHt > 0)
                {

                    int DeleteRouteDetail = Context.Deleteable<RouteDetailInfo>().Where(it => it.routeName == routeName).ExecuteCommand();
                    if (DeleteRouteDetail > 0)
                    {
                        foreach (RouteDetailVo entity in routeDetailVo)
                        {

                            routeDetailInfos.routeName = entity.routeName;
                            routeDetailInfos.stationType = entity.stationType;
                            routeDetailInfos.nextStationType = entity.nextStationType;
                            routeDetailInfos.seq = (int)entity.seq;
                            routeDetailInfos.result = (int)entity.result;
                            routeDetailInfos.pdCode = entity.pdCode;
                            routeDetailInfos.necessary = entity.necessary;
                            routeDetailInfos.step = (int)entity.step;
                            routeDetailInfos.defaultInStationType = entity.defaultInStationType;
                            routeDetailInfos.createTime = DateTime.Now;
                            routeDetailInfos.createEmpno = name;
                            routeDetailInfos.updateTime = DateTime.Now; routeDetailInfos.updateEmpno = name; ;
                            routeDetailInfos.site = site; routeDetailInfos.enaBled = enaBled;
                            insertRouteDetail = Context.Insertable(routeDetailInfos).AS("SAJET.M_ROUTE_DETAIL").ExecuteCommand();

                        }
                        return insertRouteDetail;
                    }
                }
            }
                else
                {

                     foreach (RouteDetailVo entity in routeDetailVo)
                     {
                           routeDetailInfos.routeName = entity.routeName;
                           routeDetailInfos.stationType = entity.stationType;
                           routeDetailInfos.nextStationType = entity.nextStationType;
                           routeDetailInfos.seq = (int)entity.seq;
                           routeDetailInfos.result = (int)entity.result;
                           routeDetailInfos.pdCode = entity.pdCode;
                           routeDetailInfos.necessary = entity.necessary;
                           routeDetailInfos.step = (int)entity.step;
                           routeDetailInfos.defaultInStationType = entity.defaultInStationType;
                           routeDetailInfos.createTime = DateTime.Now;
                           routeDetailInfos.createEmpno = name;
                           routeDetailInfos.updateTime = DateTime.Now; routeDetailInfos.updateEmpno = name; ;
                           routeDetailInfos.site = site; routeDetailInfos.enaBled = enaBled;
                           insertRouteDetail = Context.Insertable(routeDetailInfos).AS("SAJET.M_ROUTE_DETAIL").ExecuteCommand();

                     }
                    return insertRouteDetail;
                }
            return 0;
        }
         
     }
 }
        
    
