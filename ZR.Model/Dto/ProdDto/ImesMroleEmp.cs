using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_ROLE_EMP")]
    public class ImesMroleEmp
    {
        [SugarColumn(ColumnName = "ID")]
        public int id { get; set; }

        [SugarColumn(ColumnName = "ROLE_ID")]
        public int roleId { get; set; }

        [SugarColumn(ColumnName = "EMP_ID")]
        public int empId { get; set; }

        [SugarColumn(ColumnName = "ROLE_NAME")]
        public string roleName { get; set; }

        [SugarColumn(ColumnName = "EMP_NO")]
        public string empNo { get; set; }

        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string updateEmpno { get; set; }

        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string createEmpno { get; set; }

        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }

        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }

        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? updateTime { get; set; }//时间

    }
}
