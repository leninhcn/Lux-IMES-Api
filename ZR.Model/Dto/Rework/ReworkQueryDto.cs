using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Rework
{
    public class ReworkQueryDto : PagerInfo
    {

    }

    public class ReworkDto 
    {
        public string Sn { get; set; }
        public string Panel { get; set; }
        public string Wo { get; set; }
        public string Ipn { get; set; }
        public string Model { get; set; }
        public string Csn { get; set; }
        public string Pallet { get; set; }
        public string Carton { get; set; }
        public string Box { get; set; }
        public string StationType { get; set; }
        public string NextStationType { get; set; }
        public string OutStationTypeTime { get; set; }
        public string RouteName { get; set; }
    }
}
