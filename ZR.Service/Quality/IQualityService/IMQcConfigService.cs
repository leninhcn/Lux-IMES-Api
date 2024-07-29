using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Quality;
using ZR.Model;
using ZR.Model.Dto.Quality;
using ZR.Service.IService;
using System.Data;

namespace ZR.Service.Quality.IQualityService
{
    /// <summary>
    /// service接口
    /// </summary>
    public interface IMQcConfigService : IBaseService<MQcConfig>
    {
        PagedInfo<MQcConfigDto> GetList(MQcConfigQueryDto parm);
        PagedInfo<MQcConfig> GetList(MQcConfigQueryDto parm, PagerInfo pager);
        DataTable GetLines(string parm);
        DataTable GetWos(string parm);
        DataTable GetIpns(string parm);
        DataTable GetOnlineRoute(string parm);
        DataTable GetQcRoute(string parm);
        DataTable GetRouteDetail(string parm);
        int GetQcLevel(string onlinestation, string checkrule);

        string GetRuleType(string val);

        MQcConfig GetInfo(int Id);

        MQcConfig AddMQcConfig(MQcConfig parm);

        int UpdateMQcConfig(MQcConfig parm);

    }




}
