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
    public interface IFAETransferUsersService
    {
        Task<ExecuteResult> EditValueInfo(FAETransInfo model, string userNo);
        Task<ExecuteResult> GetHtValues(string ID);
        Task<ExecuteResult> importDataToTable(List<FAETransInfo> faeTrans, string userNo);
        Task<ExecuteResult> ShowData(FAETransUser retData);
    }
}
