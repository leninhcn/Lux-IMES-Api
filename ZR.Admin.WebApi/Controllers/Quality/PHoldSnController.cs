using Microsoft.AspNetCore.Mvc;
using ZR.Admin.WebApi.Filters;
using ZR.Model.Dto.Quality;
using ZR.Model.Quality;
using ZR.Service.Quality.IQualityService;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using JinianNet.JNTemplate.Dynamic;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Schema;

namespace ZR.Admin.WebApi.Controllers.Quality
{


    /// <summary>
    /// 卡控管理
    /// </summary>
    /// 
    [Verify]
    [Route("quality/hold")]
    public class PHoldSnController : BaseController
    {
        /// <summary>
        /// 接口
        /// </summary>
        private readonly IPHoldSnService _PHoldSnService;
        private readonly IHoldOptionsService _HoldOptionsService;

        public PHoldSnController(IPHoldSnService PHoldSnService, IHoldOptionsService PHoldOptionsService )
        {
            _PHoldSnService = PHoldSnService;
            _HoldOptionsService = PHoldOptionsService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "quality:hold:list")]
        //public IActionResult QueryPHoldSn([FromQuery] Dictionary<string,string> parm)
        public IActionResult QueryPHoldSn([FromQuery] PHoldSnQueryDto parm)
        {
            var response = _PHoldSnService.GetList(parm);
            return SUCCESS(response);
            //return SUCCESS("");
        }


        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("options")]
        [ActionPermissionFilter(Permission = "quality:hold:list")]
        //public IActionResult QueryPHoldSn([FromQuery] Dictionary<string,string> parm)
        public IActionResult QueryPHoldOptions([FromQuery] PHoldOptionsQueryDto parm)
        {
            var response = _HoldOptionsService.GetPHoldOptionsDto();
            return SUCCESS(response);
            //return SUCCESS("");
        }



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "business:pholdsn:query")]
        public IActionResult GetPHoldSn(int Id)
        {
            var response = _PHoldSnService.GetInfo(Id);

            var info = response.Adapt<PHoldSn>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost("excute")]
        [ActionPermissionFilter(Permission = "business:pholdsn:add")]
        [Log(Title = "", BusinessType = BusinessType.INSERT)]
        //public IActionResult AddPHoldSn([FromBody] PHoldSnDto parm)
        public IActionResult AddPHoldSn([FromBody] dynamic parm)
        {
            //var modal = parm.Adapt<PHoldSn>().ToCreate(HttpContext);

            //var response = _PHoldSnService.AddPHoldSn(modal);
            var root = (JsonElement)parm;
            var jtoken = JToken.Parse(root.ToString());
            var holdcode = jtoken["holdcode"].ParseToString();
            var station = jtoken["allstations"].ParseToBool() ? "ALL" : string.Join(",", jtoken["station"].Select(s=>s.ToString()));
            var barcodelist = jtoken["barcodelist"].IsNullOrEmpty() ? "" : string.Join(",", jtoken["barcodelist"].Select(s => s["barcode"]?.ParseToString()));
            //var barcodes = root.GetProperty("barcodelist")
            //                     .EnumerateArray()
            //                     .Select(r=>r.GetProperty("barcode").GetString())
            //                     .ToArray();
            var modal =new PHoldSn { Sn=barcodelist, StationType=station,CreateEmpno = jtoken["empno"]?.ParseToString(), HoldEmpno = jtoken["holddemond"]?.ParseToString() };
               
            var response = _PHoldSnService.AddPHoldSnByTran(modal);
            return SUCCESS(response);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPost("release")]
        [ActionPermissionFilter(Permission = "business:pholdsn:edit")]
        [Log(Title = "", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePHoldSn([FromBody] dynamic parm)
        {
            //var modal = parm.Adapt<PHoldSn>().ToUpdate(HttpContext);
            var root = (JsonElement)parm;
            var jtoken = JToken.Parse(root.ToString());
            var modals = jtoken.Select(r => new PHoldSn() {Id= r["id"].ParseToInt() ,Enabled="N"}).ToArray();

            var response = _PHoldSnService.UpdatePHoldSns(modals);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete")]
        [ActionPermissionFilter(Permission = "business:pholdsn:delete")]
        [Log(Title = "", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePHoldSn([FromBody] dynamic parm)
        {
            //int[] idsArr = Tools.SpitIntArrary(ids);
            //if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            //var response = _PHoldSnService.Delete(idsArr);
            var root = (JsonElement)parm;
            var jtoken = JToken.Parse(root.ToString());
            var idsArr = jtoken.Select(r => r["id"].ParseToInt()).ToArray();
            var response = _PHoldSnService.Delete(idsArr);

            return ToResponse(response);
        }




    }





}
