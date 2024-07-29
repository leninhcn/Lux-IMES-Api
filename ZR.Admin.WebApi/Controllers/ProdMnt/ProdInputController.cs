using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.ProdMnt
{
    [Route("prodMnt/prodInput/[action]")]
    [ApiController]
    public class ProdInputController : BaseController
    {
        readonly IProdInputService prodInputService;
        public ProdInputController(IProdInputService prodInputService) {
            this.prodInputService = prodInputService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStationInfo(string stationType)
        {
            return Ok(new { stationDesc = await prodInputService.GetStationDesc(stationType) });
        }

        [HttpGet]
        public async Task<IActionResult> GetLineWo(string line, string? wo = "")
        {
            return Ok(new { workOrders = await prodInputService.GetLineWo(line, wo) });
        }

        [HttpGet]
        public async Task<IActionResult> GetOprateStep(string stationType)
        {
            return Ok(new
            {
                steps = await prodInputService.GetOprateStep(stationType)
                .Select(a => new { a.StepSeq, a.StepInput, a.StepDesc })
                .ToListAsync()
            });
        }

        [HttpGet]
        public async Task<IActionResult> CheckSNStatus(string sn)
        {
            return Ok(new { status = await prodInputService.CheckSNStatus(sn) });
        }

        [HttpGet]
        public async Task<IActionResult> GetValues(string inputValue, string item, string? modelName = "")
        {
            var ok = false;
            var (tValue, tRes) = await prodInputService.GetValues(inputValue, item, modelName);

            string? data;
            if (tRes != "OK")
            {
                data = tRes;
            }
            else
            {
                ok = true;
                data = tValue;
            }

            return Ok(new {ok, data});
        }

        [HttpGet]
        public async Task<IActionResult> GetPamSn(string wo)
        {
            var ok = false;
            var (tSn, tRes) = await prodInputService.GetPamSn(wo);

            string? data;
            if (tRes != "OK")
            {
                data = tRes;
            }
            else
            {
                ok = true;
                data = tSn;
            }

            return Ok(new { ok, data });
        }

        [HttpGet]
        public async Task<IActionResult> CheckValue(string inputValue, string item, string? modelName = "")
        {
            var ok = false;
            var (_, tRes) = await prodInputService.CheckValue(inputValue, item, modelName);

            string data = default;
            if (tRes != "OK")
            {
                data = tRes;
            }
            else
            {
                ok = true;
            }

            return Ok(new { ok, data });
        }

        [HttpGet]
        public async Task<IActionResult> GetSNLinkKpsnInfo(string stationType, string sn)
        {
            return Ok(new { kpList = await prodInputService.GetSNLinkKpsnInfo(stationType, sn) });
        }

        [HttpGet]
        public async Task<IActionResult> ClearGetSN(string sn)
        {
            return Ok(new { ok = await prodInputService.ClearGetSN(sn) });
        }

        [HttpGet]
        public async Task<IActionResult> GetWoSnStatus(string wo)
        {
            return Ok(new
            {
                data = await prodInputService.GetWoSnStatus(wo).Select(a => new
                {
                    a.WorkOrder,
                    a.SerialNumber,
                    a.StationType,
                    a.NextStationType,
                }).ToListAsync()
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetWoBomKeyparts(string wo, string stationType)
        {
            var data = await prodInputService.GetWoBomKeyparts(wo, stationType)
                .Select((a, b, c, d, e) => new {
                    a.WorkOrder,
                    a.Ipn,
                    b.KpSpec,
                    c.ItemIpn,
                    c.ItemGroup,
                    c.ItemCount
                })
                .Distinct().ToListAsync();

            return Ok(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> GetStationLinkCount(string model, string stationType)
        {
            return Ok(new {count = await prodInputService.GetStationLinkCount(model, stationType) });
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpWoPassCount(string station)
        {
            var uid = HttpContext.GetName();
            return Ok(new { count = await prodInputService.GetEmpWoPassCount(station, uid) });
        }

        [HttpGet]
        public async Task<IActionResult> CheckKpsn(string sn, string wo, string kpsn, string station)
        {
            var uid = HttpContext.GetName();

            var ok = false;
            var (tItemIpn, tRes) = await prodInputService.CheckKpsn(sn, wo, kpsn, station, uid);

            string data = default;
            if (tRes != "OK")
            {
                data = tRes;
            }
            else
            {
                ok = true;
                data = tItemIpn;
            }

            return Ok(new { ok, data });
        }

        [HttpGet]
        public async Task<IActionResult> CheckStockOut(string stationType)
        {
            return Ok(new { any = await prodInputService.CheckStockOut(stationType) });
        }

        [HttpGet]
        public async Task<IActionResult> CheckLogic(string? sn, string? wo, string? kpsn, string? model, string? tool, string? glue, string? reel, string? station, string? route, string? step, string? errorCode)
        {
            var uid = HttpContext.GetName();

            var msg = await prodInputService.CheckLogic(sn, wo, kpsn, model, tool, glue, reel, station, route, step, errorCode, uid);

            var ok = msg == "OK";

            return Ok(new { ok, data = msg });
        }
    }
}
