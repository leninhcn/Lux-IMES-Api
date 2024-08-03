using Infrastructure.Attribute;
using OracleInternal.Secure.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Model.System;
using ZR.Repository;
using ZR.Service.IService;

namespace ZR.Service.PordService
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    internal class MntnlStandardTimeService : BaseService<SnStatus>, IMntnlStandardTimeService
    {
        public PagedInfo<imesMstandardtime> StandardTimelist(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<imesMstandardtime>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "id", it => it.id.ToString().Contains(textData));
                exp.AndIF(optionData == "ipn", it => it.ipn.Contains(textData));
                exp.AndIF(optionData == "desc1", it => it.desc1.Contains(textData));
                exp.AndIF(optionData == "model", it => it.model.Contains(textData));
                exp.AndIF(optionData == "dutyofmodel", it => it.dutyofmodel.Contains(textData));
                exp.AndIF(optionData == "remark", it => it.remark.Contains(textData));
                exp.AndIF(optionData == "ct", it => it.ct.Contains(textData));
                exp.AndIF(optionData == "human", it => it.human.Contains(textData));
                exp.AndIF(optionData == "workhours", it => it.workhours.Contains(textData));
                exp.AndIF(optionData == "uph", it => it.uph.Contains(textData));
                exp.AndIF(optionData == "side", it => it.side.Contains(textData));
                exp.AndIF(optionData == "stationtype", it => it.stationtype.Contains(textData));
                exp.AndIF(optionData == "line", it => it.line.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
                exp.AndIF(optionData == "updateTime", it => it.updateTime.ToString().Contains(textData));
                exp.AndIF(optionData == "createEmpno", it => it.createEmpno.Contains(textData));
                exp.AndIF(optionData == "createTime", it => it.createTime.ToString().Contains(textData));
            }
            exp.ToExpression();
            return Context.Queryable<imesMstandardtime>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
        }

        public (string, object, object) StandardTimeImport(List<imesMstandardtime> imes, string site, string name)
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
                        x.id = Context.Queryable<imesMstandardtime>().Max(it => it.id) + 1;
                        if (this.StandardTimeInsert(x) == 1)
                        {
                            addNum++;
                        }
                        else
                        {
                            errNum++;
                        }
                    }
                    else {
                        if (this.StandardTimeUpdate(x) > 0)
                        {
                            upNum++;
                        }
                        else {
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

        public object StandardTimelistHt(int Id, string site)
        {
            string listHt = $"select * FROM SAJET.M_STANDARDTIME_HT  where ID = " + Id;
            if (site != null && site != "")
            {
                listHt = listHt + " and site = '" + site + "'";
            }
            return Context.Ado.SqlQuery<Object>(listHt+ " order by UPDATE_TIME ");
        }

        public int StandardTimeUpdate(imesMstandardtime imes)
        {
            int id = imes.id;
            string site = imes.site;
            string insertHt = $"Insert INTO SAJET.M_STANDARDTIME_HT (Select * FROM SAJET.M_STANDARDTIME where id = " + id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            imes.updateTime = DateTime.Now;
            return Context.Updateable(imes).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
        }

        public int StandardTimeDelet(imesMstandardtime imes)
        {
            int id = imes.id;
            string site = imes.site;
            imes.updateTime = DateTime.Now;
            Context.Updateable(imes).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();

            string insertHt = $"Insert INTO SAJET.M_STANDARDTIME_HT (Select * FROM SAJET.M_STANDARDTIME where id = " + id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");

            return Context.Deleteable<imesMstandardtime>().Where(it => it.id == id && it.site == site).ExecuteCommand();
        }

        public int StandardTimeCopy(imesMstandardtime mstandardtime)
        {
            List<imesMstandardtime> imes = Context.Queryable<imesMstandardtime>().Where(it => it.id == mstandardtime.id && it.site == mstandardtime.site).ToList();
            if (imes.Count > 0)
            {
                string site = mstandardtime.site;
                mstandardtime.updateTime = DateTime.Now;
                mstandardtime.createTime = DateTime.Now;
                mstandardtime.option1 = imes[0].option1;
                mstandardtime.option2 = imes[0].option2;
                mstandardtime.enabled = imes[0].enabled;
                int id = Context.Queryable<imesMstandardtime>().Max(it => it.id) + 1;
                mstandardtime.id = id;
                int insertCount = Context.Insertable(mstandardtime).ExecuteCommand();
                string insertHt = $"Insert INTO SAJET.M_STANDARDTIME_HT (Select * FROM SAJET.M_STANDARDTIME where id = " + id;
                if (site != null && site != "")
                {
                    insertHt = insertHt + " and site = '" + site + "'";
                }
                Context.Ado.SqlQuery<Object>(insertHt + ")");
                return insertCount;
            }
            return 0;

        }

        public int StandardTimeInsert(imesMstandardtime imesMsn)
        {
            imesMsn.updateTime = DateTime.Now;
            imesMsn.createTime = DateTime.Now;
            string site = imesMsn.site;
            string ipn = imesMsn.ipn;
            string stationtype = imesMsn.stationtype;
            string line = imesMsn.line;
            int count = Context.Queryable<imesMstandardtime>()
                .Where(it =>
                it.ipn == ipn &&
                it.stationtype == stationtype &&
                it.line == line && it.site == site)
                .Count();

            if (count > 0)
            {
                return 2;// "此料号 " + ipn + " 的MES工序" + stationtype + " 和线体 " + line + " 在系统中已存在，请确认!!!";
            }

            int MaxId = Context.Queryable<imesMstandardtime>().Max(it => it.id) + 1;
            imesMsn.id = MaxId;
            int insertMaterial = Context.Insertable(imesMsn).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertMaterial > 0)
            {
                string insertHt = $"insert INTO SAJET.M_STANDARDTIME_HT(select * FROM SAJET.M_STANDARDTIME  where ID = " + MaxId;
                if (site != null && site != "")
                {
                    insertHt = insertHt + " and site = '" + site + "'";
                }
                Context.Ado.SqlQuery<Object>(insertHt + ")");
                return 1;// "新增成功";
            }

            return 0;//"新增失败，检测数据是否有误！";
        }

        public object FactoryStandardTimelist(string ipn, string site)
        {
            string sSQL = $"Select IPN,SPEC2 FROM SAJET.M_PART Where enabled = 'Y' ";
            if (ipn != null && ipn != "") 
            {
                sSQL = sSQL + "and IPN like '" + ipn + "%'";
            }
            if (site != "")
            {
                sSQL = sSQL + " and site =  '" + site + "'";
            }

            return Context.Ado.SqlQuery<Object>(sSQL + "order by IPN ");
        }

        public object ModelStandardTimelist(string model, string site)
        {
            string sSQL = $"Select MODEL,MODEL_CUSTOMER,MODEL_DESC FROM SAJET.M_MODEL Where enabled = 'Y'  ";
            if (model != null && model != "")
            {
                sSQL = sSQL + "and MODEL like  '" + model + "%'";
            }
            if (site != "")
            {
                sSQL = sSQL + " and site =  '" + site + "'";
            }
            return Context.Ado.SqlQuery<Object>(sSQL + "order by create_time desc  ");
        }

        public object StationtypeStandardTimelist(string stationtype,string line, string site)
        {
            string sSQL = $"Select station_name,station_type,line,stage FROM SAJET.M_STATION Where enabled = 'Y'  ";
            if (stationtype != null && stationtype != "")
            {
                sSQL = sSQL + " and station_type like  '" + stationtype + "%'";
            }
            if (line != "")
            { 
                sSQL = sSQL + " and line like '" + line + "%'";
            }
            if (site != "")
            {
                sSQL = sSQL + " and site = '" + site + "'";
            }
            return Context.Ado.SqlQuery<Object>(sSQL + " order by create_time desc  ");
        }

        public object LineStandardTimelist(string site)
        {
            string sSQL = $"SELECT LINE FROM SAJET.M_LINE WHERE ENABLED = 'Y'";
            if (site != null && site != "")
            {
                sSQL = sSQL + " and site =  '" + site + "'";
            }
            
            return Context.Ado.SqlQuery<Object>(sSQL + " ORDER BY CREATE_TIME DESC ");
        }

        public List<SysUser> ModelNameStandardTimeList(string site)
        {
            return Context.Queryable<SysUser>().Where(it => it.Site == site).ToList();
        }
    }
}
