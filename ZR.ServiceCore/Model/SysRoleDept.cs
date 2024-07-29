using SqlSugar;

namespace ZR.Model.System
{
    [SugarTable("SAJET.m_zr_role_dept", "角色部门")]
    [Tenant(0)]
    public class SysRoleDept
    {
        [SugarColumn(ExtendedAttribute = ProteryConstant.NOTNULL, IsPrimaryKey = true)]
        public long RoleId { get; set; }

        [SugarColumn(ExtendedAttribute = ProteryConstant.NOTNULL, IsPrimaryKey = true)]
        public long DeptId { get; set; }
    }
}
