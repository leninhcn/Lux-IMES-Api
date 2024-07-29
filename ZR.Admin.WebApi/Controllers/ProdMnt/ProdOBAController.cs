using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.ProdMnt
{
    [Route("prodMnt/prodOBA/[action]")]
    [ApiController]
    public class ProdOBAController : BaseController
    {
        readonly IProdOBAService service;
        public ProdOBAController(IProdOBAService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetSnInfo(string sn)
        {
            return Ok(new
            {
                data = await service.GetSnInfo(sn).Select(x => new
                {
                    x.SerialNumber,
                    x.Line,
                    x.StationType,
                }).SingleAsync()
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetSnFromKpItem(string kpItem)
        {
            return Ok(new { data = await service.GetSnFromKpItem(kpItem) });
        }

        [HttpGet]
        public async Task<IActionResult> InsertOBA(string sn, string? errorMsg, string line, string stationType, string status) {
            return Ok(new { yes = await service.InsertOBA(sn, errorMsg, line, stationType, status, HttpContext.GetName()) });
        }

        [HttpGet]
        public async Task<IActionResult> InsertHold(string sn, string? holdMsg)
        {
            return Ok(new {yes = await service.InsertHold(sn, holdMsg, HttpContext.GetName()) });
        }

        void Ins() => new string("");
    }
}
