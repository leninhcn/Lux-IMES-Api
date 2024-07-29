using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Model.Dto.Tooling;
using ZR.Service.ToolingManagement.IService;
using ZR.ServiceCore.Model.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZR.Admin.WebApi.Controllers.ToolingManagement
{
    /// <summary>
    /// 治具维修
    /// </summary>
    [Route("toolingmanagement/toolingrepair/[action]")]
    public class ToolingRepairController : BaseController
    {

        public IToolingRepairService _toolingRepairService;
        public ToolingRepairController(IToolingRepairService _toolingRepairService)
        {
            this._toolingRepairService = _toolingRepairService;
        }
        /// <summary>
        /// 检查治具SN
        /// </summary>
        /// <param name="toolingSn"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckToolingSn(string toolingSn)
        {
            
            var site = HttpContext.GetSite();
            site = site == ""? "DEF" : site;
            var exeRes =  _toolingRepairService.GetToolingSnInfo(toolingSn,site);
            if (exeRes.Status)
            {
                DataTable DT_SNDATA = (DataTable)exeRes.Anything;
                if (DT_SNDATA.Rows.Count == 0)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Tooling Error!or  not in D, NG"));
                }
                if (DT_SNDATA.Rows[0]["ENABLED"].ToString() != "Y")
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Tooling Disabled!"));
                }

                ToolingSnShowDataVo toolingSnShowDataVo = new ToolingSnShowDataVo();
                toolingSnShowDataVo.ToolingType = DT_SNDATA.Rows[0]["tooling_type"].ToString();
                toolingSnShowDataVo.ToolingLastMaintainTime = DT_SNDATA.Rows[0]["dt"].ToString();

                exeRes = _toolingRepairService.GetToolingDefectInfo(toolingSn,site);
                if (exeRes.Status)
                {
                    DataTable dt_dedata = (DataTable)exeRes.Anything;
                    if (dt_dedata.Rows.Count> 0)
                    {
                        
                        string S = "";
                        List<ToolingDefectVo> toolingDefectVos = new List<ToolingDefectVo>();
                        for (int i = 0; i <= dt_dedata.Rows.Count - 1; i++)
                        {
                            ToolingDefectVo toolingDefectVo = new ToolingDefectVo();
                            string sDefectCode = dt_dedata.Rows[i]["DEFECT_CODE"].ToString();
                            if (S != sDefectCode)
                            {
                                toolingDefectVo.DefectCode = sDefectCode;
                                toolingDefectVo.DefectDesc2 = dt_dedata.Rows[i]["DEFECT_DESC2"].ToString();
                                toolingDefectVo.RecId = dt_dedata.Rows[i]["REC_ID"].ToString();
                                toolingDefectVo.RpStatus = dt_dedata.Rows[i]["RP_STATUS"].ToString();
                                S = dt_dedata.Rows[i]["DEFECT_CODE"].ToString();

                                toolingDefectVos.Add(toolingDefectVo);
                            }
                        }
                        toolingSnShowDataVo.toolingDefectVos = toolingDefectVos;
                    }

                    return SUCCESS(toolingSnShowDataVo);
                }
                else
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Select Error Or Tooling No Data!"));
                }
            }
            else
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Select Error Or Tooling No Data!"));
                
            }
        }
        /// <summary>
        /// 检查不良代码
        /// </summary>
        /// <param name="toolingSn"></param>
        /// <param name="DefectCode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckDefectCode(string toolingSn, string DefectCode)
        {
            var site = HttpContext.GetSite();
            site = site == "" ? "DEF" : site;
            var empNo = HttpContext.GetName();
            empNo = empNo == "" ? "1" : empNo;

            var exeRes = _toolingRepairService.GetToolingSnInfo(toolingSn, site);
            if (exeRes.Status)
            {
                DataTable DT_SNDATA = (DataTable)exeRes.Anything;
                if (DT_SNDATA.Rows.Count == 0)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Tooling Error!or  not in D, NG"));
                }
                if (DT_SNDATA.Rows[0]["ENABLED"].ToString() != "Y")
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Tooling Disabled!"));
                }

                exeRes = _toolingRepairService.GetDefect(DefectCode, site);
                if (exeRes.Status)
                {
                    DataTable DT_DEFECT = (DataTable)exeRes.Anything;
                    if (DT_DEFECT.Rows.Count == 0)
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Defect Code Error!"));
                    }

                    exeRes = _toolingRepairService.InsertToolingSnDefect(toolingSn, DefectCode, empNo, site);
                    if (exeRes.Status)
                    {
                        ToolingDefectVo toolingDefectVo = new ToolingDefectVo();
                        toolingDefectVo.DefectCode = DefectCode;
                        toolingDefectVo.DefectDesc2 = DT_DEFECT.Rows[0]["DEFECT_DESC2"].ToString();
                        return SUCCESS(toolingDefectVo);
                    }
                    else
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
                    }
                }
                else
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Defect Code Error!"));
                }
            }
            else
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
            }
        }
        /// <summary>
        /// 检查原因
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckReason(string reason)
        {
            var site = HttpContext.GetSite();
            site = site == "" ? "DEF" : site;
            try
            {
                ExecuteResult exeRes = new ExecuteResult();
                exeRes = _toolingRepairService.GetReson(reason, site);
                if (exeRes.Status)
                {
                    DataTable dt_reason = (DataTable)exeRes.Anything;
                    if (dt_reason.Rows.Count == 0)
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Reason Code Error!"));
                    }
                    return SUCCESS(dt_reason.Rows[0][1].ToString());
                }
                else
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
                }
            }
            catch (Exception ex)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, ex.ToString()));
            }
        }
        /// <summary>
        /// 治具SN维修
        /// </summary>
        /// <param name="toolingSn"></param>
        /// <param name="defectCode"></param>
        /// <param name="reason"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ToolingSnRepair(string toolingSn,string defectCode, string reason, string remark)
        {
            var site = HttpContext.GetSite();
            site = site == "" ? "DEF" : site;
            var empNo = HttpContext.GetName();
            empNo = empNo == "" ? "1" : empNo;

            try
            {
                ExecuteResult exeRes = new ExecuteResult();
                exeRes = _toolingRepairService.GetReson(reason, site);
                if (exeRes.Status)
                {
                    DataTable dt_reason = (DataTable)exeRes.Anything;
                    if (dt_reason.Rows.Count == 0)
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Reason Code Error!"));
                    }

                    exeRes = _toolingRepairService.GetToolingSnRepairData(toolingSn, reason,site);
                    if (exeRes.Status)
                    {
                        DataTable dt_dis = (DataTable)exeRes.Anything;
                        if (dt_dis.Rows.Count > 0)
                        {
                            return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Reason Code Duplicate!"));
                        }
                        exeRes = _toolingRepairService.ToolingSnRepair(toolingSn, reason, site, remark, empNo);
                        if (exeRes.Status)
                        {
                            return SUCCESS("OK");
                        }
                        else
                        {
                            return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
                        }

                    }
                    else
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, "Reason Code Duplicate!"));
                    }
                }
                else
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, "Reason Code Error!"));
                }
            }
            catch (Exception ex)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, ex.ToString()));
            }
        }
        /// <summary>
        ///删除不良
        /// </summary>
        /// <param name="toolingSn"></param>
        /// <param name="defectCode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DeleteToolingSnDefectCode(string toolingSn,string defectCode)
        {

            ExecuteResult exeRes = new ExecuteResult();
            exeRes = _toolingRepairService.DeleteToolingSnDefectCode(toolingSn, defectCode);
            if (exeRes.Status)
            {
                return SUCCESS("OK");
            }
            else
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
            }
        }
        /// <summary>
        /// 治具SN维修完成
        /// </summary>
        /// <param name="toolingSn"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ToolingSnComplete(string toolingSn)
        {
            var site = HttpContext.GetSite();
            site = site == "" ? "DEF" : site;
            var empNo = HttpContext.GetName();
            empNo = empNo == "" ? "1" : empNo;
            ExecuteResult exeRes = new ExecuteResult();
            exeRes = _toolingRepairService.ToolingSnComplete(toolingSn, site, empNo);
            if (exeRes.Status)
            {
                return SUCCESS("OK");
            }
            else
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, "Repair not Complete!"));
            }
        }
        /// <summary>
        /// 治具SN报废
        /// </summary>
        /// <param name="toolingSn"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ToolingSnScrap(string toolingSn)
        {
            var site = HttpContext.GetSite();
            site = site == "" ? "DEF" : site;
            var empNo = HttpContext.GetName();
            empNo = empNo == "" ? "1" : empNo;
            ExecuteResult exeRes = new ExecuteResult();
            exeRes = _toolingRepairService.CheckToolingSnScrap(toolingSn, site);
            if (exeRes.Status)
            {
                DataTable DT_S = (DataTable)exeRes.Anything;
                if (DT_S.Rows.Count > 0)
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, "Tooling is Scraped!"));
                }
                exeRes = _toolingRepairService.ToolingSnScrap(toolingSn, empNo, site);
                if (exeRes.Status)
                {
                    return SUCCESS("OK");
                }
                else
                {
                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
                }
            }
            else
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, "Tooling is Error!"));
            }

            
        }
    }
}
