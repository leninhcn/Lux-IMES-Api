using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Quality
{
    /// <summary>
    /// 查询输入对象
    /// </summary>
    public class PHoldSnQueryDto : PagerInfo
    {
        public string Pn { get; set; }

        public string Panel { get; set; }

        public string Sn { get; set; }
        public string Wo { get; set; }


    }

    /// <summary>
    ///输出对象
    /// </summary>
    public class PHoldSnDto 
    {


        public string Id { get; set; }
        public string Pn{ get; set; }

        public string Panel { get; set; }

        public string Sn { get; set; }
        public string Wo { get; set; }

        public string StationType { get; set; }
        public string HoldReason { get; set; }
        public string HoldEmpno { get; set; }
        public DateTime HoldTime { get; set; }

        public string UnholdReason { get; set; }
        public string UnholdEmpno { get; set; }
        public DateTime UnholdTime { get; set; }
        public string HoldMethod { get; set; }

        public DateTime CreateTime { get; set; }
        public string CreateEmpno { get; set; }
        public string Enabled { get; set; }

    }
}
