using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Service.Repair.IRepairService
{
    public interface IRapidRepairService
    {
        Task<bool> AddRapidRepair(RapidRequst rapid, string userNo);
        Task<ExecuteResult> CheckIsErrorCode(string errorcode);
        Task<DataTable> checkstatus(string sn);
        Task<DataTable> getDefect1(string sNInfo);
        Task<DataTable> getDetail(string snno);
        Task<ExecuteResult> RapidRepairGo(string station, string sn, string nstation, string userNo);
    }
}
