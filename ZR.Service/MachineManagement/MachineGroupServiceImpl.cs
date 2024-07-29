using Infrastructure.Attribute;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.System;
using ZR.Service.MachineManagement.IMachineManagementService;
using ZR.Repository;
using ZR.Model.Dto;

namespace ZR.Service.MachineManagement
{
    [AppService(ServiceType = typeof(IMachineGroupService), ServiceLifetime = LifeTime.Transient)]
    public class MachineGroupServiceImpl : BaseService<MMachineGroup>, IMachineGroupService
    {
        public long AddMachineGroup(MMachineGroup group)
        {
            long MaxId = Context.Queryable<MMachineGroup>().Max(it => it.Id) + 1;
            MaxId = 0 == MaxId ? 101 : MaxId;
            group.Id = MaxId;
            long insertErp = Context.Insertable(group).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"INSERT INTO IMES.M_MACHINE_GROUP_HT(SELECT * FROM IMES.M_MACHINE_GROUP WHERE ID = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }

        public async Task SwitchStatus(long id, string status, string empNo)
        {
            await Context.Updateable<MMachineGroup>()
                .SetColumns(_ => new MMachineGroup
                {
                    Enabled = status,
                    UpdateEmpNo = empNo,
                    UpdateTime = DateTime.Now,
                })
                .Where(x => x.Id == id)
                .ExecuteCommandAsync();
        }

        public string CheckMachineGroupUnique(MMachineGroup group)
        {
            long Id = 0 == group.Id ? -1L : group.Id;
            MMachineGroup info = GetFirst(it => it.Name == group.Name);
            if (info != null && info.Id != Id)
            {
                return UserConstants.NOT_UNIQUE;
            }
            return UserConstants.UNIQUE;
        }

        public MMachineGroup GetMachineGroupById(long id)
        {
            var response = Queryable()
                .Where(x => x.Id == id)
                .First();
            return response;
        }

        public PagedInfo<MMachineGroup> GetMachineGroupList(MntnMachineCommonQuery query, string site)
        {
            var exp = Expressionable.Create<MMachineGroup>();
            exp.AndIF(!string.IsNullOrEmpty(site), it => it.Site == site);
            exp.AndIF(query.Enabled != "ALL", it => it.Enabled == query.Enabled);
            exp.AndIF(!string.IsNullOrEmpty(query.FilterValue), it => SqlFunc.MappingColumn<string>(query.FilterField).Contains(query.FilterValue));

            return Context.Queryable<MMachineGroup>()
                .Where(exp.ToExpression())
                .OrderBy(it => it.UpdateTime)
                .ToPage(query);
        }

        public int UpdateMachineGroup(MMachineGroup group)
        {
            int updateMachineType = Context.Updateable(group).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.Id).ExecuteCommand();

            if (updateMachineType > 0)
            {
                string sqlStr = $"INSERT INTO IMES.M_MACHINE_GROUP_HT(SELECT * FROM IMES.M_MACHINE_GROUP WHERE ID =  " + group.Id + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return 0;
        }
    }
}
