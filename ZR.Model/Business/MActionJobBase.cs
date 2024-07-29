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
    [SugarTable("SAJET.M_ACTION_JOB_BASE")]
    public class MActionJobBase
    {
        /// <summary>
        /// JobId 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true,ColumnName = "jOB_ID")]
        public long? JobId { get; set; }

        /// <summary>
        /// TypeId 
        /// </summary>
        [SugarColumn(ColumnName = "tYPE_ID")]
        public long? TypeId { get; set; }

        /// <summary>
        /// JobName 
        /// </summary>
        [SugarColumn(ColumnName = "jOB_NAME")]
        public string JobName { get; set; }

        /// <summary>
        /// JobDesc 
        /// </summary>
        [SugarColumn(ColumnName = "jOB_DESC")]
        public string JobDesc { get; set; }

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

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }
    }
}
