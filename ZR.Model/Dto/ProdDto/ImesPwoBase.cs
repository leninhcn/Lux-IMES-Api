using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.P_WO_BASE")]
    public class ImesPwoBase
    {
        [SugarColumn(ColumnName = "WORK_ORDER")]
        public string workOrder { get; set; }
        [SugarColumn(ColumnName = "WO_TYPE")]
        public string woType { get; set; }
        [SugarColumn(ColumnName = "MODEL")]
        public string model { get; set; }
        [SugarColumn(ColumnName = "IPN")]
        public string ipn { get; set; }
        [SugarColumn(ColumnName = "TARGET_QTY")]
        public int targetQty { get; set; }
        [SugarColumn(ColumnName = "WO_CREATE_DATE")]
        public DateTime woCreateDate { get; set; }
        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }
    }
}
