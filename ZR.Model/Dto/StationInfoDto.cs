using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;

namespace ZR.Model.Dto
{
    public class StationInfoDto
    {
        public string? LineName { get; set; }
        public string? StageName { get; set; }
        public string? StationType { get; set; }
        public string? StationName { get; set; }
        public string? StationTypeDesc { get; set; }
    }
}
