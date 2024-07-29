using System.ComponentModel.DataAnnotations;

namespace ZR.Model.Dto
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public class PSnTravelQueryDto : PagerInfo 
    {
        public string WorkOrder { get; set; }
        public string StationType { get; set; }
        public string Enabled { get; set; }
        
    }

    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class PSnTravelDto
    {
        public string WorkOrder { get; set; }

        public string LocationNo { get; set; }

        public string SerialNumber { get; set; }

        public string Ipn { get; set; }

        public string Version { get; set; }

        public string RouteName { get; set; }

        public string Line { get; set; }

        public string Stage { get; set; }

        public string StationType { get; set; }

        public string ClientType { get; set; }

        public string StationName { get; set; }

        public string NextStationType { get; set; }

        public string CurrentStatus { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? FixedQty { get; set; }

        public string WorkFlag { get; set; }

        public DateTime? InStationtypeTime { get; set; }

        public DateTime? OutStationtypeTime { get; set; }

        public DateTime? InLineTime { get; set; }

        public DateTime? OutLineTime { get; set; }

        public string PalletNo { get; set; }

        public string CartonNo { get; set; }

        public string QcNo { get; set; }

        public string QcResult { get; set; }

        public string Customer { get; set; }

        public string ReworkNo { get; set; }

        public string EmpNo { get; set; }

        public string CustomerSn { get; set; }

        public string WipStationType { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? WipQty { get; set; }

        public string BoxNo { get; set; }

        public string PanelNo { get; set; }

        public string RcNo { get; set; }

        public string HoldFlag { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? PassCnt { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? StateFlag { get; set; }

        public string StateDesc { get; set; }

        public string SnVersion { get; set; }

        public string RpRoute { get; set; }

        public DateTime? CreateTime { get; set; }

        public string OPTION1 { get; set; }

        public string OPTION2 { get; set; }

        public string OPTION3 { get; set; }

        public string OPTION4 { get; set; }

        public string OPTION5 { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? SnCounter { get; set; }

        public string MachineNo { get; set; }

        public string ToolingNo { get; set; }

        public string CavityNo { get; set; }

        public string ShippingNo { get; set; }

        public string WarehouseNo { get; set; }

        public string Model { get; set; }



    }
}