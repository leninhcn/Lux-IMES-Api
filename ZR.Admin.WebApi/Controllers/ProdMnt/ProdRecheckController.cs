using Microsoft.AspNetCore.Mvc;
using ZR.Common;
using ZR.Model.Business;
using ZR.Service;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.ProdMnt
{
    [Route("prodMnt/prodRecheck/[action]")]
    [ApiController]
    public class ProdRecheckController : BaseController
    {
        readonly IProdRecheckService service;
        public ProdRecheckController(IProdRecheckService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetHtValues()
        {
            return Ok(new { data = await service.GetHtValues() });
        }

        [HttpGet]
        public async Task<IActionResult> GetSnDetail(string sn, string type)
        {
            return Ok(new {data = await service.GetSnDetail(sn, type) });
        }

        [HttpGet]
        public async Task<IActionResult> CheckLogic(string sn, string type, string qcInStationType, string qcOutStationType)
        {
            var uid = HttpContext.GetName();
            var msg = await service.CheckLogic(sn, type, qcInStationType, qcOutStationType, uid);
            var ok = msg == "OK";

            return Ok(new { ok, data = msg });
        }
    }
}
