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
    [SugarTable("SAJET.P_SN_BLACK_LIST")]
    public class PSnBlackList
    {

        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public long Id { get; set; }
        /// <summary>
        /// Kpsn 
        /// </summary>
        public string Kpsn { get; set; }

        /// <summary>
        /// VendorCode 
        /// </summary>
        [SugarColumn(ColumnName = "vENDOR_CODE")]
        public string VendorCode { get; set; }

        /// <summary>
        /// DefectCode 
        /// </summary>
        [SugarColumn(ColumnName = "dEFECT_CODE")]
        public string DefectCode { get; set; }

        /// <summary>
        /// CreateUser 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_USER")]
        public string CreateUser { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        /// 

        [SugarColumn(ColumnName = "cREATE_TIME", IsOnlyIgnoreInsert = true)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// StationId 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_ID")]
        public string StationId { get; set; }

        /// <summary>
        /// Message 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Line 
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// UpdateUser 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_USER")]
        public string UpdateUser { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        /// 
        [SugarColumn(ColumnName = "uPDATE_TIME", IsOnlyIgnoreInsert = true)]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        /// 
        [SugarColumn(IsOnlyIgnoreInsert = true)]
        public string Enabled { get; set; }

        /// <summary>
        /// BlackType 
        /// </summary>
        [SugarColumn(ColumnName = "bLACK_TYPE")]
        public string BlackType { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// TestStationName 
        /// </summary>
        [SugarColumn(ColumnName = "tEST_STATION_NAME")]
        public string TestStationName { get; set; }

    }



}
