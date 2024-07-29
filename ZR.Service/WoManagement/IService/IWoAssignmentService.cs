using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Model.Dto.Machine;
using ZR.Model.Dto.WorkOrder;

namespace ZR.Service.WoManagement.IService
{
    public interface IWoAssignmentService : IBaseService<MWoAssignment>
    {
        public List<WoAssignmentDto> GetList(string site, DateTime WoCreateDateStart, DateTime WoCreateDateEnd);

        public ExecuteResult CheckPKG(string wo);


        public bool ExistedWorkproc(string StationType);

        public ExecuteResult CheckReleaseWo(string wo, string site);

        public ExecuteResult CheckDrawCardVersion(string wo, string site);

        public ExecuteResult PackCheck(string wo, string site);



        public void ModifyAssignDate(int type, string wo, string machine, string empno, string assingStartDate, string assignEndDate);

        public void ModifyWoBaseAssignDate(string wo, string site);

        public ExecuteResult ModifyData(string xmldata);

        public MWoAssignment CehckAssignInfo(MWoAssignment mWoAssignment);

        public DataTable GetWoInfo(string wo ,string site);

        public ExecuteResult CheckOutputQMS(string stationtype);

        public MApiConfig GetApiConfig(string apicode);

       

        public ExecuteResult SendQms(string wo, string site, string empno);

        public List<MachineGroupToWoAssign> GetMachineGroupToWoAssign(string stationType, string site);

        public List<MachineToWoAssign> GetMachineToWoAssign(string machineGroup, string site);

        public DataTable GetWoAssignment(string wo);


        public DataTable getMachine(string[] group);


        public DataTable getMachine(string[] machines, string workproc, string workorder = null);

        public DataTable getAssignment(string wo);


    }
}
