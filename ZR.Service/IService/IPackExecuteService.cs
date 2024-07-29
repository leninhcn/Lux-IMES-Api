using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Dto.ProdDto;

namespace ZR.Service.IService
{
    public interface IPackExecuteService
    {
        Task<(bool, string)> CheckWo(string wo, string station);
        Task<List<StationLabelDto>> GetLabelList(string wo, string stationType);
        Task<MLabelServer> GetLabelServerInfo(StationLabelDto label);
        Task<DataTable> GetOptionData(string station);
        Task<(int, int)> GetPackQty(string type, string packNo, string wo = "");
        Task<DataTable> GetPackingAction();
        Task<MPKSpecDto> GetPkSpec(string wo);
        Task<string> GetUnfinishPallet(string type, string typeValue, string station, string pkSpecName);
        Task<WoBase> GetWoInfo(string wo);
        Task<string> GetWorkOrderBySn(string input);
        Task<(bool, string, string)> CreateNewPack(string type, string wo, string ipn, string specName, string station, long uid, string userNo);
        Task<string> GetUnfinishCarton(string type, string typeValue, string station, string pkSpecName);
        Task<string> GetUnfinishBox(string type, string typeValue, string station, string pkSpecName);
        Task<string> GetSn(string input);
        Task<bool> CheckDefectcode(string input);
        Task<(bool, string)> CheckSnBefore(string station, string wo, string sn, string empNo);
        Task<(string, string, string)> GetOldPackNoBySn(string sn);
        Task<(bool, string)> CheckRoute(string station, string sn);
        Task<(bool, string)> CheckStationSN(string station, string wo, string sn, string empNo);
        Task<(bool, string)> CheckMix(string station, string wo, string sn, string box, string carton, string pallet, string empNo);
        Task<(bool, string)> PackingRepackGo(string station, string pkAction, string packNo, string empNo);
        Task<bool> PackIsClosed(string type, string packNo);
        Task<(bool, string)> CheckPackInfoByCarton(string pallet, string carton);
        Task<(bool, string)> CheckPackInfoByBox(string box, string carton);
        Task<(bool, string, string)> GetNewNo(string type, string wo, long uid);
        Task<(string, string)> Get_NextNewNo(string type, string wo, string sNewNo, long uid);
        Task Append_PackNo(string sType, string wo, string ipn, string station, string sPackSpecName, string sNo, string userNo);
        Task<(bool, string)> PackingGo(string station, string pkAction, string sn, string csn, string box, string carton, string pallet, string empNo);
        Task<(string, Dictionary<string, string>)> GetLabelVarsPrintData(string inputData, string labelType);
        Task<bool> CheckPackNo(string packNo, string type);
        Task ClosePackNoByPack(string packNo, string type);
        Task SavePackForceClose(string packNo, string packType, string userNo);
        Task<bool> CheckPackNoByPack(string packNo, string type);
        Task DeletePackNoByPack(string packNo, string type);
        Task<object> GetPkConfigData();
        Task<List<ModuleParam>> GetModuleParamList(string station);
        Task SavePkConfig(PkConfigSaveDto config, string uid);
    }
}
