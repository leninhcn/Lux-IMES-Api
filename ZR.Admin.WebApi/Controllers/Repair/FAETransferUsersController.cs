using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZR.Infrastructure.Model;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Admin.WebApi.Controllers.Repair
{
    /// <summary>
    /// FAE借出人员维护
    /// </summary>
    [Route("repair/faetransferuser")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class FAETransferUsersController : BaseController
    {
        public IFAETransferUsersService repairService;
        public FAETransferUsersController(IFAETransferUsersService _repairService)
        {
            repairService = _repairService;
        }

        /// <summary>
        /// Show数据
        /// </summary>
        /// <param name="retData"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:faetransferuser:getShowData")]
        [HttpPost("getShowData")]
        public async Task<IActionResult> getShowData([FromBody] FAETransUser retData)
        {
            ExecuteResult exeRes = await repairService.ShowData(retData);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// 新增修改删除
        /// </summary>
        /// <param name="retData"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:faetransferuser:EditValueInfo")]
        [HttpPost("EditValueInfo")]
        public async Task<IActionResult> EditValueInfo([FromBody] FAETransInfo retData)
        {
            string userNo = HttpContext.GetName();
            ExecuteResult exeRes = await repairService.EditValueInfo(retData, userNo);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:faetransferuser:GetHtValues")]
        [HttpGet("GetHtValues")]
        public async Task<IActionResult> GetHtValues(string id)
        {
            ExecuteResult exeRes = await repairService.GetHtValues(id);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="retData"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:faetransferuser:importData")]
        [HttpPost("importData")]
        public async Task<IActionResult> importData([FromBody] List<FAETransInfo> retData)
        {
            string userNo = HttpContext.GetName();
            ExecuteResult exeRes = await repairService.importDataToTable(retData, userNo);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", ""));
        }

    }
}
