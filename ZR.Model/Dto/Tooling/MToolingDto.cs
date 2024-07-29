using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Tooling
{
    public class MToolingDto
    { 

        public long Id { get; set; }


        public string ToolingNo { get; set; }


        public string ToolingType { get; set; }


        public string ToolingDesc { get; set; }


        public long MaxUseTimes { get; set; }


        public long MaxUseDay { get; set; }


        public long MaxMaintainTimes { get; set; }


        public long DayMaintain { get; set; }


        public long TimeMaintain { get; set; }


        public string AllowTimeOut { get; set; }


        public string AllowOverTime { get; set; }


        public string Location { get; set; }


        public string Job { get; set; }


        public string Line { get; set; }


        public string Stage { get; set; }


        public string StationType { get; set; }


        public long WarnUsedTimes { get; set; }


        public long WarnUsedDay { get; set; }


        public long MaintainTime { get; set; }


        public long CavityQty { get; set; }



    }
}
