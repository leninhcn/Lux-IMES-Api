using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    [SugarTable("SAJET.M_LINE_TYPE", "线体类型维护表")]

    public class LineTypeInfo
    {
       
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public int id { get; set; }
      
        [SugarColumn(ColumnName = "LINE_TYPE", IsPrimaryKey = true)]
        public string  lineType { get; set; }

        [SugarColumn(ColumnName = "LINE_TYPE_DESC", IsPrimaryKey = true)]
        public string  lineTypeDesc { get; set; }

        [SugarColumn(ColumnName = "LINE_ON", IsPrimaryKey = true)]
        public string  lineOn { get; set; }

        [SugarColumn(ColumnName = "UPDATE_EMPNO", IsPrimaryKey = true)]
        public string updateEmpno { get; set; }

        [SugarColumn(ColumnName = "UPDATE_TIME", IsPrimaryKey = true)]
        public DateTime? updateTime { get; set; }

        [SugarColumn(ColumnName = "CREATE_EMPNO", IsPrimaryKey = true)]
        public string createEmpno { get; set; }

        [SugarColumn(ColumnName = "CREATE_TIME", IsPrimaryKey = true)]
        public DateTime? createTime { get; set; }

        [SugarColumn(ColumnName = "ENABLED", IsPrimaryKey = true)]
        public string enabled { get; set; }

        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]
        public string site { get; set; }

    }
}
