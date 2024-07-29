using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.System;
using ZR.Service;
using ZR.ServiceCore.Model;
using ZR.ServiceCore.Services.IService;

namespace ZR.ServiceCore.Services
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class SiteService : BaseService<SysSite>, ISiteService
    {
        public List<SysSite> GetSiteList()
        {
                return Queryable().Where(m => m.Enabled == "Y" ).ToList();
        }
    }
}
