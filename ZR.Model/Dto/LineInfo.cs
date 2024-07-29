using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    [SugarTable("SAJET.m_line", "线体表")]
    public class  LineInfo
    {

        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public int id { get; set; }

        [SugarColumn(ColumnName = "LINE", IsPrimaryKey = true)] 
        public string line { get; set; }

        [SugarColumn(ColumnName = "LINE_CUSTOMER", IsPrimaryKey = true)] 
        public string lineCustomer { get; set; }

        [SugarColumn(ColumnName = "LINE_SAP", IsPrimaryKey = true)]
        public string lineSap { get; set; }

        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]
        public string site { get; set; }

        [SugarColumn(ColumnName = "LINE_DESC", IsPrimaryKey = true)]
        public string lineDesc { get; set; }


        [SugarColumn(ColumnName = "LINE_TYPE", IsPrimaryKey = true)]
        public string lineType { get; set; }

        [SugarColumn(ColumnName = "LINE_LEVEL", IsPrimaryKey = true)]
        public string lineLevel { get; set; }

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

        [SugarColumn(ColumnName = "WORK_CENTER", IsPrimaryKey = true)]
        public string workCenter { get; set; }
    }
}
