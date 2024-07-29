using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Service.IService
{
    public interface IProdVIService
    {
        Task<bool> CheckErrorCode(string errorCode);
        Task<bool> GetMarinaStation();
    }
}
