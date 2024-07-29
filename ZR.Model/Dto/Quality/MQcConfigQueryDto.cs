using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Quality
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public class MQcConfigQueryDto : PagerInfo
    {
        public string filterfield { get; set; }
        public string filtervalue { get; set; }

        public string enabled { get; set; }

    }

    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MQcConfigDto
    {




        [Required(ErrorMessage = "Id不能为空")]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string Remarks { get; set; }

        public string CheckRule { get; set; }

        public string OnlineFlag { get; set; }

        public string OnlineStationType { get; set; }

        public string ReturnStationType { get; set; }

        public string QcRoute { get; set; }

        public string QcStationType { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? Target { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? Qty { get; set; }

        public string ReQc { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? QcLevel { get; set; }

        public string QcType { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        public string AutoHold { get; set; }

        public string AllPass { get; set; }

        public string Model { get; set; }



    }



}
