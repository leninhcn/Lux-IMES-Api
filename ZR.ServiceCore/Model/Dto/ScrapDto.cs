using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.ServiceCore.Model.Dto
{
    public class ScrapDto
    {
        public string wo { get; set; }
        public string sn { get; set; }
        public string partNo { get; set; }

        public string version { get; set; }
        public string stationName { get; set; }

        public string stationType { get; set; }
        public string scrapMemo { get; set; }
        public string defectLine { get; set; }

        /// <summary>
        /// Scrap  Return Material
        /// </summary>
        public string scrapType { get; set;} 
    }
}
