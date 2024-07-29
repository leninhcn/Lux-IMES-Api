using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Tooling
{
    public class MToolingSnDto
    {

        public long ToolingSnId { get; set; }


        public string ToolingSn { get; set; }


        public long ToolingId { get; set; }


        public string ToolingSnDesc { get; set; }


        public long UsedCount { get; set; } = 0;


        public string ToolingStatus { get; set; } = "I";


        public DateTime? LastMaintainTime { get; set; }


        public long MaxUsedCount { get; set; }


        public long LimitUsedCount { get; set; }


        public long TotalUsedCount { get; set; }


        public string Length { get; set; }


        public string Width { get; set; }


        public string Height { get; set; }


        public string Face { get; set; }


        public string DamageDegree { get; set; }


        public string CleanDegree { get; set; }


        public string Rough { get; set; }


        public string Scrape { get; set; }


        public string ViewCheck { get; set; }


        public string JobNo { get; set; }


        public string Location { get; set; }


        public string Apn { get; set; }


        public string EfName { get; set; }


        public string StationName { get; set; }


        public string StationDesc { get; set; }



    }
}
