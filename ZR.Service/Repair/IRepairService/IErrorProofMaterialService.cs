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
    public interface IErrorProofMaterialService
    {
        Task<ExecuteResult> CheckNewReelPn(string wo, string oldReelPn, string newReelPn);
        Task<ExecuteResult> CheckReelNo(string reelno);
        Task<ExecuteResult> CheckWoBomItemIpn(string wo, string ipn);
        Task<DataTable> GetSnStatusInfo(string sn);
        Task<ExecuteResult> SaveReplaceReelDetail(RepairInfo repairInfo, string recID, string empno, string time);
    }
}
