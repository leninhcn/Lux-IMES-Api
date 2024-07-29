using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.Quality;

namespace ZR.Model.Dto
{
    /// <summary>
    /// 查询
    /// </summary>
    public class MReasonQueryDto : PagerInfo
    {
        public long Id { get; set; }
        public string Enabled { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonDesc { get; set; }
        public string Site {  get; set; }
    }
    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MReasonDto
    {
        [Required(ErrorMessage = "不良原因代码不能为空")]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        [Required(ErrorMessage = "不良原因代码不能为空")]
        public string ReasonCode { get; set; }

        public string ReasonLevel { get; set; }

        public string ReasonDesc { get; set; }

        public string ReasonDesc2 { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? ParentReasonId { get; set; }

        public string CodeLevel { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        public string Site { get; set; }

        public string ReasonType { get; set; }

        public string OPTION1 { get; set; }

        public string OPTION2 { get; set; }

    }
}
