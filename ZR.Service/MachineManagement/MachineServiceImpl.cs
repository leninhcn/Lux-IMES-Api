using Infrastructure.Attribute;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.System;
using ZR.Repository;
using ZR.Service.MachineManagement.IMachineManagementService;

namespace ZR.Service.MachineManagement
{
    [AppService(ServiceType = typeof(IMachineService), ServiceLifetime = LifeTime.Transient)]
    public class MachineServiceImpl : BaseService<MMachine>, IMachineService
    {
        public long AddMachine(MMachine machine)
        {
            long MaxId = Context.Queryable<MMachine>().Max(it => it.Id) + 1;
            MaxId = 0 == MaxId ? 101 : MaxId;
            machine.Id = MaxId;
            long insertErp = Context.Insertable(machine).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"INSERT INTO IMES.M_MACHINE_HT(SELECT * FROM IMES.M_MACHINE WHERE ID = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }

        public string CheckMachineCodeUnique(MMachine machine)
        {
            long Id = 0 == machine.Id ? -1L : machine.Id;
            MMachine info = GetFirst(it => it.MachineCode == machine.MachineCode);
            if (info != null && info.Id != Id)
            {
                return UserConstants.NOT_UNIQUE;
            }
            return UserConstants.UNIQUE;
        }

        public MMachine GetMachineCodeById(long id)
        {
            var response = Queryable()
                .Where(x => x.Id == id)
                .First();
            return response;
        }

        public PagedInfo<MMachine> GetMachineList(MntnMachineQuery query, string site)
        {
            var exp = Expressionable.Create<MMachine>();
            exp.AndIF(!string.IsNullOrEmpty(site), it => it.Site == site);
            exp.AndIF(query.Enabled != "ALL", it => it.Enabled == query.Enabled);
            exp.AndIF(query.GroupId != null, it => it.GroupId == query.GroupId.Value);
            exp.AndIF(!string.IsNullOrEmpty(query.FilterValue), it => SqlFunc.MappingColumn<string>(query.FilterField).Contains(query.FilterValue));

            return Context.Queryable<MMachine>()
                .Where(exp.ToExpression())
                .OrderBy(it => it.UpdateTime)
                .ToPage(query);
        }

        public async Task SwitchStatus(long id, string status, string empNo)
        {
            await Context.Updateable<MMachine>()
                .SetColumns(_ => new MMachine
                {
                    Enabled = status,
                    UpdateEmpNo = empNo,
                    UpdateTime = DateTime.Now,
                })
                .Where(x => x.Id == id)
                .ExecuteCommandAsync();
        }

        public async Task<List<string>> GetAllMachineLocs()
        {
            return await Context.Queryable<MMachine>()
                .Where(x => x.Machineloc != null)
                .Select(x => x.Machineloc)
                .Distinct()
                .ToListAsync();
        }

        public List<string> GetMachineTypeList(string site)
        {
            string sqlStr = $"SELECT MACHINE_TYPE_NAME FROM IMES.M_MACHINE_TYPE mmt  WHERE ENABLED = 'Y' AND SITE = '" + site + "'";
            sqlStr = sqlStr + "  order by MACHINE_TYPE_NAME ";
            List<string> list = Context.Ado.SqlQuery<string>(sqlStr);
            return list;
        }

        public List<StationInfo> GetStationInfoList(string site)
        {
            string sqlStr = $"select b.line,b.stage,b.station_type,b.STATION_NAME from  imes.m_station b  where b.SITE ='" + site + "'";
            sqlStr = sqlStr + "  order by b.line,b.stage,b.station_type,b.STATION_NAME    ";
            List<StationInfo> list = Context.Ado.SqlQuery<StationInfo>(sqlStr);
            return list;
        }

        public int UpdateMachine(MMachine machine)
        {
            int updateMachine = Context.Updateable(machine).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.Id).ExecuteCommand();

            if (updateMachine > 0)
            {
                string sqlStr = $"INSERT INTO IMES.M_MACHINE_HT(SELECT * FROM IMES.M_MACHINE WHERE ID =  " + machine.Id + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return 0;
        }
    }
}
