using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Quality
{
    public class PartQueryDto : PagerInfo
    {
        public string ipn { get; set; }
    }
    public class PartDto 
    {
      public string Ipn { get; set; }
    }
}
