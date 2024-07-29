using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business;
using ZR.Model;
using ZR.Service.ToolingManagement.IService;
using ZR.Service.WoManagement.IService;
using ZR.Model.Dto.WorkOrder;
using ZR.Model.Dto;
using ZR.Infrastructure.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic;
using JinianNet.JNTemplate;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using ZR.Model.Dto.Machine;
using System.Security.Policy;
using System.Xml;

namespace ZR.Admin.WebApi.Controllers.WoManagement
{
    /// <summary>
    /// 工单派工
    /// </summary>
    [Route("womanagement/WoAssignment/[action]")]
    public class WoAssignmentController : BaseController
    {

        IWoAssignmentService _woAssignmentService;

        public WoAssignmentController(IWoAssignmentService _woAssignmentService)
        {
            this._woAssignmentService = _woAssignmentService;
        }

        /// <summary>
        /// 获取数据list
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetList([FromBody] WoAssignmentFilter req)
        {
            if (req.AssignStatus != "ALL" && req.AssignStatus != "Y" && req.AssignStatus != "N")
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请选择正确的派工状态"));
            }

            if (req.WoCreateDateStart >= req.WoCreateDateEnd)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请选择正确的显示时间范围"));
            }

            // 获取数据
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            var resData = _woAssignmentService.GetList(site, req.WoCreateDateStart, req.WoCreateDateEnd);
            var temp = resData;

            // 根据派工状态获取数据
            if (req.AssignStatus != "ALL")
            {
                resData = resData.FindAll(x => x.AssignStatus == req.AssignStatus);
            }

            // 根据筛选条件获取数据
            if (req.filterField != "" && req.filterValue != "")
            {
                if (req.filterField == "WoScheduleStartDate" || req.filterField == "WoScheduleCloseDate" || req.filterField == "WoAssingStartDate" || req.filterField == "WoAssingEndDate")
                {
                    if (req.startDate >= req.endDate)
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请输入正确的时间范围"));
                    }
                    if (req.filterField == "WoScheduleStartDate")
                    {
                        resData = resData.FindAll(x => x.WoScheduleStartDate >= req.startDate && x.WoScheduleStartDate <= req.endDate);
                    }
                    if (req.filterField == "WoScheduleCloseDate")
                    {
                        resData = resData.FindAll(x => x.WoScheduleCloseDate >= req.startDate && x.WoScheduleCloseDate <= req.endDate);
                    }
                    if (req.filterField == "WoAssingStartDate")
                    {
                        resData = resData.FindAll(x => x.WoAssingStartDate >= req.startDate && x.WoAssingStartDate <= req.endDate);
                    }
                    if (req.filterField == "WoAssingEndDate")
                    {
                        resData = resData.FindAll(x => x.WoAssingEndDate >= req.startDate && x.WoAssingEndDate <= req.endDate);
                    }

                }
                else
                {
                    if (req.filterField == "WorkOrder")
                    {
                        resData = resData.FindAll(x => x.WorkOrder.Contains(req.filterValue));
                    }
                    if (req.filterField == "Ipn")
                    {
                        resData = resData.FindAll(x => x.Ipn.Contains(req.filterValue));
                    }
                    if (req.filterField == "StationType")
                    {
                        resData = resData.FindAll(x => x.StationType.Contains(req.filterValue));
                    }
                    if (req.filterField == "TopWorkOrder")
                    {
                        resData = resData.FindAll(x => x.TopWorkOrder.Contains(req.filterValue));
                    }

                }

            }

            return SUCCESS(resData);
        }

        /// <summary>
        /// 派工前检查
        /// </summary>
        /// <param name="assigned">派工:assign,修改:modify</param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("assigned={assigned}")]
        public IActionResult AssignBefore(string assigned, [FromBody] WoAssignmentDto dto)
        {
            if (dto == null)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Please choose one record at least!"));
            }

            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();

            ExecuteResult exeRes = _woAssignmentService.CheckPKG(dto.WorkOrder);
            if (!exeRes.Status)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
            }

            if (dto.AssignStatus == "Y" && assigned.Equals("assign"))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"the Workorder has Assigned!"));
            }

            if (dto.AssignStatus == "N" && assigned.Equals("modify"))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"the workorder you choose must be Assigned!"));
            }

            //检查是否存在这样的工序的设备组，如果不存在，就提醒用户创建该工序的设备组
            if (!_woAssignmentService.ExistedWorkproc(dto.StationType))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"the workproc of the machine group not existed!"));
            }

            exeRes = _woAssignmentService.CheckReleaseWo(dto.WorkOrder, site);
            if (!exeRes.Status)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"工单不是下达状态，不允许派工!"));
            }

            // 检查设计卡版本，不存在设计卡版本时查询最新生效的设计卡，将版本号更新到工单。若未找到抛出错误提示
            exeRes = _woAssignmentService.CheckDrawCardVersion(dto.WorkOrder, site);
            if (!exeRes.Status)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
            }

            //倒轴工序
            exeRes = _woAssignmentService.PackCheck(dto.WorkOrder, site);
            if (!exeRes.Status)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
            }

            return SUCCESS("OK");

        }

        /// <summary>
        /// 获取已经派工的记录
        /// </summary>
        /// <param name="wo"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWoAssigns([FromQuery] string wo)
        {
            var assigns = _woAssignmentService.GetWoAssignment(wo);
            DataTable dt_assigned = new DataTable();
            if (assigns.Rows.Count > 0)
            {
                var by_machine_group = assigns.Rows[0]["BY_MACHINE_GROUP"].ToString();
                if (by_machine_group == "GROUP")
                {
                    var groups = assigns.AsEnumerable()
                                       .Select(r => r["MACHINE_GROUP"].ToString())
                                       .Distinct()
                                       .ToArray();
                    dt_assigned = _woAssignmentService.getMachine(groups);

                }
                if (by_machine_group == "MACHINE")
                {
                    var machines = assigns.AsEnumerable()
                   .Select(r => r["MACHINE"].ToString())
                   .Distinct()
                   .ToArray();
                    dt_assigned = _woAssignmentService.getMachine(machines, "");
                }
            }

            return SUCCESS(dt_assigned);
        }


        /// <summary>
        /// 修改派工后的派工计划时间
        /// </summary>
        /// <param name="type">0:修改开始时间；1:修改结束时间</param>
        /// <param name="mWoAssignmentDto"></param>
        /// <returns></returns>
        [HttpPost("type={type}")]
        public IActionResult ModifyAssingDate(int type, [FromBody] MWoAssignmentDto mWoAssignmentDto)
        {
            string empno = HttpContext.GetName() == "" ? "1" : HttpContext.GetName();
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();

            var dto = mWoAssignmentDto.Adapt<MWoAssignment>();
            //检查是否存在派工记录
            var info = _woAssignmentService.CehckAssignInfo(dto);
            if (info == null)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"数据不存在,请刷新后重新操作!"));
            }

            // 修改计划开始时间
            if (type == 0)
            {
                if (dto.WoAssignStartDate == null || dto.WoAssignEndDate == null || dto.WoAssignStartDate > dto.WoAssignEndDate)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"派工计划开始日期不得大于派工计划结束日期!"));
                }
                _woAssignmentService.ModifyAssignDate(0, dto.WorkOrder, dto.Machine, empno, dto.WoAssignStartDate.ToString(), dto.WoAssignEndDate.ToString());
            }


            // 修改计划结束时间
            if (type == 1)
            {
                if (dto.WoAssignStartDate == null || dto.WoAssignEndDate == null || dto.WoAssignStartDate > dto.WoAssignEndDate)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"派工计划开始日期不得大于派工计划结束日期!"));
                }
                _woAssignmentService.ModifyAssignDate(1, dto.WorkOrder, dto.Machine, empno, dto.WoAssignStartDate.ToString(), dto.WoAssignEndDate.ToString());
            }

            // 修改 WO_BASE 的计划派工时间
            _woAssignmentService.ModifyWoBaseAssignDate(dto.WorkOrder, site);

            return SUCCESS("OK");
        }

        /// <summary>
        /// 发送QMS(工序必须是 外被押出)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ResendQms([FromBody] MWoAssignment dto)
        {
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            string empno = HttpContext.GetName() == "" ? "1" : HttpContext.GetName();

            if (dto.StationType.Equals("外被押出"))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"只有外被押出站点允许操作!"));
            }
            //检查是否存在派工记录
            var info = _woAssignmentService.CehckAssignInfo(dto);
            if (info == null)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"数据不存在,请刷新后重新操作!"));
            }

            ExecuteResult send = _woAssignmentService.SendQms(dto.WorkOrder, site, empno);
            if (!send.Status)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, send.Message));
            }
            return SUCCESS("OK");
        }

        
        


        /// <summary>
        /// 获取设备组
        /// </summary>
        /// <param name="stationType">工序</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMachineGroupToWoAssign([FromQuery] string stationType)
        {
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            return SUCCESS(_woAssignmentService.GetMachineGroupToWoAssign(stationType, site));
        }


        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="machineGroup">设备组名称</param>
        /// <param name="wo"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMachineToWoAssign([FromQuery] string machineGroup, [FromQuery] string wo)
        {
            // 通过设备组获取设备 
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            var machinelist = _woAssignmentService.GetMachineToWoAssign(machineGroup, site);


            // 通过工单获取派工信息
            var assigns = _woAssignmentService.GetWoAssignment(wo);
            DataTable dt_assigned = new DataTable();
            if (assigns.Rows.Count > 0)
            {
                var by_machine_group = assigns.Rows[0]["BY_MACHINE_GROUP"].ToString();
                if (by_machine_group == "GROUP")
                {
                    var groups = assigns.AsEnumerable()
                                       .Select(r => r["MACHINE_GROUP"].ToString())
                                       .Distinct()
                                       .ToArray();
                    dt_assigned = _woAssignmentService.getMachine(groups);

                }
                if (by_machine_group == "MACHINE")
                {
                    var machines = assigns.AsEnumerable()
                   .Select(r => r["MACHINE"].ToString())
                   .Distinct()
                   .ToArray();
                    dt_assigned = _woAssignmentService.getMachine(machines, "");
                }
            }
            List<MachineToWoAssign> machinelist2 = new List<MachineToWoAssign>();
             //machinelist2 = machinelist;
            foreach (var row in machinelist)
            {
                int num = dt_assigned.AsEnumerable().Where(a => a["MACHINE"].ToString() == row.machine).Count();

                int num2 = machinelist2.AsEnumerable().Where(a => a.machine == row.machine).Count();
                if (num == 0 && num2 == 0)
                {
                    //machinelist2.Remove(row);
                    machinelist2.Add(row);
                }
                
            }

            return SUCCESS(machinelist2);
        }

        /// <summary>
        /// 派工确认
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Confirm([FromBody] WoAssignVo dto)
        {
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            string empno = HttpContext.GetName() == "" ? "1" : HttpContext.GetName();

            if (dto == null || dto.pendingList.Count <= 0 || dto.confirmList.Count <= 0)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请输入正确的参数"));
            }

            if (string.IsNullOrEmpty(dto.startDate) || string.IsNullOrEmpty(dto.endDate))
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请选择派工计划开始和结束时间"));
            }
            else
            {
                if (!DateTime.TryParse(dto.startDate, out DateTime dt1)
                  || !DateTime.TryParse(dto.endDate, out DateTime dt2)
                  || dt1 > dt2)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"请选择正确的派工计划开始和结束时间"));
                    
                }
            }

            var wos = dto.pendingList.AsEnumerable()
                .Select(w => w.WorkOrder.ToString())
                .Distinct()
                .ToArray();

            var wos_wb = dto.pendingList.AsEnumerable()
                .Where(w => "外被押出".Equals(w.StationType))
                .Select(w => w.WorkOrder.ToString())
                .Distinct()
                .ToArray();

            foreach (var temp in wos_wb)
            {

                ExecuteResult send = _woAssignmentService.SendQms(temp, site, empno);
                if (!send.Status)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, "传送QMS失败，请重新派工"+ send.Message));
                }
            }

            var workproc = dto.pendingList.AsEnumerable().First().StationType.ToString();
            var groups = dto.confirmList.AsEnumerable()
                                                       .Select(mg => mg.machineGroupName.ToString())
                                                       .Distinct()
                                                       .ToArray();
            var machines = dto.confirmList.AsEnumerable()
                                           .Select(m => m.machine.ToString())
                                           .Distinct()
                                           .ToArray();

            if (dto.operate.ToUpper().Equals("MODIFY"))
            {
                var assigns = _woAssignmentService.getAssignment(dto.pendingList.AsEnumerable().First().WorkOrder);
                var by_machine_group = assigns.Rows[0]["BY_MACHINE_GROUP"].ToString();
                if (by_machine_group == "GROUP")
                {
                    var assigns_group = assigns.AsEnumerable()
                                             .Select(a => a["MACHINE_GROUP"].ToString())
                                             .Distinct()
                                             .ToArray();
                    var newrows = groups.Where(mg => !assigns_group.Contains(mg));
                    if (newrows.Count() == 0)
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"No Record Add in! "));
                    }
                    groups = newrows.ToArray();
                }
                if (by_machine_group == "MACHINE")
                {
                    var assigns_machine = assigns.AsEnumerable()
                         .Select(a => a["MACHINE"].ToString())
                         .Distinct()
                         .ToArray();
                    var newrows = machines.Where(m => !assigns_machine.Contains(m));
                    if (newrows.Count() == 0)
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"No Record Add in! "));
                    }
                    machines = newrows.ToArray();
                }
            }

            //构造xml字符串
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            // <ROWS>
            XmlElement root = doc.CreateElement("ROWS");
            doc.AppendChild(root);
            // <ROWS>/<ROW>
            foreach (var wo in wos)
            {
                //foreach (var it in cbxByMG.Checked ? groups : machines)
                foreach (var it in machines)
                {
                    XmlElement row = doc.CreateElement("ROW");
                    //创建子节点
                    XmlElement w = doc.CreateElement("WO");
                    w.InnerText = wo;
                    row.AppendChild(w);

                    XmlElement byw = doc.CreateElement("BYW");

                    //byw.InnerText = cbxByMG.Checked ? "GROUP" : "MACHINE";
                    byw.InnerText = "MACHINE";
                    row.AppendChild(byw);

                    XmlElement code = doc.CreateElement("CODE");
                    code.InnerText = it;
                    row.AppendChild(code);

                    XmlElement wp = doc.CreateElement("WORKPROC");
                    wp.InnerText = workproc;
                    row.AppendChild(wp);

                    XmlElement status = doc.CreateElement("STATUS");
                    status.InnerText = "Y";
                    row.AppendChild(status);

                    XmlElement creator = doc.CreateElement("CREATOR");
                    creator.InnerText = empno;
                    row.AppendChild(creator);

                    XmlElement date1 = doc.CreateElement("WO_ASSING_START_DATE");
                    date1.InnerText = dto.startDate;
                    row.AppendChild(date1);

                    XmlElement date2 = doc.CreateElement("WO_ASSING_END_DATE");
                    date2.InnerText = dto.endDate;
                    row.AppendChild(date2);

                    root.AppendChild(row);
                }

            }
            var xmldata = doc.InnerXml;
            var result = _woAssignmentService.ModifyData(xmldata);
            if (!result.Status)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"ModifyData Failed:{result.Message}"));
            }

            return SUCCESS("OK");
        }
     
        


    }
}
