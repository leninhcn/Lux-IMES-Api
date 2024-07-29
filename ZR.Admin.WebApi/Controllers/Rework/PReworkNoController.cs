using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using ZR.Admin.WebApi.Filters;
using ZR.Model.Rework;
using ZR.Service.Rework.IReworkService;

namespace ZR.Admin.WebApi.Controllers.Rework
{
    /// <summary>
    /// 
    /// </summary>
    [Verify]
    [Route("rework/excute")]
    public class PReworkNoController : BaseController
    {
        /// <summary>
        /// 接口
        /// </summary>
        private readonly IPReworkNoService _PReworkNoService;

        public PReworkNoController(IPReworkNoService PReworkNoService)
        {
            _PReworkNoService = PReworkNoService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        //[HttpGet("list")]
        //[ActionPermissionFilter(Permission = "business:preworkno:list")]
        //public IActionResult QueryPReworkNo([FromQuery] PReworkNoQueryDto parm)
        //{
        //    var response = _PReworkNoService.GetList(parm);
        //    return SUCCESS(response);
        //}


        /// <summary>
        /// 查询spec
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("spec")]
        [ActionPermissionFilter(Permission = "rework:excute:list")]
        public IActionResult Queryspec([FromQuery] Dictionary<string, string> parm)
        {
            var input = parm["input"]?.ToString();
            var response = _PReworkNoService.GetSpec(input);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询reworkno
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("reworkno")]
        [ActionPermissionFilter(Permission = "rework:excute:reworkno")]
        public IActionResult Queryreworkno([FromQuery] Dictionary<string, string> parm)
        {
            var input = parm["input"]?.ToString();
            var response = _PReworkNoService.GetReworkno(input).Rows[0][0].ToString();
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询reworkno
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("stationoptions")]
        [ActionPermissionFilter(Permission = "rework:excute:stationoptions")]
        public IActionResult Querystationoptions([FromQuery] Dictionary<string, string> parm)
        {
            var input = parm["input"]?.ToString();
            var response = _PReworkNoService.GetStationOptions(input);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询途程名称
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("route")]
        [ActionPermissionFilter(Permission = "rework:excute:route")]
        public IActionResult QueryRouteName([FromQuery] Dictionary<string, string> parm)
        {
            var input = parm["input"]?.ToString();
            var response = _PReworkNoService.GetRoute(input);
            return SUCCESS(response);
        }


        /// <summary>
        /// 提前检查重工条件
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>

        [HttpGet("precheck")]
        [ActionPermissionFilter(Permission = "rework:excute:precheck")]
        public IActionResult PreCheck([FromQuery] Dictionary<string, string> parm)
        {
            var input = parm["input"]?.ToString();
            var inputtype= parm["inputtype"]?.ToString();
            var isnewwo = int.Parse(parm["isnewwo"]?.ToString());
            var newwo= parm["newwo"]?.ToString();
            var tstation = parm["tstation"]?.ToString();
            var response = _PReworkNoService.PreCheck(input,inputtype,isnewwo,newwo,tstation);
            return SUCCESS(response);
        }



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "rework:preworkno:query")]
        public IActionResult GetPReworkNo(int Id)
        {
            var response = _PReworkNoService.GetInfo(Id);

            var info = response.Adapt<PReworkNo>();
            return SUCCESS(info);
        }




        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "rework:excute:excute")]
        [Log(Title = "", BusinessType = BusinessType.OTHER)]
        public IActionResult ExcuteRework([FromBody] dynamic parm)
        {
            //var modal = parm.Adapt<PReworkNo>().ToCreate(HttpContext);

            //var response = _PReworkNoService.AddPReworkNo(modal);
            var root = (JsonElement)parm;
            var jtoken = JToken.Parse(root.ToString());
            var emp= jtoken["emp"].ParseToString();
            var imported= jtoken["imported"].ParseToBool();
            var isnewwo = jtoken["isnewwo"].ParseToBool();
            var inputtype= jtoken["inputtype"].ParseToString();
            var newwo= jtoken["newwo"].ParseToString();
            var inputvalue= jtoken["barcode"].ParseToString();
            var reworkno = jtoken["reworkno"].ParseToString();
            var routename = jtoken["routename"].ParseToString();
            var returnstation = jtoken["returnstation"].ParseToString();
            var checkstation = jtoken["checkstation"].ParseToString();
            var remark = jtoken["remark"].ParseToString();
            var incidentals = jtoken["incidentals"]
                .ToArray()
                .Select(x=>x.ParseToString())
                .ToArray();
            var tvalue= jtoken["tvalue"]
                .ToArray()
                .Select(x => x.ParseToString())
                .ToArray();
            var reworkList = new string[1];
            var dict = new Dictionary<string, object>() ;
            dict.Add("imported", imported);
            dict.Add("isnewwo", isnewwo);
            dict.Add("inputtype", inputtype);
            dict.Add("newwo", newwo);
            dict.Add("inputvalue", inputvalue);
            dict.Add("reworkno", reworkno);
            dict.Add("routename", routename);
            dict.Add("returnstation", returnstation);
            dict.Add("checkstation", checkstation);
            dict.Add("remark", remark);
            dict.Add("incidentals", incidentals);
            dict.Add("tvalue", tvalue);
            dict.Add("emp", emp);
            if (imported)
            {
                reworkList= jtoken["reworkList"]
                .ToArray()
                .Select(x => x["sn"].ParseToString())
                .ToArray();
                dict.Add("reworkList", reworkList);
            }
            var res = _PReworkNoService.ReworkExcute(dict);
            return SUCCESS(res);
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        [ActionPermissionFilter(Permission = "rework:excute:add")]
        [Log(Title = "", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPReworkNo([FromBody] dynamic parm)
        {
            var modal = parm.Adapt<PReworkNo>().ToCreate(HttpContext);

            var response = _PReworkNoService.AddPReworkNo(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        //[HttpPut]
        //[ActionPermissionFilter(Permission = "business:preworkno:edit")]
        //[Log(Title = "", BusinessType = BusinessType.UPDATE)]
        //public IActionResult UpdatePReworkNo([FromBody] PReworkNoDto parm)
        //{
        //    var modal = parm.Adapt<PReworkNo>().ToUpdate(HttpContext);
        //    var response = _PReworkNoService.UpdatePReworkNo(modal);

        //    return ToResponse(response);
        //}

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:preworkno:delete")]
        [Log(Title = "", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePReworkNo(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _PReworkNoService.Delete(idsArr);

            return ToResponse(response);
        }




    }



}
