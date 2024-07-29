using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class MntnWoStatusDto
    {
        public string WorkOrder {  get; set; }  
        public string Status { get; set; }
        public string Memo { get; set; }
        public string Site { get; set; }
        public string UpdateEmpno { get; set; }

    }
}
