using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;

namespace ZR.Service.MachineManagement.IMachineManagementService
{
    public interface IMachineService : IBaseService<MMachine>
    {
        public PagedInfo<MMachine> GetMachineList(MntnMachineQuery query, string site);

        public MMachine GetMachineCodeById(long id);

        public string CheckMachineCodeUnique(MMachine machine);

        public long AddMachine(MMachine machine);

        public int UpdateMachine(MMachine machine);

        public List<string> GetMachineTypeList(string site);

        public List<StationInfo> GetStationInfoList(string site);

        Task<List<string>> GetAllMachineLocs();

        Task SwitchStatus(long id, string status, string empNo);
    }
}
