using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.ServiceCore.Model;

namespace ZR.ServiceCore.Services.IService
{
    public interface ISiteService
    {
        List<SysSite> GetSiteList();
    }
}
