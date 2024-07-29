using Infrastructure.Attribute;
using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Quality;
using ZR.Model.Repair.Dto;
using ZR.Model.System.Dto;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class RepairExeService : BaseService<SnStatus>, IRepairExeService
    {
        DataTable dtTemp = new DataTable();

        public async Task<DataTable> getEmpName(string empNo)
        {
            string sqlstr = @"SELECT EMP_NO,EMP_NAME FROM imes.m_emp where EMP_NO='" + empNo + "'  AND ENABLED ='Y' AND ROWNUM=1";
            return await Context.Ado.GetDataTableAsync(sqlstr);
        }
        
        public async Task<DataTable> checkstatus(string snno)
        {
            string getHold = string.Format(@" SELECT  *  FROM imes.p_sn_status WHERE serial_number= '{0}' ", snno);
            return await Context.Ado.GetDataTableAsync(getHold);
        }

        public async Task<ExecuteResult> CheckSN(string sn)
        {
            ExecuteResult exeRes = new ExecuteResult();

            try
            {
                var tRes = new SugarParameter("TRES", null, true);
                var tPsn = new SugarParameter("PSN", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_SN_PSN",
                new SugarParameter[]
                    {
                    new SugarParameter("TREV", sn),
                    tPsn,
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
                    exeRes.Anything = tPsn.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }

            return exeRes;
        }

        public async Task<ExecuteResult> CheckRoute(string station_name, string sn)
        {
            ExecuteResult exeRes = new ExecuteResult();

            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_ROUTE_REPAIR",
                new SugarParameter[]
                    {
                    new SugarParameter("T_STATIONNAME", station_name),
                    new SugarParameter("T_SN", sn),
                    tRes
                    });

                string sRes = tRes.Value.ToString();
                if (sRes != "OK")
                {
                    exeRes.Status = false;
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

        public async Task<DataTable> getDetail(string sNInfo)
        {
            string sSQL = @"SELECT A.WORK_ORDER,A.VERSION,
                       TO_CHAR (A.OUT_STATIONTYPE_TIME, 'yyyy/mm/dd hh24:mi:ss')
                           OUT_PROCESS_TIME,
                       a.route_name,
                       A.IPN,
                       A.STATION_TYPE
                  FROM imes.p_SN_STATUS A WHERE A.SERIAL_NUMBER = '" + sNInfo + "' AND ROWNUM = 1";

            return await Context.Ado.GetDataTableAsync(sSQL);
        }


        public async Task<DataTable> getStep(repairExe baseInfo)
        {
            string sSQL = @"SELECT STEP FROM IMES.M_ROUTE_DETAIL  WHERE ROUTE_NAME='" + baseInfo.routName + "' AND STATION_TYPE='" + baseInfo.CurrentStationType + "' AND NEXT_STATION_TYPE='" + baseInfo.stationType + "' AND ROWNUM=1";
           
            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<DataTable> getDefect(string sSN)
        {
            string sSQL = "";
            sSQL = @" SELECT A.RECID,
                                 A.Location,
                                 A.RP_STATUS,
                                 B.DEFECT_CODE,
                                 B.DEFECT_DESC,
                                 B.DEFECT_DESC2,
                                 a.LINE,
                                 a.station_name,
                                 a.station_type,
                                 NVL (G.REASON_CODE, 'N/A') REASON_CODE
                            FROM imes.p_SN_DEFECT A,
                                 imes.m_DEFECT   B,
                                 imes.p_SN_REPAIR F,
                                 imes.m_REASON   G
                           WHERE     A.SERIAL_NUMBER = '" + sSN + @"' AND NVL (rp_status, 1) <> '0' AND  A.DEFECT_code = B.DEFECT_code AND A.RECID = F.RECID(+) AND F.REASON_code = G.REASON_code(+) ORDER BY B.Defect_Code";

            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<DataTable> GetRepair1(string sn, string code, string id)
        {
            string sSQL = @" select A.RP_STATUS,B.ENABLED from  IMES.P_SN_DEFECT a,IMES.P_SN_REPAIR_DETAIL B  where A.RECID=B.RECID AND  a.SERIAL_NUMBER='" + sn + "'" +
                "and a.defect_code='" + code + "'AND  B.RECID='" + id + "' AND ROWNUM<2 ORDER BY b.CREATE_TIME DESC";
           
            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<DataTable> getDefectReasonType()
        {
            string sSQL = @"SELECT DISTINCT A.REASON_TYPE FROM IMES.M_REASON A WHERE  A.ENABLED = 'Y' ";

            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<DataTable> getDefectReason(string resonType)
        {
            string sSQL = @"SELECT DISTINCT A.REASON_TYPE, A.REASON_CODE, A.REASON_DESC, A.REASON_DESC2   FROM IMES.M_REASON A Where A.enabled = 'Y'  ";
            if (!string.IsNullOrEmpty(resonType))
            {
                sSQL = sSQL + "and A.REASON_TYPE like '" + resonType + "%'";
            }
            sSQL = sSQL + "order by A.REASON_CODE ";

            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<DataTable> getLocation(string sn, string code)
        {
            string sSQL = @" SELECT LOCATION FROM  IMES.P_SN_DEFECT WHERE SERIAL_NUMBER='" + sn + "'  and  DEFECT_CODE='" + code + "' AND (RP_STATUS = '1' or RP_STATUS IS NULL) ORDER BY REC_TIME DESC";
          
            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<DataTable> GetKPReplaceInfo(string recid, string sn)
        {
            string sSQL = string.Format(@" SELECT ITEM_IPN,OLD_IPN_SN,NEW_IPN_SN,NEW_IPN, REPLACE_EMPNO,REPLACE_TIME FROM imes.P_SN_REPAIR_REPLACE_KP WHERE RECID= '{0}' AND serial_number= '{1}' AND REMARK is null ", recid, sn);
            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<DataTable> Checkoldsn(string sn, string oldsn)
        {
            string sSQL = @" SELECT C.item_sn  FROM IMES.P_SN_KEYPARTS C WHERE  C.SERIAL_NUMBER = '" + sn + "'" +
                 "AND C.ITEM_SN = '" + oldsn + "' ";
            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<DataTable> Checkparttype(string sn, string oldsn)
        {
            string sSQL = @" SELECT b.part_type  FROM IMES.P_WO_BOM A,IMES.M_SN_FEATURE B,IMES.P_SN_KEYPARTS C
                 WHERE A.ITEM_IPN = B.IPN AND A.WORK_ORDER = C.WORK_ORDER AND A.ITEM_IPN = C.ITEM_IPN AND C.SERIAL_NUMBER = '" + sn + "'" +
                 "AND C.ITEM_SN = '" + oldsn + "' AND B.ENABLED='Y' ";
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

        public async Task<ExecuteResult> CheckReelNo(string reelno)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sSQL = @"select ipn, datecode, vendor, lot  from IMES.P_MATERIAL where reel_no= '" + reelno + "'  ";
                exeRes.Anything = await Context.Ado.GetDataTableAsync(sSQL);
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<bool> InsertReelNo(string ReelNo, string editPartNo, string editDateCode, string txtUnitQty, string editVendor, string editLot,string userNo)
        {
            string sSQL = @"Insert Into IMES.P_MATERIAL (IPN,DATECODE,MATERIAL_NO,REEL_NO,REEL_QTY,VENDOR,LOT,UPDATE_USERID,UPDATE_TIME,RT_RECEIVE_TIME,REEL_MEMO,EXP_DATE,REMARK)
              Values (@IPN,@DATECODE,@MATERIAL_NO,@REEL_NO,@REEL_QTY,@VENDOR,@LOT,@UPDATE_USERID,@UPDATE_TIME,@RT_RECEIVE_TIME,@REEL_MEMO,@EXP_DATE,@REMARK)   ";
            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@IPN", editPartNo),
                new SugarParameter("@DATECODE", editDateCode),
                new SugarParameter("@MATERIAL_NO", "电子料"),
                new SugarParameter("@REEL_NO", ReelNo),
                new SugarParameter("@REEL_QTY", txtUnitQty),
                new SugarParameter("@VENDOR", editVendor),
                new SugarParameter("@LOT", editLot),
                new SugarParameter("@UPDATE_USERID", userNo),
                new SugarParameter("@UPDATE_TIME", DateTime.Now),
                new SugarParameter("@RT_RECEIVE_TIME", DateTime.Now),
                new SugarParameter("@REEL_MEMO", "原厂标签"),
                new SugarParameter("@EXP_DATE",  DateTime.Now.AddDays(60)),
                new SugarParameter("@REMARK", "1")
            });

            return affected > 0;
        }

        public async Task<bool> InsertReelNo_HT(string ReelNo, string editPartNo, string editDateCode, string txtUnitQty, string editVendor, string editLot,string userNo)
        {
            string sSQL = @"Insert Into IMES.P_MATERIAL_HT (IPN,DATECODE,MATERIAL_NO,REEL_NO,REEL_QTY,VENDOR,LOT,UPDATE_USERID,UPDATE_TIME,RT_RECEIVE_TIME,REEL_MEMO,EXP_DATE,REMARK) Values 
                   (@IPN,@DATECODE,@MATERIAL_NO,@REEL_NO,@REEL_QTY,@VENDOR,@LOT,@UPDATE_USERID,@UPDATE_TIME,@RT_RECEIVE_TIME,@REEL_MEMO,@EXP_DATE,@REMARK) ";

            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@IPN", editPartNo),
                new SugarParameter("@DATECODE", editDateCode),
                new SugarParameter("@MATERIAL_NO", "电子料"),
                new SugarParameter("@REEL_NO", ReelNo),
                new SugarParameter("@REEL_QTY", txtUnitQty),
                new SugarParameter("@VENDOR", editVendor),
                new SugarParameter("@LOT", editLot),
                new SugarParameter("@UPDATE_USERID", userNo),
                new SugarParameter("@UPDATE_TIME", DateTime.Now),
                new SugarParameter("@RT_RECEIVE_TIME", DateTime.Now),
                new SugarParameter("@REEL_MEMO", "原厂标签"),
                new SugarParameter("@EXP_DATE",  DateTime.Now.AddDays(60)),
                new SugarParameter("@REMARK", "1")
            });

            return affected > 0;
        }
        public async Task<bool> InsertReelNoSMT(string ReelNo, string editPartNo, string editDateCode, string txtUnitQty, string editVendor, string editLot,string userNo)
        {
            string sSQL = @" INSERT INTO ISMT.P_MATERIAL_SN (ID,
                            MODEL_SN,
                            VENDOR_CODE,
                            MODEL_TYPE,
                            DATE_CODE,
                            LOT_CODE，
                            CREATE_EMPNO,
                            OPTION1,
                            OPTION2,
                            OPTION3,OPTION4,OPTION5,OPTION6)
                         VALUES (ISMT.SEQ_P_MATERIAL_SN_ID.nextval,
                                 @MODEL_SN,
                                 @VENDOR_CODE,
                                 @MODEL_TYPE,
                                 @DATE_CODE,
                                 @LOT_CODE,
                                 @CREATE_EMPNO,
                                 @OPTION1,
                                 @OPTION2,
                                 @OPTION3,@OPTION4,@OPTION5,@OPTION6)";

            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@MODEL_SN", ReelNo),
                new SugarParameter("@VENDOR_CODE", editVendor),
                new SugarParameter("@MODEL_TYPE", editPartNo),
                new SugarParameter("@DATE_CODE", editDateCode),
                new SugarParameter("@LOT_CODE", editLot),
                new SugarParameter("@CREATE_EMPNO", userNo),
                new SugarParameter("@OPTION1", "电子料"),
                new SugarParameter("@OPTION2", txtUnitQty),
                new SugarParameter("@OPTION3", editPartNo),
                new SugarParameter("@OPTION4", DateTime.Now.AddDays(60)),
                new SugarParameter("@OPTION5", "原厂标签" ),
                new SugarParameter("@OPTION6", "1")
            });

            return affected > 0;
        }

        public async Task<bool> InsertReelNo1(string ReelNo, string editPartNo, string editDateCode, string txtUnitQty, string editVendor, string editLot, string msd,string userNo)
        {
            string sSQL = @"Insert Into IMES.P_MATERIAL (IPN,DATECODE,MATERIAL_NO,REEL_NO,REEL_QTY,VENDOR,LOT,UPDATE_USERID,UPDATE_TIME,RT_RECEIVE_TIME,REEL_MEMO,EXP_DATE,REMARK)
                      Values (@IPN,@DATECODE,@MATERIAL_NO,@REEL_NO,@REEL_QTY,@VENDOR,@LOT,@UPDATE_USERID,@UPDATE_TIME,@RT_RECEIVE_TIME,@REEL_MEMO,@EXP_DATE,@REMARK) ";
          
            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@IPN", editPartNo),
                new SugarParameter("@DATECODE", editDateCode),
                new SugarParameter("@MATERIAL_NO", "电子料"),
                new SugarParameter("@REEL_NO", ReelNo),
                new SugarParameter("@REEL_QTY", txtUnitQty),
                new SugarParameter("@VENDOR", editVendor),
                new SugarParameter("@LOT", editLot),
                new SugarParameter("@UPDATE_USERID", userNo),
                new SugarParameter("@UPDATE_TIME", DateTime.Now),
                new SugarParameter("@RT_RECEIVE_TIME", DateTime.Now),
                new SugarParameter("@REEL_MEMO", "原厂标签"),
                new SugarParameter("@EXP_DATE",  DateTime.Now.AddDays(60)),
                new SugarParameter("@REMARK", msd)
            });

            return affected > 0;
        }
       
       public  async Task<bool> InsertReelNo1_HT(string ReelNo, string editPartNo, string editDateCode, string txtUnitQty, string editVendor, string editLot, string msd,string userNo)
       {
           string sSQL = @"Insert Into IMES.P_MATERIAL_HT (IPN,DATECODE,MATERIAL_NO,REEL_NO,REEL_QTY,VENDOR,LOT,UPDATE_USERID,UPDATE_TIME,RT_RECEIVE_TIME,REEL_MEMO,EXP_DATE,REMARK) 
                   Values (@IPN,@DATECODE,@MATERIAL_NO,@REEL_NO,@REEL_QTY,@VENDOR,@LOT,@UPDATE_USERID,@UPDATE_TIME,@RT_RECEIVE_TIME,@REEL_MEMO,@EXP_DATE,@REMARK) ";
          
            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@IPN", editPartNo),
                new SugarParameter("@DATECODE", editDateCode),
                new SugarParameter("@MATERIAL_NO", "电子料"),
                new SugarParameter("@REEL_NO", ReelNo),
                new SugarParameter("@REEL_QTY", txtUnitQty),
                new SugarParameter("@VENDOR", editVendor),
                new SugarParameter("@LOT", editLot),
                new SugarParameter("@UPDATE_USERID", userNo),
                new SugarParameter("@UPDATE_TIME", DateTime.Now),
                new SugarParameter("@RT_RECEIVE_TIME", DateTime.Now),
                new SugarParameter("@REEL_MEMO", "原厂标签"),
                new SugarParameter("@EXP_DATE",  DateTime.Now.AddDays(60)),
                new SugarParameter("@REMARK", msd)
            });

            return affected > 0;
        }

       public  async Task<bool> InsertReelNoSMT1(string ReelNo, string editPartNo, string editDateCode, string txtUnitQty, string editVendor, string editLot, string msd,string userNo)
       {
           string sSQL = @" INSERT INTO ISMT.P_MATERIAL_SN (ID,
                           MODEL_SN,
                           VENDOR_CODE,
                           MODEL_TYPE,
                           DATE_CODE,
                           LOT_CODE，
                           CREATE_EMPNO,
                           OPTION1,
                           OPTION2,
                           OPTION3,OPTION4,OPTION5,OPTION6)
                        VALUES (ISMT.SEQ_P_MATERIAL_SN_ID.nextval,
                                @MODEL_SN,
                                @VENDOR_CODE,
                                @MODEL_TYPE,
                                @DATE_CODE,
                                @LOT_CODE,
                                @CREATE_EMPNO,
                                @OPTION1,
                                @OPTION2,
                                @OPTION3,@OPTION4,@OPTION5,@OPTION6)";
            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@MODEL_SN", ReelNo),
                new SugarParameter("@VENDOR_CODE", editVendor),
                new SugarParameter("@MODEL_TYPE", editPartNo),
                new SugarParameter("@DATE_CODE", editDateCode),
                new SugarParameter("@LOT_CODE", editLot),
                new SugarParameter("@CREATE_EMPNO", userNo),
                new SugarParameter("@OPTION1", "电子料"),
                new SugarParameter("@OPTION2", txtUnitQty),
                new SugarParameter("@OPTION3", editPartNo),
                new SugarParameter("@OPTION4", DateTime.Now.AddDays(60)),
                new SugarParameter("@OPTION5", "原厂标签" ),
                new SugarParameter("@OPTION6", msd)
            });

            return affected > 0;
        }

       public async Task<DataTable> CheckReason(string reason)
        {
            string sSQL = @" SELECT REASON_CODE,REASON_DESC,REASON_DESC2 FROM  IMES.M_REASON WHERE REASON_CODE='" + reason + "' AND ENABLED='Y'";

            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<ExecuteResult> RepairSnReason(string sn, string wo, string ipn, string recid, string reason, string reasontype, string duty, string station_name,string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_REPAIR_REASON",
                new SugarParameter[]
                    {
                    new SugarParameter("TSN", sn),
                    new SugarParameter("TWO", wo),
                    new SugarParameter("T_IPN", ipn),
                    new SugarParameter("TRECID", recid),
                    new SugarParameter("T_REASON_CODE", reason),
                    new SugarParameter("T_REASON_TYPE", reasontype),
                    new SugarParameter("TDUTYID", duty),
                    new SugarParameter("T_EMPNO", userNo),
                    new SugarParameter("T_STATION_NAME", station_name),
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

        public async Task<ExecuteResult> SaveRepairDetail(RepairInfo repairInfo, string recID, string ipn, string oldsn, string newsn, string empno, string time, string station,string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                foreach (RepairDetail repairDetail in repairInfo.RepairDetails)
                {
                    string sqlStr = @"insert into IMES.P_SN_REPAIR_DETAIL(RECID, SERIAL_NUMBER, CASETYPE, STATION, REMARK, DEFECTSSYM, OLD_CSN, NEW_CSN, OLD_CPN, NEW_CPN, LOCATION, VENDOR, DATECODE, LOTCODE, REMARK1,OPTION1,OPTION2,OPTION3,OPTION4,OPTION5,OPTION6,OPTION7,OPTION8,OPTION9, UPDATE_EMPNO, CREATE_EMPNO) 
                VALUES(@RECID, @SERIAL_NUMBER, @CASETYPE, @STATION, @REMARK, @DEFECTSSYM, @OLD_CSN, @NEW_CSN, @OLD_CPN, @NEW_CPN, @LOCATION, @VENDOR, @DATECODE, @LOTCODE,
                         @REMARK1,@OPTION1,@OPTION2,@OPTION3,@OPTION4,@OPTION5,@OPTION6,@OPTION7,@OPTION8,@OPTION9,@UPDATE_EMPNO, @CREATE_EMPNO)";

                    await Context.Ado.ExecuteCommandAsync(sqlStr, new List<SugarParameter>
                    {
                new SugarParameter("@RECID", recID),
                new SugarParameter("@SERIAL_NUMBER", repairInfo.Sn),
                new SugarParameter("@CASETYPE", repairInfo.Casetype),
                new SugarParameter("@STATION", repairInfo.Station),
                new SugarParameter("@REMARK",  repairInfo.Remark),
                new SugarParameter("@DEFECTSSYM", repairInfo.DefectsSym),
                new SugarParameter("@OLD_CSN", oldsn),
                new SugarParameter("@NEW_CSN", newsn),
                new SugarParameter("@OLD_CPN",ipn),
                new SugarParameter("@NEW_CPN", ipn),
                new SugarParameter("@LOCATION", repairDetail.Location),
                new SugarParameter("@VENDOR", repairDetail.Vendor),
                new SugarParameter("@DATECODE", repairDetail.Datecode),
                new SugarParameter("@LOTCODE", repairDetail.Lotcode),
                new SugarParameter("@REMARK1", repairDetail.Remark1),
                new SugarParameter("@OPTION1", repairDetail.Vendor1),
                new SugarParameter("@OPTION2",repairDetail.Datecode1),
                new SugarParameter("@OPTION3",repairDetail.Lotcode1),
                new SugarParameter("@OPTION4",repairDetail.Action),
                new SugarParameter("@OPTION5",repairDetail.Old_Reel),
                new SugarParameter("@OPTION6",repairDetail.New_Reel),
                new SugarParameter("@OPTION7",empno),
                new SugarParameter("@OPTION8",time),
                new SugarParameter("@OPTION9",station),
                new SugarParameter("@UPDATE_EMPNO",userNo),
                new SugarParameter("@CREATE_EMPNO", userNo)
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

        public async Task<ExecuteResult> SaveRepairDetail1(RepairInfo repairInfo, string recID, string empno, string time, string station,string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                foreach (RepairDetail repairDetail in repairInfo.RepairDetails)
                {
                    string sqlStr = @"insert into IMES.P_SN_REPAIR_DETAIL(RECID, SERIAL_NUMBER, CASETYPE, STATION, REMARK, DEFECTSSYM, OLD_CSN, NEW_CSN, OLD_CPN, NEW_CPN, LOCATION, VENDOR, DATECODE, LOTCODE, REMARK1,OPTION1,OPTION2,OPTION3,OPTION4,OPTION5,OPTION6,OPTION7,OPTION8,OPTION9, UPDATE_EMPNO, CREATE_EMPNO) 
                VALUES(@RECID, @SERIAL_NUMBER, @CASETYPE, @STATION, @REMARK, @DEFECTSSYM, @OLD_CSN, @NEW_CSN, @OLD_CPN, @NEW_CPN, @LOCATION, @VENDOR, @DATECODE, @LOTCODE, @REMARK1,@OPTION1,@OPTION2,@OPTION3,@OPTION4,@OPTION5,@OPTION6,@OPTION7,@OPTION8,@OPTION9,@UPDATE_EMPNO, @CREATE_EMPNO)";
                   
                    await Context.Ado.ExecuteCommandAsync(sqlStr, new List<SugarParameter>
                    {
                new SugarParameter("@RECID", recID),
                new SugarParameter("@SERIAL_NUMBER", repairInfo.Sn),
                new SugarParameter("@CASETYPE", repairInfo.Casetype),
                new SugarParameter("@STATION", repairInfo.Station),
                new SugarParameter("@REMARK",  repairInfo.Remark),
                new SugarParameter("@DEFECTSSYM", repairInfo.DefectsSym),
                new SugarParameter("@OLD_CSN", repairDetail.Old_Csn),
                new SugarParameter("@NEW_CSN", repairDetail.New_Csn),
                new SugarParameter("@OLD_CPN", repairDetail.Old_Cpn),
                new SugarParameter("@NEW_CPN",  repairDetail.New_Cpn),
                new SugarParameter("@LOCATION", repairDetail.Location),
                new SugarParameter("@VENDOR", repairDetail.Vendor),
                new SugarParameter("@DATECODE", repairDetail.Datecode),
                new SugarParameter("@LOTCODE", repairDetail.Lotcode),
                new SugarParameter("@REMARK1", repairDetail.Remark1),
                new SugarParameter("@OPTION1", repairDetail.Vendor1),
                new SugarParameter("@OPTION2",repairDetail.Datecode1),
                new SugarParameter("@OPTION3",repairDetail.Lotcode1),
                new SugarParameter("@OPTION4",repairDetail.Action),
                new SugarParameter("@OPTION5",repairDetail.Old_Reel),
                new SugarParameter("@OPTION6",repairDetail.New_Reel),
                new SugarParameter("@OPTION7",empno),
                new SugarParameter("@OPTION8",time),
                new SugarParameter("@OPTION9",station),
                new SugarParameter("@UPDATE_EMPNO",userNo),
                new SugarParameter("@CREATE_EMPNO", userNo)
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

        public async Task<DataTable> GetDefectCodeInfo(string sDefectCode)
        {
            string sSQL = @"select a.DEFECT_DESC,a.DEFECT_DESC2 from IMES.M_DEFECT a where a.DEFECT_CODE = @DEFECT_CODE and a.ENABLED = 'Y'";

            return await Context.Ado.GetDataTableAsync(sSQL, new List<SugarParameter>
                    { new SugarParameter("@DEFECT_CODE", sDefectCode) });

        }

        public async Task<string> GetDefectRECID()
        {
            string sID = "0";

            string sSQL = @" SELECT RPAD(NVL(PARAM_VALUE, '1'), 2, '0') || TO_CHAR(SYSDATE, 'YYMMDD') || LPAD(IMES.S_DEF_CODE.NEXTVAL, 5, '0') SNID
                    FROM IMES.S_BASE
                   WHERE PARAM_NAME = 'DBID' ";
            DataTable dtTemp = await Context.Ado.GetDataTableAsync(sSQL);
            sID = dtTemp.Rows[0][0].ToString();
            return sID;
        }

        public async Task<bool> AddDefectInfo(string sRecID, string userNo, repairDef repDef)
        {
            string sSQL = @"INSERT INTO IMES.P_SN_DEFECT 
(RECID, SERIAL_NUMBER, WORK_ORDER, IPN, REC_TIME, DEFECT_CODE, LOCATION, STATION_NAME, STATION_TYPE, STAGE, LINE, RECEIVE_EMPNO,TEST_EMPNO,RP_STATUS) 
VALUES(@RECID,@SN,@WO,@IPN,sysdate,@DEFCODE,@LOCATION,@STATION_NAME,@STATION_TYPE,@STAGE,@LINE,@REC_EMPNO,@TEST_EMPNO,'1')";
           
            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@RECID", sRecID),
                new SugarParameter("@SN", repDef.sn),
                new SugarParameter("@WO", repDef.wo),
                new SugarParameter("@IPN", repDef.partNo),
                new SugarParameter("@DEFCODE", repDef.sDefCode),
                new SugarParameter("@LOCATION", repDef.sLocation),
                new SugarParameter("@STATION_NAME", repDef.stationName),
                new SugarParameter("@STATION_TYPE", repDef.stationType),
                new SugarParameter("@STAGE", repDef.stageName),
                new SugarParameter("@LINE", repDef.lineName),
                new SugarParameter("@REC_EMPNO", userNo),
                new SugarParameter("@TEST_EMPNO", userNo)
            });

            return affected > 0;
        }

        public async Task<bool> DeleteDefectByRecid(string sRECID)
        {
            string sSQL = @" Delete IMES.P_SN_REPAIR Where RECID = '" + sRECID + "'";

            var affected = await Context.Ado.ExecuteCommandAsync(sSQL);

            sSQL = " Delete IMES.P_SN_DEFECT   Where RECID = '" + sRECID + "'";
            affected = await Context.Ado.ExecuteCommandAsync(sSQL);

            return affected > 0;
        }

        public async Task<DataTable> GetReturnStation(string sn, string process)
        {
            string sSQL = string.Format(@"SELECT D.NEXT_STATION_TYPE 
                                        FROM IMES.P_SN_STATUS C, IMES.M_ROUTE_DETAIL D ,
                                             (SELECT B.STEP ,B.SEQ
                                                FROM IMES.P_SN_STATUS A, IMES.M_ROUTE_DETAIL B
                                                WHERE A.SERIAL_NUMBER = '{0}' 
                                                AND A.ROUTE_NAME = B.ROUTE_NAME
                                                AND A.STATION_TYPE = B.STATION_TYPE
                                                AND B.NEXT_STATION_TYPE = '{1}'
                                                ORDER BY B.STEP,B.SEQ)E
                                        WHERE 
                                            D.STEP = E.STEP
                                        AND C.SERIAL_NUMBER = '{2}' 
                                        AND C.ROUTE_NAME = D.ROUTE_NAME
                                        AND D.STATION_TYPE = '{3}'
                                        ORDER BY D.STEP,D.SEQ", sn, process, sn, process);
            dtTemp = await Context.Ado.GetDataTableAsync(sSQL);
            return dtTemp;
        }

        public async Task<ExecuteResult> RepairGo(string station, string sn, string nstation,string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var tRes = new SugarParameter("TRES", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_REPAIR_GO_CHECK",
                new SugarParameter[]
                    {
                    new SugarParameter("T_STATION_NAME", station),                  
                    new SugarParameter("TSN", sn),
                    new SugarParameter("TEMPNO", userNo),
                    new SugarParameter("T_NEXT_STATION_TYPE", nstation),
                    tRes
                    });

                string sRes = tRes.Value.ToString();
                if (sRes.Substring(0, 2) != "OK")
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

        public async Task<bool> AddScrapInfo(ScrapDto scrapDto,string userNo)
        {
            string sSQL = @"INSERT INTO IMES.P_SN_SCRAP 
( WORK_ORDER, IPN,VERSION,SERIAL_NUMBER, STATION_NAME, SCRAP_MEMO, SCRAP_TYPE, UPDATE_EMPNO,UPDATE_TIME,CREATE_TIME, LINE, STATION_TYPE,ENABLED,OPTION1) 
VALUES(@wo,@IPN,@VERSION,@SERIAL_NUMBER,@STATION_NAME,@SCRAP_MEMO,'S',@UPDATE_EMPNO,sysdate,sysdate,@LINE,@STATION_TYPE,'Y',@OPTION1)";
           
            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@wo", scrapDto.wo),
                new SugarParameter("@IPN", scrapDto.sn),
                new SugarParameter("@VERSION", scrapDto.version),
                new SugarParameter("@SERIAL_NUMBER", scrapDto.sn),
                new SugarParameter("@STATION_NAME", scrapDto.stationName),
                new SugarParameter("@SCRAP_MEMO", scrapDto.scrapMemo),
                new SugarParameter("@UPDATE_EMPNO", userNo),
                new SugarParameter("@LINE", scrapDto.defectLine),
                new SugarParameter("@STATION_TYPE", scrapDto.stationType),
                new SugarParameter("@OPTION1", scrapDto.scrapType),
            });

            return affected > 0;
        }

        public async Task<bool> UpdateSnStatus(string sn,string userNo)
        {
            string sSQL = @"UPDATE IMES.P_SN_STATUS SET CURRENT_STATUS='S',WORK_FLAG=1 ,WIP_STATION_TYPE='SCRAP',NEXT_STATION_TYPE='SCRAP',OUT_STATIONTYPE_TIME=sysdate,EMP_NO=@EMP_NO 
                            WHERE SERIAL_NUMBER=@SN";
           
            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@EMP_NO", userNo),
                new SugarParameter("@SN", sn)
            });

            return affected > 0;
        }

        public async Task<bool> AddSnTravel(string sn)
        {
            string sSQL = @"INSERT INTO IMES.P_SN_TRAVEL SELECT * FROM IMES.P_SN_STATUS WHERE SERIAL_NUMBER=@SN";
            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@SN", sn)
            });

            return affected > 0;
        }

        public async Task<DataTable> getReason(string sn,string sRECID)
        {
                string sSQL = @" SELECT REASON_CODE,
                                REASON_DESC,
                                REASON_DESC2, LOCATION, defect_code,
                                DECODE (SUGGEST_SCRAPE, 'Y', '是', 'N', '否', SUGGEST_SCRAPE) SUGGEST_SCRAPE,
                             REMARK,MIN(SEQ)
                             FROM(
                             SELECT D.REASON_CODE,
                             D.REASON_DESC,
                             D.REASON_DESC2, E.LOCATION, a.defect_code,
                             DECODE(C.SUGGEST_SCRAPE, 'Y', '是', 'N', '否', SUGGEST_SCRAPE) SUGGEST_SCRAPE,
                             C.REMARK, ROW_NUMBER() OVER(PARTITION BY D.REASON_CODE, E.LOCATION, A.DEFECT_CODE  ORDER BY E.CREATE_TIME DESC) SEQ
                        FROM IMES.P_SN_DEFECT A,
                             IMES.M_DEFECT   B,
                             IMES.P_SN_REPAIR C,
                             IMES.M_REASON   D, IMES.P_SN_REPAIR_DETAIL E
                        WHERE     A.SERIAL_NUMBER = '" + sn + "' AND A.DEFECT_code = B.DEFECT_code AND A.RECID = '" + sRECID + "' AND A.RECID = C.RECID  " +
                           "AND C.REASON_code = D.REASON_code AND C.RECID = E.RECID  " +
                           "ORDER BY D.REASON_CODE, D.REASON_DESC ) X " +
                          "GROUP BY REASON_CODE,REASON_DESC,REASON_DESC2, LOCATION, defect_code, SUGGEST_SCRAPE, REMARK";
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sSQL);

                return dtTemp;
        }

    }

}
