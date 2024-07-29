using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class BomInfo
    {
        public string MainPartNo { get; set; }
        public string ItemPartNo { get; set; }
        public string ItemPartDesc { get; set; }
        public long? ItemCount { get; set; }
        public string Slot { get; set; }
        public string ItemVersion { get; set; }

        public string ItemMpn { get; set; }

        public string ItemGroup { get; set; }
        public string ItemPartCode { get; set; }

        public int IpnFinishCount { get; set; }
        public string ItemPartType { get; set; }
    }
}
