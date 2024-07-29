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
    [SugarTable("SAJET.P_WO_STOCK_IN_WMS_DATA")]
    public class PWoStockInWmsData
    {
        /// <summary>
        /// DataType 
        /// </summary>
        [SugarColumn(ColumnName = "dATA_TYPE")]
        public long? DataType { get; set; }

        /// <summary>
        /// CreateEmp 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMP")]
        public string CreateEmp { get; set; }

        /// <summary>
        /// WmsFlag 
        /// </summary>
        [SugarColumn(ColumnName = "wMS_FLAG")]
        public long? WmsFlag { get; set; }

        /// <summary>
        /// HoldFlag 
        /// </summary>
        [SugarColumn(ColumnName = "hOLD_FLAG")]
        public string HoldFlag { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Id 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Usn 
        /// </summary>
        public string Usn { get; set; }

        /// <summary>
        /// CartonId 
        /// </summary>
        [SugarColumn(ColumnName = "cARTON_ID")]
        public string CartonId { get; set; }

        /// <summary>
        /// Plant 
        /// </summary>
        public string Plant { get; set; }

        /// <summary>
        /// Mo 
        /// </summary>
        public string Mo { get; set; }

        /// <summary>
        /// Status 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Qty 
        /// </summary>
        public long? Qty { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// UpdateEmp 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMP")]
        public string UpdateEmp { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// OutFlag 
        /// </summary>
        [SugarColumn(ColumnName = "oUT_FLAG")]
        public long? OutFlag { get; set; }

    }
}
