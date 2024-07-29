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
    [SugarTable("SAJET.M_ACTION_GROUP_LINK")]
    public class MActionGroupLink
    {
        /// <summary>
        /// GroupId 
        /// </summary>
        [SugarColumn(ColumnName = "gROUP_ID")]
        public long? GroupId { get; set; }

        /// <summary>
        /// GroupSeq 
        /// </summary>
        [SugarColumn(ColumnName = "gROUP_SEQ")]
        public long? GroupSeq { get; set; }

        /// <summary>
        /// JobId 
        /// </summary>
        [SugarColumn(ColumnName = "jOB_ID")]
        public long? JobId { get; set; }

        /// <summary>
        /// ValueKind 
        /// </summary>
        [SugarColumn(ColumnName = "vALUE_KIND")]
        public long? ValueKind { get; set; }

        /// <summary>
        /// LoopCount 
        /// </summary>
        [SugarColumn(ColumnName = "lOOP_COUNT")]
        public long? LoopCount { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

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
        /// ValueTransformation 
        /// </summary>
        [SugarColumn(ColumnName = "vALUE_TRANSFORMATION")]
        public long? ValueTransformation { get; set; }

    }
}
