using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class StationInfoVo
    {
        public int? number { get; set; }

        public string? site { get; set; }

        public string? stationType { get; set; }

        public string? line { get; set; }

        public string? stationName { get; set; }

        public string? stage { get; set; }
        public string? updateEmpno { get; set; }
        public DateTime? updateTime { get; set; }
        public string? createEmpno { get; set; }
        public DateTime? createTime { get; set; }
        public string? enabled { get; set; }
     
    }
}
