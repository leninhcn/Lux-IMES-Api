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
    [SugarTable("SAJET.S_BASE_PARAM")]
    public class SBaseParam
    {
        /// <summary>
        /// Program 
        /// </summary>
        public string Program { get; set; }

        /// <summary>
        /// ParamName 
        /// </summary>
        [SugarColumn(ColumnName = "pARAM_NAME")]
        public string ParamName { get; set; }

        /// <summary>
        /// ParamValue 
        /// </summary>
        [SugarColumn(ColumnName = "pARAM_VALUE")]
        public string ParamValue { get; set; }

        /// <summary>
        /// ParamType 
        /// </summary>
        [SugarColumn(ColumnName = "pARAM_TYPE")]
        public string ParamType { get; set; }

        /// <summary>
        /// DefaultValue 
        /// </summary>
        [SugarColumn(ColumnName = "dEFAULT_VALUE")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

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
        /// ParamDesc 
        /// </summary>
        [SugarColumn(ColumnName = "pARAM_DESC")]
        public string ParamDesc { get; set; }

    }
}