using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "SAJET.M_HT_TOOLING_LOAD")]
    public class MHtToolingLoad
    {
        [SugarColumn(ColumnName = "TOOLING_SN_ID")]
        public long ToolingSnId { get; set; }

        [SugarColumn(ColumnName = "WORK_ORDER")]
        public string Wo { get; set; }

        [SugarColumn(ColumnName = "PDLINE_NAME")]
        public string Line { get; set; }

        [SugarColumn(ColumnName = "STAGE_NAME")]
        public string Stage { get; set; }

        [SugarColumn(ColumnName = "STATION_TYPE")]
        public string StationType { get; set; }

        [SugarColumn(ColumnName = "STATION_NAME")]
        public string StationName { get; set; }

        [SugarColumn(ColumnName = "STATUS")]
        public string Status { get; set; }

        [SugarColumn(ColumnName = "USED_COUNT")]
        public long UsedCount { get; set; }

        [SugarColumn(ColumnName = "TOTAL_USED_COUNT")]
        public long TotalusedCount { get; set; }

        [SugarColumn(ColumnName = "LOAD_EMPNO")]
        public string LoadEmpno { get; set; }

        [SugarColumn(ColumnName = "LOAD_TIME")]
        public DateTime? LoadTime { get; set; }

        [SugarColumn(ColumnName = "TOOLING_SEQ")]
        public string ToolingSeq { get; set; }


        [SugarColumn(ColumnName = "IPN")]
        public string Ipn { get; set; }


        [SugarColumn(ColumnName = "LOCATION")]
        public string Location { get; set; }

        [SugarColumn(ColumnName = "REMARK")]
        public string Remark { get; set; }

        [SugarColumn(ColumnName = "TOOLING_SN")]
        public string ToolingSn { get; set; }


        // <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string UpdateEmpNo { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string CreateEmpNo { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public string CreateTime { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public char Enabled { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string Site { get; set; }
    }
}
