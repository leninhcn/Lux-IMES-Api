using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using System.Data;
using System.IO.Pipelines;
using System.Net;
using ZR.Model.Dto;
using ZR.Model.Dto.ProdDto;
using ZR.Service.IService;
using ZR.ServiceCore.Bascon;

namespace ZR.Admin.WebApi.Controllers.ProdMnt
{
    [Route("prodMnt/packExecute/[action]")]
    [ApiController]
    public class PackExecuteController : BaseController
    {
        readonly IPackExecuteService service;
        public PackExecuteController(IPackExecuteService service)
        {
            this.service = service;
        }
        public struct TOptionSetup
        {
            public string pkBase;  //包装依据
            public int pkAction;   //包装行为
            public string pkActionName;   //包装行为
            public bool inputEC;   // 
            public string ruleFun; //Check 规则函数
            public bool removeCSN; //移除CSN
            public bool weightCarton;  //箱称重
            public string weightPort;//城中端口
            public bool capsLock;

        }

        public struct TOptionData
        {
            //0:CSN,1:Box,2:Carton,3:Pallet,4:InnerBox            
            public bool sysCreate;      //System Create   
            public bool inputRelease;  //Input(Release) 
            public bool print;          //if 打印标签
            public string printMethod;  // 打印方法
            public string printPort;    // 打印端口
            public int printQty;        // 打印数量            
            public bool notChange;      // CSN
            public bool sameSN;         // CSN=SN
            public bool checkSameSN;    // Check CSN=SN
            public string weight;       //add by hua 增加称重 2015/05/23
            public string weightPort;   //add by hua 增加称重 2015/05/23
        }


        [HttpGet]
        public async Task<IActionResult> GetOptionData(string station)
        {
            var dt = await service.GetOptionData(station);

            if (dt.Rows.Count == 0)
            {
                return ToResponse(ResultCode.FAIL, "Configuration not Exist");
            }

            var TOption = new TOptionData[5];
            var TSetup = new TOptionSetup();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                string sParamItem = dt.Rows[i]["PARAME_ITEM"].ToString();
                string sParamValue = dt.Rows[i]["PARAME_VALUE"].ToString();
                switch (sParamItem)
                {
                    //Customer SN
                    case "CSN":
                        TOption[0].sysCreate = (sParamValue == "System Create");
                        TOption[0].inputRelease = (sParamValue == "Input (Released)");
                        TOption[0].notChange = (sParamValue == "Don't Change");
                        TOption[0].sameSN = (sParamValue == "CSN=SN");
                        TOption[0].checkSameSN = (sParamValue == "CSN=SN (Check)");
                        continue;
                    case "Print CSN Label":
                        TOption[0].print = (sParamValue == "Y");
                        continue;
                    case "Print CSN Label Method":
                        TOption[0].printMethod = sParamValue;
                        continue;
                    case "Print CSN Label Port":
                        TOption[0].printPort = sParamValue;
                        continue;
                    case "Print CSN Label Qty":
                        TOption[0].printQty = Convert.ToInt32(sParamValue);
                        continue;
                    //InnerBox
                    case "InnerBox":
                        TOption[4].sysCreate = (sParamValue == "System Create");
                        TOption[4].inputRelease = (sParamValue == "Input (Released)");
                        continue;
                    case "Print InnerBox Label":
                        TOption[4].print = (sParamValue == "Y");
                        continue;
                    case "Print InnerBox Label Method":
                        TOption[4].printMethod = sParamValue;
                        continue;
                    case "Print InnerBox Label Port":
                        TOption[4].printPort = sParamValue;
                        continue;
                    case "Print InnerBox Label Qty":
                        TOption[4].printQty = Convert.ToInt32(sParamValue);
                        continue;
                    //Box
                    case "Box":
                        TOption[1].sysCreate = (sParamValue == "System Create");
                        TOption[1].inputRelease = (sParamValue == "Input (Released)");
                        continue;
                    case "Print Box Label":
                        TOption[1].print = (sParamValue == "Y");
                        continue;
                    case "Print Box Label Method":
                        TOption[1].printMethod = sParamValue;
                        continue;
                    case "Print Box Label Port":
                        TOption[1].printPort = sParamValue;
                        continue;
                    case "Print Box Label Qty":
                        TOption[1].printQty = Convert.ToInt32(sParamValue);
                        continue;
                    //Carton
                    case "Carton":
                        TOption[2].sysCreate = (sParamValue == "System Create");
                        TOption[2].inputRelease = (sParamValue == "Input (Released)");
                        continue;
                    case "Print Carton Label":
                        TOption[2].print = (sParamValue == "Y");
                        continue;
                    case "Print Carton Label Method":
                        TOption[2].printMethod = sParamValue;
                        continue;
                    case "Print Carton Label Port":
                        TOption[2].printPort = sParamValue;
                        continue;
                    case "Print Carton Label Qty":
                        TOption[2].printQty = Convert.ToInt32(sParamValue);
                        continue;
                    case "Weight Carton":
                        TOption[2].weight = sParamValue;
                        TSetup.weightCarton = sParamValue == "Y";
                        continue;
                    case "Weight COM Port":
                        TOption[2].weightPort = sParamValue;
                        TSetup.weightPort = sParamValue;
                        continue;
                    //Pallet
                    case "Pallet":
                        TOption[3].sysCreate = (sParamValue == "System Create");
                        TOption[3].inputRelease = (sParamValue == "Input (Released)");
                        continue;
                    case "Print Pallet Label":
                        TOption[3].print = (sParamValue == "Y");
                        continue;
                    case "Print Pallet Label Method":
                        TOption[3].printMethod = sParamValue;
                        continue;
                    case "Print Pallet Label Port":
                        TOption[3].printPort = sParamValue;
                        continue;
                    case "Print Pallet Label Qty":
                        TOption[3].printQty = Convert.ToInt32(sParamValue);
                        continue;
                    case "Packing Base":
                        TSetup.pkBase = sParamValue;
                        continue;
                    case "Packing Action":
                        TSetup.pkAction = Convert.ToInt32(sParamValue);
                        DataTable dsTemp1 = await service.GetPackingAction();
                        if (dsTemp1.Rows.Count > 0)
                        {
                            string sValue = dsTemp1.Rows[0]["param_value"].ToString().TrimEnd(new Char[] { ',' });
                            string[] sAction = sValue.Split(new Char[] { ',' });
                            for (int j = 0; j <= sAction.Length - 1; j++)
                            {
                                if (sAction[j].ToString().Substring(0, 1) == sParamValue)
                                {
                                    TSetup.pkActionName = sAction[j].ToString().Substring(1) + " ";
                                    break;
                                }
                            }
                        }
                        continue;
                    case "Input Error Code":
                        TSetup.inputEC = (sParamValue == "Y");
                        continue;
                    case "Check Rule by Function":
                        TSetup.ruleFun = sParamValue;
                        continue;
                    case "Caps Lock":
                        if (sParamValue == "Y")
                        {
                            TSetup.capsLock = true;
                        }
                        continue;
                    case "Remove Customer SN":
                        TSetup.removeCSN = (sParamValue == "Y");
                        continue;
                }
            }

            return SUCCESS(new { optionData = TOption, setup = TSetup });
        }

        [HttpGet]
        public async Task<IActionResult> CheckWo(string wo, string station)
        {
            wo = await service.GetWorkOrderBySn(wo);

            var (isOk, msg) = await service.CheckWo(wo, station);
            if (!isOk) return ToResponse(ResultCode.FAIL, msg);

            var woInfo = await service.GetWoInfo(wo);

            var packSpec = await service.GetPkSpec(wo);

            return SUCCESS(new { wo = woInfo, packSpec = packSpec });
        }

        [HttpPost]
        public async Task<IActionResult> GetLabelList([FromQuery]string wo, [FromQuery]string stationType)
        {
            return SUCCESS(await service.GetLabelList(wo, stationType));
        }

        async Task<(FtpWebResponse?, long)> GetFtpResponseAsync(string url, string user, string password)
        {
            var ftpSizeReq = (FtpWebRequest)WebRequest.Create(url);
            ftpSizeReq.Timeout = 1000 * 10;
            ftpSizeReq.Method = WebRequestMethods.Ftp.GetFileSize;
            ftpSizeReq.Credentials = new NetworkCredential(user, password);
            var contentSize = (await ftpSizeReq.GetResponseAsync()).ContentLength;
            if (contentSize == 0) return (null, contentSize);

            var ftpReq = (FtpWebRequest)WebRequest.Create(url);
            ftpReq.Timeout = 1000 * 10;
            ftpReq.Method = WebRequestMethods.Ftp.DownloadFile;
            ftpReq.Credentials = new NetworkCredential(user, password);

            var response = (FtpWebResponse)await ftpReq.GetResponseAsync();

            return (response, contentSize);
        }

        [HttpPost]
        public async Task<IActionResult> DownloadLabelFile([FromBody] StationLabelDto label)
        {
            var serverInfo = await service.GetLabelServerInfo(label);
            var (response, _) = await GetFtpResponseAsync($"ftp://{serverInfo.ServerIp}/{label.LabelName}",
                serverInfo.ServerUser, serverInfo.ServerPassword);

            if (response is null) return ToResponse(ResultCode.FAIL, "文件尺寸错误：ftp 反馈文件大小为0。");

            return File(response.GetResponseStream(), "application/octet-stream");
        }

        [HttpPost]
        public async Task<IActionResult> GetUnfinishPallet([FromQuery]string type, 
            [FromQuery] string typeValue, [FromQuery] string station, [FromQuery] string pkSpecName)
        {
            var data = await service.GetUnfinishPallet(type, typeValue, station, pkSpecName);
            return SUCCESS(data);
        }

        [HttpPost]
        public async Task<IActionResult> GetUnfinishCarton([FromQuery] string type,
            [FromQuery] string typeValue, [FromQuery] string station, [FromQuery] string pkSpecName)
        {
            var data = await service.GetUnfinishCarton(type, typeValue, station, pkSpecName);
            return SUCCESS(data);
        }

        [HttpPost]
        public async Task<IActionResult> GetUnfinishBox([FromQuery] string type,
            [FromQuery] string typeValue, [FromQuery] string station, [FromQuery] string pkSpecName)
        {
            var data = await service.GetUnfinishBox(type, typeValue, station, pkSpecName);
            return SUCCESS(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetPackQty(string type, string packNo, string? wo = "")
        {
            var (qty, snQty) = await service.GetPackQty(type, packNo, wo);
            return SUCCESS(new { qty, snQty });
        }
        [HttpGet]
        public async Task<IActionResult> CreateNewPack(string type, string wo, string ipn, string specName, string station)
        {
            var (ok, msg, newNo) = await service.CreateNewPack(type, wo, ipn, specName, station, HttpContext.GetUId(), HttpContext.GetName());
            if (!ok)
                return ToResponse(ResultCode.FAIL, msg);
            else
                return SUCCESS(newNo);
        }

        public class CheckSnReq
        {
            public int PkAction { get; set; }

            public bool InputEC { get; set; }

            public string Wo { get; set; }
            public string Ipn { get; set; }
            public string Sn { get; set; }
            public string Station { get; set; }
            public string SpecName { get; set; }

            public string Box { get; set; }
            public bool BoxEnabled { get; set; }
            public int BoxQty { get; set; }
            public int BoxCty { get; set; }

            public string Carton { get; set; }
            public bool CartonEnabled { get; set; }
            public int CartonQty { get; set; }
            public int CartonCty { get; set; }

            public string Pallet { get; set; }
            public bool PalletEnabled { get; set; }
            public int PalletQty { get; set; }
            public int PalletCty { get; set; }

            public bool RefreshQty { get; set; } = false;

            public bool SysCreate { get; set; }
            public bool NotChange { get; set; }
            public bool SameSN { get; set; }
        }

        public class CheckSnResult
        {
            public string Sn { get; set; }
            public string ClearTarget { get; set; }
            public string FocusTarget { get; set; }

            public int BoxQty { get; set; } = -1;
            public int CartonQty { get; set; } = -1;
            public int CartonSnQty { get; set; } = -1;
            public int PalletQty { get; set; } = -1;
            public int PalletSnQty { get; set; } = -1;

            public string NewCSN { get; set; }
        }

        class CheckSnMidDto
        {
            public bool Ok { get; set; } = false;
            public string Msg { get; set; }
            public string OldBox { get; set; }
            public string OldCarton { get; set; }
            public string OldPallet { get; set; }
        }
        [HttpGet]
        async Task<CheckSnMidDto> CheckSn(string station, string wo, string sn, string box, string carton, string pallet)
        {
            var ret = new CheckSnMidDto();

            async Task CheckCore()
            {
                var empNo = HttpContext.GetName();
                var (ok, msg) = await service.CheckSnBefore(station, wo, sn, empNo);
                if(!ok) {
                    ret.Msg = msg;
                    return;
                }

                var (oldBox, oldCarton, oldPallet) = await service.GetOldPackNoBySn(sn);
                ret.OldBox = oldBox;
                ret.OldCarton = oldCarton;
                ret.OldPallet = oldPallet;

                (ok, msg) = await service.CheckRoute(station, sn);

                if (!ok)
                {
                    ret.Msg = msg;
                    return;
                }

                (ok, msg) = await service.CheckStationSN(station, wo, sn, empNo);

                if (!ok)
                {
                    ret.Msg = msg;
                    return;
                }

                (ok, msg) = await service.CheckMix(station, wo, sn, box, carton, pallet, empNo);

                if (!ok)
                {
                    ret.Msg = msg;
                    return;
                }

                ret.Ok = true;
            }

            await CheckCore();
            return ret;
        }
        [HttpGet]
        async Task<string> RefreshBoxQty(CheckSnResult ret, string type, string packNo)
        {
            if(await service.PackIsClosed(type, packNo))
            {
                ret.FocusTarget = "Box";
                return "Box had been Closed";
            }

            var (boxQty, _) = await service.GetPackQty(type, packNo);
            ret.BoxQty = boxQty;

            return string.Empty;
        }
        [HttpGet]
        async Task<(bool, string?)> F_CHECK_DUP_NO(CheckSnReq req)
        {
            if(req.PalletEnabled) {
                var (ok, msg) = await service.CheckPackInfoByCarton(req.Pallet, req.Carton);
                if (!ok) return (ok, msg);
            }

            if(req.BoxEnabled & req.CartonEnabled)
            {
                var (ok, msg) = await service.CheckPackInfoByBox(req.Box, req.Carton);
                if (!ok) return (ok, msg);
            }

            return (true, null);
        }
        [HttpGet]
        async Task<(bool, string?)> Input_SN(CheckSnReq req, string csn)
        {
            if(req.PalletEnabled && string.IsNullOrEmpty(req.Pallet))
            {
                return (false, "Data is null" + Environment.NewLine + "Pallet No");
            }

            if (req.CartonEnabled && string.IsNullOrEmpty(req.Carton))
            {
                return (false, "Data is null" + Environment.NewLine + "Carton No");
            }

            if (req.BoxEnabled && string.IsNullOrEmpty(req.Box))
            {
                return (false, "Data is null" + Environment.NewLine + "Box No");
            }

            var userNo = HttpContext.GetName();
            var pkAction = req.PkAction;
            if(pkAction != 0 && pkAction != 2 && pkAction != 5)
            {
                await service.Append_PackNo("Pallet", req.Wo, req.Ipn, req.Station, req.SpecName, req.Pallet, userNo);
            }

            if(pkAction != 2)
            {
                if(pkAction != 3)
                {
                    await service.Append_PackNo("Carton", req.Wo, req.Ipn, req.Station, req.SpecName, req.Carton, userNo);
                }

                if(pkAction != 4 && pkAction != 5)
                {
                    await service.Append_PackNo("Box", req.Wo, req.Ipn, req.Station, req.SpecName, req.Box, userNo);
                }
            }

            var (ok, msg) = await service.PackingGo(req.Station, req.PkAction.ToString(), req.Sn, csn, req.Box, req.Carton, req.Pallet, userNo);

            if(!ok)
            {
                return (false, msg);
            }
            return (true, null);
        }
        [HttpGet]
        async Task UpdatePackQty(CheckSnReq req, CheckSnResult ret)
        {
            var pkAction = req.PkAction;
            if(pkAction == 3 && !string.IsNullOrEmpty(req.Box))
            {
                var (boxQty, _) = await service.GetPackQty("Box", req.Box);
                ret.BoxQty = boxQty;
            }

            if (!string.IsNullOrEmpty(req.Carton))
            {
                var (cartonQty, cartonSnQty) = await service.GetPackQty("Carton", req.Carton);
                ret.CartonQty= cartonQty;
                ret.CartonSnQty = cartonSnQty;
            }

            if (!string.IsNullOrEmpty(req.Pallet))
            {
                var (palletQty, palletSnQty) = await service.GetPackQty("Pallet", req.Pallet);
                ret.PalletQty = palletQty;
                ret.PalletSnQty = palletSnQty;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckAndGoSn([FromBody]CheckSnReq req)
        {
            var input = req.Sn;
            var sn = await service.GetSn(input);
            var ret = new CheckSnResult();
            var station = req.Station;
            var pkAction = req.PkAction;

            var empNo = HttpContext.GetName();

            ret.Sn = sn;

            if (req.InputEC)//是否检查输入不良代码errorCode
            {
                if (await service.CheckDefectcode(input))
                {
                    ret.ClearTarget = "CSN";
                    ret.FocusTarget = "SN";
                    return SUCCESS(ret);
                }
            }

            var midR = await CheckSn(station, req.Wo, sn, req.Box, req.Carton, req.Pallet);
            if(!midR.Ok)
            {
                ret.FocusTarget = "SN";
                return ToResponse(new ApiResult((int)ResultCode.FAIL, midR.Msg, ret));
            }

            var oldCarton = midR.OldCarton;
            var oldPallet = midR.OldPallet;
            var oldBox = midR.OldBox;

            var boxEnabled = req.BoxEnabled;
            string? msg;
            if (oldPallet != "N/A" && oldCarton != "N/A")
            {
                msg = sn + " OK " + Environment.NewLine
                        + "Pallet No : " + oldPallet + Environment.NewLine
                        + "Carton No : " + oldCarton;

                if (!boxEnabled || (boxEnabled && oldBox != "N/A"))
                {
                    if (boxEnabled)
                        msg += Environment.NewLine + "Box No : " + oldBox;

                    await service.PackingRepackGo(station, "BOX", sn, empNo);
                    ret.FocusTarget = "SN";
                }

                return ToResponse(new ApiResult((int)ResultCode.FAIL, msg, ret));
            }

            var boxQty = req.BoxQty;
            var boxQtyEnabled = boxEnabled && pkAction != 2 && pkAction < 4;
            if (boxQty != -1 && boxQtyEnabled)
            {
                if(req.RefreshQty)
                {
                    msg = await RefreshBoxQty(ret, "Box", req.Box);
                    if(!string.IsNullOrEmpty(msg))
                    {
                        return ToResponse(new ApiResult((int)ResultCode.FAIL, msg, ret));
                    }
                }
                
                if(boxQty >= req.BoxCty)
                {
                    return ToResponse(new ApiResult((int)ResultCode.FAIL, "Please Close Box", ret));
                }
            }

            if(pkAction != 2 && pkAction != 3)
            {
                var cartonQty = req.CartonQty;
                if(cartonQty != -1)
                {
                    if(cartonQty >= req.CartonCty)
                    {
                        return ToResponse(new ApiResult((int)ResultCode.FAIL, "Please Close Carton", ret));
                    }
                }
            }

            if (pkAction == 0 || pkAction == 2)
            {
                var palletQty = req.PalletQty;
                if (palletQty != -1)
                {
                    if (palletQty >= req.PalletCty)
                    {
                        return ToResponse(new ApiResult((int)ResultCode.FAIL, "Please Close Pallet", ret));
                    }
                }
            }

            bool ok;
            (ok, msg) = await F_CHECK_DUP_NO(req);
            if (ok!)
            {
                return ToResponse(new ApiResult((int)ResultCode.FAIL, msg, ret));
            }

            var newCSN = string.Empty;
            if (req.SysCreate)
            {
                var uid = HttpContext.GetUId();
                (ok, msg, newCSN) = await service.GetNewNo("CSN", req.Wo, uid);
                if(!ok)
                {
                    ret.FocusTarget = "SN";
                    return ToResponse(new ApiResult((int)ResultCode.FAIL, msg, ret));
                }

                (_, newCSN) = await service.Get_NextNewNo("CSN", req.Wo, newCSN, uid);

                ret.NewCSN = newCSN;
            }
            else if(req.SameSN)
            {
                ret.NewCSN = sn;
            }
            else if(!req.NotChange)
            {
                ret.FocusTarget = "CSN";
                return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "SN OK", ret));
            }

            (ok, msg) = await Input_SN(req, newCSN);

            await UpdatePackQty(req, ret);

            if (ok)
            {
                return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "SN OK", ret));
            }
            else {
                ret.FocusTarget = "SN";
                return ToResponse(new ApiResult((int)ResultCode.FAIL, msg, ret));
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetLabelPrintData([FromQuery]string inputData, [FromQuery]string labelType)
        {
            var (msg, data) = await service.GetLabelVarsPrintData(inputData, labelType);
            if (string.IsNullOrEmpty(msg))
                return SUCCESS(data);
            else
                return ToResponse(ResultCode.FAIL, msg);
        }

        [HttpGet]
        public async Task<IActionResult> CheckPackNo(string packNo, string type)
        {
            return SUCCESS(await service.CheckPackNo(packNo, type));
        }

        [HttpGet]
        public async Task<IActionResult> CheckPackNoByPack(string packNo, string type)
        {
            return SUCCESS(await service.CheckPackNoByPack(packNo, type));
        }

        [HttpGet]
        public async Task<IActionResult> DeletePackNoByPack(string packNo, string type)
        {
            await service.DeletePackNoByPack(packNo, type);
            return SUCCESS(true);
        }

        [HttpGet]
        public async Task<IActionResult> ClosePackNoByPack(string packNo, string type)
        {
            await service.ClosePackNoByPack(packNo, type);
            return SUCCESS(true);
        }

        [HttpGet]
        public async Task<IActionResult> SavePackForceClose(string packNo, string packType)
        {
            await service.SavePackForceClose(packNo, packType, HttpContext.GetName());
            return SUCCESS(true);
        }

        [HttpGet]
        public async Task<IActionResult> GetPkConfigData()
        {
            return SUCCESS(await service.GetPkConfigData());
        }

        [HttpGet]
        public async Task<IActionResult> GetModuleParamList(string station)
        {
            return SUCCESS(await service.GetModuleParamList(station));
        }

        [HttpPost]
        public async Task<IActionResult> SavePkConfig(PkConfigSaveDto config)
        {
            await service.SavePkConfig(config, HttpContext.GetName());
            return SUCCESS(null);
        }
    }
}
