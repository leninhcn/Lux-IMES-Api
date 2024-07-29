using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text.RegularExpressions;
using ZR.Infrastructure.Model;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Admin.WebApi.Controllers.Repair
{
    /// <summary>
    /// 连板维修
    /// </summary>
    [Route("repair/repairpanel")]
    [ApiExplorerSettings(GroupName = "repair")]
    public class RepairPanelController : BaseController
    {
        public IRepairPanelService repairService;
        public RepairPanelController(IRepairPanelService _repairService)
        {
            repairService = _repairService;
        }

        /// <summary>
        /// 查询panel信息
        /// </summary>
        /// <param name="stationType"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairpanel:getPanelByStationType")]
        [HttpGet("getPanelByStationType")]
        public async Task<IActionResult> getPanelByStationType(string stationType)
        {
            if (string.IsNullOrEmpty(stationType))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "请输入StationType信息");
            }

            ExecuteResult exeRes = await repairService.getPanelByStationType(stationType);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// 获取SN信息
        /// editPanel_KeyPress
        /// </summary>
        /// <param name="panel">sn</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairpanel:getSNByPanel")]
        [HttpPost("getSNByPanel")]
        public async Task<IActionResult> getSNByPanel([FromBody] RepairPanel panel)
        {
            if (string.IsNullOrEmpty(panel.panel))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "请输入Panel信息");
            }

            ExecuteResult exeRes = await repairService.getSNByPanel(panel);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", (DataTable)exeRes.Anything));
        }

        /// <summary>
        /// 获取维修信息by sn
        /// combSN_SelectedIndexChanged
        /// </summary>
        /// <param name="cboSn">sn</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairpanel:geInfoBySN")]
        [HttpPost("geInfoBySN")]
        public async Task<IActionResult> geInfoBySN(string cboSn)
        {
            if (string.IsNullOrEmpty(cboSn))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "请输入SN信息");
            }

            ExecuteResult exeRes = await repairService.geInfoBySN(cboSn);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
            panelDefect panelDef  = new panelDefect();
            DataTable dtTemp = new DataTable();
            dtTemp = (DataTable)exeRes.Anything;
            if (dtTemp.Rows.Count > 0)
            {
                panelDef.wo = dtTemp.Rows[0]["WORK_ORDER"].ToString();
                panelDef.partNo = dtTemp.Rows[0]["ipn"].ToString();

                DateTime dtTime = (DateTime)dtTemp.Rows[0]["OUT_STATIONTYPE_TIME"];
                DataTable dt =await  repairService.ShowDefect(cboSn,dtTime);

                if (dt != null && dt.Rows.Count > 0)
                {
                    panelDef.defLine = dtTemp.Rows[0]["line"].ToString();
                    panelDef.defProcess = dtTemp.Rows[0]["STATION_TYPE"].ToString();
                    panelDef.defTerminal = dtTemp.Rows[0]["station_name"].ToString();

                    List<LVDefect> liDef = new List<LVDefect>();
                   
                    string S = "";
                    // add by catherine 2015.03.09 21:35 相同的errorcode可以有不同的location，分别显示出来
                    string L = "";
                    // end by catherine 2015.03.09 21:36
                    for (int i = 0; i <= dtTemp.Rows.Count - 1; i++)
                    {
                        if (dtTemp.Rows[i]["DEFECT_CODE"].ToString() != S || dtTemp.Rows[i]["LOCATION"].ToString() != L)    //同一块板子不能有相同的errorcode
                        {
                            LVDefect vDef = new LVDefect();
                            vDef.DefectCode = dtTemp.Rows[i]["DEFECT_CODE"].ToString();
                            vDef.DEFECT_DESC = dtTemp.Rows[i]["DEFECT_DESC"].ToString();
                            vDef.LOCATION = dtTemp.Rows[i]["LOCATION"].ToString();
                            vDef.RECID = dtTemp.Rows[i]["RECID"].ToString();
                            vDef.STATION_TYPE = dtTemp.Rows[i]["STATION_TYPE"].ToString();
                            liDef.Add(vDef);
                        }
                        S = dtTemp.Rows[i]["DEFECT_CODE"].ToString();
                        L = dtTemp.Rows[i]["LOCATION"].ToString();
                    }
                    panelDef.lVDefects = liDef;
                }
                panelDef.btnRepair = true;
            }

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "", panelDef));
        }

        /// <summary>
        /// 维修 By Panel
        /// btnRepair_Click
        /// </summary>
        /// <param name="panelDto">panel</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "repair:repairpanel:repairByPanel")]
        [HttpPost("repairByPanel")]
        public async Task<IActionResult> repairByPanel([FromBody] panelDto panelDto)
        {
            if (string.IsNullOrEmpty(panelDto.panel))
            {
                return ToResponse(ResultCode.PARAM_ERROR, "请输入Panel信息");
            }

            ExecuteResult exeRes = await repairService.getNextStationType(panelDto.panel);
            if (!exeRes.Status)
            {
                return ToResponse(ResultCode.PARAM_ERROR, exeRes.Message);
            }
            string sNextStationType =(string)exeRes.Anything;

            bool ret = await repairService.repairPanel(panelDto, sNextStationType);

            return ToResponse(new ApiResult((int)ResultCode.SUCCESS, "下一站：" + sNextStationType, ""));
        }
    }
}
