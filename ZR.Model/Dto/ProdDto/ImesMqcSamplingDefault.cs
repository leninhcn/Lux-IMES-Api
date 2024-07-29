using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{

    [SugarTable("SAJET.m_QC_SAMPLING_DEFAULT")]
    public class ImesMqcSamplingDefault
    {
        /// <summary>
        /// PartType 
        /// </summary>
        [SugarColumn(ColumnName = "IPN")]
        public string ipn { get; set; }

        /// <summary>
        /// SAMPLING_ID 
        /// </summary>
        [SugarColumn(ColumnName = "SAMPLING_ID")]
        public string samplingId { get; set; }

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
        /// ENABLED 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public string enabnled { get; set; }

        /// <summary>
        /// CREATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public string createTime { get; set; }
    }
}
