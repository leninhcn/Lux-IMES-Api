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
    public class ProdTransferInOutService : BaseService<SnStatus>, IProdTransferInOutService
    {

    }
}
