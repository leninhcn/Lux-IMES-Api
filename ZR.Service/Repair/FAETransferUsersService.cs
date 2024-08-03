using Infrastructure.Attribute;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using NLog.Filters;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Model.System.Generate;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class FAETransferUsersService : BaseService<SnStatus>, IFAETransferUsersService
    {
        public async Task<ExecuteResult> ShowData(FAETransUser retData)
        {
            ExecuteResult exeRes = new ExecuteResult();
            string sName = string.Empty;
            try
            {
                exeRes = new ExecuteResult();
                string sqlStr = @" select  a.*  FROM SAJET.M_REPAIR_FAE_TRANSFER_USERS a where 1=1 ";

                if (!string.IsNullOrEmpty(retData.sFieldName) && !string.IsNullOrEmpty(retData.sFieldText))
                {
                    sqlStr = sqlStr + " and " + retData.sFieldName + $" like '%{retData.sFieldText.ToUpper()}%' ";
                }
                if (retData.Enable == 0)
                {
                    sqlStr = sqlStr + " and enabled = 'Y' ";
                }
                else if (retData.Enable == 1)
                {
                    sqlStr = sqlStr + " and enabled = 'N' ";
                }
                sqlStr = sqlStr + " order by ID ";


                exeRes.Anything = await Context.Ado.GetDataTableAsync(sqlStr);
                exeRes.Status = true;

            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }
            return exeRes;
        }

        private async Task<ExecuteResult> GetValue(FAETransInfo mi)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlStr = @" select A.*
                                     FROM SAJET.M_REPAIR_FAE_TRANSFER_USERS A
                                     WHERE EMP_NO=@EMP_NO
                                           and a.enabled= 'Y'";
                
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlStr, new List<SugarParameter>
                    {   new SugarParameter("@EMP_NO", mi.EMP_NO) });
                exeRes.Anything = dtTemp;
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        private async Task<ExecuteResult> GetMaxId()
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var sRes = new SugarParameter("TRES", null, true);
                var tMaxID = new SugarParameter("T_MAXID", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_GET_MAXID",
                new SugarParameter[]
                    {
                    new SugarParameter("TFIELD", "ID"),
                    new SugarParameter("TTABLE", "SAJET.M_REPAIR_FAE_TRANSFER_USERS"),
                    new SugarParameter("TNUM","8"),
                    sRes,tMaxID
                    });

                if (sRes.Value.ToString() != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = sRes.Value.ToString();
                }
                else
                {
                    string sMaxID = tMaxID.Value.ToString();
                    exeRes.Anything = sMaxID;
                    exeRes.Status = true;
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        private async Task<ExecuteResult> InsertValueInfo(string sMaxID, FAETransInfo transferInfo,string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sSQL = @" Insert INTO SAJET.M_REPAIR_FAE_TRANSFER_USERS 
                   (ID, EMP_NO,EMP_NAME,LAB,PHONE_NO,ENABLED,UPDATE_empno,CREATE_EMPNO) 
                   Values (@ID,@EMP_NO,@EMP_NAME,@LAB,@PHONE_NO ,'Y',@UPDATE_empno,@CREATE_EMPNO) ";

                var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
              {
                new SugarParameter("@ID", sMaxID),
                new SugarParameter("@EMP_NO", transferInfo.EMP_NO),
                new SugarParameter("@EMP_NAME",transferInfo.EMP_NAME),
                new SugarParameter("@LAB", transferInfo.LAB),
                new SugarParameter("@PHONE_NO",transferInfo.PHONE_NO),
                new SugarParameter("@UPDATE_empno",userNo),
                new SugarParameter("@CREATE_EMPNO",userNo),
               });

                exeRes.Status = affected>0;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        private async Task<ExecuteResult> InsertValueLogInfo(string ID)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlStr = @" insert INTO SAJET.M_REPAIR_FAE_TRANSFER_USERS_HT 
                                    ( select * FROM SAJET.M_REPAIR_FAE_TRANSFER_USERS a where a.ID =@ID )";
               
                var affected = await Context.Ado.ExecuteCommandAsync(sqlStr, new List<SugarParameter>
              {
                new SugarParameter("@ID", ID)
               });

                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        private async Task<ExecuteResult> UpdateValueInfo(FAETransInfo transferInfo, string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
              string  sSQL = @" update SAJET.M_REPAIR_FAE_TRANSFER_USERS 
                       set EMP_NO = @EMP_NO 
                          ,EMP_NAME = @EMP_NAME 
                          ,LAB = @LAB 
                          ,PHONE_NO = @PHONE_NO 
                          ,UPDATE_empno = @UPDATE_empno 
                          ,UPDATE_TIME = SYSDATE 
                          where ID = @ID ";
               
                var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
              {
                new SugarParameter("@EMP_NO", transferInfo.EMP_NO),
                new SugarParameter("@EMP_NAME", transferInfo.EMP_NAME),
                new SugarParameter("@LAB", transferInfo.LAB),
                new SugarParameter("@PHONE_NO", transferInfo.PHONE_NO),
                new SugarParameter("@UPDATE_empno", userNo),
                new SugarParameter("@ID", transferInfo.ID)
               });

                exeRes.Status = true;

            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        private async Task<ExecuteResult> DeleteValueInfo(string ID)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlStr = @" delete IMES.M_REPAIR_FAE_TRANSFER_USERS a where a.ID = @ID  ";
                var affected = await Context.Ado.ExecuteCommandAsync(sqlStr, new List<SugarParameter>
              {
                new SugarParameter("@ID", ID)
               });

                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        private async Task<ExecuteResult> EnableRow(string ID)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlStr = @" update SAJET.M_REPAIR_FAE_TRANSFER_USERS a set a.enabled='Y' where a.ID = @ID  ";
                var affected = await Context.Ado.ExecuteCommandAsync(sqlStr, new List<SugarParameter>
              {
                new SugarParameter("@ID", ID)
               });

                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        private async Task<ExecuteResult> DisableRow(string ID)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlStr = @" update SAJET.M_REPAIR_FAE_TRANSFER_USERS a set a.enabled='N' where a.ID = @ID  ";
                var affected = await Context.Ado.ExecuteCommandAsync(sqlStr, new List<SugarParameter>
                {
                  new SugarParameter("@ID", ID)
                 });

                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> EditValueInfo(FAETransInfo model,string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            if (model.Type == "APPEND")
            {
                exeRes =await GetValue(model);
                if (exeRes.Status)
                {
                    DataTable dtTemp = (DataTable)exeRes.Anything;
                    if (dtTemp.Rows.Count > 0)
                    {
                        exeRes.Status = false;
                        exeRes.Message = model.EMP_NO + " 数据已存在！";
                        return exeRes;
                    }
                }
                string MaxID = "";
                exeRes =await GetMaxId();
                if (!exeRes.Status)
                {
                    return exeRes;
                }
                else
                {
                    MaxID = (string)exeRes.Anything;
                }

                exeRes =await InsertValueInfo(MaxID, model, userNo);
                if (exeRes.Status)
                {
                    exeRes = await InsertValueLogInfo(MaxID);
                }
                if (!exeRes.Status)
                {
                    return exeRes;
                }
            }
            else if (model.Type == "MODIFY")
            {
                exeRes = await UpdateValueInfo(model,userNo);
                if (exeRes.Status)
                {
                    exeRes = await InsertValueLogInfo(model.ID.ToString());
                }
                if (!exeRes.Status)
                {
                    return exeRes;
                }

            }
            else if (model.Type == "DELETE")
            {
                exeRes = await InsertValueLogInfo(model.ID.ToString());
                if (exeRes.Status)
                {
                    exeRes = await DeleteValueInfo(model.ID.ToString());
                }
                if (!exeRes.Status)
                {
                    return exeRes;
                }
            }
            else if (model.Type == "DISABLE")
            {
                exeRes = await InsertValueLogInfo(model.ID.ToString());
                if (exeRes.Status)
                {
                    exeRes = await DisableRow(model.ID);
                }
                if (!exeRes.Status)
                {
                    return exeRes;
                }
            }
            else if (model.Type == "ENABLE")
            {
                exeRes = await InsertValueLogInfo(model.ID.ToString());
                if (exeRes.Status)
                {
                    exeRes = await EnableRow(model.ID);
                }
                if (!exeRes.Status)
                {
                    return exeRes;
                }
            }
            return exeRes;
        }

        public async Task<ExecuteResult> GetHtValues(string ID)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlStr = @" SELECT * FROM SAJET.M_REPAIR_FAE_TRANSFER_USERS_HT A WHERE A.ID = @V_ID ORDER BY A.UPDATE_TIME DESC ";

                DataTable tempDt = await Context.Ado.GetDataTableAsync(sqlStr, new List<SugarParameter>
                {
                  new SugarParameter("@V_ID", ID)
                 });
                exeRes.Anything = tempDt;
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> importDataToTable(List<FAETransInfo> fAETrans,string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                exeRes = new ExecuteResult() { Status = true };
                foreach (var item in fAETrans)
                {
                    string sMaxID = "";
                    exeRes = await GetMaxId();
                    if (!exeRes.Status)
                    {
                        return exeRes;
                    }
                    sMaxID = (string)exeRes.Anything;

                    item.ID = sMaxID;
                    item.Enabled = "Y";
                    
                    //先判断数据是否存在
                    DataTable dt = new DataTable();
                    exeRes = await GetValue(item);
                    if (!exeRes.Status)
                    {
                        return exeRes;
                    }
                    if (exeRes.Status)
                    {
                        dt = (DataTable)exeRes.Anything;
                        if (dt.Rows.Count > 0)
                        {
                            continue;
                        }
                    }

                    exeRes = await InsertValueInfo(sMaxID, item, userNo);
                    if (!exeRes.Status)
                    {
                        return exeRes;
                    }
                    exeRes = await InsertValueLogInfo(sMaxID);
                }
            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }
            return exeRes;
        }
    }
}
