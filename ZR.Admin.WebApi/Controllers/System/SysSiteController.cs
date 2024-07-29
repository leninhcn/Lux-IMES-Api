using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZR.ServiceCore.Services.IService;

namespace ZR.Admin.WebApi.Controllers.System
{
    [Route("system/site")]
    [ApiExplorerSettings(GroupName = "sys")]
    [ApiController]
    public class SysSiteController : BaseController
    {
        readonly ISiteService service;

        public SysSiteController(ISiteService service)
        {
            this.service = service;
        }

        [HttpGet("list")]
        public IActionResult List()
        {
            return SUCCESS(service.GetSiteList());
        }
    }
}
