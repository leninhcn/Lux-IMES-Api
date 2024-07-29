using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "SAJET.M_TOOLING", TableDescription = "治具表")]
    public class MTooling
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public long Id { get; set; }

        /// <summary>
        /// ToolingNo
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_NO")]
        public string ToolingNo { get; set; }

        /// <summary>
        /// TOOLING_TYPE
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_TYPE")]
        public string ToolingType { get; set; }

        /// <summary>
        /// ToolingDesc
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_DESC")]
        public string ToolingDesc { get; set; }

        /// <summary>
        /// MaxUseTimes
        /// </summary>
        [SugarColumn(ColumnName = "MAX_USE_TIMES")]
        public long MaxUseTimes { get; set; }

        /// <summary>
        /// MaxUseDay
        /// </summary>
        [SugarColumn(ColumnName = "MAX_USE_DAY")]
        public long MaxUseDay { get; set; }

        /// <summary>
        /// MaxMaintainTimes
        /// </summary>
        [SugarColumn(ColumnName = "MAX_MAINTAIN_TIMES")]
        public long MaxMaintainTimes { get; set; }

        /// <summary>
        /// DayMaintain
        /// </summary>
        [SugarColumn(ColumnName = "DAY_MAINTAIN")]
        public long DayMaintain { get; set; }

        /// <summary>
        /// TimeMaintain
        /// </summary>
        [SugarColumn(ColumnName = "TIMES_MAINTAIN")]
        public long TimeMaintain { get; set; }

        /// <summary>
        /// AllowTimeOut
        /// </summary>
        [SugarColumn(ColumnName = "ALLOW_TIMEOUT")]
        public string AllowTimeOut { get; set; }

        /// <summary>
        /// AllowOverTime
        /// </summary>
        [SugarColumn(ColumnName = "ALLOW_OVERTIME")]
        public string AllowOverTime { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [SugarColumn(ColumnName = "LOCATION")]
        public string Location { get; set; }

        /// <summary>
        /// Job
        /// </summary>
        [SugarColumn(ColumnName = "JOB")]
        public string Job { get; set; }

        /// <summary>
        /// Line
        /// </summary>
        [SugarColumn(ColumnName = "LINE")]
        public string Line { get; set; }

        /// <summary>
        /// Stage
        /// </summary>
        [SugarColumn(ColumnName = "STAGE")]
        public string Stage { get; set; }

        /// <summary>
        /// StationType
        /// </summary>
        [SugarColumn(ColumnName = "STATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// WarnUsedTimes
        /// </summary>
        [SugarColumn(ColumnName = "WARN_UESD_TIMES")]
        public long WarnUsedTimes { get; set; }

        /// <summary>
        /// WarnUsedDay
        /// </summary>
        [SugarColumn(ColumnName = "WARN_USED_DAY")]
        public long WarnUsedDay { get; set; }

        /// <summary>
        /// MaintainTime
        /// </summary>
        [SugarColumn(ColumnName = "MAINTAIN_TIME")]
        public long MaintainTime { get; set; }

        /// <summary>
        /// CavityQty
        /// </summary>
        [SugarColumn(ColumnName = "CAVITY_QTY")]
        public long CavityQty { get; set; }


        /// <summary>
        /// Enabled 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public string Enabled { get; set; } = "Y";

        /// <summary>
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
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string Site { get; set; } = "DEF";

    }
}
