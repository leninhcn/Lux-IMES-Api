using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;
using ZR.Service.IService;

namespace ZR.Service
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class ProdVIService: BaseService<SnStatus>, IProdVIService
    {
        public async Task<bool> GetMarinaStation()
        {
            return await Context.Queryable<SBaseParam>()
                .Where(x => x.Program == "MarinaCheck").AnyAsync();
        }

        public async Task<bool> CheckErrorCode(string errorCode)
        {
            return await Context.Queryable<MDefect>()
                .Where(x => x.DefectCode == errorCode).AnyAsync();
        }

    }
}
