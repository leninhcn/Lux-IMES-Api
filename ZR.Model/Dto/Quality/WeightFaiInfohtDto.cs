using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Quality
{
    public class WeightFaiInfohtDto
    {
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public string Id { get; set; }

        [SugarColumn(ColumnName = "TYPE", IsPrimaryKey = true)]
        public string Type { get; set; }

        [SugarColumn(ColumnName = "MPN", IsPrimaryKey = true)]
        public string Mpn { get; set; }

        [SugarColumn(ColumnName = "IPN", IsPrimaryKey = true)]
        public string Ipn { get; set; }

        [SugarColumn(ColumnName = "FAI_VALUE", IsPrimaryKey = true)]
        public string FaiValue { get; set; }

        [SugarColumn(ColumnName = "UNIT", IsPrimaryKey = true)]
        public string Unit { get; set; }

        [SugarColumn(ColumnName = "UPDATE_EMPNO", IsPrimaryKey = true)]
        public string UpdateEmpno { get; set; }

        [SugarColumn(ColumnName = "UPDATE_TIME", IsPrimaryKey = true)]
        public string UpdateTime { get; set; }

        [SugarColumn(ColumnName = "CREATE_EMPNO", IsPrimaryKey = true)]
        public string CreateEmpno { get; set; }

        [SugarColumn(ColumnName = "CREATE_TIME", IsPrimaryKey = true)]

        public string CreateTime { get; set; }


        [SugarColumn(ColumnName = "PLANT", IsPrimaryKey = true)]
        public string Plant { get; set; }

        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]
        public string Site { get; set; }

        [SugarColumn(ColumnName = "ENABLED", IsPrimaryKey = true)]
        public string Eabled { get; set; }
    }
}
