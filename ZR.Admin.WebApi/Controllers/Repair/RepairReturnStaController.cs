using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Xml.Linq;
using ZR.Infrastructure.Model;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Admin.WebApi.Controllers.Repair
{

    /// <summary>
    /// 维修回流（材料）
    /// </summary>
    [Route("repair/returnstation")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class RepairReturnStaController : BaseController
    {
        public IRepairReturnStaService repairService;
        public RepairReturnStaController(IRepairReturnStaService _repairService)
        {
            repairService = _repairService;
        }

        /// <summary>
        /// 获取Main数据
        /// </summary>
        /// <param name="retData"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:getShowData")]
        [HttpPost("getShowData")]
        public async Task<IActionResult> getShowData([FromBody] returnStaData retData)
        {
            ExecuteResult exeRes = await repairService.ShowData(retData);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// 获取MainDetail数据
        /// </summary>
        /// <param name="retData"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:getShowDetailData")]
        [HttpPost("getShowDetailData")]
        public async Task<IActionResult> getShowDetailData([FromBody] stationDetailData retData)
        {
            ExecuteResult exeRes = await repairService.ShowDetailData(retData);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// 获取Model
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:getModel")]
        [HttpGet("getModel")]
        public async Task<IActionResult> getModel()
        {
            DataTable dt = await repairService.getModel();
           
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", dt));
        }

        /// <summary>
        /// 获取Category
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:getCategory")]
        [HttpGet("getCategory")]
        public async Task<IActionResult> getCategory()
        {
            List<string> liCategory = new List<string>();

            liCategory.Add("GB");
            liCategory.Add("nCG");

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", liCategory));
        }

        /// <summary>
        /// 获取AREA
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:getAREA")]
        [HttpGet("getAREA")]
        public async Task<IActionResult> getAREA()
        {
            List<string> liArea = new List<string>();

            liArea.Add("TD");
            liArea.Add("FA");
            liArea.Add("RE");

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", liArea));
        }

        /// <summary>
        /// Main OK
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:repairMainPart")]
        [HttpPost("repairMainPart")]
        public async Task<IActionResult> repairMainPart([FromBody] ModelInfo modelInfo)
        {
            DataTable dt = await repairService.getDt(modelInfo);

            if (dt.Rows.Count > 0)
            {
                string sData = modelInfo.MODEL + " : " + modelInfo.CATEGORY;
                string sMsg = "数据重复" + Environment.NewLine + sData;

                return ToResponse(ResultCode.PARAM_ERROR, sMsg);
            }

            ExecuteResult exeRes = new ExecuteResult();
            string userNo = HttpContext.GetName();
            try
            {
                if (modelInfo.Type == "APPEND")
                {
                    exeRes = await repairService.AppendData(modelInfo, userNo);
                    if (exeRes.Status)
                    {
                        return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "增加成功",""));
                    }
                    else
                    {
                        return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
                    }
                }
                else if (modelInfo.Type == "MODIFY")
                {
                    exeRes = await repairService.ModifyData(modelInfo, userNo);
                    if (exeRes.Status)
                    {
                        return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "增加成功", ""));
                    }
                    else
                    {
                        return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return ToResponse(ResultCode.PARAM_ERROR, ex.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", ""));
        }

        /// <summary>
        /// DETAIL OK
        /// </summary>
        /// <param name="stations"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:repairDetailPart")]
        [HttpPost("repairDetailPart")]
        public async Task<IActionResult> repairDetailPart([FromBody] detailPartStation stations)
        {
            DataTable dtTemp = await repairService.getStage(stations);

            if (stations.sUpdateType == "APPEND")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    string sData =" Return StationType: " + stations.returnStationType;
                    string sMsg = "数据重复" + Environment.NewLine + sData;

                    return ToResponse(ResultCode.PARAM_ERROR, sMsg);
                }
            }

            try
            {
                ExecuteResult exeRes = new ExecuteResult();
                string userNo = HttpContext.GetName();
                if (stations.sUpdateType == "APPEND")
                {
                    exeRes = await repairService.AppendDetailData(stations, userNo);

                    if (exeRes.Status)
                    {
                        return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "增加成功", ""));
                    }
                    else
                    {
                        return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
                    }
                }
                else if (stations.sUpdateType == "MODIFY")
                {
                    exeRes= await repairService.ModifyDetailData(stations, userNo);
                   
                    if (!exeRes.Status)
                    {
                        return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
                    }
                    return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "修改成功", ""));
                }
                return ToResponse(ResultCode.PARAM_ERROR, "操作类型错误！！！");
            }
            catch (Exception ex)
            {
                return ToResponse(ResultCode.PARAM_ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Disable
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:Disable")]
        [HttpPost("Disable")]
        public async Task<IActionResult> Disable([FromBody] ModelInfo modelInfo)
        {
            ExecuteResult exeRes = await repairService.Disable(modelInfo,HttpContext.GetName());
            if (exeRes.Status)
            {
                return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "success", ""));
            }
            else
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
        }

        /// <summary>
        /// DetailDisabled
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:DetailDisabled")]
        [HttpPost("DetailDisabled")]
        public async Task<IActionResult> DetailDisabled([FromBody] ModelInfo modelInfo)
        {
            ExecuteResult exeRes = await repairService.DetailDisabled(modelInfo, HttpContext.GetName());
            if (exeRes.Status)
            {
                return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "success", ""));
            }
            else
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="delData"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:Delete")]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteData delData)
        {
            DataTable dtTemp = await repairService.getStage(delData.sName);
            if (dtTemp.Rows.Count > 0)
            {
                string sErrMsg1 = "has one or more DETAILS";
                string sErrMsg2 = "Can not delete it";

                return ToResponse(ResultCode.PARAM_ERROR, delData.sData + " " + sErrMsg1 + Environment.NewLine
                                        + sErrMsg2 + "!");
            }

            string userNo = HttpContext.GetName();

            ExecuteResult exeRes = await repairService.Delete(delData.sId, userNo);

            if (exeRes.Status)
            {
                return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "success", ""));
            }
            else
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
        }

        /// <summary>
        /// DetailDelete
        /// </summary>
        /// <param name="delData"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:DetailDelete")]
        [HttpPost("DetailDelete")]
        public async Task<IActionResult> DetailDelete([FromBody] DeleteData delData)
        {
            string userNo = HttpContext.GetName();

            ExecuteResult exeRes = await repairService.DetailDelete(delData.sId, userNo);

            if (exeRes.Status)
            {
                return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "success", ""));
            }
            else
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
        }

        /// <summary>
        /// 获取历史修改记录
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:HistoryData")]
        [HttpGet("HistoryData")]
        public async Task<IActionResult> HistoryData(string sName)
        {
            DataTable dtTemp = await repairService.HistoryData(sName);
           
           return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "success", dtTemp));
        }

        /// <summary>
        /// 获取历史修改detail记录
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:DetailHistoryData")]
        [HttpGet("DetailHistoryData")]
        public async Task<IActionResult> DetailHistoryData(string sName)
        {
            DataTable dtTemp = await repairService.HistoryData(sName);

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "success", dtTemp));
        }

        /// <summary>
        /// 保存导入Excel
        /// </summary>
        /// <param name="stations"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:returnstation:SaveExcel")]
        [HttpGet("SaveExcel")]
        public async Task<IActionResult> SaveExcel([FromBody] detailPartStation stations)
        {
            ExecuteResult exeRes = await repairService.SaveExcel(stations,HttpContext.GetName());
            if (exeRes.Status)
            {
                return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "Excel导入成功", ""));
            }
            else
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
        }

    }
}
