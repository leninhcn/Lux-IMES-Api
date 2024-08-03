using Aliyun.OSS;
using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Infrastructure.Model;
using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using ZR.Common;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Dto.Quality;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Model.System.Vo;
using ZR.Repository;
using ZR.Service.IService;
using ZR.Service.System.IService;
using static ZR.Model.Dto.DataToWMS;

namespace ZR.Service
{
    /// <summary>
    /// 入库程式业务处理层
    /// </summary>
    [AppService(ServiceType = typeof(IWareHousingService), ServiceLifetime = LifeTime.Transient)]
    public class WareHousingService:BaseService<PWoStockInWmsData>,IWareHousingService
    {
        /// <summary>
        /// 入库数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<(bool,string,List<WareHousingResultDto>)> WareHousingInData(WareHousingDto parm)
        {
            bool isok = false;
            string msg = "";
            string id = "";
            if(!parm.Qty.IsNullOrEmpty())
            {
                int temp;
                if (!int.TryParse(parm.Qty, out temp))
                {
                    return (false, "参数parm.Qty必须为数字 " + parm.Qty, null);
                }; 
            }
            else
            {
                parm.Qty = "0";
            }
            //输入类型为SN则先取出系统内的sn
            if (parm.Inputtype == "2")
            {
                (isok, msg) = await GetSN(parm.Inputdata,parm.Site);
                if(!isok)
                {
                    return (false,msg,null);
                }
                parm.Inputdata = msg;
            }
            //入mes库和wms 
            if (parm.Cmbtype=="0")
            {
                return await StorinIN(parm);
            }
            //出mes库
            else if(parm.Cmbtype=="1")
            {
                (isok,msg)=await StorinOut(parm);
            }
            //取消WMS入库
            else if(parm.Cmbtype=="2")
            {
                (isok,msg)=await CancelSendData(parm);
            }
            //HOLD WMS库存
            else if(parm.Cmbtype=="3")
            {
                (isok, msg) = await HoldSNListWMS(parm, "Y");
            }
            else if(parm.Cmbtype=="4")
            {
                (isok, msg) = await HoldSNListWMS(parm, "N");
            }
            else
            {
                return (false,"入库类型错误",null);
            }
            return (isok, msg, null);
        }
        public  async Task<(bool, string)> CheckIsStorin(string inputdata,string site)
        {
           // string sql = @"";
           var result =await Context.Queryable<SnStatus,PWoStockInWmsData>((a,b)=>a.SerialNumber==b.Usn && a.WorkOrder==b.Mo && a.Site==b.Site && b.Site==site && b.WmsFlag==1 &&(b.OutFlag==0 ||b.OutFlag==1) &&(a.SerialNumber==inputdata || a.CartonNo==inputdata||a.PalletNo==inputdata)).Select((a)=>new {a.SerialNumber}).ToListAsync();
           if(result.Count > 0 )
            {
                return (false, inputdata + " 以下SN已入库回传WMS" + string.Join(",", result.Take(result.Count >= 10 ? 10 : result.Count).ToList()));
            }
            return (true, "OK");
        }
        public async Task<(bool, string)> Checkdata(WareHousingDto param)
        {
            // string sql = @"";
            // Carton_No  Pallet_No  Serial_Number WO
            var sql = @"SAJET.SP_PGI_CHECK_WMS";
            List<SugarParameter> sugarParameters = new List<SugarParameter> {
                   new SugarParameter("@T_TYPE",param.Inputtype),
                   new SugarParameter("@T_STATIONNAME",param.Stationname),
                   new SugarParameter("@T_IPN",param.Ipn),
                   new SugarParameter("@T_WAREHOUSECODE",param.Warehousecode),
                   new SugarParameter("@T_LOCATIONCODE",param.Locationcode),
                   new SugarParameter("@T_DATA",param.Inputdata),
                   new SugarParameter("@T_QTY",param.Qty),
                   new SugarParameter("@T_EMP",null),
                   new SugarParameter("@TRES",null,true),
                   new SugarParameter("@T_SITE",param.Site)
            };
           await Context.Ado.UseStoredProcedure().ExecuteCommandAsync(sql,sugarParameters);
            
           return (sugarParameters[8].Value.ToString()=="OK", sugarParameters[8].Value.ToString());
        }
        public async Task<(bool, string)> GetSN(string param,string site)
        {
            var sql = @"SAJET.SP_GET_SN_PSN";
            List<SugarParameter> sugarParameters = new List<SugarParameter> {
                   new SugarParameter("@TREV",param),
                   new SugarParameter("@TRES",null,true),
                   new SugarParameter("@PSN",null,true),
                   new SugarParameter("@T_SITE",site)
            };
            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync(sql, sugarParameters);

            return (sugarParameters[1].Value.ToString() == "OK", sugarParameters[1].Value.ToString() == "OK"?sugarParameters[2].Value.ToString(): sugarParameters[1].Value.ToString());
        }
        public async Task<(bool, string,string)> SendWMS(WareHousingDto param)
        {
            var sql = @"SAJET.SP_PGI_IN_WMS";
            List<SugarParameter> sugarParameters = new List<SugarParameter> {
                   new SugarParameter("@T_TYPE",param.Inputtype),
                   new SugarParameter("@T_STATIONNAME",param.Stationname),
                   new SugarParameter("@T_IPN",param.Ipn),
                   new SugarParameter("@T_WAREHOUSECODE",param.Warehousecode),
                   new SugarParameter("@T_LOCATIONCODE",param.Locationcode),
                   new SugarParameter("@T_DATA",param.Inputdata),
                   new SugarParameter("@T_QTY",param.Qty),
                   new SugarParameter("@T_EMP",param.UpdateEmpno),
                   new SugarParameter("@T_ID",null,true),
                   new SugarParameter("@TRES",null,true),
                   new SugarParameter("@T_SITE",param.Site)
            };
            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync(sql, sugarParameters);
            if (sugarParameters[9].Value.ToString()!="OK")
            {
                return (false,  sugarParameters[9].Value.ToString(),null);
            }
            var id = sugarParameters[8].Value.ToString();
            var (isok, msg) = await SendDataToWMS(id,param.Site);
            return(isok, msg,id);
            
        }
        public async Task<(bool, string)> GetWMSUrl(string code,string site)
        {
            //WMSInWarehouseCheck
            string url = "";
            string sSql = @"SELECT b.CONFIG_VALUE FROM SAJET.m_BLOCK_CONFIG_TYPE A, imes.m_BLOCK_CONFIG_VALUE B
                    WHERE A.CONFIG_TYPE_ID = B.CONFIG_TYPE_ID
                            AND A.ENABLED = 'Y'
                            AND B.ENABLED = 'Y'
                            AND A.CONFIG_TYPE_NAME = 'WMSReturnInterface'
                            AND A.SITE = B.SITE
                            AND A.SITE=@site
                            AND B.CONFIG_NAME = @code";
            DataTable sDt = await Context.Ado.GetDataTableAsync(sSql,new List<SugarParameter> {new SugarParameter("@site",site), new SugarParameter("@code",code)});
            if (sDt.Rows.Count > 0)
            {
                url = sDt.Rows[0]["CONFIG_VALUE"].ToString();
                return(true, url);
            }
            else
            {
                return (false, "WMS接口未配置");
            }
        }
            public async Task<(bool, string)> SendDataToWMS(string id,string site)
        {
            bool isok = false;
            string url = "";
            string wmscode = "WMSInWarehouseCheck";

            (isok, url) = await GetWMSUrl(wmscode,site);
            if (!isok)
            {
                return (false, url);
            }
            List<HData> HDatas = new List<HData>();
            HDatas.Clear();
            string sReJson = string.Empty;
           string  sSql = $"SELECT * FROM SAJET.P_WO_STOCK_IN_WMS_HEAD WHERE SITE ='{site}' AND ID='{id}'";
           DataTable  sDt = await Context.Ado.GetDataTableAsync(sSql);
            foreach (DataRow R in sDt.Rows)
            {
                List<Data> Datas = new List<Data>();
                HData sHData = new HData();
                Head sHead = new Head();
                sHead.MES_PALLET_ID = R["PALLET_ID"].ToString();
                sHead.CARTON_ID = R["CARTON_ID"].ToString();
                sHead.PLANT = R["PLANT"].ToString();
                sHead.PN = R["PN"].ToString();
                sHead.LINE = R["LINE"].ToString();
                sHead.CQTY = R["CQTY"].ToString();
                sHead.UNIT = R["UNIT"].ToString();
                sHead.WAREHOUSE_CODE = R["WAREHOUSE_CODE"].ToString();
                sHead.STATUS = R["STATUS"].ToString();
                sHead.FULL_QTY = R["FULL_QTY"].ToString();
                sSql = $"SELECT * FROM SAJET.P_WO_STOCK_IN_WMS_DATA  WHERE SITE='{site}' AND CARTON_ID='{sHead.CARTON_ID}' AND ID='{id}'";
                sDt = await Context.Ado.GetDataTableAsync(sSql);
                foreach (DataRow R1 in sDt.Rows)
                {
                    Data sData = new Data();
                    sData.USN = R1["USN"].ToString();
                    sData.CARTON_ID = R1["CARTON_ID"].ToString();
                    sData.PLANT = R1["PLANT"].ToString();
                    sData.MO = R1["MO"].ToString();
                    sData.STATUS = R1["STATUS"].ToString();
                    sData.QTY = R1["QTY"].ToString();
                    Datas.Add(sData);
                }
                sHData.HEAD = sHead;
                sHData.DATA = Datas;
                HDatas.Add(sHData);
            }
            SMessage sRsapMsg;
            string sJson = JsonConvert.SerializeObject(HDatas);
            //sReJson = HttpHelper.HttpPost(url, sJson, "application/json");
            sReJson = "{\r\n  \"MSG\": [\r\n    {\r\n      \"plant\": \"W337\",\r\n      \"mes_pallet_id\": \"\",\r\n      \"carton_id\": \"CBU29210729JXJ0018\",\r\n      \"usn\": null,\r\n      \"MSGTY\": \"S\",\r\n      \"MSGTX\": \"ok\",\r\n      \"WMSID\": \"853979131249975004\"\r\n    }\r\n  ]\r\n}";
            try
            {
                sRsapMsg = JsonConvert.DeserializeObject<SMessage>(sReJson);
            }
            catch
            {
                return(false, "调用WMS接口[" + url + "]过账 接口返回:" + sReJson);
            }
            if (sRsapMsg.MSG == null)
            {
                return(false,"调用WMS接口[" + url + "]过账 接口返回:" + sReJson);
            }
            foreach (var S in sRsapMsg.MSG)
            {
                try
                {
                    if (S.MSGTY == "S")
                    {
                        sSql = $@"update SAJET.P_WO_STOCK_IN_WMS_DATA 
                                          SET WMS_FLAG = 1 
                                          WHERE
	                                        ID = '{id}' AND SITE = '{site}'
	                                        AND WMS_FLAG =0";
                        await Context.Ado.ExecuteCommandAsync(sSql);
                       return(true,sReJson);
                    }
                    else
                    {
                        return (false, sReJson);
                        
                    }
                }
                catch (Exception ex)
                {

                    return (false, ex.Message);
                    
                }
            }
            return (false, sReJson); 
        }
        public async Task<(bool,string, List<WareHousingResultDto>)> StorinIN(WareHousingDto parm)
        {
            bool isok = false;
            string msg = "";
            string id = "";
            //检查条码是否存在
            (isok, msg) = await Checkdata(parm);
            if (!isok)
            {
                return (false,msg, null);
            }
            //判断 条码是否已入库
            (isok, msg) = await CheckIsStorin(parm.Inputdata,parm.Site);
            if (!isok)
            {
                return (false,msg.ToString(), null);
            }
            (isok, msg, id) = await SendWMS(parm);
            if (!isok)
            {
                return (false,msg.ToString(), null);
            }
            if (parm.Inputtype != "3")//WO
            {
                //string sSQL = $"SELECT MAX(A.WORK_ORDER) WORK_ORDER,MAX(B.IPN) PART_NO,MAX(A.PALLET_NO) PALLET_NO,MAX(A.CARTON_NO) CARTON_NO,MAX(A.SERIAL_NUMBER) SERIAL_NUMBER,MAX(A.CUSTOMER_SN) CUSTOMER_SN,COUNT(*) QTY,MAX(A.FIXED_QTY) FIXED_QTY,MAX(C.BASE_UNIT_MEASURE) SN_UNIT FROM SAJET.P_SN_STATUS A, IMES.M_PART B,IMES.P_WO_BASE C WHERE A.IPN = B.IPN AND A.WORK_ORDER = C.WORK_ORDER AND  (a.carton_no='{parm.Inputdata}' or a.serial_number='{parm.Inputdata}' or a.pallet_no='{parm.Inputdata}')";
                string sSQL = $"select MAX(mo) WORK_ORDER,MAX(B.IPN) PART_NO,MAX(C.PALLET_NO) PALLET_NO, MAX(C.CARTON_NO) CARTON_NO, MAX(C.SERIAL_NUMBER) SERIAL_NUMBER, MAX(C.CUSTOMER_SN) CUSTOMER_SN, COUNT(*) QTY,MAX(B.BASE_UNIT_MEASURE) SN_UNIT FROM SAJET.P_WO_STOCK_IN_WMS_DATA A, IMES.P_WO_BASE B, IMES.P_SN_STATUS C where A.MO = B.WORK_ORDER AND A.USN = C.SERIAL_NUMBER AND A.SITE = B.SITE AND B.SITE = C.SITE AND A.SITE = '{parm.Site}' AND id = '{id}'";
                var resulelist = await Context.Ado.SqlQueryAsync<WareHousingResultDto>(sSQL);
                return (true,msg, resulelist);
            }
            else
            {
                string sSQL = @"SELECT A.MO WORK_ORDER,
                             B.IPN PART_NO,
                             'N/A' PALLET_NO,
                             'N/A' CARTON_NO,
                             'N/A' SERIAL_NUMBER,
                             'N/A' CUSTOMER_SN,
                             B.TARGET_QTY,
                             SUM (QTY) AS QTY,
                             B.BASE_UNIT_MEASURE SN_UNIT
                        FROM SAJET.P_WO_STOCK_IN_WMS_DATA A, IMES.P_WO_BASE B
                       WHERE  B.WORK_ORDER = A.MO
                             AND a.DATA_TYPE = 3
                             AND A.SITE = B.SITE
                             AND A.SITE = @site
                             AND a.ID = @ID
                         GROUP BY A.MO,
                             B.IPN,
                             B.BASE_UNIT_MEASURE,
                             B.TARGET_QTY";
                var resultmo = await Context.Ado.SqlQueryAsync<WareHousingResultDto>(sSQL, new List<SugarParameter> { new SugarParameter("@site",parm.Site),new SugarParameter("@ID", id) });
                return (true,msg, resultmo);
            }
        }
        public async Task<(bool,string)> StorinOut(WareHousingDto param)
        {
            try
            {
                List<SugarParameter> sugarparmeters=new List<SugarParameter>() { 
                new SugarParameter("@T_TYPE",param.Inputtype),
                new SugarParameter("@T_STATIONNAME",null),
                new SugarParameter("@T_WAREHOUSEcode",null),
                new SugarParameter("@T_LOCATIONcode",null),
                new SugarParameter("@T_DATA",param.Inputdata),
                new SugarParameter("@T_EMP",param.UpdateEmpno),
                new SugarParameter("@TRES",null,true),
                new SugarParameter("@T_SITE",param.Site)
                };
                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_PGI_OUT_WMS", sugarparmeters);
                string tres = sugarparmeters[6].Value.ToString();

                if (tres == "OK")
                {
                    return (true,param.Inputdata + ":出库成功 OK");
                }
                else
                {
                    return (false,param.Inputdata+ ":出库失败[" + tres + "]");
                }
            }
            catch (Exception ex)
            {
                return (false,ex.Message + "<" + ex.StackTrace.ToString() + ">");
            }
        }
        public async Task<(bool,string)> CancelSendData(WareHousingDto param)
        {
            try
            {//CancelUSN , CancelCarton , CancelMESPallet 
                bool isok = false;
                string url = "";
                string wmscode = "WMSInWarehouseCheck";
                string sdata = param.Inputdata;
                int stype=Convert.ToInt32(param.Inputtype);
                 if(stype == 3)
                {
                    return (false,"NG;输入类型错误!，只允许产品序列号，箱号，栈板");
                    
                }
                string sJson="";
                //箱号
                if (stype == 0)
                {
                    wmscode = "CancelCarton";             
                    string sSql = $@"SELECT
	                                    * 
                                    FROM
	                                    IMES.P_WO_STOCK_IN_WMS_DATA 
                                    WHERE
	                                    CARTON_ID = '{sdata}'
                                        AND SITE = '{param.Site}'
	                                    AND OUT_FLAG IN (0,1)
	                                    AND WMS_FLAG =1";
                    DataTable sDt = await Context.Ado.GetDataTableAsync(sSql);
                    if (sDt.Rows.Count > 0)
                    {
                        List<CancelCARTONDATA> cancelCARTONDATAs = new List<CancelCARTONDATA>();
                        CancelCARTONDATA cancelCARTONDATA = new CancelCARTONDATA();
                        cancelCARTONDATA.PLANT = sDt.Rows[0]["PLANT"].ToString();
                        cancelCARTONDATA.CARTON_ID = sdata;
                        cancelCARTONDATAs.Add(cancelCARTONDATA);
                        Dictionary<string, List<CancelCARTONDATA>> sProperties = new Dictionary<string, List<CancelCARTONDATA>>();
                        sProperties.Add("DATA", cancelCARTONDATAs);
                        sJson = JsonConvert.SerializeObject(sProperties);

                    }
                    else
                    {
                        return (false,$"NG;输入的箱号[{sdata}]未入库或已出库!");
                        
                    }
                }
                //栈板
                else if (stype == 1)
                {
                    wmscode = "CancelMESPallet";
                    string sSql = $@"SELECT
	                                    * 
                                    FROM
	                                    IMES.P_WO_STOCK_IN_WMS_DATA 
                                    WHERE
	                                    OUT_FLAG IN (0,1)
                                        AND SITE = '{param.Site}'
	                                    AND WMS_FLAG = 1
	                                    AND CARTON_ID IN ( SELECT CARTON_ID FROM SAJET.P_WO_STOCK_IN_WMS_HEAD WHERE PALLET_ID = '{sdata}' )";
                    DataTable sDt = await Context.Ado.GetDataTableAsync(sSql);
                    if (sDt.Rows.Count > 0)
                    {
                        List<CancelPALLETDATA> cancelPALLETDATAs = new List<CancelPALLETDATA>();
                        CancelPALLETDATA cancelPALLETDATA = new CancelPALLETDATA();
                        cancelPALLETDATA.PLANT = sDt.Rows[0]["PLANT"].ToString();
                        cancelPALLETDATA.MES_PALLET_ID = sdata;
                        cancelPALLETDATAs.Add(cancelPALLETDATA);
                        Dictionary<string, List<CancelPALLETDATA>> sProperties = new Dictionary<string, List<CancelPALLETDATA>>();
                        sProperties.Add("DATA", cancelPALLETDATAs);
                        sJson = JsonConvert.SerializeObject(sProperties);
                    }
                    else
                    {
                        return (false,$"NG;输入的栈板[{sdata}]未入库或已出库!");
                        
                    }
                }
                //sn
                else if (stype == 2)
                {
                    wmscode = "CancelUSN";
                    string sSql = $@"SELECT
	                                    * 
                                    FROM
	                                    IMES.P_WO_STOCK_IN_WMS_DATA 
                                    WHERE
	                                    USN = '{sdata}'
                                        AND SITE = '{param.Site}'
	                                    AND OUT_FLAG IN (0,1)
	                                    AND WMS_FLAG =1";
                    DataTable sDt = await Context.Ado.GetDataTableAsync(sSql);
                    if (sDt.Rows.Count > 0)
                    {
                        List<CancelUSNDATA> cancelUSNDATAs = new List<CancelUSNDATA>();
                        CancelUSNDATA cancelUSNDATA = new CancelUSNDATA();
                        cancelUSNDATA.PLANT = sDt.Rows[0]["PLANT"].ToString();
                        cancelUSNDATA.USN = sdata;
                        cancelUSNDATAs.Add(cancelUSNDATA);
                        Dictionary<string, List<CancelUSNDATA>> sProperties = new Dictionary<string, List<CancelUSNDATA>>();
                        sProperties.Add("DATA", cancelUSNDATAs);
                        sJson = JsonConvert.SerializeObject(sProperties);
                    }
                    else
                    {
                        return (false,$"NG;输入的SN[{sdata}]未入库或已出库!");
                        
                    }
                }

                (isok, url) = await GetWMSUrl(wmscode,param.Site);
                if (!isok)
                {
                    return (isok, url);
                }
                //string sReJson = HttpHelper.HttpPost(url, sJson, "application/json");
                string sReJson = "{\r\n  \"MSG\": [\r\n    {\r\n      \"plant\": \"W337\",\r\n      \"mes_pallet_id\": \"\",\r\n      \"carton_id\": \"CBU29210729JXJ0018\",\r\n      \"usn\": null,\r\n      \"MSGTY\": \"S\",\r\n      \"MSGTX\": \"ok\",\r\n      \"WMSID\": \"853979131249975004\"\r\n    }\r\n  ]\r\n}";
                SMessage sRsapMsg;
                try
                {
                    sRsapMsg = JsonConvert.DeserializeObject<SMessage>(sReJson);
                }
                catch
                {
                    return (false,"NG;调用WMS接口[" + url + "]Cancel 接口返回:" + sReJson);   
                }
                try
                {
                    foreach (var S in sRsapMsg.MSG)
                    {
                    
                        if (S.MSGTY == "S")
                        {
                            string sSql="";
                            //箱号
                            if (sdata == "0")
                            {
                                sSql = $@"update SAJET.P_WO_STOCK_IN_WMS_DATA 
                                          SET OUT_FLAG = 3 
                                          WHERE
	                                        CARTON_ID = '{sdata}' 
                                            AND SITE = '{param.Site}'
	                                        AND OUT_FLAG IN (0,1)
	                                        AND WMS_FLAG =1";
                            }
                            //栈板
                            else if (stype == 1)
                            {
                                sSql = $@"UPDATE
                                            IMES.P_WO_STOCK_IN_WMS_DATA
                                        SET
                                            OUT_FLAG = 3
                                        WHERE
                                            OUT_FLAG IN (0,1)
                                            AND SITE = '{param.Site}'
                                            AND WMS_FLAG = 1
                                            AND CARTON_ID IN ( SELECT CARTON_ID FROM SAJET.P_WO_STOCK_IN_WMS_HEAD WHERE PALLET_ID = '{sdata}' AND SITE = '{param.Site}' )";
                            }
                            //sn
                            else if (stype == 2)
                            {
                                sSql = $@"update SAJET.P_WO_STOCK_IN_WMS_DATA 
                                          SET OUT_FLAG = 3 
                                          WHERE
	                                        USN = '{sdata}' 
                                            AND SITE = '{param.Site}'
	                                        AND OUT_FLAG IN (0,1) 
	                                        AND WMS_FLAG =1";
                            }
                            await Context.Ado.ExecuteCommandAsync(sSql);
                            return (true,"OK;撤销回传成功!接口返回:" + sReJson);
                        }
                        else
                        {
                            return (false,"NG;" + sReJson);
                        }
                   
                    }
                     return (false,"NG;" + sReJson);
                }
                catch (Exception ex)
                {
                    return (false, "NG;WMS接口返回值解析失败 " + sReJson);
                }
            }
            catch (Exception ex)
            {
                return (false,"NG;" + ex.ToString());
            }
        }
        public async Task<(bool,string)> HoldSNListWMS(WareHousingDto param,string holdflag)
        {
            try
            {
                bool isok = false;
                string url = "";
                string sMsg = "";
                string wmscode = "HoldSNList";
                string sdata = param.Inputdata;
                int stype = Convert.ToInt32(param.Inputtype);
                string sJson;
                (isok, url) = await GetWMSUrl(wmscode, param.Site);
                if (!isok)
                {
                    return (isok, url);
                }

                List<HoldModel> listHoldModel = new List<HoldModel>();
                //箱号
                if (stype == 0)
                {
                    string sSql = $@"SELECT
	                                    * 
                                    FROM
	                                    IMES.P_WO_STOCK_IN_WMS_DATA 
                                    WHERE
	                                    CARTON_ID = '{sdata}' 
                                        AND SITE = '{param.Site}'
	                                    AND OUT_FLAG IN (0,1)
	                                    AND WMS_FLAG =1";
                    DataTable sDt = await Context.Ado.GetDataTableAsync(sSql);
                    if (sDt.Rows.Count > 0)
                    {
                        HoldModel holdModel = new HoldModel();
                        holdModel.plant_code = sDt.Rows[0]["PLANT"].ToString();
                        holdModel.warehouse_code = "Z001";
                        holdModel.sn = sdata;
                        holdModel.origin_system = sDt.Rows[0]["PLANT"].ToString() + "_IMES";
                        holdModel.hold_flag = holdflag;
                        holdModel.hold_reasoncode = "Z001";
                        holdModel.hold_reasondesc = param.Mark;
                        holdModel.create_by = sDt.Rows[0]["PLANT"].ToString() + param.UpdateEmpno;
                        listHoldModel.Add(holdModel);
                        sJson = JsonConvert.SerializeObject(listHoldModel);
                    }
                    else
                    {
                        return (false,$"NG;输入的箱号[{sdata}]未入库或已出库!");
                    }
                }
                //栈板
                else if (stype == 1)
                {

                    string sSql = $@"SELECT DISTINCT CARTON_ID,PLANT 
                                    FROM
	                                    IMES.P_WO_STOCK_IN_WMS_DATA 
                                    WHERE
	                                    OUT_FLAG IN (0,1)
                                        AND SITE = '{param.Site}'
	                                    AND WMS_FLAG = 1
	                                    AND CARTON_ID IN ( SELECT CARTON_ID FROM SAJET.P_WO_STOCK_IN_WMS_HEAD WHERE PALLET_ID = '{sdata}' AND SITE = '{param.Site}' )";
                    DataTable sDt = await Context.Ado.GetDataTableAsync(sSql);
                    if (sDt.Rows.Count > 0)
                    {
                        for (int i = 0; i < sDt.Rows.Count; i++)
                        {
                            HoldModel holdModel = new HoldModel();
                            holdModel.plant_code = sDt.Rows[i]["PLANT"].ToString();
                            holdModel.warehouse_code = "Z0001";
                            holdModel.sn = sDt.Rows[i]["CARTON_ID"].ToString();
                            holdModel.origin_system = sDt.Rows[i]["PLANT"].ToString() + "_IMES";
                            holdModel.hold_flag = holdflag;
                            holdModel.hold_reasoncode = "Z0001";
                            holdModel.hold_reasondesc = param.Mark;
                            holdModel.create_by = sDt.Rows[i]["PLANT"].ToString() + param.UpdateEmpno;
                            listHoldModel.Add(holdModel);
                        }
                        sJson = JsonConvert.SerializeObject(listHoldModel);
                    }
                    else
                    {
                        return (false,$"NG;输入的栈板[{sdata}]未入库或已出库!");                   
                    }
                }
                //sn
                else if (stype == 2)
                {
                    string sSql = $@"SELECT
	                                    * 
                                    FROM
	                                    IMES.P_WO_STOCK_IN_WMS_DATA 
                                    WHERE
	                                    USN = '{sdata}' 
                                        AND SITE = '{param.Site}'
	                                    AND OUT_FLAG IN (0,1)
	                                    AND WMS_FLAG =1";
                    DataTable sDt = await Context.Ado.GetDataTableAsync(sSql);
                    if (sDt.Rows.Count > 0)
                    {
                        HoldModel holdModel = new HoldModel();
                        holdModel.plant_code = sDt.Rows[0]["PLANT"].ToString();
                        holdModel.warehouse_code = "Z001";
                        holdModel.sn = sdata;
                        holdModel.origin_system = sDt.Rows[0]["PLANT"].ToString() + "_IMES";
                        holdModel.hold_flag = holdflag;
                        holdModel.hold_reasoncode = "Z001";
                        holdModel.hold_reasondesc = param.Mark;
                        holdModel.create_by = sDt.Rows[0]["PLANT"].ToString() + param.UpdateEmpno;
                        listHoldModel.Add(holdModel);
                        sJson = JsonConvert.SerializeObject(listHoldModel);
                    }
                    else
                    {
                        return (false,$"NG;输入的SN[{sdata}]未入库或已出库!");        
                    }
                }
                else
                {
                    return (false,"MG;输入类型错误!");         
                }
                 //string sReJson = HttpHelper.HttpPost(url, sJson, "application/json");
                string sReJson = "{\r\n  \"DATA\": [\r\n    {\r\n      \"hold_id\": \"853979778153359206\",\r\n      \"plant_code\": \"W337\",\r\n      \"warehouse_code\": \"Z001\",\r\n      \"sn\": \"BU29210729JXJ0005\",\r\n      \"origin_system\": \"W337_IMES\",\r\n      \"hold_flag\": \"Y\",\r\n      \"hold_reasoncode\": \"Z001\",\r\n      \"hold_reasondesc\": \"string\",\r\n      \"status\": \"FP\",\r\n      \"status_reasoncode\": \"序号不存在WMS\",\r\n      \"create_by\": \"W337\"\r\n    }\r\n  ],\r\n  \"MSGTY\": \"S\",\r\n  \"MSGTX\": \"接收成功!\",\r\n  \"WMSID\": null\r\n}";
                 HoldModelMSG sRsapMsg;
                try
                {
                    sRsapMsg = JsonConvert.DeserializeObject<HoldModelMSG>(sReJson);
                }
                catch
                {
                    return (false,$"NG;调用WMS接口[{url}]Cancel 接口返回:" + sReJson);
                    
                }

                foreach (HoldModel S in sRsapMsg.DATA)
                {
                    try
                    {
                        if (S.status == "FP")
                        {
                            string sSql;
                            //箱号
                            if (stype == 0)
                            {
                                sSql = $@"update SAJET.P_WO_STOCK_IN_WMS_DATA 
                                          SET HOLD_FLAG = '{holdflag}' 
                                          WHERE
	                                        CARTON_ID = '{S.sn}' 
                                            AND SITE = '{param.Site}'
	                                        AND OUT_FLAG IN (0,1)
	                                        AND WMS_FLAG =1";
                            }
                            //栈板
                            else if (stype == 1)
                            {
                                sSql = $@"UPDATE
                                            IMES.P_WO_STOCK_IN_WMS_DATA
                                        SET
                                            HOLD_FLAG = '{holdflag}'
                                        WHERE
                                            OUT_FLAG IN (0,1)
                                            AND SITE = '{param.Site}'
                                            AND WMS_FLAG = 1
                                            AND CARTON_ID='{S.sn}'";
                            }
                            //sn
                            else if (stype == 2)
                            {
                                sSql = $@"update SAJET.P_WO_STOCK_IN_WMS_DATA 
                                          SET HOLD_FLAG = '{holdflag}' 
                                          WHERE
	                                        USN = '{S.sn}' 
                                            AND SITE = '{param.Site}'
	                                        AND OUT_FLAG IN (0,1) 
	                                        AND WMS_FLAG =1";
                            }
                            else
                            {
                                return (false,"NG;输入类型错误!"); 
                            }
                            await Context.Ado.ExecuteCommandAsync(sSql);
                            sMsg = "OK";
                        }
                        else
                        {
                            return (false,"NG;" + S.sn + ":" + S.status_reasoncode);    
                        }
                    }
                    catch (Exception ex)
                    {

                        return (false,"NG;" + ex.Message);                    
                    }
                }
                if ("OK".Equals(sMsg))
                {
                    if ("Y".Equals(holdflag))
                    {
                        sMsg = "OK;HOLD成功!接口返回:" + sReJson;
                    }
                    else
                    {
                        sMsg = "OK;UNHOLD成功!接口返回:" + sReJson;
                    }
                    return (true,sMsg);
                }
                return (false,"NG;" + sReJson);       
            }
            catch (Exception ex)
            {

                return (false, "NG;" + ex.ToString());       
            }
        }
    }
}
