using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Repair.Dto;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Service.Repair.IRepairService
{
    public interface IRepairExeService
    {
        Task<bool> AddDefectInfo(string sRecID, string userNo, repairDef repDef);
        Task<bool> AddScrapInfo(ScrapDto scrapDto, string userNo);
        Task<bool> AddSnTravel(string sn);
        Task<ExecuteResult> CheckNewKeypart(string sn, string oldkpsn, string parttype, string newkpsn);
        Task<DataTable> Checkoldsn(string sn, string oldsn);
        Task<DataTable> Checkparttype(string sn, string oldsn);
        Task<DataTable> CheckReason(string reason);
        Task<ExecuteResult> CheckReelNo(string reelno);
        Task<ExecuteResult> CheckRoute(string station_name, string sn);
        Task<ExecuteResult> CheckSN(string sn);
        Task<DataTable> checkstatus(string snno);
        Task<bool> DeleteDefectByRecid(string sRECID);
        Task<DataTable> getDefect(string sSN);
        Task<DataTable> GetDefectCodeInfo(string sDefectCode);
        Task<DataTable> getDefectReason(string resonType);
        Task<DataTable> getDefectReasonType();
        Task<string> GetDefectRECID();
        Task<DataTable> getDetail(string sNInfo);
        Task<DataTable> getEmpName(string empNo);
        Task<DataTable> GetKPReplaceInfo(string recid, string sn);
        Task<DataTable> getLocation(string sn, string code);
        Task<DataTable> getReason(string sn, string sRECID);
        Task<DataTable> GetRepair1(string sn, string code, string id);
        Task<DataTable> GetReturnStation(string sn, string process);
        Task<DataTable> getStep(repairExe baseInfo);
        Task<bool> InsertReelNo(string ReelNo, string editPartNo, string editDateCode, string txtUnitQty, string editVendor, string editLot, string userNo);
        Task<bool> InsertReelNo1(string ReelNo, string editPartNo, string editDateCode, string txtUnitQty, string editVendor, string editLot, string msd, string userNo);
        Task<bool> InsertReelNo1_HT(string ReelNo, string editPartNo, string editDateCode, string txtUnitQty, string editVendor, string editLot, string msd, string userNo);
        Task<bool> InsertReelNoSMT(string ReelNo, string editPartNo, string editDateCode, string txtUnitQty, string editVendor, string editLot, string userNo);
        Task<bool> InsertReelNoSMT1(string ReelNo, string editPartNo, string editDateCode, string txtUnitQty, string editVendor, string editLot, string msd, string userNo);
        Task<bool> InsertReelNo_HT(string ReelNo, string editPartNo, string editDateCode, string txtUnitQty, string editVendor, string editLot, string userNo);
        Task<ExecuteResult> RepairGo(string station, string sn, string nstation, string userNo);
        Task<ExecuteResult> RepairSnReason(string sn, string wo, string ipn, string recid, string reason, string reasontype, string duty, string station_name, string userNo);
        Task<ExecuteResult> SaveRepairDetail(RepairInfo repairInfo, string recID, string ipn, string oldsn, string newsn, string empno, string time, string station, string userNo);
        Task<ExecuteResult> SaveRepairDetail1(RepairInfo repairInfo, string recID, string empno, string time, string station, string userNo);
        Task<bool> UpdateSnStatus(string sn, string userNo);
    }
}
