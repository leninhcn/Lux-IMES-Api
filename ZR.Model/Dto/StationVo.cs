using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
     public class StationVo
    {
        public string? line { get; set; }
        public int? stationId { get; set; }

        public string? stationTypesCustomer { get; set; }

        public string? stage { get; set; }
    }
}
