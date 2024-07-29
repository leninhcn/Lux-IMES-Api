using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;
using ZR.Model.Quality;
using ZR.Service.IService;

namespace ZR.Service
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class ProdOBAService : BaseService<SnStatus>, IProdOBAService
    {
        public ISugarQueryable<SnStatus> GetSnInfo(string sn)
        {
            return Context.Queryable<SnStatus>().Where(x => x.SerialNumber == sn);
        }

        public async Task<string> GetSnFromKpItem(string kpItem)
        {
            return await Context.Queryable<PSnKeyparts>().Where(x => x.ItemSn == kpItem)
                .Select(x => x.SerialNumber).SingleAsync();
        }

        public async Task<bool> InsertOBA(string sn, string errorMsg, string line, string stationType, string status, string uid)
        {
            var sql = @"insert into imes.P_OBA_ERRINFO (serial_number,errcode,line,stationname,trndate,empno,status) values (@1,@2,@3,@4,sysdate,@5,@6)";

            var affected = await Context.Ado.ExecuteCommandAsync(sql, new List<SugarParameter>
            {
                new SugarParameter("@1", sn),
                new SugarParameter("@2", errorMsg),
                new SugarParameter("@3", line),
                new SugarParameter("@4", stationType),
                new SugarParameter("@5", uid),
                new SugarParameter("@6", status),
            });

            return affected > 0;
        }
       
        public async Task<bool> InsertHold(string sn, string errorMsg, string uid)
        {
            var holdModel = new PHoldSn
            {
                Id = (await Context.Queryable<PHoldSn>().MaxAsync(x => x.Id)) + 1,
                Sn = sn,
                StationType = "*",
                HoldReason = $"{errorMsg},CALL CA CHECK!(OBA UNITS)",
                HoldEmpno = uid,
                HoldTime = DateTime.Now,
                CreateTime = DateTime.Now,
                Enabled = "Y",
                CreateEmpno = uid,
            };
            var affected = await Context.Insertable(holdModel)
                .ExecuteCommandAsync();

            return affected > 0;
        }
    }
}
