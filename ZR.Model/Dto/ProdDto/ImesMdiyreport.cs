using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_DIYReport")]
    public class ImesMdiyreport
    {
        [SugarColumn(ColumnName = "TKEY")]
        public string tkey { get; set; }

        [SugarColumn(ColumnName = "TPARENT")]
        public string tparent { get; set; }

        [SugarColumn(ColumnName = "TTEXT")]
        public string ttext { get; set; }

        [SugarColumn(ColumnName = "ORDERBY")]
        public int orderby { get; set; }

        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }

        /// <summary>
        /// 该字段实体类不存在 
        /// </summary>
        [@SugarColumn(IsIgnore = true)]
        public int status
        {
            get; set;
        }

    }
}
