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
    [SugarTable("SAJET.M_LOCATION")]
    public class MLocation
    {
        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

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
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// WarehouseCode 
        /// </summary>
        [SugarColumn(ColumnName = "wAREHOUSE_CODE")]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// LocationName 
        /// </summary>
        [SugarColumn(ColumnName = "lOCATION_NAME")]
        public string LocationName { get; set; }

        /// <summary>
        /// LocationType 
        /// </summary>
        [SugarColumn(ColumnName = "lOCATION_TYPE")]
        public string LocationType { get; set; }

        /// <summary>
        /// UpdateEmp 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMP")]
        public string UpdateEmp { get; set; }

        /// <summary>
        /// CurrentStatus 
        /// </summary>
        [SugarColumn(ColumnName = "cURRENT_STATUS")]
        public string CurrentStatus { get; set; }

        /// <summary>
        /// LocationCode 
        /// </summary>
        [SugarColumn(ColumnName = "lOCATION_CODE")]
        public string LocationCode { get; set; }

    }
}
