using Infrastructure.Attribute;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.Quality;
using ZR.Service.Quality.IQualityService;

namespace ZR.Service.Quality
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IHoldOptionsService), ServiceLifetime = LifeTime.Transient)]
    public class PHoldOptionsService : BaseService<PHoldOptionsDto>, IHoldOptionsService
    {
        public Dictionary<string,List<PHoldOptionsDto>> GetPHoldOptionsDto()
        {
            var sql = " WITH METHODOPTIONS "
                    +" AS(SELECT 101 ID,'小板码' NAME,'SN' VALUE,0 STATUS FROM DUAL "
                    +"      UNION "
                    +"    SELECT 102 ID,'工单' NAME,'WO' VALUE,0 STATUS FROM DUAL "
                    +"      UNION "
                    +"    SELECT 103 ID,'料号' NAME,'PART' VALUE,0 STATUS FROM DUAL "
                    +"      UNION "
                    +"    SELECT 104 ID,'大板码' NAME,'PANEL_NO' VALUE,0 STATUS FROM DUAL) "
                    +",OPERATEOPTIONS "
                    +" AS(SELECT 201 ID,'卡控' NAME,'HOLD' VALUE,0 STATUS FROM DUAL "
                    +"      UNION "
                    +"    SELECT 202 ID,'解除' NAME,'RELEASE' VALUE,0 STATUS FROM DUAL) "
                    +",STATIONOPTIONS "
                    +" AS(SELECT DISTINCT STATION_TYPE NAME,STATION_TYPE VALUE,0 STATUS FROM IMES.M_STATION_TYPE) "
                    +",STAGEOPTIONS "
                    +" AS(SELECT DISTINCT STAGE NAME,STAGE VALUE,0 STATUS FROM IMES.M_STATION_TYPE) "
                    +" SELECT *FROM METHODOPTIONS "
                    +"     UNION  "
                    +" SELECT *FROM OPERATEOPTIONS "
                    +"     UNION  "
                    +" SELECT ROWNUM+300 ID, A.* FROM STATIONOPTIONS A"
                    +"     UNION "
                    +" SELECT ROWNUM+1000 ID, B.* FROM STAGEOPTIONS B";
            var res = SqlQueryToList(sql);
            var dict = new Dictionary<string, List<PHoldOptionsDto>>();
            dict.Add("METHODOPTIONS", res.Where(r => r.Id > 100 && r.Id < 200).ToList());
            dict.Add("OPERATEOPTIONS", res.Where(r => r.Id > 200 && r.Id < 300).ToList());
            dict.Add("STATIONOPTIONS", res.Where(r => r.Id > 300 && r.Id < 1000).ToList());
            dict.Add("STAGEOPTIONS", res.Where(r => r.Id > 1000).ToList());
            return dict;
        }
    }
}
