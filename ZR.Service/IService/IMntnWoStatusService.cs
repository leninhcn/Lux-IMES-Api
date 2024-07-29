using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto;

namespace ZR.Service.IService
{
    public interface IMntnWoStatusService
    {
        Task<string> CheckWoBase(MntnWoStatusDto param);
        Task<List<dynamic>> GetWoBase(string wo, string site);
        Task<string> UpdateWoBase(MntnWoStatusDto param);
    }
}
