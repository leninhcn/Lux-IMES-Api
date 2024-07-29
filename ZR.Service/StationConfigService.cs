using Infrastructure.Attribute;
using JinianNet.JNTemplate;
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
    public class StationConfigService : BaseService<SnStatus>, IStationConfigService
    {
        public async Task<List<string>> GetSites()
        {
            return await Context.Queryable<MSite>()
                .Where(a => a.Enabled == "Y")
                .OrderBy(a => a.Site)
                .Select(a => a.Site)
                .ToListAsync();
        }
        
        public async Task<List<string>> GetSites(string[] sites)
        {
            return await Context.Queryable<MSite>()
                .Where(a => sites.Contains(a.Site) && a.Enabled == "Y") // "LKKS", "LXKS" 
                .OrderBy(a => a.Site)
                .Select(a => a.Site)
                .ToListAsync();
        }

        public ISugarQueryable<Station, MLine, MStage, MStationType, OperateType> GetStationInfo(string site, string[] opTypes, string[] clientTypes)
        {
            return Context.Queryable<Station, MLine, MStage, MStationType, OperateType>((a, b, c, d, e) =>
                a.Line == b.Line
                && a.Stage == c.Stage
                && a.StationType == d.StationType
                && d.OperateType == e.ScanType
                )
                .Where((a, b, c, d, e) =>
                    a.Enabled == "Y"
                    && b.Enabled == "Y"
                    && c.Enabled == "Y"
                    && d.Enabled == "Y"
                    && opTypes.Contains(d.OperateType.ToUpper())
                    && clientTypes.Contains(d.ClientType.ToUpper())
                    && b.Site == site)
                .OrderBy((a, b, c, d, e) => new
                {
                    b.Site,
                    b.Line,
                    c.Stage,
                    d.StationType,
                    a.StationName
                }, OrderByType.Asc);
        }


        public ISugarQueryable<Station, MLine, MStage, MStationType, OperateType> GetStationInfoBychildren(string site, string[] opTypes, string[] clientTypes,string stationName)
        {
            return Context.Queryable<Station, MLine, MStage, MStationType, OperateType>((a, b, c, d, e) =>
                a.Line == b.Line
                && a.Stage == c.Stage
                && a.StationType == d.StationType
                && d.OperateType == e.ScanType
                )
                .Where((a, b, c, d, e) =>
                    a.Enabled == "Y"
                    && b.Enabled == "Y"
                    && c.Enabled == "Y"
                    && d.Enabled == "Y"
                    && opTypes.Contains(d.OperateType.ToUpper())
                    && clientTypes.Contains(d.ClientType.ToUpper())
                    && b.Site == site
                    && a.StationName== stationName)
                .OrderBy((a, b, c, d, e) => new
                {
                    b.Site,
                    b.Line,
                    c.Stage,
                    d.StationType,
                    a.StationName
                }, OrderByType.Asc);
        }


        

    }
}
