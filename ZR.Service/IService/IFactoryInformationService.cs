using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Dto;

namespace ZR.Service.IService
{
    public interface IFactoryInformationService
    {
        int DeleteSiteByID(SiteInfo siteInfo);
        int InsertSiteInfo(SiteInfo siteInfo);
        PagedInfo<SiteInfo> ShowSiteInfo(string enaBled, string optionData, string textData, int pageNum, int pageSize);
        int UpdateSite(SiteInfo siteInfo);
        List<SiteInfo> SiteHistory(string id);
        List<String> GetSiteCode();
        List<String> GetLineType();
        List<String> GetLineLevel();
        List<String>  GetWorkCenter();
        int DeleteLineByID(LineInfo lineInfo);
        int  UpdateLine(LineInfo lineInfo);
        int InsertLineInfo(LineInfo lineInfo);
        PagedInfo<LineInfo>  ShowLineInfo(string enaBled, string optionData, string textData, int pageNum, int pageSize);
        List<LineInfo>  LineHistory(string id);
        (string, object, object)  LineImportData(List<LineInfo> line, string site, string name);
        List<StationMaintenanceVo> GetLine(string line, string site);
        List<StationMaintenanceVo> GetLineStation(string line, string site);
        List<StationMaintenanceVo> GetStageStationtype(string stage, string site);
        List<StationVo> GetLineStationOutExport(string line, string site);
        (string, object, object) StationImportData(List<StationInfo> line, string site, string name);

        int InsertStationInfo(StationInfoVo stationInfoVo);
        int DeleteStation(StationInfoVo stationInfoVo);
    }
}
