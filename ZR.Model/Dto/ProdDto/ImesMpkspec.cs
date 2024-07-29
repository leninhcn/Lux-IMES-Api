using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_PKSPEC")]
    public class ImesMpkspec
    {


        [SugarColumn(ColumnName = "ID")]
        public int id { get; set; }

        [SugarColumn(ColumnName = "PKSPEC_NAME")]
        public string pkspecName { get; set; }

        [SugarColumn(ColumnName = "PALLET_QTY")]
        public int palletQty { get; set; }

        [SugarColumn(ColumnName = "CARTON_QTY")]
        public int cartonQty { get; set; }

        [SugarColumn(ColumnName = "BOX_QTY")]
        public int boxQty { get; set; }

        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string updateEmpno { get; set; }

        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string createEmpno { get; set; }

        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }
        
        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }

        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }

        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? updateTime { get; set; }//时间

    }
}
