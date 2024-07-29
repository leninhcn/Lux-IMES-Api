using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("ISMT.M_MATERIAL_INFO")]
    public class IsmtMMaterialnfo
    {
        [SugarColumn(ColumnName = "MATERIAL_PN")]
        public string materialPn { get; set; }
        [SugarColumn(ColumnName = "MATERIAL_CNAME")]
        public string materialCname { get; set; }
        [SugarColumn(ColumnName = "VENDOR")]
        public string vendor { get; set; }
        [SugarColumn(ColumnName = "MODEL_DESC")]
        public string modelDesc { get; set; }
        [SugarColumn(ColumnName = "MATERIAL_TYPE")]
        public string materialType { get; set; }
        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }
        //[SugarColumn(ColumnName = "SITE")]
        //public string site { get; set; }
    }
}
