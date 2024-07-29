using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class RouteDetailVo
    {

        [SugarColumn(ColumnName = "ROUTE_NAME", IsPrimaryKey = true)]
          public string ? routeName { get; set; }

        [SugarColumn(ColumnName = "STATION_TYPE", IsPrimaryKey = true)]
        public string? stationType { get; set; }

        [SugarColumn(ColumnName = "NEXT_STATION_TYPE", IsPrimaryKey = true)]

        public string ?  nextStationType { get; set; }

        [SugarColumn(ColumnName = "RESULT", IsPrimaryKey = true)]

        public int? result { get; set; }

        [SugarColumn(ColumnName = "SEQ", IsPrimaryKey = true)]
        public int? seq { get; set; }


        [SugarColumn(ColumnName = "PD_CODE", IsPrimaryKey = true)]

        public string? pdCode { get; set; }

        [SugarColumn(ColumnName = "NECESSARY", IsPrimaryKey = true)]
        public string? necessary { get; set; }

        [SugarColumn(ColumnName = "STEP", IsPrimaryKey = true)]

        public int? step { get; set; }

        [SugarColumn(ColumnName = "DEFAULT_INSTATIONTYPE", IsPrimaryKey = true)]

        public string? defaultInStationType { get; set; }

        [SugarColumn(ColumnName = "UPDATE_EMPNO", IsPrimaryKey = true)]

        public string? updateEmpno { get; set; }

        [SugarColumn(ColumnName = "UPDATE_TIME", IsPrimaryKey = true)]

        public DateTime? updateTime { get; set; }

        [SugarColumn(ColumnName = "CREATE_EMPNO", IsPrimaryKey = true)]

        public string? createEmpno { get; set; }

        [SugarColumn(ColumnName = "CREATE_TIME", IsPrimaryKey = true)]

        public DateTime? createTime { get; set; }

        [SugarColumn(ColumnName = "ENABLED", IsPrimaryKey = true)]

        public string? enaBled { get; set; }

        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]

        public string? site { get; set; }
    }
}
