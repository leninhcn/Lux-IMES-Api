using Infrastructure.Attribute;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZR.Infrastructure.Model;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Model.Quality;
using ZR.Repository;
using ZR.Service.IService;

namespace ZR.Service.PordService
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    internal class ProdSamplingPlanService : BaseService<SnStatus>, IProdSamplingPlanService
    {
        public PagedInfo<ImesMqcSamplingPlan> SamplingList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMqcSamplingPlan>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "samplingType", it => it.samplingType.Contains(textData));
                exp.AndIF(optionData == "samplingDesc", it => it.samplingDesc.Contains(textData));
            }
            var expPagedInfo = Context.Queryable<ImesMqcSamplingPlan>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
            return expPagedInfo;
        }
        public PagedInfo<ImesMqcSamplingPlanDefault> SamplingDetaitList(String level,string samplingId, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMqcSamplingPlanDefault>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(samplingId!=""&&samplingId!=null, it => it.samplingId.ToString()==samplingId);
            exp.AndIF(level != "" && level != null, it => it.samplingLevel.ToString() == level);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "MIN_LOT_SIZE", it => it.minLotsize.ToString().Contains(textData));
                exp.AndIF(optionData == "MAX_LOT_SIZE", it => it.maxLotsize.ToString().Contains(textData));
                exp.AndIF(optionData == "SAMPLE_SIZE", it => it.sampleSize.ToString().Contains(textData));
                exp.AndIF(optionData == "CRITICAL_REJECT_QTY", it => it.criticalRejectqty.ToString().Contains(textData));
                exp.AndIF(optionData == "MAJOR_REJECT_QTY", it => it.majorRejectqty.ToString().Contains(textData));
                exp.AndIF(optionData == "MINOR_REJECT_QTY", it => it.minorRejectqty.ToString().Contains(textData));
                exp.AndIF(optionData == "SAMPLING_UNIT", it => it.samplingUnit.ToString().Contains(textData));
            }
            var expPagedInfo = Context.Queryable<ImesMqcSamplingPlanDefault>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
            return expPagedInfo;
        }
        //public List<string> GetSamplingInfo(string samplingType, string samplingDesc, string site)
        //{
        //    String sqlStr = $"select b.samplingType,b.samplingDesc from  imes.m_QC_SAMPLING_PLAN b  where 1=1 ";

        //    if (samplingType != null && samplingType != "")
        //    {
        //        sqlStr += " and b.SAMPLING_TYPE ='" + samplingType + "'";
        //    }
        //    if (samplingDesc != null && samplingDesc != "")
        //    {
        //        sqlStr += " and b.SAMPLING_DESC ='" + samplingDesc + "'"; ;
        //    }
        //    if (site != null && site != "")
        //    {
        //        sqlStr += " and b.SITE ='" + site + "'";
        //    }
        //    sqlStr = sqlStr + "  order by b.STATION_NAME    ";
        //    List<string> list = Context.Ado.SqlQuery<string>(sqlStr);
        //    return list;
        //}
        public int InsertSamplingPlan(ImesMqcSamplingPlan imesMqcSamplingPlan, string site, string updateEmp)
        {
            //string sqlStr = $"SELECT MAX(ID) AS MaxID FROM IMES.M_WEIGHT_FAI";
            int MaxId = Context.Queryable<ImesMqcSamplingPlan>().Max(it =>Convert.ToInt32(it.samplingId)) + 1;
            imesMqcSamplingPlan.samplingId = MaxId.ToString();
            imesMqcSamplingPlan.site = site;
            imesMqcSamplingPlan.updateEmp = updateEmp;
            imesMqcSamplingPlan.createTime = DateTime.Now.ToString();
            imesMqcSamplingPlan.updateTime = DateTime.Now.ToString();
            int insertErp = Context.Insertable(imesMqcSamplingPlan).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertErp > 0)
            {
                string sqlStr1 = $"insert into imes.m_QC_SAMPLING_PLAN_HT(select * from imes.m_QC_SAMPLING_PLAN where sampling_id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr1 + ")");
                return 1;
            }
            return insertErp;
        }
        public int InsertSamplingDefault(ImesMqcSamplingPlanDefault imesMqcSamplingPlanDefault, string site, string updateEmp)
        {
            //string sqlStr = $"SELECT MAX(ID) AS MaxID FROM IMES.M_WEIGHT_FAI";
            int MaxId = Context.Queryable<ImesMqcSamplingPlanDefault>().Max(it => it.Id) + 1;
            imesMqcSamplingPlanDefault.Id = MaxId;
            imesMqcSamplingPlanDefault.site = site;
            imesMqcSamplingPlanDefault.updateEmp = updateEmp;
            imesMqcSamplingPlanDefault.updateTime = DateTime.Now;
            imesMqcSamplingPlanDefault.createTime = DateTime.Now;
            int insertErp = Context.Insertable(imesMqcSamplingPlanDefault).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertErp > 0)
            {
                string sqlStr1 = $"insert into imes.m_QC_SAMPLING_PLAN_DETAIL_HT(select * from imes.m_QC_SAMPLING_PLAN_DETAIL where id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr1 + ")");
                return 1;
            }
            return insertErp;
        }
        public int UpdateSamplingDefault(ImesMqcSamplingPlanDefault imesMqcSamplingPlanDefault, string site)
        {
            int updateSamplingDefault = Context.Updateable(imesMqcSamplingPlanDefault).UpdateColumns(it => new { it.minLotsize, it.maxLotsize,it.sampleSize,it.criticalRejectqty,it.majorRejectqty,it.minorRejectqty,it.samplingUnit }).WhereColumns(it => new { it.Id, site }).ExecuteCommand();
            if (updateSamplingDefault > 0)
            {
                string sqlStr = $"insert into IMES.m_QC_SAMPLING_PLAN_DETAIL_HT(select * from IMES.m_QC_SAMPLING_PLAN_DETAIL  where id = '" + imesMqcSamplingPlanDefault.Id + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return updateSamplingDefault;
        }
        public int UpdateSamplingPlan(ImesMqcSamplingPlan imesMqcSamplingPlan, string site)
        {
            int updateSamplingPlan = Context.Updateable(imesMqcSamplingPlan).UpdateColumns(it => new { it.samplingType, it.samplingDesc }).WhereColumns(it => new { it.samplingId, site }).ExecuteCommand();
            if (updateSamplingPlan > 0)
            {
                string sqlStr = $"insert into IMES.m_QC_SAMPLING_PLAN_HT(select * from IMES.m_QC_SAMPLING_PLAN  where sampling_id = '" + imesMqcSamplingPlan.samplingId + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return updateSamplingPlan;
        }
        public int SamplingPlanabled(ImesMqcSamplingPlan imesMqcSamplingPlan, string site)
        {
            if (imesMqcSamplingPlan.enabled == "Y")
            {
                int updateSamplingPlan = Context.Updateable(imesMqcSamplingPlan).UpdateColumns(it => it.enabled == "N").WhereColumns(it => it.samplingId).ExecuteCommand();
                if (updateSamplingPlan > 0)
                {
                    string sqlStr = $"insert into IMES.m_QC_SAMPLING_PLAN_HT(select * from IMES.m_QC_SAMPLING_PLAN  where sampling_id = '" + imesMqcSamplingPlan.samplingId + "')";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return 1;
                }
            }
            else
            {
                int updateSamplingPlan = Context.Updateable(imesMqcSamplingPlan).UpdateColumns(it => it.enabled == "Y").WhereColumns(it => it.samplingId).ExecuteCommand();
                if (updateSamplingPlan > 0)
                {
                    string sqlStr = $"insert into IMES.m_QC_SAMPLING_PLAN_HT(select * from IMES.m_QC_SAMPLING_PLAN   where sampling_id = '" + imesMqcSamplingPlan.samplingId + "')";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return 1;
                }
            }
            return 0;
        }
        public int DeleteSamplingDefault(ImesMqcSamplingPlanDefault imesMqcSamplingPlanDefault)
        {
            int id = imesMqcSamplingPlanDefault.Id;
            string site = imesMqcSamplingPlanDefault.site;
            string insertHt = $"insert into IMES.m_QC_SAMPLING_PLAN_DETAIL_HT(select * from IMES.m_QC_SAMPLING_PLAN_DETAIL  where id = " + id + " and site = '" + site + "'";
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            return Context.Deleteable<ImesMqcSamplingPlanDefault>().Where(it => it.Id == id && it.site == site).ExecuteCommand();
        }
        public int DeleteSamplingPlan(ImesMqcSamplingPlan imesMqcSamplingPlan)
        {
            string id =  imesMqcSamplingPlan.samplingId;
            string site = imesMqcSamplingPlan.site;
            string insertHt = $"insert into IMES.m_QC_SAMPLING_PLAN_HT(select * from IMES.m_QC_SAMPLING_PLAN  where sampling_id = " + id + " and site = '" + site + "'";
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            return Context.Deleteable<ImesMqcSamplingPlan>().Where(it => it.samplingId == id && it.site == site).ExecuteCommand();
        }

    }
}
