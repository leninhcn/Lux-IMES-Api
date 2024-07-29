using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{

    [SugarTable("SAJET.M_STATIONTYPE_PARTSPEC")]
    public class ImesMstationTypePartSpec
    {

        /// <summary>
        /// ID 
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public int id { get; set; }

        /// <summary>
        /// MODEL 
        /// </summary>
        [SugarColumn(ColumnName = "MODEL")]
        public string model { get; set; }

        /// <summary>
        /// STATION_TYPE 
        /// </summary>
        [SugarColumn(ColumnName = "STATION_TYPE")]
        public string stationType { get; set; }

        /// <summary>
        /// KP_SPEC 
        /// </summary>
        [SugarColumn(ColumnName = "KP_SPEC")]
        public string kpSpec { get; set; }

        /// <summary>
        /// UPDATE_EMPNO 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string updateEmpno { get; set; }

        /// <summary>
        /// UPDATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? updateTime { get; set; }

        /// <summary>
        /// CREATE_EMPNO 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string createEmpno { get; set; }

        /// <summary>
        /// CREATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }

        /// <summary>
        /// ENABLED 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }

        /// <summary>
        /// BOBCAT_SPEC 
        /// </summary>
        [SugarColumn(ColumnName = "BOBCAT_SPEC")]
        public string bobcatSpec { get; set; }

        /// <summary>
        /// STATION_DESC 
        /// </summary>
        [SugarColumn(ColumnName = "STATION_DESC")]
        public string stationDesc { get; set; }

        /// <summary>
        /// KP_SPEC_DESC 
        /// </summary>
        [SugarColumn(ColumnName = "KP_SPEC_DESC")]
        public string kpSpecDesc { get; set; }

        /// <summary>
        /// SITE 
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }


    }
}
