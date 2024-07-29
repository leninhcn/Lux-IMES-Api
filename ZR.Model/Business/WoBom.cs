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
    [SugarTable("SAJET.P_WO_BOM")]
    public class WoBom
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID")]
        public string Id { get; set; }
        /// <summary>
        /// 工单 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_ORDER", ColumnDescription = "工单")]
        public string WorkOrder { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// 品名 
        /// </summary>
        public string SPEC1 { get; set; }

        /// <summary>
        /// 子料 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_IPN")]
        public string ItemIpn { get; set; }

        /// <summary>
        /// 子料品名 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_SPEC1")]
        public string ItemSpec1 { get; set; }

        /// <summary>
        /// 子料分组 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_GROUP")]
        public string ItemGroup { get; set; }

        /// <summary>
        /// 数量 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_COUNT")]
        public long? ItemCount { get; set; }

        /// <summary>
        /// 站点类型 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// Version 
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 仓库编号 
        /// </summary>
        [SugarColumn(ColumnName = "wAREHOUSE_NO")]
        public string WarehouseNo { get; set; }

        /// <summary>
        /// 仓库储位 
        /// </summary>
        [SugarColumn(ColumnName = "wAREHOUSE_LOCATION")]
        public string WarehouseLocation { get; set; }

        /// <summary>
        /// BackflushFlag 
        /// </summary>
        [SugarColumn(ColumnName = "bACKFLUSH_FLAG")]
        public string BackflushFlag { get; set; }

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
        /// OPTION1 
        /// </summary>
        public string OPTION1 { get; set; }

        /// <summary>
        /// OPTION2 
        /// </summary>
        public string OPTION2 { get; set; }

        /// <summary>
        /// OPTION3 
        /// </summary>
        public string OPTION3 { get; set; }

        /// <summary>
        /// OPTION4 
        /// </summary>
        public string OPTION4 { get; set; }

        /// <summary>
        /// OPTION5 
        /// </summary>
        public string OPTION5 { get; set; }

        /// <summary>
        /// OPTION6 
        /// </summary>
        public string OPTION6 { get; set; }

        /// <summary>
        /// Category 
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 槽体 
        /// </summary>
        public string Slot { get; set; }

        /// <summary>
        /// ItemVersion 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_VERSION")]
        public string ItemVersion { get; set; }

        /// <summary>
        /// Plant 
        /// </summary>
        public string Plant { get; set; }

        /// <summary>
        /// 料号 
        /// </summary>
        public string Ipn { get; set; }
    }
}
