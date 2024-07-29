using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.m_QC_SAMPLING_PLAN")]
    public class ImesMqcSamplingPlan
    {
        /// <summary>
        /// SAMPLING_ID 
        /// </summary>
        [SugarColumn(ColumnName = "SAMPLING_ID")]
        public string samplingId { get; set; }

        /// <summary>
        /// SAMPLING_TYPE 
        /// </summary>
        [SugarColumn(ColumnName = "SAMPLING_TYPE")]
        public string samplingType { get; set; }

        /// <summary>
        /// SAMPLING_DESC 
        /// </summary>
        [SugarColumn(ColumnName = "SAMPLING_DESC")]
        public string samplingDesc { get; set; }

        /// <summary>
        /// UPDATE_EMP 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMP")]
        public string updateEmp { get; set; }

        /// <summary>
        /// UPDATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public string updateTime { get; set; }

        /// <summary>
        /// CREATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public string createTime { get; set; }


        /// <summary>
        /// ENABLED 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }

        /// <summary>
        /// SITE 
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }

    }
}
