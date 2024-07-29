using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.ServiceCore.Model.Dto
{
    public class RapidRepair
    {
        public string sn {  get; set; }
        public string wo { get; set; }

        public string version { get; set; }
        public string parnNo { get; set; }
        public string line { get; set; }
        public string route { get; set; }
        public string stationName { get; set; }
        public string stationType { get; set; }

        public string wipStationType { get; set; }
        public string defectCode { get; set; }
        public string defectCode1 { get; set; }
        public string location { get; set; }
        public string repairTime { get; set; }

    }

    public class RapidRequst
    {
        public string sn { get; set; }
        public string wo { get; set; }
        public string parnNo { get; set; }
        public string line { get; set; }
        public string stationName { get; set; }
        public string stationType { get; set; }
        public string defectCode { get; set; }
        public string memo { get; set; }

        public string nStation { get; set; }

    }
}
