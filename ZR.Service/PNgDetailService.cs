using System;
using SqlSugar;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using ZR.Common;
using System.Text;
using Infrastructure;
using ZR.Service.IService;
using System.Data;
using JinianNet.JNTemplate;
using Aliyun.OSS;
using System.Security.Policy;
using Org.BouncyCastle.Utilities;

namespace ZR.Service.Business
{
    /// <summary>
    /// 不良品统计Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IPNgDetailService), ServiceLifetime = LifeTime.Transient)]
    public class PNgDetailService : BaseService<PNgDetail>, IPNgDetailService
    {

        private readonly IMntnWorkOrderService _mntnWorkOrderService;

        public PNgDetailService(IMntnWorkOrderService mntnWorkOrderService)
        {
            _mntnWorkOrderService = mntnWorkOrderService;
        }



        /// <summary>
        /// 查询不良品统计列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PNgDetailDto> GetList(PNgDetailQueryDto parm)
        {
            var predicate = Expressionable.Create<PNgDetail>();
            predicate.AndIF(!parm.WorkOrder.IsNullOrEmpty(), it => it.WorkOrder.Contains(parm.WorkOrder));
            predicate.AndIF(!parm.StationType.IsNullOrEmpty(), it => it.StationType.Contains(parm.StationType));
            predicate.And(it => it.Site == parm.Site).And(it => it.Enabled == "Y");
            parm.Sort = "CreateTime";
            parm.SortType = "desc";
            var response = Queryable()
            
                .Where(predicate.ToExpression())
                .ToPage<PNgDetail, PNgDetailDto>(parm);

                

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PNgDetail GetInfo(long Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加不良品统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddPNgDetail(PNgDetail model, List<PNgDetail> datalist)
        {

            var groupedObjects = datalist
           .GroupBy(obj => obj.MachineCode)
           .Select(group => new
           {
               GroupField = group.Key,
               TotalCount = group.Sum(obj => obj.SamplingInspectionNg),
           });

            foreach (var group in groupedObjects)
            {
                Console.WriteLine($"Group: {group.GroupField}, Total Count: {group.TotalCount}");
                var qtySql = @"SELECT QTY-NGQTY AS QTY FROM 
                 (SELECT NVL(MAX(QTY),0) AS QTY FROM IMES.P_STATION_LOAD_MATERIAL_HT 
                  WHERE WORK_ORDER=@WORK_ORDER AND MACHINE_CODE=@MACHINE_CODE) t1,
                 (SELECT NVL(SUM(SAMPLING_INSPECTION_NG),0) AS NGQTY FROM IMES.P_NG_DETAIL 
                     WHERE WORK_ORDER=@WORK_ORDER AND MACHINE_CODE=@MACHINE_CODE AND ENABLED='Y') t2";
                //机台投入数量
                decimal qty = Context.Ado.SqlQuery<decimal>(qtySql,
                  new List<SugarParameter> {
                    new SugarParameter("@WORK_ORDER",model.WorkOrder),
                    new SugarParameter("@MACHINE_CODE",group.GroupField)
                 }).First();
                
                 if (group.TotalCount > qty)
                 {
                     throw new CustomException($"机台号: {group.GroupField}不能超过总投入数量");
                 }
            }



            WoBase WoBase = Context.Queryable<WoBase>().Where(i => i.WorkOrder == model.WorkOrder).First();

            if (WoBase == null)
            {
                throw new CustomException("未查询到工单!");
            }

            long? ngSum = 0;
            Context.Ado.BeginTran();
            try
            {
                foreach (var item in datalist)
            {

                    ngSum += item.SamplingInspectionNg;
                    item.Ipn = model.Ipn;
                    item.Operator = model.Operator;
                    item.SamplingInspection = model.SamplingInspection;
                    item.Targetqty = model.Targetqty;
                    item.WorkOrder = model.WorkOrder;
                    item.Site = model.Site;
                    item.CreateEmpno = model.CreateEmpno;
                    item.Enabled = "Y";
                    item.CreateTime = model.CreateTime;
                    item.Id=Tools.GenerateLongUUID();
                    int v = Context.Insertable(item).ExecuteReturnIdentity();

                    var parameters = new List<SugarParameter>
                     {
                      new SugarParameter("@T_NG_ID",v),
                      new SugarParameter("@T_EMP", item.CreateEmpno),
                      new SugarParameter("@T_TYPE", "ADD"),
                      new SugarParameter("@T_SITE", item.Site),
                      new SugarParameter("@tres", null, true), // 设置为输出参数
                      new SugarParameter("@tmsg", null, true) // 设置为输出参数
                     };
                    Context.Ado.UseStoredProcedure().ExecuteCommand("SAJET.SP_NG_DEDUCT", parameters);
                    // 获取输出参数值
                    var tres = parameters[4].Value;
                    var tmsg = parameters[5].Value;
                }
            }
            catch (Exception e)
            {
                Context.Ado.RollbackTran();
                throw new CustomException(e.Message);
            }
            WoBase.UpdateTime = DateTime.Now;
            long? ngQty = WoBase.NgQty;
            WoBase.NgQty = ngSum + ngQty;
            Context.Ado.CommitTran();
            return _mntnWorkOrderService.UpdatePWoBaseById(WoBase);
        }

        /// <summary>
        /// 修改不良品统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdatePNgDetail(PNgDetail model)
        {
            //原来的数据
            PNgDetail pNgDetail = GetInfo(model.Id);
            //新的数据
            WoBase WoBase = Context.Queryable<WoBase>().Where(i => i.WorkOrder == model.WorkOrder).First();
            long? ngQty = WoBase.NgQty;
            WoBase.NgQty = ngQty - pNgDetail.SamplingInspectionNg + model.SamplingInspectionNg;
            _mntnWorkOrderService.UpdatePWoBaseById(WoBase);
            return Update(model, true);
        }




        public List<dynamic> ListMaChine(string site)
        {
            return Context.Ado.SqlQuery<dynamic>(
                  @"
                   SELECT M.ID,M.MACHINE_CODE,M.MACHINE_DESC,M.MACHINE_TYPE,
                   G.STATION_TYPE,M.STAGE,M.LINE FROM IMES.M_MACHINE M
                   LEFT JOIN IMES.M_MACHINE_GROUP G ON (M.GROUP_ID=G.ID)
                   WHERE 
                   M.SITE=@SITE AND M.ENABLED =@ENABLED",
                new List<SugarParameter> {
                    new SugarParameter("@SITE",site),
                    new SugarParameter("@ENABLED","Y")
                });
        }

        public List<dynamic> GetListstatistics(PNgDetailQueryDto parm)
        {
            string sql = @"SELECT * FROM (
            SELECT IPN,WORK_ORDER, TARGETQTY, STATION_TYPE, SUM(SAMPLING_INSPECTION) AS OK, SUM(SAMPLING_INSPECTION_NG) AS NG,
            ROW_NUMBER() OVER (ORDER BY WORK_ORDER) AS RN
            FROM IMES.P_NG_DETAIL
            GROUP BY WORK_ORDER, TARGETQTY, STATION_TYPE,IPN )
            WHERE 1=1 AND RN BETWEEN @start1 AND @end1 ";

            if (!parm.WorkOrder.IsNullOrEmpty())
            {
                sql += " and WORK_ORDER LIKE '%" + parm.WorkOrder + "%'  ";
            }

            var response = Context.Ado.SqlQuery<dynamic>(sql, new List<SugarParameter>
            {
              new SugarParameter("@start1",parm.PageNum),
              new SugarParameter("@end1",parm.PageSize)
            });
            return response;
        }

        public int DelPNgDetail(PNgDetail parm)
        {

            WoBase WoBase = Context.Queryable<WoBase>().Where(i => i.WorkOrder == parm.WorkOrder).First();


            var parameters = new List<SugarParameter>
                     {
                      new SugarParameter("@T_NG_ID",parm.Id),
                      new SugarParameter("@T_EMP", parm.CreateEmpno),
                      new SugarParameter("@T_TYPE", "CANCEL"),
                      new SugarParameter("@T_SITE", parm.Site),
                      new SugarParameter("@tres", null, true), // 设置为输出参数
                      new SugarParameter("@tmsg", null, true) // 设置为输出参数
                     };
            Context.Ado.UseStoredProcedure().ExecuteCommand("SAJET.SP_NG_DEDUCT", parameters);


            // 获取输出参数值
            var tres = parameters[4].Value;
            var tmsg = parameters[5].Value;

            long? ngQty = WoBase.NgQty;
            WoBase.NgQty = ngQty - parm.SamplingInspectionNg; 
            _mntnWorkOrderService.UpdatePWoBaseById(WoBase);
            return Update(parm, true);
        }

        public List<dynamic> QuerySnTravel(string workOrder)
        {
            string sql = @"SELECT DISTINCT STATION_TYPE  FROM IMES.P_SN_TRAVEL 
                WHERE WORK_ORDER =@workOrder";
            return Context.Ado.SqlQuery<dynamic>(sql, new SugarParameter("@workOrder", workOrder));
        }


        public List<dynamic> SnTravelMacine(string Macine)
        {
            string sql = @"SELECT DISTINCT MACHINE_NO FROM IMES.P_SN_TRAVEL
                WHERE STATION_TYPE=@Macine";
            return Context.Ado.SqlQuery<dynamic>(sql, new SugarParameter("@Macine", Macine));
        }

    }
}