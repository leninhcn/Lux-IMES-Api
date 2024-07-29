using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Service.IService
{
    public interface IProdRecheckService
    {
        Task<string> CheckLogic(string sn, string type, string qcInStationType, string qcOutStationType, string uid);
        Task<List<string>> GetHtValues();
        Task<DataTable> GetSnDetail(string sn, string type);
    }
}
