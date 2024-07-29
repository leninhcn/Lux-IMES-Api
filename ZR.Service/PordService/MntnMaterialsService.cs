using Infrastructure.Attribute;
using JinianNet.JNTemplate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Repository;
using ZR.Service.IService;

namespace ZR.Service.PordService
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    internal class MntnMaterialsService : BaseService<SnStatus>, IMntnMaterialsService
    {

        public PagedInfo<ImesMsnFeature> Materialsllist(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMsnFeature>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "ipn", it => it.ipn.Contains(textData));
                exp.AndIF(optionData == "partType", it => it.partType.Contains(textData));
                exp.AndIF(optionData == "snFeature", it => it.snFeature.Contains(textData));
                exp.AndIF(optionData == "mesSpec", it => it.mesSpec.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
                exp.AndIF(optionData == "updateTime", it => it.updateTime.ToString().Contains(textData));
            }
            return Context.Queryable<ImesMsnFeature>().Where(exp.ToExpression()).OrderBy(it => it.ipn).ToPage(pager);

        }

        public (string, object, object) MaterialsImportData(List<ImesMsnFeature> imes, string site, string name)
        {
            try { 
                int addNum = 0;
                int upNum = 0;
                int errNum = 0;
                imes.ForEach(x =>
                {
                    x.createTime = DateTime.Now;
                    x.updateTime = DateTime.Now;
                    x.createEmpno = name;
                    x.updateEmpno = name;
                    x.site = site;
                    if (x.id == 0)
                    {
                        x.id = Context.Queryable<ImesMsnFeature>().Max(it => it.id) + 1;
                        if (this.MaterialInsert(x) == 1)
                        {
                            addNum++;
                        }
                        else { 
                            errNum++;
                        }
                    
                    }
                    else 
                    {
                        if (this.MaterialsUpdate(x)==1) {
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
            catch (Exception e)
            {
                return ("上传失败，总数:", imes.Count + " 添加:0", " 修改:0");
            }
        }

        public int MaterialsUpdate(ImesMsnFeature imesMsn)
        {
            string site = imesMsn.site;
            int count = Context.Queryable<ImesMsnFeature>()
                .Where(it =>
                it.ipn == imesMsn.ipn &&
                it.snFeature == imesMsn.snFeature &&
                it.enabled == imesMsn.enabled &&
                it.site == site)
                .Count();
            if (count > 0)
            {

                return 0;// imesMsn.ipn + "料号存在，不能修改为该料号！";
            }
            int id = imesMsn.id;
            string insertHt = $"insert into IMES.M_SN_FEATURE_HT(select * from IMES.M_SN_FEATURE  where ID = " + id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            imesMsn.updateTime = DateTime.Now;
            int Updateable = Context.Updateable(imesMsn).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            if (Updateable > 0)
            {
                return 1;// "修改成功！";
            }
            return 2;// "修改失败，联系资讯！";
        }

        public int MaterialsDelet(ImesMsnFeature imesMsn)
        {
            int id = imesMsn.id;
            string site = imesMsn.site;
            imesMsn.updateTime = new DateTime();
            Context.Updateable(imesMsn).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            string insertHt = $"insert into IMES.M_SN_FEATURE_HT(select * from IMES.M_SN_FEATURE  where ID = " + id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");

            return Context.Deleteable<ImesMsnFeature>().Where(it => it.id == id && it.site == site).ExecuteCommand();

        }

        public object MaterialsllistHt(int Id, string site)
        {
            string listHt = $"select * from IMES.M_SN_FEATURE_HT  where ID = " + Id;
            if (site != null && site != "")
            {
                listHt = listHt + " and site = '" + site + "'";
            }
            return Context.Ado.SqlQuery<Object>(listHt);
        }

        public int MaterialInsert(ImesMsnFeature imesMsn)
        {
            imesMsn.updateTime = DateTime.Now;
            imesMsn.createTime = DateTime.Now;
            string site = imesMsn.site;
            int count = Context.Queryable<ImesMsnFeature>()
                .Where(it =>
                it.ipn == imesMsn.ipn &&
                it.snFeature == imesMsn.snFeature &&
                it.enabled == imesMsn.enabled &&
                it.site == site)
                .Count();

            if (count > 0)
            {
                return 0;// imesMsn.ipn + "料号存在，不能修改为该料号！";
            }

            int MaxId = Context.Queryable<ImesMsnFeature>().Max(it => it.id) + 1;
            imesMsn.id = MaxId;
            int insertMaterial = Context.Insertable(imesMsn).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertMaterial > 0)
            {
                string insertHt = $"insert into IMES.M_SN_FEATURE_HT(select * from IMES.M_SN_FEATURE  where ID = " + MaxId;
                if (site != null && site != "")
                {
                    insertHt = insertHt + " and site = '" + site + "'";
                }
                Context.Ado.SqlQuery<Object>(insertHt + ")");
                return 1;//"新增成功";
            }

            return 2;//"新增失败，检测数据是否有误！";

        }

        public object MaterialslIpnlist(string dateIpn, string dateIpnText, string site)
        {
            if (dateIpn.Equals("Ipn"))
            {
                string insert = $"SELECT * FROM IMES.M_PART A WHERE A.ENABLED = 'Y' AND A.IPN LIKE '%" + dateIpnText + "%'";
                if (site != null && site != "")
                {
                    insert = insert + " and site = '" + site + "'";
                }
                return Context.Ado.SqlQuery<Object>(insert + " ORDER BY ID ASC");
            }
            else
            {
                string insert = $"SELECT * FROM IMES.M_DERIVE_ITEM A WHERE A.DERIVE_IPN LIKE  '%" + dateIpnText + "%'";
                /*if (site != null && site != "")
                {
                    insert = insert + " and site = '" + site + "'";
                }*/
                return Context.Ado.SqlQuery<Object>(insert + " ORDER BY ID ASC");
            }
        }

        public object MaterialslDescriptionlist(string dateIpn, string dateIpnText, string site)
        {
            string sqlStr;

            if (dateIpn.Equals("Ipn"))
            {
                 sqlStr = @"  SELECT B.MES_SPEC FROM IMES.M_PART A, IMES.M_PART_SPEC_ERP_MES_MAPPING B WHERE A.SPEC1 = B.ERP_SPEC AND A.site = b.site  and A.IPN = '" + dateIpnText + "' AND ROWNUM = 1 ";
            }
            else
            {
                 sqlStr = @"  SELECT B.MES_SPEC FROM IMES.M_DERIVE_ITEM A, IMES.M_PART_SPEC_ERP_MES_MAPPING B  WHERE A.DERIVE_SPEC1 = B.ERP_SPEC AND A.DERIVE_IPN = '" + dateIpnText + "' AND ROWNUM = 1 ";
            }

            if (site != null && site != "")
            {
                sqlStr = sqlStr + " and B.site = '" + site + "'";
            }
            IEnumerable<Object> data = Context.Ado.SqlQuery<Object>(sqlStr);
            Object txtKPDesc =  data.FirstOrDefault();
            if (txtKPDesc is not null and not (object)"")
            {
                return txtKPDesc;
            }
            else 
            {
                return 0;
                //return "未查询到IPN对应的SPEC1不存在或者未维护料号对应关系,请确认！";
            }
        }

        //-------------------------------分隔---------------------------------------------------------------------------

        public PagedInfo<ImesMpartRoute> MntnPartRoutelist(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMpartRoute>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "ipn", it => it.ipn.Contains(textData));
                exp.AndIF(optionData == "routeName", it => it.routeName.Contains(textData));
                exp.AndIF(optionData == "packSpec", it => it.packSpec.Contains(textData));
                exp.AndIF(optionData == "ruleSetName", it => it.ruleSetName.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
                exp.AndIF(optionData == "updateTime", it => it.updateTime.ToString().Contains(textData));
            }
            return Context.Queryable<ImesMpartRoute>().Where(exp.ToExpression()).OrderBy(it => it.ipn).ToPage(pager);
        }

        public (string, object, object) MntnPartRouteImportData(List<ImesMpartRoute> imes, string site, string name)
        {
            try {
                int addNum = 0;
                int errNum = 0;
                int upNum = 0;
                if (imes.Count<1001) { 
                    imes.ForEach(x =>
                    {
                        x.createTime = DateTime.Now;
                        x.updateTime = DateTime.Now;
                        x.createEmpno = name;
                        x.updateEmpno = name;
                        x.site = site;
                        if (x.id == 0)
                        {
                            x.id = Context.Queryable<ImesMpartRoute>().Max(it => it.id) + 1;
                            if (this.MntnPartRouteInsert(x) == 1)
                            {
                                addNum++;
                            }
                            else { 
                                errNum++;
                            }
                        }
                        else {
                            if (this.MntnPartRouteUpdate(x)>0)
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
                return ("一次插入条数不能大于1000条！","","");
            }
            catch (Exception ex)
            {
                return ("上传失败，总数:", imes.Count + " 添加:0", " 修改:0");
            }
        }

        public int MntnPartRouteUpdate(ImesMpartRoute imesMsn)
        {
            string insertHt = $"insert into IMES.M_PART_ROUTE_HT(select * from IMES.M_PART_ROUTE  where ID = " + imesMsn.id + " and site = '" + imesMsn.site + "')";
            Context.Ado.SqlQuery<Object>(insertHt );
            imesMsn.updateTime = DateTime.Now;
            int Updateable = Context.Updateable(imesMsn).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            if (Updateable > 0)
            {
                return 1;// "修改成功！";
            }
            return 0;// "修改失败，联系资讯！";
        }

        public int MntnPartRouteDelet(ImesMpartRoute imesMsn)
        {
            int id = imesMsn.id;
            string site = imesMsn.site;
            imesMsn.updateTime = new DateTime();
            Context.Updateable(imesMsn).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            string insertHt = $"insert into IMES.M_PART_ROUTE_HT(select * from IMES.M_PART_ROUTE  where ID = " + id+ " and site = '" + site + "'";
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            return Context.Deleteable<ImesMpartRoute>().Where(it => it.id == id && it.site == site).ExecuteCommand();
        }

        public object MntnPartRoutelistHt(int Id, string site)
        {
            string listHt = $"select * from IMES.M_PART_ROUTE_HT  where ID = " + Id+ " and site = '" + site + "'";
            return Context.Ado.SqlQuery<Object>(listHt);
        }

        public object MntnPartRouteIpnlist(string ipn, string site)
        {
            string listIpn = $"SELECT IPN,APN,SPEC1,SPEC2,MODEL FROM IMES.M_PART WHERE  1=1 AND TYPE IN ('HALB','FERT') and site = '" + site + "'";
            if (ipn != null) 
            {
                listIpn = listIpn + "AND IPN LIKE '%" + ipn + "%'";
                
            }
            return Context.Ado.SqlQuery<Object>(listIpn + "ORDER BY IPN");
        }

        public object MntnPartRouteRoadNamelist(string routeName, string site)
        {
            string listRoadName = $"SELECT * FROM IMES.M_ROUTE where 1=1 and enabled='Y' and site = '" + site + "'";
            if (routeName != null)
            {
                listRoadName = listRoadName + "AND ROUTE_NAME LIKE '%" + routeName + "%'";
            }
            return Context.Ado.SqlQuery<Object>(listRoadName + "ORDER BY ROUTE_NAME");
        }

        public object MntnPartRuleSetNameRulelist(string ruleSetName, string site)
        {
            string listRuleSetName = $"select * from imes.M_RULE_SET WHERE 1=1 AND ENABLED='Y' and site = '" + site + "'";
            if (ruleSetName != null)
            {
                listRuleSetName = listRuleSetName + "AND RULE_SET_NAME LIKE '%" + ruleSetName + "%'";
            }
            return Context.Ado.SqlQuery<Object>(listRuleSetName + " ORDER BY RULE_SET_NAME ");
        }

        public object MntnPartPkspecNamelist(string pkspecName, string site)
        {
            string listRuleSetName = $"SELECT * FROM IMES.M_PKSPEC where  site = '" + site + "'";
            if (pkspecName != null)
            {
                listRuleSetName = listRuleSetName + "AND PKSPEC_NAME LIKE '%" + pkspecName + "%'";
            }
            return Context.Ado.SqlQuery<Object>(listRuleSetName + "ORDER BY PKSPEC_NAME");
        }

        public int MntnPartRouteInsert(ImesMpartRoute imes)
        {

            imes.updateTime = DateTime.Now;
            imes.createTime = DateTime.Now;
            string site = imes.site;
            int count = Context.Queryable<ImesMpartRoute>()
                .Where(it =>
                it.ipn == imes.ipn &&
                it.site == site)
                .Count();

            if (count > 0)
            {
                return 2;
            }

            int MaxId = Context.Queryable<ImesMpartRoute>().Max(it => it.id) + 1;
            imes.id = MaxId;
            int insertMaterial = Context.Insertable(imes).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertMaterial > 0)
            {
                string insertHt = $"insert into IMES.M_PART_ROUTE_HT(select * from IMES.M_PART_ROUTE  where ID = " + MaxId;
                if (site != null && site != "")
                {
                    insertHt = insertHt + " and site = '" + site + "'";
                }
                Context.Ado.SqlQuery<Object>(insertHt + ")");
                return 1;
            }

            return 0;
        }
    }
}
