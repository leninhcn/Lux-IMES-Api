using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "SAJET.M_TOOLING_SN")]
    public class MToolingSn
    {
        /// <summary>
        /// TOOLING_SN_ID
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_SN_ID")]
        public long ToolingSnId { get; set; }

        /// <summary>
        /// ToolingSn
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_SN")]
        public string ToolingSn { get; set; }

        /// <summary>
        /// ToolingId
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_ID")]
        public long ToolingId { get; set; }

        /// <summary>
        /// ToolingSnDesc
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_SN_DESC")]
        public string ToolingSnDesc { get; set; }

        /// <summary>
        /// UsedCount
        /// </summary>
        [SugarColumn(ColumnName = "USED_COUNT")]
        public long UsedCount { get; set; } = 0;

        /// <summary>
        /// ToolingStatus
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_STATUS")]
        public string ToolingStatus { get; set; } = "I";

        /// <summary>
        /// LastMaintainTime
        /// </summary>
        [SugarColumn(ColumnName = "LAST_MAINTAIN_TIME")]
        public DateTime? LastMaintainTime { get; set; }

        /// <summary>
        /// MaxUsedCount
        /// </summary>
        [SugarColumn(ColumnName = "MAX_USED_COUNT")]
        public long MaxUsedCount { get; set; }

        /// <summary>
        /// LimitUsedCount
        /// </summary>
        [SugarColumn(ColumnName = "LIMIT_USED_COUNT")]
        public long LimitUsedCount { get; set; }

        /// <summary>
        /// TotalUsedCount
        /// </summary>
        [SugarColumn(ColumnName = "TOTAL_USED_COUNT")]
        public long TotalUsedCount { get; set; }

        /// <summary>
        /// Length
        /// </summary>
        [SugarColumn(ColumnName = "LENGTH")]
        public string Length { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        [SugarColumn(ColumnName = "WIDTH")]
        public string Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        [SugarColumn(ColumnName = "HEIGHT")]
        public string Height { get; set; }

        /// <summary>
        /// Face
        /// </summary>
        [SugarColumn(ColumnName = "FACE")]
        public string Face { get; set; }

        /// <summary>
        /// DamageDegree
        /// </summary>
        [SugarColumn(ColumnName = "DAMAGE_DEGREE")]
        public string DamageDegree { get; set; }

        /// <summary>
        /// CleanDegree
        /// </summary>
        [SugarColumn(ColumnName = "CLEAN_DEGREE")]
        public string CleanDegree { get; set; }

        /// <summary>
        /// Rough
        /// </summary>
        [SugarColumn(ColumnName = "ROUGH")]
        public string Rough { get; set; }

        /// <summary>
        /// Scrape
        /// </summary>
        [SugarColumn(ColumnName = "SCRAPE")]
        public string Scrape { get; set; }

        /// <summary>
        /// ViewCheck
        /// </summary>
        [SugarColumn(ColumnName = "VIEWCHECK")]
        public string ViewCheck { get; set; }

        /// <summary>
        /// JobNo
        /// </summary>
        [SugarColumn(ColumnName = "JOBNO")]
        public string JobNo { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [SugarColumn(ColumnName = "LOCATION")]
        public string Location { get; set; }

        /// <summary>
        /// Apn
        /// </summary>
        [SugarColumn(ColumnName = "APN")]
        public string Apn { get; set; }

        /// <summary>
        /// EfName
        /// </summary>
        [SugarColumn(ColumnName = "EF_NAME")]
        public string EfName { get; set; }

        /// <summary>
        /// StationName
        /// </summary>
        [SugarColumn(ColumnName = "STATION_NAME")]
        public string StationName { get; set; }

        /// <summary>
        /// StationDesc
        /// </summary>
        [SugarColumn(ColumnName = "STATION_DESC")]
        public string StationDesc { get; set; }

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


        /// <summary>
        /// UpdateEmpno1
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO1")]
        public string UpdateEmpNo1 { get; set; }

        /// <summary>
        /// UpdateTime1 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME1")]
        public DateTime? UpdateTime1 { get; set; }


    }
}
