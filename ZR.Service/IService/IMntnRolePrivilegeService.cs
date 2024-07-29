using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.ProdDto;
using ZR.Model;

namespace ZR.Service.IService
{
    public interface IMntnRolePrivilegeService
    {
        PagedInfo<ImesMrole> RolePrivilegeList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site);

        string RolePrivilegeInsert(ImesMrole imes);

        string RolePrivilegeUpdate(ImesMrole imes);

        List<ImesSprogramFunName> RolePrivilegeBindingList(int enaBled,  string textData,  string site);

        Object RolePrivilegeBindingPermission(int id, string roleName, string authoritys, string updateUserNo, string idStr);

        object RolePrivilegeBindingPermissionHt(int id, string function);

        (List<ImesMroleReport>, List<ImesMdiyreport>) RolePrivilegeBindingReportPermission(int id);

        object RolePrivilegeBindingInsertReportPermission(int roleId, string roleName, string op, string idStr);
        //---------------------------------------------------------------------------------------------------------
        PagedInfo<ImesMemp> ImesMemplist(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site);

        int ImesMempInsert(ImesMemp imes);

        int ImesMempUpdate(ImesMemp imes);

        int ImesMempDelet(ImesMemp imes);

        object ImesMemplistHt(string empNo, string site);

        object ImesMemplistFactory();

        object ImesMemplistBranch(string site);

        List<ImesMrole> ImesMemplistRole(int id, string textData, string site);

        Object ImesMemplistRoleInsert(int empId, string empNo, string authoritys, string updateNo);

        object ImesMemplistCopy(int empId, string empNo,string op);


    }
}
