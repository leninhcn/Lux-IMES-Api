using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.ProdDto;
using ZR.Model;

namespace ZR.Service.IService
{
    public interface IProdSamplingPlanService
    {
        PagedInfo<ImesMqcSamplingPlan> SamplingList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site);
        PagedInfo<ImesMqcSamplingPlanDefault> SamplingDetaitList(String level, string samplingId, string optionData, string textData, int pageNum, int pageSize, string site);
        int InsertSamplingPlan(ImesMqcSamplingPlan imesMqcSamplingPlan, string site,string updateEmp);
        int UpdateSamplingPlan(ImesMqcSamplingPlan imesMqcSamplingPlan, string site);
        int SamplingPlanabled(ImesMqcSamplingPlan imesMqcSamplingPlan, string site);
        int DeleteSamplingPlan(ImesMqcSamplingPlan imesMqcSamplingPlan);
        //List<string> GetSamplingInfo(string samplingType, string samplingDesc, string site);
        int DeleteSamplingDefault(ImesMqcSamplingPlanDefault imesMqcSamplingPlanDefault);
        int UpdateSamplingDefault(ImesMqcSamplingPlanDefault imesMqcSamplingPlanDefault, string site);
        int InsertSamplingDefault(ImesMqcSamplingPlanDefault imesMqcSamplingPlanDefault, string site, string updateEmp);
    }
}
