using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;

namespace ZR.Service.Repair.IRepairService
{
    public interface IFAMaintainMService
    {
        Task<ExecuteResult> GetDefectByCode(string defCode);
        Task<ExecuteResult> getDefectType();
        Task<ExecuteResult> GetEmpInfoByNo(string empno);
        Task<ExecuteResult> getLine();
        Task<ExecuteResult> getModel();
        Task<ExecuteResult> getShowData();
    }
}
