using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    [SugarTable("SAJET.m_station", "机种表")]
    public class StationInfos
    {

        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public int id { get; set; }

        [SugarColumn(ColumnName = "STATION_ID", IsPrimaryKey = true)]
        public int stationId { get; set; }

        [SugarColumn(ColumnName = "STATION_NAME", IsPrimaryKey = true)]
        public string  stationName { get; set; }

        [SugarColumn(ColumnName = "STATION_TYPE", IsPrimaryKey = true)]
        public string stationType { get; set; }

        [SugarColumn(ColumnName = "Line", IsPrimaryKey = true)]
        public string line { get; set; }

        [SugarColumn(ColumnName = "Stage", IsPrimaryKey = true)]
        public string stage { get; set; }

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

        [SugarColumn(ColumnName = "JOB", IsPrimaryKey = true)]
        public string job { get; set; }

        [SugarColumn(ColumnName = "JOB_VERSION", IsPrimaryKey = true)]
        public string jobVersion { get; set; }

        [SugarColumn(ColumnName = "MAX_QTY", IsPrimaryKey = true)]
        public string maxQty { get; set; }

        [SugarColumn(ColumnName = "FAIL_QTY", IsPrimaryKey = true)]
        public string fallQty { get; set; }

        [SugarColumn(ColumnName = "PASS_QTY", IsPrimaryKey = true)]
        public string passQty { get; set; }

        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]
        public string site { get; set; }
    }
}
