using Microsoft.AspNetCore.Mvc;
using ZR.Model.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;
using MiniExcelLibs;
using System.Runtime.CompilerServices;
using SqlSugar;
using System.Xml.Linq;
using ZR.CodeGenerator.Service;
using System.Data;
using Newtonsoft.Json;
using System.Text.Json;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using ZR.Model.System;
using ZR.Service.System;
using ZR.Service.System.IService;
using Microsoft.Extensions.Options;
using System.Text;

//创建时间：2024-03-29
namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// NPI项目管理
    /// </summary>
    [ApiController]
    [Verify]
    [Route("business/NpiProjet")]
    public class NpiProjetController : BaseController
    {
        private OptionsSetting OptionsSetting;
        private ISysFileService SysFileService;
        // private INpiProjetService NpiProjetService;
        private IWebHostEnvironment WebHostEnvironment;

        public string ToUser = "Jerry.Rong@luxshare-ict.com,Xiao.Liu2@luxshare-ict.com,Zhipan.Yuan@luxshare-ict.com,Qiang.Wang3@luxshare-ict.com,Jamson.Chen@luxshare-ict.com,Yongjie.Zhang@luxshare-ict.com,Xin.Liang2@luxshare-ict.com,Liang.Min@luxshare-ict.com,Shuxin.Fang@luxshare-ict.com,Ruiyuan.Zhang@luxshare-ict.com,Qi.Zhang3@luxshare-ict.com,Ping.Guo2@luxshare-ict.com,Jun.Su@luxshare-ict.com,Dan.Liu5@luxshare-ict.com,Haijun.Liu2@luxshare-ict.com,Linyuan.He@luxshare-ict.com,Yanzhang.Lv@luxshare-ict.com,Guoguang.Li@luxshare-ict.com,Jinlai.Du@luxshare-ict.com,yong.li2@luxshare-ict.com,Weitao.Liu@luxshare-ict.com,Bruze.Liu@luxshare-ict.com,Xingang.Wang@luxshare-ict.com,Tao.Wang2@luxshare-ict.com,Jinhai.Han@luxshare-ict.com,Yongping.Cheng@luxshare-ict.com,Shengjin.Gui@luxshare-ict.com,Guokai.Xie@luxshare-ict.com,Jianbing.Peng@luxshare-ict.com,Qiang.Lii@luxshare-ict.com,Wang.Cao@luxshare-ict.com,Mike.Ye2@luxshare-ict.com,Pei.Wang@luxshare-ict.com,YY.Yang@luxshare-ict.com,wenliang.li@luxshare-ict.com,Fukui.Liu@luxshare-ict.com,Louisa.Tian@luxshare-ict.com,Paula.Ding@luxshare-ict.com,Leon.Ren2@luxshare-ict.com,chao.li2@luxshare-ict.com";

        /// <summary>
        /// NPI项目管理接口
        /// </summary>
        private readonly INpiProjetService _NpiProjetService;

        public NpiProjetController(INpiProjetService NpiProjetService, ISysFileService fileService, IWebHostEnvironment webHostEnvironment, IOptions<OptionsSetting> options)
        {
            _NpiProjetService = NpiProjetService;
            SysFileService = fileService;
            WebHostEnvironment = webHostEnvironment;
            OptionsSetting = options.Value;


        }

        /// <summary>
        /// 查询NPI项目管理列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "business:npiprojet:list")]
        public IActionResult QueryNpiProjet([FromQuery] NpiProjetQueryDto parm)
        {
            var response = _NpiProjetService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询NPI项目管理详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "business:npiprojet:query")]
        public IActionResult GetNpiProjet(int Id)
        {
            var response = _NpiProjetService.GetInfo(Id);

            var info = response.Adapt<NpiProjet>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加NPI项目管理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "business:npiprojet:add")]
        [Log(Title = "NPI项目管理", BusinessType = BusinessType.INSERT)]
        public IActionResult AddNpiProjet([FromBody] NpiProjetDto parm)
        {
            var modal = parm.Adapt<NpiProjet>().ToCreate(HttpContext);
            modal.NpiNo = parm.ProductType.ToString() + DateTime.Now.ToString("yyyyMMdd") + _NpiProjetService.GetMaxID();
            modal.UpdateTime = DateTime.Now;
            modal.CreateTime = DateTime.Now;
            modal.CreateEmpno = HttpContext.GetName();
            modal.Id = _NpiProjetService.GetMaxID();
            var response = _NpiProjetService.AddNpiProjet(modal);



            MailHelper mailHelper = new();
            SendEmailDto sendEmailVo = new SendEmailDto();
            //取出邮件地址
            string emails = _NpiProjetService.Getemails();

            sendEmailVo.ToUser = emails;
            sendEmailVo.Subject = "NPI单号:" + modal.NpiNo + "流程提醒";
            //  sendEmailVo.Content = "新增NPI单号:" + modal.NpiNo + "，请研发上传试确认制准备资料" + "\r\n" + "地址: http://l0.57.7.48:8887/business/NPI";
            string[] toUsers = sendEmailVo.ToUser.Split(",", StringSplitOptions.RemoveEmptyEntries);

            DataTable tabInfo = _NpiProjetService.GetApprovalLogById(Convert.ToInt32(modal.Id));
            string htmlBody = _NpiProjetService.ConvertToHtmlTable(tabInfo);
            sendEmailVo.HtmlContent = htmlBody;
            if (sendEmailVo.SendMe)
            {
                toUsers.Append(mailHelper.FromEmail);
            }
            mailHelper.SendMail(toUsers, sendEmailVo.Subject, sendEmailVo.Content, sendEmailVo.FileUrl, sendEmailVo.HtmlContent);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新NPI项目管理
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "business:npiprojet:edit")]
        [Log(Title = "NPI项目管理", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateNpiProjet([FromBody] NpiProjetDto parm)
        {
            var modal = parm.Adapt<NpiProjet>().ToUpdate(HttpContext);
            modal.UpdateEmpno = HttpContext.GetName();
            var response = _NpiProjetService.UpdateNpiProjet(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除NPI项目管理
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:npiprojet:delete")]
        [Log(Title = "NPI项目管理", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteNpiProjet(string ids)
        {
            //int[] idsArr = Tools.SpitIntArrary(ids);
            //if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            //var response = _NpiProjetService.Delete(idsArr);

            //return ToResponse(response);

            long[] operIdss = Tools.SpitLongArrary(ids);

            return SUCCESS(_NpiProjetService.DeleteDataByIds(operIdss));
        }

        /// <summary>
        /// 导出NPI项目管理
        /// </summary>
        /// <returns></returns>
        [Log(Title = "NPI项目管理", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "business:npiprojet:export")]
        public IActionResult Export([FromQuery] NpiProjetQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _NpiProjetService.GetList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "NPI项目管理", "NPI项目管理");
            return ExportExcel(result.Item2, result.Item1);
        }


        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "NPI项目管理导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "business:npiprojet:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<NpiProjetDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<NpiProjetDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_NpiProjetService.ImportNpiProjet(list.Adapt<List<NpiProjet>>()));
        }

        /// <summary>
        /// NPI项目管理导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "NPI项目管理模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<NpiProjetDto>() { }, "NpiProjet");
            return ExportExcel(result.Item2, result.Item1);
        }


        [HttpGet("GetStepById/{Id}")]
        public IActionResult GetStepById(int id)
        {
            DataTable tabInfo = _NpiProjetService.GetStepById(id);
            List<Step> steps = new List<Step>();

            foreach (DataRow row in tabInfo.Rows)
            {
                Step step = new Step();
                step.seq = row["SEQ"].ToString();
                step.step = row["STEP"].ToString();
                step.time = row["TIME"].ToString();
                step.status = row["STATUS"].ToString();
                if (row["DONE"].ToString() == "true")
                {
                    step.done = true;
                }
                else
                { step.done = false; }
                step.color = row["COLOR"].ToString();
                step.advice = row["ADVICE"].ToString();
                step.emp = row["EMP"].ToString();
                step.url = row["URL"].ToString();
                steps.Add(step);
            }
            //string json = JsonConvert.SerializeObject(steps);

            return SUCCESS(steps);

            //DataTable tabInfo = _NpiProjetService.GetStepById(id);
            //var rowDict = new Dictionary<string, object>();
            //if (tabInfo.Rows.Count > 0)
            //{
            //    for (int i = 0; i < tabInfo.Columns.Count; i++)
            //    {
            //        rowDict[tabInfo.Columns[i].ColumnName] = tabInfo.Rows[0][i];
            //    }

            //}

            //string json=JsonConvert.SerializeObject(tabInfo, Formatting.Indented);

            //return SUCCESS(json);

        }
        [HttpGet("GetApprovalLogById/{Id}")]
        public IActionResult GetApprovalLogById(int id)
        {



            DataTable tabInfo = _NpiProjetService.GetApprovalLogById(id);
            var rowDict = new Dictionary<string, object>();
            if (tabInfo.Rows.Count > 0)
            {
                for (int i = 0; i < tabInfo.Columns.Count; i++)
                {
                    rowDict[tabInfo.Columns[i].ColumnName] = tabInfo.Rows[0][i];
                }

            }

            string jsonStr = JsonConvert.SerializeObject(tabInfo);
            return Content(jsonStr, "application/json");
            //return SUCCESS(json);

        }


        [HttpGet("GetStepInfoById/{Id}/{step}")]
        // [ActionPermissionFilter(Permission = "business:npiprojet:rditem")]
        public IActionResult GetStepInfoById(int id, string step)
        {
            DataTable tabInfo = _NpiProjetService.GetStepInfoById(id, step);
            if (tabInfo.Rows.Count > 0)
            {
                StepInfo stepInfo = new StepInfo();
                List<SubItems> subItems = new List<SubItems>();
                stepInfo.npino = tabInfo.Rows[0]["NPI_NO"].ToString();
                stepInfo.advice = tabInfo.Rows[0]["ADVICE"].ToString();
                stepInfo.status = Convert.ToBoolean(tabInfo.Rows[0]["STATUS"].ToString());
                stepInfo.commit = Convert.ToBoolean(tabInfo.Rows[0]["COMMIT"].ToString());
                stepInfo.update_empno = tabInfo.Rows[0]["UPDATE_EMPNO"].ToString();
                stepInfo.update_time = tabInfo.Rows[0]["UPDATE_time"].ToString();

                foreach (DataRow row in tabInfo.Rows)
                {

                    SubItems items = new SubItems();
                    items.dictsort = row["DICTSORT"].ToString();
                    items.name = row["NAME"].ToString();
                    items.remarks = row["REMARKS"].ToString();
                    items.result = row["RESULT"].ToString();

                    subItems.Add(items);
                }
                stepInfo.subItems = subItems;
                //string json = JsonConvert.SerializeObject(steps);

                return SUCCESS(stepInfo);
            }
            return SUCCESS("");

        }


        [HttpGet("GetOrderInfoById/{Id}/{step}")]
        // [ActionPermissionFilter(Permission = "business:npiprojet:rditem")]
        public IActionResult GetOrderInfoById(int id, string step)
        {
            DataTable tabInfo = _NpiProjetService.GetOrderInfoById(id, step);

            if (tabInfo.Rows.Count > 0)
            {
                OrderInfo orderInfo = new OrderInfo();
                List<Orders> orders = new List<Orders>();
                orderInfo.npino = tabInfo.Rows[0]["NPI_NO"].ToString();

                orderInfo.advice = tabInfo.Rows[0]["ADVICE"].ToString();
                orderInfo.status = Convert.ToBoolean(tabInfo.Rows[0]["STATUS"].ToString());
                orderInfo.commit = Convert.ToBoolean(tabInfo.Rows[0]["COMMIT"].ToString());
                orderInfo.update_empno = tabInfo.Rows[0]["UPDATE_EMPNO"].ToString();
                orderInfo.update_time = tabInfo.Rows[0]["UPDATE_time"].ToString();

                foreach (DataRow row in tabInfo.Rows)
                {

                    Orders order = new Orders();
                    order.seq = row["SEQ"].ToString();
                    order.ipn = row["IPN"].ToString();
                    order.plan_qty = row["PLAN_QTY"].ToString();
                    order.lot = row["LOT"].ToString();
                    order.work_order = row["WORK_ORDER"].ToString();
                    order.po = row["PO"].ToString();
                    order.actual_qty = row["ACTUAL_QTY"].ToString();
                    orders.Add(order);

                }
                orderInfo.orders = orders;
                //string json = JsonConvert.SerializeObject(steps);

                return SUCCESS(orderInfo);
            }
            return SUCCESS("");




        }


        [HttpPost("AddRdItem")]
        // [ActionPermissionFilter(Permission = "business:npiprojet:add")]
        public IActionResult AddRdItem([FromBody] StepInfo parm)
        {
            var modal = parm.Adapt<StepInfo>().ToCreate(HttpContext);

            modal.update_empno = HttpContext.GetName();

            var response = _NpiProjetService.AddRdItem(modal);

            MailHelper mailHelper = new();
            SendEmailDto sendEmailVo = new SendEmailDto();
            string emails = _NpiProjetService.Getemails();

            sendEmailVo.ToUser = emails;
            sendEmailVo.Subject = "NPI单号:" + modal.npino + "流程提醒";
            // sendEmailVo.Content = "NPI单号:" + modal.npino + ",节点:" + modal.step + "在:" + DateTime.Now + "已提交,提交意见：" + modal.advice + "\r\n" + "地址:http://10.57.7.48:8887/business/NPI";

            string[] toUsers = sendEmailVo.ToUser.Split(",", StringSplitOptions.RemoveEmptyEntries);

            DataTable tabInfo = _NpiProjetService.GetApprovalLogById(Convert.ToInt32(modal.id));

            string htmlBody = _NpiProjetService.ConvertToHtmlTable(tabInfo);
            sendEmailVo.HtmlContent = htmlBody;
            if (sendEmailVo.SendMe)
            {
                toUsers.Append(mailHelper.FromEmail);
            }
            mailHelper.SendMail(toUsers, sendEmailVo.Subject, sendEmailVo.Content, sendEmailVo.FileUrl, sendEmailVo.HtmlContent);

            return SUCCESS(response);
        }



        //public string ConvertToHtmlTable(DataTable table)
        //{
        //    StringBuilder html = new StringBuilder();
        //    html.AppendLine("<table border='1'>");
        //    html.AppendLine("<tr>");

        //    // 添加表头
        //    foreach (DataColumn column in table.Columns)
        //    {
        //        string headerText = column.ColumnName; // 默认使用列名作为表头

        //        // 根据列名自定义表头文本
        //        if (column.ColumnName == "STATUS")
        //        {
        //            headerText = "状态";
        //        }
        //        else if (column.ColumnName == "STEP")
        //        {
        //            headerText = "节点";
        //        }
        //        else if (column.ColumnName == "UPDATE_EMPNO")
        //        {
        //            headerText = "提交人员";
        //        }
        //        else if (column.ColumnName == "UPDATE_TIME")
        //        {
        //            headerText = "提交时间";
        //        }
        //        else if (column.ColumnName == "ADVICE")
        //        {
        //            headerText = "意见";
        //        }

        //        // 添加表头到HTML字符串中
        //        html.AppendLine("<th>" + headerText + "</th>");
        //    }

        //    html.AppendLine("</tr>");

        //    // 添加表数据
        //    foreach (DataRow row in table.Rows)
        //    {
        //        html.AppendLine("<tr>");
        //        foreach (var item in row.ItemArray)
        //        {
        //            // 根据条件设置单元格的背景颜色
        //            string cellColor = getStatusColor(row);
        //            html.AppendLine("<td style='" + cellColor + "'>" + item.ToString() + "</td>");
        //        }
        //        html.AppendLine("</tr>");
        //    }

        //    html.AppendLine("</table>");
        //    html.AppendLine("<p>链接: <a href='" + "http://10.57.7.48:8887/business/NPI" + "'>Click Here</a></p>"); // 添加链接
        //    return html.ToString();
        //}

        [HttpPost("AddOrderInfo")]
        // [ActionPermissionFilter(Permission = "business:npiprojet:add")]
        public IActionResult AddOrderInfo([FromBody] OrderInfo parm)
        {
            var modal = parm.Adapt<OrderInfo>().ToCreate(HttpContext);

            modal.update_empno = HttpContext.GetName();

            var response = _NpiProjetService.AddOrderInfo(modal);
            MailHelper mailHelper = new();
            SendEmailDto sendEmailVo = new SendEmailDto();
            string emails = _NpiProjetService.Getemails();

            sendEmailVo.ToUser = emails;
            sendEmailVo.Subject = "NPI单号:" + modal.npino + "流程提醒";
            //sendEmailVo.Content = "NPI单号:" + modal.npino + ",节点:" + modal.step + "在:" + DateTime.Now + "已提交,提交意见：" + modal.advice + "\r\n" + "地址:http://10.57.7.48:8887/business/NPI";
            string[] toUsers = sendEmailVo.ToUser.Split(",", StringSplitOptions.RemoveEmptyEntries);

            DataTable tabInfo = _NpiProjetService.GetApprovalLogById(Convert.ToInt32(modal.id));
            string htmlBody = _NpiProjetService.ConvertToHtmlTable(tabInfo);
            sendEmailVo.HtmlContent = htmlBody;
            if (sendEmailVo.SendMe)
            {
                toUsers.Append(mailHelper.FromEmail);
            }
            mailHelper.SendMail(toUsers, sendEmailVo.Subject, sendEmailVo.Content, sendEmailVo.FileUrl, sendEmailVo.HtmlContent);
            return SUCCESS(response);
        }



        [HttpGet("GetStationType")]
        // [ActionPermissionFilter(Permission = "business:npiprojet:rditem")]
        public IActionResult GetStationType()
        {
            DataTable tabInfo = _NpiProjetService.GetStationType();

            List<Dictionary<string, object>> rowDicts = new List<Dictionary<string, object>>();
            if (tabInfo.Rows.Count > 0)
            {
                foreach (DataRow row in tabInfo.Rows)
                {
                    Dictionary<string, object> rowDict = new Dictionary<string, object>();
                    foreach (DataColumn column in tabInfo.Columns)
                    {
                        rowDict.Add(column.ColumnName.ToLower(), row[column]);
                    }
                    rowDicts.Add(rowDict);
                }

            }

            string jsonStr = JsonConvert.SerializeObject(rowDicts, Newtonsoft.Json.Formatting.Indented);
            return Content(jsonStr, "application/json");




        }


        [HttpGet("GetStationTypeConfig")]
        // [ActionPermissionFilter(Permission = "business:npiprojet:rditem")]
        public IActionResult GetStationTypeConfig()
        {
            DataTable tabInfo = _NpiProjetService.GetStationTypeConfig();

            List<int> columnValues = new List<int>();
            if (tabInfo.Rows.Count > 0)
            {
                foreach (DataRow row in tabInfo.Rows)
                {
                    if (row["KEY"] != DBNull.Value)
                    {
                        columnValues.Add(Convert.ToInt32(row["KEY"].ToString()));
                    }
                }

            }

            //  string jsonStr = JsonConvert.SerializeObject(rowDicts, Newtonsoft.Json.Formatting.Indented);
            return Ok(columnValues);




        }

        [HttpPost("AddWipStation")]
        public IActionResult AddWipStation([FromBody] List<string> stationid)
        {
            if (stationid.Count>0)
            {
                for (int i=0;i<stationid.Count;i++)
                {
                    
                }
            }
            return SUCCESS("");
        }

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="uploadDto">自定义文件名</param>
        /// <param name="storeType">上传类型1、保存到本地 2、保存到阿里云</param>
        /// <returns></returns>
        [HttpPost("UploadFile")]
        [Verify]
        //[ActionPermissionFilter(Permission = "common")]
        public async Task<IActionResult> UploadFile([FromForm] UploadNpiDto uploadDto, StoreType storeType = StoreType.LOCAL)
        {
            IFormFile formFile = uploadDto.File;
            if (formFile == null) throw new CustomException(ResultCode.PARAM_ERROR, "上传文件不能为空");
            NpiProjetFile file = new();
            string fileExt = Path.GetExtension(formFile.FileName);//文件后缀
            double fileSize = Math.Round(formFile.Length / 1024.0, 2);//文件大小KB
            string npino = uploadDto.NpiNo;

            if (OptionsSetting.Upload.NotAllowedExt.Contains(fileExt))
            {
                return ToResponse(ResultCode.CUSTOM_ERROR, "上传失败，未经允许上传类型");
            }
            if (uploadDto.FileNameType == 1)
            {
                uploadDto.FileName = Path.GetFileNameWithoutExtension(formFile.FileName);
            }
            else if (uploadDto.FileNameType == 3)
            {
                uploadDto.FileName = SysFileService.HashFileName();
            }
            switch (storeType)
            {
                case StoreType.LOCAL:
                    string savePath = Path.Combine(WebHostEnvironment.WebRootPath);
                    if (uploadDto.FileDir.IsEmpty())
                    {
                        uploadDto.FileDir = OptionsSetting.Upload.LocalSavePath;
                    }
                    file = await _NpiProjetService.SaveNpiFileToLocal(savePath, uploadDto.ID, uploadDto.Step, uploadDto.FileName, uploadDto.FileDir, HttpContext.GetName(), formFile);
                    break;
                case StoreType.REMOTE:
                    break;
                case StoreType.ALIYUN:
                    break;
                case StoreType.TENCENT:
                    break;
                case StoreType.QINIU:
                    break;
                default:
                    break;
            }

            return SUCCESS(new
            {
                url = file.AccessUrl,
                fileName = file.FileName,
                fileId = uploadDto.ID
            });
        }
    }
}