using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.Quality;

namespace ZR.Model.Dto
{
    public class MntnDefect :PagerInfo
    {
        public long Id { get; set; }
        public string Enabled { get; set; }
        public string DefectCode { get; set; }
        public string DefectDesc { get; set; }
        public string Site { get;set; }
    }
}
