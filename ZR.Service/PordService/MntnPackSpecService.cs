using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Repository;
using ZR.Service.IService;

namespace ZR.Service.PordService
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    internal class MntnPackSpecService : BaseService<SnStatus>, ImntnPackSpecService
    {
        public PagedInfo<ImesMpkspec> MntnPackSpeclist(string textData, int pageNum, int pageSize, string site)
        {

            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMpkspec>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(textData != "" && textData != null, it => it.pkspecName == textData);
            return Context.Queryable<ImesMpkspec>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
        }

        public int MntnPackSpecInsert(ImesMpkspec imes)
        {
            string site = imes.site;
            string pkspecName = imes.pkspecName;
            int count = Context.Queryable<ImesMpkspec>()
                .Where(it => it.pkspecName == pkspecName && it.site == imes.site)
                .Count();
            if (count>0) {
                return 0;
            }
            imes.updateTime = DateTime.Now;
            imes.id = Context.Queryable<ImesMpkspec>().Max(it => it.id) + 1;
            int insertMpk = Context.Insertable(imes).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            string insertHt = $"insert INTO SAJET.M_PKSPEC_HT(select * FROM SAJET.M_PKSPEC where id= " + imes.id+ " and PKSPEC_NAME='"+ pkspecName + "'";
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            return 1;
        }

        public int MntnPackSpecUpdate(ImesMpkspec imes)
        {
            var id = imes.id;
            string site = imes.site;
            string pkspecName = imes.pkspecName;
            string insertHt = $"insert INTO SAJET.M_PKSPEC_HT(select * FROM SAJET.M_PKSPEC where id= " + imes.id + " and PKSPEC_NAME='" + pkspecName + "'";
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");

            imes.updateTime = DateTime.Now;
            return Context.Updateable(imes).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.pkspecName, it.site }).ExecuteCommand();
        }

        public int MntnPackSpecDelet(ImesMpkspec imes)
        {
            string site = imes.site;
            string pkspecName = imes.pkspecName;
            imes.updateTime = DateTime.Now;
            string insertHt = $"insert INTO SAJET.M_PKSPEC_HT(select * FROM SAJET.M_PKSPEC where id= " + imes.id + " and PKSPEC_NAME='" + pkspecName + "'";
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");

            int det = Context.Deleteable<ImesMpkspec>().Where(it => it.id == imes.id && it.pkspecName == pkspecName).ExecuteCommand();
            return det;
        }

        public object MntnPackSpeclistHt(string pkspecName, string id, string site)
        {
            return Context.Ado.SqlQuery<object>($"select * FROM SAJET.M_PKSPEC_HT where PKSPEC_NAME ='" + pkspecName + "' and ID ='" + id + "' and site = '" + site + "'");
        }

    }
}
