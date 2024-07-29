using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;

namespace ZR.Service.Repair.IRepairService
{
    public interface IRepairExeReplaceService
    {
        Task<ExecuteResult> CheckNewKeypart(string sn, string oldkpsn, string parttype, string newkpsn);
        Task<DataTable> getDefect(string defect);
        Task<DataTable> getKpsn(string sn);
        Task<bool> InsertReplaceKP(string recid, string station, string sn, string oldkpsn, string oldpartno, string newkpsn, string newpartno, string remark, string EMPNO, string lotcode, string datecode, string rid);
        Task<ExecuteResult> RemoveKp(string station, string sn, string recid, string kpsn, string partno, string kpflag, string defect_data, string userNo);
        Task<ExecuteResult> ReplaceKpsn(string station, string sn, string recid, string oldkpsn, string oldpartno, string newkpsn, string newpartno, string newitemGroup, string kpflag, string kpdefect_data, string remark, string EMPNO, string KP_CUSTOMERSN, string KP_MAC, string lotcode, string datecode);
    }
}
