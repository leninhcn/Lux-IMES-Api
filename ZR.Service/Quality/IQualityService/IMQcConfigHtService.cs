using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Quality;

namespace ZR.Service.Quality.IQualityService
{
    /// <summary>
    /// service接口
    /// </summary>
    public interface IMQcConfigHtService : IBaseService<MQcConfigHt>
    {
        MQcConfigHt AddMQcConfigHt(MQcConfigHt parm);
    }





}
