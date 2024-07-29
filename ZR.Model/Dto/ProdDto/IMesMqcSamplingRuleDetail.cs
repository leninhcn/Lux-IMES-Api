using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.m_QC_SAMPLING_RULE_DETAIL")]
    public class IMesMqcSamplingRuleDetail
    {
        [SugarColumn(ColumnName = "SAMPLING_RULE_ID")]
        public int samplingRuleid { get; set; }
        [SugarColumn(ColumnName = "REJECT_CNT")]
        public int rejectCnt { get; set; }
        [SugarColumn(ColumnName = "PASS_CNT")]
        public int passCnt { get; set; }
        [SugarColumn(ColumnName = "CONTINUE_CNT")]
        public int continueCnt { get; set; }
        [SugarColumn(ColumnName = "UPDATE_EMP")]
        public string updateEmp { get; set; }
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? updateTime { get; set; }
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }
        [SugarColumn(ColumnName ="ENABLED")]
        public string enaBled { get; set;}
        [SugarColumn(ColumnName = "DETAIL_ID")]
        public int detailId { get; set; }
        [SugarColumn(ColumnName = "SAMPLING_LEVEL")]
        public string samplingLevel { get; set; }
        [SugarColumn(ColumnName = "NEXT_SAMPLING_LEVEL")]
        public string nextSamplinglevel { get; set; }
        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }
    }
}
