using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class StationMaintenanceVo
    {

        public string? site { get; set; }
        public string? line { get; set; }
        public int? id { get; set; }
        public int? stationId { get; set; }
        public string? stationType { get; set; }

        public string? stationName { get; set; }

        public string? stationTypes { get; set; }

        public string? stationTypesDesc { get; set; }

        public string? lineCustomer { get; set; }

        public string? clientType { get; set; }

        public string? stage { get; set; }

        public string?  siteDesc{ get; set; }
        public string? enabled { get; set; }
    }
}
