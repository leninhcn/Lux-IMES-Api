using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class MesCommonQuery : PagerInfo
    {
        public string FilterField { get; set; }

        public string FilterValue { get; set; }

        public string Enabled { get; set; }
    }
}
