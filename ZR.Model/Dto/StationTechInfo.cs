using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    [SugarTable("SAJET.M_STATIONTECH", "站点工艺维护表")]

    public class StationTechInfo
    {
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public int  id { get; set; }

        [SugarColumn(ColumnName = "STATION_TYPE", IsPrimaryKey = true)]
        public string  stationType{ get; set; }

        [SugarColumn(ColumnName = "PROCESS_TYPE", IsPrimaryKey = true)]
        public string  processType { get; set; }

        [SugarColumn(ColumnName = "VALUE", IsPrimaryKey = true)]
        public string value { get; set; }

        [SugarColumn(ColumnName = "UPDATE_EMPNO", IsPrimaryKey = true)]
        public string  updateEmpno { get; set; }

        [SugarColumn(ColumnName = "UPDATE_TIME", IsPrimaryKey = true)]
        public DateTime? updateTime { get; set; }

        [SugarColumn(ColumnName = "CREATE_EMPNO", IsPrimaryKey = true)]
        public string createEmpno { get; set; }
        
        [SugarColumn(ColumnName = "CREATE_TIME", IsPrimaryKey = true)]
        public DateTime? createTime { get; set; }
        
        [SugarColumn(ColumnName = "ENABLED", IsPrimaryKey = true)]
        public string enabled { get; set; }

        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]
        public string  site { get; set; }

    }
}
