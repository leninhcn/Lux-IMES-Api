using Aliyun.OSS;
using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Infrastructure.Model;
using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
using ZR.Common;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Dto.ProdDto;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Model.System.Vo;
using ZR.Repository;
using ZR.Service.IService;
using ZR.Service.System.IService;

namespace ZR.Service
{
    /// <summary>
    /// 
    /// </summary>
    [AppService(ServiceType = typeof(IMesGetService), ServiceLifetime = LifeTime.Transient)]
    public class MesGetService : BaseService<MBlockConfigType>, IMesGetService
    {
        /// <summary>
        /// //获取所有route信息
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<Route> GetListRoute(string parm, string site)
        {
            var response = Context.Queryable<Route>().WhereIF(!parm.IsNullOrEmpty(), it => it.RouteName.ToLower().StartsWith(parm.ToLower())).WhereIF(true, it => it.Enabled == "Y").Where(it => it.Site == site).ToList();
            return response;
        }
        /// <summary>
        /// 获取所有model机种信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public dynamic GetListModel(MesGetModel parm)
        {
            var response = Context.Queryable<ImesMmodel>().WhereIF(!parm.Model.IsNullOrEmpty(), it => it.Model.ToLower().StartsWith(parm.Model.ToLower())).WhereIF(true, it => it.EnaBled == "Y").Where(it => it.site == parm.Site).Select(a=>new {model=a.Model,modelDesc=a.ModelDesc}).OrderBy(a=>a.model).ToList();
            return response;
        }
        /// <summary>
        /// 获取打印参数的sql捞取列明信息信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public dynamic GetListPrintField(MesGetPrintField parm)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var inputData = "";
            var response = Context.Queryable<MPrintData>().Where( it => it.DataType == parm.DataType).WhereIF(true, it => it.Enabled == "Y").Where(it => it.Site == parm.Site).Select(a => new { DataType = a.DataType, DataOrder = a.DataOrder, DataSql = a.DataSql, InputParam = a.InputParam, InputField = a.InputField }).OrderBy(a=>a.DataOrder).ToList();
            if (response.Count > 0)
            {
                foreach (var rowitem in response)
                {
                    string inputparam = rowitem.InputParam.ToString();
                    string sqlvalue = rowitem.DataSql.ToString().Replace(inputparam, " '" + inputData + "' ");
                    DataTable dtvalue = Context.Ado.GetDataTable(sqlvalue);

                    foreach (DataColumn dc in dtvalue.Columns)
                    {
                        if (dic.Keys.Count(q => q.ToUpper() == dc.ColumnName.ToUpper()) > 0)
                        {
                            continue;
                        }
                        else
                        {
                            dic.Add(dc.ColumnName, "");
                        }
                    }
                }
            }
            return dic.Keys.ToArray();
        }
        /// <summary>
        /// //获取所有line信息
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<MLine> GetListLine(string parm, string site)
        {
            var response = Context.Queryable<MLine>().WhereIF(!parm.IsNullOrEmpty(), it => it.Line.ToLower().StartsWith(parm.ToLower())).WhereIF(true, it => it.Enabled == "Y").Where(it => it.Site == site).
               ToList();
            return response;
        }
        /// <summary>
        /// //获取所有StationType信息
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<MStationType> GetListStationType(string parm, string site)
        {
            var response = Context.Queryable<MStationType>().WhereIF(!parm.IsNullOrEmpty(), it => it.StationType.ToLower().StartsWith(parm.ToLower())).WhereIF(true, it => it.Enabled == "Y").Where(it => it.Site == site).ToList();
            return response;
        }
        /// <summary>
        /// //标签信息维护获取站点类型
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public dynamic GetListLabelStationType(MesGetLabelStationType parm)
        {
            var response = Context.Queryable<MStationType>().WhereIF(!parm.StationType.IsNullOrEmpty(), it => it.StationType == parm.StationType).WhereIF(true, it => it.Enabled == "Y" && it.ClientType == "MANU").Where(it => it.Site == parm.Site).Select(it => new{it.StationType, it.StationTypeDesc}).OrderBy(it=>it.StationType).ToList();
            return response;
        }
        /// <summary>
        /// //获取所有Station信息
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<Station> GetListStation(string parm, string site)
        {
            var response = Context.Queryable<Station>().WhereIF(!parm.IsNullOrEmpty(), it => it.StationName.ToLower().StartsWith(parm.ToLower())).WhereIF(true, it => it.Enabled == "Y").Where(it => it.Site == site).ToList();
            return response;
        }
        /// <summary>
        /// //获取包装规格信息
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<MPkspec> GetListPKSPEC(string parm, string site)
        {
            var response = Context.Queryable<MPkspec>().WhereIF(!parm.IsNullOrEmpty(), it => it.PkspecName.ToLower().StartsWith(parm.ToLower())).WhereIF(true, it => it.Enabled == "Y").Where(it => it.Site == site).ToList();
            return response;
        }
        /// <summary>
        /// //获取规则条码信息
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<MRuleSet> GetListRule(string parm, string site)
        {
            var response = Context.Queryable<MRuleSet>().WhereIF(!parm.IsNullOrEmpty(), it => it.RuleSetName.ToLower().StartsWith(parm.ToLower())).WhereIF(true, it => it.Enabled == "Y").Where(it => it.Site == site).ToList();
            return response;
        }
        /// <summary>
        /// //获取部门信息
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<MDept> GetListDept(string parm, string site)
        {
            var response = Context.Queryable<MDept,MSite>((a,b)=>a.Enabled=="Y"&&b.SiteCustomer==site).Select((a)=>new MDept { DeptName=a.DeptName }).ToList();
            return response;
        }
        /// <summary>
        /// //获取工单类型
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<dynamic> GetListWoType(string parm, string site)
        {
            string sql = @"SELECT CASE WO_TYPE  WHEN 'ZBTS' THEN 'ZBTS(一般工单)'  WHEN 'ZMPR' THEN 'ZMPR(重工工单)'   WHEN 'ZPRR' THEN 'ZPRR(重工工单)'         WHEN 'ZENG' THEN 'ZENG(NPI工单)'   WHEN '8' THEN '8(重工委外工单)'   WHEN '11' THEN '11(拆件式工单)'  WHEN '13' THEN '13(预测工单)'    WHEN '15' THEN '15(试产工单)'   ELSE WO_TYPE || '(其他工单类型)'  END WO_TYPE FROM IMES.P_WO_BASE  WHERE WO_TYPE IS NOT NULL  GROUP BY WO_TYPE";
            var response = Context.Ado.SqlQuery<dynamic>(sql);
            return response;
        }
        /// <summary>
        /// //获取数据库IMES账号下的的Procedures
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<dynamic> GetListProcedures(string parm)
        {
            string sql = @"SELECT A.OWNER ||'.'||A.OBJECT_NAME PROCEDURE_NAME, 
                     LISTAGG (A.ARGUMENT_NAME, ',') WITHIN GROUP (ORDER BY A.SEQUENCE)
                     INPUT_PARAM
                     FROM ALL_ARGUMENTS A, ALL_PROCEDURES B
                     WHERE     A.OBJECT_ID = B.OBJECT_ID
                     AND A.OWNER = 'IMES'
                     AND B.OBJECT_TYPE = 'PROCEDURE'";
            if (!string.IsNullOrEmpty(parm))
            {
                sql += " AND  A.OWNER ||'.'||A.OBJECT_NAME LIKE UPPER('%" + parm + "%')";
            }

            sql += " GROUP BY A.OWNER ||'.'||A.OBJECT_NAME";
            var response = Context.Ado.SqlQuery<dynamic>(sql);
            return response;
        }
        /// <summary>
        /// 获取labeltypebase
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public dynamic GetListLabelTypeBase(MesGetLabelTypeBase parm)
        {
            var response = Context.Queryable<MLabelTypeBase>().WhereIF(!parm.TypeName.IsNullOrEmpty(),it=>it.TypeName==parm.TypeName).Where(it=>it.Site==parm.Site && it.Enabled=="Y").Select(it=>new {it.TypeName,it.TypeDesc}).ToList();
            return response;
        }
        /// <summary>
        /// 标签信息维护配置有station信息时获取imes的Label_type
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public dynamic GetListLabelTypeByStation(MesGetLabelType parm)
        {
            var response = Context.Queryable<MLabelType>().WhereIF(!parm.Id.IsNullOrEmpty(),it=>it.Id==parm.Id).WhereIF(!parm.Model.IsNullOrEmpty(), it => it.Model == parm.Model).WhereIF(!parm.Ipn.IsNullOrEmpty(), it => it.Ipn == parm.Ipn).WhereIF(!parm.LabelType.IsNullOrEmpty(), it => it.LabelType == parm.LabelType).Where(it => it.Site == parm.Site && it.Enabled == "Y").Select(it => new { it.Id,it.LabelType, it.Ipn, it.LabelTypeDesc,it.LabelName }).ToList();
            return response;
        }
        /// <summary>
        /// 根据料号获取机种
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<string> GetModelByPart(string parm, string site)
        {
            string sql = @"SELECT model FROM  IMES.M_PART where IPN=@ipn AND SITE=@site";
            var response = Context.Ado.SqlQuery<string>(sql,
                new List<SugarParameter>
            {
                    new SugarParameter("@ipn",parm),
                    new SugarParameter("@site",site)
            });
            return response;
        }

        /// <summary>
        /// 获取stationtype生成树
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<dynamic> GetStationTypeTree( string site)
        {
            string sql = @"SELECT B.STAGE,B.STATION_TYPE, B.STATION_TYPE||' '||B.STATION_TYPE_DESC STATION_TYPE_DESC, B.CLIENT_TYPE
                                      FROM IMES.M_STAGE A, IMES.M_STATION_TYPE B
                                     WHERE A.STAGE = B.STAGE
                                       AND A.ENABLED = 'Y'
                                       AND B.ENABLED = 'Y' AND B.SITE=@site ORDER BY B.STAGE, B.STATION_TYPE";
            var response = Context.Ado.SqlQuery<dynamic>(sql,
                new List<SugarParameter>
            {
                    new SugarParameter("@site",site)
            });
            return response;
        }

        /// <summary>
        /// 获取station生成树
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<dynamic> GetStationTree( string site)
        {
            string sql = @"SELECT B.SITE, B.LINE, C.STAGE,D.STATION_TYPE, D.STATION_TYPE||' '||D.STATION_TYPE_DESC STATION_TYPE_DESC, A.STATION_NAME
                                      FROM IMES.M_STATION      A,
                                           IMES.M_LINE         B,
                                           IMES.M_STAGE        C,
                                           IMES.M_STATION_TYPE D,
                                           IMES.M_OPERATE_TYPE F
                                     WHERE A.LINE = B.LINE
                                       AND A.STAGE = C.STAGE
                                       AND A.STATION_TYPE = D.STATION_TYPE
                                       AND D.OPERATE_TYPE = F.SCAN_TYPE
                                       AND A.ENABLED = 'Y'
                                       AND B.ENABLED = 'Y'
                                       AND C.ENABLED = 'Y'
                                       AND D.ENABLED = 'Y'
                                       AND B.SITE= @site
                                     ORDER BY B.SITE, B.LINE, C.STAGE, D.STATION_TYPE, A.STATION_NAME";
            var response = Context.Ado.SqlQuery<dynamic>(sql,
                new List<SugarParameter>
            {
                    new SugarParameter("@site",site)
            });
            return response;
        }
        /// <summary>
        /// //获取料号
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<MPart> GetListPart(string parm, string site)
        {
            var response = Context.Queryable<MPart>().WhereIF(!parm.IsNullOrEmpty(),a=>a.Ipn.ToLower().StartsWith(parm.ToLower())).Where(a=>a.Enabled=="Y").Where(a=>a.Site==site).Select(a=>new MPart { Ipn=a.Ipn,SPEC1=a.SPEC1,SPEC2=a.SPEC2}).ToList();
            return response;
        }
        /// <summary>
        /// 检查料号
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<MPart> GetPart(string parm, string site)
        {
            var response = Context.Queryable<MPart>().Where(a => a.Ipn==parm).Where(a => a.Enabled == "Y").Where(a => a.Site == site).Select(a => new MPart { Ipn = a.Ipn, SPEC1 = a.SPEC1, SPEC2 = a.SPEC2 }).ToList();
            return response;
        }
        /// <summary>
        /// 获取WareHouse
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<dynamic> GetWareHouse(MesCheckWareHouse parm)
        {
            var listhouse = await Context.Queryable<MWarehouse>().Where(it=> it.Enabled == "Y" && it.Site == parm.Site).WhereIF(!parm.WarehouseCode.IsNullOrEmpty(), it => it.WarehouseCode == parm.WarehouseCode).Select(it=>new {it.WarehouseCode,it.WarehouseName}).ToListAsync();
            return listhouse;
        }
        /// <summary>
        /// 获取工号
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<dynamic> GetEMP(MesGetEMP parm)
        {
            var listhouse = await Context.Queryable<ImesMemp>().Where(it => it.enabled == "Y" && it.site == parm.Site).WhereIF(!parm.EMP_NAME.IsNullOrEmpty(), it => it.empName == parm.EMP_NAME).WhereIF(!parm.EMP_NO.IsNullOrEmpty(),it=>it.empNo==parm.EMP_NO).Select(it => new { it.empName, it.empNo,it.deptName }).ToListAsync();
            return listhouse;
        }
        /// <summary>
        /// 检查WareHouse
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<string> CheckWareHouse(MesCheckWareHouse parm)
        {
            var listhouse = await Context.Queryable<MWarehouse>().Where(it=>it.WarehouseCode==parm.WarehouseCode && it.Enabled=="Y" && it.Site==parm.Site).ToListAsync();
            if(listhouse.Count ==0)
            {
                return "Warehouse不存在" + parm.WarehouseCode;
            }
            var listlocation = await Context.Queryable<MLocation>().Where(it=>it.LocationCode==parm.LocationCode && it.Enabled== "Y" && it.Site==parm.Site).ToListAsync();
            if (listlocation.Count == 0)
            {
                return "Location不存在" + parm.WarehouseCode;
            }
            var check = await Context.Queryable<MLocation>().Where(it => it.LocationCode == parm.LocationCode && it.WarehouseCode==parm.WarehouseCode && it.Enabled == "Y" && it.Site == parm.Site).ToListAsync();
            if (check.Count == 0)
            {
                return "储位与库位不一致";
            }
            return "OK";
        }
        /// <summary>
        /// labeltype程式获取料号
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public dynamic GetListPartlabeltype(MesGetPart parm)
        {
            string[] alltypes = new string[] { "FERT", "HALB" };
            var response = Context.Queryable<MPart>().WhereIF(!parm.Model.IsNullOrEmpty(), a => a.Model==parm.Model).WhereIF(!parm.Ipn.IsNullOrEmpty(),a=>a.Ipn==parm.Ipn).Where(a=>alltypes.Contains(a.Type)).Where(a => a.Enabled == "Y").Where(a => a.Site == parm.Site).Select(a => new { Ipn = a.Ipn, SPEC2 = a.SPEC2 }).OrderBy(a=>a.Ipn).ToList();
            return response;
        }
        /// <summary>
        /// 获取投入站
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<RouteDetail> GetInProcess(string parm, string site)
        {
            var response = Context.Queryable<RouteDetail>().WhereIF(!parm.IsNullOrEmpty(), a => a.RouteName==parm).Where (a=>a.Result !=1).Where(a => a.Enabled == "Y").Where(a => a.Site == site).OrderBy(a=>a.Seq).Select(a => new RouteDetail { NextStationType = a.NextStationType, Result = a.Result, Necessary=a.Necessary, DefaultInstationtype=a.DefaultInstationtype }).ToList();
            return response;
        }
        /// <summary>
        /// 获取产出站
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<RouteDetail> GetOutProcess(string parm, string site)
        {
            var response = Context.Queryable<RouteDetail>().WhereIF(!parm.IsNullOrEmpty(), a => a.RouteName == parm).Where(a => a.Result != 1).Where(a => a.Enabled == "Y").Where(a=>a.Necessary=="Y").Where(a => a.Site == site).OrderBy(a=>a.Seq).Select(a => new RouteDetail { NextStationType = a.NextStationType, Result = a.Result, Necessary = a.Necessary, DefaultInstationtype = a.DefaultInstationtype }).ToList();
            return response;
        }
        /// <summary>
        /// 获取SN信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<dynamic> GetSNInfo(MesGetSNInfo parm)
        {
            var sql = @"SELECT a.WORK_ORDER,a.serial_number, A.Customer_SN, A.PANEL_NO,a.IPN,A.VERSION, a.LINE, a.STAGE,a.STATION_TYPE,
                   IMES.fn_SNStatus_Result (a.CURRENT_STATUS)  CURRENT_STATUS,
                   DECODE (A.Work_Flag, 1, 'Scrap', '')  Scrap,
                   TO_CHAR (A.IN_STATIONTYPE_TIME, 'yyyy/mm/dd hh24:mi:ss') In_Process_Time,
                   TO_CHAR (A.OUT_STATIONTYPE_TIME, 'yyyy/mm/dd hh24:mi:ss')  Out_Process_Time,
                   TO_CHAR (A.IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss')  In_PDLine_Time,
                   TO_CHAR (A.OUT_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') Out_PDLine_Time,
                   A.Box_No,A.CARTON_NO,A.PALLET_NO, A.QC_NO,A.QC_RESULT,A.Rework_No,A.SN_VERSION
              FROM IMES.P_SN_STATUS a where 1=1 ";
            List<SugarParameter> ls=new List<SugarParameter>();
            if(!parm.WorkOrder.IsNullOrEmpty())
            {
                sql += " and a.work_order=@works";
                ls.Add(new SugarParameter("@works", parm.WorkOrder));
            }
            if (!parm.SerialNumber.IsNullOrEmpty())
            {
                sql += " and a.SERIAL_NUMBER=@sn";
                ls.Add(new SugarParameter("@sn", parm.SerialNumber));
            }
            if (!parm.PanelNo.IsNullOrEmpty())
            {
                sql += " and a.PANEL_NO=@panel";
                ls.Add(new SugarParameter("@panel", parm.PanelNo));
            }
            if (!parm.PalletNo.IsNullOrEmpty())
            {
                sql += " and a.PALLET_NO=@pallet";
                ls.Add(new SugarParameter("@pallet", parm.PalletNo));
            }
            if (!parm.CartonNo.IsNullOrEmpty())
            {
                sql += " and a.CARTON_NO=@carton";
                ls.Add(new SugarParameter("@carton", parm.CartonNo));
            }
            if (!parm.Site.IsNullOrEmpty())
            {
                sql += " and a.SITE=@site";
                ls.Add(new SugarParameter("@site", parm.Site));
            }
            var response =Context.Ado.SqlQuery<dynamic>(sql,ls);
            return response;
        }
        /// <summary>
        /// 获取工单生产的Panel信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<dynamic> GetWoPanelInfo(MesGetSNInfo parm)
        {
            var sql = @"SELECT a.WORK_ORDER,a.IPN, A.PANEL_NO, a.CLOSE_FLAG, a.STATUS,
                   TO_CHAR (A.CREATE_TIME, 'yyyy/mm/dd hh24:mi:ss') CREATE_TIME
              FROM IMES.P_WO_PANEL a where 1=1 ";
            List<SugarParameter> ls = new List<SugarParameter>();
            if (!parm.WorkOrder.IsNullOrEmpty())
            {
                sql += " and a.work_order=@works";
                ls.Add(new SugarParameter("@works", parm.WorkOrder));
            }
            if (!parm.PanelNo.IsNullOrEmpty())
            {
                sql += " and a.PANEL_NO=@panel";
                ls.Add(new SugarParameter("@panel", parm.PanelNo));
            }
            if (!parm.Site.IsNullOrEmpty())
            {
                sql += " and a.SITE=@site";
                ls.Add(new SugarParameter("@site", parm.Site));
            }
            var response = Context.Ado.SqlQuery<dynamic>(sql, ls);
            return response;
        }
    }
}
