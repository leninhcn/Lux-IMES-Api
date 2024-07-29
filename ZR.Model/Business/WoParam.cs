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
    [SugarTable("SAJET.P_WO_PARAM")]
    public class WoParam
    {
        /// <summary>
        /// WorkOrder 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_ORDER")]
        public string WorkOrder { get; set; }

        /// <summary>
        /// ModuleName 
        /// </summary>
        [SugarColumn(ColumnName = "mODULE_NAME")]
        public string ModuleName { get; set; }

        /// <summary>
        /// FunctionName 
        /// </summary>
        [SugarColumn(ColumnName = "fUNCTION_NAME")]
        public string FunctionName { get; set; }

        /// <summary>
        /// ParameName 
        /// </summary>
        [SugarColumn(ColumnName = "pARAME_NAME")]
        public string ParameName { get; set; }

        /// <summary>
        /// ParameItem 
        /// </summary>
        [SugarColumn(ColumnName = "pARAME_ITEM")]
        public string ParameItem { get; set; }

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
        /// ParameValue 
        /// </summary>
        [SugarColumn(ColumnName = "pARAME_VALUE")]
        public string ParameValue { get; set; }

    }
}