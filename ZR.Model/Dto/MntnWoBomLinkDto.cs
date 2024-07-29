using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public class PWoBomQueryDto : PagerInfo
    {
        public string WorkOrder {  get; set; }
    }
    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class PWoBomDto
    {
        public string Id { get; set; }  
        [Required(ErrorMessage = "工单不能为空")]
        public string WorkOrder { get; set; }

        [Required(ErrorMessage = "Site不能为空")]
        public string Site { get; set; }

        public string SPEC1 { get; set; }

        public string ItemIpn { get; set; }

        public string ItemSpec1 { get; set; }

        public string ItemGroup { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? ItemCount { get; set; }

        public string StationType { get; set; }

        public string Version { get; set; }

        public string WarehouseNo { get; set; }

        public string WarehouseLocation { get; set; }

        public string BackflushFlag { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public string OPTION1 { get; set; }

        public string OPTION2 { get; set; }

        public string OPTION3 { get; set; }

        public string OPTION4 { get; set; }

        public string OPTION5 { get; set; }

        public string OPTION6 { get; set; }

        public string Category { get; set; }

        public string Slot { get; set; }

        public string ItemVersion { get; set; }

        [Required(ErrorMessage = "Plant不能为空")]
        public string Plant { get; set; }

        public string Ipn { get; set; }

    }
}
