using Infrastructure.Attribute;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Service.Quality.IQualityService;
using ZR.Model.Quality;

namespace ZR.Service.Quality
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IMQcConfigHtService), ServiceLifetime = LifeTime.Transient)]
    public class MQcConfigHtService : BaseService<MQcConfigHt>, IMQcConfigHtService
    {

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MQcConfigHt AddMQcConfigHt(MQcConfigHt model)
        {
            return Context.Insertable(model).ExecuteReturnEntity();
        }


    }




}
