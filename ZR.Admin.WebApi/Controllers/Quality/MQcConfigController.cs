using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Dynamic;
using System.Text.Json;
using ZR.Admin.WebApi.Filters;
using ZR.Model;
using ZR.Model.Dto.Quality;
using ZR.Model.Quality;
using ZR.Model.System.Dto;
using ZR.Service.Quality.IQualityService;

namespace ZR.Admin.WebApi.Controllers.Quality
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [Verify]
    [Route("quality/qcconfig")]
    public class MQcConfigController : BaseController
    {
        /// <summary>
        /// 接口
        /// </summary>
        private readonly IMQcConfigService _MQcConfigService;

        public MQcConfigController(IMQcConfigService MQcConfigService)
        {
            _MQcConfigService = MQcConfigService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "quality:qcconfig:list")]
        public IActionResult QueryMQcConfig([FromQuery] MQcConfigQueryDto parm)
        {
            //var root = JsonElement.parm;
            //var jtoken = JToken.Parse(root.ToString());
            //var query = new MQcConfigQueryDto();
            var response = _MQcConfigService.GetList(parm);
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询检查规则
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("rules")]
        [ActionPermissionFilter(Permission = "quality:qcconfig:list")]
        public IActionResult Queryrules([FromQuery] Dictionary<string,string> parm)
        {
            var rule = parm["txtcheckrule"]?.ToString();
            var checkrule= parm["checkrule"]?.ToString();
            var response = new DataTable();
            if (checkrule.Equals("工单"))
            {
                response = _MQcConfigService.GetWos(rule);
            }
            if (checkrule.Equals("线别"))
            {
                response = _MQcConfigService.GetLines(rule);
            }
            if (checkrule.Equals("料号"))
            {
                response = _MQcConfigService.GetIpns(rule);
            }

            //datatable转成对象数组
            var res = response.AsEnumerable().Select(r => {
                var cols = response.Columns;
                
                var dr =
                r.ItemArray.Select((it, index) => new KeyValuePair<string, string>(cols[index].ColumnName, it.ToString()));
                return dr;
            });
            return SUCCESS(res);
        }



        /// <summary>
        /// 查询途程
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("route")]
        [ActionPermissionFilter(Permission = "quality:qcconfig:list")]
        public IActionResult Queryroute([FromQuery] Dictionary<string, string> parm)
        {
            var route = parm["route"]?.ToString();
            var txtroute= parm["txtroute"]?.ToString();
            var response = new DataTable();

            if (route.Equals("online"))
            {
                response = _MQcConfigService.GetOnlineRoute(txtroute);
            }
            if (route.Equals("qc"))
            {
                response = _MQcConfigService.GetQcRoute(txtroute);
            }

            //datatable转成对象数组
            var res = response.AsEnumerable().Select(r => {
                var cols = response.Columns;

                var dr =
                r.ItemArray.Select((it, index) => new KeyValuePair<string, string>(cols[index].ColumnName, it.ToString()));
                return dr;
            });
            return SUCCESS(res);
        }


        /// <summary>
        /// 查询途程细节
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("stations")]
        [ActionPermissionFilter(Permission = "quality:qcconfig:list")]
        public IActionResult Querystations([FromQuery] Dictionary<string, string> parm)
        {
            var route = parm["route"]?.ToString();
            var response = new DataTable();
            response = _MQcConfigService.GetRouteDetail(route);
            //datatable转成行对象数组
            var res = response.AsEnumerable().Select(r => {
                var cols = response.Columns;
                var exobj = new ExpandoObject();
                for (int i = 0; i < r.ItemArray.Length; i++)
                {
                    exobj.TryAdd(cols[i].ColumnName, r.ItemArray[i].ToString());
                }
                return exobj;
            });
            return SUCCESS(res);

        }


        /// <summary>
        /// 查询途程细节
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("qclevel")]
        [ActionPermissionFilter(Permission = "quality:qcconfig:list")]
        public IActionResult QueryQcLevel([FromQuery] Dictionary<string, string> parm)
        {
            var station = parm["station"]?.ToString();
            var checkrule = parm["checkrule"]?.ToString();
            var res = _MQcConfigService.GetQcLevel(station,checkrule);
            return SUCCESS(new {val=res });

        }


        /// <summary>
        /// 根据给定一个值确定是工单还是线别还是料号
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("ruletype")]
        [ActionPermissionFilter(Permission = "quality:qcconfig:list")]
        public IActionResult QueryQcRuleType([FromQuery] Dictionary<string, string> parm)
        {
            var ruletype = parm["ruletype"]?.ToString();
            var res = _MQcConfigService.GetRuleType(ruletype);
            return SUCCESS(new { val = res });

        }



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "quality:qcconfig:query")]
        public IActionResult GetMQcConfig(int Id)
        {
            var response = _MQcConfigService.GetInfo(Id);

            var info = response.Adapt<MQcConfig>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "quality:qcconfig:add")]
        [Log(Title = "", BusinessType = BusinessType.INSERT)]
        public IActionResult AddMQcConfig([FromBody] MQcConfigDto parm)
        {
            var modal = parm.Adapt<MQcConfig>().ToCreate(HttpContext);

            var response = _MQcConfigService.AddMQcConfig(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "quality:qcconfig:edit")]
        [Log(Title = "", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateMQcConfig([FromBody] MQcConfigDto parm)
        {
            var modal = parm.Adapt<MQcConfig>().ToUpdate(HttpContext);
            var response = _MQcConfigService.UpdateMQcConfig(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "quality:qcconfig:delete")]
        [Log(Title = "", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteMQcConfig(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _MQcConfigService.Delete(idsArr);

            return ToResponse(response);
        }



        /// <summary>
        /// MQcConfig导出
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpGet("export")]
        [Log(Title = "MQcConfig导出", BusinessType = BusinessType.EXPORT)]
        [ActionPermissionFilter(Permission = "quality:qcconfig:export")]
        public IActionResult ExportMQcConfig([FromQuery] MQcConfigQueryDto config)
        {
            var list = _MQcConfigService.GetList(config, new PagerInfo(1, 10000));

            var result = ExportExcelMini(list.Result, "qcconfig", "qcconfig");
            return ExportExcel(result.Item2, result.Item1);
        }


    }



}
