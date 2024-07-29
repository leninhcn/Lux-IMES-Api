using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;

namespace ZR.Service.Repair.IRepairService
{
    public interface IRepPrintlabService
    {
        Task<ExecuteResult> checkprintlog(string sn, string labeltype);
        Task<ExecuteResult> checkrepairin(string sn);
        Task<ExecuteResult> checksn(string sn);
        Task<ExecuteResult> chekprintrole(string empno, string pwd);
        Task<ExecuteResult> GetLabelInfo(string SN, string StationType);
        Task<ExecuteResult> GetLabelInfo(string StationType);
    }
}
