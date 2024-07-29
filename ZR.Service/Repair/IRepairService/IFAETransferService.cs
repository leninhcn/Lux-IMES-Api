using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Service.Repair.IRepairService
{
    public interface IFAETransferService
    {
        Task<ExecuteResult> CheckBackUser(string empno);
        Task<ExecuteResult> CheckOutUser(string empno);
        Task<ExecuteResult> CheckRepairIn(string SN);
        Task<ExecuteResult> FAESNOUTCheck(string sn);
        Task<ExecuteResult> GetFAESNOUT(string sn);
        Task<ExecuteResult> GetRepairInRecid(string sn);
        Task<ExecuteResult> GetValues(string INPUTVALUE, string ITEM, SNTInfo sn);
        Task<ExecuteResult> InsertTransfer(FAETransferInfo transferInfo);
        Task<ExecuteResult> UpdateTransfer(FAETransferInfo transferInfo);
    }
}
