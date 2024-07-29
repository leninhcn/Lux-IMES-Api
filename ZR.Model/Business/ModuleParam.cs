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
    [SugarTable("SAJET.M_MODULE_PARAM")]
    public class ModuleParam
    {
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
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// ParameValue 
        /// </summary>
        [SugarColumn(ColumnName = "pARAME_VALUE")]
        public string ParameValue { get; set; }

        /// <summary>
        /// UpdateUserid 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_USERID")]
        public string UpdateUserid { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// ParameItem 
        /// </summary>
        [SugarColumn(ColumnName = "pARAME_ITEM")]
        public string ParameItem { get; set; }

    }
}
