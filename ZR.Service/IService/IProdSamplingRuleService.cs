using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.ProdDto;
using ZR.Model;

namespace ZR.Service.IService
{
    public interface IProdSamplingRuleService
    {
        PagedInfo<IMesMqcSamplingRule> SamplingRuleList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site);
        PagedInfo<IMesMqcSamplingRuleDetail> SamplingDetailList(int samplingRuleid,int pageNum, int pageSize, string site);
        int InsertSamplingRule(IMesMqcSamplingRule iMesMqcSamplingRule, string site,string updateEmp);
        int UpdateSamplingRule(IMesMqcSamplingRule iMesMqcSamplingRule, string site);
        int SamplingRuleabled(IMesMqcSamplingRule iMesMqcSamplingRule, string site);
        int DeleteSamplingRule(IMesMqcSamplingRule iMesMqcSamplingRule);
        int SamplingRulePreset(string samplingRuleid, long updateEmp, string site);
        int DeleteSamplingDefault(IMesMqcSamplingRuleDetail iMesMqcSamplingRuleDetait);
        int UpdateSamplingRuleDetait(IMesMqcSamplingRuleDetail iMesMqcSamplingRuleDetait, string site);
        int InsertSamplingRuleDetait(IMesMqcSamplingRuleDetail iMesMqcSamplingRuleDetait, string site, string updateEmp);
    }
}
