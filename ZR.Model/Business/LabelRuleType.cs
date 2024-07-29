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
    [SugarTable("SAJET.M_RULE_TYPE")]
    public class LabelRuleType
    {
        /// <summary>
        /// RuleType 
        /// </summary>
        [SugarColumn(ColumnName = "rULE_TYPE")]
        public string RuleType { get; set; }

        /// <summary>
        /// RuleDesc 
        /// </summary>
        [SugarColumn(ColumnName = "rULE_DESC")]
        public string RuleDesc { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

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
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

    }

    [SugarTable("SAJET.M_RULE_TYPE_HT")]
    public class LabelRuleTypeHis
    {
        /// <summary>
        /// RuleType 
        /// </summary>
        [SugarColumn(ColumnName = "rULE_TYPE")]
        public string RuleType { get; set; }

        /// <summary>
        /// RuleDesc 
        /// </summary>
        [SugarColumn(ColumnName = "rULE_DESC")]
        public string RuleDesc { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

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
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

    }
}
