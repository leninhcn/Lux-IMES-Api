using Aliyun.OSS;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Repository;
using ZR.Service.IService;

namespace ZR.Service.WoManagement
{
    [AppService(ServiceType = typeof(IMntnWorkOrderService), ServiceLifetime = LifeTime.Transient)]
    public class MntnWorkOrderService : BaseService<WoBase>, IMntnWorkOrderService
    {
        public PagedInfo<PWoBaseDto> GetWoBaseList(PWoBaseQueryDto parm)
        {
            // var wobase=await Context.Queryable<WoBase>().Where(a=>a.Site==site&&a.WorkOrder==wo).ToListAsync();
            // string sql = @"SELECT A.*, IMES.SJ_WOSTATUS_RESULT (A.WO_STATUS) WOSTATUS FROM  IMES.P_WO_BASE A where A.WORK_ORDER=@wo and A.SITE= @site";
            //return await Context.Ado.SqlQueryAsync<dynamic>(sql,new List<SugarParameter> { new SugarParameter ("@wo",parm.Value),new SugarParameter("@site",parm.Select) });
            if (parm.WoStatus == "ALL")
            {
                parm.WoStatus = "";
            }
            var predicate = Expressionable.Create<WoBase>();
            predicate = predicate.AndIF(!parm.WoStatus.IsNullOrEmpty(), it => it.WoStatus == parm.WoStatus);
            predicate = predicate.AndIF(parm.Select == "WORK_ORDER", it => it.WorkOrder.ToLower().StartsWith(parm.Value.ToLower()));
            predicate = predicate.AndIF(parm.Select == "IPN", it => it.Ipn.ToLower().StartsWith(parm.Value.ToLower()));
            predicate = predicate.AndIF(parm.Select == "WO_TYPE", it => it.WoType.ToLower().StartsWith(parm.Value.ToLower()));
            predicate = predicate.AndIF(parm.Select == "LINE", it => it.Line.ToLower().StartsWith(parm.Value.ToLower()));
            predicate = predicate.AndIF(parm.Select == "ROUTE_NAME", it => it.RouteName.ToLower().StartsWith(parm.Value.ToLower()));
            //predicate = predicate.AndIF(!parm.site.IsNullOrEmpty(),it => it.Site == parm.site);
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            // var response1 = GetPages(predicate.ToExpression(), parm, s => new { s.CreateTime });
            // var response = GetPages((predicate.ToExpression());
            //排序
            parm.Sort = "CreateTime";
            parm.SortType = "desc";
            var response = Queryable().Where(predicate.ToExpression()).ToPage<WoBase, PWoBaseDto>(parm);
            return response;
        }
        public List<PWoBaseHt> GetWoBaseHistoryList(PWoBaseHTQueryDto parm)
        {
            var response = Context.Queryable<PWoBaseHt>().Where(it=>it.WorkOrder==parm.WorkOrder && it.Site==parm.Site).ToList();
            return response;
        }

        public List<dynamic> GetWoStatusList(PWoBaseHTQueryDto parm)
        {
            var sql = @"SELECT A.WO_STATUS,
                   A.UPDATE_TIME,
                   IMES.FN_WOSTATUS_RESULT (A.WO_STATUS) WOSTATUS,
                   A.MEMO REMARK,
                   A.UPDATE_EMPNO
              FROM SAJET.P_WO_STATUS A where A.WORK_ORDER= @wo AND A.SITE=@site order by A.UPDATE_TIME";
            var response = Context.Ado.SqlQuery<dynamic>(sql, new List<SugarParameter> { new SugarParameter("@wo", parm.WorkOrder), new SugarParameter("@site", parm.Site) });
            return response;
        }
        public String AddPWoBase(WoBase parm)
        {
            try
            {
                //检查是否有重复
                var predicate = Expressionable.Create<WoBase>();
                predicate.And(it => it.WorkOrder == parm.WorkOrder).And(it => it.Site == parm.Site);
                var response = GetList(predicate.ToExpression());
                if (response.Count() > 0)
                {
                    return $"工单 :{parm.WorkOrder} 重复，请重新检查";
                }
                //获取机种
                string sql = @"SELECT model FROM  IMES.M_PART where IPN=@ipn AND SITE=@site";
                var response1 = Context.Ado.SqlQuery<string>(sql,
                new List<SugarParameter>
                {
                new SugarParameter("@ipn",parm.Ipn),
                    new SugarParameter("@site",parm.Site)
                });
                if (response1.Count > 0)
                {
                    parm.Model = response1[0];
                }
                //插入资料
                int i = Context.Insertable(parm).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                //备份备份代码取消,数据库trigger
                //Context.Ado.ExecuteCommand("insert INTO SAJET.M_BLOCK_CONFIG_TYPE_HT select * FROM SAJET.M_BLOCK_CONFIG_TYPE where CONFIG_TYPE_ID = @ID", new List<SugarParameter>{ new SugarParameter("@ID", model.ConfigTypeId ) });
                return i == 1 ? "OK" : "插入失败";
            }
            catch (Exception ex)
            {
                return "新增失败，请检查 " + ex.ToString();
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int UpdatePWoBase(WoBase param)
        {
            ////动态切换数据库
            //var db0 = Context.AsTenant().GetConnection("0").Ado.SqlQuery<dynamic>("select * FROM SAJET.M_SITE");
            ////动态切换数据库
            //var db1= Context.AsTenant().GetConnection("1").Ado.SqlQuery<dynamic>("select * FROM SAJET.M_SITE");
            //获取机种
            string sql = @"SELECT model FROM  IMES.M_PART where IPN=@ipn AND SITE=@site";
            var response1 = Context.Ado.SqlQuery<string>(sql,
            new List<SugarParameter>
            {
                new SugarParameter("@ipn",param.Ipn),
                    new SugarParameter("@site",param.Site)
            });
            if (response1.Count > 0)
            {
                param.Model = response1[0];
            }
            param.UpdateTime = DateTime.Now;
            //int result = Context.Updateable(param).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.P_WO_BASE_HT select * FROM SAJET.P_WO_BASE where WORK_ORDER = @workorder AND SITE=@site", new List<SugarParameter> { new SugarParameter("@workorder", param.WorkOrder), new SugarParameter("@site", param.Site) });
            var response = Context.Updateable(param).UpdateColumns(it => new { it.Ipn, it.Model, it.Version, it.WoType, it.TargetQty, it.WoScheduleStartDate, it.WoScheduleCloseDate, it.WoPurpose, it.WoBuild, it.WoConfig, it.WoPhsae, it.WoVersion, it.Remark, it.WarehouseNo, it.WarehouseLocation, it.Line, it.RouteName, it.StartStationType, it.EndStationType, it.PkspecName, it.RuleSetName, it.DeptName, it.RmaCustomer, it.RmaAccountValue, it.RmaNo, it.UpdateEmpno, it.UpdateTime }).WhereColumns(it => new { it.WorkOrder, it.Site }).ExecuteCommand();
            return response;

        }
        /// <summary>
        /// 修改绑定设备
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int UpdatePWoBaseEquipment(WoBase param)
        {
           
            param.UpdateTime = DateTime.Now;
            //int result = Context.Updateable(param).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.P_WO_BASE_HT select * FROM SAJET.P_WO_BASE where WORK_ORDER = @workorder AND SITE=@site", new List<SugarParameter> { new SugarParameter("@workorder", param.WorkOrder), new SugarParameter("@site", param.Site) });
            var response = Context.Updateable(param).UpdateColumns(it => new { it.EquipmentCode,it.CompanyName, it.UpdateEmpno, it.UpdateTime }).WhereColumns(it => new { it.WorkOrder, it.Site }).ExecuteCommand();
            return response;

        }

        public List<WoBase> GetWoBaseListPda(PWoBaseQueryDto parm)
        {

            StringBuilder sql = new StringBuilder(@"SELECT DISTINCT B.* FROM SAJET.P_WO_BASE B LEFT JOIN 
            IMES. P_WO_CUTTING G ON (B.WORK_ORDER=G.WORK_ORDER)
            WHERE  (B.WO_STATUS =2 OR  (B.WO_STATUS =3 AND G.CLOSED='N'))");

            if (!string.IsNullOrEmpty(parm.Value))
            {
                sql.Append(" AND B.WORK_ORDER like '%" + parm.Value + "%'   ");
            }
            //var predicate = Expressionable.Create<WoBase>();
            //predicate = predicate.AndIF(parm.Select == "WORK_ORDER", it => it.WorkOrder.ToLower().StartsWith(parm.Value.ToLower()));
            //predicate = predicate.AndIF(parm.Select == "IPN", it => it.Ipn.ToLower().StartsWith(parm.Value.ToLower()));
            //predicate = predicate.AndIF(parm.Select == "WO_TYPE", it => it.WoType.ToLower().StartsWith(parm.Value.ToLower()));
            //predicate = predicate.AndIF(parm.Select == "LINE", it => it.Line.ToLower().StartsWith(parm.Value.ToLower()));
            //predicate = predicate.AndIF(parm.Select == "ROUTE_NAME", it => it.RouteName.ToLower().StartsWith(parm.Value.ToLower()));
            //predicate = predicate.And(it => it.WoStatus == "3" || it.WoStatus == "2");
            //predicate = predicate.And(it => it.Site == parm.Site);
            ////排序
            //parm.Sort = "CreateTime";
            //parm.SortType = "desc";
            //                var response = Queryable(sql).ToPage<WoBase, PWoBaseDto>(parm);
            List<WoBase> sugarQueryable = Context.SqlQueryable<WoBase>(sql.ToString()).ToList();

            return sugarQueryable;
            // DataTable dt = SqlQuery(sql.ToString()).ToList<;
            //   return dt;
        }

        public int UpdatePWoBaseById(WoBase model)
        {
            int result = Context.Updateable(model).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.P_WO_BASE_HT select * FROM SAJET.P_WO_BASE where WORK_ORDER = @workorder AND SITE=@site", new List<SugarParameter> { new SugarParameter("@workorder", model.WorkOrder), new SugarParameter("@site", model.Site) });
            var response = Context.Updateable(model).UpdateColumns(it => new { it.Ipn, it.Model, it.Version, it.WoType, it.TargetQty, it.WoScheduleStartDate, it.WoScheduleCloseDate, it.WoPurpose, it.WoBuild, it.WoConfig, it.WoPhsae, it.WoVersion, it.Remark, it.WarehouseNo, it.WarehouseLocation, it.Line, it.RouteName, it.StartStationType, it.EndStationType, it.PkspecName, it.RuleSetName, it.DeptName, it.RmaCustomer, it.RmaAccountValue, it.RmaNo, it.UpdateEmpno, it.UpdateTime }).WhereColumns(it => new { it.WorkOrder, it.Site }).ExecuteCommand();
            return response;

        }

        public PagedInfo<PWoBaseDto> GetWoBaseListNg(PWoBaseQueryDto parm)

        {
            if (parm.WoStatus == "ALL")
            {
                parm.WoStatus = "";
            }
            var predicate = Expressionable.Create<WoBase>();
            predicate = predicate.AndIF(!parm.WoStatus.IsNullOrEmpty(), it => it.WoStatus == parm.WoStatus);
            predicate = predicate.AndIF(parm.Select == "WORK_ORDER", it => it.WorkOrder.ToLower().StartsWith(parm.Value.ToLower()));
            predicate = predicate.AndIF(parm.Select == "IPN", it => it.Ipn.ToLower().StartsWith(parm.Value.ToLower()));
            predicate = predicate.AndIF(parm.Select == "WO_TYPE", it => it.WoType.ToLower().StartsWith(parm.Value.ToLower()));
            predicate = predicate.AndIF(parm.Select == "LINE", it => it.Line.ToLower().StartsWith(parm.Value.ToLower()));
            predicate = predicate.AndIF(parm.Select == "ROUTE_NAME", it => it.RouteName.ToLower().StartsWith(parm.Value.ToLower()));
            predicate = predicate.AndIF(parm.Select == "ROUTE_NAME", it => it.RouteName.ToLower().StartsWith(parm.Value.ToLower()));
            predicate = predicate.And(it => it.InputQty > 0);

            //排序
            parm.Sort = "CreateTime";
            parm.SortType = "desc";
            var response = Queryable().Where(predicate.ToExpression()).ToPage<WoBase, PWoBaseDto>(parm);
            return response;
        }
    }
}
