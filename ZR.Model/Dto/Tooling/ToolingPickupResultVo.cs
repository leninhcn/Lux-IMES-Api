using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Tooling
{
    public class ToolingPickupResultVo
    {
        public string ToolingNo { get; set; }

        public string ToolingType { get; set; }

        public string ToolingSn { get; set; }

        public string ToolingSnDesc { get; set; }

        public long TotalUsedCount { get; set; }

        public long MaxUseTimes { get; set; }

        public string ToolingStatus { get; set; }

        public string EmpName { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
