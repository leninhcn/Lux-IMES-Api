using System.ComponentModel.DataAnnotations;

namespace ZR.Model.Dto
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public class MBillMaterialQueryDto : PagerInfo 
    {
    }

    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MBillMaterialDto
    {
        public string ItemVersion { get; set; }

        public string StationType { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreateTime { get; set; }

       
        public string Id { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? ItemCount { get; set; }

        public string Ipn { get; set; }

        public string SPEC1 { get; set; }

        public string Version { get; set; }

        public string ItemIpn { get; set; }

        public string ItemSpec1 { get; set; }

        public string ItemGroup { get; set; }

        public string Site { get; set; }



    }
}