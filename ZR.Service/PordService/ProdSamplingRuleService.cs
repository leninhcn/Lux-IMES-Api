using Infrastructure.Attribute;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Utilities.Zlib;
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
    internal class ProdSamplingRuleService : BaseService<SnStatus>, IProdSamplingRuleService
    {
        public PagedInfo<IMesMqcSamplingRule> SamplingRuleList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<IMesMqcSamplingRule>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "SAMPLING_RULE_NAME", it => it.samplingRulename.Contains(textData));
                exp.AndIF(optionData == "SAMPLING_RULE_DESC", it => it.samplingRuledesc.Contains(textData));
            }
            var expPagedInfo = Context.Queryable<IMesMqcSamplingRule>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
            return expPagedInfo;
        }
        public PagedInfo<IMesMqcSamplingRuleDetail> SamplingDetailList(int samplingRuleid, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<IMesMqcSamplingRuleDetail>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(samplingRuleid != 0, it => it.samplingRuleid == samplingRuleid);
            var expPagedInfo = Context.Queryable<IMesMqcSamplingRuleDetail>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
            return expPagedInfo;

        }

        public int InsertSamplingRule(IMesMqcSamplingRule iMesMqcSamplingRule, string site, string updateEmp)
        {
            int count = Context.Queryable<IMesMqcSamplingRule>().Where(it => it.samplingRulename == iMesMqcSamplingRule.samplingRulename && it.enabled == iMesMqcSamplingRule.enabled && it.site == site).Count();
            if (count > 0)
            {
                return 0;
            }
            int MaxId = Context.Queryable<IMesMqcSamplingRule>().Max(it => it.samplingRuleid) + 1;
            iMesMqcSamplingRule.samplingRuleid = MaxId;
            iMesMqcSamplingRule.site = site;
            iMesMqcSamplingRule.updateEmp = updateEmp;
            iMesMqcSamplingRule.createTime = DateTime.Now;
            iMesMqcSamplingRule.updateTime = DateTime.Now;
            int insertErp = Context.Insertable(iMesMqcSamplingRule).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertErp > 0)
            {
                string sqlStr1 = $"insert INTO SAJET.m_QC_SAMPLING_RULE_HT(select * FROM SAJET.m_QC_SAMPLING_RULE where sampling_rule_id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr1 + ")");
                return 1;
            }
            return insertErp;
        }
        public int InsertSamplingRuleDetait(IMesMqcSamplingRuleDetail iMesMqcSamplingRuleDetait, string site, string updateEmp)
        {
            //string sqlStr = $"SELECT MAX(ID) AS MaxID FROM SAJET.M_WEIGHT_FAI";
            int MaxId = Context.Queryable<IMesMqcSamplingRuleDetail>().Max(it => it.detailId) + 1;
            iMesMqcSamplingRuleDetait.detailId = MaxId;
            //iMesMqcSamplingRuleDetait.site = site;
            iMesMqcSamplingRuleDetait.updateEmp = updateEmp;
            iMesMqcSamplingRuleDetait.updateTime = DateTime.Now;
            iMesMqcSamplingRuleDetait.createTime = DateTime.Now;
            int insertErp = Context.Insertable(iMesMqcSamplingRuleDetait).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertErp > 0)
            {
                string sqlStr1 = $"insert INTO SAJET.m_QC_SAMPLING_RULE_DETAIL_HT(select * FROM SAJET.m_QC_SAMPLING_RULE_DETAIL where detail_id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr1 + ")");
                return 1;
            }
            return insertErp;
        }
        public int SamplingRulePreset(string samplingRuleid, long updateEmp, string site)
        {
            string sql = string.Format(@" update SAJET.m_QC_SAMPLING_RULE set DEFAULT_FLAG ='N',UPDATE_emp ='" + updateEmp + "',UPDATE_TIME = SYSDATE WHERE DEFAULT_FLAG ='Y'");
            Context.Ado.SqlQuery<Object>(sql);
            if (samplingRuleid != "")
            {
                string sql1 = $" update SAJET.m_QC_SAMPLING_RULE set DEFAULT_FLAG ='Y'Where SAMPLING_RULE_ID  = '" + samplingRuleid + "'";
                Context.Ado.SqlQuery<string>(sql1);
                return 1;
            }
            return 2;
        }
        public int UpdateSamplingRuleDetait(IMesMqcSamplingRuleDetail iMesMqcSamplingRuleDetait, string site)
        {
            int updateSamplingDefault = Context.Updateable(iMesMqcSamplingRuleDetait).UpdateColumns(it => new { it.samplingLevel, it.continueCnt, it.passCnt, it.rejectCnt, it.nextSamplinglevel }).WhereColumns(it => new { it.detailId, site }).ExecuteCommand();
            if (updateSamplingDefault > 0)
            {
                string sqlStr = $"insert INTO SAJET.m_QC_SAMPLING_RULE_DETAIL_HT(select * FROM SAJET.m_QC_SAMPLING_RULE_DETAIL  where detail_id = " + iMesMqcSamplingRuleDetait.detailId + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return updateSamplingDefault;
        }
        public int UpdateSamplingRule(IMesMqcSamplingRule iMesMqcSamplingRule, string site)
        {
            int UpdateSamplingRule = Context.Updateable(iMesMqcSamplingRule).UpdateColumns(it => new { it.samplingRulename, it.samplingRuledesc }).WhereColumns(it => new { it.samplingRuleid, site }).ExecuteCommand();
            if (UpdateSamplingRule > 0)
            {
                string sqlStr = $"insert INTO SAJET.m_QC_SAMPLING_RULE_HT(select * FROM SAJET.m_QC_SAMPLING_RULE  where sampling_rule_id = '" + iMesMqcSamplingRule.samplingRuleid + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return UpdateSamplingRule;
        }
        public int SamplingRuleabled(IMesMqcSamplingRule iMesMqcSamplingRule, string site)
        {
            if (iMesMqcSamplingRule.enabled == "Y")
            {
                int updateStation = Context.Updateable(iMesMqcSamplingRule).UpdateColumns(it => it.enabled == "N").WhereColumns(it => it.samplingRuleid).ExecuteCommand();
                if (updateStation > 0)
                {
                    string sqlStr = $"insert INTO SAJET.m_QC_SAMPLING_RULE_ht(select * FROM SAJET.m_QC_SAMPLING_RULE  where sampling_rule_id = '" + iMesMqcSamplingRule.samplingRuleid + "')";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return 1;
                }
            }
            else
            {
                int updateStation = Context.Updateable(iMesMqcSamplingRule).UpdateColumns(it => it.enabled == "Y").WhereColumns(it => it.samplingRuleid).ExecuteCommand();
                if (updateStation > 0)
                {
                    string sqlStr = $"insert INTO SAJET.m_QC_SAMPLING_RULE_ht(select * FROM SAJET.m_QC_SAMPLING_RULE   where sampling_rule_id = '" + iMesMqcSamplingRule.samplingRuleid + "')";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return 1;
                }
            }
            return 0;
        }
        public int DeleteSamplingDefault(IMesMqcSamplingRuleDetail iMesMqcSamplingRuleDetail)
        {
            int id = iMesMqcSamplingRuleDetail.detailId;
            string site = iMesMqcSamplingRuleDetail.site;
            string insertHt = $"insert INTO SAJET.m_QC_SAMPLING_RULE_DETAIL_HT(select * FROM SAJET.m_QC_SAMPLING_RULE_DETAIL  where detail_id = " + id + " ";
            if (site != null && site != "")
            {
                insertHt = insertHt + "and site ='" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            return Context.Deleteable<IMesMqcSamplingRuleDetail>().Where(it => it.detailId == id && it.site == site).ExecuteCommand();
            //Context.Ado.SqlQuery<Object>("insert INTO SAJET.m_QC_SAMPLING_RULE_DETAIL_HT(select * FROM SAJET.m_QC_SAMPLING_RULE_DETAIL  where detail_id = " + detailId + ")");
            //return Context.Deleteable<IMesMqcSamplingRuleDetail>().Where(it => it.detailId == detailId && it.site == site).ExecuteCommand();
        }
        public int DeleteSamplingRule(IMesMqcSamplingRule iMesMqcSamplingRule)
        {
            int id = iMesMqcSamplingRule.samplingRuleid;
            string site = iMesMqcSamplingRule.site;
            string insertHt = $"insert INTO SAJET.m_QC_SAMPLING_RULE_ht(select * FROM SAJET.m_QC_SAMPLING_RULE  where sampling_rule_id = " + id + " ";
            if (site != null && site != "")
            {
                insertHt = insertHt + "and site ='" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            return Context.Deleteable<IMesMqcSamplingRule>().Where(it => it.samplingRuleid == id && it.site == site).ExecuteCommand();
        }

    }
}
