using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_STANDARDTIME", "标准工时维护查询表")]
    public class imesMstandardtime
    {

        /// <summary>
        /// ID 
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public int id { get; set; }

        /// <summary>
        /// CREATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }

        /// <summary>
        /// UPDATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? updateTime { get; set; }

        /// <summary>
        /// IPN 
        /// </summary>
        [SugarColumn(ColumnName = "IPN")]
        public string ipn { get; set; }

        /// <summary>
        /// DESC1--- 
        /// </summary>
        [SugarColumn(ColumnName = "DESC1")]
        public string desc1 { get; set; }

        /// <summary>
        /// MODEL 
        /// </summary>
        [SugarColumn(ColumnName = "MODEL")]
        public string model { get; set; }

        /// <summary>
        /// DUTYOFMODEL 
        /// </summary>
        [SugarColumn(ColumnName = "DUTYOFMODEL")]
        public string dutyofmodel { get; set; }

        /// <summary>
        /// REMARK 
        /// </summary>
        [SugarColumn(ColumnName = "REMARK")]
        public string remark { get; set; }

        /// <summary>
        /// CT 
        /// </summary>
        [SugarColumn(ColumnName = "CT")]
        public string ct { get; set; }

        /// <summary>
        /// HUMAN 
        /// </summary>
        [SugarColumn(ColumnName = "HUMAN")]
        public string human { get; set; }

        /// <summary>
        /// WORKHOURS 
        /// </summary>
        [SugarColumn(ColumnName = "WORKHOURS")]
        public string workhours { get; set; }

        /// <summary>
        /// UPH 
        /// </summary>
        [SugarColumn(ColumnName = "UPH")]
        public string uph { get; set; }

        /// <summary>
        /// SIDE 
        /// </summary>
        [SugarColumn(ColumnName = "SIDE")]
        public string side { get; set; }

        /// <summary>
        /// LINE 
        /// </summary>
        [SugarColumn(ColumnName = "LINE")]
        public string line { get; set; }

        /// <summary>
        /// UPDATE_EMPNO 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string updateEmpno { get; set; }

        /// <summary>
        /// CREATE_EMPNO 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string createEmpno { get; set; }

        /// <summary>
        /// ENABLED 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }

        /// <summary>
        /// STATIONTYPE 
        /// </summary>
        [SugarColumn(ColumnName = "STATIONTYPE")]
        public string stationtype { get; set; }

        /// <summary>
        /// OPTION1 
        /// </summary>
        [SugarColumn(ColumnName = "OPTION1")]
        public string option1 { get; set; }

        /// <summary>
        /// OPTION2 
        /// </summary>
        [SugarColumn(ColumnName = "OPTION2")]
        public string option2 { get; set; }

        /// <summary>
        /// SITE 
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }
    }
}
