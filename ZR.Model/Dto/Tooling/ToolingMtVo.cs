using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Tooling
{
    public class ToolingMtVo
    {
        public string ToolingType { get; set; }
        public string ToolingSn { get; set; }
        public string ToolingDesc { get; set; }
        public string Face { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string DamageDegree { get; set; }
        public string CleanDegree { get; set; }
        public string Rough { get; set; }
        public string Scrape { get; set; }
        public DateTime? LastMaintainTime { get; set; }
    }
}
