using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "SAJET.P_TOOLING_MT_TRAVEL")]
    public class PToolingMtTravel
    {
        /// <summary>
        /// ToolingSn
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_SN")]
        public string ToolingSn { get; set; }

        /// <summary>
        /// UpdateEmpNo
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string UpdateEmpNo { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// Memo
        /// </summary>
        [SugarColumn(ColumnName = "MEMO")]
        public string Memo { get; set; }

        /// <summary>
        /// PriorMaintainTime
        /// </summary>
        [SugarColumn(ColumnName = "PRIOR_MAINTAIN_TIME")]
        public DateTime? PriorMaintainTime { get; set; }

        /// <summary>
        /// UsedCount
        /// </summary>
        [SugarColumn(ColumnName = "USED_COUNT")]
        public long UsedCount { get; set; }

        /// <summary>
        /// MaintainItem
        /// </summary>
        [SugarColumn(ColumnName = "MAINTAIN_ITEM")]
        public string MaintainItem { get; set; }

        /// <summary>
        /// MaintainResult
        /// </summary>
        [SugarColumn(ColumnName = "MAINTAIN_RESULT")]
        public string MaintainResult { get; set; }

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
        /// ToolingMtId
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_MT_ID")]
        public long ToolingMtId { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public char Enabled { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Site
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string Site { get; set; }


       
    }
}
