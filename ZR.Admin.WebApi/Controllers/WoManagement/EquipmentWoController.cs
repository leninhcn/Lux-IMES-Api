using Aliyun.OSS;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Dto.WorkOrder;
using ZR.Service.ToolingManagement.IService;
using ZR.Service.WoManagement.IService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Admin.WebApi.Controllers.WoManagement
{
    /// <summary>
    /// 设备派工
    /// </summary>
    [Route("womanagement/equipmentwo/[action]")]
    public class EquipmentWoController : BaseController
    {

        public IEquipmentWoService _equipmentWoService;
        public EquipmentWoController(IEquipmentWoService _equipmentWoService)
        {
            this._equipmentWoService = _equipmentWoService;
        }




        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ShowData([FromQuery] EquipmentWoFilter filter, [FromQuery] PagerInfo pager)
        {
            var site = HttpContext.GetSite();
            site = site == "" ? "DEF" : site;

            var empno = HttpContext.GetName();
            empno = empno == null ? "1" : empno;

            try
            {
                var startDate = DateTime.Parse(filter.startDate);

                var endDate = DateTime.Parse(filter.endDate);

                if (startDate >= endDate)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"开始时间必须大于结束时间"));
                }
                else
                {
                    filter.startDate = startDate.ToString("yyyy-MM-dd");
                    filter.endDate = endDate.ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"开始或结束时间格式错误（正确格式 yyyy-MM-dd）"));
            }

            var response = _equipmentWoService.ShowData(filter, pager, empno);
            return SUCCESS(response);

        }

        /// <summary>
        /// 获取车间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult getChejian()
        {
            var site = HttpContext.GetSite();
            site = site == "" ? "DEF" : site;

            var response = _equipmentWoService.getChejian(site);
            return SUCCESS(response);
        }

        /// <summary>
        /// 获取设备编号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetEquipmentCode()
        {
            string devType = "自动切压机";
            var response = _equipmentWoService.GetEquipmentCode(devType);
            return SUCCESS(response);
        }

        /// <summary>
        /// 检查设备编号状态
        /// </summary>
        /// <param name="devCode">设备编号</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckEquipmentCodeStatus(string devCode)
        {
            var dt = _equipmentWoService.CheckEquipmentCodeStatus(devCode);
            if (dt.Rows.Count>= 1)
            {
                string status = dt.Rows[0]["Status"].ToString();
                if (status.Equals("正常") == false)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"设备[{devCode}] 状态是:{status}不能选取！"));
                }

            }
            return SUCCESS("OK");
        }

        /// <summary>
        /// 获取生产批次
        /// </summary>
        /// <param name="Chejian"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProductLot(string Chejian)
        {
            var response = _equipmentWoService.GetProductLot(Chejian);
            return SUCCESS(response);
        }


        /// <summary>
        /// 初始化用户按钮权限
        /// </summary>
        /// <param name="pAction">首件打印/末件打印/全打/手打</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ModuleAuth([FromQuery] string pAction)
        {
            var empno = HttpContext.GetName();
            empno = empno == null ? "1" : empno;

            var site = HttpContext.GetSite();
            site = site == "" ? "DEF" : site;

            var response = _equipmentWoService.ModuleAuth("ProdEquipmentWo", pAction, site, empno);
            return SUCCESS(response);
        }

        /// <summary>
        /// 获取工单线别信息(派工任务选择)
        /// </summary>
        /// <param name="WorkNum">生产批次</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWorkLine([FromQuery] string WorkNum)
        {
            var response = _equipmentWoService.getWorkLine(WorkNum);
            return SUCCESS(response);
        }

        //流程图
        /// <summary>
        /// 获取操作流程
        /// </summary>
        /// <param name="billNum">单据编号</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult getTraceAll([FromQuery] string billNum)
        {
            var response = _equipmentWoService.getTraceAll(billNum);
            return SUCCESS(response);
        }


        // 痕迹
        /// <summary>
        /// 通过单据编号获取信息
        /// </summary>
        /// <param name="billNum">单据编号</param>
        /// <param name="strType">审批/会签/驳回/收回/委托</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult getTraceByType([FromQuery] string billNum, [FromQuery] string strType)
        {
            if (string.IsNullOrEmpty(billNum))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请求参数错误，单据编号不能为空"));
            }
            var response = _equipmentWoService.getTraceByType(billNum, strType);
            return SUCCESS(response);
        }

        /// <summary>
        /// 修改时，派工任务清单
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult getDispatchList_b(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请求参数错误，Id不能为空"));
            }
            var response = _equipmentWoService.getDispatchList_b(Id);
            return SUCCESS(response);
        }


        // 转交下一步 Transfer to the next step
        /// <summary>
        /// 转交下一步 或 暂存
        /// </summary>
        /// <param name="diSpatchlistHBDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult TransferToTheNextStep([FromBody] HjxsPieWtEquDiSpatchlistHBDto diSpatchlistHBDto)
        {
            var empno = HttpContext.GetName();
            empno = empno == null ? "1" : empno;

            if (diSpatchlistHBDto == null)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请求参数不能为空"));
            }

            if (diSpatchlistHBDto.diSpatchlistH == null)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"未选择必要的设备派工信息!"));
            }
            else
            {
                if (string.IsNullOrEmpty(diSpatchlistHBDto.diSpatchlistH.ProLot))
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"生产批次数据不能为空!"));
                }

                if (string.IsNullOrEmpty(diSpatchlistHBDto.diSpatchlistH.EquCode))
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"设备编号不能为空!"));
                }

                if (string.IsNullOrEmpty(diSpatchlistHBDto.diSpatchlistH.EquType))
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"设备类别不能为空!"));
                }

                if (diSpatchlistHBDto.diSpatchlistH.CabQty <= 0)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"每扎线数量必须大于0!"));
                }
                else
                {
                    try
                    {
                        Convert.ToInt32(diSpatchlistHBDto.diSpatchlistH.CabQty);
                    }
                    catch (Exception ex)
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"每扎线数量：数据格式不合法,必须为数值!"));
                    }
                }
            }

            if (diSpatchlistHBDto.dispatchListBs == null || diSpatchlistHBDto.dispatchListBs.Count <= 0)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"没有派工明细,请先进行派工任务选择!"));
            }

            HjxsPieWtEquDiSpatchListH diSpatchlistHDto = diSpatchlistHBDto.diSpatchlistH;
            if (diSpatchlistHBDto.sUpdateType.ToUpper() == "APPEND")
            {
                string vbillCode = _equipmentWoService.GetNoteNum("EQP");
                if (vbillCode.StartsWith("EQP") == false)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"EQP单据获取失败" + vbillCode));
                }
                string strID = _equipmentWoService.GetGuID();
                diSpatchlistHDto.Id = strID;
                diSpatchlistHDto.VbillCode = vbillCode;

                bool bSave = _equipmentWoService.SaveDispatchList_h(diSpatchlistHDto, empno);
                if (bSave)
                {
                    foreach (var dispatchListB in diSpatchlistHBDto.dispatchListBs)
                    {
                        if (_equipmentWoService.SaveDispatchList_B(dispatchListB) == false)
                        {
                            return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"数据保存失败:HJXS_Pie_WtEquDispatchList_b"));
                        }
                    }
                    //更新投料数据
                    foreach (var sld in diSpatchlistHBDto.scanLableDatas)
                    {
                        sld.Note = vbillCode;
                        _equipmentWoService.ModifyScanLableData(sld);
                    }
                }
                else
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"数据保存失败:HJXS_Pie_WtEquDispatchList_h"));
                }
            }
            else
            {
                //转交下一步
                if (diSpatchlistHBDto.sUpdateType.ToUpper() == "NEXT")
                {
                    if (diSpatchlistHDto.WfState.Equals("生产班长派工"))
                    {
                        diSpatchlistHDto.WfState = "作业员接单";
                    }
                    else if (diSpatchlistHDto.WfState.Equals("作业员接单"))
                    {
                        diSpatchlistHDto.WfState = "IPQC确认";
                    }
                    else if (diSpatchlistHDto.WfState.Equals("IPQC确认"))
                    {
                        diSpatchlistHDto.WfState = "完成";
                    }
                    else
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"单据已完成,无法再转交下一步!"));
                    }
                }
                //驳回
                if (diSpatchlistHBDto.sUpdateType.ToUpper() == "REJECT")
                {
                    if (diSpatchlistHDto.WfState.Equals("生产班长派工"))
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"单据刚新建,无法驳回!"));
                    }
                    else if (diSpatchlistHDto.WfState.Equals("作业员接单"))
                    {
                        diSpatchlistHDto.WfState = "生产班长派工";
                    }
                    else if (diSpatchlistHDto.WfState.Equals("IPQC确认"))
                    {
                        diSpatchlistHDto.WfState = "作业员接单";
                    }
                    else
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"单据已完成,无法驳回!"));
                    }
                }

                bool bSave = _equipmentWoService.ModifyDispatchList_h(diSpatchlistHDto, empno, diSpatchlistHBDto.sUpdateType);
                if (bSave)
                {
                    foreach (var dispatchListB in diSpatchlistHBDto.dispatchListBs)
                    {

                        if (dispatchListB.Id.Equals("0"))
                        {
                            dispatchListB.Id = _equipmentWoService.GetGuID();

                            if (_equipmentWoService.SaveDispatchList_B(dispatchListB) == false)
                            {
                                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"数据保存失败:HJXS_Pie_WtEquDispatchList_b"));
                            }
                        }
                        else
                        {
                            if (_equipmentWoService.ModifyDispatchList_B(dispatchListB) == false)
                            {
                                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"数据保存失败:HJXS_Pie_WtEquDispatchList_b"));
                            }
                        }


                    }
                    if (diSpatchlistHBDto.scanLableDatas!= null && diSpatchlistHBDto.scanLableDatas.Count > 0)
                    {
                        //更新投料数据
                        foreach (var sld in diSpatchlistHBDto.scanLableDatas)
                        {
                            sld.Note = diSpatchlistHBDto.diSpatchlistH.VbillCode;
                            _equipmentWoService.ModifyScanLableData(sld);
                        }
                    }
                }
                else
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"数据保存失败:HJXS_Pie_WtEquDispatchList_h"));
                }

            }

            return SUCCESS("OK");

        }

        /// <summary>
        /// 扫描批次信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InputScan([FromBody] InputScanDto dto)
        {
            string str = dto.inputScan;
            string flag = _equipmentWoService.CheckSix(str);
            List<HjxsPieWtEquDispatchListB> HjxsPieWtEquDispatchListBList = new List<HjxsPieWtEquDispatchListB>();
            HjxsPieWtScanLableData sld = new HjxsPieWtScanLableData();
            string isSuccess = "NG";
            if (flag.Equals("1"))
            {
                string Gbillcode = "";
                string Gbatchcode = "";
                string Gproum = "";
                string Gprolot = "";
                string Gmodel = "";
                string Glabqty = "";
                string Glabpage = "";
                List<HjxsPieWtScanLableData> scanLableDataList = null;

                if (str.Contains("[") && str.Contains("]"))   //旧条码
                {
                    Gbillcode = str.Split(']')[0];
                    sld.BillCode = Gbillcode.Split('[')[1];//[发料单号]
                    Gbatchcode = str.Split(']')[1];
                    sld.BatchCode = Gbatchcode.Split('[')[1];//[物料批次号]
                    Gproum = str.Split(']')[2];
                    sld.ProNum = Gproum.Split('[')[1];//[物料编号]
                    Gprolot = str.Split(']')[5];
                    sld.ProLot = Gprolot.Split('[')[1];//[生产批次]
                    Gmodel = str.Split(']')[6];
                    sld.Model = Gmodel.Split('[')[1];//[产品型号]
                    Glabqty = str.Split(']')[9];
                    sld.LabQty = Convert.ToInt32(Glabqty.Split('[')[1]);//[标贴数量]
                    Glabpage = str.Split(']')[10];
                    sld.LabPage = Int32.Parse(Glabpage.Split('[')[1]);//[标贴页码]
                }
                else if (str.Contains("$"))  //新条码
                {
                    string[] strList = str.Split('$');
                    Gbillcode = strList[0];
                    sld.BillCode = Gbillcode;//[发料单号]ReelNo
                    Gbatchcode = strList[1];
                    sld.BatchCode = Gbatchcode;//[供应商代码]
                    Gproum = strList[3];
                    sld.ProNum = Gproum;//[物料编号]
                    Gprolot = strList[5];
                    sld.ProLot = Gprolot;//[生产批号]
                    Gmodel = strList[6];
                    sld.Model = Gmodel;//[生产日期]
                    Glabqty = strList[7];
                    sld.LabQty = Convert.ToDouble(Glabqty);//[标贴数量]
                    Glabpage = strList[9];
                    sld.LabPage = Int32.Parse(Glabpage);//[BIN号]
                }
                else
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"扫描的条码无效,请扫描正确的条码,谢谢!"));
                }
                scanLableDataList = _equipmentWoService.GetScanLableData(sld);

                if (scanLableDataList.Count == 0)
                {
                    string strID = _equipmentWoService.GetGuID();
                    sld.Id = strID;
                    if (_equipmentWoService.SaveScanLableData(sld) == false)
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"数据保存失败:HJXS_PIE_WTSCANLABLEDATA"));

                    }
                    sld.LeftQty = sld.LabQty;
                    sld.ScanQty = 0;
                }
                else
                {
                    sld.Id = scanLableDataList[0].Id;
                    sld.LeftQty = scanLableDataList[0].LeftQty;// Convert.ToInt32(dt.Rows[0]["LEFTQTY"].ToString());
                    sld.ScanQty = scanLableDataList[0].ScanQty; //Convert.ToInt32(dt.Rows[0]["SCANQTY"].ToString());
                }

                if (sld.LeftQty == 0)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"该物料条码的系统剩余数量为0,请扫描其它条码,谢谢!"));
                }




                foreach (var temp in dto.dispatchListBs)
                {
                    if (sld.ProNum.Equals(temp.WireProNum) && temp.CableQty > temp.CableFinishQty)
                    {
                        string pcc = sld.BatchCode + "[#]" + sld.Id;
                        if (temp.BatchCode.Contains(pcc) == false)
                        {
                            isSuccess = "OK";

                            if (String.IsNullOrEmpty(temp.BatchCode) && String.IsNullOrEmpty(temp.TerminalBatchA) && String.IsNullOrEmpty(temp.TerminalBatchB) && String.IsNullOrEmpty(temp.ModelBatchA) && String.IsNullOrEmpty(temp.ModelBatchB))
                            {
                                //dr["开始时间"]
                                temp.StartTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                //dr["投入状态"] = "处理中";
                                temp.FiniStatue = "处理中";
                            }

                            if (sld.LeftQty <= temp.CableQty - temp.CableFinishQty)
                            {
                                temp.CableFinishQty = temp.CableFinishQty + sld.LeftQty;
                                //dr["线材来料批次"] 
                                temp.BatchCode = temp.BatchCode + pcc + "[#]" + sld.LeftQty.ToString() + ",";
                                //dr["已扫数量"]
                                sld.ScanQty = sld.ScanQty + sld.LeftQty;
                                sld.LeftQty = 0;
                                break;
                            }
                            else
                            {
                                double NeedQty = temp.CableQty - temp.CableFinishQty;
                                temp.CableFinishQty = temp.CableQty;
                                //dr["线材来料批次"]
                                temp.BatchCode = temp.BatchCode + pcc + "[#]" + NeedQty.ToString() + ",";
                                //dr["已扫数量"] = CableFinishQty.ToString();
                                sld.ScanQty = sld.ScanQty + NeedQty;
                                sld.LeftQty = sld.LeftQty - NeedQty;
                            }
                        }
                    }
                    //匹配A端子
                    if (sld.ProNum.Equals(temp.TerminalNoF) && temp.TerAQty > temp.TerAFinishQty)
                    {
                        string pcc = sld.BatchCode + "[#]" + sld.Id;
                        if (temp.TerminalBatchA.Contains(pcc) == false)
                        {
                            isSuccess = "OK";

                            if (String.IsNullOrEmpty(temp.BatchCode) && String.IsNullOrEmpty(temp.TerminalBatchA) && String.IsNullOrEmpty(temp.TerminalBatchB) && String.IsNullOrEmpty(temp.ModelBatchA) && String.IsNullOrEmpty(temp.ModelBatchB))
                            {
                                //dr["开始时间"]
                                temp.StartTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                //dr["投入状态"] = "处理中";
                                temp.FiniStatue = "处理中";
                            }
                            if (sld.LeftQty <= temp.TerAQty - temp.TerAFinishQty)
                            {
                                temp.TerAFinishQty = temp.TerAFinishQty + sld.LeftQty;
                                //dr["A端子批次"]
                                temp.TerminalBatchA = temp.TerminalBatchA + pcc + "[#]" + sld.LeftQty.ToString() + ",";
                                // dr["A已扫数量"] = temp.TerAFinishQty.ToString();
                                sld.ScanQty = sld.ScanQty + sld.LeftQty;
                                sld.LeftQty = 0;
                                break;
                            }
                            else
                            {
                                double NeedQty = temp.TerAQty - temp.TerAFinishQty;
                                temp.TerAFinishQty = temp.TerAQty;
                                //dr["A端子批次"]
                                temp.TerminalBatchA = temp.TerminalBatchA + pcc + "[#]" + NeedQty.ToString() + ",";
                                //dr["A已扫数量"] = temp.TerAFinishQty.ToString();
                                sld.ScanQty = sld.ScanQty + NeedQty;
                                sld.LeftQty = sld.LeftQty - NeedQty;
                            }
                        }
                    }
                    //匹配B端子
                    if (sld.ProNum.Equals(temp.TerminalNoA) && temp.TerBQty > temp.TerBFinishQty)
                    {
                        string pcc = sld.BatchCode + "[#]" + sld.Id;
                        if (temp.TerminalBatchB.Contains(pcc) == false)
                        {
                            isSuccess = "OK";

                            if (String.IsNullOrEmpty(temp.BatchCode) && String.IsNullOrEmpty(temp.TerminalBatchA) && String.IsNullOrEmpty(temp.TerminalBatchB) && String.IsNullOrEmpty(temp.ModelBatchA) && String.IsNullOrEmpty(temp.ModelBatchB))
                            {
                                //dr["开始时间"]
                                temp.StartTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                //dr["投入状态"] = "处理中";
                                temp.FiniStatue = "处理中";
                            }

                            if (sld.LeftQty <= temp.TerBQty - temp.TerBFinishQty)
                            {
                                temp.TerBFinishQty = temp.TerBFinishQty + sld.LeftQty;
                                //dr["B端子批次"]
                                temp.TerminalBatchB = temp.TerminalBatchB + pcc + "[#]" + sld.LeftQty.ToString() + ",";
                                //dr["B已扫数量"] = temp.TerBFinishQty.ToString();
                                sld.ScanQty = sld.ScanQty + sld.LeftQty;
                                sld.LeftQty = 0;
                                break;
                            }
                            else
                            {
                                double NeedQty = temp.TerBQty - temp.TerBFinishQty;
                                temp.TerBFinishQty = temp.TerBQty;
                                //dr["B端子批次"]
                                temp.TerminalBatchB = temp.TerminalBatchB + pcc + "[#]" + NeedQty.ToString() + ",";
                                //dr["B已扫数量"] = temp.TerBFinishQty.ToString();
                                sld.ScanQty = sld.ScanQty + NeedQty;
                                sld.LeftQty = sld.LeftQty - NeedQty;
                            }
                        }
                    }

                    if (temp.CableFinishQty == temp.CableQty && temp.TerAFinishQty == temp.TerAQty && temp.TerBFinishQty == temp.TerBQty && String.IsNullOrEmpty(temp.ModelBatchA) == false && String.IsNullOrEmpty(temp.ModelBatchB) == false)
                    {
                        //dr["开始时间"]
                        temp.StartTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        //dr["投入状态"] = "处理中";
                        temp.FiniStatue = "完成";
                    }
                    HjxsPieWtEquDispatchListBList.Add(temp);
                }
                //该条码未匹配到则报错
                if (isSuccess.Equals("NG"))
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"该物料条码没有匹配到任何需求,请继续扫描其它条码进行匹配,谢谢!"));
                }
                else
                {
                    InputScanOut inputScanOut = new InputScanOut();
                    inputScanOut.hjxsPieWtScan = sld;
                    inputScanOut.hjxsPieWtEqus = HjxsPieWtEquDispatchListBList;

                    return SUCCESS(inputScanOut);

                }



            }
            else if (flag.Equals("0"))
            {
                if (str.Contains("[") == false || str.Contains("]") == false)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"该模具条码不合法,请继续扫描其它条码进行匹配,谢谢!"));

                }

            }

            string Gbillcode1 = str.Split(']')[0];
            Gbillcode1 = Gbillcode1.Split('[')[1];//[模具编号]
                                                  //进行模具匹配

            foreach (var temp in dto.dispatchListBs)
            {
                //匹配模具A
                if (String.IsNullOrEmpty(temp.ModelBatchA) && temp.ModelCodeA.Contains(Gbillcode1))
                {
                    isSuccess = "OK";

                    if (String.IsNullOrEmpty(temp.BatchCode) && String.IsNullOrEmpty(temp.TerminalBatchA) && String.IsNullOrEmpty(temp.TerminalBatchB) && String.IsNullOrEmpty(temp.ModelBatchA) && String.IsNullOrEmpty(temp.ModelBatchB))
                    {
                        temp.StartTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        temp.FiniStatue = "处理中";

                    }
                    temp.ModelBatchA = Gbillcode1;
                }
                //匹配模具B
                if (String.IsNullOrEmpty(temp.ModelBatchB) && temp.ModelCodeB.Contains(Gbillcode1))
                {
                    isSuccess = "OK";

                    if (String.IsNullOrEmpty(temp.BatchCode) && String.IsNullOrEmpty(temp.TerminalBatchA) && String.IsNullOrEmpty(temp.TerminalBatchB) && String.IsNullOrEmpty(temp.ModelBatchA) && String.IsNullOrEmpty(temp.ModelBatchB))
                    {
                        //dr["开始时间"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        //dr["投入状态"] = "处理中";
                        temp.StartTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        temp.FiniStatue = "处理中";
                    }

                    //dr["B模具条码"] = Gbillcode;
                    temp.ModelBatchB = Gbillcode1;
                }

                if (temp.CableFinishQty == temp.CableQty && temp.TerAFinishQty == temp.TerAQty && temp.TerBFinishQty == temp.TerBQty && String.IsNullOrEmpty(temp.ModelBatchA) == false && String.IsNullOrEmpty(temp.ModelBatchB) == false)
                {
                    //dr["结束时间"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    //dr["投入状态"] = "完成";
                    temp.EndTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    temp.FiniStatue = "完成";
                }
                HjxsPieWtEquDispatchListBList.Add(temp);
            }
            //该条码未匹配到则报错
            if (isSuccess.Equals("NG"))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"该模具条码没有匹配到任何需求,请继续扫描其它条码进行匹配,谢谢!"));
            }
            else
            {
                InputScanOut inputScanOut = new InputScanOut();
                inputScanOut.hjxsPieWtScan = sld;
                inputScanOut.hjxsPieWtEqus = HjxsPieWtEquDispatchListBList;

                return SUCCESS(inputScanOut);
            }
         
        }

    }
}
