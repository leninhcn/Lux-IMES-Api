using Infrastructure.Attribute;
using Infrastructure.Model;
using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Middleware;
using ZR.ServiceCore.Model.Dto;
using static System.Collections.Specialized.BitVector32;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class ErrorProofMaterialService : BaseService<SnStatus>, IErrorProofMaterialService
    {
        ExecuteResult exeRes = new ExecuteResult();

        DataTable dtTemp = new DataTable();

        public async Task<DataTable> GetSnStatusInfo(string sn)
        {
            string sSQL = @" SELECT * FROM  IMES.P_SN_status WHERE SERIAL_NUMBER='" + sn + "' and rownum = 1";

            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<ExecuteResult> CheckWoBomItemIpn(string wo, string ipn)
        {
            try
            {
                exeRes = new ExecuteResult();
                DataTable dt = new DataTable();
                string sql = @"SELECT ITEM_GROUP FROM SAJET.P_WO_BOM where WORK_ORDER =@WO  and  ITEM_IPN =@IPN ";
               
                dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter>
                { new SugarParameter("@WO", wo), new SugarParameter("@IPN", ipn)});

                if (dt.Rows.Count == 0)
                {
                    exeRes.Status = false;
                    exeRes.Message = " The reel partno does not exist in the BOM list ";
                    return exeRes;
                }

                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> CheckNewReelPn(string wo, string oldReelPn, string newReelPn)
        {
            try
            {
                exeRes = new ExecuteResult();
                DataTable dt = new DataTable();
                string sql = @"SELECT ITEM_GROUP FROM SAJET.P_WO_BOM where WORK_ORDER =@WO  and  ITEM_IPN =@IPN ";

                dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter>
                { new SugarParameter("@WO", wo),
                  new SugarParameter("@IPN", oldReelPn)});

                if (dt.Rows.Count == 0)
                {
                    exeRes.Status = false;
                    exeRes.Message = " The old reel partno does not exist in the BOM list ";
                    return exeRes;
                }

                if (oldReelPn == newReelPn)
                {
                    exeRes.Status = true;
                    return exeRes;
                }

                if (dt.Rows.Count == 1 && dt.Rows[0][0].ToString() == "0")
                {
                    exeRes.Status = false;
                    exeRes.Message = " The item_group the old reel partno is 0 ";
                    return exeRes;
                }

                sql = @"SELECT item_group  
                        FROM SAJET.P_WO_BOM 
                        where work_order = @WO and ITEM_IPN = @newIPN  and  item_group != '0' and item_group in 
                        (SELECT distinct item_group FROM SAJET.P_WO_BOM where work_order =@WO and ITEM_IPN = @oldIPN  and  item_group != '0')";
               
                dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter>
                { new SugarParameter("@WO", wo),
                  new SugarParameter("@newIPN", newReelPn),
                  new SugarParameter("@oldIPN", oldReelPn)
                });
                if (dt.Rows.Count > 0)
                {
                    exeRes.Status = true;
                    return exeRes;
                }
                exeRes.Status = false;
                exeRes.Message = " The old reel part is inconsistent with the new reel part or there is no substitute material relationship";

            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> CheckReelNo(string reelno)
        {
            try
            {
                exeRes = new ExecuteResult();

                string sql = @"select ipn, datecode, vendor, lot  FROM SAJET.P_MATERIAL where reel_no= '" + reelno + "'  ";
                exeRes.Anything = await Context.Ado.GetDataTableAsync(sql);
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> SaveReplaceReelDetail(RepairInfo repairInfo, string recID, string empno, string time)
        {
            try
            {
                exeRes = new ExecuteResult();
                foreach (RepairDetail repairDetail in repairInfo.RepairDetails)
                {
                    string sqlStr = @"insert INTO SAJET.P_SN_REPAIR_REPLACE_REEL RECID,SERIAL_NUMBER,REPLACE_EMPNO,REPLACE_TIME,OLD_REEL_IPN,
OLD_REEL_NO,OLD_REEL_SN,OLD_REEL_DATECODE,OLD_REEL_LOTCODE,NEW_REEL_IPN,NEW_REEL_NO,NEW_REEL_SN,NEW_REEL_DATECODE,NEW_REEL_LOTCODE,
option1,option2,option3) 
                VALUES(@RECID,@SERIAL_NUMBER,@REPLACE_EMPNO,@REPLACE_TIME,@OLD_REEL_IPN,@OLD_REEL_NO,@OLD_REEL_SN,@OLD_REEL_DATECODE,@OLD_REEL_LOTCODE,
@NEW_REEL_IPN,@NEW_REEL_NO,@NEW_REEL_SN,@NEW_REEL_DATECODE,@NEW_REEL_LOTCODE, @option1,@option2,@option3)";
                  
                    await Context.Ado.ExecuteCommandAsync(sqlStr, new List<SugarParameter>
                    {
                new SugarParameter("@RECID", recID),
                new SugarParameter("@SERIAL_NUMBER", repairInfo.Sn),
                new SugarParameter("@REPLACE_EMPNO", empno),
                new SugarParameter("@REPLACE_TIME", time),
                new SugarParameter("@OLD_REEL_IPN",  repairDetail.Old_Cpn),
                new SugarParameter("@OLD_REEL_NO",  repairDetail.Old_Reel),
                new SugarParameter("@OLD_REEL_SN",  repairDetail.Old_Csn),
                new SugarParameter("@OLD_REEL_DATECODE",  repairDetail.Datecode),
                new SugarParameter("@OLD_REEL_LOTCODE",  repairDetail.Lotcode),
                new SugarParameter("@NEW_REEL_IPN",  repairDetail.New_Cpn),
                new SugarParameter("@NEW_REEL_NO",  repairDetail.New_Reel),
                new SugarParameter("@NEW_REEL_SN",  repairDetail.New_Csn),
                new SugarParameter("@NEW_REEL_DATECODE",   repairDetail.Datecode1),
                new SugarParameter("@NEW_REEL_LOTCODE",  repairDetail.Lotcode1),
                new SugarParameter("@option1",  repairDetail.Action),
                new SugarParameter("@option2",  repairDetail.Location),
                new SugarParameter("@option3",  repairDetail.Remark1)
                    });
                }
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

    }
}
