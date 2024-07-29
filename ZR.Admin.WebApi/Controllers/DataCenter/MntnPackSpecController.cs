using Microsoft.AspNetCore.Mvc;
using ZR.Model.Dto.ProdDto;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.DataCenter
{
    /// <summary>
    /// 包装信息->包规维护
    /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("MntnPackSpec/MntnPackSpecController/")]
    [ApiController]
    public class MntnPackSpecController : BaseController
    {

        readonly ImntnPackSpecService service;

        public MntnPackSpecController(ImntnPackSpecService service)
        {
            this.service = service;
        }


        /// <summary>
        /// 包规维护-->分页查询1
        /// </summary>
        [HttpGet("MntnPackSpeclist")]
        public Task<IActionResult> MntnPackSpeclist( string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.MntnPackSpeclist(textData, pageNum, pageSize, HttpContext.GetSite())));
        }

        /// <summary>
        /// 包规维护-->新增 pkspecName
        /// </summary>
        [HttpPost("MntnPackSpecInsert")]
        public Task<IActionResult> MntnPackSpecInsert(ImesMpkspec imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();

            return Task.FromResult<IActionResult>(Ok(service.MntnPackSpecInsert(imes)));
        }

        /// <summary>
        /// 包规维护-->修改
        /// </summary>
        [HttpPut("MntnPackSpecUpdate")]
        public Task<IActionResult> MntnPackSpecUpdate(ImesMpkspec imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            imes.createEmpno = HttpContext.GetName();
            return Task.FromResult<IActionResult>(Ok(service.MntnPackSpecUpdate(imes)));
        }

        /// <summary>
        /// 包规维护-->删除
        /// </summary>
        [HttpDelete("MntnPackSpecDelet")]
        public Task<IActionResult> MntnPackSpecDelet(ImesMpkspec imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.MntnPackSpecDelet(imes)));
        }

        /// <summary>
        /// 包规维护-->历史记录查询
        /// </summary>
        [HttpGet("MntnPackSpeclistHt")]
        public Task<IActionResult> MntnPackSpeclistHt(string pkspecName, string id)
        {
            return Task.FromResult<IActionResult>(Ok(service.MntnPackSpeclistHt(pkspecName, id, HttpContext.GetSite())));
        }





    }
}
