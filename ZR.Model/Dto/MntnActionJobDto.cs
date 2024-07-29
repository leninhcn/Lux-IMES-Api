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
    public class MActionJobTypeBaseQueryDto
    {
        public long? TypeId { get; set; }
        public string Enabled { get; set; }
        public string TypeName {  get; set; }
        public string Site { get; set; }
    }
    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MActionJobTypeBaseDto
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long? TypeId { get; set; }
        [Required(ErrorMessage = "TypeName不能为空")]
        public string TypeName { get; set; }

        public string TypeDesc { get; set; }

        public string ProcParam { get; set; }

        [Required(ErrorMessage = "Site不能为空")]
        public string Site { get; set; }

        [Required(ErrorMessage = "UpdateTime不能为空")]
        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }
        [Required(ErrorMessage = "CreateTime不能为空")]
        public DateTime? CreateTime { get; set; }
        [Required(ErrorMessage = "Enabled不能为空")]
        public string Enabled { get; set; }
        public string UpdateEmpno { get; set; }
    }
    /// <summary>
    /// 查询对象
    /// </summary>
    public class MActionJobBaseQueryDto 
    {
        public long? JobId { get; set; }
        public long? TypeId { get; set; }
        public string Enabled { get; set; }
        public string JobName { get; set; }
        public string Site { get; set; }
    }
    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MActionJobBaseDto
    {
        [Required(ErrorMessage = "TypeId不能为空")]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long? TypeId { get; set; }

        [Required(ErrorMessage = "JobName不能为空")]
        public string JobName { get; set; }

        public string JobDesc { get; set; }

        public string UpdateEmpno { get; set; }

        [Required(ErrorMessage = "JobId不能为空")]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long? JobId { get; set; }

        public string CreateEmpno { get; set; }

        [Required(ErrorMessage = "CreateTime不能为空")]
        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        [Required(ErrorMessage = "Site不能为空")]
        public string Site { get; set; }

        [Required(ErrorMessage = "UpdateTime不能为空")]
        public DateTime? UpdateTime { get; set; }
    }
    /// <summary>
    /// 查询对象
    /// </summary>
    public class MActionJobLinkQueryDto 
    {
        public long? JobId { get; set; }
        public string Enabled { get; set; }
        public string Site { get; set; }
    }
    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MActionJobLinkDto
    {
        [Required(ErrorMessage = "JobId不能为空")]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long? JobId { get; set; }

        [Required(ErrorMessage = "JobSeq不能为空")]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long? JobSeq { get; set; }

        [Required(ErrorMessage = "LogicType不能为空")]
        public string LogicType { get; set; }

        public string LogicDesc { get; set; }

        [Required(ErrorMessage = "LogicProsql不能为空")]
        public string LogicProsql { get; set; }

        public string InputParam { get; set; }

        [Required(ErrorMessage = "Site不能为空")]
        public string Site { get; set; }

        public string UpdateEmpno { get; set; }

        [Required(ErrorMessage = "UpdateTime不能为空")]
        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }

        [Required(ErrorMessage = "CreateTime不能为空")]
        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        public string OutputParam { get; set; }
    }
    /// <summary>
    /// 查询对象
    /// </summary>
    public class MActionGroupBaseQueryDto 
    {
        public long? GroupId { get; set; }
        public string GroupName { get; set; }
        public string Enabled { get; set; }
        public string Site { get; set; }
    }
    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MActionGroupBaseDto
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long? GroupId { get; set; }
 
        public string GroupName { get; set; }

        public string GroupDesc { get; set; }

        public string UpdateEmpno { get; set; }

        public string Site { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        public DateTime? UpdateTime { get; set; }

    }
    /// <summary>
    /// 查询对象
    /// </summary>
    public class MStationActionQueryDto 
    {
        public string Site { get; set; }
        public string StationType { get; set; }
        public string StationName { get; set; }
    }

    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MStationActionDto
    {
        public string Line { get; set; }

        public string Site { get; set; }

        public string StationType { get; set; }

        public string StationName { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? GroupId { get; set; }

        public string ShowBom { get; set; }

        public string CheckLine { get; set; }

        public string PrintFlag { get; set; }

        public string AutoReadsn { get; set; }

        public string AutoReadPath { get; set; }

        public string CheckFont { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        public string PrintQty { get; set; }

        public string Stage { get; set; }
    }
    /// <summary>
    /// 查询对象
    /// </summary>
    public class MActionGroupLinkQueryDto
    {
        public long? GroupId { get; set; }
    }
    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MActionGroupLinkDto
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long? GroupId { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? GroupSeq { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? JobId { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? ValueKind { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? LoopCount { get; set; }

        public string Site { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? ValueTransformation { get; set; }
    }

}
