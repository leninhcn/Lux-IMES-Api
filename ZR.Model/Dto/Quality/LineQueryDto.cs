using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Quality
{
    public class LineQueryDto : PagerInfo
    {
        public string rule { get; set; }
    }
    public class LineDto
    { 
     public string Line { get; set; }
    }
}
