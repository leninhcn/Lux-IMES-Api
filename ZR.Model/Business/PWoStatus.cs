using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable("SAJET.P_WO_STATUS")]
    public class PWoStatus
    {
        /// <summary>
        /// WorkOrder 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_ORDER")]
        public string WorkOrder { get; set; }

        /// <summary>
        /// WoStatus 
        /// </summary>
        [SugarColumn(ColumnName = "wO_STATUS")]
        public string WoStatus { get; set; }

        /// <summary>
        /// Memo 
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// Plant 
        /// </summary>
        public string Plant { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }
    }
}
