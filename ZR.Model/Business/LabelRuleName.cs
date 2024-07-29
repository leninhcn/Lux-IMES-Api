using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable("SAJET.M_RULE_NAME")]
    [Tenant("0")]
    public class LabelRuleName
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true, OracleSequenceName = "M_RULE_NAME_ID")]
        public long? Id { get; set; }

        /// <summary>
        /// RuleType 
        /// </summary>
        [SugarColumn(ColumnName = "RULE_TYPE")]
        public string RuleType { get; set; }

        /// <summary>
        /// RuleName 
        /// </summary>
        [SugarColumn(ColumnName = "RULE_NAME")]
        public string RuleName { get; set; }

        /// <summary>
        /// RuleDesc 
        /// </summary>
        [SugarColumn(ColumnName = "RULE_DESC")]
        public string RuleDesc { get; set; }

        /// <summary>
        /// GroupQty 
        /// </summary>
        [SugarColumn(ColumnName = "GROUP_QTY")]
        public long? GroupQty { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// SafetyStock 
        /// </summary>
        [SugarColumn(ColumnName = "SAFETY_STOCK")]
        public long? SafetyStock { get; set; }
    }

    [SugarTable("SAJET.M_RULE_NAME_HT")]
    [Tenant("0")]
    public class LabelRuleNameHis
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public long? Id { get; set; }

        /// <summary>
        /// RuleType 
        /// </summary>
        [SugarColumn(ColumnName = "RULE_TYPE")]
        public string RuleType { get; set; }

        /// <summary>
        /// RuleName 
        /// </summary>
        [SugarColumn(ColumnName = "RULE_NAME")]
        public string RuleName { get; set; }

        /// <summary>
        /// RuleDesc 
        /// </summary>
        [SugarColumn(ColumnName = "RULE_DESC")]
        public string RuleDesc { get; set; }

        /// <summary>
        /// GroupQty 
        /// </summary>
        [SugarColumn(ColumnName = "GROUP_QTY")]
        public long? GroupQty { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// SafetyStock 
        /// </summary>
        [SugarColumn(ColumnName = "SAFETY_STOCK")]
        public long? SafetyStock { get; set; }
    }
}
