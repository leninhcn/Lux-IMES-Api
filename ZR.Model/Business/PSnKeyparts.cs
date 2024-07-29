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
    [SugarTable("SAJET.P_SN_KEYPARTS")]
    public class PSnKeyparts
    {
        /// <summary>
        /// WorkOrder 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_ORDER")]
        public string WorkOrder { get; set; }

        /// <summary>
        /// Options 
        /// </summary>
        public string Options { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// Ipn 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// ItemIpn 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_IPN")]
        public string ItemIpn { get; set; }

        /// <summary>
        /// ItemSn 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_SN")]
        public string ItemSn { get; set; }

        /// <summary>
        /// ItemGroup 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_GROUP")]
        public string ItemGroup { get; set; }

        /// <summary>
        /// Version 
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// StationName 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_NAME")]
        public string StationName { get; set; }

        /// <summary>
        /// ItemSnCustomer 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_SN_CUSTOMER")]
        public string ItemSnCustomer { get; set; }

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
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// MesSpec 
        /// </summary>
        [SugarColumn(ColumnName = "mES_SPEC")]
        public string MesSpec { get; set; }

        /// <summary>
        /// Slot 
        /// </summary>
        public string Slot { get; set; }

        /// <summary>
        /// Mac 
        /// </summary>
        public string Mac { get; set; }

        /// <summary>
        /// Bluetooth 
        /// </summary>
        public string Bluetooth { get; set; }

        /// <summary>
        /// SerialNumber 
        /// </summary>
        [SugarColumn(ColumnName = "sERIAL_NUMBER")]
        public string SerialNumber { get; set; }

    }
}
