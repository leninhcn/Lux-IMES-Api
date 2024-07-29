using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    [SugarTable("SAJET.M_STATION_TYPE", "站点工艺维护表")]
    public class StationTypeInfo
    {
       [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public int id { get; set; }

        [SugarColumn(ColumnName = "STATION_TYPE", IsPrimaryKey = true)]
        public string stationType { get; set; }

        [SugarColumn(ColumnName = "STATIONTYPE_CUSTOMER", IsPrimaryKey = true)]
        public string stationTypeCustomer { get; set; }

        [SugarColumn(ColumnName = "OPERATE_TYPE", IsPrimaryKey = true)]
        public string operateType { get; set; }

        [SugarColumn(ColumnName = "CLIENT_TYPE", IsPrimaryKey = true)]
        public string clientType { get; set; }

        [SugarColumn(ColumnName = "STATION_TYPE_SEQ", IsPrimaryKey = true)]
        public string stationTypeSeq { get; set; }

        [SugarColumn(ColumnName = "STAGE", IsPrimaryKey = true)]
        public string stage { get; set; }

        [SugarColumn(ColumnName = "STATION_TYPE_DESC", IsPrimaryKey = true)]
        public string  stationTypeDesc { get; set; }

        [SugarColumn(ColumnName = "CUSTOMER_STATION_DESC", IsPrimaryKey = true)]
        public string customerStationDesc { get; set; }

        [SugarColumn(ColumnName = "UPDATE_EMPNO", IsPrimaryKey = true)]
        public string updateEmpno { get; set; }

        [SugarColumn(ColumnName = "UPDATE_TIME", IsPrimaryKey = true)]
        public DateTime? updateTime { get; set; }

        [SugarColumn(ColumnName = "CREATE_EMPNO", IsPrimaryKey = true)]
        public string createEmpno { get; set; }

        [SugarColumn(ColumnName = "CREATE_TIME", IsPrimaryKey = true)]
        public DateTime? createTime { get; set; }

        [SugarColumn(ColumnName = "ENABLED", IsPrimaryKey = true)]
        public string  enabled { get; set; }

        [SugarColumn(ColumnName = "FPP", IsPrimaryKey = true)]
        public string fpp { get; set; }

        [SugarColumn(ColumnName = "CURRENT_CT", IsPrimaryKey = true)]
        public string  currentCT { get; set; }

        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]
        public string  site { get; set; }


    }
}
