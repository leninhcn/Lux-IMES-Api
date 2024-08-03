using Infrastructure.Attribute;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;
using static System.Collections.Specialized.BitVector32;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class FAETransferService : BaseService<SnStatus>, IFAETransferService
    {
        public async Task<ExecuteResult> GetValues(string INPUTVALUE, string ITEM, SNTInfo sn)
        {
            SNTInfo snInfo = new SNTInfo();
            ExecuteResult exeRes =await GetTValues(INPUTVALUE, ITEM, sn.MODEL);
            if (exeRes.Status)
            {
                try
                {
                    JObject obj = (JObject)JsonConvert.DeserializeObject(exeRes.Anything.ToString());
                    switch (ITEM)
                    {
                        case "SN":
                            snInfo.SN = INPUTVALUE;
                            snInfo.WO = sn.WO = obj["SN_GET_WO"].ToString();
                            snInfo.MODEL = sn.MODEL = obj["SN_GET_MODEL"].ToString();
                            snInfo.IPN = sn.IPN = obj["SN_GET_IPN"].ToString();
                            snInfo.ROUTE = sn.ROUTE = obj["SN_GET_ROUTE"].ToString();
                            if (obj["SN_GET_KOL"].ToString() == "D52JP/CA-NK" || obj["SN_GET_KOL"].ToString() == "D52ROW-NK")
                                snInfo.KOL = "NK";
                            else
                                snInfo.KOL = "K";
                            break;
                        case "WO":
                            snInfo.WO = INPUTVALUE;
                            snInfo.TARTGET = obj["WO_GET_WO_TARGET_QTY"].ToString();
                            snInfo.INPUT = obj["WO_GET_WO_INPUT_QTY"].ToString();
                            snInfo.OUTPUT = obj["WO_GET_WO_OUTPUT_QTY"].ToString();
                            snInfo.NEEDINPUT = (Convert.ToInt32(snInfo.OUTPUT) - Convert.ToInt32(snInfo.INPUT)).ToString();
                            snInfo.IPN= obj["WO_GET_IPN"].ToString();
                            snInfo.MODEL = obj["WO_GET_MODEL"].ToString();
                            snInfo.ROUTE = obj["WO_GET_ROUTE"].ToString();
                            break;
                        case "TOOL":
                            snInfo.TOOL = INPUTVALUE;
                            break;
                        case "KPSN":
                            snInfo.KPSN = INPUTVALUE;
                            break;
                        case "GLUE":
                            snInfo.GLUENO = INPUTVALUE;
                            break;
                        case "REEL":
                            snInfo.REELNO = INPUTVALUE;
                            break;
                    }
                    exeRes.Anything = snInfo;
                }
                catch (Exception ex)
                {
                    exeRes.Status = false;
                    exeRes.Message = ex.Message;
                }
            }

            return exeRes;
        }

        private async Task<ExecuteResult> GetTValues(string INPUTVALUE, string ITEM, string MODEL_NAME)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);
                var tValue = new SugarParameter("T_VALUE", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_QUERY_VALUE_GROUP",
                new SugarParameter[]
                    {
                    new SugarParameter("T_INPUT_VALUE", INPUTVALUE),
                    new SugarParameter("T_ITEM", ITEM),
                    new SugarParameter("T_MODEL_NAME", MODEL_NAME),
                    tValue,
                    tRes
                    });
                string sRes = tRes.Value.ToString();
                if (sRes != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = sRes;
                }
                else
                {
                    exeRes.Status = true;
                    exeRes.Anything = tValue.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> CheckRepairIn(string SN)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                exeRes = new ExecuteResult();
                string sqlText = @"    SELECT COUNT (T.SERIAL_NUMBER)
                                      FROM SAJET.P_REPAIR_IN T
                                     WHERE T.SERIAL_NUMBER = @SN AND T.REPAIR_FLAG = 'N'";
               
               DataTable dtTemp =  await Context.Ado.GetDataTableAsync(sqlText, new List<SugarParameter>
                    {   new SugarParameter("@SN", SN) });

                int icount = Convert.ToInt32(dtTemp.Rows[0][0].ToString());
                if (icount > 0)
                {
                    exeRes.Status = true;
                }
                else
                {
                    exeRes.Message = "Check RepairIn Fail!";
                    exeRes.Status = false;
                }

            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> CheckOutUser(string empno)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlText = @"  SELECT A.EMP_NAME,A.EMP_NO,A.LAB ,A.PHONE_NO FROM SAJET.M_REPAIR_FAE_TRANSFER_USERS A
                                     WHERE A.EMP_NO =@EMP_NO AND A.ENABLED='Y'";

                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlText, new List<SugarParameter>
                    {   new SugarParameter("@EMP_NO", empno) });
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

        public async Task<ExecuteResult> CheckBackUser(string empno)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlText = @" select count(*) FROM SAJET.P_POST_EMP_CARD a WHERE EMP_CODE=@EMP_CODE";
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlText, new List<SugarParameter>
                    {   new SugarParameter("@EMP_CODE", empno) });
                exeRes.Anything = dtTemp;
                exeRes.Status = true;
                int icount = Convert.ToInt32(dtTemp.Rows[0][0].ToString());
                if (icount > 0)
                {
                    exeRes.Status = true;
                }
                else
                {
                    exeRes.Message = "Check BackUser Fail!";
                    exeRes.Status = false;
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> FAESNOUTCheck(string sn)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlText = @" SELECT * FROM SAJET.P_REPAIR_FAE_TRANSFERINOUT WHERE STATUS<>1 AND SN=@SN ";
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlText, new List<SugarParameter>
                    {   new SugarParameter("@SN", sn) });
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

        public async Task<ExecuteResult> GetFAESNOUT(string sn)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlText = @" SELECT * FROM SAJET.P_REPAIR_FAE_TRANSFERINOUT WHERE STATUS<>1 AND SN=@SN ";
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlText, new List<SugarParameter>
                    {   new SugarParameter("@SN", sn) });
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

        public async Task<ExecuteResult> UpdateTransfer(FAETransferInfo transferInfo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlStr = @"update SAJET.P_REPAIR_FAE_TRANSFERINOUT SET      
                                    BACKDATE=SYSDATE,
                                    BACKFROMUSERID=@BACKFROMUSERID,       
                                    BACKTOUSERID=@BACKTOUSERID,
                                    UPDATE_TIME=SYSDATE,
                                    UPDATE_EMPNO=@UPDATE_EMPNO,
                                    STATUS=@STATUS
                                    WHERE ID=@ID";

                var affected = await Context.Ado.ExecuteCommandAsync(sqlStr, new List<SugarParameter>
              {
                new SugarParameter("@BACKFROMUSERID", transferInfo.BACKFROMUSERID),
                new SugarParameter("@BACKTOUSERID", transferInfo.BACKTOUSERID),
                new SugarParameter("@UPDATE_EMPNO",transferInfo.UPDATE_EMPNO),
                new SugarParameter("@STATUS", transferInfo.STATUS),
                new SugarParameter("@ID",transferInfo.ID)
               });
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> GetRepairInRecid(string sn)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlText = @"    SELECT RECID
                                      FROM SAJET.P_REPAIR_IN T
                                     WHERE T.SERIAL_NUMBER = @SN AND T.REPAIR_FLAG = 'N'";
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sqlText, new List<SugarParameter>
                    {   new SugarParameter("@SN", sn) });
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

        public async Task<ExecuteResult> InsertTransfer(FAETransferInfo transferInfo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                exeRes = await GetMaxID("SAJET.P_REPAIR_FAE_TRANSFERINOUT", "ID", 8);
                if (!exeRes.Status)
                {
                    return exeRes;
                }
                string maxID =(string)exeRes.Anything;

                string sqlText = @"INSERT INTO SAJET.P_REPAIR_FAE_TRANSFERINOUT(
                                    ID,
                                    SN,
                                    MODEL, 
                                    IPN,
                                    WORK_ORDER,
                                    OUTDATE,
                                    OUTFROMUSERID,
                                    OUTTOUSERID,
                                    OUTTOUSERPHONE,
                                    BACKFROMUSERID,
                                    BACKTOUSERID,
                                    UPDATE_TIME,
                                    UPDATE_EMPNO,
                                    CREATE_TIME,
                                    CREATE_EMPNO,LAB,RECID)
                                    VALUES(
                                    @ID,
                                    @SN,
                                    @MODEL, 
                                    @IPN,
                                    @WORK_ORDER,
                                    SYSDATE,
                                    @OUTFROMUSERID,
                                    @OUTTOUSERID,
                                    @OUTTOUSERPHONE,
                                    @BACKFROMUSERID,
                                    @BACKTOUSERID,
                                    SYSDATE,
                                    @UPDATE_EMPNO,
                                     SYSDATE,
                                    @CREATE_EMPNO,
                                    @LAB,
                                    @RECID)";

                var affected = await Context.Ado.ExecuteCommandAsync(sqlText, new List<SugarParameter>
            {
                new SugarParameter("@ID", maxID),
                new SugarParameter("@SN", transferInfo.SN),
                new SugarParameter("@MODEL",transferInfo.Model),
                new SugarParameter("@IPN", transferInfo.IPN),
                new SugarParameter("@WORK_ORDER",transferInfo.WORK_ORDER),
                new SugarParameter("@OUTFROMUSERID", transferInfo.OUTFROMUSERID),
                new SugarParameter("@OUTTOUSERID", transferInfo.OUTTOUSERID),
                new SugarParameter("@OUTTOUSERPHONE", transferInfo.OUTTOUSERPHONE),
                new SugarParameter("@BACKFROMUSERID",transferInfo.BACKFROMUSERID),
                new SugarParameter("@BACKTOUSERID",transferInfo.BACKTOUSERID),
                new SugarParameter("@UPDATE_EMPNO",transferInfo.UPDATE_EMPNO),
                new SugarParameter("@CREATE_EMPNO",transferInfo.CREATE_EMPNO),
                new SugarParameter("@LAB",transferInfo.LAB),
                new SugarParameter("@RECID",transferInfo.RECID),
            });
                
                exeRes.Status = affected > 0;


            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }
            return exeRes;
        }

        private async Task<ExecuteResult> GetMaxID(string sTable, string sField, int iIDLength)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var sRes = new SugarParameter("TRES", null, true);
                var tMaxID = new SugarParameter("T_MAXID", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_GET_MAXID",
                new SugarParameter[]
                    {
                    new SugarParameter("TFIELD", sField),
                    new SugarParameter("TTABLE", sTable),
                    new SugarParameter("TNUM", iIDLength.ToString()),
                    sRes,tMaxID
                    });


                exeRes.Anything = "0";
                if (sRes.Value.ToString() != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = sRes.Value.ToString();
                }
                else
                {
                    exeRes.Status = true;
                    exeRes.Anything = tMaxID.Value.ToString();

                }
            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
                exeRes.Anything = "0";
            }
            return exeRes;
        }
    }
}
