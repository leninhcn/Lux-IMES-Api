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
    [SugarTable("SAJET.M_STATIONTYPE_PARTSPEC")]
    public class StationtypePartSpec
    {
        /// <summary>
        /// Id 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Model 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// KpSpec 
        /// </summary>
        [SugarColumn(ColumnName = "kP_SPEC")]
        public string KpSpec { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// KpSpecDesc 
        /// </summary>
        [SugarColumn(ColumnName = "kP_SPEC_DESC")]
        public string KpSpecDesc { get; set; }

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
        /// BobcatSpec 
        /// </summary>
        [SugarColumn(ColumnName = "bOBCAT_SPEC")]
        public string BobcatSpec { get; set; }

        /// <summary>
        /// StationDesc 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_DESC")]
        public string StationDesc { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

    }
}
