using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class RouteMaintenanceInfoVo
    {
        public int? stageId { get; set; }
        public string? stage { get; set; }
        public int? stationTypeId { get; set; }
        public string? stationType { get; set; }
        public string? scanType { get; set; }
        public string? stationTypeDesc { get; set; }
        public string? clientType { get; set; }
        public string? routeName { get; set; }

        public string? nextStationType { get; set; }

        public string? nextstationTypeDesc { get; set; }

        public string? necessary { get; set; }
        
        public int ? result { get; set; }

        public int? seq { get; set; }

        public int? step { get; set; }

        public string? enabled { get; set; }



        








    }
}
