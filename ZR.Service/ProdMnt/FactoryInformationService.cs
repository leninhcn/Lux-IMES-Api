using Infrastructure.Attribute;
using Infrastructure.Extensions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Repository;
using ZR.Service.IService;
using static ZR.Model.Dto.LabelRuleDto;

namespace ZR.Service.ProdMnt
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class FactoryInformationService : BaseService<SnStatus>, IFactoryInformationService
    {
        public int DeleteSiteByID(SiteInfo siteInfo)
        {

            int updateSite = Context.Updateable(siteInfo).UpdateColumns(it => new { it.updateEmpno, it.updateTime }).WhereColumns(it => new { it.id }).ExecuteCommand();
            if (updateSite > 0)
            {
                string sqlStr = $"insert into  IMES.M_SITE_HT(select * FROM SAJET.M_SITE   where id = " + siteInfo.id + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return Context.Deleteable<SiteInfo>().Where(it => it.id == siteInfo.id).ExecuteCommand();
            }
            return 0;

        }

        public int InsertSiteInfo(SiteInfo siteInfo)
        {
            int MaxId = Context.Queryable<SiteInfo>().Max(it => it.id) + 1;
            siteInfo.id = MaxId;


            int insertErp = Context.Insertable(siteInfo).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_SITE_HT(select * FROM SAJET.M_SITE   where id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }

        public PagedInfo<SiteInfo> ShowSiteInfo(string enaBled, string optionData, string textData, int pageNum, int pageSize)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<SiteInfo>();
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "id", it => it.id.Equals(textData));
                exp.AndIF(optionData == "site", it => it.site.Contains(textData));
                exp.AndIF(optionData == "siteCustomer", it => it.siteCustomer.Contains(textData));
                exp.AndIF(optionData == "siteDesc", it => it.siteDesc.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
                exp.AndIF(optionData == "updateTime", it => it.updateTime.Equals(textData));
                exp.AndIF(optionData == "createTime", it => it.createTime.Equals(textData));
                exp.AndIF(optionData == "createEmpno", it => it.createEmpno.Contains(textData));
                exp.AndIF(optionData == "enabled", it => it.enabled.Equals(textData));
            }
            var expPagedInfo = Context.Queryable<SiteInfo>().Where(exp.ToExpression()).OrderBy(it => it.createTime);

            return expPagedInfo.ToPage(pager);

        }

        public List<SiteInfo> SiteHistory(string id)
        {

            String sqlStr = $"select *  from   IMES.M_SITE_HT  b  where 1=1 ";

            if (id != null && id != "")
            {
                sqlStr += " and b.id ='" + id + "'";
            }
            List<SiteInfo> list = Context.Ado.SqlQuery<SiteInfo>(sqlStr);

            return list;
        }

        public int UpdateSite(SiteInfo siteInfo)
        {
            int updateStation = Context.Updateable(siteInfo).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.id).ExecuteCommand();
            if (updateStation > 0)
            {
                string sqlStr = $"insert into  IMES.M_SITE_HT(select * FROM SAJET.M_SITE   where id = " + siteInfo.id + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return 0;
        }
        public List<string> GetSiteCode()
        {
            String sqlStr = $"select distinct(a.site) FROM SAJET.m_site a WHERE a.enabled='Y'  ";

            List<string> list = Context.Ado.SqlQuery<string>(sqlStr);

            return list;
        }

        public List<string> GetLineType()
        {

            String sqlStr = $" select distinct(a.line_type) FROM SAJET.m_line_type a WHERE a.enabled = 'Y'  ";

            List<string> list = Context.Ado.SqlQuery<string>(sqlStr);

            return list;
        }
        public List<string> GetLineLevel()
        {

            String sqlStr = $"select distinct(a.LINE_LEVEL)  FROM SAJET.m_line a WHERE a.enabled = 'Y' ";


            List<string> list = Context.Ado.SqlQuery<string>(sqlStr);

            return list;
        }
        public List<string> GetWorkCenter()
        {
            String sqlStr = $"select distinct(WORK_CENTER)  from  imes.m_line a WHERE a.enabled = 'Y' and  a.work_center is not null ";


            List<string> list = Context.Ado.SqlQuery<string>(sqlStr);

            return list;
        }

        public int DeleteLineByID(LineInfo lineInfo)
        {
            int updateLine = Context.Updateable(lineInfo).UpdateColumns(it => new { it.updateEmpno, it.updateTime }).WhereColumns(it => new { it.id }).ExecuteCommand();
            if (updateLine > 0)
            {
                string sqlStr = $"insert into  imes.m_line_HT(select * FROM SAJET.m_line   where id = " + lineInfo.id + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return Context.Deleteable<LineInfo>().Where(it => it.id == lineInfo.id).ExecuteCommand();
            }
            return 0;
        }

        public int UpdateLine(LineInfo lineInfo)
        {
            int updateLine = Context.Updateable(lineInfo).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.id).ExecuteCommand();
            if (updateLine > 0)
            {
                string sqlStr = $"insert into  imes.m_line_HT(select * FROM SAJET.m_line   where id = " + lineInfo.id + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return 0;
        }

        public int InsertLineInfo(LineInfo lineInfo)
        {
            int count = Context.Queryable<LineInfo>().Where(it => it.line == lineInfo.line).Count();
            if (count > 0)
            {
                return 2;
            }
            int MaxId = Context.Queryable<LineInfo>().Max(it => it.id) + 1;
            lineInfo.id = MaxId;


            int insertErp = Context.Insertable(lineInfo).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"insert INTO SAJET.m_line_HT(select * from  imes.m_line   where id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }

        public PagedInfo<LineInfo> ShowLineInfo(string enaBled, string optionData, string textData, int pageNum, int pageSize)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<LineInfo>();
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "id", it => it.id.Equals(textData));
                exp.AndIF(optionData == "site", it => it.site.Contains(textData));
                exp.AndIF(optionData == "line", it => it.line.Contains(textData));
                exp.AndIF(optionData == "lineDesc", it => it.lineDesc.Contains(textData));
                exp.AndIF(optionData == "lineCustomer", it => it.lineCustomer.Contains(textData));
                exp.AndIF(optionData == "lineSap", it => it.lineSap.Equals(textData));
                exp.AndIF(optionData == "lineType", it => it.lineType.Equals(textData));
                exp.AndIF(optionData == "lineLevel", it => it.lineLevel.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
                exp.AndIF(optionData == "updateTime", it => it.updateTime.Equals(textData));
                exp.AndIF(optionData == "workCenter", it => it.workCenter.Equals(textData));
                exp.AndIF(optionData == "enabled", it => it.enabled.Equals(textData));
            }
            var expPagedInfo = Context.Queryable<LineInfo>().Where(exp.ToExpression()).OrderBy(it => it.id);

            return expPagedInfo.ToPage(pager);

        }

        public List<LineInfo> LineHistory(string id)
        {
            String sqlStr = $"select *  from  imes.m_line_HT  b  where 1=1 ";

            if (id != null && id != "")
            {
                sqlStr += " and b.id ='" + id + "'";
            }
            List<LineInfo> list = Context.Ado.SqlQuery<LineInfo>(sqlStr);

            return list;
        }

        public (string, object, object) LineImportData(List<LineInfo> line, string site, string name)
        {
            try
            {
                int add = 0;
                int up = 0;
                line.ForEach(x =>
                {
                    x.createTime = DateTime.Now;
                    x.updateTime = DateTime.Now;
                    x.createEmpno = name;
                    x.updateEmpno = name;
                    x.site = site;
                    if (x.id == 0)
                    {
                        x.id = Context.Queryable<LineInfo>().Max(it => it.id) + 1;
                        int Insertable = Context.Insertable(x).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                        if (Insertable > 0)
                        {
                            add++;
                        }
                    }
                    else
                    {
                        int UpdateLine = Context.Updateable(x).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.id).ExecuteCommand();
                        up++;
                    }
                    Context.Ado.SqlQuery<Object>($"insert into  imes.m_line_ht (select * from   imes.m_line   where id = " + x.id + ")");
                    
                });
                return ("数量：" + line.Count, " 添加;" + add, " 修改;" + up);
            }
            catch (Exception e)
            {
                return ("站点/站点类型重复，请检查！", " 添加;0", " 修改;0");
            }
        }

        public List<StationMaintenanceVo> GetLine(string line, string site)
        {
            string enaBled = "Y";
            var exp = Context.Queryable<SiteInfo>()
            .LeftJoin<LineInfo>((a, b) => a.site == b.site)
            .WhereIF(enaBled != null && enaBled != "", (a, b) => a.enabled == enaBled && b.enabled == enaBled)
            .WhereIF(site != null && site != "", a => a.site == site);
            if (line != null && line != "")
            {
                exp.Where((a, b) => b.line.Contains(line));
            }
            var result = exp.OrderBy((a, b) => b.line)
              .Select((a, b) =>
               new StationMaintenanceVo
               {
                   line = b.line,
                   lineCustomer = b.lineCustomer,
                   site = a.site,
                   siteDesc = a.siteDesc
               });
            return result.ToList();
        }

        public List<StationMaintenanceVo> GetLineStation(string line, string site)
        {
            string enaBled = "Y";
            var exp = Context.Queryable<LineInfo>()
            .LeftJoin<StationInfo>((a, b) => a.line == b.line)
            .LeftJoin<StationTypeInfo>((a, b, c) => b.stationType == c.stationType || b.stationType == c.stationTypeCustomer && b.stage == c.stage)
            .WhereIF(enaBled != null && enaBled != "", (a, b, c) => a.enabled == enaBled && b.enabled == enaBled && c.enabled == enaBled)
            .WhereIF(site != null && site != "", a => a.site == site);
            if (line != null && line != "")
            {
                exp.Where(a => a.line.Contains(line));
            }
            var result = exp.OrderBy((a, b, c) => a.site)
              .Select((a, b, c) =>
               new StationMaintenanceVo
               {
                   site = a.site,
                   line = a.line,
                   id = b.id,
                   stationId = b.stationId,
                   stationType = c.stationType,
                   stationName = b.stationName,
                   stationTypes = c.stationType,
                   stationTypesDesc = c.stationTypeDesc,
                   stage = c.stage,
                   enabled = b.enabled

               });
            return result.ToList();
        }

        public List<StationMaintenanceVo> GetStageStationtype(string stage, string site)
        {
            string enaBled = "Y";
            var exp = Context.Queryable<StageInfo>()
            .LeftJoin<StationTypeInfo>((a, b) => a.stage == b.stage)
            .WhereIF(enaBled != null && enaBled != "", (a, b) => a.enabled == enaBled && b.enabled == enaBled)
            .WhereIF(site != null && site != "", a => a.site == site);
            if (stage != null && stage != "")
            {
                exp.Where(a => a.stage.Contains(stage));
            }
            var result = exp.OrderBy((a, b) => b.stage)
              .Select((a, b) =>
               new StationMaintenanceVo
               {
                   stage = b.stage,
                   stationType = b.stationType,
                   stationTypesDesc = b.stationTypeDesc,
                   clientType = b.clientType
               });
            return result.ToList();


        }

        public List<StationVo> GetLineStationOutExport(string line, string site)
        {
            string enaBled = "Y";
            var exp = Context.Queryable<LineInfo>()
            .LeftJoin<StationInfo>((a, b) => a.line == b.line)
            .LeftJoin<StationTypeInfo>((a, b, c) => b.stationType == c.stationType || b.stationType == c.stationTypeCustomer && b.stage == c.stage)
            .WhereIF(enaBled != null && enaBled != "", (a, b, c) => a.enabled == enaBled && b.enabled == enaBled && c.enabled == enaBled)
            .WhereIF(site != null && site != "", a => a.site == site);
            if (line != null && line != "")
            {
                exp.Where((a, b) => a.line.Contains(line));
            }
            var result = exp.OrderBy((a, b, c) => a.site)
              .Select((a, b, c) =>
               new StationVo
               {

                   line = a.line,
                   stationId = b.stationId,
                   stationTypesCustomer = c.stationTypeCustomer,
                   stage = c.stage,


               });
            return result.ToList();
        }
        public (string, object, object) StationImportData(List<StationInfo> line, string site, string name)
        {
            try
            {
                int add = 0;
                int up = 0;
                line.ForEach(x =>
                {
                    x.createTime = DateTime.Now;
                    x.updateTime = DateTime.Now;
                    x.createEmpno = name;
                    x.updateEmpno = name;
                    x.site = site;
                    if (x.id == 0)
                    {
                        x.id = Context.Queryable<StationInfo>().Max(it => it.id) + 1;
                        int Insertable = Context.Insertable(x).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                        if (Insertable > 0)
                        {
                            add++;
                        }
                    }
                    else
                    {
                        int UpdateLine = Context.Updateable(x).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.id).ExecuteCommand();
                        up++;
                    }
                    Context.Ado.SqlQuery<Object>($"insert into  IMES.M_STATION_ht (select * from   imes.m_line   where id = " + x.id + ")");

                });
                return ("数量：" + line.Count, " 添加;" + add, " 修改;" + up);
            }
            catch (Exception e)
            {
                return ("站点/站点类型重复，请检查！", " 添加;0", " 修改;0");
            }
        }


        public int InsertStationInfo(StationInfoVo stationInfoVo)
        {
            StationInfo stationInfo = new StationInfo();
            int MaxId = Context.Queryable<StationInfo>().Max(it => it.id);
            int MaxStationId = Context.Queryable<StationInfo>().Where(it => it.stationType == stationInfoVo.stationType && it.line == stationInfoVo.line && it.stage == stationInfoVo.stage).Max(it => it.stationId);
            stationInfo.stationType = stationInfoVo.stationType;
            stationInfo.line = stationInfoVo.line;
            stationInfo.stage = stationInfoVo.stage;
            stationInfo.site = stationInfoVo.site;
            stationInfo.updateEmpno = stationInfoVo.updateEmpno;
            stationInfo.updateTime = DateTime.Now;
            stationInfo.createEmpno = stationInfoVo.createEmpno;
            stationInfo.createTime = DateTime.Now;
            stationInfo.enabled = "Y";
            stationInfo.maxQty = "1";
            stationInfo.passQty = "1";
            stationInfo.fallQty = "1";
            var number = stationInfoVo.number;
            for (int i = 1; i <= number; i++)
            {

                stationInfo.id = MaxId + i;
                stationInfo.stationId = MaxStationId + i;
                stationInfo.stationName = stationInfo.site + "_" + stationInfo.line + "_" + stationInfo.stationId + "_" + stationInfo.stationType;
                int insertErp = Context.Insertable(stationInfo).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

                if (insertErp > 0)
                {
                    string sqlStr = $"insert INTO SAJET.M_STATION_HT(select * from  IMES.M_STATION   where id = " + stationInfo.id;
                    Context.Ado.SqlQuery<string>(sqlStr + ")");
                }
            }
            return 1;
        }

        public int DeleteStation(StationInfoVo stationInfoVo)
        {
            var exp = Expressionable.Create<StationInfo>();
            exp.AndIF(stationInfoVo.line != "" && stationInfoVo.line != null, it => it.line == stationInfoVo.line);
            exp.AndIF(stationInfoVo.stage != "" && stationInfoVo.stage != null, it => it.stage == stationInfoVo.stage);
            exp.AndIF(stationInfoVo.stationType != "" && stationInfoVo.stationType != null, it => it.stationType == stationInfoVo.stationType);
            if (stationInfoVo.number != null)
            {
                int MaxStationId = Context.Queryable<StationInfo>().Where(exp.ToExpression()).Max(it => it.stationId);
                exp.AndIF(stationInfoVo.number.HasValue, it => it.stationId == MaxStationId);
                List<StationInfo> stationInfos = Context.Queryable<StationInfo>().Where(exp.ToExpression()).ToList();
                int insertStatin = Context.Insertable(stationInfos).AS("SAJET.M_STATION_HT").ExecuteCommand();
                if (insertStatin > 0)
                {
                    int DeleteStationCount = Context.Deleteable<StationInfo>().Where(exp.ToExpression()).ExecuteCommand();
                    if (DeleteStationCount > 0) {
                        return 1;
                    }      
                }
                return 0;
            }else
            {
                List<StationInfo> stationInfos = Context.Queryable<StationInfo>().Where(exp.ToExpression()).ToList();
                int insertStatin = Context.Insertable(stationInfos).AS("SAJET.M_STATION_HT").ExecuteCommand();
                if (insertStatin > 0)
                {
                    int DeleteStationCount = Context.Deleteable<StationInfo>().Where(exp.ToExpression()).ExecuteCommand();
                    if (DeleteStationCount > 0)
                    {
                        return 1;
                    }else { return 0; }
                }
                else {  return 0; } 
              
            }
        }
    }
}
        
        
    
  
