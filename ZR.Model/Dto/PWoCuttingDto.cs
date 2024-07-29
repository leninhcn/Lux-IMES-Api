using System.ComponentModel.DataAnnotations;

namespace ZR.Model.Dto
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public class PWoCuttingQueryDto : PagerInfo
    {
    }

    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class PWoCuttingDto
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long? Id { get; set; }

        public string WorkOrder { get; set; }

        public string Model { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? TargetQty { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? BomRatio { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? MaxQty { get; set; }

        public string UpdateEmpno { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? OutputQty { get; set; }

        public string Closed { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string Enabled { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? MaxShelfQty { get; set; }



    }
}