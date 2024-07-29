using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Dto.Quality;

namespace ZR.Service.IService
{
    public interface IStationPassQtyService
    {
   
        List<string> GetLineInfo(string site);

        List<string>  GetStationTypeInfo(string line, string site);

        List<string> GetStationNameInfo(string stationType, string stationName, string site);

        PagedInfo<StationInfos>  GetStationInfo(string line, string stationType, string stationName, string site, int pageNum, int pageSize);

        int UpdateStationInfo(StationInfos stationInfos);

        PagedInfo<WeightFaiInfo> ShowData(string enaBled, string textData,string optionData, int pageNum, int pageSize, string site);

        int InsertWeightFai(WeightFaiInfo weightFaiInfo, string site);

        int ExistIPN(string ipn);

        int UpdateWeightFai(WeightFaiInfo weightFaiInfo, string site);

        int UpdateWeightFaiState(WeightFaiInfo weightFaiInfo, string site);

        List <WeightFaiInfo> History(string id, string site);

        int DeleteWeightFai(int id, string site);
    }
} 
