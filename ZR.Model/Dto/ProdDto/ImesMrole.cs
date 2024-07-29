using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_ROLE", "权限表")]
    public class ImesMrole
    {
        [SugarColumn(ColumnName = "ID")]
        public int id { get; set; }

        [SugarColumn(ColumnName = "ROLE_NAME")]
        public string roleName { get; set; }

        [SugarColumn(ColumnName = "ROLE_DESC")]
        public string roleDesc { get; set; }

        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string updateEmpno { get; set; }

        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? updateTime { get; set; }

        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }


        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string createEmpno { get; set; }


        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }


        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }


        [@SugarColumn(IsIgnore = true)]
        public int status
        {
            get; set;
        }
    }
}
