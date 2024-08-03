using Infrastructure.Attribute;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.System;
using ZR.Repository;
using ZR.Service.MachineManagement.IMachineManagementService;

namespace ZR.Service.MachineManagement
{
    [AppService(ServiceType = typeof(IMachineTypeService), ServiceLifetime = LifeTime.Transient)]
    public class MachineTypeServiceImpl : BaseService<MMachineType>, IMachineTypeService
    {
        public string CheckMachineTypeNameUnique(MMachineType machineType)
        {
            long Id = 0 == machineType.Id ? -1L : machineType.Id;
            MMachineType info = GetFirst(it => it.MachineTypeName == machineType.MachineTypeName);
            if (info != null && info.Id != Id)
            {
                return UserConstants.NOT_UNIQUE;
            }
            return UserConstants.UNIQUE;
        }

        public MMachineType GetMachineTypeById(long id)
        {
            var response = Queryable()
                .Where(x => x.Id == id)
                .First();

            return response;
        }

        public PagedInfo<MMachineType> GetMachineTypeList(MntnMachineCommonQuery query, string site)
        {
            var exp = Expressionable.Create<MMachineType>();
            exp.AndIF(!string.IsNullOrEmpty(site), it => it.Site == site);
            exp.AndIF(query.Enabled != "ALL", it => it.Enabled == query.Enabled);
            exp.AndIF(!string.IsNullOrEmpty(query.FilterValue), it => SqlFunc.MappingColumn<string>(query.FilterField).Contains(query.FilterValue));

            return Context.Queryable<MMachineType>()
                .Where(exp.ToExpression())
                .OrderBy(it => it.UpdateTime)
                .ToPage(query);
        }

        public async Task SwitchStatus(long id, string status, string empNo)
        {
            await Context.Updateable<MMachineType>()
                .SetColumns(_ => new MMachineType
                {
                    Enabled = status,
                    UpdateEmpNo = empNo,
                    UpdateTime = DateTime.Now,
                })
                .Where(x => x.Id == id)
                .ExecuteCommandAsync();
        }

        public long AddMachineType(MMachineType machineType)
        {
            long MaxId = Context.Queryable<MMachineType>().Max(it => it.Id) + 1;
            MaxId = 0 == MaxId ? 101 : MaxId;
            machineType.Id = MaxId;
            long insertErp = Context.Insertable(machineType).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"INSERT INTO SAJET.M_MACHINE_TYPE_HT(SELECT * FROM SAJET.M_MACHINE_TYPE WHERE ID = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }

        public int UpdateMachineType(MMachineType machineType)
        {
            int updateMachineType = Context.Updateable(machineType).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.Id ).ExecuteCommand();
            
            if (updateMachineType > 0)
            {
                string sqlStr = $"INSERT INTO SAJET.M_MACHINE_TYPE_HT(SELECT * FROM SAJET.M_MACHINE_TYPE WHERE ID =  " + machineType.Id + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return 0;
        }      
    }
}
