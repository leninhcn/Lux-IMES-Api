using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;

namespace ZR.Service.IService
{
    public interface IProdOBAService
    {
        Task<string> GetSnFromKpItem(string kpItem);
        ISugarQueryable<SnStatus> GetSnInfo(string sn);
        Task<bool> InsertHold(string sn, string errorMsg, string uid);
        Task<bool> InsertOBA(string sn, string errorMsg, string line, string stationType, string status, string uid);
    }
}
