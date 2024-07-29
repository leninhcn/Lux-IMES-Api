using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable("SAJET.M_RULE_PARAM")]
    public class LabelRuleParam
    {
        /// <summary>
        /// RuleType 
        /// </summary>
        [SugarColumn(ColumnName = "rULE_TYPE")]
        public string RuleType { get; set; }

        /// <summary>
        /// RuleName 
        /// </summary>
        [SugarColumn(ColumnName = "rULE_NAME")]
        public string RuleName { get; set; }

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
        /// ParameValue 
        /// </summary>
        [SugarColumn(ColumnName = "pARAME_VALUE")]
        public string ParameValue { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// RuleId 
        /// </summary>
        [SugarColumn(ColumnName = "rULE_ID")]
        public long? RuleId { get; set; }

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
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

    }

    [SugarTable("SAJET.M_RULE_PARAM_HT")]
    public class LabelRuleParamHis
    {
        /// <summary>
        /// RuleType 
        /// </summary>
        [SugarColumn(ColumnName = "rULE_TYPE")]
        public string RuleType { get; set; }

        /// <summary>
        /// RuleName 
        /// </summary>
        [SugarColumn(ColumnName = "rULE_NAME")]
        public string RuleName { get; set; }

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
        /// ParameValue 
        /// </summary>
        [SugarColumn(ColumnName = "pARAME_VALUE")]
        public string ParameValue { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// RuleId 
        /// </summary>
        [SugarColumn(ColumnName = "rULE_ID")]
        public long? RuleId { get; set; }

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
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

    }
}
