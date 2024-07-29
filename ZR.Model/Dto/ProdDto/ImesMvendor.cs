using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_VENDOR")]
    public class ImesMvendor
    {
        /// <summary>
        /// ID 
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public int id { get; set; }

        /// <summary>
        /// VENDOR_CODE 
        /// </summary>
        [SugarColumn(ColumnName = "VENDOR_CODE")]
        public string vendorCode { get; set; }

        /// <summary>
        /// VENDOR_NAME 
        /// </summary>
        [SugarColumn(ColumnName = "VENDOR_NAME")]
        public string vendorName { get; set; }

        /// <summary>
        /// VENDOR_DESC 
        /// </summary>
        [SugarColumn(ColumnName = "VENDOR_DESC")]
        public string vendorDesc { get; set; }


        /*/// <summary>
        /// ERP_ID 
        /// </summary>
        [SugarColumn(ColumnName = "ERP_ID")]
        public int erpId { get; set; }*/


        /*/// <summary>
        /// VLEVEL 
        /// </summary>
        [SugarColumn(ColumnName = "VLEVEL")]
        public int vlevel { get; set; }*/

        /// <summary>
        /// UPDATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? updateTime { get; set; }

        /// <summary>
        /// CREATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }

        /// <summary>
        /// VENDOR_TEL 
        /// </summary>
        [SugarColumn(ColumnName = "VENDOR_TEL")]
        public string vendorTel { get; set; }

        /*/// <summary>
        /// VENDOR_CONTACT 
        /// </summary>
        [SugarColumn(ColumnName = "VENDOR_CONTACT")]
        public string vendorContact { get; set; }*/

        /// <summary>
        /// VENDOR_EMAIL 
        /// </summary>
        [SugarColumn(ColumnName = "VENDOR_EMAIL")]
        public string vendorEmail { get; set; }

        /*/// <summary>
        /// VENDOR_NAME_ABBR 
        /// </summary>
        [SugarColumn(ColumnName = "VENDOR_NAME_ABBR")]
        public string vendorNameAbbr { get; set; }*/

        /*/// <summary>
        /// VENDOR_ABB 
        /// </summary>
        [SugarColumn(ColumnName = "VENDOR_ABB")]
        public string vendorAbb { get; set; }*/

        /*/// <summary>
        /// VENDOR_GROUP 
        /// </summary>
        [SugarColumn(ColumnName = "VENDOR_GROUP")]
        public string vendorGroup { get; set; }*/

        /*/// <summary>
        /// VENDOR_DESC2 
        /// </summary>
        [SugarColumn(ColumnName = "VENDOR_DESC2")]
        public string vendorDesc2 { get; set; }

        /// <summary>
        /// VENDOR_TEL2 
        /// </summary>
        [SugarColumn(ColumnName = "VENDOR_TEL2")]
        public string vendorTel2 { get; set; }

        /// <summary>
        /// SHIP_LIST 
        /// </summary>
        [SugarColumn(ColumnName = "SHIP_LIST")]
        public string shipList { get; set; }*/

        /// <summary>
        /// UPDATE_EMPNO 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string updateEmpno { get; set; }

        /// <summary>
        /// CREATE_EMPNO 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string createEmpno { get; set; }

        /// <summary>
        /// ENABLED 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }

        /// <summary>
        /// SITE 
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }
    }
}
