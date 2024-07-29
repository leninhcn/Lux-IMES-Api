using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Tooling
{
    public class MToolingToolingSnVo
    {
        public string Site { get; set; }
        public long toolingId { get; set; }
        public long toolingSnId { get; set; }

        public string toolingType { get; set; }
        public long WarnUsedTimes { get; set; }
        public long MaxUseTimes { get; set; }
        public long WarnUsedDay { get; set; }
        public long MaxUseDay { get; set; }

        public long MaintainTime {  get; set; }

        public string toolingSn { get; set; }
        public long totalUsedCount { get; set; }
        public long usedCount { get; set; }
        public string toolingStatus { get; set; }
        
        public DateTime? updateStatusTime { get; set; }
        public string updateStatusEmpName {  get; set; }
    }
}
