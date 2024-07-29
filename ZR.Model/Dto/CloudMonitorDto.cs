using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;

namespace ZR.Model.Dto
{
    public class PTicketStatusQueryDto : PagerInfo
    {
        public string EmpNo {  get; set; }
        public string UpdateEmpno { get; set; }
        public string ProgramMent { get; set;}
        public string StationName { get; set; }
        public string AssignEmp {  get; set; }
        public string ClentIp { get; set; }
        public long Status { get; set;}
        public string Type { get; set; }
    }
    public class PTicketReportQueryDto 
    {
        public string Type { get; set; }
        public string TimeType { get; set; }
        public string Site { get; set; }
    }
        public class PTicketReportRes
        {
        public string   Time { get; set; }
            public string Name { get; set; }
            public int Qty { get; set; }
        }
    

}
