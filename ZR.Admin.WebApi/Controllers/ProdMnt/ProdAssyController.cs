using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;
using System.Threading;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Service.IService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZR.Admin.WebApi.Controllers.ProdMnt
{
    [Route("prodMnt/prodAssy/[action]")]
    [ApiController]
    public class ProdAssyController : BaseController
    {
        readonly IProdAssyService prodAssyService;

        public ProdAssyController(IProdAssyService prodAssyService)
        {
            this.prodAssyService = prodAssyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStationActionList([FromQuery] StationInfoDto stationInfo)
        {
            return Ok(new { list = await prodAssyService.GetStationActionList(stationInfo) });
        }

        [HttpGet]
        public async Task<IActionResult> CheckIsLotAssy(string line, string stationType)
        {
            return Ok(new { isLotAssy = await prodAssyService.CheckIsLotAssy(line, stationType) });
        }

        [HttpGet]
        public async Task<IActionResult> GetWoList(string station, string cmd)
        {
            var (data, res) = await prodAssyService.GetWoList(station, cmd);

            return Ok(new { ok = res == "OK", msg = res, list = data.Split('|').ToList() });
        }

        [HttpGet]
        public async Task<IActionResult> GetMainSn(string input, string station)
        {
            var (data, msg) = await prodAssyService.GetMainSn(input, station);

            var ok = msg == "OK";
            return Ok(new { ok, msg, data = ok ? data : input });
        }

        [HttpGet]
        public async Task<IActionResult> GetSnInfoBySn(string mainSn)
        {
            return Ok(new { data = await prodAssyService.GetSnInfoBySn(mainSn) });
        }

        [HttpGet]
        public async Task<IActionResult> CheckWo(string wo, string station)
        {
            var (data, msg) = await prodAssyService.CheckWo(wo, station);

            return Ok(new { ok = msg == "OK", msg = msg, woMsg = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetWoInfoByWo(string wo)
        {
            return Ok(new { data = await prodAssyService.GetWoInfoByWo(wo) });
        }

        [HttpGet]
        public async Task<IActionResult> GetBomInfosByWO(string stationType, string wo)
        {
            return Ok(new { data = await prodAssyService.GetBomInfosByWO(stationType, wo) });
        }

        [HttpGet]
        public async Task<IActionResult> GetModel(string mainSn)
        {
            return Ok(new { list = await prodAssyService.GetModel(mainSn) });
        }

        [HttpGet]
        public async Task<IActionResult> CheckMachine(string station, string? wo, string sn, string machine)
        {
            var uid = HttpContext.GetName();

            var msg = await prodAssyService.CheckMachine(station, wo, sn, machine, uid);
            return Ok(new { ok = msg == "OK", msg });
        }

        [HttpGet]
        public async Task<IActionResult> CheckTooling(string station, string? wo, string sn, string tool)
        {
            var uid = HttpContext.GetName();

            var (data, msg) = await prodAssyService.CheckTooling(station, wo, sn, tool, uid);
            return Ok(new { ok = msg == "OK", msg, data });
        }

        [HttpGet]
        public async Task<IActionResult> CheckCarrier(string station, string? wo, string sn, string carrier)
        {
            var uid = HttpContext.GetName();

            var (data, msg) = await prodAssyService.CheckCarrier(station, wo, sn, carrier, uid);
            return Ok(new { ok = msg == "OK", msg, data });
        }

        [HttpGet]
        public async Task<IActionResult> CheckSNInput1WO(string station, string? wo, string sn)
        {
            var uid = HttpContext.GetName();

            sn += "-" + Guid.NewGuid().ToString("N").ToUpper();

            var (data, msg) = await prodAssyService.CheckSNInput1WO(station, wo, sn, uid);
            return Ok(new { ok = msg == "OK", msg, data });
        }

        [HttpGet]
        public async Task<IActionResult> CheckIsErrorCode(string ecode)
        {
            return Ok(new { yes = await prodAssyService.CheckIsErrorCode(ecode) });
        }

        [HttpGet]
        public async Task<IActionResult> CheckSnBefore(string station, string? wo, string sn)
        {
            var uid = HttpContext.GetName();

            var (data, msg) = await prodAssyService.CheckSnBefore(station, wo, sn, uid);
            return Ok(new { ok = msg == "OK", msg, data });
        }

        [HttpGet]
        public async Task<IActionResult> CheckEssTest(string stationType, string ess)
        {
            return Ok(new { ok = await prodAssyService.CheckEssTest(stationType, ess) });
        }

        [HttpGet]
        public async Task<IActionResult> CheckKpsnIputMPN(string mpn, string ipn)
        {
            var msg = await prodAssyService.CheckKpsnIputMPN(mpn, ipn);
            return Ok(new { ok = msg == "OK", msg, });
        }

        [HttpGet]
        public async Task<IActionResult> CheckPalos(string sn)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<ActionResult> Check5DX()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<ActionResult> CheckSnPass(string station, string wo, string sn)
        {
            var (data, msg) = await prodAssyService.CheckSnPass(station, wo, sn, HttpContext.GetName());
            return Ok(new { ok = msg == "OK", msg, data });
        }


        [HttpGet]
        public async Task<ActionResult> CetItemInfo(string sn)
        {
            return Ok(new { data = await prodAssyService.GetItemInfo(sn) });
        }

        [HttpGet]
        public async Task<ActionResult> GetBomInfosBySN(string stationType, string sn)
        {
            return Ok(new { data = await prodAssyService.GetBomInfosBySN(stationType, sn) });
        }

        [HttpGet]
        public async Task<ActionResult> GetCompareImageFileName(string station, string wo, string sn)
        {
            var (data, msg) = await prodAssyService.GetCompareImageFileName(station, wo, sn, HttpContext.GetName());
            return Ok(new { ok = msg == "OK", msg, data });
        }

        [HttpGet]
        public async Task<ActionResult> GetPanelLinkQtyBySN(string sn)
        {
            return Ok(new { data = await prodAssyService.GetPanelLinkQtyBySN(sn) });
        }

        [HttpGet]
        public async Task<ActionResult> CheckSNInput2WO(string station, string wo, string sn)
        {
            var (data, msg) = await prodAssyService.CheckSNInput2WO(station, wo, sn, HttpContext.GetName());
            return Ok(new { ok = msg == "OK", msg, data });
        }

        [HttpGet]
        public async Task<ActionResult> CheckSnPassPanel(string station, string wo, string sn)
        {
            var  msg = await prodAssyService.CheckSnPassPanel(station, wo, sn, HttpContext.GetName());
            return Ok(new { ok = msg == "OK", msg });
        }

        [HttpGet]
        public async Task<ActionResult> CheckSnPassBundle(string station, string wo, string sn)
        {
            var msg = await prodAssyService.CheckSnPassBundle(station, wo, sn, HttpContext.GetName());
            return Ok(new { ok = msg == "OK", msg });
        }

        [HttpGet]
        public async Task<ActionResult> CheckSNInputNoWo(string station, string wo, string input)
        {
            var (data, msg) = await prodAssyService.CheckSNInputNoWo(station, wo, input, HttpContext.GetName());
            return Ok(new { ok = msg == "OK", msg, data });
        }

        [HttpGet]
        public async Task<ActionResult> GetHDDInfo(string sn)
        {
            return Ok(new { data = await prodAssyService.GetHDDInfo(sn) });
        }

        [HttpGet]
        public async Task<ActionResult> CheckFixedAssets(string sn)
        {
            return Ok(new { yes = await prodAssyService.CheckFixedAssets(sn) });
        }

        [HttpGet]
        public async Task<ActionResult> RelieveLink(string sn)
        {
            return Ok(new { yes = await prodAssyService.RelieveLink(sn) });
        }

        [HttpGet]
        public async Task<ActionResult> GetIPNAPNBySN(string sn)
        {
            return Ok(new { data = await prodAssyService.GetIPNAPNBySN(sn) });
        }

        [HttpGet]
        public async Task<ActionResult> GetRule()
        {
            return Ok(new { data = await prodAssyService.GetRule().Select((a, b) => new { 
                b.ConfigName,
                b.ConfigDesc,
                b.ConfigValue
            }).ToListAsync() });
        }

        [HttpGet]
        public async Task<ActionResult> CheckIPN(string sn)
        {
            var msg = await prodAssyService.CheckIPN(sn);
            return Ok(new { ok = msg == "OK", msg });
        }

        [HttpGet]
        public async Task<ActionResult> CheckALFixedAssetsIsExist(string sn)
        {
            var dt = await prodAssyService.CheckALFixedAssetsIsExist(sn);
            return Ok(new { ok = dt != null, msg = dt == null ? "CheckALFixedAssetsIsExist: 获取数据异常" : null, data = dt });
        }

        [HttpGet]
        public async Task<ActionResult> GetTdId()
        {
            var dt = await prodAssyService.GetTdId();
            return Ok(new { ok = dt != null, msg = dt == null ? "GetTdId: 获取数据异常" : null, data = dt });
        }

        [HttpGet]
        public async Task<ActionResult> InsertALLabelInfo(string tdId, string sn) 
        {
            return Ok(new { ok = await prodAssyService.InsertALLabelInfo(tdId, sn, HttpContext.GetName()) });
        }
    }
}
