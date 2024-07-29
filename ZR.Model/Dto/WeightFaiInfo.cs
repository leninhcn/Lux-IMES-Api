using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    [SugarTable("SAJET.M_WEIGHT_FAI", "重量维护表")]

    public class WeightFaiInfo

    {
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public int id { get; set; }

        [SugarColumn(ColumnName = "TYPE", IsPrimaryKey = true)]
        public string type { get; set; }

        [SugarColumn(ColumnName = "MPN", IsPrimaryKey = true)]
        public string mpn { get; set; }

        [SugarColumn(ColumnName = "IPN", IsPrimaryKey = true)]
        public string ipn { get; set; }

        [SugarColumn(ColumnName = "FAI_VALUE", IsPrimaryKey = true)]
        public string  faiValue { get; set; }

        [SugarColumn(ColumnName = "UNIT", IsPrimaryKey = true)]
        public string  unit { get; set; }

        [SugarColumn(ColumnName = "UPDATE_EMPNO", IsPrimaryKey = true)]
        public string   updateEmpno { get; set; }

        [SugarColumn(ColumnName = "UPDATE_TIME", IsPrimaryKey = true)]
        public DateTime? updateTime { get; set; }

        [SugarColumn(ColumnName = "CREATE_EMPNO", IsPrimaryKey = true)]
        public string createEmpno { get; set; }

       [SugarColumn(ColumnName = "CREATE_TIME", IsPrimaryKey = true)]

        public DateTime? createTime { get; set; }


        [SugarColumn(ColumnName = "PLANT", IsPrimaryKey = true)]
        public string plant { get; set; }

        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]
        public string site { get; set; }

        [SugarColumn(ColumnName = "ENABLED", IsPrimaryKey = true)]
        public string  enabled { get; set; }

    }
}
