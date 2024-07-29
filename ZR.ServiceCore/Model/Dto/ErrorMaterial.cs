using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.ServiceCore.Model.Dto
{
    public class ErrorMaterial
    {
        public ErrorMaterial() { }

        public class snStatusInfo
        {
            public string panelNo { get; set; }
            public string snCounter { get; set; }
        }

        public class errorPM
        {
            public string dateCode { get; set;}
            public string lotCode { get; set; }
            public string newReelNo { get; set; }
        }

     }
}
