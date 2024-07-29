using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using System.Security.Policy;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.System;
using ZR.Service;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.WoManagement
{
    [Route("womanagement/mntnwobomlink")]
    [ApiController]
    public class MntnWoBomLinkController : BaseController
    {
        private readonly IMntnWoBomLinkService _mntnWoBomLinkService;
        public MntnWoBomLinkController(IMntnWoBomLinkService mntnWoBomLinkService)
        {
            _mntnWoBomLinkService = mntnWoBomLinkService;
        }
        [HttpGet("getwobase")]
        public IActionResult GetWoBaseList([FromQuery]PWoBomQueryDto parm)
        {
            parm.site=HttpContext.GetSite();
            var data = _mntnWoBomLinkService.GetWoBase(parm);
            return  SUCCESS(data, TIME_FORMAT_FULL);

        }
        [HttpGet("getwobom")]
        public IActionResult GetWoBomList([FromQuery] PWoBomQueryDto parm)
        {
            parm.site = HttpContext.GetSite();
            var data = _mntnWoBomLinkService.GetWoBom(parm);
            return SUCCESS(data, TIME_FORMAT_FULL);

        }
        [HttpPost("updatewobom")]
        public IActionResult UpdateWoBom([FromBody] WoBom parm)
        {
            parm.Site = HttpContext.GetSite();
            parm.UpdateEmpno = HttpContext.GetName();
            parm.UpdateTime = DateTime.Now;
            var data = _mntnWoBomLinkService.UpdatePWoBom(parm);
            return SUCCESS(data, TIME_FORMAT_FULL);

        }
        [HttpPost("addwobom")]
        public IActionResult AddWoBom([FromBody] WoBom parm)
        {
            parm.Site=HttpContext.GetSite();
            parm.UpdateEmpno = HttpContext.GetName();
            var data = _mntnWoBomLinkService.AddPWoBom(parm);
            return SUCCESS(data, TIME_FORMAT_FULL);
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "用户导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        //[ActionPermissionFilter(Permission = "system:user:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            try { 
            var site = HttpContext.GetSite();
            List<WoBom> wobom = new();
            using (var stream = formFile.OpenReadStream())
            {
                wobom = stream.Query<WoBom>(startCell: "A1").ToList();
            }
            return SUCCESS(_mntnWoBomLinkService.ImportWoBom(wobom,site));
            }
            catch (Exception ex)
            {
                (string, object, object) res = ("ItemCount必须为数字 或者 "+ex.ToString().Substring(0,20), "", "");
                return SUCCESS(res);
            }
        }
        /// <summary>
        /// 用户导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "用户模板", BusinessType = BusinessType.EXPORT, IsSaveRequestData = true, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            (string, string) result = DownloadImportTemplate("wobom");
            return ExportExcel(result.Item2, result.Item1);
        }

    }
}
