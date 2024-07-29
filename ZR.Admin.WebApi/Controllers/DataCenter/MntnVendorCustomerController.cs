using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using ZR.Model.Dto.ProdDto;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.DataCenter
{
    /// <summary>
    /// 基础信息->用户信息
    /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("MntnVendorMntnCustomer/VendorMntnCustomer/")]
    [ApiController]
    public class MntnVendorCustomerController : BaseController
    {



        readonly IMntnVendorCustomerService service;

        public MntnVendorCustomerController(IMntnVendorCustomerService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 用户信息-->供应商管理查询
        /// </summary>
        [HttpGet("VendorList")]
        public Task<IActionResult> VendorList(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.VendorList(enaBled, optionData, textData, pageNum, pageSize, HttpContext.GetSite())));
        }

        /// <summary>
        /// 用户信息-->供应商管理导出
        /// </summary>
        [HttpGet("VendorExport")]
        [Log(Title = "供应商管理导出", BusinessType = BusinessType.EXPORT)]
        //[ActionPermissionFilter(Permission = "system:user:export")]
        public IActionResult UserExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.VendorList(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());

            var result = ExportExcelMini(list.Result, "供应商", "供应商管理导出");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 用户信息-->供应商管理导入
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        [HttpPost("VendorImportData")]
        [Log(Title = "供应商管理导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = true)]
        public IActionResult VendorImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<ImesMvendor> imes = new();
            using (var stream = formFile.OpenReadStream())
            {
                imes = stream.Query<ImesMvendor>(startCell: "A1").ToList();
            }
            return SUCCESS(service.VendorImportData(imes, HttpContext.GetSite(), HttpContext.GetName()));
        }


        /// <summary>
        /// 用户信息-->供应商管理新增
        /// </summary>
        [HttpPost("VendorInsert")]
        public IActionResult VendorInsert(ImesMvendor imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.createEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return SUCCESS(service.VendorInsert(imes));
        }


        /// <summary>
        /// 用户信息-->供应商管理修改
        /// </summary>
        [HttpPut("VendorUpdate")]
        public IActionResult VendorUpdate(ImesMvendor imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return SUCCESS(service.VendorUpdate(imes));
        }


        /// <summary>
        /// 用户信息-->供应商管理删除
        /// </summary>
        [HttpDelete("VendorDelet")]
        public Task<IActionResult> VendorDelet(ImesMvendor imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.VendorDelet(imes)));
        }


        /// <summary>
        /// 用户信息-->供应商管理历史查询
        /// </summary>
        [HttpGet("VendorListHt")]
        public Task<IActionResult> VendorListHt([FromQuery] ImesMvendor imes)
        {
            return Task.FromResult<IActionResult>(Ok(service.VendorListHt(imes.id, HttpContext.GetSite())));
        }



        //-----------客户管理-------------------------------------------------------------------------------

        /// <summary>
        /// 用户信息-->客户管理查询//
        /// </summary>
        [HttpGet("CustomerList")]
        public Task<IActionResult> CustomerList(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.CustomerList(enaBled, optionData, textData, pageNum, pageSize, HttpContext.GetSite())));
        }

        /// <summary>
        /// 用户信息-->客户管理导出
        /// </summary>
        [HttpGet("CustomerExport")]
        [Log(Title = "客户管理导出", BusinessType = BusinessType.EXPORT)]
        //[ActionPermissionFilter(Permission = "system:user:export")]
        public IActionResult CustomerExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.CustomerList(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());

            var result = ExportExcelMini(list.Result, "客户管理", "客户管理导出");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 用户信息-->客户管理新增//
        /// </summary>
        [HttpPost("CustomerInsert")]
        public IActionResult CustomerInsert(ImesmMcustomer imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.createEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return SUCCESS(service.CustomerInsert(imes));
        }

        /// <summary>
        /// 用户信息-->客户管理修改
        /// </summary>
        [HttpPut("CustomerUpdate")]
        public IActionResult CustomerUpdate(ImesmMcustomer imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return SUCCESS(service.CustomerUpdate(imes));
        }



        //---------------------部门管理-------------------------------------------------------------

        /// <summary>
        /// 用户信息-->部门管理查询
        /// </summary>
        [HttpGet("MdeptList")]
        public Task<IActionResult> MdeptList(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.MdeptList(enaBled, optionData, textData, pageNum, pageSize, HttpContext.GetSite())));
        }


        [HttpGet("MdeptExport")]
        [Log(Title = "部门管理导出", BusinessType = BusinessType.EXPORT)]
        //[ActionPermissionFilter(Permission = "system:user:export")]
        public IActionResult MdeptExport(string? enaBled, string? optionData, string? textData, int pageSize)
        {
            var list = service.MdeptList(enaBled, optionData, textData, 1, pageSize, HttpContext.GetSite());

            var result = ExportExcelMini(list.Result, "部门", "部门管理导出");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 用户信息-->部门管理历史查询
        /// </summary>
        [HttpGet("MdeptListHt")]
        public Task<IActionResult> MdeptListHt([FromQuery] ImesMvendor imes)
        {
            return Task.FromResult<IActionResult>(Ok(service.MdeptListHt(imes.id, HttpContext.GetSite())));
        }

        /// <summary>
        /// 用户信息-->部门管理新增
        /// </summary>
        [HttpPost("MdeptInsert")]
        public IActionResult MdeptInsert(ImesMdept imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.createEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return SUCCESS(service.MdeptInsert(imes));
        }


        /// <summary>
        /// 用户信息-->部门管理修改
        /// </summary>
        [HttpPut("MdeptUpdate")]
        public IActionResult MdeptUpdate(ImesMdept imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return SUCCESS(service.MdeptUpdate(imes));
        }


        /// <summary>
        /// 用户信息-->部门管理删除
        /// </summary>
        [HttpDelete("MdeptDelet")]
        public Task<IActionResult> MdeptDelet(ImesMdept imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.MdeptDelet(imes)));
        }
        
        /// <summary>
        /// 用户信息-->部门管理厂区获取
        /// </summary>
        [HttpPut("MdeptListFactory")]
        public IActionResult MdeptListFactory( )
        {
            return SUCCESS(service.MdeptListFactory());
        }


    }
}
