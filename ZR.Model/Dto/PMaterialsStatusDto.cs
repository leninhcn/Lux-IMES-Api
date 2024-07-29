using System.ComponentModel.DataAnnotations;

namespace ZR.Model.Dto
{
    /// <summary>
    /// 辅材上下线查询对象
    /// </summary>
    public class PMaterialsStatusQueryDto : PagerInfo
    {
        public string WorkOrder { get; set; }

        public string StationType { get; set; }

        public string Enabled { get; set; }

        public string Site { get; set; }

    }

    /// <summary>
    /// 辅材上下线输入输出对象
    /// </summary>
    public class PMaterialsStatusDto
    {
        public string ScanType { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string Ipn { get; set; }

        public long? MateSum { get; set; }

        public string MachineCode { get; set; }

        public string MachineType { get; set; }

        public string MachineStauts { get; set; }

        public string PartType { get; set; }

        public string LineId { get; set; }

      
        public long MaterialsId { get; set; }

        public string Stage { get; set; }

        public string FcCode { get; set; }

        public string StationType { get; set; }

        public string Enabled { get; set; }

        public string Site { get; set; }

        public string CreateEmpno { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string WorkOrder { get; set; }

        public long Id { get; set; }

    }
}   