using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;
using ZR.Admin.WebApi.Filters;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Service.System.IService;
using ZR.Model.Business;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ZR.Model.Dto;
using ZR.Service.IService;
using MiniExcelLibs;
using ZR.Service.WoManagement;

namespace ZR.Admin.WebApi.Controllers.System
{
    /// <summary>
    /// MntnLabelManagement 标签信息维护
    /// </summary>
    [Verify]
    [Route("labelmnt/labelmanagement")]
    //[ApiExplorerSettings(GroupName = "DataCenter")]
    public class LabelManagementController : BaseController
    {
        public ILabelManagementService _LabelManagement;
        public LabelManagementController(ILabelManagementService defectService)
        {
            _LabelManagement = defectService;
        }
        /// <summary>
        /// 获取label_type信息
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:querylistlabeltype")]
        [HttpGet("querylistlabeltype")]
        public IActionResult QueryLabelTypelist([FromQuery] MLabelTypeQueryDto param)
        {
            var site = HttpContext.GetSite();
            param.site = site;
            return SUCCESS(_LabelManagement.GetListlabeltype(param), TIME_FORMAT_FULL);
            //return SUCCESS("", TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取label_type明细
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:querylabeltypeinfo")]
        [HttpGet("queryinfolabeltype")]
        public IActionResult QueryLabelTypeInfo(string param)
        {
            return SUCCESS(_LabelManagement.GetInfoLabelType(param), TIME_FORMAT_FULL);
            //return SUCCESS("", TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 新增label_type信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("addlabeltype")]
        [Log(Title = "新增labeltype", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:addlabeltype")]
        public IActionResult AddLabelType([FromBody] MLabelType param)
        {
            param.CreateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = _LabelManagement.AddLabelType(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 修改label_type
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updatelabeltype")]
        [Log(Title = "修改label_type", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:updatelabeltype")]
        public IActionResult UpdateLabelType([FromBody] MLabelType param)
        {
            param.UpdateEmpno = HttpContext.GetName();
            string result = _LabelManagement.UpdateLabelType(param);
            // return ToResponse(new ApiResult(200, result == "OK" ? "sucess" : "faile", result));
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        // <summary>
        // 删除labe_ltype
        // </summary>
        /// <param name="param"></param>
        // <returns></returns>
        [HttpPost("removelabeltype")]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:removelabeltype")]
        [Log(Title = "不良维护", BusinessType = BusinessType.DELETE)]
        public IActionResult RemoveLabelType([FromBody] MLabelType param)
        {
            param.UpdateEmpno = HttpContext.GetName();
            param.Enabled = "N";
            param.UpdateTime = DateTime.Now;
            string result = _LabelManagement.DeleteLabelType(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 新增标签模版label_file信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("addlabelfile")]
        [Log(Title = "新增labelfile", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:addlabelfile")]
        public IActionResult AddLabelFile([FromBody] MLabelTemplate param)
        {
            if (string.IsNullOrEmpty(param.TemplateFile) || string.IsNullOrEmpty(param.LabelName))
            {
                return ToResponse(new ApiResult( (int)ResultCode.CUSTOM_ERROR,"Invalid input", "Invalid input"));
            }
            try
            {
                // 将 Base64 字符串转换为字节数组
                byte[] fileBytes = Convert.FromBase64String(param.TemplateFile);
                MLabelTemplateFile mlabel=new MLabelTemplateFile();
                mlabel.CreateEmp = HttpContext.GetName();
                mlabel.TemplateFile = fileBytes;
                mlabel.LabelName= param.LabelName;
                string result = _LabelManagement.AddLabelFile(mlabel);
                return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
            }
            catch(Exception ex)
            {
                return ToResponse(new ApiResult((int)ResultCode.CUSTOM_ERROR, ex.ToString(), ex.ToString()));
            }
           
        }
        /// <summary>
        /// 获取模版label_file
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:querylabelfile")]
        [HttpGet("querylabelfile")]
        public IActionResult QueryLabelfile(string param)
        {
            var result = _LabelManagement.GetListLabelFile(param);
            if(result.IsNullOrZero())
            {
               return ToResponse(new ApiResult( (int)ResultCode.CUSTOM_ERROR, "无模板文件" , "无模板文件"));
            }
            else
            { 
            return Bytes2File(result.TemplateFile, result.LabelName);
            }
            //return SUCCESS(_LabelManagement.GetListLabelFile(param), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取模版label_params
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:querylabelparams")]
        [HttpGet("querylabelparams")]
        public IActionResult QueryLabelparams([FromQuery]MStationtypeLabelParamsQueryDto param)
        {
            param.site = HttpContext.GetSite();
            return SUCCESS(_LabelManagement.GetListStationtypeLabelParam(param), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 获取模版label_station
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:querylistlabelstation")]
        [HttpGet("querylistlabelstation")]
        public IActionResult QueryLabelstation([FromQuery] MStationtypeLabelQueryDto param)
        {
            param.site = HttpContext.GetSite();
            return SUCCESS(_LabelManagement.GetListStationtypeLabel(param), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 查询print_data信息
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:querylistprintdata")]
        [HttpGet("querylistprintdata")]
        public IActionResult QueryLabelSql([FromQuery] MPrintDataQueryDto param)
        {
            var site = HttpContext.GetSite();
            param.site = site;
            return SUCCESS(_LabelManagement.GetListprintdata(param), TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 查询label_station明细
        /// </summary>
        /// <returns></returns>
        [HttpGet("queryinfolabelstation")]
        public IActionResult QueryInfoLabelStation(string param)
        {
            return SUCCESS(_LabelManagement.GetInfostationtypelabel(param), TIME_FORMAT_FULL);
            //return SUCCESS("", TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 查询label_params明细
        /// </summary>
        /// <returns></returns>
        [HttpGet("queryinfolabelparams")]
        public IActionResult QueryInfoLabelParams(string param)
        {
            return SUCCESS(_LabelManagement.GetInfostationtypelabelParam(param), TIME_FORMAT_FULL);
            //return SUCCESS("", TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 查询print_data明细
        /// </summary>
        /// <returns></returns>
        [HttpGet("queryinfoprintdata")]
        public IActionResult QueryPrintDataInfo(string param)
        {
            return SUCCESS(_LabelManagement.GetInfoprintdata(param), TIME_FORMAT_FULL);
            //return SUCCESS("", TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 新增label_station
        /// </summary>
        /// <returns></returns>
        [HttpPost("addlabelstation")]
        [Log(Title = "新增labelstation", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:addlabelstation")]
        public IActionResult AddLabelStation([FromBody] MStationtypeLabel param)
        {
            param.CreateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = _LabelManagement.AddStationLabel(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 新增label_params
        /// </summary>
        /// <returns></returns>
        [HttpPost("addlabelparams")]
        [Log(Title = "新增labelparams", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:addlabelparams")]
        public IActionResult AddLabelParams([FromBody] MStationtypeLabelParams param)
        {
            param.CreateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = _LabelManagement.AddStationLabelParams(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 新增print_data
        /// </summary>
        /// <returns></returns>
        [HttpPost("addprintdata")]
        [Log(Title = "新增printdata", BusinessType = BusinessType.INSERT)]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:addprintdata")]
        public IActionResult AddPrintData([FromBody] MPrintData param)
        {
            param.CreateEmpno = HttpContext.GetName();
            param.Site = HttpContext.GetSite();
            string result = _LabelManagement.AddPrintData(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 修改label_station
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updatelabelstation")]
        [Log(Title = "更新labelstation", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:updatelabelstation")]
        public IActionResult UpdatelabelStation([FromBody] MStationtypeLabel param)
        {
            param.UpdateEmpno = HttpContext.GetName();
            string result = _LabelManagement.UpdateStationtypeLabel(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 修改label_params
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updatelabelparams")]
        [Log(Title = "更新labelparams", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:updatelabelparams")]
        public IActionResult Updatelabelparams([FromBody] MStationtypeLabelParams param)
        {
            param.UpdateEmpno = HttpContext.GetName();
            string result = _LabelManagement.UpdateStationtypeLabelParam(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// 修改print_data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("updateprintdata")]
        [Log(Title = "更新printdata", BusinessType = BusinessType.UPDATE)]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:updateprintdata")]
        public IActionResult UpdatePrintData([FromBody] MPrintData param)
        {
            param.UpdateEmpno = HttpContext.GetName();
            string result = _LabelManagement.UpdatePrintData(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }

        // <summary>
        // 删除label_station
        // </summary>
        // <returns></returns>
        [HttpPost("deletelabelstation")]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:removelabelstation")]
        [Log(Title = "删除labelstation", BusinessType = BusinessType.DELETE)]
        public IActionResult RemoveLabelStation([FromBody] MStationtypeLabel param)
        {
            param.UpdateEmpno = HttpContext.GetName();
            param.UpdateTime = DateTime.Now;
            string result = _LabelManagement.DeleteStationtypeLabel(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }

        // <summary>
        // 删除label_params
        // </summary>
        // <returns></returns>
        [HttpPost("deletelabelparams")]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:removelabelparams")]
        [Log(Title = "删除labelparams", BusinessType = BusinessType.DELETE)]
        public IActionResult RemoveLabelParams([FromBody] MStationtypeLabelParams param)
        {
            param.UpdateEmpno = HttpContext.GetName();
            param.UpdateTime = DateTime.Now;
            string result = _LabelManagement.DeleteStationtypeLabelParam(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }

        // <summary>
        // 删除print_data
        // </summary>
        // <returns></returns>
        [HttpPost("deleteprintdata")]
        //[ActionPermissionFilter(Permission = "labelmnt:labelmanagement:removeprintdata")]
        [Log(Title = "删除printdata", BusinessType = BusinessType.DELETE)]
        public IActionResult RemovePrintData([FromBody]MPrintData param)
        {
            param.UpdateEmpno = HttpContext.GetName();
            param.UpdateTime = DateTime.Now;
            string result = _LabelManagement.DeletePrintData(param);
            return ToResponse(new ApiResult(result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, result == "OK" ? "sucess" : result, result));
        }
        /// <summary>
        /// stationtypelabel导入
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "用户导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        //[ActionPermissionFilter(Permission = "system:user:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            try
            {
                var site = HttpContext.GetSite();
                List<MStationtypeLabel> labels = new();
                using (var stream = formFile.OpenReadStream())
                {
                    labels = stream.Query<MStationtypeLabel>(startCell: "A1").ToList();
                }
                return SUCCESS(_LabelManagement.ImportStationtypeLabel(labels, site));
            }
            catch (Exception ex)
            {
                (string, object, object) res = (ex.ToString().Substring(0, 20), "", "");
                return SUCCESS(res);

            }
        }
    }
}
