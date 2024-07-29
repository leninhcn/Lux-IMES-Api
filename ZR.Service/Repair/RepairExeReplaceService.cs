using Infrastructure.Attribute;
using JinianNet.JNTemplate;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Macs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Service.Repair.IRepairService;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class RepairExeReplaceService : BaseService<SnStatus>, IRepairExeReplaceService
    {
        DataTable dtTemp = new DataTable();

        public async Task<DataTable> getKpsn(string sn)
        {
            string sSQL = @"SELECT distinct A.WORK_ORDER,A.ITEM_IPN,A.ITEM_GROUP,A.STATION_TYPE,A.ITEM_SN,A.ITEM_SN_CUSTOMER,B.PART_TYPE
                FROM  IMES.P_SN_KEYPARTS A 
                LEFT JOIN  IMES.M_SN_FEATURE B ON A.ITEM_IPN = B.IPN
                WHERE A.SERIAL_NUMBER = '" + sn + "'  and B.PART_TYPE  in ('SK','SL')  ORDER BY A.ITEM_IPN";

            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<ExecuteResult> CheckNewKeypart(string sn, string oldkpsn, string parttype, string newkpsn)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);
                var tStatus = new SugarParameter("T_STATUS", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_NEWKPSN",
                new SugarParameter[]
                    {
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_OLDKPSN", oldkpsn),
                    new SugarParameter("T_PartType", parttype),
                    new SugarParameter("T_NEWKPSN", newkpsn),
                    tStatus,
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
                    exeRes.Message = tStatus.Value.ToString();

                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }

            return exeRes;
        }

        public async Task<DataTable> getDefect(string defect)
        {
            string sSQL = @"select defect_code,defect_desc,id from IMES.M_DEFECT where defect_code='" + defect + "' and enabled='Y'";

            return await Context.Ado.GetDataTableAsync(sSQL);

        }

        public async Task<ExecuteResult> ReplaceKpsn(string station, string sn, string recid, string oldkpsn, string oldpartno, string newkpsn, string newpartno, string newitemGroup, string kpflag, string kpdefect_data, string remark, string EMPNO, string KP_CUSTOMERSN, string KP_MAC, string lotcode, string datecode)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                // 添加 lotcode, datecode
                var tRes = new SugarParameter("TRES", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_REPAIR_REPLACE_KP",
                new SugarParameter[]
                   {
                    new SugarParameter("T_STATION", station),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_DEFECT_RECID", recid ),
                    new SugarParameter("T_OLD_KPSN", oldkpsn),
                    new SugarParameter("T_OLD_PART", oldpartno),
                    new SugarParameter("T_NEW_KPSN", newkpsn),
                    new SugarParameter("T_NEW_PART", newpartno),
                    new SugarParameter("T_NEW_ITEMGROUP", newitemGroup),
                    new SugarParameter("T_KP_FLAG", kpflag),
                    new SugarParameter("T_KPDEFECT_DATA", kpdefect_data),
                    new SugarParameter("T_REMARK", remark),
                    new SugarParameter("T_EMPNO", EMPNO),
                    new SugarParameter("T_KP_CUSTOMERSN", KP_CUSTOMERSN),
                    new SugarParameter("T_KP_MAC", KP_MAC),
                    new SugarParameter("T_LotCode", lotcode),
                    new SugarParameter("T_dateCode", datecode),
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
                    exeRes.Message = sRes;
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }


        public async Task<bool> InsertReplaceKP(string recid, string station, string sn, string oldkpsn, string oldpartno, string newkpsn, string newpartno, string remark, string EMPNO, string lotcode, string datecode, string rid)
        {
            string sSQL = @"INSERT INTO IMES.P_SN_REPAIR_REPLACE_KP (recid,serial_number,REPLACE_EMPNO,replace_time,ITEM_IPN,OLD_IPN_SN,NEW_IPN_SN,NEW_IPN,remark,flag,lot_Code,date_Code,rid)
              VALUES ('" + recid + "','" + sn + "','" + EMPNO + "',SYSDATE,'" + oldpartno + "','" + oldkpsn + "','" + newkpsn + "','" + station + "','" + remark + "','Y','" + lotcode + "','" + datecode + "','" + rid + "') ";
            
            var affected = await Context.Ado.ExecuteCommandAsync(sSQL);

            return affected > 0;
        }

        public async Task<ExecuteResult> RemoveKp(string station, string sn, string recid, string kpsn, string partno, string kpflag, string defect_data,string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_REPAIR_REMOVE_KP",
                new SugarParameter[]
                   {
                    new SugarParameter("T_STATION_NAME", station),
                    new SugarParameter("TSN", sn),
                    new SugarParameter("TDEFECT_RECID", recid ),
                    new SugarParameter("TKPSN", kpsn),
                    new SugarParameter("TPARTNO", partno),
                    new SugarParameter("TKPFLAG", kpflag),
                    new SugarParameter("TKPDEFECT_DATA", defect_data),
                    new SugarParameter("TEMPNO", userNo),
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
                    exeRes.Message = sRes;
                }
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
