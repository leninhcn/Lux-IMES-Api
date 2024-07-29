using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Model.Dto.Tooling;
using ZR.Service.ToolingManagement.IService;

namespace ZR.Admin.WebApi.Controllers.ToolingManagement
{
    /// <summary>
    /// 治具保养
    /// </summary>
    [Route("toolingmanagement/toolingmaintain/[action]")]
    public class ToolingMaintainController : BaseController
    {
        IToolingMtTravelService toolingMtTravelService;
        IToolingSnService toolingSnService;
        IToolingPickUpService toolingPickUpService;
        public ToolingMaintainController(IToolingMtTravelService toolingMtTravelService, IToolingSnService toolingSnService, IToolingPickUpService toolingPickUpService ) 
        { 
            this.toolingMtTravelService = toolingMtTravelService;
            this.toolingSnService = toolingSnService;
            this.toolingPickUpService = toolingPickUpService;
        }

        /// <summary>
        /// 检查工号 
        /// </summary>
        /// <param name="empno"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SelectEmpByNo(string empno)
        {
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            var resp = toolingMtTravelService.SelectEmpByNo(empno, site);
            if (resp == null)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"用户不存在"));
            }
            return SUCCESS(resp);
        }

        /// <summary>
        /// 检查治具，获取治具信息
        /// </summary>
        /// <param name="toolingsn"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckToolingSn([FromQuery]string toolingsn)
        {
            string site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            MToolingSn mToolingSn = toolingSnService.GetInfoByToolingSn( site, toolingsn);
            if (mToolingSn == null)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具不存在"));
            }

            if (mToolingSn.ToolingStatus.ToString() != "P" &&
                mToolingSn.ToolingStatus.ToString() != "I" &&
                mToolingSn.ToolingStatus.ToString() != "D" &&
                mToolingSn.ToolingStatus.ToString() != "M" )
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"该治具不是领用、下线、在库状态，不需要保养"));
            }

            //MToolingToolingSnVo toolingVo = toolingPickUpService.selectToolingByToolingSn(toolingsn, site);
            //if (toolingVo.MaintainTime <= 0)
            //{
            //    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具使用需要维护保养周期"));
            //}

            ExecuteResult exeRes = toolingMtTravelService.GetDateATime(toolingsn);
            if(exeRes.Status)
            {
                DataTable dt_time = (DataTable)exeRes.Anything;
                if (dt_time.Rows.Count > 0)
                {
                    string Ptime = dt_time.Rows[0][0].ToString();
                    if (dt_time.Rows[0][2].ToString() == "")
                    {
                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具使用需要维护保养周期"));
                    }
                    else
                    {
                        int sDay = Convert.ToInt32(dt_time.Rows[0][2].ToString());
                        exeRes = toolingMtTravelService.GetDay(Ptime);
                        if (exeRes.Status)
                        {
                            DataTable dt_hours = (DataTable)exeRes.Anything;
                            int hours = Convert.ToInt32(dt_hours.Rows[0][0].ToString());
                            if (hours >= sDay * 24)
                            {
                                exeRes = toolingMtTravelService.CgToolingDEC(toolingsn);
                            }
                            exeRes = toolingMtTravelService.GetUsedCount(toolingsn);
                            if (exeRes.Status)
                            {
                                DataTable dt_Ucount = (DataTable)exeRes.Anything;
                                string count = dt_Ucount.Rows[0][0].ToString();
                                exeRes = toolingMtTravelService.GetMAXCount(toolingsn);
                                if (exeRes.Status)
                                {
                                    DataTable DT_Mcount = (DataTable)exeRes.Anything;
                                    string mCount = DT_Mcount.Rows[0][1].ToString();
                                    int UC = Convert.ToInt32(count);
                                    int MC = Convert.ToInt32(mCount);
                                    if (UC >= MC)
                                    {
                                        return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"治具使用次数超过最大使用次数"));
                                    }
                                }
                                else
                                {
                                    return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"Max Used Count Not Setting"));
                                }
                            }
                            else
                            {
                                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"No data"));
                            }
                        }
                        else
                        {
                            return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, $"No data"));
                        }
                        
                    }
                }

                return SUCCESS(mToolingSn);
            }
            else
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR,exeRes.Message.ToString()));
            }
        }

        /// <summary>
        /// 治具保养执行
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ToolingMaintain([FromBody] ToolingMtTravelDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.ToolingSn))
            {
                return ToResponse(ApiResult.Error(101, "请求参数错误"));
            }
            if (string.IsNullOrEmpty(dto.Site))
            {
                dto.Site = HttpContext.GetSite() == "" ? "DEF" : HttpContext.GetSite();
            }

            if (string.IsNullOrEmpty(dto.UpdateEmpNo))
            {
                dto.Site = HttpContext.GetName() ;
            }

            MToolingSn mToolingSn = toolingSnService.GetInfoByToolingSn(dto.Site, dto.ToolingSn);
            if (mToolingSn == null)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR,$"治具不存在,请刷新"));
            }

            MToolingSn mToolingSn2 = new()
            {
                ToolingSnId = mToolingSn.ToolingSnId,
                LastMaintainTime = DateTime.Now,
                UsedCount = 0,
                ToolingStatus = "I",
                UpdateEmpNo1 = dto.UpdateEmpNo,
                UpdateTime1 = DateTime.Now
            };
            toolingMtTravelService.UpdateInfo(mToolingSn2);

            ExecuteResult exeRes = toolingMtTravelService.ChangeLStatus(mToolingSn.ToolingSn, mToolingSn.ToolingStatus, dto.UpdateEmpNo, mToolingSn.Site);

            if (!exeRes.Status)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
            }

            exeRes = toolingMtTravelService.INSERTDATA(dto);
            if (!exeRes.Status)
            {
                return ToResponse(ApiResult.Error((int)ResultCode.CUSTOM_ERROR, exeRes.Message.ToString()));
            }
            return SUCCESS(await toolingMtTravelService.GetMaintainResult(dto.ToolingSn));
        }

    }
}
