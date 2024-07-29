using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Service.IService;
namespace ZR.Admin.WebApi.Controllers.PDA
{
    [Route("pda/Api")]
    [ApiController]
    public class pdaController : BaseController
    {
        private readonly IMntnWorkOrderService _mntnWorkOrderService;
        private IConfiguration Configuration { get; set; }


        //public PagedInfo<MStationTypeDto> GetList(MStationTypeQueryDto parm)
        //{
        //    var predicate = Expressionable.Create<MStationType>();

        //    var response = Queryable()
        //        .Where(predicate.ToExpression())
        //        .ToPage<MStationType, MStationTypeDto>(parm);

        //    return response;
        //}




        public pdaController(IMntnWorkOrderService mntnWorkOrderService, IConfiguration configuration)
        {
            Configuration = configuration;
            _mntnWorkOrderService = mntnWorkOrderService;
        }
        /**
         * Pda 选择工单的时候查询工单
         * */
        [HttpGet("getListOrders")]
        [AllowAnonymous]
        public IActionResult GetWoBaseListPda([FromQuery] PWoBaseQueryDto parm)
        {
            parm.site = HttpContext.GetSite();
            var data = _mntnWorkOrderService.GetWoBaseListPda(parm);
            return SUCCESS(data, TIME_FORMAT_FULL);
        }


     


        [HttpPost("APP")]
        [AllowAnonymous]
        public IActionResult APP([FromForm(Name = "file")] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return SUCCESS("No file uploaded");
            }
            
            var corsUrls = Configuration.GetSection("DirectoryUpload").Get<string>();

            var path = Path.Combine(Directory.GetCurrentDirectory(), corsUrls, file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok(new { count = 1, path });
        }


        [HttpGet("GETAPP")]
        [AllowAnonymous]
        public IActionResult GETAPP()
        {
            var corsUrls = Configuration.GetSection("DirectoryUpload").Get<string>();
            var filePath = corsUrls + "SAJET.apk"; // 替换为您要下载的文件路径
            var stream = new FileStream(filePath, FileMode.Open);
            return File(stream, "application/vnd.android.package-archive", filePath);
        }


    }
}
