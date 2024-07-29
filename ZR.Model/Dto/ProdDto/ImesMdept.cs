using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_DEPT")]
    public class ImesMdept
    {

        [SugarColumn(ColumnName = "ID")]
        public int id { get; set; }

        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }

        [SugarColumn(ColumnName = "DEPT_CODE")]
        public string deptCode { get; set; }

        [SugarColumn(ColumnName = "DEPT_NAME")]
        public string deptName { get; set; }

        [SugarColumn(ColumnName = "DEPT_DESC")]
        public string deptDesc { get; set; }

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
