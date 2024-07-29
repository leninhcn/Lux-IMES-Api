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
    [SugarTable("SAJET.P_HOLD_SN")]
    public class PHoldSn
    {
        /// <summary>
        /// Part 
        /// </summary>
        /// 
        [SugarColumn(ColumnName = "ID",IsPrimaryKey =true)]
        public int Id { get; set; }


        [SugarColumn(ColumnName = "PART")]
        public string Pn { get; set; }

        /// <summary>
        /// PanelNo 
        /// </summary>
        [SugarColumn(ColumnName = "PANEL_NO")]
        public string Panel { get; set; }

        /// <summary>
        /// Sn 
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// Stage 
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// HoldReason 
        /// </summary>
        [SugarColumn(ColumnName = "hOLD_REASON")]
        public string HoldReason { get; set; }

        /// <summary>
        /// UnholdReson 
        /// </summary>
        [SugarColumn(ColumnName = "uNHOLD_RESON")]
        public string UnholdReason { get; set; }

        /// <summary>
        /// HoldEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "hOLD_EMPNO")]
        public string HoldEmpno { get; set; }

        /// <summary>
        /// HoldTime 
        /// </summary>
        [SugarColumn(ColumnName = "hOLD_TIME", IsIgnore = true)]
        public DateTime? HoldTime { get; set; }

        /// <summary>
        /// UnholdEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uNHOLD_EMPNO")]
        public string UnholdEmpno { get; set; }

        /// <summary>
        /// UnholdTime 
        /// </summary>
        [SugarColumn(ColumnName = "uNHOLD_TIME",IsOnlyIgnoreInsert =true)]
        public DateTime? UnholdTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMPNO")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME", IsOnlyIgnoreInsert =true)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>

        [SugarColumn(IsOnlyIgnoreInsert =true)]
        public string Enabled { get; set; }

        /// <summary>
        /// Islms 
        /// </summary>
        public string Islms { get; set; }

        /// <summary>
        /// StationName 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_NAME")]
        public string StationName { get; set; }

        /// <summary>
        /// MainSn 
        /// </summary>
        [SugarColumn(ColumnName = "mAIN_SN")]
        public string MainSn { get; set; }

        /// <summary>
        /// Wo 
        /// </summary>
        public string Wo { get; set; }

        [SugarColumn(ColumnName = "HOLD_METHOD")]
        public string HoldMethod { get; set; }

    }



}
