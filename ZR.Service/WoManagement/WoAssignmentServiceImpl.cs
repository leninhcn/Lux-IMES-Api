using Aliyun.OSS;
using Infrastructure;
using Infrastructure.Attribute;
using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Dto.Machine;
using ZR.Model.Dto.ProdDto;
using ZR.Model.Dto.Tooling;
using ZR.Model.Dto.WorkOrder;
using ZR.Service.Business.IBusinessService;
using ZR.Service.WoManagement.IService;

namespace ZR.Service.WoManagement
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IWoAssignmentService), ServiceLifetime = LifeTime.Transient)]
    public class WoAssignmentServiceImpl : BaseService<MWoAssignment>, IWoAssignmentService
    {

        public List<WoAssignmentDto> GetList(string site , DateTime WoCreateDateStart, DateTime WoCreateDateEnd)
        {
            string filter = string.Format("A.SITE = '{0}' AND A.STATION_TYPE IS NOT NULL AND A.WO_STATUS='2' AND A.WO_CREATE_DATE between to_date('{1}','yyyy-mm-dd') and to_date('{2}','yyyy-mm-dd')  ", site, WoCreateDateStart.ToString("yyyy-MM-dd"), WoCreateDateEnd.ToString("yyyy-MM-dd"));
            var exp = Context.Queryable<WoBase, MStationType, MPart,SProductLevel>((a, b, c, d) => new JoinQueryInfos(
                JoinType.Left, (a.StationType == b.StationType && a.Site == b.Site),
                JoinType.Left, (a.Ipn == c.Ipn && a.Site == c.Site),
                JoinType.Left, d.ProdCode == c.Progrp ))
                .Select((a, b, c, d) => new WoAssignmentDto
                {
                    WorkOrder = a.WorkOrder,
                    Ipn = a.Ipn,
                    StationType = a.StationType,
                    WorkShop = a.WorkShop,
                    StationTypeDesc = b.StationTypeDesc,
                    WoType = a.WoType,
                    WoScheduleStartDate = a.WoScheduleStartDate,
                    WoScheduleCloseDate = a.WoScheduleCloseDate,
                    WoAssingStartDate = a.WoAssingStartDate,
                    WoAssingEndDate = a.WoAssingEndDate,
                    AssignStatus = a.Assignstatus,
                    TopWorkOrder = a.TopWorkOrder,
                    Spec2 = c.SPEC2,
                    CustomerCode = a.CustomerCode,
                    SoText = a.SoText,
                    OrderNo = a.OrderNo,
                    SendAddress = a.SendAddress,
                    Progrp = c.Progrp,
                    ProdName = d.ProdName
                })
                .Where(filter).ToList();


            return exp;
        }


        public ExecuteResult CheckPKG( string wo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("t_result", null, true);

                Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_WO_KITTING", 
                    new SugarParameter[]
                    {
                        new SugarParameter("t_checktype", "JobPkgCheckSwitch"),
                        new SugarParameter("t_workorder", wo),
                        tRes
                     });
                string sRes = tRes.Value.ToString();
                if (sRes != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = sRes + "派工SAP齐套检查提示";
                }
                else
                {
                    exeRes.Status = true;
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;

        }

        public bool ExistedWorkproc(string StationType)
        {
            string sqlStr = " SELECT ID,NAME FROM SAJET.M_MACHINE_GROUP "
                     + $" WHERE ENABLED='Y' AND STATION_TYPE = '{StationType}' ";
           DataTable dt =  Context.Ado.GetDataTable(sqlStr);
            if (dt.Rows.Count > 0 )
            {
                return true;
            }
            return false;
        }

        public ExecuteResult CheckReleaseWo(string wo, string site)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_WO_RELEASE",
                    new SugarParameter[]
                    {
                        new SugarParameter("T_WO", wo),
                        new SugarParameter("T_STATUS", ""),
                        tRes,
                        new SugarParameter("T_PANET", "N/A"),
                        new SugarParameter("T_SITE", site)
                     });
                string sRes = tRes.Value.ToString();
                if (sRes != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = sRes ;
                }
                else
                {
                    exeRes.Status = true;
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult CheckDrawCardVersion(string wo, string site)
        {
            ExecuteResult exeRes = new ExecuteResult();
            var sSQL = "";
            var dtTemp = new DataTable();
            try
            {
                sSQL = $"select WORK_ORDER,IPN,DRAWNUM,DRAWNUM_VERSION,TOP_WORK_ORDER  FROM SAJET.p_wo_base where WORK_ORDER='{wo}'";
                dtTemp = Context.Ado.GetDataTable(sSQL);
                var exists = dtTemp.Rows[0]["DRAWNUM_VERSION"] != null
                && dtTemp.Rows[0]["DRAWNUM_VERSION"] != DBNull.Value
                && !string.IsNullOrWhiteSpace(dtTemp.Rows[0]["DRAWNUM_VERSION"].ToString());
                if (exists)
                {
                    exeRes.Status = true;
                    return exeRes;
                }

                //改为从视图中获取
                sSQL = $"select DR.DRAWNUM,DR.DRAW_VER,DR.TOP_WORK_ORDER FROM SAJET.V_WORKORDER_DRAWCARD DR" +
                    $" where WORK_ORDER='{wo}' and DRAWNUM is not null and DRAW_VER is not null";
                dtTemp = Context.Ado.GetDataTable(sSQL);
                if (dtTemp.Rows.Count > 0)
                {
                    var drawnum = dtTemp.Rows[0]["DRAWNUM"].ToString();
                    var drawnumver = dtTemp.Rows[0]["DRAW_VER"].ToString();
                    var top_wo = $"{dtTemp.Rows[0]["TOP_WORK_ORDER"]}";
                    if (!string.IsNullOrEmpty(top_wo))
                    {
                        sSQL = $"update SAJET.p_wo_base set DRAWNUM_VERSION='{drawnumver}',DRAWNUM='{drawnum}' where TOP_WORK_ORDER='{top_wo}'";
                    }
                    else
                    {
                        sSQL = $"update SAJET.p_wo_base set DRAWNUM_VERSION='{drawnumver}',DRAWNUM='{drawnum}' where WORK_ORDER='{wo}'";
                    }
                    dtTemp = Context.Ado.GetDataTable(sSQL);

                    exeRes.Status = true;
                    return exeRes;
                }
                else
                {
                    sSQL = $@"select w.WORK_ORDER,nvl(nvl(w.DRAWNUM,g.DRAWNUM),i.DRAWNUM) as DRAWNUM,
nvl(nvl(w.DRAWNUM_VERSION,g.DRAW_VER),i.DRAW_VER) as DRAW_VER,
w.TOP_WORK_ORDER,w.IPN,w.ASSIGNSTATUS
,nvl(g.ENABLED,i.ENABLED) as ENABLED
,nvl(g.STATUS1,i.STATUS1) as STATUS1
FROM SAJET.P_WO_BASE w 
left join IMES.P_WO_BASE wt on w.TOP_WORK_ORDER = wt.WORK_ORDER
left join (
    select ANCESTOR,DRAWNUM,DRAW_VER,row_number() over(partition by ANCESTOR order by DRAW_VER desc) as RID ,ENABLED, STATUS1
    FROM SAJET.M_DRAWCARD where  DR='0' and ANCESTOR like '%-XXXX'
) g on w.DRAWNUM =g.DRAWNUM and g.RID=1
left join (
    select ANCESTOR,DRAWNUM,DRAW_VER,row_number() over(partition by ANCESTOR order by DRAW_VER desc) as RID ,ENABLED, STATUS1
    FROM SAJET.M_DRAWCARD where DR='0' and ANCESTOR like '%-XXXX'
) i on nvl(wt.IPN,w.IPN) like replace(i.ANCESTOR,'-XXXX','-')||'%' and i.RID=1
where w.WORK_ORDER='{wo}'";
                    dtTemp = Context.Ado.GetDataTable(sSQL);
                    if (dtTemp.Rows.Count > 0
                        && !string.IsNullOrEmpty((string)dtTemp.Rows[0]["DRAWNUM"])
                        && !string.IsNullOrEmpty((string)dtTemp.Rows[0]["DRAW_VER"])
                        && (((string)dtTemp.Rows[0]["ENABLED"]).Equals("N", StringComparison.OrdinalIgnoreCase)
                            || ((string)dtTemp.Rows[0]["STATUS1"]).Equals("N", StringComparison.OrdinalIgnoreCase)
                        ))
                    {
                        dtTemp = Context.Ado.GetDataTable(sSQL);
                        exeRes.Message += $"工单[{wo}]设计卡版本已失效！{Environment.NewLine}{Environment.NewLine} 请生效设计卡: {dtTemp.Rows[0]["DRAWNUM"]} 版本: {dtTemp.Rows[0]["DRAW_VER"]}{Environment.NewLine}";

                    }
                    else
                    {
                        exeRes.Message += $"工单[{wo}]未找到设计卡！请检查：{Environment.NewLine}{Environment.NewLine}" +
                            $" 1. SAP成品工单是否已推送致MES{Environment.NewLine}" +
                            $" 2. 工单是否有填写设计卡号{Environment.NewLine}" +
                            $" 3. 是否未建立原型设计卡{Environment.NewLine}";
                    }
                    exeRes.Status = false;

                }

            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public ExecuteResult PackCheck(string wo, string site)
        {
            ExecuteResult exeRes = new ExecuteResult();
            var dtTemp = new DataTable();
            try
            {
                exeRes.Status = false;
                var exp = Expressionable.Create<WoBase>();
                exp.AndIF(site != "" && site != null, it => it.Site == site);
                exp.And(it => it.WorkOrder == wo);
                var wk = Context.Queryable<WoBase>().Where(exp.ToExpression()).First();

                if (wk == null)
                {
                    exeRes.Message += $"[{wo}]没有找到工单信息";
                    return exeRes;
                }
                if (!string.IsNullOrWhiteSpace(wk.Ipn)
                    && (wk.Ipn.EndsWith("ZZ") && (wk.FixedLength <= 0  || wk.FixedUnit == null  || wk.FixedUnit == "") ))
                {
                    exeRes.Message += $"产品编辑以ZZ结尾的工单[{wo}]的定长和定长单位不能为空!{Environment.NewLine}请检查SAP工单的定长和定长单位设置！{Environment.NewLine}";
                    return exeRes;
                }

                if (isDaoZhou(wk.StationType))
                {
                    string sql = "select * FROM SAJET.V_WO_ROLLINFO WHERE WORK_ORDER='" + wo + "'";
                    DataTable dt = Context.Ado.GetDataTable(sql);
                    if (dt.Rows.Count == 0)
                    { // 未设置包装规格
                        exeRes.Message += "工单[" + wo + "]未找到包装规格:" + Environment.NewLine + Environment.NewLine;
                        exeRes.Message += "    工厂型号: " + wk.Ipn + Environment.NewLine;
                        exeRes.Message += "    客户编码: " + wk.CustomerCode + Environment.NewLine;
                        exeRes.Message += "    客户型号: " + wk.Cuspart + Environment.NewLine;
                        exeRes.Message += "    送往地区: " + wk.SendAddress + Environment.NewLine;
                        exeRes.Message += "    工单备注: " + wk.Remark + Environment.NewLine + Environment.NewLine;
                        exeRes.Message += "  请检查是否有符合以上条件的包装资料。";
                        return exeRes;
                    }
                    if (wk.FixedLength <= 0 || string.IsNullOrEmpty(wk.FixedUnit))
                    {
                        var length = dt.Rows[0]["LENGTH"].ToString();
                        var unit = dt.Rows[0]["UNIT"].ToString();
                        var sSQL = $"update SAJET.p_wo_base set FIXED_LENGTH='{length}',FIXED_UNIT='{unit}' where WORK_ORDER='{wo}'  and site = '{site}'";
                        Context.Ado.SqlQuery<string>(sSQL);
                    }
                }
                exeRes.Status = true;

            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        bool isDaoZhou(string station_type)
        {
            if (station_type.Equals("倒轴") || station_type.Equals("包装") || station_type.Equals("打饼"))
                return true;
            return false;
        }

        public void ModifyAssignDate(int type, string wo, string machine, string empno, string assingStartDate, string assignEndDate)
        {
            //var sSQL = $"update SAJET.M_WOASSIGNMENT SET "
            //    + (type == 0 ? $" WO_ASSING_START_DATE=TO_DATE('{assingStartDate}','yyyy/mm/dd')" : $" WO_ASSING_END_DATE=TO_DATE('{assignEndDate}','yyyy/mm/dd')")
            //    + $" where WORK_ORDER='{wo}' and MACHINE='{machine}' and LASTUPDATE= SYSDATE AND LASTUPDATEUSER = '{empno}'";
            var sSQL = $"update SAJET.M_WOASSIGNMENT SET "
                + (type == 0 ? $" WO_ASSING_START_DATE=TO_DATE('{assingStartDate}','YYYY-MM-DD HH24:MI:SS')" : $" WO_ASSING_END_DATE=TO_DATE('{assignEndDate}','YYYY-MM-DD HH24:MI:SS')")
                + $", LASTUPDATE= SYSDATE  where WORK_ORDER='{wo}' and MACHINE='{machine}' ";
            var line = Context.Ado.SqlQuery<string>(sSQL);
        }

        public void ModifyWoBaseAssignDate(string wo,string site)
        {
            //派工/增派关闭画面更新wo的派工日期
            var sSQL = $"update SAJET.P_WO_BASE  "
                + $" SET(WO_ASSING_START_DATE, WO_ASSING_END_DATE) = (SELECT MIN(WO_ASSING_START_DATE) AS STARTDATE, MAX(WO_ASSING_END_DATE) AS ENDDATE FROM SAJET.M_WOASSIGNMENT WHERE WORK_ORDER = '{wo}')"
                + $" where WORK_ORDER='{wo}' and site = '{site}'";
            var line = Context.Ado.SqlQuery<string>(sSQL);
        }

        public ExecuteResult ModifyData(string xmldata)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_WOASSIGN",
                    new SugarParameter[]
                    {
                        new SugarParameter("DATA", xmldata),
                        tRes
                     });
                string sRes = tRes.Value.ToString();
                if (sRes != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = sRes;
                }
                else
                {
                    exeRes.Status = true;
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public MWoAssignment CehckAssignInfo(MWoAssignment mWoAssignment)
        {
            var exp = Expressionable.Create<MWoAssignment>();
            exp.And(it => it.WorkOrder == mWoAssignment.WorkOrder);
            exp.And(it => it.Machine == mWoAssignment.Machine);

            return Context.Queryable<MWoAssignment>().Where(exp.ToExpression()).First();
        }

        public DataTable GetWoInfo(string wo, string site)
        {
            string sqlStr = $@"  SELECT A.*,B.STATION_TYPE_DESC FROM SAJET.P_WO_BASE A left join IMES.M_STATION_TYPE B ON A.STATION_TYPE=B.STATION_TYPE WHERE A.WORK_ORDER = '{wo}' and a.site = '{site}'";
            return Context.Ado.GetDataTable(sqlStr);
        }

        public ExecuteResult CheckOutputQMS(string stationtype)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_OUTPUTTOQMS",
                    new SugarParameter[]
                    {
                        new SugarParameter("T_STATION_TYPE", stationtype),
                        tRes
                     });
                string sRes = tRes.Value.ToString();
                if (sRes != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = sRes;
                }
                else
                {
                    exeRes.Status = true;
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public MApiConfig GetApiConfig(string apicode)
        {
            var exp = Expressionable.Create<MApiConfig>();
            exp.And(it => it.ApiCode == apicode);
            return Context.Queryable<MApiConfig>().Where(exp.ToExpression()).First();
        }

        public ExecuteResult SendQms(string wo,string site,string empno)
        {
            ExecuteResult exeResult = new ExecuteResult();

            var wotable = GetWoInfo(wo, site);

            string stationtype = wotable.Rows[0]["STATION_TYPE"].ToString();
            string stationtype_code = wotable.Rows[0]["STATION_TYPE_DESC"].ToString();

            ExecuteResult execute = CheckOutputQMS(stationtype);
            if (!execute.Status)
            {
                return execute;
            }
            ProductToQms vo = new ProductToQms();

            vo.Werks = wotable.Rows[0]["Plant_Code"].ToString();
            vo.LotNO = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            vo.ProdLine = "SJ,首检";
            vo.PlanProdDate = ((DateTime)wotable.Rows[0]["WO_SCHEDULE_START_DATE"]).ToString("yyyy-MM-dd");
            vo.WorkOrder = wo;
            vo.MaterialNo = wotable.Rows[0]["IPN"].ToString();
            vo.Line = wotable.Rows[0]["LINE"].ToString();
            decimal planProqty = 0;
            decimal.TryParse(wotable.Rows[0]["TARGET_QTY"].ToString(), out planProqty);
            vo.PlanProdQty = planProqty;
            vo.DRAWNUM = wotable.Rows[0]["DRAWNUM"].ToString();
            vo.DRAWNUM_VERSION = wotable.Rows[0]["DRAWNUM_VERSION"].ToString();
            vo.Zqty = "0";

            var apiconfig = GetApiConfig("PROOUTTOQMSCALLBACK");
            if (apiconfig != null)
            {
                vo.CallbackPostAPI = apiconfig.ApiUrl;
            }
            vo.MODIFYBY = empno;
            vo.MODIFYTIME = DateTime.Now.ToString("yyyy-MM-dd mm:HH:ss");

            ExecuteResult callQMS = HttpPostSendQMS("PROOUTTOQMS", vo);
            if (!callQMS.Status)
            {
                return callQMS;
            }
            exeResult.Status = true;
            return exeResult;
        }

        public ExecuteResult HttpPostSendQMS(string apicode, ProductToQms json)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sql = $"select * FROM SAJET.M_APICONFIG WHERE API_CODE='{apicode}'";
                DataTable dataTable = Context.Ado.GetDataTable(sql);
                if (dataTable.Rows.Count > 0)
                {
                    exeRes.Anything = dataTable;// snlist;
                    exeRes.Message = "查询API配置OK,";
                    string url = dataTable.Rows[0]["API_URL"].ToString();
                    string data = JsonConvert.SerializeObject(json);
                    string result = "";
                    string BRES = "1";
                    try
                    {
                        result = HttpHelper.HttpPost(url, data);
                        ProductToQmsResult revo = JsonConvert.DeserializeObject<ProductToQmsResult>(result);

                        if (revo.Code.Equals("S"))
                        {
                            exeRes.Message = exeRes.Message + "执行API:" + revo.Message;
                            exeRes.Status = true;
                            BRES = "1";
                        }
                        else
                        {
                            exeRes.Message = exeRes.Message + "执行API:" + revo.Message; ;
                            exeRes.Status = false;
                            BRES = "0";
                        }
                    }
                    catch (Exception ee)
                    {
                        result = ee.Message;
                        BRES = "0";
                        exeRes.Status = false;
                    }


                    long id = (long)Math.Floor((new Random()).NextDouble() * 100000000000000D);
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("insert INTO SAJET.M_DATASYNC_LOG(API_URL,BRES,ID,JSON,OTYPE,RESULT,SEND_TIME,TABLE_NAME)").Append
                        (" values('").Append(url).Append("','").Append(BRES).Append("',").Append(id).Append(",'").Append(data).Append("','PROOUTTOQMS").Append("','")
                        .Append(result.Replace("'", "''")).Append("',CURRENT_DATE,'").Append(apicode).Append("')");
                    long sqlresult = Context.Ado.SqlQuery<string>(stringBuilder.ToString()).Count();

                    if (sqlresult > 0)
                    {
                        exeRes.Message = exeRes.Message + "增加LOG:OK";
                        exeRes.Status = true;
                    }
                    else
                    {
                        exeRes.Message = exeRes.Message + "增加LOG:FAIL";
                        exeRes.Status = false;
                    }
                }
                else
                {
                    exeRes.Message = "未配置推送QMS的地址";
                    exeRes.Status = false;
                    return exeRes;
                }
            }
            catch (Exception ee)
            {
                exeRes.Message = ee.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public List<MachineGroupToWoAssign> GetMachineGroupToWoAssign(string stationType, string site)
        {
            var exp = Expressionable.Create<MMachineGroup>();
            exp.And(it => it.StationType == stationType);
            exp.And(it => it.Site == site);
            exp.And(it => it.Enabled == "Y");
            var res = Context.Queryable<MMachineGroup>().Where(exp.ToExpression())
                .Select(it => new MachineGroupToWoAssign
                {
                    machineGroupId = it.Id,
                    machineGroupName = it.Name,
                    description = it.Description,
                    stationType = stationType
                }).ToList();
             return res;
        }

        public List<MachineToWoAssign> GetMachineToWoAssign(string machineGroup, string site)
        {
            var res = Context.Queryable<MMachine,MMachineGroup>((a,b) => new JoinQueryInfos(
                JoinType.Left,a.GroupId == b.Id
                ))
                .Select((a, b) => new MachineToWoAssign
                {
                    id = a.Id,
                    machine = a.MachineCode,
                    stationType = b.StationType,
                    machineGroupId = a.GroupId,
                    machineGroupName = b.Name
                }).Where($"A.ENABLED='Y' AND B.NAME='{machineGroup}' and a.site = '{site}'").ToList();
            
            return res;
        }

        public DataTable GetWoAssignment(string wo)
        {
            string sqlStr = " SELECT * FROM SAJET.M_WOASSIGNMENT "
                     + $" WHERE WORK_ORDER='{wo}' ";
            return Context.Ado.GetDataTable(sqlStr);
        }

        public DataTable getMachine(string[] groups)
        {
            var sSQL = "";
            var dtTemp = new DataTable();
            try
            {
                sSQL = " SELECT B.ID,B.NAME,B.DESCRIPTION,MACHINE_CODE MACHINE,C.CREATETIME OPTIME,C.WO_ASSING_START_DATE,C.WO_ASSING_END_DATE,C.WORK_ORDER  FROM SAJET.M_MACHINE A  "
                     + " LEFT JOIN IMES.M_MACHINE_GROUP B ON A.GROUP_ID=B.ID "
                     + " LEFT JOIN IMES.M_WOASSIGNMENT C ON A.MACHINE_CODE=C.MACHINE"
                     + $" WHERE A.ENABLED='Y' AND B.NAME IN({string.Join(",", groups.Select(g => "'" + g + "'").ToArray())})";


                dtTemp = Context.Ado.GetDataTable(sSQL);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return dtTemp;
        }

        public DataTable getMachine(string[] machines, string workproc, string workorder = null)
        {
            var sSQL = "";
            var dtTemp = new DataTable();
            try
            {
                sSQL = " SELECT B.ID,B.NAME,B.DESCRIPTION,MACHINE_CODE MACHINE,C.CREATETIME OPTIME ,C.WO_ASSING_START_DATE,C.WO_ASSING_END_DATE,C.WORK_ORDER FROM SAJET.M_MACHINE A  "
                     + " LEFT JOIN IMES.M_MACHINE_GROUP B ON A.GROUP_ID=B.ID "
                     + " LEFT JOIN IMES.M_WOASSIGNMENT C ON A.MACHINE_CODE=C.MACHINE"
                     + $" WHERE A.ENABLED='Y' AND A.MACHINE_CODE IN({string.Join(",", machines.Select(m => "'" + m + "'").ToArray())})";

                if (!string.IsNullOrEmpty(workorder))
                {
                    sSQL += $" and C.WORK_ORDER in ('{workorder}')";
                }

                dtTemp = Context.Ado.GetDataTable(sSQL);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return dtTemp;
        }

        public DataTable getAssignment(string wo)
        {
            var dt = new DataTable();
            var sSQL = " SELECT * FROM SAJET.M_WOASSIGNMENT "
                     + $" WHERE WORK_ORDER='{wo}' ";
            try
            {
                dt = Context.Ado.GetDataTable(sSQL);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
