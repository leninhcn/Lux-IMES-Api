using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable("SAJET.M_TOOLING_TYPE")]
    public class MToolingType
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public long Id { get; set; }

        /// <summary>
        /// TooliongType
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_TYPE")]
        public string ToolingType { get; set; }

        /// <summary>
        /// ToolingTypeDesc
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_TYPE_DESC")]
        public string ToolingTypeDesc { get; set; }

        /// <summary>
        /// Options
        /// </summary>
        [SugarColumn(ColumnName = "OPTIONS")]
        public string Options { get; set; }

        /// <summary>
        /// ToolingTypeDesc
        /// </summary>
        [SugarColumn(ColumnName = "LOCATION_QTY")]
        public long LocationQty { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public string Enabled { get; set; }

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
