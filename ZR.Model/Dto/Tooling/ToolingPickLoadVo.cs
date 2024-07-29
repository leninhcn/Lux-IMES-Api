using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Tooling
{
    public class ToolingPickLoadVo
    {
        public string ToolingType { get; set; }
        public string ToolingNo {  get; set; }

        public string ToolingSn {  get; set; }

        public string Line {  get; set; }

        public string Status {  get; set; }

        public long TotalUsedCount { get; set; }

        public long MaxUseTimes {  get; set; }

        public string EmpName {  get; set; }

        public DateTime? UpdateTime { get; set; }


    }
}
