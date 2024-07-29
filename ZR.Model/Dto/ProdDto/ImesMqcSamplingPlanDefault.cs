using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.m_QC_SAMPLING_PLAN_DETAIL")]
    public class ImesMqcSamplingPlanDefault
    {
        /// <summary>
        /// SAMPLING_ID 
        /// </summary>
        [SugarColumn(ColumnName = "SAMPLING_ID")]
        public int samplingId { get; set; }
        [SugarColumn(ColumnName = "MIN_LOT_SIZE")]
        public int minLotsize { get; set;}
        [SugarColumn(ColumnName = "MAX_LOT_SIZE")]
        public int maxLotsize { get; set; }
        [SugarColumn(ColumnName = "SAMPLE_SIZE")]
        public int sampleSize { get; set; }
        [SugarColumn(ColumnName = "CRITICAL_REJECT_QTY")]
        public int criticalRejectqty { get; set;}
        [SugarColumn(ColumnName = "MAJOR_REJECT_QTY")]
        public int majorRejectqty { get; set; }
        [SugarColumn(ColumnName = "MINOR_REJECT_QTY")]
        public int minorRejectqty { get; set; }
        [SugarColumn(ColumnName = "UPDATE_EMP")]
        public string updateEmp { get; set; }
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime updateTime { get; set; }
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime createTime { get; set; }
        [SugarColumn(ColumnName = "SAMPLING_LEVEL")]
        public string  samplingLevel { get; set; }
        [SugarColumn(ColumnName = "SAMPLING_UNIT")]
        public string samplingUnit { get; set; }
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public int Id { get; set; }
        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]
        public string site { get; set; }
    }
}
