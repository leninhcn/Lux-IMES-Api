using Infrastructure;
using Infrastructure.Attribute;
using Mapster.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ZR.Common;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Model.System;
using ZR.Repository;
using ZR.Service.IService;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace ZR.Service.PordService
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    internal class MntnErpMesService : BaseService<SnStatus>, IMntnErpMesService
    {

        public PagedInfo<ImesMpartSpecErpMesMapping> ErpMeslist(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            /*String sqlStr = $" select * FROM SAJET.M_PART_SPEC_ERP_MES_MAPPING  where 1=1";
            if (textData != "" && optionData != null && textData != null)
            {
                sqlStr = sqlStr + " and " + optionData + " LIKE '%" + textData + "%' ";
            }
            if (enaBled == "Y")
            {
                sqlStr = sqlStr + " and enabled = 'Y' ";
            }
            sqlStr = sqlStr + "order by MES_SPEC asc ";*/

            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMpartSpecErpMesMapping>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "mesSpec", it => it.mesSpec.Contains(textData));
                exp.AndIF(optionData == "erpSpec", it => it.erpSpec.Contains(textData));
                exp.AndIF(optionData == "model", it => it.model.ToString().Contains(textData));
                exp.AndIF(optionData == "category", it => it.category.Contains(textData));
                exp.AndIF(optionData == "stage", it => it.stage.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateTime.ToString().Contains(textData));
                exp.AndIF(optionData == "customerSpec", it => it.customerSpec.Contains(textData));
            }
            return Context.Queryable<ImesMpartSpecErpMesMapping>().Where(exp.ToExpression()).OrderBy(it => it.mesSpec).ToPage(pager);
        }

        /// <summary>
        /// 导入
        /// </summary>
        public (string, object, object) ImportErp(List<ImesMpartSpecErpMesMapping> imes, string site, string name)
        {
            try { 
                int addNum = 0;
                int errNum = 0;
                int upNum = 0;
                imes.ForEach(x =>
                {
                    x.createTime = DateTime.Now;
                    x.updateTime = DateTime.Now;
                    x.createEmpno = name;
                    x.updateEmpno = name;
                    x.site = site;
                    if (x.id == 0)
                    {
                        x.id = Context.Queryable<ImesMpartSpecErpMesMapping>().Max(it => it.id) + 1;
                        int MesInsert = this.ErpMesInsert(x);
                        if (MesInsert > 0)
                        {
                            addNum++;
                        }
                        else 
                        { 
                            errNum++;
                        }
                    
                    }
                    else
                    {
                        int MesUpdate = this.ErpMesUpdate(x);
                        if (MesUpdate > 0)
                        {
                            upNum++;
                        }
                        else
                        {
                            errNum++;
                        }
                    }
                });
                string msg = "总数:" + imes.Count;
                return (msg, " 添加:"+addNum, " 修改:"+upNum+" 失败:"+errNum);
            }catch (Exception e)
            {
                return ("上传失败，总数:", imes.Count+ " 添加:0" , " 修改:0");
            }
        }

        public object ErpMeslModellist(string site)
        {
            String sqlStr = $" SELECT distinct MODEL  FROM SAJET.M_MODEL where 1=1 ";

            if (site != null && site != "") 
            {
                sqlStr = sqlStr + " and site = '" + site + "'";
            }

            sqlStr = sqlStr + " ORDER BY MODEL";
            return Context.Ado.SqlQuery<Object>(sqlStr);
        }

        public object ErpMeslStageNamelist(string site)
        {
            String sqlStr = $" select distinct stage FROM SAJET.m_stage where ENABLED='Y' ";
            if (site != null && site != "")
            {
                sqlStr = sqlStr + " and site = '" + site + "'";
            }

            sqlStr = sqlStr + " ORDER BY stage";

            return Context.Ado.SqlQuery<Object>(sqlStr);
        }

        public int ErpMesInsert(ImesMpartSpecErpMesMapping imesMpart)
        {
            string site = imesMpart.site;
            int count = Context.Queryable<ImesMpartSpecErpMesMapping>()
                .Where(it => it.mesSpec==imesMpart.mesSpec && it.model == imesMpart.model)
                .Count();
            if (count < 1)
            {
                imesMpart.updateTime = DateTime.Now;
                imesMpart.createTime = DateTime.Now;
                int id = imesMpart.id;
                int MaxId = Context.Queryable<ImesMpartSpecErpMesMapping>().Max(it => it.id);
                imesMpart.id = MaxId + 1;
                int insertErp = Context.Insertable(imesMpart).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                string insertHt = $"Insert INTO SAJET.M_PART_SPEC_ERP_MES_MAPPING_HT (Select * FROM SAJET.M_PART_SPEC_ERP_MES_MAPPING where id = " + id ;
                if (site != null && site != "")
                {
                    insertHt = insertHt + " and site = '" + site + "'";
                }
                Context.Ado.SqlQuery<Object>(insertHt+ ")");
                return insertErp;
            }
            return 0;
        }

        public int ErpMesUpdate(ImesMpartSpecErpMesMapping imesMpart)
        {
            int id= imesMpart.id;
            string site = imesMpart.site;
            string insertHt = $"Insert INTO SAJET.M_PART_SPEC_ERP_MES_MAPPING_HT (Select * FROM SAJET.M_PART_SPEC_ERP_MES_MAPPING where id = " + id ;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            imesMpart.updateTime = DateTime.Now;
            return Context.Updateable(imesMpart).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
        }

        public int ErpMesDelet(ImesMpartSpecErpMesMapping imesMpart)
        {
            int id = imesMpart.id;
            string site = imesMpart.site;
            imesMpart.updateTime= DateTime.Now;
            Context.Updateable(imesMpart).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            string insertHt = $"Insert INTO SAJET.M_PART_SPEC_ERP_MES_MAPPING_HT (Select * FROM SAJET.M_PART_SPEC_ERP_MES_MAPPING where id = " + id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");

            return Context.Deleteable<ImesMpartSpecErpMesMapping>().Where(it => it.id == id && it.site == site).ExecuteCommand();
        }

        public object ErpMeslistHt(int Id, string site)
        {
            string listHt = $"select * FROM SAJET.M_PART_SPEC_ERP_MES_MAPPING_HT  where ID = " + Id;
            if (site != null && site != "")
            {
                listHt = listHt + " and site = '" + site + "'";
            }
            return Context.Ado.SqlQuery<Object>(listHt);
        }

        //-----------------分割线-------------------------------------------------------------------------------------


        public PagedInfo<ImesMstationTypePartSpec> Stationlist(string enaBled, string optionData, string textData, string site, int pageNum, int pageSize)
        {
            /*String sqlStr = @"SELECT MODEL,STATION_TYPE,STATION_DESC,KP_SPEC,KP_SPEC_DESC,UPDATE_EMPNO,UPDATE_TIME,CREATE_TIME,ENABLED,ID FROM SAJET.M_STATIONTYPE_PARTSPEC WHERE 1=1";

            if (textData != "" && optionData != null && textData != null)
            {
                sqlStr = sqlStr + " and " + optionData + " LIKE '%" + textData + "%' ";
            }

            if (enaBled == "Y")
            {
                sqlStr = sqlStr + " and enabled = 'Y' ";
            }
            else if (enaBled == "N")
            {
                sqlStr = sqlStr + " and enabled = 'N' ";
            }
            sqlStr = sqlStr + "order by MES_SPEC asc ";
            Object list = Context.Ado.SqlQuery<Object>(sqlStr);
            return list;*/

            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMstationTypePartSpec>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);

            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "model", it => it.model.Contains(textData));
                exp.AndIF(optionData == "stationType", it => it.stationType.Contains(textData));
                exp.AndIF(optionData == "stationDesc", it => it.stationDesc.ToString().Contains(textData));
                exp.AndIF(optionData == "kpSpec", it => it.kpSpec.Contains(textData));
                exp.AndIF(optionData == "kpSpecDesc", it => it.kpSpecDesc.Contains(textData));
            }
            return Context.Queryable<ImesMstationTypePartSpec>().Where(exp.ToExpression()).OrderBy(it => it.model).ToPage(pager);

        }



        public object StationStagelist(string site)
        {
            String sqlStr = @" SELECT DISTINCT STAGE FROM SAJET.M_STAGE WHERE ENABLED = 'Y' ";
            if (site != null && site != "")
            {
                sqlStr = sqlStr + " and site = '" + site + "'";
            }
            return Context.Ado.SqlQuery<Object>(sqlStr + "order by STAGE");
        }

        public (string, object, object) StationImportData(List<ImesMstationTypePartSpec> imes, string site, string name)
        {
            try { 
                int addNum = 0;
                int errNum = 0;
                int upNum = 0;
                imes.ForEach(x =>
                {
                    x.createTime = DateTime.Now;
                    x.updateTime = DateTime.Now;
                    x.createEmpno = name;
                    x.updateEmpno = name;
                    x.site = site;
                    if (x.id == 0)
                    {
                        x.id = Context.Queryable<ImesMstationTypePartSpec>().Max(it => it.id) + 1;
                        if (this.StationInsert(x) > 0)
                        {
                            addNum++;
                        }
                        else 
                        { 
                            errNum++;
                        }
                    }
                    else 
                    {
                        if (this.StationtUpdate(x) > 0)
                        {
                            upNum++;
                        }
                        else 
                        { 
                            errNum++;
                        }
                    
                    }
                });
                string msg = "总数:" + imes.Count;
                return (msg, " 添加:" + addNum, " 修改:" + upNum + " 失败:" + errNum);
            }
            catch (Exception ex) {

                return ("上传失败，总数:", imes.Count + " 添加:0", " 修改:0");
            }
        }

        public object StationTypelist(string stage, string stationType, string site)
        {
            string sql = string.Format(@"SELECT STATION_TYPE,STAGE,STATION_TYPE_SEQ,OPERATE_TYPE,STATION_TYPE_DESC FROM SAJET.M_STATION_TYPE WHERE ENABLED = 'Y' AND  SITE = '" + site + "' and STAGE = '" + stage + "'");
            if (!string.IsNullOrWhiteSpace(stationType))
                sql = sql + " AND STATION_TYPE LIKE '%" + stationType + "%'";
            return Context.Ado.SqlQuery<Object>(sql);
        }

        public object StationModellist(string site)
        {
            String sqlStr = @" SELECT DISTINCT MODEL FROM SAJET.M_MODEL WHERE ENABLED = 'Y' ";
            if (site != null && site != "")
            {
                sqlStr = sqlStr + " and site = '" + site + "'";
            }
            return Context.Ado.SqlQuery<Object>(sqlStr + " order by MODEL");
        }

        public object StationBrandlist(string mesSpec, string stage, string site)
        {
            string sql = string.Format(@"SELECT MES_SPEC,ERP_SPEC,MODEL,CATEGORY,STAGE FROM SAJET.M_PART_SPEC_ERP_MES_MAPPING WHERE ENABLED ='Y' and  SITE = '" + site + "'");
            if (!string.IsNullOrWhiteSpace(stage))
                sql += string.Format(" AND stage = '"+ stage+"'");
            if (!string.IsNullOrWhiteSpace(mesSpec))
                sql += " AND MES_SPEC LIKE '%" + mesSpec + "%'";
            return Context.Ado.SqlQuery<Object>(sql);
        }

        public int StationInsert(ImesMstationTypePartSpec imesMpart)
        {
            string site = imesMpart.site;
            int count = Context.Queryable<ImesMstationTypePartSpec>()
                .Where(it => it.stationType == imesMpart.stationType 
                && it.kpSpec == imesMpart.stationType 
                && it.site == site)
                .Count();

            if (count < 1) 
            {
                int id = Context.Queryable<ImesMstationTypePartSpec>().Max(it => it.id) + 1;
                imesMpart.id = id;
                imesMpart.createTime= DateTime.Now;
                int insert = Context.Insertable(imesMpart).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                string insertHt = $"Insert INTO SAJET.M_STATIONTYPE_PARTSPEC_HT (Select * FROM SAJET.M_STATIONTYPE_PARTSPEC where id = " + id;
                if (site != null && site != "")
                {
                    insertHt = insertHt + " and site = '" + site + "'";
                }
                Context.Ado.SqlQuery<Object>(insertHt + ")");
                return insert;
            }
            return 0;
        }

        public int StationtUpdate(ImesMstationTypePartSpec imesMpart)
        {
            int id = imesMpart.id;
            string site = imesMpart.site;
            imesMpart.updateTime = DateTime.Now;
            string insertHt = $"Insert INTO SAJET.M_STATIONTYPE_PARTSPEC_HT (Select * FROM SAJET.M_STATIONTYPE_PARTSPEC where id = " + id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");

            int Updateable = Context.Updateable(imesMpart).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            return Updateable;
        }

        public int StationDelete(ImesMstationTypePartSpec imesMpart)
        {
            int id = imesMpart.id;
            string site = imesMpart.site;
            imesMpart.updateTime= DateTime.Now;
            Context.Updateable(imesMpart).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();

            string insertHt = $"Insert INTO SAJET.M_STATIONTYPE_PARTSPEC_HT (Select * FROM SAJET.M_STATIONTYPE_PARTSPEC where id = " + id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");

            return Context.Deleteable<ImesMstationTypePartSpec>().Where(it => it.id == id && it.site == site).ExecuteCommand();
        }

        public int StationCopy(ImesMstationTypePartSpec imesMpart)
        {
            List<ImesMstationTypePartSpec> imes =  Context.Queryable<ImesMstationTypePartSpec>().Where(it => it.id == imesMpart.id && it.site == imesMpart.site).ToList();
            if (imes.Count < 1) 
            {
                string site = imesMpart.site;
                imesMpart.updateTime = DateTime.Now;
                imesMpart.createTime = DateTime.Now;
                imesMpart.model = imes[0].model;
                imesMpart.enabled = imes[0].enabled;
                imesMpart.bobcatSpec = imes[0].bobcatSpec;
                imesMpart.stationDesc = imes[0].stationDesc;
                imesMpart.kpSpecDesc = imes[0].kpSpecDesc;
                int id = Context.Queryable<ImesMstationTypePartSpec>().Max(it => it.id) + 1;
                imesMpart.id = id;
                int insertCount = Context.Insertable(imesMpart).ExecuteCommand();
                string insertHt = $"Insert INTO SAJET.M_STATIONTYPE_PARTSPEC_HT (Select * FROM SAJET.M_STATIONTYPE_PARTSPEC where id = " + id;
                if (site != null && site != "")
                {
                    insertHt = insertHt + " and site = '" + site + "'";
                }
                Context.Ado.SqlQuery<Object>(insertHt + ")");
                return insertCount;
            }
            return 0;

        }

        public object StationtlistHt(int Id, string site)
        {
            string listHt = $"select * FROM SAJET.M_STATIONTYPE_PARTSPEC_HT  where ID = " + Id;
            if (site != null && site != "")
            {
                listHt = listHt + " and site = '" + site + "'";
            }
            return Context.Ado.SqlQuery<Object>(listHt);
        }

        
        public object StationtlistStage(string stationType, string site)
        {
            string listHt = $"select STAGE,STATION_TYPE_DESC FROM SAJET.M_STATION_TYPE WHERE ENABLED = 'Y' AND STATION_TYPE = '" + stationType + "'";
            if (site != null && site != "")
            {
                listHt = listHt + " and site = '" + site + "'";
            }
            return Context.Ado.SqlQuery<Object>(listHt);
        }
    }
}
