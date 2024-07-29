using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_CallPart")]
    public class ImesMCallPart
    {
        [SugarColumn(ColumnName = "ID")]
        public string Id { get; set; }
        [SugarColumn(ColumnName = "PART_TYPE")]
        public string partType { get; set; }
        [SugarColumn(ColumnName = "IPN")]
        public string ipn { get; set; }
        [SugarColumn(ColumnName = "QTY")]
        public int qty { get; set; }
        [SugarColumn(ColumnName = "LINE")]
        public string line { get; set; }
        [SugarColumn(ColumnName = "SHIFT_TYPE")]
        public string shiftType { get; set; }
        [SugarColumn(ColumnName = "OPTION1")]
        public string option1 { get; set; }
        [SugarColumn(ColumnName = "OPTION2")]
        public string option2 { get; set; }
        [SugarColumn(ColumnName = "OPTION3")]
        public string option3 { get; set; }
        [SugarColumn(ColumnName = "OPTION4")]
        public string option4 { get; set; }
        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }
        [SugarColumn(ColumnName = "CREATE_EMP")]
        public string createEmp { get; set; }
        [SugarColumn(ColumnName = "UPDATE_EMP")]
        public string updateEmp { get; set; }
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? updateTime { get; set;}
        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }
    }
}
