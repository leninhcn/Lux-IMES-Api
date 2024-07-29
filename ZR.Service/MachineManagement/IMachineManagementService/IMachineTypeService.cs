using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;

namespace ZR.Service.MachineManagement.IMachineManagementService
{
    public interface IMachineTypeService : IBaseService<MMachineType>
    {
        public PagedInfo<MMachineType> GetMachineTypeList(MntnMachineCommonQuery machineType, string site);

        public MMachineType GetMachineTypeById(long id);

        public string CheckMachineTypeNameUnique(MMachineType machineType);

        public long AddMachineType(MMachineType machineType);

         public int UpdateMachineType(MMachineType machineType);

        Task SwitchStatus(long id, string status, string empNo);
    }
}
