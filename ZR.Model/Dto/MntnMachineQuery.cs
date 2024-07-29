using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class MntnMachineCommonQuery: PagerInfo
    {
        public string FilterField { get; set; }
        public string FilterValue { get; set; }

        public string Enabled { get; set; }
    }

    public class MntnMachineQuery : PagerInfo
    {
        public string FilterField { get; set; }
        public string FilterValue { get; set; }

        public long? GroupId { get; set; }

        public string Enabled { get; set; }
    }
}
