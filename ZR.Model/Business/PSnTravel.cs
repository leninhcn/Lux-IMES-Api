
namespace ZR.Model.Business
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("SAJET.P_SN_TRAVEL")]
    public class PSnTravel
    {
        /// <summary>
        /// WorkOrder 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_ORDER")]
        public string WorkOrder { get; set; }

        /// <summary>
        /// FixedQty 
        /// </summary>
        [SugarColumn(ColumnName = "fIXED_QTY")]
        public long? FixedQty { get; set; }

        /// <summary>
        /// LocationNo 
        /// </summary>
        [SugarColumn(ColumnName = "lOCATION_NO")]
        public string LocationNo { get; set; }

        /// <summary>
        /// SerialNumber 
        /// </summary>
        [SugarColumn(ColumnName = "sERIAL_NUMBER")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Ipn 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// Version 
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// RouteName 
        /// </summary>
        [SugarColumn(ColumnName = "rOUTE_NAME")]
        public string RouteName { get; set; }

        /// <summary>
        /// Line 
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// Stage 
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// ClientType 
        /// </summary>
        [SugarColumn(ColumnName = "cLIENT_TYPE")]
        public string ClientType { get; set; }

        /// <summary>
        /// StationName 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_NAME")]
        public string StationName { get; set; }

        /// <summary>
        /// NextStationType 
        /// </summary>
        [SugarColumn(ColumnName = "nEXT_STATION_TYPE")]
        public string NextStationType { get; set; }

        /// <summary>
        /// CurrentStatus 
        /// </summary>
        [SugarColumn(ColumnName = "cURRENT_STATUS")]
        public string CurrentStatus { get; set; }

        /// <summary>
        /// WorkFlag 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_FLAG")]
        public string WorkFlag { get; set; }

        /// <summary>
        /// InStationtypeTime 
        /// </summary>
        [SugarColumn(ColumnName = "iN_STATIONTYPE_TIME")]
        public DateTime? InStationtypeTime { get; set; }

        /// <summary>
        /// OutStationtypeTime 
        /// </summary>
        [SugarColumn(ColumnName = "oUT_STATIONTYPE_TIME")]
        public DateTime? OutStationtypeTime { get; set; }

        /// <summary>
        /// InLineTime 
        /// </summary>
        [SugarColumn(ColumnName = "iN_LINE_TIME")]
        public DateTime? InLineTime { get; set; }

        /// <summary>
        /// OutLineTime 
        /// </summary>
        [SugarColumn(ColumnName = "oUT_LINE_TIME")]
        public DateTime? OutLineTime { get; set; }

        /// <summary>
        /// PalletNo 
        /// </summary>
        [SugarColumn(ColumnName = "pALLET_NO")]
        public string PalletNo { get; set; }

        /// <summary>
        /// CartonNo 
        /// </summary>
        [SugarColumn(ColumnName = "cARTON_NO")]
        public string CartonNo { get; set; }

        /// <summary>
        /// QcNo 
        /// </summary>
        [SugarColumn(ColumnName = "qC_NO")]
        public string QcNo { get; set; }

        /// <summary>
        /// QcResult 
        /// </summary>
        [SugarColumn(ColumnName = "qC_RESULT")]
        public string QcResult { get; set; }

        /// <summary>
        /// Customer 
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// ReworkNo 
        /// </summary>
        [SugarColumn(ColumnName = "rEWORK_NO")]
        public string ReworkNo { get; set; }

        /// <summary>
        /// EmpNo 
        /// </summary>
        [SugarColumn(ColumnName = "eMP_NO")]
        public string EmpNo { get; set; }

        /// <summary>
        /// CustomerSn 
        /// </summary>
        [SugarColumn(ColumnName = "cUSTOMER_SN")]
        public string CustomerSn { get; set; }

        /// <summary>
        /// WipStationType 
        /// </summary>
        [SugarColumn(ColumnName = "wIP_STATION_TYPE")]
        public string WipStationType { get; set; }

        /// <summary>
        /// WipQty 
        /// </summary>
        [SugarColumn(ColumnName = "wIP_QTY")]
        public long? WipQty { get; set; }

        /// <summary>
        /// BoxNo 
        /// </summary>
        [SugarColumn(ColumnName = "bOX_NO")]
        public string BoxNo { get; set; }

        /// <summary>
        /// PanelNo 
        /// </summary>
        [SugarColumn(ColumnName = "pANEL_NO")]
        public string PanelNo { get; set; }

        /// <summary>
        /// RcNo 
        /// </summary>
        [SugarColumn(ColumnName = "rC_NO")]
        public string RcNo { get; set; }

        /// <summary>
        /// HoldFlag 
        /// </summary>
        [SugarColumn(ColumnName = "hOLD_FLAG")]
        public string HoldFlag { get; set; }

        /// <summary>
        /// PassCnt 
        /// </summary>
        [SugarColumn(ColumnName = "pASS_CNT")]
        public long? PassCnt { get; set; }

        /// <summary>
        /// StateFlag 
        /// </summary>
        [SugarColumn(ColumnName = "sTATE_FLAG")]
        public long? StateFlag { get; set; }

        /// <summary>
        /// StateDesc 
        /// </summary>
        [SugarColumn(ColumnName = "sTATE_DESC")]
        public string StateDesc { get; set; }

        /// <summary>
        /// SnVersion 
        /// </summary>
        [SugarColumn(ColumnName = "sN_VERSION")]
        public string SnVersion { get; set; }

        /// <summary>
        /// RpRoute 
        /// </summary>
        [SugarColumn(ColumnName = "rP_ROUTE")]
        public string RpRoute { get; set; }

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
        /// SnCounter 
        /// </summary>
        [SugarColumn(ColumnName = "sN_COUNTER")]
        public long? SnCounter { get; set; }

        /// <summary>
        /// MachineNo 
        /// </summary>
        [SugarColumn(ColumnName = "mACHINE_NO")]
        public string MachineNo { get; set; }

        /// <summary>
        /// ToolingNo 
        /// </summary>
        [SugarColumn(ColumnName = "tOOLING_NO")]
        public string ToolingNo { get; set; }

        /// <summary>
        /// CavityNo 
        /// </summary>
        [SugarColumn(ColumnName = "cAVITY_NO")]
        public string CavityNo { get; set; }

        /// <summary>
        /// ShippingNo 
        /// </summary>
        [SugarColumn(ColumnName = "sHIPPING_NO")]
        public string ShippingNo { get; set; }

        /// <summary>
        /// WarehouseNo 
        /// </summary>
        [SugarColumn(ColumnName = "wAREHOUSE_NO")]
        public string WarehouseNo { get; set; }

        /// <summary>
        /// Model 
        /// </summary>
        public string Model { get; set; }

    }
}