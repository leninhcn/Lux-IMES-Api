using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;

namespace ZR.Service.IService
{
    public interface IProdInputService
    {
        public Task<string> GetStationDesc(string stationType);

        public Task<List<string>> GetLineWo(string line, string wo);

        public ISugarQueryable<MOperationStep> GetOprateStep(string stationType);
        public Task<bool> CheckSNStatus(string sn);

        public Task<(string, string)> GetValues(string inputValue, string item, string modelName);

        public Task<(string, string)> GetPamSn(string wo);

        public Task<(string, string)> CheckValue(string inputValue, string item, string modelName);

        public Task<List<PSnKeyparts>> GetSNLinkKpsnInfo(string stationType, string sn);

        public Task<bool> ClearGetSN(string sn);

        public ISugarQueryable<SnStatus> GetWoSnStatus(string wo);
        public ISugarQueryable<WoBase, StationtypePartSpec, WoBom, PartSpecErpMesMapping, SnFeature> GetWoBomKeyparts(string wo, string stationType);

        public Task<int> GetStationLinkCount(string model, string stationType);

        public Task<int> GetEmpWoPassCount(string stationType, string uid);

        public Task<(string, string)> CheckKpsn(string sn, string wo, string kpsn, string station, string uid);

        public Task<bool> CheckStockOut(string stationType);

        Task<string> CheckLogic(string sn, string wo, string kpsn, string model, string tool, string glue, string reel, string station, string route, string step, string errorCode, string uid);
    }
}
