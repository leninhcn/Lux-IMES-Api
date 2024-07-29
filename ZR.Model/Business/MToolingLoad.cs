using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    /// <summary>
    /// MToolingLoad
    /// </summary>
    [SugarTable(TableName = "SAJET.M_TOOLING_LOAD")]
    public class MToolingLoad 
    {
        /// <summary>
        /// ToolingSnId
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_SN_ID")]
        public long ToolingSnId {  get; set; }

        /// <summary>
        /// Wo
        /// </summary>
        [SugarColumn(ColumnName = "WORK_ORDER")]
        public string Wo { get; set; }

        /// <summary>
        /// Line
        /// </summary>
        [SugarColumn(ColumnName = "LINE")]
        public string Line { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "STAGE")]
        public string Stage { get; set; }

        /// <summary>
        /// StationType
        /// </summary>
        [SugarColumn(ColumnName = "STATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// StationName
        /// </summary>
        [SugarColumn(ColumnName = "STATION_NAME")]
        public string StationName { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [SugarColumn(ColumnName = "STATUS")]
        public string Status { get; set; }

        /// <summary>
        /// UsedCount
        /// </summary>
        [SugarColumn(ColumnName = "USED_COUNT")]
        public long UsedCount { get; set; }

        /// <summary>
        /// TotalusedCount
        /// </summary>
        [SugarColumn(ColumnName = "TOTAL_USED_COUNT")]
        public long TotalusedCount { get; set; }

        /// <summary>
        /// LoadEmpno
        /// </summary>
        [SugarColumn(ColumnName = "LOAD_EMPNO")]
        public string LoadEmpno { get; set; }

        /// <summary>
        /// LoadTime
        /// </summary>
        [SugarColumn(ColumnName = "LOAD_TIME")]
        public DateTime? LoadTime { get; set; }

        /// <summary>
        /// ToolingSeq
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_SEQ")]
        public string ToolingSeq { get; set; }

        /// <summary>
        /// Ipn
        /// </summary>
        [SugarColumn(ColumnName = "IPN")]
        public string Ipn { get; set; }


        /// <summary>
        /// Location
        /// </summary>
        [SugarColumn(ColumnName = "LOCATION")]
        public string Location { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [SugarColumn(ColumnName = "REMARK")]
        public string Remark { get; set; }

        /// <summary>
        /// ToolingSn
        /// </summary>
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
