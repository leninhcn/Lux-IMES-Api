using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.System;
using ZR.Service.MachineManagement.IMachineManagementService;
using ZR.Service.ToolingManagement.IService;
using ZR.Repository;
using ZR.Model.Dto;

namespace ZR.Service.ToolingManagement
{
    [AppService(ServiceType = typeof(IToolingTypeService), ServiceLifetime = LifeTime.Transient)]
    public class ToolingTypeServiceImpl : BaseService<MToolingType>, IToolingTypeService
    {
        public long AddInfo(MToolingType type)
        {
            long MaxId = Context.Queryable<MToolingType>().Max(it => it.Id) + 1;
            MaxId = 0 == MaxId ? 101 : MaxId;
            type.Id = MaxId;
            long insertErp = Context.Insertable(type).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"INSERT INTO IMES.M_TOOLING_TYPE_HT(SELECT * FROM IMES.M_TOOLING_TYPE  WHERE ID = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }

        public string CheckInfoNameUnique(MToolingType type)
        {
            long Id = 0 == type.Id ? -1L : type.Id;
            MToolingType info = GetFirst(it => it.ToolingType == type.ToolingType);
            if (info != null && info.Id != Id)
            {
                return UserConstants.NOT_UNIQUE;
            }
            return UserConstants.UNIQUE;
        }

        public List<string> GetAllTypeList(string site)
        {
            string strSql = @"SELECT TOOLING_TYPE FROM IMES.M_TOOLING_TYPE WHERE SITE =@SITE AND ENABLED = 'Y'";
            var TypeList = Context.Ado.SqlQuery<dynamic>(strSql, new SugarParameter("@SITE", site));
            List<string> resList = new List<string>();
            foreach (var item in TypeList)
            {
                resList.Add(item.TOOLING_TYPE.ToString());
            }
            return resList;
        }

        public MToolingType GetInfoById(long id)
        {
            var response = Queryable()
               .Where(x => x.Id == id).OrderByDescending(x => x.CreateTime)
               .First();

            return response;
        }

        public PagedInfo<MToolingType> GetInfoList(MesCommonQuery query, string site)
        {
            var exp = Expressionable.Create<MToolingType>();
            exp.AndIF(!string.IsNullOrEmpty(site), it => it.Site == site);
            exp.AndIF(query.Enabled != "ALL", it => it.Enabled == query.Enabled);
            exp.AndIF(!string.IsNullOrEmpty(query.FilterValue), it => SqlFunc.MappingColumn<string>(query.FilterField).Contains(query.FilterValue));

            return Context.Queryable<MToolingType>().Where(exp.ToExpression()).OrderBy(it => it.UpdateTime).ToPage(query);
        }

        public async Task SwitchStatus(long id, string status, string empNo)
        {
            await Context.Updateable<MToolingType>()
                .SetColumns(_ => new MToolingType
                {
                    Enabled = status,
                    UpdateEmpNo = empNo,
                    UpdateTime = DateTime.Now,
                })
                .Where(x => x.Id == id)
                .ExecuteCommandAsync();
        }

        public int UpdateInfo(MToolingType type)
        {
            int updateType = Context.Updateable(type).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => it.Id).ExecuteCommand();

            if (updateType > 0)
            {
                string sqlStr = $"INSERT INTO IMES.M_TOOLING_TYPE_HT(SELECT * FROM IMES.M_TOOLING_TYPE WHERE ID =  " + type.Id + ")";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return 0;
        }
    }
}
