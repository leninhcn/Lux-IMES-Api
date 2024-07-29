using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    [SugarTable("SAJET.M_OPERATE_TYPE", "操作类型维护表")]
    public class OperateTypeInfo
    {
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public int id { get; set; }

        [SugarColumn(ColumnName = "SCAN_TYPE", IsPrimaryKey = true)]
        public string scanType { get; set; }


        [SugarColumn(ColumnName = "UPDATE_EMPNO", IsPrimaryKey = true)]
        public string updateEmpno { get; set; }


        [SugarColumn(ColumnName = "UPDATE_TIME", IsPrimaryKey = true)]
        public DateTime? updateTime { get; set; }

        [SugarColumn(ColumnName = "CREATE_EMPNO", IsPrimaryKey = true)]
        public string createEmpno { get; set; }

        [SugarColumn(ColumnName = "CREATE_TIME", IsPrimaryKey = true)]
        public DateTime? createTime { get; set; }

        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]
        public string site { get; set; }

    }
}
