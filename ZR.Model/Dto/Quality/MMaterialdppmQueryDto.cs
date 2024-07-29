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
    public class MMaterialdppmQueryDto : PagerInfo
    {


    }

    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MMaterialdppmDto
    {
        public string Uploaddate { get; set; }

        public string CreateEmpno { get; set; }

        public string SerialNumber { get; set; }

        public string Ngdescription { get; set; }

        public string Ngrate { get; set; }

        public string Dppm { get; set; }

        public string Receivedate { get; set; }

        public string Rmanumber { get; set; }

        public string Status { get; set; }

        public string Report { get; set; }

        public string Remark { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        [Required(ErrorMessage = "Id不能为空")]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string Month { get; set; }

        public string Week { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        public string OPTION1 { get; set; }

        public string OPTION2 { get; set; }

        public string OPTION3 { get; set; }

        public string OPTION4 { get; set; }

        public string OPTION5 { get; set; }

        public string ReportFile { get; set; }

        public string Model { get; set; }



    }





}
