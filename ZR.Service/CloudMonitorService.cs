using Aliyun.OSS;
using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Microsoft.Data.SqlClient;
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
using static System.Net.WebRequestMethods;

namespace ZR.Service
{
    /// <summary>
    /// 运维云监控
    /// </summary>
    [AppService(ServiceType =typeof(ICloudMonitorService),ServiceLifetime =LifeTime.Transient)]
    public class CloudMonitorService:BaseService<PTicketStatus>, ICloudMonitorService
    {
        /// <summary>
        /// //获取所有信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PagedInfo<PTicketStatus> GetLsit (PTicketStatusQueryDto param)
        {
            var predicate = Expressionable.Create<PTicketStatus>();
            if(param.Type=="OPEN")
            {
                predicate = predicate.And(it => it.Status == 0 && it.AssignEmp==null);
            }
            else if(param.Type=="ONGOING")
            {
                predicate = predicate.And(it => it.Status ==1 ||it.Status==3);
                predicate=predicate.AndIF(!param.AssignEmp.IsNullOrEmpty(),it=>it.AssignEmp==param.AssignEmp);
                predicate = predicate.AndIF(!param.UpdateEmpno.IsNullOrEmpty(), it => it.UpdateEmpno == param.UpdateEmpno);
            }
            else if(param.Type=="COSE")
            {
                predicate = predicate.And(it => it.Status == 2);
                predicate = predicate.AndIF(param.AssignEmp.IsNotEmpty(), it => it.AssignEmp == param.AssignEmp);
                predicate = predicate.AndIF(!param.UpdateEmpno.IsNullOrEmpty(), it => it.UpdateEmpno == param.UpdateEmpno);
            }  
            predicate = predicate.AndIF(!param.ClentIp.IsNullOrEmpty(),it=> it.ClentIp == param.ClentIp);
            predicate = predicate.AndIF(!param.EmpNo.IsNullOrEmpty(),it=> it.EmpNo == param.EmpNo);
            predicate = predicate.AndIF(!param.ProgramMent.IsNullOrEmpty(),it=> it.ProgramMent == param.ProgramMent);
            predicate = predicate.And(it => it.Site == param.site);
            param.Sort = "CreateTime";
            param.SortType = "desc";
            var response = Queryable().Where(predicate.ToExpression()).ToPage<PTicketStatus>(param);
            return response;
        }
        /// <summary>
        /// 获取明细详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<PCollecterrorLog> GetInfo(string ID)
        {
            var response = Context.Queryable<PCollecterrorLog>().Where(it=>it.TicketId==ID).ToList();
            return response;
        }
        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<PTicketTravel> GetTravel(string ID)
        {
            var response = Context.Queryable<PTicketTravel>().Where(it => it.Id == ID).OrderBy(it=>it.UpdateTime, OrderByType.Asc).ToList();
            return response;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddTicket(PTicketStatus model)
        {
            try
            {
                model.Id = Guid.NewGuid().ToString("D");
                int i = Context.Insertable(model).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                //备份
                Context.Ado.ExecuteCommand("insert into imes.P_TICKET_TRAVEL select * from imes.P_TICKET_STATUS where ID=@id", new List<SugarParameter> { new SugarParameter("@id", model.Id) });
                return i == 1 ? "OK" : "插入失败";
            }
            catch (Exception ex)
            {
                return "新增失败，请检查 " + ex.ToString();
            }
        }
        /// <summary>
        /// 签核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Resultinfo<string> UpdateTicket(PTicketStatus model)
        {
            Resultinfo<string> resutinfo = new Resultinfo<string>();
            //定义2个流程
            /*状态 status
             * 0 待处理
             * 1 待优化
             * 2 流程结束关单
             * 3 已派单
             * 派单 0-3
             * errortype 0 不需要优化，处理完问题直接关单，中心负责人，状态status 0 ->2，被指派人 3 ->2
             *           1 需要优化,处理完问题后第一次签核，状态status 0 ->1,优化后第二次签核，status 1->2
             *               指派人第一次签核 ，状态status 3-1，第二次签核1-2
            */
            if (model.ErrorType==0)
            {
                model.Status = 2;
            }
            else if(model.ErrorType==1)
            {
                if(model.Status==0 ||model.Status==3)
                {
                    model.Status=1;
                }
                else
                {
                    model.Status=2;
                }
            }
            else {
                //return "异常类型无法识别，请确认 "+model.ErrorType;
                resutinfo.Result = "IMESERR007";
                resutinfo.ResErrCodeParam = model.ErrorType.ToString();
                return resutinfo;
            }
            model.UpdateTime = DateTime.Now;
            int result = Context.Updateable(model).UpdateColumns(it => new {it.Status,it.Mark,it.UpdateTime,it.UpdateEmpno,it.ErrorType,it.AssignEmp}).WhereColumns(it => new {it.Id}).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert into imes.P_TICKET_TRAVEL select * from imes.P_TICKET_STATUS where ID=@id", new List<SugarParameter> { new SugarParameter("@id", model.Id) });
            // return "OK";
            if (result == 0)
            {
                resutinfo.Result = "IMESERR008";
               // resutinfo.ErrCode = "UpdateWareHouse:1";
            }
            return resutinfo;
        }
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Resultinfo<string> UpdateTicketStatus(PTicketStatus model)
        {
            Resultinfo<string> resutinfo=new Resultinfo<string>();
            model.UpdateTime = DateTime.Now;
            int result = Context.Updateable(model).UpdateColumns(it => new { it.Status,  it.UpdateTime, it.UpdateEmpno, it.ErrorType, it.AssignEmp }).WhereColumns(it => new { it.Id }).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert into imes.P_TICKET_TRAVEL select * from imes.P_TICKET_STATUS where ID=@id", new List<SugarParameter> { new SugarParameter("@id", model.Id) });
            //return "OK";
            if (result == 0)
            {
                resutinfo.Result = "IMESERR008";
             // resutinfo.ErrCode = "UpdateWareHouse:1";
            }
            return resutinfo;
        }
        /// <summary>
        /// 指派单据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Resultinfo<string> AssignTicket(PTicketStatus model)
        {
            Resultinfo<string> resutinfo = new Resultinfo<string>();
            model.UpdateTime = DateTime.Now;
            model.Status = 3;
            int result = Context.Updateable(model).UpdateColumns(it => new { it.Status, it.UpdateTime, it.UpdateEmpno, it.AssignEmp }).WhereColumns(it => new { it.Id }).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert into imes.P_TICKET_TRAVEL select * from imes.P_TICKET_STATUS where ID=@id", new List<SugarParameter> { new SugarParameter("@id", model.Id) });
            //return "OK";
            if (result == 0)
            {
                resutinfo.Result = "IMESERR008";
                // resutinfo.ErrCode = "UpdateWareHouse:1";
            }
            return resutinfo;
        }

        /// <summary>
        /// 报表获取数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Resultinfo<List<PTicketReportRes>> GetReport(PTicketReportQueryDto param)
        {
            Resultinfo<List<PTicketReportRes>> resutinfo = new Resultinfo<List<PTicketReportRes>>();
            //
            /* param
             * week     周
             * month    月
             * year     年 
             * ReportQty 获取状态数量
             * ReportIMESErrorTop 获取imeserror top10
             * ReportTimeTop      获取处理时间
            */
            var sqltype = "";
            if (param.TimeType.ToUpper() == "week".ToUpper())
            { sqltype = "yyyyMMIW"; }
            else if (param.TimeType.ToUpper() == "month".ToUpper())
            {
                sqltype = "yyyyMM";
            }
            else if(param.TimeType.ToUpper() == "year".ToUpper())
            {
                sqltype = "yyyy";
            }
            else
            { sqltype = "yyyyMM"; }

            if(param.Type.ToUpper() == "ReportQty".ToUpper())
            { //select to_char(sysdate,'yyyyMMIW') time, status,count(status) qty from imes.P_TICKET_STATUS where to_char(CREATE_TIME,'yyyyMMIW')=to_char(sysdate,'yyyyMMIW') group by status order by status
                var qtylist = Context.Ado.SqlQuery<PTicketReportRes>($"select to_char(sysdate,'{sqltype}') time, status name,count(status) qty from imes.P_TICKET_STATUS where to_char(CREATE_TIME,'{sqltype}')=to_char(sysdate,'{sqltype}') and site=@site group by status order by status", new List<SugarParameter> { new SugarParameter("site", param.Site) }).ToList();
                resutinfo.Data = qtylist;
            }
            else if( param.Type.ToUpper()== "ReportIMESErrorTop".ToUpper())
            {
                //select * from (select to_char(sysdate,'yyyyMMIW') time,RES_ERRCODE,count(RES_ERRCODE) qty from imes.P_COLLECTERROR_LOG  where to_char(CREATE_TIME,'yyyyMMIW')= to_char(sysdate, 'yyyyMMIW') - 1 and site = 'DEF' group by RES_ERRCODE order by qty desc) where rownum <= 10
                var qtylist = Context.Ado.SqlQuery<PTicketReportRes>($"select * from (select to_char(sysdate,'{sqltype}') time,RES_ERRCODE name,count(RES_ERRCODE) qty from imes.P_COLLECTERROR_LOG  where to_char(CREATE_TIME,'{sqltype}')=to_char(sysdate,'{sqltype}') and site=@site group by RES_ERRCODE order by qty desc) where rownum <=10 ", new List<SugarParameter> { new SugarParameter("site", param.Site) }).ToList();
                resutinfo.Data = qtylist;
            }
            else if(param.Type.ToUpper()== "ReportTimeTop".ToUpper())
            {
                //            "with b as( SELECT CASE
                //        WHEN 10 >= trunc((SYSDATE - a.CREATE_TIME) * 24 * 60)  THEN 'Less than 10 minutes'
                //        WHEN 20 >= trunc((SYSDATE - a.CREATE_TIME) * 24 * 60) and trunc((SYSDATE -a.CREATE_TIME)*24 * 60) >= 10 THEN 'Between 10 and 20 minutes'
                //        ELSE 'More than 30 minutes'
                //    END AS name,to_char(CREATE_TIME, 'yyyyMMIW') time
                //FROM (select ID, CREATE_TIME from imes.P_TICKET_STATUS where to_char(CREATE_TIME, 'yyyyMMIW') = to_char(sysdate, 'yyyyMMIW') and site = 'DEF') A) select time, name, count(name) from b group by name,time;
                var qtylist = Context.Ado.SqlQuery<PTicketReportRes>($@"with b as( SELECT   CASE
        WHEN 10 >= trunc((SYSDATE - a.CREATE_TIME)*24*60)  THEN 'Less than 10 minutes'
        WHEN 20 >= trunc((SYSDATE - a.CREATE_TIME)*24*60) and trunc((SYSDATE - a.CREATE_TIME)*24*60) >10 THEN 'Between 10 and 20 minutes'
        ELSE 'More than 30 minutes'
    END AS name,to_char(CREATE_TIME,'{sqltype}') time  FROM
    (select ID, CREATE_TIME from imes.P_TICKET_STATUS where status NOT IN (1,2) and to_char(CREATE_TIME,'{sqltype}')=to_char(sysdate,'{sqltype}') and site=@site ) A)
    select time,name,count(name) qty from b group by name,time", new List<SugarParameter> { new SugarParameter("site", param.Site) }).ToList();
                resutinfo.Data = qtylist;
            }
            else
            {
                resutinfo.Result = "IMESERR009";
            }
           
            return resutinfo;
        }



    }
}
