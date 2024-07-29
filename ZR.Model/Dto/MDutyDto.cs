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
    public class MDutyQueryDto : PagerInfo
    {
        public long Id { get; set; }
        public string Enabled { get; set; }
        public string DutyCode { get; set; }
        public string Site {  get; set; }
    }
    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MDutyDto
    {
        [Required(ErrorMessage = "Id不能为空")]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string DutyCode { get; set; }

        public string DutyDesc { get; set; }

        public string DutyDesc2 { get; set; }

        public string Enabled { get; set; }

        public string Site { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string UpdateEmpno { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

    }
}
