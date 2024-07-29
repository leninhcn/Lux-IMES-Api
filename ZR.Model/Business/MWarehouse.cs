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
    [SugarTable("SAJET.M_WAREHOUSE")]
    public class MWarehouse
    {
        /// <summary>
        /// WarehouseCode 
        /// </summary>
        [SugarColumn(ColumnName = "wAREHOUSE_CODE")]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// WarehouseName 
        /// </summary>
        [SugarColumn(ColumnName = "wAREHOUSE_NAME")]
        public string WarehouseName { get; set; }

        /// <summary>
        /// WarehouseType 
        /// </summary>
        [SugarColumn(ColumnName = "wAREHOUSE_TYPE")]
        public string WarehouseType { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// CreateEmp 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMP")]
        public string CreateEmp { get; set; }

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

    }
}
