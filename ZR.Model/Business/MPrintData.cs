using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZR.Model.Business
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("SAJET.M_PRINT_DATA")]
    public class MPrintData
    {
        /// <summary>
        /// DataType 
        /// </summary>
        [SugarColumn(ColumnName = "dATA_TYPE")]
        public string DataType { get; set; }

        /// <summary>
        /// Id 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// DataSql 
        /// </summary>
        [SugarColumn(ColumnName = "dATA_SQL")]
        public string DataSql { get; set; }

        /// <summary>
        /// InputParam 
        /// </summary>
        [SugarColumn(ColumnName = "iNPUT_PARAM")]
        public string InputParam { get; set; }

        /// <summary>
        /// InputField 
        /// </summary>
        [SugarColumn(ColumnName = "iNPUT_FIELD")]
        public string InputField { get; set; }

        /// <summary>
        /// OutputParam 
        /// </summary>
        [SugarColumn(ColumnName = "oUTPUT_PARAM")]
        public string OutputParam { get; set; }

        /// <summary>
        /// DataSql2 
        /// </summary>
        [SugarColumn(ColumnName = "dATA_SQL2")]
        public string DataSql2 { get; set; }

        /// <summary>
        /// InputParam2 
        /// </summary>
        [SugarColumn(ColumnName = "iNPUT_PARAM2")]
        public string InputParam2 { get; set; }

        /// <summary>
        /// InputField2 
        /// </summary>
        [SugarColumn(ColumnName = "iNPUT_FIELD2")]
        public string InputField2 { get; set; }

        /// <summary>
        /// OutputParam2 
        /// </summary>
        [SugarColumn(ColumnName = "oUTPUT_PARAM2")]
        public string OutputParam2 { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMPNO")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// DataOrder 
        /// </summary>
        [SugarColumn(ColumnName = "dATA_ORDER")]
        public string DataOrder { get; set; }

    }
}
