using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Service.IService;
using ZR.Model.Business;
using Infrastructure.Attribute;
using System.Data;
using System.Collections;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Routing;
using ZR.Common;

namespace ZR.Service
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class ProdInputService : BaseService<SnStatus>, IProdInputService
    {
        public async Task<string> GetStationDesc(string stationType)
        {
            return await Context.Queryable<MStationType>()
                .Where(a => a.StationtypeCustomer == stationType && a.Enabled == "Y")
                .Select(a => a.StationTypeDesc).FirstAsync();
        }

        public async Task<List<string>> GetLineWo(string line, string wo)
        {
            return await Context.Queryable<WoBase>()
                .Where(a => a.Line == line && new string[] { "2", "3" }.Contains(a.WoStatus) && a.WorkOrder.Contains(wo))
                .Select(a => a.WorkOrder).ToListAsync();
        }

        public ISugarQueryable<MOperationStep> GetOprateStep(string stationType)
        {
            return Context.Queryable<MOperationStep>()
                .Where(a => a.StationType == stationType && a.Enabled == "Y")
                .OrderBy(a => a.StepSeq);
        }

        public async Task<bool> CheckSNStatus(string sn)
        {
            return await Context.Queryable<SnStatus>().Where(a=>a.SerialNumber == sn).AnyAsync();
        }

        public async Task<(string, string)> GetValues(string inputValue, string item, string modelName)
        {
            var tValue = new SugarParameter("T_VALUES", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_QUERY_VALUE_GROUP",
                new SugarParameter[]
                {
                    new SugarParameter("T_INPUT_VALUE", inputValue),
                    new SugarParameter("T_ITEM_GROUP", item),
                    new SugarParameter("T_MODEL_NAME", modelName),
                    tValue, tRes
                });

            return (tValue.Value.ToString(), tRes.Value.ToString());
        }

        public async Task<(string, string)> GetPamSn(string wo)
        {
            var tSn = new SugarParameter("T_SN", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_GET_PAMSN",
                new SugarParameter[]
                {
                    new SugarParameter("T_WO", wo),
                    tSn, tRes
                });

            return (tSn.Value.ToString(), tRes.Value.ToString());
        }

        public async Task<(string, string)> CheckValue(string inputValue, string item, string modelName)
        {
            var tCheckValue = new SugarParameter("T_CHECK_VALUE", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_VALUE",
                new SugarParameter[]
                {
                    new SugarParameter("T_INPUT_VALUE", inputValue),
                    new SugarParameter("T_ITEM", item),
                    new SugarParameter("T_MODEL_NAME", modelName),
                    tCheckValue, tRes
                });

            return (tCheckValue.Value.ToString(), tRes.Value.ToString());
        }

        public async Task<List<PSnKeyparts>> GetSNLinkKpsnInfo(string stationType, string sn)
        {
            return await Context.Queryable<PSnKeyparts>()
                .Where(a => a.SerialNumber == sn && a.StationType == stationType)
                .ToListAsync();
        }

        public async Task<bool> ClearGetSN(string sn)
        {
            try
            {
                await Context.Updateable<SnStatus>().SetColumns(_ => new SnStatus
                {
                    OPTION1 = string.Empty,
                }).Where(a => a.SerialNumber == sn).ExecuteCommandAsync();
            }
            catch { return false; }

            return true;
        }

        public ISugarQueryable<SnStatus> GetWoSnStatus(string wo)
        {
            return Context.Queryable<SnStatus>().Where(a => a.WorkOrder == wo).OrderBy(a => a.OutStationtypeTime, OrderByType.Desc);
        }

        public ISugarQueryable<WoBase, StationtypePartSpec, WoBom, PartSpecErpMesMapping, SnFeature> GetWoBomKeyparts(string wo, string stationType)
        {
            return Context.Queryable<WoBase, StationtypePartSpec, WoBom, PartSpecErpMesMapping, SnFeature>((a, b, c, d, e) => 
            a.WorkOrder == c.WorkOrder
            && c.ItemSpec1 == d.ErpSpec
            && b.KpSpec == d.MesSpec
            && c.ItemIpn == e.Ipn)
                .Where((a, b, c, d, e) => 
                a.WorkOrder == wo
                && b.StationType == stationType
                && b.Enabled == "Y"
                && d.Enabled == "Y"
                && e.Enabled == "Y");
        }

        public async Task<int> GetStationLinkCount(string model, string stationType)
        {
            return await Context.Queryable<StationtypePartSpec>()
                .Where(a => a.Model == model && a.StationType == stationType && a.Enabled == "Y")
                .CountAsync();
        }

        public async Task<int> GetEmpWoPassCount(string station, string uid)
        {
            return await Context.Queryable<SnTravel>().Where(a =>
            SqlFunc.MappingColumn<string>("TO_CHAR(OUT_STATIONTYPE_TIME, 'yyyy-mm-dd')") == SqlFunc.MappingColumn<string>(
            "TO_CHAR(SYSDATE, 'yyyy-mm-dd')")
            && a.EmpNo == uid && a.StationName == station)
            .CountAsync();
        }

        public async Task<(string, string)> CheckKpsn(string sn, string wo, string kpsn, string station, string uid)
        {
            var tRes = new SugarParameter("TRES", null, true);
            var tItemIpn = new SugarParameter("V_ITEM_IPN", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_KPSN_BATCH",
                new SugarParameter[]
                {
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_KPSN", kpsn),
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_EMP", uid),
                    tRes, tItemIpn
                });

            return (tItemIpn.Value.ToString(), tRes.Value.ToString());
        }

        public async Task<bool> CheckStockOut(string stationType)
        {
            return await Context.Queryable<SBaseParam>()
                .Where(a => a.Program == "CheckStockOutStation" && a.ParamValue == stationType)
                .AnyAsync();
        }

        public async Task<string> CheckLogic(string sn, string wo, string kpsn, string model, string tool, string glue, string reel, string station, string route, string step, string errorCode, string uid)
        {
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_LOGIC",
                new SugarParameter[]
                {
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_KPSN", kpsn),
                    new SugarParameter("T_TOOL", tool),
                    new SugarParameter("T_GLUE", glue),
                    new SugarParameter("T_REEL", reel),
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_ROUTE", route),
                    new SugarParameter("T_EMP", uid),
                    new SugarParameter("T_STEP", step),
                    new SugarParameter("T_ERRORCODE", errorCode ?? "N/A"),
                    new SugarParameter("T_CARTON", string.Empty),
                    //new SugarParameter("T_MODEL", model),
                    new SugarParameter("T_MASTER", string.Empty),
                    tRes
                });

            return tRes.Value.ToString();
        }
    }
}
