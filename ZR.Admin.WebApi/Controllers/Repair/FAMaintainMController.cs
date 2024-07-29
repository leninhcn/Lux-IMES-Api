using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZR.Infrastructure.Model;
using ZR.Service.Repair.IRepairService;

namespace ZR.Admin.WebApi.Controllers.Repair
{
    /// <summary>
    /// FA
    /// </summary>
    [Route("repair/famaintainm")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class FAMaintainMController : BaseController
    {
        public IFAMaintainMService repairService;
        public FAMaintainMController(IFAMaintainMService _repairService)
        {
            repairService = _repairService;
        }

        /// <summary>
        /// getLine
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:famaintainm:getLine")]
        [HttpGet("getLine")]
        public async Task<IActionResult> getLine()
        {
            ExecuteResult exeRes = await repairService.getLine();
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
          
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// 不良类型
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:famaintainm:getDefectType")]
        [HttpGet("getDefectType")]
        public async Task<IActionResult> getDefectType()
        {
            ExecuteResult exeRes = await repairService.getDefectType();
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// 查询FA
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:famaintainm:getShowData")]
        [HttpGet("getShowData")]
        public async Task<IActionResult> getShowData()
        {
            ExecuteResult exeRes = await repairService.getShowData();
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// 机种数据
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:famaintainm:getModel")]
        [HttpGet("getModel")]
        public async Task<IActionResult> getModel()
        {
            ExecuteResult exeRes = await repairService.getModel();
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// 获取员工信息by 工号
        /// </summary>
        /// <param name="empno"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:famaintainm:GetEmpInfoByNo")]
        [HttpGet("GetEmpInfoByNo")]
        public async Task<IActionResult> GetEmpInfoByNo(string empno)
        {
            ExecuteResult exeRes = await repairService.GetEmpInfoByNo(empno);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// 获取不良信息
        /// </summary>
        /// <param name="defCode"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:famaintainm:GetDefectByCode")]
        [HttpGet("GetDefectByCode")]
        public async Task<IActionResult> getDefectByCode(string defCode)
        {
            ExecuteResult exeRes = await repairService.GetDefectByCode(defCode);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        ///// <summary>
        ///// 录入
        ///// </summary>
        ///// <param name="sn"></param>
        ///// <param name="defCode"></param>
        ///// <returns></returns>
        //[ActionPermissionFilter(Permission = "repair:famaintainm:appendData")]
        //[HttpGet("appendData")]
        //public async Task<IActionResult> appendData(string sn, string defCode)
        //{
        //    ExecuteResult exeRes = await repairService.appendData(defCode);
        //    if (!exeRes.Status)
        //    {
        //        return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
        //    }

        //    return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        //}

    }
}
