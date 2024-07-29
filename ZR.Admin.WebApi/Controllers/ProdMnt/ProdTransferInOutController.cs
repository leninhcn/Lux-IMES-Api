using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.ProdMnt
{
    [Route("prodMnt/prodTransferInOut/[action]")]
    [ApiController]
    public class ProdTransferInOutController : BaseController
    {
        readonly IProdTransferInOutService service;
        public ProdTransferInOutController(IProdTransferInOutService service)
        {
            this.service = service;
        }


    }
}
