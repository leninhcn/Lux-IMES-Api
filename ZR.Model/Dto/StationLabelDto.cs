using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class StationLabelDto
    {
        public string Ipn { get; set; }
        public string LabelType { get; set; }
        public string LabelName { get; set; }

        public string LabelSrvIp { get; set; }

        public string LabelDlUrl { get; set; }
    }
}
