using JinianNet.JNTemplate.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;

namespace ZR.Service.IService
{
    public interface IProcessTechnologyService
    {
        int DeleteStationByID(StationTechInfo stationTechInfo);
        int UpdateStation(StationTechInfo stationTechInfo);

        int InsertStatinoInfo(StationTechInfo stationTechInfo);
        int CopyStationInfo(StationTechInfo stationTechInfo);
        PagedInfo<StationTechInfo>ShowStationInfo(string enaBled, string optionData, string textData, int pageNum, int pageSize);
        List<StationTypeInfo> ShowStationType(string textData);

        (string, object, object) StationImportData(List<StationTechInfo> station, string site, string name);
        List<StationTechInfo> StationHistory(string id);
       PagedInfo<LineTypeInfo> ShowLineInfo(string enaBled, string optionData, string textData, int pageNum, int pageSize);
        int InsertLineInfo(LineTypeInfo lineTypeInfo);
        int UpdateLine(LineTypeInfo lineTypeInfo);
        int DeleteLineByID(LineTypeInfo lineTypeInfo);
        (string, object, object) LineImportData(List<LineTypeInfo> stationTypes, string site, string name);
        PagedInfo<StageInfo> ShowStageInfo(string enaBled, string optionData, string textData, int pageNum, int pageSize);
        int InsertStageInfo(StageInfo stageInfo);
        int UpdateStage(StageInfo stageInfo);
        int DeleteStageByID(StageInfo stageInfo);
        List<StageInfo> StageHistory(string id);
        PagedInfo<StationTypeInfo> ShowStationTypeInfo(string stage, string enaBled, string optionData, string textData, int pageNum, int pageSize);
        int InsertStationTypeInfo(StationTypeInfo stationTypeInfo);
        int UpdateStationType(StationTypeInfo stationTypeInfo);
        int DeleteStationTypeByID(StationTypeInfo stationTypeInfo);
        List<StationTypeInfo> StationTypeHistory(string id);

        (string, object, object) StationTypeImportData(List<StationTypeInfo> stationTypes, string site, string name);
        List<String>  GetStationType();
        List<String>  GetClientType();
        List<RouteInfo> GetRouteName(string routeName, string site);
        List<RouteMaintenanceInfoVo> GetStationTypeInfo(string stage, string site);
        List<RouteMaintenanceInfoVo> GetRouteDetail(string routeName, string site);
        List<String> CheckRouteWIP(string routeName, string site);
        int UpdateRoute(RouteInfo routeInfo);
        int UpdateMustStation(RouteDetailInfo routeDetailInfo);
        int CheckRouteName(List<RouteDetailVo> routeDetailVo, string site, string name);
    }

}
