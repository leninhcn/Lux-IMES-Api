using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;

namespace ZR.Service.MachineManagement.IMachineManagementService
{
    public interface IMachineGroupService : IBaseService<MMachineGroup>
    {
        public PagedInfo<MMachineGroup> GetMachineGroupList(MntnMachineCommonQuery query, string site);

        public MMachineGroup GetMachineGroupById(long id);

        public string CheckMachineGroupUnique(MMachineGroup group);

        public long AddMachineGroup(MMachineGroup group);

        public int UpdateMachineGroup(MMachineGroup group);

        Task SwitchStatus(long id, string status, string empNo);
    }
}
