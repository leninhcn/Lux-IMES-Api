using Infrastructure.Attribute;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Rework;
using ZR.Model;
using ZR.Service.Rework.IReworkService;
using System.Data;
using ZR.Model.Business;
using Oracle.ManagedDataAccess.Client;
using DbType = System.Data.DbType;
using System.Xml;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using ZR.Model.Dto.Rework;

namespace ZR.Service.Rework
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IPReworkNoService), ServiceLifetime = LifeTime.Transient)]
    public class PReworkNoService : BaseService<PReworkNo>, IPReworkNoService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        //public PagedInfo<PReworkNoDto> GetList(PReworkNoQueryDto parm)
        //{
        //    var predicate = Expressionable.Create<PReworkNo>();

        //    var response = Queryable()
        //        .Where(predicate.ToExpression())
        //        .ToPage<PReworkNo, PReworkNoDto>(parm);

        //    return response;
        //}


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PReworkNo GetInfo(int Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PReworkNo AddPReworkNo(PReworkNo model)
        {
            return Context.Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdatePReworkNo(PReworkNo model)
        {
            //var response = Update(w => w.Id == model.Id, it => new PReworkNo()
            //{
            //    ReworkNo = model.ReworkNo,
            //    Remark = model.Remark,
            //    CreateTime = model.CreateTime,
            //    CreateEmpno = model.CreateEmpno,
            //    Condition = model.Condition,
            //});
            //return response;
            return Update(model, true);
        }

        public DataTable GetSpec(string parm)
        {
            var sql=string.IsNullOrEmpty(parm)? $@"SELECT * FROM (
SELECT DISTINCT(B.STATION_TYPE ||'#'||E.MES_SPEC) AS  MES_SPEC 
FROM  
    (SELECT WORK_ORDER 
    FROM IMES.P_SN_STATUS ) A,
    IMES.M_STATIONTYPE_PARTSPEC      B,
    IMES.P_WO_BOM                    C,
    IMES.M_PART_SPEC_ERP_MES_MAPPING D,
    IMES.M_SN_FEATURE                E
WHERE A.WORK_ORDER = C.WORK_ORDER
AND C.ITEM_SPEC1 = D.ERP_SPEC
AND D.MES_SPEC = B.KP_SPEC
AND C.ITEM_IPN = E.IPN                       
AND A.WORK_ORDER = C.WORK_ORDER ) ORDER BY MES_SPEC":$@"SELECT * FROM (
SELECT DISTINCT(B.STATION_TYPE ||'#'||E.MES_SPEC) AS  MES_SPEC 
FROM  
    (SELECT WORK_ORDER 
    FROM IMES.P_SN_STATUS 
    WHERE  (PALLET_NO ='{parm}' OR WORK_ORDER ='{parm}' OR PANEL_NO ='{parm}' 
    OR BOX_NO='{parm}' OR SERIAL_NUMBER='{parm}' OR CARTON_NO='{parm}' OR SHIPPING_NO='{parm}')  AND ROWNUM=1) A,
    IMES.M_STATIONTYPE_PARTSPEC      B,
    IMES.P_WO_BOM                    C,
    IMES.M_PART_SPEC_ERP_MES_MAPPING D,
    IMES.M_SN_FEATURE                E
WHERE A.WORK_ORDER = C.WORK_ORDER
AND C.ITEM_SPEC1 = D.ERP_SPEC
AND D.MES_SPEC = B.KP_SPEC
AND C.ITEM_IPN = E.IPN                       
AND A.WORK_ORDER = C.WORK_ORDER ) ORDER BY MES_SPEC";

            var res = SqlQuery(sql);
            return res;
        }

        public DataTable GetReworkno(string parm)
        {
           var sql= $"SELECT   IMES.FN_REWORK_RWNO('{parm}') AS REWORK_NO FROM DUAL";
           var res = SqlQuery(sql);
            return res;
        }

        public DataTable GetStationOptions(string parm)
        {
            var sql=$@"     
     WITH SRC AS(SELECT ROUTE_NAME,STATION_TYPE,NEXT_STATION_TYPE FROM IMES.M_ROUTE_DETAIL WHERE ROUTE_NAME='{parm}' AND NECESSARY='Y' AND RESULT=0)
     SELECT NEXT_STATION_TYPE FROM SRC A
                     START WITH A.STATION_TYPE='0' CONNECT BY PRIOR A.NEXT_STATION_TYPE=A.STATION_TYPE";
            var res = SqlQuery(sql);
            return res;
        }

        public DataTable GetRoute(string parm)
        {
            var sql= $@"SELECT DISTINCT ROUTE_NAME 
    FROM IMES.P_SN_STATUS 
    WHERE  (PALLET_NO ='{parm}' OR WORK_ORDER ='{parm}' OR PANEL_NO ='{parm}' 
    OR BOX_NO='{parm}' OR SERIAL_NUMBER='{parm}' OR CARTON_NO='{parm}' OR SHIPPING_NO='{parm}')";
            var res = SqlQuery(sql);
            return res;
        }

        public DataTable PreCheck(string input,string inputtype, int isnewwo,string newwo, string tstation)
        {

            var sql = $@"   SELECT IMES.FUN_REWORK_PRECHECK('{input}','{inputtype}',{isnewwo},'{newwo}','{tstation}') RES,SERIAL_NUMBER SN
          ,PANEL_NO PANEL,WORK_ORDER WO,IPN,MODEL,CUSTOMER_SN CSN,PALLET_NO PALLET
          ,CARTON_NO CARTON,BOX_NO BOX,STATION_TYPE STATIONTYPE
          ,NEXT_STATION_TYPE NEXTSTATIONTYPE,OUT_STATIONTYPE_TIME OUTSTATIONTYPETIME
          ,ROUTE_NAME ROUTENAME
   FROM IMES.P_SN_STATUS 
   WHERE  (PALLET_NO ='{input}' OR WORK_ORDER ='{input}' OR PANEL_NO ='{input}' 
   OR BOX_NO='{input}' OR SERIAL_NUMBER='{input}' OR CARTON_NO='{input}' OR SHIPPING_NO='{input}')";
            var res = SqlQuery(sql);
            return res;
        }

        public string ReworkExcute(Dictionary<string,object> parm)
        {

            //dict.Add("imported", imported);
            //dict.Add("isnewwo", isnewwo);
            //dict.Add("inputtype", inputtype);
            //dict.Add("newwo", newwo);
            //dict.Add("inputvalue", inputvalue);
            //dict.Add("reworkno", reworkno);
            //dict.Add("routename", routename);
            //dict.Add("returnstation", returnstation);
            //dict.Add("checkstation", checkstation);
            //dict.Add("remark", remark);
            //dict.Add("incidentals", incidentals);
            //dict.Add("tvalue", tvalue);

            //构建xml
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmldecl, root);
            XmlElement element = doc.CreateElement("ROWS");
            doc.AppendChild(element);
            XmlElement row = doc.CreateElement("ROW");
            //row.SetAttribute("ID", "1");
            foreach (var kp in parm)
            {
                //对于非数组类型的变量设置成xml的节点属性
                if (kp.Value is not Array)
                {
                    row.SetAttribute(kp.Key.ToUpper(), kp.Value is bool ? ((bool)kp.Value ? "1" : "0") : kp.Value.ToString());
                }
                else 
                {
                    //对于数组类型的变量设置成ROW节点下的子节点
                    foreach (var it in ((string[])kp.Value))
                    {
                        var child = doc.CreateElement(kp.Key.ToUpper());
                        child.InnerText= it.ToString();
                        row.AppendChild(child);
                    }
                  
                }
            }
            element.AppendChild(row);

            var tres = new SugarParameter("TRES", "", true);
            
            var res = Context.Ado.UseStoredProcedure().ExecuteCommand("SAJET.SP_REWORK_EXCUTE",
                 new SugarParameter[]
                 {
                    new SugarParameter("XML", doc.OuterXml,DbType.Xml),
                    tres
                 });
            return tres.Value.ToString();
        }
    }




}
