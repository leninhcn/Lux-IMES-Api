using Aliyun.OSS;
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
using ZR.Service.System.IService;

namespace ZR.Service.System
{
    /// <summary>
    /// 收集异常信息
    /// </summary>
    [AppService(ServiceType = typeof(ICollectErrorService), ServiceLifetime = LifeTime.Transient)]
    public class CollectErrorService : BaseService<PCollecterrorLog>,ICollectErrorService
    {
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        public string Status()
        {
            try
            {
                return AppSettings.App(new string[] { "CollectError", "Status" });
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add(PCollecterrorLog model)
        {
            try
            {
                //获取ID
                //var oData = new SugarParameter("O_MSG", null, true);
                //判断异常是否已产生异常单据
                var tprogram = "";
                if (model.ProgramMent == "N/A")
                {
                    tprogram = model.ProgramMent;
                }
                 else if (model.ProgramMent.Contains('/'))
                {
                    tprogram = model.ProgramMent.Split('/')?.LastOrDefault() ?? model.ProgramMent;
                }
                else if(model.ProgramMent.Contains(':'))
                {
                     tprogram = model.ProgramMent.Split(':')?.FirstOrDefault() ?? model.ProgramMent;
                }
                else
                {
                   tprogram = model.ProgramMent;
                }
                var sid= Context.Queryable<PTicketStatus>().Where(it => it.ClentIp == model.ClentIp && it.ProgramMent == tprogram && it.EmpNo == model.EmpNo  && it.StationName == model.StationName && it.Status == 0).Select(it => new { it.Id}).ToList();
               if (sid.Count ==0 )
                {
                    PTicketStatus stp = new()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        ClentIp=model.ClentIp,
                        ProgramMent=tprogram,
                        StationName=model.StationName,
                        EmpNo=model.EmpNo,
                        Site =model.Site,
                        CreateEmpno=model.EmpNo
                    };
                    Context.Insertable(stp).IgnoreColumns(ignoreNullColumn:true).ExecuteCommand();
                    Context.Ado.ExecuteCommand("insert INTO SAJET.P_TICKET_TRAVEL select * FROM SAJET.P_TICKET_STATUS where ID=@id",new List<SugarParameter> { new SugarParameter("@id",stp.Id)});
                    model.TicketId=stp.Id;
                }
               else
                {
                    model.TicketId = sid[0].Id;
                }
                //记录errorlog
                model.Id=Guid.NewGuid().ToString("D");
                int i = Context.Insertable(model).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            return i==1?"OK":"插入失败";
            }
            catch(Exception ex)
            {
                return "新增失败，请检查 "+ex.ToString();
            }
        }
    }
}
