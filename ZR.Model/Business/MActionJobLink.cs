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
    [SugarTable("SAJET.M_ACTION_JOB_LINK")]
    public class MActionJobLink
    {
        /// <summary>
        /// JobId 
        /// </summary>
        [SugarColumn(ColumnName = "jOB_ID")]
        public long? JobId { get; set; }

        /// <summary>
        /// JobSeq 
        /// </summary>
        [SugarColumn(ColumnName = "jOB_SEQ")]
        public long? JobSeq { get; set; }

        /// <summary>
        /// LogicType 
        /// </summary>
        [SugarColumn(ColumnName = "lOGIC_TYPE")]
        public string LogicType { get; set; }

        /// <summary>
        /// LogicDesc 
        /// </summary>
        [SugarColumn(ColumnName = "lOGIC_DESC")]
        public string LogicDesc { get; set; }

        /// <summary>
        /// LogicProsql 
        /// </summary>
        [SugarColumn(ColumnName = "lOGIC_PROSQL")]
        public string LogicProsql { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// OutputParam 
        /// </summary>
        [SugarColumn(ColumnName = "oUTPUT_PARAM")]
        public string OutputParam { get; set; }

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
        /// InputParam 
        /// </summary>
        [SugarColumn(ColumnName = "iNPUT_PARAM")]
        public string InputParam { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }
    }
}