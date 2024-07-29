using Infrastructure.Attribute;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.Quality;
using ZR.Model.Quality;
using ZR.Model;
using ZR.Service.Quality.IQualityService;
using ZR.Repository;
using Aliyun.OSS;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Infrastructure.Extensions;
using ZR.Service.IService;

namespace ZR.Service.Quality
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IMQcConfigService), ServiceLifetime = LifeTime.Transient)]
    public class MQcConfigService : BaseService<MQcConfig>,IMQcConfigService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MQcConfigDto> GetList(MQcConfigQueryDto parm)
        {
            var predicate = Expressionable.Create<MQcConfig>();
            var enabled = JToken.Parse(parm?.enabled)["val"].ParseToString();
            var exp = predicate
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "model", qcc => qcc.Model.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "checkrule", qcc => qcc.CheckRule.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "onlinestationtype", qcc => qcc.OnlineStationType.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "returnstationtype", qcc => qcc.ReturnStationType.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "qcroute", qcc => qcc.QcRoute.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "qctype", qcc => qcc.QcType.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "onlineflag", qcc => qcc.OnlineFlag.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "reqc", qcc => qcc.ReQc.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "autohold", qcc => qcc.AutoHold.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "allpass", qcc => qcc.AllPass.Equals(parm.filtervalue))
                    .AndIF(enabled != "A",qcc=>qcc.Enabled.Equals(enabled));
            var cconfigs = Queryable();

            var response = cconfigs
                .Where(exp.ToExpression())
                .ToPage<MQcConfig,MQcConfigDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MQcConfig GetInfo(int Id)
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
        public MQcConfig AddMQcConfig(MQcConfig model)
        {

            MQcConfig res = null;
            var rep = UseTran(() => {
                //1.查询获取数据表的总行数
                //2.更新model的ID
                //3.插入到表中
                var totalcount = Queryable().Count();
                model.Id = totalcount + 1;
                res = Context.Insertable(model).ExecuteReturnEntity();
            });
            return res;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateMQcConfig(MQcConfig model)
        {
            //var response = Update(w => w.Id == model.Id, it => new MQcConfig()
            //{
            //    Id = model.Id,
            //    Remarks = model.Remarks,
            //    CheckRule = model.CheckRule,
            //    OnlineFlag = model.OnlineFlag,
            //    OnlineStationType = model.OnlineStationType,
            //    ReturnStationType = model.ReturnStationType,
            //    QcRoute = model.QcRoute,
            //    QcStationType = model.QcStationType,
            //    Target = model.Target,
            //    Qty = model.Qty,
            //    ReQc = model.ReQc,
            //    QcLevel = model.QcLevel,
            //    QcType = model.QcType,
            //    UpdateEmpno = model.UpdateEmpno,
            //    UpdateTime = model.UpdateTime,
            //    CreateEmpno = model.CreateEmpno,
            //    CreateTime = model.CreateTime,
            //    Enabled = model.Enabled,
            //    AutoHold = model.AutoHold,
            //    AllPass = model.AllPass,
            //    Model = model.Model,
            //});
            //return response;
            return Update(model, true);
        }

        public PagedInfo<MQcConfig> GetList(MQcConfigQueryDto parm, PagerInfo pager)
        {
            var predicate = Expressionable.Create<MQcConfig>();
            var enabled = JToken.Parse(parm.enabled)["val"].ParseToString();
            var exp = predicate
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "model", qcc => qcc.Model.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "checkrule", qcc => qcc.CheckRule.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "onlinestationtype", qcc => qcc.OnlineStationType.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "returnstationtype", qcc => qcc.ReturnStationType.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "qcroute", qcc => qcc.QcRoute.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "qctype", qcc => qcc.QcType.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "onlineflag", qcc => qcc.OnlineFlag.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "reqc", qcc => qcc.ReQc.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "autohold", qcc => qcc.AutoHold.Equals(parm.filtervalue))
                    .AndIF((!string.IsNullOrEmpty(parm.filterfield)) && parm.filterfield == "allpass", qcc => qcc.AllPass.Equals(parm.filtervalue))
                    .AndIF(enabled != "A", qcc => qcc.Enabled.Equals(enabled));
            var cconfigs = Queryable();

            var response = cconfigs
                .Where(exp.ToExpression())
                .ToPage<MQcConfig>(pager);

            return response;
        }

        public DataTable GetLines(string parm)
        {
            var sql = string.IsNullOrEmpty(parm) ? "SELECT DISTINCT A.LINE FROM M_LINE A WHERE A.ENABLED = 'Y' ORDER BY A.LINE ASC" : $" SELECT DISTINCT A.LINE FROM M_LINE A WHERE A.ENABLED = 'Y' AND A.LINE LIKE '%{parm}%' ORDER BY A.LINE ASC ";
            return SqlQuery(sql);
        }

        public DataTable GetWos(string parm)
        { 
           var sql = string.IsNullOrEmpty(parm) ? "SELECT DISTINCT A.WORK_ORDER FROM P_WO_BASE A WHERE A.WO_STATUS <> '6' ORDER BY A.WORK_ORDER ASC" : $" SELECT DISTINCT A.WORK_ORDER FROM P_WO_BASE A WHERE A.WO_STATUS <> '6' AND A.WORK_ORDER LIKE '%{parm}%' ORDER BY A.WORK_ORDER ASC ";
            return SqlQuery(sql);
        }

        public DataTable GetIpns(string parm)
        {
            var sql = string.IsNullOrEmpty(parm) ? "SELECT DISTINCT A.IPN FROM M_PART A WHERE A.ENABLED = 'Y' ORDER BY A.IPN ASC " : $" SELECT DISTINCT A.IPN FROM M_PART A WHERE A.ENABLED = 'Y' AND A.IPN LIKE '%{parm}%' ORDER BY A.IPN ASC";
            return SqlQuery(sql);
        }

        public DataTable GetOnlineRoute(string parm)
        {
            var sql = string.IsNullOrEmpty(parm) ? "SELECT A.ROUTE_NAME ROUTE FROM M_ROUTE A WHERE A.ENABLED = 'Y' ORDER BY A.ROUTE_NAME ASC" : $" SELECT A.ROUTE_NAME ROUTE FROM M_ROUTE A WHERE A.ENABLED = 'Y' AND A.ROUTE_NAME LIKE '%{parm}%' ORDER BY A.ROUTE_NAME ASC";
            return SqlQuery(sql);
        }

        public DataTable GetQcRoute(string parm)
        {
            var sql = string.IsNullOrEmpty(parm) ? "SELECT A.ROUTE_NAME ROUTE FROM M_ROUTE A WHERE A.ENABLED = 'Y' ORDER BY A.ROUTE_NAME ASC" : $" SELECT A.ROUTE_NAME ROUTE FROM M_ROUTE A WHERE A.ENABLED = 'Y' AND A.ROUTE_NAME LIKE '%{parm}%' ORDER BY A.ROUTE_NAME ASC";
            return SqlQuery(sql);
        }

        public DataTable GetRouteDetail(string parm)
        {
            var sql =  @$"  SELECT A.STATION_TYPE, A.NEXT_STATION_TYPE
                                      FROM M_ROUTE_DETAIL A
                                     WHERE A.ROUTE_NAME = '{parm}'
                                       AND A.NECESSARY = 'N'
                                       AND A.RESULT = '0'
                                     ORDER BY A.STEP, A.SEQ ";
            return SqlQuery(sql);
        }

        public int GetQcLevel(string onlinestation, string checkrule)
        {
            var sql = @$"  SELECT A.QC_LEVEL
                                      FROM M_QC_CONFIG A
                                     WHERE A.ONLINE_STATION_TYPE = '{onlinestation}'
                                       AND A.CHECK_RULE = '{checkrule}'
                                     ORDER BY A.QC_LEVEL DESC ";
            var tb= SqlQuery(sql);
            
            return tb.Rows.Count==0? 1:tb.Rows[0][0].ParseToInt()+1;
        }

        public string GetRuleType(string val)
        {
            var sql=@$"WITH LINES AS(SELECT COUNT(*) EXISTED,'LINE' RULETYPE FROM M_LINE WHERE LINE ='{val}' AND ENABLED = 'Y')
    ,WOS AS(SELECT COUNT(*) EXISTED,'WO' RULETYPE FROM P_WO_BASE WHERE WORK_ORDER ='{val}' )
    ,IPNS AS(SELECT COUNT(*) EXISTED,'IPN' RULETYPE FROM M_PART WHERE IPN ='{val}' AND ENABLED = 'Y')
    ,RES AS(SELECT *FROM LINES
            UNION
            SELECT *FROM WOS
            UNION
            SELECT *FROM IPNS
    )
SELECT *FROM RES WHERE EXISTED>0";
            var tb = SqlQuery(sql);
            var res = tb.Rows.Count == 0 ? "INVALID" : tb.Rows[0]["RULETYPE"].ToString();
            return res;
        }
    }



}
