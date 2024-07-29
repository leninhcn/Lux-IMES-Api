using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.ServiceCore.Model.Dto
{
    public class LVDefect
    {
        public string DefectCode { get; set; }
        public string DEFECT_DESC { get; set; }
        public string DEFECT_DESC2 { get; set; }
        public string LOCATION { get; set; }
        public string RECID { get; set; }
        public string STATION_TYPE { get; set; }

        public int ImageIndex { get;set; }
    }

    public class responseLV
    {
        public List<string> selectRecid { get; set; }
        public List<failDetail> failItem { get; set; }
    }
    public class failDetail { 
        public string defCode { get; set; }
        public string failMsg { get; set; }
    }
}
