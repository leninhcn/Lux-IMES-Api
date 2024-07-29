using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "SAJET.HJXS_PIE_WTSCANLABLEDATA")]
    public class HjxsPieWtScanLableData
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public string Id { get; set; }

        /// <summary>
        /// BillCode
        /// </summary>
        [SugarColumn(ColumnName = "BILLCODE")]
        public string BillCode { get; set; }

        /// <summary>
        /// BatchCode
        /// </summary>
        [SugarColumn(ColumnName = "BATCHCODE")]
        public string BatchCode { get; set; }

        /// <summary>
        /// ProNum
        /// </summary>
        [SugarColumn(ColumnName = "PRONUM")]
        public string ProNum { get; set; }

        /// <summary>
        /// ProLot
        /// </summary>
        [SugarColumn(ColumnName = "PROLOT")]
        public string ProLot { get; set; }

        /// <summary>
        /// Model
        /// </summary>
        [SugarColumn(ColumnName = "MODEL")]
        public string Model { get; set; }

        /// <summary>
        /// LabQty
        /// </summary>
        [SugarColumn(ColumnName = "LABQTY")]
        public double LabQty { get; set; }

        /// <summary>
        /// LabPage
        /// </summary>
        [SugarColumn(ColumnName = "LABPAGE")]
        public int LabPage { get; set; }

        /// <summary>
        /// ScanUser
        /// </summary>
        [SugarColumn(ColumnName = "SCANUSER")]
        public string ScanUser { get; set; }

        /// <summary>
        /// ScanTime
        /// </summary>
        [SugarColumn(ColumnName = "SCANTIME")]
        public string ScanTime { get; set; }

        /// <summary>
        /// ScanQty
        /// </summary>
        [SugarColumn(ColumnName = "SCANQTY")]
        public double ScanQty { get; set; }

        /// <summary>
        /// LeftQty
        /// </summary>
        [SugarColumn(ColumnName = "LEFTQTY")]
        public double LeftQty { get; set; }

        /// <summary>
        /// Note
        /// </summary>
        [SugarColumn(ColumnName = "NOTE")]
        public string Note { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        [SugarColumn(ColumnName = "UPDATETIME")]
        public string UpdateTime { get; set; }
    }
}
