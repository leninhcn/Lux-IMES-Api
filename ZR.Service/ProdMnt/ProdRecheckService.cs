using Infrastructure.Attribute;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZR.Common;
using ZR.Model.Business;
using ZR.Service.IService;
using static System.Collections.Specialized.BitVector32;

namespace ZR.Service
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class ProdRecheckService : BaseService<SnStatus>, IProdRecheckService
    {
        public async Task<List<string>> GetHtValues()
        {
            return await Context.Queryable<MStationType>()
                .Where(x => x.Enabled == "Y" && x.StationType.StartsWith("QF"))
                .OrderBy(x=> x.UpdateTime, OrderByType.Desc)
                .Select(x => x.StationType).ToListAsync();
        }

        public async Task<DataTable> GetSnDetail(string sn, string type)
        {
            if(type == "KP")
            {
                return await Context.Queryable<SnStatus, PSnKeyparts>((a, b) => a.SerialNumber == b.SerialNumber)
                    .Where((a, b) =>  b.ItemSn == sn)
                    .Select((a, b) => new { a.Ipn, a.WorkOrder }).ToDataTableAsync();
            }
            else if(type == "CSN")
            {
                return await Context.Queryable<SnStatus>()
                    .Where(a => a.CustomerSn == sn)
                    .Select(a => new { a.Ipn, a.WorkOrder }).ToDataTableAsync();
            }
            else
            {
                return await Context.Queryable<SnStatus>()
                    .Where(a => a.SerialNumber == sn)
                    .Select(a => new { a.Ipn, a.WorkOrder }).ToDataTableAsync();
            }
        }

        public async Task<string> CheckLogic(string sn, string type, string qcInStationType, string qcOutStationType, string uid)
        {
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_OFFLINE_QC",
                new SugarParameter[]
                {
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_TYPE", type),
                    new SugarParameter("T_EMP", uid),
                    new SugarParameter("T_QC_IN_STATION_TYPE", qcInStationType),
                    new SugarParameter("T_QC_OUT_STATION_TYPE", qcOutStationType),
                    tRes
                });

            return tRes.Value.ToString();
        }
    }
}
