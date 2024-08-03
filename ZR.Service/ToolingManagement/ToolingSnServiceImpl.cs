using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Service.ToolingManagement.IService;
using ZR.Repository;
using ZR.Model.System;
using ZR.Model.Dto.Tooling;
using System.Security.Cryptography;

namespace ZR.Service.ToolingManagement
{
    [AppService(ServiceType = typeof(IToolingSnService), ServiceLifetime = LifeTime.Transient)]
    public class ToolingSnServiceImpl : BaseService<MToolingSn>, IToolingSnService
    {
        public long AddInfo(MToolingSn toolingsn)
        {
            long MaxId = Context.Queryable<MToolingSn>().Max(it => it.ToolingSnId) + 1;
            MaxId = 0 == MaxId ? 101 : MaxId;
            toolingsn.ToolingSnId = MaxId;
            long insertErp = Context.Insertable(toolingsn).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"INSERT INTO SAJET.M_TOOLING_SN_HT(SELECT * FROM SAJET.M_TOOLING_SN WHERE TOOLING_SN_ID = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }

        public string CheckInfoNameUnique(MToolingSn toolingsn)
        {
            long Id = 0 == toolingsn.ToolingSnId ? -1L : toolingsn.ToolingSnId;
            MToolingSn info = GetFirst(it => it.ToolingSn == toolingsn.ToolingSn);
            if (info != null && info.ToolingSnId != Id)
            {
                return UserConstants.NOT_UNIQUE;
            }
            return UserConstants.UNIQUE;
        }

        public MToolingSn GetInfoByToolingSn(string site, string toolingSn)
        {
            var response = Queryable()
               .Where(x => x.Site == site)
               .Where(x => x.ToolingSn == toolingSn)
               .Where(x => x.Enabled == "Y")
               .First();

            return response;
        }

        public MToolingSn GetInfoByToolingSnId(long toolingSnId)
        {
            var response = Queryable()
               .Where(x => x.ToolingSnId == toolingSnId)
               .First();

            return response;
        }

        public PagedInfo<MToolingSn> GetInfoList(ToolingSnQuery query, string site)
        {
            var exp = Expressionable.Create<MToolingSn>();
            exp.AndIF(!string.IsNullOrEmpty(site), it => it.Site == site);
            exp.AndIF(query.ToolingId != null, it => it.ToolingId == query.ToolingId);
            exp.AndIF(query.Enabled != "ALL", it => it.Enabled == query.Enabled);
            exp.AndIF(!string.IsNullOrEmpty(query.FilterValue), it => SqlFunc.MappingColumn<string>(query.FilterField).Contains(query.FilterValue));
            return Context.Queryable<MToolingSn>().Where(exp.ToExpression()).OrderBy(it => it.UpdateTime).ToPage(query);
        }

        public async Task SwitchStatus(long id, string status, string empNo)
        {
            await Context.Updateable<MToolingSn>()
                .SetColumns(_ => new MToolingSn
                {
                    Enabled = status,
                    UpdateEmpNo = empNo,
                    UpdateTime = DateTime.Now,
                })
                .Where(x => x.ToolingSnId == id)
                .ExecuteCommandAsync();
        }

        public int UpdateInfo(MToolingSn toolingsn)
        {
            int updateType = Context.Updateable(toolingsn).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.ToolingSnId).ExecuteCommand();

            if (updateType > 0)
            {
                string sqlStr = $"INSERT INTO SAJET.M_TOOLING_SN_HT(SELECT * FROM SAJET.M_TOOLING_SN WHERE TOOLING_SN_ID =  " + toolingsn.ToolingSnId + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return 0;
        }
    }
}
