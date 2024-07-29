using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;
using ZR.Model.Dto;

namespace ZR.Service.IService
{
    public interface IProdAssyService
    {
        Task<DataTable> CheckALFixedAssetsIsExist(string sn);
        Task<(string, string)> CheckCarrier(string station, string wo, string sn, string carrier, string uid);
        Task<bool> CheckEssTest(string stationType, string ess);
        Task<bool> CheckFixedAssets(string sn);
        Task<string> CheckIPN(string sn);
        Task<bool> CheckIsErrorCode(string ecode);
        Task<bool> CheckIsLotAssy(string lineName, string stationType);
        Task<string> CheckKpsnIputMPN(string mpn, string ipn);
        Task<string> CheckMachine(string station, string wo, string sn, string machine, string uid);
        Task<(string, string)> CheckSnBefore(string station, string wo, string sn, string uid);
        Task<(string, string)> CheckSNInput1WO(string station, string wo, string sn, string uid);
        Task<(string, string)> CheckSNInput2WO(string station, string wo, string sn, string uid);
        Task<(string, string)> CheckSNInputNoWo(string station, string wo, string input, string uid);
        Task<(string, string)> CheckSnPass(string station, string wo, string sn, string uid);
        Task<string> CheckSnPassBundle(string station, string wo, string sn, string uid);
        Task<string> CheckSnPassPanel(string station, string wo, string sn, string uid);
        Task<(string, string)> CheckTooling(string station, string wo, string sn, string tool, string uid);
        Task<(string, string)> CheckWo(string wo, string station);
        Task<List<BomInfo>> GetBomInfosBySN(string stationType, string sn);
        Task<List<BomInfo>> GetBomInfosByWO(string stationType, string wo);
        Task<(string, string)> GetCompareImageFileName(string station, string wo, string sn, string uid);
        Task<DataTable> GetHDDInfo(string sn);
        Task<DataTable> GetIPNAPNBySN(string sn);
        Task<List<ItemInfo>> GetItemInfo(string sn);
        Task<(string, string)> GetMainSn(string input, string station);
        Task<List<dynamic>> GetModel(string mainSn);
        Task<int> GetPanelLinkQtyBySN(string sn);
        ISugarQueryable<MBlockConfigType, MBlockConfigValue> GetRule();
        Task<SnInfo> GetSnInfoBySn(string mainSn);
        Task<List<StationAction>> GetStationActionList(StationInfoDto stationInfo);
        Task<DataTable> GetTdId();
        Task<WoInfo> GetWoInfoByWo(string wo);
        Task<(string, string)> GetWoList(string station, string cmd);
        Task<bool> InsertALLabelInfo(string tdId, string sn, string uid);
        Task<bool> RelieveLink(string sn);
    }
}
