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
    public class PSnBlackListQueryDto : PagerInfo
    {
       public string BlackType { get; set; }
       public string DefectCode { get; set; }

       public string VendorCode { get; set; }

       public string Message { get; set; }

       public string Kpsn { get; set; }


    }

    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class PSnBlackListDto
    {
        [Required(ErrorMessage = "Kpsn不能为空")]
        public string Kpsn { get; set; }

        public long Id { get; set; }
        public string VendorCode { get; set; }

        public string DefectCode { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateTime { get; set; }

        public string StationId { get; set; }

        public string Message { get; set; }

        public string Line { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string Enabled { get; set; }

        public string BlackType { get; set; }

        public string StationType { get; set; }

        public string TestStationName { get; set; }



    }





}
