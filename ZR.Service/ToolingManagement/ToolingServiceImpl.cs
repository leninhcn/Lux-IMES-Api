using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.System;
using ZR.Service.ToolingManagement.IService;
using ZR.Repository;
using ZR.Model.Dto;
using Mapster;

namespace ZR.Service.ToolingManagement
{
    [AppService(ServiceType = typeof(IToolingService), ServiceLifetime = LifeTime.Transient)]
    public class ToolingServiceImpl : BaseService<MTooling>, IToolingService
    {
        public long AddInfo(MTooling tooling)
        {
            long MaxId = Context.Queryable<MTooling>().Max(it => it.Id) + 1;
            MaxId = 0 == MaxId ? 101 : MaxId;
            tooling.Id = MaxId;
            long insertErp = Context.Insertable(tooling).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"INSERT INTO IMES.M_TOOLING_HT(SELECT * FROM IMES.M_TOOLING WHERE ID = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }

        public string CheckInfoNameUnique(MTooling tooling)
        {
            long Id = 0 == tooling.Id ? -1L : tooling.Id;
            MTooling info = GetFirst(it => it.ToolingNo == tooling.ToolingNo);
            if (info != null && info.Id != Id)
            {
                return UserConstants.NOT_UNIQUE;
            }
            return UserConstants.UNIQUE;
        }

        public MTooling GetInfoById(long id)
        {
            var response = Queryable()
               .Where(x => x.Id == id)
               .First();

            return response;
        }

        public PagedInfo<MTooling> GetInfoList(MesCommonQuery query, string site)
        {
            var exp = Expressionable.Create<MTooling>();
            exp.AndIF(!string.IsNullOrEmpty(site), it => it.Site == site);
            exp.AndIF(query.Enabled != "ALL", it => it.Enabled == query.Enabled);

            exp.AndIF(!string.IsNullOrEmpty(query.FilterValue), it => SqlFunc.MappingColumn<string>(query.FilterField).Contains(query.FilterValue));
            return Context.Queryable<MTooling>().Where(exp.ToExpression()).OrderBy(it => it.UpdateTime).ToPage(query);
        }

        public async Task SwitchStatus(long id, string status, string empNo)
        {
            await Context.Updateable<MTooling>()
                .SetColumns(_ => new MTooling
                {
                    Enabled = status,
                    UpdateEmpNo = empNo,
                    UpdateTime = DateTime.Now,
                })
                .Where(x => x.Id == id)
                .ExecuteCommandAsync();
        }

        public int UpdateInfo(MTooling tooling)
        {
            int updateType = Context.Updateable(tooling).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.Id).ExecuteCommand();

            if (updateType > 0)
            {
                string sqlStr = $"INSERT INTO IMES.M_TOOLING_HT(SELECT * FROM IMES.M_TOOLING WHERE ID =  " + tooling.Id + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return 0;
        }




    }
}
