using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Service.IService;

namespace ZR.Service.WoManagement
{
    [AppService(ServiceType =typeof(IMntnWoStatusService), ServiceLifetime =LifeTime.Transient)]
    public  class MntnWoStatusService:BaseService<WoBase>,IMntnWoStatusService
    {
        public async Task<List<dynamic>> GetWoBase(string wo,string site)
        {
            // var wobase=await Context.Queryable<WoBase>().Where(a=>a.Site==site&&a.WorkOrder==wo).ToListAsync();
            string sql = @"SELECT A.*, IMES.SJ_WOSTATUS_RESULT (A.WO_STATUS) WOSTATUS FROM  IMES.P_WO_BASE A where A.WORK_ORDER=@wo and A.SITE= @site";
           return await Context.Ado.SqlQueryAsync<dynamic>(sql,new List<SugarParameter> { new SugarParameter ("@wo",wo),new SugarParameter("@site",site) });
        }

        public async Task<string> CheckWoBase(MntnWoStatusDto param)
        {
            var tres = new SugarParameter("TRES", null, true);
           await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_WO_RELEASE",
                new SugarParameter[]
                {
                    new SugarParameter("T_WO",param.WorkOrder),
                    new SugarParameter("T_STATUS",""),
                    tres,
                    new SugarParameter("T_PANET",""),
                    new SugarParameter("T_SITE",param.Site)
                });
           return tres.Value.ToString();
        }

        public async Task<string> UpdateWoBase(MntnWoStatusDto param)
        {
            string result = "OK";
            try { 
            string sql = @"update SAJET.p_wo_base set wo_status = @status where ( work_order = @wo or M_WO = @wo1 ) and site = @site";
            await Context.Ado.ExecuteCommandAsync(sql,
                 new SugarParameter[]
             {
                new SugarParameter("@status",param.Status),
                new SugarParameter("@wo",param.WorkOrder),
                new SugarParameter("@wo1",param.WorkOrder),
                new SugarParameter("@site",param.Site)
             });
            PWoStatus woStatus = new()
            {
                WorkOrder = param.WorkOrder,
                WoStatus=param.Status,
                Memo=param.Memo,
                UpdateEmpno=param.UpdateEmpno,
                Site=param.Site
            };
            //ignoreNullColumn 忽略空值，不会产生在insert语句中
            await Context.Insertable(woStatus).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
                return result;
            }
            catch (Exception ex)
            {
                result=ex.Message;
                return result;
            }
        }
    }
}
