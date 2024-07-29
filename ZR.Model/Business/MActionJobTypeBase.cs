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
    [SugarTable("SAJET.M_ACTION_JOB_TYPE_BASE")]
    public class MActionJobTypeBase
    {
        /// <summary>
        /// TypeName 
        /// </summary>
        [SugarColumn(ColumnName = "tYPE_NAME")]
        public string TypeName { get; set; }

        /// <summary>
        /// TypeDesc 
        /// </summary>
        [SugarColumn(ColumnName = "tYPE_DESC")]
        public string TypeDesc { get; set; }

        /// <summary>
        /// ProcParam 
        /// </summary>
        [SugarColumn(ColumnName = "pROC_PARAM")]
        public string ProcParam { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// TypeId 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true,ColumnName = "tYPE_ID")]
        public long? TypeId { get; set; }

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
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

    }
}
