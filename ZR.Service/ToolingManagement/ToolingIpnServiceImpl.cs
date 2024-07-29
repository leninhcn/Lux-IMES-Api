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
using System.Reflection;
using ZR.Model.System;


namespace ZR.Service.ToolingManagement
{
    [AppService(ServiceType = typeof(IToolingIpnService), ServiceLifetime = LifeTime.Transient)]
    public class ToolingIpnServiceImpl : BaseService<MToolingIpn>, IToolingIpnService
    {
        public long AddInfo(MToolingIpn toolingIpn)
        {
  
            long insertErp = Context.Insertable(toolingIpn).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            return 1;
        }

        public long AddHistory(long toolingId)
        {
            var count = Context.Queryable<MToolingIpn>().Where(it=> it.ToolingId == toolingId).Count();
            if (count > 0)
            {
                string sqlStr = string.Format(@"INSERT INTO IMES.M_TOOLING_IPN_HT (SELECT * FROM IMES.M_TOOLING_IPN WHERE TOOLING_ID = {0} )", toolingId);
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }

            return count;
        }

        public long DeleteInfo(long toolingId)
        {
            var count = Context.Queryable<MToolingIpn>().Where(it => it.ToolingId == toolingId).Count();
            if (count > 0)
            {
                string sqlStr = string.Format(@"DELETE FROM IMES.M_TOOLING_IPN WHERE TOOLING_ID = {0} ", toolingId);
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }

            return count;
        }



        public string CheckInfoNameUnique(MToolingIpn toolingipn)
        {
            var exp = Expressionable.Create<MToolingIpn>();
            exp.And(it => it.ToolingId == toolingipn.ToolingId);
            exp.And(it => it.Ipn == toolingipn.Ipn);
            var info = Queryable().Where(exp.ToExpression()).First();
            if (info != null && info.ToolingId != toolingipn.ToolingId && info.Ipn != toolingipn.Ipn)
            {
                return UserConstants.NOT_UNIQUE;
            }
            return UserConstants.UNIQUE;
        }

        public List<MToolingIpn> GetInfoList(string site, long toolingid)
        {
            var exp = Expressionable.Create<MToolingIpn>();
            exp.AndIF(site != "" && site != null, it => it.Site == site);
            exp.And(it => it.ToolingId == toolingid);
            return Context.Queryable<MToolingIpn>().Where(exp.ToExpression()).OrderBy(it => it.Ipn).ToList(); 
        }
    }
}
