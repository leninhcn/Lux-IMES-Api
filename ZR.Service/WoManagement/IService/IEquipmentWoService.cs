using Infrastructure.Attribute;
using NLog.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto.WorkOrder;

namespace ZR.Service.WoManagement.IService
{
    public interface IEquipmentWoService : IBaseService<HjxsPieWtEquDiSpatchListH>
    {
        public PagedInfo<HjxsPieWtEquDiSpatchListH> ShowData(EquipmentWoFilter filter, PagerInfo pager, string empno);

        public List<HjxsPieWtEquDispatchListB> getDispatchList_b(string strID);

        public List<string> getChejian(string site);

        public DataTable GetEquipmentCode(string devType);

        public DataTable CheckEquipmentCodeStatus(string devCode);

        public DataTable GetProductLot(string Chejian);

        public bool ModuleAuth(string moduleName, string pAction,string site, string empno);

        public DataTable getTraceAll(string billNum);

        public DataTable getWorkLine(string worknum);

        public List<HjxsPieWtEquDispatchTrace> getTraceByType(string billNum, string strType);

        public string GetNoteNum(string pNoteType);

        public string GetGuID();

        public bool SaveDispatchList_h(HjxsPieWtEquDiSpatchListH dto, string empNo);

        public bool ModifyDispatchList_h(HjxsPieWtEquDiSpatchListH dto, string empno, string sUpdateType);

        public bool SaveDispatchList_B(HjxsPieWtEquDispatchListB dispatchListB);

        public bool ModifyDispatchList_B(HjxsPieWtEquDispatchListB dispatchListB);

        public bool ModifyScanLableData(HjxsPieWtScanLableData hjxsPieWtScanLableData);

        public bool SaveScanLableData(HjxsPieWtScanLableData hjxsPieWtScanLableData);

        public string CheckSix(string inputSn);

        public List<HjxsPieWtScanLableData> GetScanLableData(HjxsPieWtScanLableData sld);


    }
}
