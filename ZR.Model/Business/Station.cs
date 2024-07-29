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
    [SugarTable("SAJET.M_STATION")]
    public class Station
    {
        /// <summary>
        /// Id 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// PassQty 
        /// </summary>
        [SugarColumn(ColumnName = "pASS_QTY")]
        public long? PassQty { get; set; }

        /// <summary>
        /// StationName 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_NAME")]
        public string StationName { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// Line 
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// Stage 
        /// </summary>
        public string Stage { get; set; }

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
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// 厂区 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Job 
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// JobVersion 
        /// </summary>
        [SugarColumn(ColumnName = "jOB_VERSION")]
        public string JobVersion { get; set; }

        /// <summary>
        /// MaxQty 
        /// </summary>
        [SugarColumn(ColumnName = "mAX_QTY")]
        public long? MaxQty { get; set; }

        /// <summary>
        /// FailQty 
        /// </summary>
        [SugarColumn(ColumnName = "fAIL_QTY")]
        public long? FailQty { get; set; }

        /// <summary>
        /// StationId 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_ID")]
        public long? StationId { get; set; }

    }
}
