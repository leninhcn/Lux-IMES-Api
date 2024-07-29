using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZR.Model.Business
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("SAJET.M_STATION_ACTION")]
    public class MStationAction
    {
        /// <summary>
        /// Line 
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// PrintQty 
        /// </summary>
        [SugarColumn(ColumnName = "pRINT_QTY")]
        public string PrintQty { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// StationName 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_NAME")]
        public string StationName { get; set; }

        /// <summary>
        /// GroupId 
        /// </summary>
        [SugarColumn(ColumnName = "gROUP_ID")]
        public long? GroupId { get; set; }

        /// <summary>
        /// ShowBom 
        /// </summary>
        [SugarColumn(ColumnName = "sHOW_BOM")]
        public string ShowBom { get; set; }

        /// <summary>
        /// CheckLine 
        /// </summary>
        [SugarColumn(ColumnName = "cHECK_LINE")]
        public string CheckLine { get; set; }

        /// <summary>
        /// PrintFlag 
        /// </summary>
        [SugarColumn(ColumnName = "pRINT_FLAG")]
        public string PrintFlag { get; set; }

        /// <summary>
        /// AutoReadsn 
        /// </summary>
        [SugarColumn(ColumnName = "aUTO_READSN")]
        public string AutoReadsn { get; set; }

        /// <summary>
        /// AutoReadPath 
        /// </summary>
        [SugarColumn(ColumnName = "aUTO_READ_PATH")]
        public string AutoReadPath { get; set; }

        /// <summary>
        /// CheckFont 
        /// </summary>
        [SugarColumn(ColumnName = "cHECK_FONT")]
        public string CheckFont { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMPNO")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// Stage 
        /// </summary>
        public string Stage { get; set; }

    }
}