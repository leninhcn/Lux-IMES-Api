using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Infrastructure.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
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
using ZR.ServiceCore.Model.Dto;

namespace ZR.Service
{
    /// <summary>
    /// 不良原因业务处理层
    /// </summary>
    [AppService(ServiceType = typeof(IMReasonService), ServiceLifetime = LifeTime.Transient)]
    public class MReasonService : BaseService<MReason>, IMReasonService
    {
        /// <summary>
        /// //获取所有信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MReasonDto> GetList(MReasonQueryDto parm)
        {
            if (parm.Enabled == "ALL")
            {
                parm.Enabled = null;
            }
            var predicate = Expressionable.Create<MReason>();
            predicate = predicate.AndIF (!parm.Enabled.IsNullOrEmpty(),it => it.Enabled == parm.Enabled);
            predicate = predicate.AndIF(parm.ReasonCode != null, it => it.ReasonCode.ToLower().StartsWith(parm.ReasonCode.ToLower()));
            predicate = predicate.AndIF(parm.ReasonDesc != null, it => it.ReasonDesc.ToLower().StartsWith(parm.ReasonDesc.ToLower()));
            predicate = predicate.And(it => it.Site == parm.Site);
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            // var response1 = GetPages(predicate.ToExpression(), parm, s => new { s.CreateTime });
            // var response = GetPages((predicate.ToExpression());
            //排序
            parm.Sort = "CreateTime";
            parm.SortType= "desc";
            var response = Queryable().Where(predicate.ToExpression()).ToPage<MReason, MReasonDto>(parm);
            return response;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public MReason GetInfo(long ID)
        {
            var response = Queryable().Where(x => x.Id == ID).First();
            return response;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultInfo AddMReason(MReason model)
        {
            var resultinfo =new ResultInfo();
            var STPE = "AddMReason:STPE";
            try
            {
                //检查是否有重复
                var predicate = Expressionable.Create<MReason>();
           predicate= predicate.And(it => it.ReasonCode == model.ReasonCode);
                predicate = predicate.And(it => it.Site == model.Site);
            var response = GetList(predicate.ToExpression());
            if(response.Count()>0)
            { 
                STPE += "1";
                resultinfo.Errcode = STPE;
                resultinfo.Result= "ReasonCode 重复，请重新检查";
                return resultinfo;
            }
            //获取ID
            //var oData = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);
            var tMax = new SugarParameter("T_MAXID",null,true);

             Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_GET_MAXID",
            new SugarParameter[]
            {
                    new SugarParameter("TFIELD", "ID"),
                    new SugarParameter("TTABLE", "SAJET.M_REASON"),
                new SugarParameter("TNUM", 8),
                     tRes,tMax
                });
            if(tRes.Value.ToString()=="OK")
            {
                model.Id = Convert.ToInt64( tMax.Value.ToString());
            }
            else
            {
                    STPE += "2";
                    resultinfo.Errcode = STPE;
                    resultinfo.Result = "获取ID最大值失败，请检查 " + tRes.Value.ToString();
                    return resultinfo;
                   // return  "获取ID最大值失败，请检查 "+tRes.Value.ToString();
            }
                int i = Context.Insertable(model).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert into IMES.M_REASON_HT select * from IMES.M_REASON where ID = @ID", new List<SugarParameter>{ new SugarParameter("@ID", model.Id ) });
                STPE += "3";
                resultinfo.Errcode = STPE;
                resultinfo.Result = i == 1 ? "OK" : "插入失败";
                return resultinfo;
                //return i==1?"OK":"插入失败";
            }
            catch(Exception ex)
            {
                STPE += "4";
                resultinfo.Errcode = STPE;
                resultinfo.Result = "新增失败，请检查 " + ex.ToString();
                return resultinfo;
                //return "新增失败，请检查 "+ex.ToString();
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UpdateMReason(MReason model)
        {   
            model.UpdateTime = DateTime.Now;
            int result = Context.Updateable(model).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert into IMES.M_REASON_HT select * from IMES.M_REASON where ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", model.Id) });
            return "OK";
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UpdateMReasonStatus(MReason model)
        {
            model.UpdateTime = DateTime.Now;
            int result = Context.Updateable(model).UpdateColumns(it => new { it.Enabled, it.UpdateEmpno, it.UpdateTime }).WhereColumns(it => new {it.Id}).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert into IMES.M_REASON_HT select * from IMES.M_REASON where ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", model.Id) });
            return "OK";
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DeleteMReason(MReasonDto model,string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0)
            {
                return "删除失败Id 不能为空";
            }
            ids = "(" + ids + ")";
            //int result = Context.Updateable(defect).ExecuteCommand();
            Context.Ado.ExecuteCommand($" update imes.M_REASON set UPDATE_TIME=SYSDATE,UPDATE_EMPNO=@EMPNO WHERE ID in {ids}", new List<SugarParameter> {  new SugarParameter("@EMPNO",model.UpdateEmpno)}
            );
            //备份
            Context.Ado.ExecuteCommand($"insert into IMES.M_REASON_HT select * from IMES.M_REASON where ID in {ids}");
            //删除
            Context.Ado.ExecuteCommand($"delete IMES.M_REASON  where ID in {ids}");
            return "OK";
        }
    }
}
