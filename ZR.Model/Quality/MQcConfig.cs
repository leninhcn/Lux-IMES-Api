using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Quality
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("SAJET.M_QC_CONFIG")]
    public class MQcConfig
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// 
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public long Id { get; set; }

        /// <summary>
        /// Remarks 
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// CheckRule 
        /// </summary>
        [SugarColumn(ColumnName = "cHECK_RULE")]
        public string CheckRule { get; set; }

        /// <summary>
        /// OnlineFlag 
        /// </summary>
        [SugarColumn(ColumnName = "oNLINE_FLAG")]
        public string OnlineFlag { get; set; }

        /// <summary>
        /// OnlineStationType 
        /// </summary>
        [SugarColumn(ColumnName = "oNLINE_STATION_TYPE")]
        public string OnlineStationType { get; set; }

        /// <summary>
        /// ReturnStationType 
        /// </summary>
        [SugarColumn(ColumnName = "rETURN_STATION_TYPE")]
        public string ReturnStationType { get; set; }

        /// <summary>
        /// QcRoute 
        /// </summary>
        [SugarColumn(ColumnName = "qC_ROUTE")]
        public string QcRoute { get; set; }

        /// <summary>
        /// QcStationType 
        /// </summary>
        [SugarColumn(ColumnName = "qC_STATION_TYPE")]
        public string QcStationType { get; set; }

        /// <summary>
        /// Target 
        /// </summary>
        public long? Target { get; set; }

        /// <summary>
        /// Qty 
        /// </summary>
        public long? Qty { get; set; }

        /// <summary>
        /// ReQc 
        /// </summary>
        [SugarColumn(ColumnName = "rE_QC")]
        public string ReQc { get; set; }

        /// <summary>
        /// QcLevel 
        /// </summary>
        [SugarColumn(ColumnName = "qC_LEVEL")]
        public long? QcLevel { get; set; }

        /// <summary>
        /// QcType 
        /// </summary>
        [SugarColumn(ColumnName = "qC_TYPE")]
        public string QcType { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME", IsOnlyIgnoreInsert = true)]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMPNO")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME", IsOnlyIgnoreInsert = true)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        /// 
        [SugarColumn(IsOnlyIgnoreInsert = true)]
        public string Enabled { get; set; }

        /// <summary>
        /// AutoHold 
        /// </summary>
        [SugarColumn(ColumnName = "aUTO_HOLD")]
        public string AutoHold { get; set; }

        /// <summary>
        /// AllPass 
        /// </summary>
        [SugarColumn(ColumnName = "aLL_PASS")]
        public string AllPass { get; set; }

        /// <summary>
        /// Model 
        /// </summary>
        public string Model { get; set; }

    }





}
