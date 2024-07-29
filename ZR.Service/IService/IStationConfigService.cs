using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;

namespace ZR.Service.IService
{
    public interface IStationConfigService
    {
        Task<List<string>> GetSites();
        Task<List<string>> GetSites(string[] sites);
        ISugarQueryable<Station, MLine, MStage, MStationType, OperateType> GetStationInfo(string site, string[] opTypes, string[] clientTypes);
        //反查   
        ISugarQueryable<Station, MLine, MStage, MStationType, OperateType> GetStationInfoBychildren(string site, string[] opTypes, string[] clientTypes,string stationName);
    }
}
