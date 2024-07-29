using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.m_QC_SAMPLING_RULE")]
    public class IMesMqcSamplingRule
    {
        /// <summary>
        /// SAMPLING_ID 
        /// </summary>
        [SugarColumn(ColumnName = "SAMPLING_RULE_ID", IsPrimaryKey = true)]
        public int samplingRuleid { get; set; }

        /// <summary>
        /// SAMPLING_TYPE 
        /// </summary>
        [SugarColumn(ColumnName = "SAMPLING_RULE_NAME")]
        public string samplingRulename { get; set; }

        /// <summary>
        /// SAMPLING_DESC 
        /// </summary>
        [SugarColumn(ColumnName = "SAMPLING_RULE_DESC")]
        public string samplingRuledesc { get; set; }

        /// <summary>
        /// UPDATE_EMP 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMP")]
        public string updateEmp { get; set; }

        /// <summary>
        /// UPDATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? updateTime { get; set; }

        /// <summary>
        /// CREATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }


        /// <summary>
        /// ENABLED 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }
        [SugarColumn(ColumnName = "DEFAULT_FLAG")]
        public string defaultFlag{ get; set; }    
        /// <summary>
        /// SITE 
        /// </summary>
        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = false)]
        public string site { get; set; }
    }
}
