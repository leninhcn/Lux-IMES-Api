using Infrastructure.Attribute;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Repair.Dto;
using ZR.Service.Repair.IRepairService;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class RepairDelinkService: BaseService<SnStatus>, IRepairDelinkService
    {
        ExecuteResult exeRes = new ExecuteResult();

        DataTable dtTemp = new DataTable();

        //public ExecuteResult GetValues(string INPUTVALUE, string ITEM, string MODEL_NAME)
        //{
        //    try
        //    {
        //        exeRes = new ExecuteResult();
        //        var para = new object[][]
        //        {
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_INPUT_VALUE", INPUTVALUE },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_ITEM", ITEM },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_MODEL_NAME", MODEL_NAME },
        //            new object[4] { ParameterDirection.Output, ImesOracleType.Varchar2, "T_VALUE", "" },
        //            new object[4] { ParameterDirection.Output, ImesOracleType.Varchar2, "TRES", "" }
        //        };
        //        object[][] obj = utility.ExecuteProc("SAJET.SP_QUERY_VALUE_GROUP", para);

        //        string sRes = obj[4][3].ToString();
        //        if (sRes != "OK")
        //        {
        //            exeRes.Status = false;
        //            exeRes.Message = sRes;
        //        }
        //        else
        //        {
        //            exeRes.Anything = obj[3][3].ToString();
        //            exeRes.Status = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        exeRes.Message = "Error:" + ex.Message;
        //        exeRes.Status = false;
        //    }
        //    return exeRes;
        //}

        //public ExecuteResult CheckValue(string INPUTVALUE, string ITEM, string MODEL_NAME, string Value)
        //{
        //    try
        //    {
        //        exeRes = new ExecuteResult();
        //        var para = new object[][]
        //        {
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_INPUT_VALUE", INPUTVALUE },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_ITEM", ITEM },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_MODEL_NAME", MODEL_NAME },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_CHECK_VALUE", "" },
        //            new object[4] { ParameterDirection.Output, ImesOracleType.Varchar2, "TRES", "" }
        //        };
        //        object[][] obj = utility.ExecuteProc("SAJET.SP_CHECK_VALUE", para);

        //        string sRes = obj[4][3].ToString();
        //        if (sRes != "OK")
        //        {
        //            exeRes.Status = false;
        //            exeRes.Message = sRes;
        //        }
        //        else
        //        {
        //            exeRes.Status = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        exeRes.Message = "Error:" + ex.Message;
        //        exeRes.Status = false;
        //    }
        //    return exeRes;
        //}

        //public ExecuteResult CheckLogic(string SN, string WO, string KPSN, string LINE,
        //    string STATIONTYPE, string STATION, string ROUTE, string STEP)
        //{
        //    try
        //    {
        //        exeRes = new ExecuteResult();
        //        var para = new object[][]
        //        {
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_SN", SN },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_WO", WO },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_KPSN",KPSN },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_LINE", LINE },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_STATIONTYPE", STATIONTYPE },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_STATIONNAME", STATION },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_ROUTE", ROUTE },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_EMP", utility.GlobalUserNo },
        //            new object[4] { ParameterDirection.Input, ImesOracleType.Varchar2, "T_STEP", STEP },
        //            new object[4] { ParameterDirection.Output, ImesOracleType.Varchar2, "TRES", "" }
        //        };
        //        object[][] obj = utility.ExecuteProc("SAJET.SP_CHECK_LOGIC", para);
        //        string sRes = obj[9][3].ToString();
        //        if (sRes != "OK")
        //        {
        //            exeRes.Status = false;
        //            exeRes.Message = sRes;
        //        }
        //        else
        //        {
        //            exeRes.Status = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        exeRes.Message = "Error:" + ex.Message;
        //        exeRes.Status = false;
        //    }
        //    return exeRes;
        //}

        public async Task<ExecuteResult> CheckSN(string sn)
        {
            try
            {
                exeRes = new ExecuteResult();

                var tRes = new SugarParameter("TRES", null, true);
                var tPsn = new SugarParameter("PSN", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_SN_PSN",
                new SugarParameter[]
                    {
                    new SugarParameter("TREV", sn),
                    tPsn,
                    tRes
                    });

                if (tRes.Value.ToString() != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = tRes.Value.ToString();
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

        public async Task<DataTable> getDetail(string sn)
        {
            string sSQL = @"SELECT A.WORK_ORDER,
                       TO_CHAR (A.OUT_STATIONTYPE_TIME, 'yyyy/mm/dd hh24:mi:ss')
                           OUT_PROCESS_TIME,
                       a.route_name,
                       A.IPN,
                       A.STATION_TYPE
                  FROM SAJET.p_SN_STATUS A WHERE A.SERIAL_NUMBER = '" + sn + "' AND ROWNUM = 1";

            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<DataTable> getDefect(string sn)
        {
            string sSQL = @" SELECT A.RECID,
                                 A.Location,
                                 A.RP_STATUS,
                                 B.DEFECT_CODE,
                                 B.DEFECT_DESC,
                                 B.DEFECT_DESC2,
                                 a.LINE,
                                 a.station_name,
                                 a.station_type,
                                 NVL (G.REASON_CODE, 'N/A') REASON_CODE
                            FROM SAJET.p_SN_DEFECT A,
                                 imes.m_DEFECT   B,
                                 imes.p_SN_REPAIR F,
                                 imes.m_REASON   G
                           WHERE     A.SERIAL_NUMBER = '" + sn + "'AND NVL (rp_status, 1) <> '0' AND A.DEFECT_code = B.DEFECT_code(+) AND A.RECID = F.RECID(+) AND F.REASON_CODE = G.REASON_CODE(+) ORDER BY B.Defect_Code";

            return await Context.Ado.GetDataTableAsync(sSQL);

        }
        public async Task<DataTable> getKpsn(string sn)
        {
            string sSQL = @" SELECT A.WORK_ORDER,A.ITEM_IPN,A.ITEM_GROUP,A.STATION_TYPE,A.ITEM_SN,A.ITEM_SN_CUSTOMER ,B.SPEC1 FROM  IMES.P_SN_KEYPARTS A ,IMES.M_PART B  WHERE A.ITEM_IPN=B.IPN AND  A.SERIAL_NUMBER = '" + sn + "' ORDER BY  A.ITEM_IPN";

            return await Context.Ado.GetDataTableAsync(sSQL);
        }

        public async Task<DataTable> getPostUrl()
        {
            string sSQL = @" select param_value FROM SAJET.S_BASE where param_name='BLUESTICKER' and program='DELINKKEYPARTS'";

            return await Context.Ado.GetDataTableAsync(sSQL);

        }

        public async Task<DataTable> getKpsnInfo(string ipn)
        {
            string sSQL = @" select A.IPN,A.PLANT,A.SPEC1 FROM SAJET.M_PART A WHERE A.IPN='" + ipn + "' ";

            return await Context.Ado.GetDataTableAsync(sSQL);

        }

        public async Task<ExecuteResult> RemoveKp(string station, string sn, string recid, string kpsn, string partno, string kpflag, string defect_data,string userNo)
        {
            try
            {
                exeRes = new ExecuteResult();

                var tRes = new SugarParameter("TRES", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_REPAIR_REMOVE_KP",
                new SugarParameter[]
                    {
                    new SugarParameter("T_STATION_NAME", station),
                    new SugarParameter("TSN", sn),
                    new SugarParameter("TDEFECT_RECID", recid),
                    new SugarParameter("TKPSN", kpsn),
                    new SugarParameter("TPARTNO", partno),
                    new SugarParameter("TKPFLAG", kpflag),
                    new SugarParameter("TKPDEFECT_DATA", defect_data),
                    new SugarParameter("TEMPNO", userNo),
                    tRes
                    });
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }

            return exeRes;
        }

        public async Task<List<lvkp>> Show_KP(string sn)
        {
            DataTable dtTemp = await getKpsn(sn);

            List<lvkp> likps = new List<lvkp>();

            for (int i = 0; i <= dtTemp.Rows.Count - 1; i++)
            {
                lvkp lp = new lvkp();
                lp.ITEM_IPN = dtTemp.Rows[i]["ITEM_IPN"].ToString();
                lp.SPEC1 = dtTemp.Rows[i]["SPEC1"].ToString();
                lp.ITEM_SN = dtTemp.Rows[i]["ITEM_SN"].ToString();
                lp.ITEM_SN_CUSTOMER = dtTemp.Rows[i]["ITEM_SN_CUSTOMER"].ToString();
                lp.STATION_TYPE = dtTemp.Rows[i]["STATION_TYPE"].ToString();
                likps.Add(lp);
            }

            return likps;
        }

        public async Task<bool> Remove_KP(repariDel repari,string userNo)
        {
            DataTable dtTemp = new DataTable();
            string sKPSN = "";
            string sKPNO = "";
            string sPartNo = "";
            var liKps = repari.lvkps;

            for (int i = 0; i < liKps.Count; i++)
            {
                sKPSN = liKps[i].ITEM_SN.ToString();
                sKPNO = liKps[i].ITEM_SN_CUSTOMER.ToString();
                sPartNo = liKps[i].ITEM_IPN.ToString();


                string sKPFlag = "N";
                string sKPDefectData = "";

                if (sKPDefectData == "")
                    sKPDefectData = "N/A";
                //获取post url
                string url = "";
                dtTemp = await getPostUrl();
                if (dtTemp.Rows.Count > 0)
                {
                    url = dtTemp.Rows[0]["param_value"].ToString();
                }
                dtTemp = await getKpsnInfo(sPartNo);
                if (dtTemp.Rows.Count > 0)
                {
                    string spec = dtTemp.Rows[0]["SPEC1"].ToString();
                    string plant = dtTemp.Rows[0]["PLANT"].ToString();
                    string vsn = sKPSN;
                    string status = "4";

                    if (spec == "XT")
                    {
                        Root objdata = new Root();
                        objdata.PLANT = plant;
                        DATAItem obj = new DATAItem();
                        objdata.DATA = new List<DATAItem>();
                        obj.VSN = vsn;
                        obj.status = status;

                        objdata.DATA.Add(obj);
                        string res = JsonConvert.SerializeObject(objdata);
                        //string response = Post.PostUrl(url, res);   //这里干嘛的，看起来没什么用
                    }
                }
                ExecuteResult exe = await RemoveKp("", repari.ipn, "", sKPSN, sPartNo, sKPFlag, sKPDefectData, userNo);

                if (!exe.Status)
                {
                    // utility.ShowMessage(exe.Message, 0);
                    return false;
                }
            }

            return true;
        }

    }
}
