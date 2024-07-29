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
    [SugarTable("SAJET.P_PACK_PALLET")]
    public class PPackPallet
    {
        /// <summary>
        /// Ipn 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// PalletNo 
        /// </summary>
        [SugarColumn(ColumnName = "pALLET_NO")]
        public string PalletNo { get; set; }

        /// <summary>
        /// CloseFlag 
        /// </summary>
        [SugarColumn(ColumnName = "cLOSE_FLAG")]
        public string CloseFlag { get; set; }

        /// <summary>
        /// StationName 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_NAME")]
        public string StationName { get; set; }

        /// <summary>
        /// PkspecName 
        /// </summary>
        [SugarColumn(ColumnName = "pKSPEC_NAME")]
        public string PkspecName { get; set; }

        /// <summary>
        /// WorkOrder 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_ORDER")]
        public string WorkOrder { get; set; }

        /// <summary>
        /// CloseTime 
        /// </summary>
        [SugarColumn(ColumnName = "cLOSE_TIME")]
        public DateTime? CloseTime { get; set; }

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
        /// Status 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Quantity 
        /// </summary>
        public long? Quantity { get; set; }

        /// <summary>
        /// CloseEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "cLOSE_EMPNO")]
        public string CloseEmpno { get; set; }

    }
}