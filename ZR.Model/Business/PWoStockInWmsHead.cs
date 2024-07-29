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
    [SugarTable("P_WO_STOCK_IN_WMS_HEAD")]
    public class PWoStockInWmsHead
    {
        /// <summary>
        /// Id 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

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
        /// Pn 
        /// </summary>
        public string Pn { get; set; }

        /// <summary>
        /// CustomerPn 
        /// </summary>
        [SugarColumn(ColumnName = "cUSTOMER_PN")]
        public string CustomerPn { get; set; }

        /// <summary>
        /// Line 
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// Weight 
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// Cqty 
        /// </summary>
        public long? Cqty { get; set; }

        /// <summary>
        /// PackType 
        /// </summary>
        [SugarColumn(ColumnName = "pACK_TYPE")]
        public string PackType { get; set; }

        /// <summary>
        /// WarehouseCode 
        /// </summary>
        [SugarColumn(ColumnName = "wAREHOUSE_CODE")]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// Status 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ProductDate 
        /// </summary>
        [SugarColumn(ColumnName = "pRODUCT_DATE")]
        public string ProductDate { get; set; }

        /// <summary>
        /// FullQty 
        /// </summary>
        [SugarColumn(ColumnName = "fULL_QTY")]
        public long? FullQty { get; set; }

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
        /// CreateEmp 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMP")]
        public string CreateEmp { get; set; }

        /// <summary>
        /// 单位 
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// PalletId 
        /// </summary>
        [SugarColumn(ColumnName = "pALLET_ID")]
        public string PalletId { get; set; }

    }
}
