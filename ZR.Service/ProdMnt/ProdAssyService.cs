using Infrastructure.Attribute;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Service.IService;

namespace ZR.Service
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class ProdAssyService : BaseService<SnStatus>, IProdAssyService
    {
        public async Task<List<StationAction>> GetStationActionList(StationInfoDto stationInfo)
        {

            var r = 
            await Context.Queryable<MStationAction, MActionGroupBase, MActionGroupLink, MActionJobBase, MActionJobLink, MActionJobTypeBase>((a, b, c, d, e, f) => new JoinQueryInfos(
                JoinType.Inner, a.GroupId == b.GroupId,
                JoinType.Inner, a.GroupId == c.GroupId,
                JoinType.Inner, c.JobId == d.JobId,
                JoinType.Left, d.JobId == e.JobId,
                JoinType.Inner, d.TypeId == f.TypeId))
                .Where(a => a.StationName == stationInfo.StationName)
                .OrderBy((a, b, c, d, e, f) => c.GroupSeq)
                .OrderBy((a, b, c, d, e, f) => e.JobSeq)
                .Select((a, b, c, d, e, f) => new StationAction
                {
                    GroupId = c.GroupId.ToString(),
                    GroupName = b.GroupName,
                    Step = (int)c.GroupSeq,
                    JobSeq = (int)e.JobSeq,
                    ValueKind = c.ValueKind.ToString(),
                    ValueTransFormation = c.ValueTransformation.ToString(),
                    LoopCount = (int)c.LoopCount,
                    JobName = d.JobName,
                    JobLogicProSql = e.LogicProsql,
                    LogicType = e.LogicType,
                    InputParam = e.InputParam,
                    OutputParam = e.OutputParam,
                    TypeName = f.TypeName,
                    TypeDesc = f.TypeDesc,
                    TypeProcParam = f.ProcParam,
                    ShowBom = a.ShowBom == "Y" ? true : false,
                    CheckLine = a.CheckLine == "Y" ? true : false,
                    PrintFlag = a.PrintFlag == "Y" ? true : false,
                    AutoReadSn = a.AutoReadsn == "Y" ? true : false,
                    AutoReadPath = a.AutoReadPath,
                    CheckFont = a.CheckFont == "Y" ? true : false,
                    PrintQty = int.Parse(string.IsNullOrEmpty(a.PrintQty) ? "0" : a.PrintQty),
                }).ToListAsync();

            if (r.Count > 0) return r;

            r =
            await Context.Queryable<MStationAction, MActionGroupBase, MActionGroupLink, MActionJobBase, MActionJobLink, MActionJobTypeBase>((a, b, c, d, e, f) => new JoinQueryInfos(
                JoinType.Inner, a.GroupId == b.GroupId,
                JoinType.Inner, a.GroupId == c.GroupId,
                JoinType.Inner, c.JobId == d.JobId,
                JoinType.Left, d.JobId == e.JobId,
                JoinType.Inner, d.TypeId == f.TypeId))
                .Where(a => a.StationType == stationInfo.StationType && a.StationName == "0")
                .OrderBy((a, b, c, d, e, f) => c.GroupSeq)
                .OrderBy((a, b, c, d, e, f) => e.JobSeq)
                .Select((a, b, c, d, e, f) => new StationAction
                {
                    GroupId = c.GroupId.ToString(),
                    GroupName = b.GroupName,
                    Step = (int)c.GroupSeq,
                    JobSeq = (int)e.JobSeq,
                    ValueKind = c.ValueKind.ToString(),
                    ValueTransFormation = c.ValueTransformation.ToString(),
                    LoopCount = (int)c.LoopCount,
                    JobName = d.JobName,
                    JobLogicProSql = e.LogicProsql,
                    LogicType = e.LogicType,
                    InputParam = e.InputParam,
                    OutputParam = e.OutputParam,
                    TypeName = f.TypeName,
                    TypeDesc = f.TypeDesc,
                    TypeProcParam = f.ProcParam,
                    ShowBom = a.ShowBom == "Y" ? true : false,
                    CheckLine = a.CheckLine == "Y" ? true : false,
                    PrintFlag = a.PrintFlag == "Y" ? true : false,
                    AutoReadSn = a.AutoReadsn == "Y" ? true : false,
                    AutoReadPath = a.AutoReadPath,
                    CheckFont = a.CheckFont == "Y" ? true : false,
                    PrintQty = int.Parse(string.IsNullOrEmpty(a.PrintQty) ? "0" : a.PrintQty),
                }).ToListAsync();

            return r;
        }

        public async Task<bool> CheckIsLotAssy(string lineName, string stationType)
        {
            var any = await Context.Queryable<MBlockConfigType, MBlockConfigValue>((a, b) => a.ConfigTypeId == b.ConfigTypeId)
                .Where((a, b) => a.ConfigTypeName == "Bolck_IsLotAssy"
                && b.Line == lineName
                && a.Enabled == "Y"
                && b.Enabled == "Y")
                .Select((a, b) => b.ConfigName)
                .AnyAsync();

            if(any) { return true; }

            any = await Context.Queryable<MBlockConfigType, MBlockConfigValue>((a, b) => a.ConfigTypeId == b.ConfigTypeId)
                .Where((a, b) => a.ConfigTypeName == "Bolck_IsLotAssy"
                && b.StationType == stationType
                && a.Enabled == "Y"
                && b.Enabled == "Y")
                .Select((a, b) => b.ConfigName)
                .AnyAsync();

            return any;
        }

        public async Task<(string, string)> GetWoList(string station, string cmd)
        {
            var oMsg = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_GETWOList",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_CMD", cmd),
                    oMsg, tRes
                });

            return (oMsg.Value.ToString(), tRes.Value.ToString());
        }

        public async Task<(string, string)> GetMainSn(string input, string station)
        {
            var oMainSn = new SugarParameter("O_MAINSN", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_GET_MAIN_SN",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_SN", input),
                    oMainSn, tRes
                });

            return (oMainSn.Value.ToString(), tRes.Value.ToString());
        }

        public async Task<SnInfo> GetSnInfoBySn(string mainSn)
        {
            return await Context.Queryable<SnStatus, MPart>((a, b) => a.Ipn == b.Ipn)
                .Where((a, b) => a.SerialNumber == mainSn)
                .Select((a, b) => new SnInfo
                {
                    MainSN = a.SerialNumber,
                    WorkOrder = a.WorkOrder,
                    PartNo = b.Ipn,
                    Line = a.Line,
                }).SingleAsync();
        }

        public async Task<(string, string)> CheckWo(string wo, string station)
        {
            var oMsg = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_CHK_WO",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    oMsg, tRes
                });

            return (oMsg.Value.ToString(), tRes.Value.ToString());
        }

        public async Task<WoInfo> GetWoInfoByWo(string wo)
        {
            return await Context.Queryable<WoBase, MPart>((a, b) => a.Ipn == b.Ipn)
                .Where((a, b) => a.WorkOrder == wo)
                .Select((a, b) => new WoInfo
                {
                    WorkOrder = a.WorkOrder,
                    PartNo = b.Ipn,
                    Line = a.Line,
                })
                .SingleAsync();
        }

        public async Task<List<BomInfo>> GetBomInfosByWO(string stationType, string wo)
        {
            return await Context.Queryable<MPart, PartSpecErpMesMapping, StationtypePartSpec, SnFeature, WoBase, WoBom>((a, b, c, d, aa, bb) =>
                a.SPEC1 == b.ErpSpec
                && b.MesSpec == c.KpSpec
                && c.KpSpec == d.MesSpec
                && d.Ipn == a.Ipn
                && aa.WorkOrder == bb.WorkOrder
                && bb.ItemIpn == a.Ipn)
            .Where((a, b, c, d, aa, bb) => c.StationType == stationType
                && b.Enabled == "Y"
                && c.Enabled == "Y"
                && d.Enabled == "Y"
                && aa.WorkOrder == wo
                && bb.ItemCount != 0)
            .Select((a, b, c, d, aa, bb) => new BomInfo
            {
                ItemCount = bb.ItemCount,
                ItemGroup = bb.ItemGroup,
                ItemPartCode = d.Feature,
                ItemPartDesc = b.ErpSpec,
                ItemPartNo = a.Ipn,
                ItemPartType = d.PartType,
                MainPartNo = bb.Ipn,
                Slot = bb.Slot,
                ItemVersion = bb.ItemVersion,
                ItemMpn = a.Mpn,
            })
            .ToListAsync();                
        }

        public async Task<List<dynamic>> GetModel(string mainSn)
        {
            string sql = @"SELECT B.MODEL FROM IMES.P_SN_STATUS A,IMES.M_PART B,(select distinct MODEL from (
              select regexp_substr(q.CONFIG_VALUE, '[^,]+', 1, ROWNUM) MODEL, CONFIG_NAME
              from IMES.M_BLOCK_CONFIG_VALUE q
               connect by ROWNUM <= LENGTH(q.CONFIG_VALUE) - LENGTH(REGEXP_REPLACE(q.CONFIG_VALUE, ',', '')) + 1)
                  WHERE CONFIG_NAME = 'IsCheckSN' order by MODEL) V
                    WHERE A.IPN = B.IPN AND A.SERIAL_NUMBER = @sn
                        and B.MODEL = V.MODEL(+)";

            return await Context.Ado.SqlQueryAsync<dynamic>(sql, new List<SugarParameter> { new SugarParameter("@sn", mainSn) });
        }

        public async Task<string> CheckMachine(string station, string wo, string sn, string machine, string uid)
        {
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_CHKMACHINE",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_MACHINE", machine),
                    new SugarParameter("T_EMP", uid),
                    tRes
                });

            return (tRes.Value.ToString());
        }

        public async Task<(string, string)> CheckTooling(string station, string wo, string sn, string tool, string uid)
        {
            var oData = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_CHKTOOL",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_TOOL", tool),
                    new SugarParameter("T_EMP", uid),
                    oData, tRes
                });

            return (oData.Value.ToString(), tRes.Value.ToString());
        }

        public async Task<(string, string)> CheckCarrier(string station, string wo, string sn, string carrier, string uid)
        {
            var oData = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_CHKCARRIER",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_CARRIER", carrier),
                    new SugarParameter("T_EMP", uid),
                    oData, tRes
                });

            return (oData.Value.ToString(), tRes.Value.ToString());
        }

        public async Task<(string, string)> CheckSNInput1WO(string station, string wo, string sn, string uid)
        {
            var tRes1 = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_CHECK_ROUTE_1WO",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_EMP", uid),
                    tRes1
                });

            var resMsg = tRes1.Value.ToString();
            if(resMsg != "OK")
            {
                return (null,  resMsg);
            }

            var oData = new SugarParameter("O_MSG", null, true);
            var tRes2 = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_CHKSN",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_EMP", uid),
                    oData, tRes2
                });

            return (oData.Value.ToString(), tRes2.Value.ToString());
        }

        public async Task<bool> CheckIsErrorCode(string ecode)
        {
            return await Context.Queryable<MDefect>().Where(a => a.DefectCode == ecode && a.Enabled == "Y")
                .AnyAsync();
        }

        public async Task<(string, string)> CheckSnBefore(string station, string wo, string sn, string uid)
        {
            var oData = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_CHKBEFORE",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_EMP", uid),
                    oData, tRes
                });

            return (oData.Value.ToString(), tRes.Value.ToString());
        }

        public async Task<bool> CheckEssTest(string stationType, string ess)
        {
            var r = await Context.Queryable<MBlockConfigType, MBlockConfigValue>((a,b)=>a.ConfigTypeId == b.ConfigTypeId)
                .Where((a,b) => b.Enabled == "Y"
                && b.StationType == stationType 
                && b.ConfigValue == "ESSAlarm")
                .AnyAsync();

            if (r)
            {
                r = await Context.Queryable<SnTravel>().Where(a => 
                a.SerialNumber == ess 
                && a.StationType == "ESS" 
                && a.CurrentStatus == "0")
                .AnyAsync();
            }

            return r;
        }

        public async Task<string> CheckKpsnIputMPN(string mpn, string ipn)
        {
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_CHECK_IPUTMPN_RULE",
                new SugarParameter[]
                {
                    new SugarParameter("T_MPN", mpn),
                    new SugarParameter("T_ITEM_PART", ipn),
                    tRes
                });

            return tRes.Value.ToString();
        }

        public async Task<string> CheckPalos(string sn)
        {
            throw new NotImplementedException();
        }

        public async Task<(string, string)> CheckSnPass(string station, string wo, string sn, string uid)
        {
            var tRes1 = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_CHECK_ROUTE",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_EMP", uid),
                    tRes1
                });

            var msg = tRes1.Value.ToString();

            if(msg != "OK")
            {
                return (null, msg);
            }

            var oData = new SugarParameter("O_MSG", null, true);
            var tRes2 = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_CHKSN",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_EMP", uid),
                    oData, tRes2
                });

            return (oData.Value.ToString(), tRes2.Value.ToString());
        }

        public async Task<List<ItemInfo>> GetItemInfo(string sn)
        {
            return await Context.Queryable<PSnKeyparts>()
                .Where(a=> a.SerialNumber == sn).OrderBy(a=>a.ItemIpn)
                .Select(a=> new ItemInfo
                {
                    ItemIpn = a.ItemIpn,
                    ItemSn = a.ItemSn,
                    ItemGroup = a.ItemGroup,
                    MesSpec = a.MesSpec,
                    Slot = a.Slot,
                }).ToListAsync();
        }

        public async Task<List<BomInfo>> GetBomInfosBySN(string stationType, string sn)
        {
            string sql = @"select c.*,case when (item_count-count<item_count)  and (item_count-count!=0)  then 0
                       WHEN item_count-count=0 THEN 2
                       else 1 end status from (
                      select a.*,nvl(b.count,0) count from (
                            SELECT  DISTINCT AA.WORK_ORDER,
                                    BB.IPN , 
                                    A.IPN ITEM_IPN,
                                    A.SPEC1 ERP_SPEC,
                                    D.SN_FEATURE ,
                                    B.MES_SPEC,
                                    BB.ITEM_GROUP,
                                    D.PART_TYPE, 
                                    BB.ITEM_COUNT,
                                    BB.SLOT,
                                    BB.ITEM_VERSION,
                                    A.MPN
                                FROM 
                                    IMES.M_PART A,
                                    IMES.M_PART_SPEC_ERP_MES_MAPPING  B,
                                    IMES.M_STATIONTYPE_PARTSPEC C,
                                    IMES.M_SN_FEATURE D,    
                                    IMES.P_WO_BASE AA,
                                    IMES.P_WO_BOM BB,
                                    IMES.P_SN_STATUS CC
                                WHERE
                                    A.SPEC1 = B.ERP_SPEC 
                                AND B.MES_SPEC = C.KP_SPEC
                                AND C.KP_SPEC = D.MES_SPEC
                                AND D.IPN = A.IPN
                                AND C.STATION_TYPE = @stationType
                                AND B.ENABLED = 'Y'
                                AND C.ENABLED = 'Y'
                                AND D.ENABLED = 'Y'
                                AND AA.WORK_ORDER = BB.WORK_ORDER
                                AND AA.WORK_ORDER = CC.WORK_ORDER
                                AND CC.SERIAL_NUMBER = @sn
                                AND BB.ITEM_IPN = A.IPN    AND BB.ITEM_COUNT!='0') a
                                left join (                                
                                select item_ipn,nvl(count(*),0) count from imes.p_sn_keyparts where serial_number =@sn
                                group by item_ipn) b on A.item_ipn=b.item_ipn) c
                                order by status,ITEM_IPN ASC";
            var dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> {
                new SugarParameter("@stationType", stationType),
                new SugarParameter("@sn", sn),
            });

            if (dt.Rows.Count > 0)
            {
                var bomInfos = new List<BomInfo>(dt.Rows.Count);
                foreach (DataRow dataRow in dt.Rows)
                {
                    BomInfo bomInfo = new()
                    {
                        ItemCount = int.Parse(dataRow["ITEM_COUNT"].ToString()),
                        ItemGroup = dataRow["ITEM_GROUP"].ToString(),
                        ItemPartCode = dataRow["SN_FEATURE"].ToString(),
                        ItemPartDesc = dataRow["MES_SPEC"].ToString(),
                        ItemPartNo = dataRow["ITEM_IPN"].ToString(),
                        ItemPartType = dataRow["PART_TYPE"].ToString(),
                        MainPartNo = dataRow["IPN"].ToString(),
                        Slot = dataRow["SLOT"].ToString(),
                        ItemVersion = dataRow["ITEM_VERSION"].ToString(),
                        ItemMpn = dataRow["MPN"].ToString(),
                        IpnFinishCount = Convert.ToInt32(dataRow["COUNT"].ToString())
                    };

                    bomInfos.Add(bomInfo);
                }
            }

            return new List<BomInfo>(0);
        }

        public async Task<(string, string)> GetCompareImageFileName(string station, string wo, string sn, string uid)
        {
            var oData = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_GETPIC",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_EMP", uid),
                    new SugarParameter("T_CMD", string.Empty),
                    oData, tRes
                });

            return (oData.Value.ToString(), tRes.Value.ToString());
        }

        public async Task<int> GetPanelLinkQtyBySN(string sn)
        {
            var r = await Context.Queryable<SnStatus, MPart>((a, b) => a.Ipn == b.Ipn)
                .Where((a, b) => a.SerialNumber == sn)
                .Select((a, b) => new { option1 = b.OPTION1 })
                .SingleAsync();

            if (r is null) return 0;

            var sPanelQty = r.option1;
            if (Regex.IsMatch(sPanelQty, @"^\d*[.]?\d*$"))
            {
                //是无符号数字
                try
                {
                    int iPanelQty = int.Parse(sPanelQty);
                    if (iPanelQty > 0)
                    {
                        return iPanelQty;
                    }
                }
                catch { }
            }

            return 0;
        }

        public async Task<(string, string)> CheckSNInput2WO(string station, string wo, string sn, string uid)
        {
            var tRes1 = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_CHECK_ROUTE_2WO",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_EMP", uid),
                    tRes1
                });

            var msg = tRes1.Value.ToString();

            if (msg != "OK")
            {
                return (null, msg);
            }

            var oData = new SugarParameter("O_MSG", null, true);
            var tRes2 = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_CHKSN",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_EMP", uid),
                    oData, tRes2
                });

            return (oData.Value.ToString(), tRes2.Value.ToString());
        }

        public async Task<string> CheckSnPassPanel(string station, string wo, string sn, string uid)
        {
            var (_, msg) = await CheckSnPass(station, wo, sn, uid);
            if (msg != "OK") return msg;

            var r = await Context.Queryable<SnStatus>()
                .Where(a => a.SerialNumber == sn && a.PanelNo != "N/A" && a.PanelNo != null)
                .Select(a => new { panelNo = a.PanelNo })
                .SingleAsync();

            if (r is null) return "Serial Number Panel No Error";

            var panelNo = r.panelNo;

            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_CHECK_ROUTE",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_CHKTYPE", "0"),
                    new SugarParameter("T_DATATYPE", "PANEL"),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("TREV", panelNo),
                    new SugarParameter("T_EMP", uid),
                    tRes
                });

            return tRes.Value.ToString();
        }

        public async Task<string> CheckSnPassBundle(string station, string wo, string sn, string uid)
        {
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_CHECK_ROUTE",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_CHKTYPE", "0"),
                    new SugarParameter("T_DATATYPE", "BUNDLE"),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("TREV", sn),
                    new SugarParameter("T_EMP", uid),
                    tRes
                });

            return tRes.Value.ToString();
        }

        public async Task<(string, string)> CheckSNInputNoWo(string station, string wo, string input, string uid)
        {
            var tRes1 = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_CHECK_ROUTE_1WO_NOWO",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", input),
                    new SugarParameter("T_EMP", uid),
                    tRes1
                });

            var msg = tRes1.Value.ToString();

            if (msg != "OK") return (null, msg);

            var oData = new SugarParameter("O_MSG", null, true);
            var tRes2 = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_CHKSN",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", input),
                    new SugarParameter("T_EMP", uid),
                    oData, tRes2
                });

            return (oData.Value.ToString(), tRes2.Value.ToString());
        }

        public async Task<DataTable> GetHDDInfo(string sn)
        {
            var sql = "SELECT T.ITEM_IPN, T.ITEM_SN, T.ITEM_GROUP, T.MES_SPEC, T.SLOT FROM IMES.P_SN_KEYPARTS T WHERE T.SERIAL_NUMBER = @sn AND REGEXP_LIKE  (T.ITEM_IPN , '80322|8033|TEST' ) ORDER BY ITEM_IPN ";
            return await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> { new SugarParameter("@sn", sn) });
        }

        public async Task<bool> CheckFixedAssets(string sn)
        {
            return await Context.Queryable<PSnKeyparts>()
                .Where(a => a.SerialNumber == sn && a.ItemGroup == "FIXED_ASSETS")
                .AnyAsync();
        }

        public async Task<bool> RelieveLink(string sn)
        {
            try
            {
                var sql = @"insert into imes.p_sn_keyparts_ht select * from imes.p_sn_keyparts_ht where serial_number = @sn and item_group = 'FIXED_ASSETS'";
                await Context.Ado.ExecuteCommandAsync(sql, new List<SugarParameter> { new SugarParameter("@sn", sn) });

                sql = @"delete from imes.p_sn_keyparts where serial_number = @sn and item_group = 'FIXED_ASSETS'";

                await Context.Ado.ExecuteCommandAsync(sql, new List<SugarParameter> { new SugarParameter("@sn", sn) });
            }
            catch
            {
                return false;
            }            

            return true;
        }

        public async Task<DataTable> GetIPNAPNBySN(string sn)
        {
            var sql = @"select nvl(a.ipn,'null') ipn,nvl(a.apn,'null') apn from imes.m_part a, imes.p_sn_status b
                        where a.ipn = b.ipn
                        and b.serial_number = @sn";

            return await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> { new SugarParameter("@sn", sn) });
        }

        public ISugarQueryable<MBlockConfigType, MBlockConfigValue> GetRule()
        {
            return Context.Queryable<MBlockConfigType, MBlockConfigValue>((a, b) => a.ConfigTypeId == b.ConfigTypeId)
                .Where((a, b) => a.ConfigTypeName == "SN_PRINT_FIXED_ASSETS");
        }

        public async Task<string> CheckIPN(string sn)
        {
            var sql = @"select b.config_value from IMES.M_BLOCK_CONFIG_TYPE a,IMES.M_BLOCK_CONFIG_VALUE b where a.config_type_id = b.config_type_id and a.config_type_name = 'AL_FIXED_ASSETS_IPN'";
            var dt = await Context.Ado.GetDataTableAsync(sql);

            if (dt.Rows[0]["config_value"].ToString() != "")
            {
                sql = @"select * from IMES.P_SN_TRAVEL where serial_number = @sn";
                string[] arr = dt.Rows[0]["config_value"].ToString().Split(',');
                string where = "";
                for (int i = 0; i < arr.Length; i++)
                {
                    if (i == 0)
                    {
                        where += " ipn ='" + arr[i] + "' ";
                    }
                    else
                    {
                        where += " or ipn ='" + arr[i] + "' ";
                    }
                }
                sql += " and (" + where + ")";

                dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> { new SugarParameter("@sn", sn) });

                if (dt.Rows.Count > 0) return "OK";

                return "非PE指定IPN不允许过此站";
            }
            return "请联系PE维护允许绑定的IPN";
        }

        public async Task<DataTable> CheckALFixedAssetsIsExist(string sn)
        {
            try
            {
                var sql = @"select * from imes.m_al_fixed_assets where serial_number = @sn";
                return await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> { new SugarParameter("@sn", sn) });
            }
            catch
            {
                return null;
            }
        }

        public async Task<DataTable> GetTdId()
        {
            try
            {
                var sql = @"select * from (select * from imes.m_al_fixed_assets where is_used = 'N' order by id) where rownum = 1";
                return await Context.Ado.GetDataTableAsync(sql);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> InsertALLabelInfo(string tdId, string sn, string uid)
        {
            try
            {
                var sql = @"update imes.m_al_fixed_assets
                set serial_number=@sn, brand=@brand,model=@model,ali_model_code=@code,
                configuration=@conf,is_used='Y',update_time=sysdate,update_emp=@uid
                where td_id = @tdId";

                await Context.Ado.ExecuteCommandAsync(sql, new List<SugarParameter> {
                    new SugarParameter("@sn", sn),
                    new SugarParameter("@brand", null),
                    new SugarParameter("@model", null),
                    new SugarParameter("@code", null),
                    new SugarParameter("@conf", null),
                    new SugarParameter("@uid", uid),
                    new SugarParameter("@tdId", tdId),
                });

                sql = @"insert into imes.m_al_fixed_assets_ht select td_id,serial_number,BRAND,MODEL,ALI_MODEL_CODE,CONFIGURATION,OPTION_1,OPTION_2,OPTION_3,sysdate create_time,UPDATE_TIME,UPDATE_EMP,ID from imes.m_al_fixed_assets from imes.m_al_fixed_assets
                where td_id = @tdId";

                await Context.Ado.ExecuteCommandAsync(sql, new List<SugarParameter> {
                    new SugarParameter("@tdId", tdId),
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
