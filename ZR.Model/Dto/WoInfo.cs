using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.Quality;

namespace ZR.Model.Dto
{
    public class WoInfo
    {
        public string WorkOrder { get; set; }
        public string PartNo { get; set; }
        public string Line { get; set; }
    }
}
