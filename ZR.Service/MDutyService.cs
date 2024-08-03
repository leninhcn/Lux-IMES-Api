using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Infrastructure.Model;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using ZR.Common;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Model.System.Vo;
using ZR.Repository;
using ZR.Service.IService;
using ZR.Service.System.IService;

namespace ZR.Service
{
    /// <summary>
    /// 不良原因业务处理层
    /// </summary>
    [AppService(ServiceType = typeof(IMDutyService), ServiceLifetime = LifeTime.Transient)]
    public class MDutyService : BaseService<MDuty>, IMDutyService
    {
        /// <summary>
        /// //获取所有信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MDutyDto> GetList(MDutyQueryDto parm)
        {
            var predicate = Expressionable.Create<MDuty>();
            predicate = predicate.AndIF (parm.Enabled.IsNotEmpty(),it => it.Enabled == parm.Enabled);
            predicate = predicate.AndIF(parm.DutyCode != null, it => it.DutyCode.Contains(parm.DutyCode));
            predicate = predicate.And(it => it.Site == parm.Site);
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            // var response1 = GetPages(predicate.ToExpression(), parm, s => new { s.CreateTime });
            // var response = GetPages((predicate.ToExpression());
            //排序
            parm.Sort = "CreateTime";
            parm.SortType= "desc";
            var response = Queryable().Where(predicate.ToExpression()).ToPage<MDuty, MDutyDto>(parm);
            return response;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MDuty GetInfo(long ID)
        {
            var response = Queryable().Where(x => x.Id == ID).First();
            return response;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddMReason(MDuty model)
        {        
            try
            {
                //检查是否有重复
                var predicate = Expressionable.Create<MDuty>();
            predicate.AndIF(model.DutyCode.IsNotEmpty(), it => it.DutyCode == model.DutyCode);
            var response = GetList(predicate.ToExpression());
            if(response.Count()>0)
            {
                return "DutyCode 重复，请重新检查";
            }
            //获取ID
            //var oData = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);
            var tMax = new SugarParameter("T_MAXID",null,true);

             Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_GET_MAXID",
            new SugarParameter[]
            {
                    new SugarParameter("TFIELD", "ID"),
                    new SugarParameter("TTABLE", "SAJET.M_DUTY"),
                new SugarParameter("TNUM", 8),
                     tRes,tMax
                });
            if(tRes.Value.ToString()=="OK")
            {
                model.Id = Convert.ToInt64( tMax.Value.ToString());
            }
            else
            {
                return  "获取ID最大值失败，请检查 "+tRes.Value.ToString();
            }
                int i = Context.Insertable(model).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.M_DUTY_HT_NEW select * FROM SAJET.M_DUTY where ID = @ID", new List<SugarParameter>{ new SugarParameter("@ID", model.Id ) });
            return i==1?"OK":"插入失败";
            }
            catch(Exception ex)
            {
                return "新增失败，请检查 "+ex.ToString();
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UpdateMReason(MDuty model)
        {   
            model.UpdateTime = DateTime.Now;
            int result = Context.Updateable(model).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.M_DUTY_HT_NEW select * FROM SAJET.M_DUTY where ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", model.Id) });
            return "OK";
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UpdateMReasonStatus(MDuty model)
        {
            model.UpdateTime = DateTime.Now;
            int result = Context.Updateable(model).UpdateColumns(it => new { it.Enabled, it.UpdateTime, it.UpdateEmpno }).WhereColumns(it => new {it.Id}).ExecuteCommand();
            //备份
             Context.Ado.ExecuteCommand("insert INTO SAJET.M_DUTY_HT_NEW select * FROM SAJET.M_DUTY where ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", model.Id) });
            return "OK";
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DeleteMReason(MDutyDto model,string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0)
            {
                return "删除失败Id 不能为空";
            }
            ids = "(" + ids + ")";
            //int result = Context.Updateable(defect).ExecuteCommand();
           Context.Ado.ExecuteCommand($" update SAJET.M_DUTY set UPDATE_TIME=SYSDATE,UPDATE_EMPNO=@EMPNO WHERE ID in {ids}", new List<SugarParameter> {  new SugarParameter("@EMPNO",model.UpdateEmpno)} );
            //备份
            Context.Ado.ExecuteCommand($"insert INTO SAJET.M_DUTY_HT_NEW select * FROM SAJET.M_DUTY where ID in {ids}");
            //删除
            Context.Ado.ExecuteCommand($"delete IMES.M_DUTY where ID in {ids}");
            return "OK";
        }
    }
}
