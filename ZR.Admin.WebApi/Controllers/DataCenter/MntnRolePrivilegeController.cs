using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Mvc;
using ZR.Model.Dto.ProdDto;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.DataCenter
{
    
    /// <summary>
     /// 基础信息->用户信息权限
     /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("MntnRolePrivilege/MntnRolePrivilegeController/")]
    [ApiController]
    public class MntnRolePrivilegeController : BaseController
    {

        readonly IMntnRolePrivilegeService service;

        public MntnRolePrivilegeController(IMntnRolePrivilegeService service)
        {
            this.service = service;
        }


        /// <summary>
        /// 用户信息-->权限管理查询(取消修改时间查询)
        /// </summary>
        [HttpGet("RolePrivilegeList")]
        public Task<IActionResult> RolePrivilegeList(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.RolePrivilegeList(enaBled, optionData, textData, pageNum, pageSize, HttpContext.GetSite())));
        }

        /// <summary>
        /// 用户信息-->权限管理新增
        /// </summary>
        [HttpPost("RolePrivilegeInsert")]
        public IActionResult RolePrivilegeInsert(ImesMrole imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.createEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return SUCCESS(service.RolePrivilegeInsert(imes));
        }


        /// <summary>
        /// 用户信息-->权限管理修改
        /// </summary>
        [HttpPut("RolePrivilegeUpdate")]
        public IActionResult RolePrivilegeUpdate(ImesMrole imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return SUCCESS(service.RolePrivilegeUpdate(imes));
        }

        /// <summary>
        /// 用户信息-->绑定权限查询(主页面id，textData输入框的数据)/status=0无权限-1有
        /// </summary>
        [HttpGet("RolePrivilegeBindingList")]
        public Task<IActionResult> RolePrivilegeBindingList(int id,  string? textData)
        {
            return Task.FromResult<IActionResult>(Ok(service.RolePrivilegeBindingList(id, textData, HttpContext.GetSite())));
        }

        /// <summary>
        /// 用户信息-->绑定权限(主页面的id 主页面roleName ,authoritys=当前页面的权限范围,idStr当前页面的集合id)
        /// </summary>
        [HttpPost("RolePrivilegeBindingPermission")]
        public Task<IActionResult> RolePrivilegeBindingPermission(ImesMrolePrivilege imes )
        {
            return Task.FromResult<IActionResult>(Ok(service.RolePrivilegeBindingPermission(imes.id, imes.roleName, imes.authoritys, HttpContext.GetName(),imes.idStr)));
        }

        /// <summary>
        /// 用户信息-->绑定权限-历史查询-主页面的id,function
        /// </summary>
        [HttpPost("RolePrivilegeBindingPermissionHt")]
        public Task<IActionResult> RolePrivilegeBindingPermissionHt(ImesMrolePrivilege imes)
        {
            return Task.FromResult<IActionResult>(Ok(service.RolePrivilegeBindingPermissionHt(imes.id, imes.fun)));
        }

        /// <summary>
        /// 用户信息-->绑定报表查询-主页面的id
        /// </summary>
        [HttpGet("RolePrivilegeBindingReportPermission")]
        public IActionResult RolePrivilegeBindingReportPermission(int id)
        {
            return SUCCESS(service.RolePrivilegeBindingReportPermission(id));
        }

        /// <summary>
        /// 用户信息-->绑定报表--新增(//roleID-角色id
        ///roleName-角色名称,idStr树状用户选择的id 用英文逗号拼接一下)
        /// </summary>
        [HttpPost("RolePrivilegeBindInRepPer")]
        public IActionResult RolePrivilegeBindInRepPer(ImesMroleReport imes)
        {
            return SUCCESS(service.RolePrivilegeBindingInsertReportPermission(imes.roleId, imes.roleName, HttpContext.GetName(), imes.idStr));
        }

        //---------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// 用户管理-->分页查询
        /// </summary>
        [HttpGet("SAJETMemplist")]
        public Task<IActionResult> ImesMemplist(string? enaBled, string? optionData, string? textData, int pageNum, int pageSize)
        {
            return Task.FromResult<IActionResult>(Ok(service.ImesMemplist(enaBled, optionData, textData, pageNum, pageSize, HttpContext.GetSite())));
        }

        /// <summary>
        /// 用户管理-->新增
        /// </summary>
        [HttpPost("SAJETMempInsert")]
        public Task<IActionResult> ImesMempInsert(ImesMemp imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            
            return Task.FromResult<IActionResult>(Ok(service.ImesMempInsert(imes)));
        }

        /// <summary>
        /// 用户管理-->修改 status=1时，为重置密码
        /// </summary>
        [HttpPut("SAJETMempUpdate")]
        public Task<IActionResult> ImesMempUpdate(ImesMemp imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();

            return Task.FromResult<IActionResult>(Ok(service.ImesMempUpdate(imes)));
        }

        /// <summary>
        /// 用户管理-->删除
        /// </summary>
        [HttpDelete("SAJETMempDelet")]
        public Task<IActionResult> ImesMempDelet(ImesMemp imes)
        {
            imes.updateEmpno = HttpContext.GetName();
            imes.site = HttpContext.GetSite();
            return Task.FromResult<IActionResult>(Ok(service.ImesMempDelet(imes)));
        }

        /// <summary>
        /// 用户管理-->历史记录查询 传工号
        /// </summary>
        [HttpGet("SAJETMemplistHt")]
        public Task<IActionResult> ImesMemplistHt(string empNo)
        {
            return Task.FromResult<IActionResult>(Ok(service.ImesMemplistHt(empNo, HttpContext.GetSite())));
        }

        /// <summary>
        /// 用户管理-->厂区数据查询
        /// </summary>
        [HttpGet("SAJETMemplistFactory")]
        public Task<IActionResult> ImesMemplistFactory()
        {
            return Task.FromResult<IActionResult>(Ok(service.ImesMemplistFactory()));
        }

        /// <summary>
        /// 用户管理-->部门数据查询(班别的下拉框写死)
        /// </summary>
        [HttpGet("SAJETMemplistBranch")]
        public Task<IActionResult> ImesMemplistBranch(string site)
        {
            return Task.FromResult<IActionResult>(Ok(service.ImesMemplistBranch(site)));
        }

        /// <summary>
        /// 用户管理-->绑定角色(主页面id，textData输入框的数据)/status=0无权限-1有
        /// </summary>
        [HttpGet("SAJETMemplistRole")]
        public Task<IActionResult> ImesMemplistRole(int id, string? textData)
        {
            return Task.FromResult<IActionResult>(Ok(service.ImesMemplistRole(id, textData, HttpContext.GetSite())));
        }

        /// <summary>
        /// 用户信息-->绑定角色权(empId员工ID)--(empNo员工工号)-(authoritys当前页面的id(多个用逗号拼接))
        /// </summary>
        [HttpPost("SAJETMemplistRoleInsert")]
        public Task<IActionResult> ImesMemplistRoleInsert(ImesMemp imes)
        {
            
            return Task.FromResult<IActionResult>(Ok(service.ImesMemplistRoleInsert((int)imes.id, imes.empNo, imes.authoritys, HttpContext.GetName())));
        }

        /// <summary>
        /// 用户信息-->权限复制(empId--当前员工ID)--(开通的empNo员工工号)
        /// </summary>
        [HttpPost("SAJETMemplistCopy")]
        public Task<IActionResult> ImesMemplistCopy(ImesMemp imes)
        {
            return Task.FromResult<IActionResult>(Ok(service.ImesMemplistCopy((int)imes.id, imes.empNo, HttpContext.GetName())));
        }


    }
}
