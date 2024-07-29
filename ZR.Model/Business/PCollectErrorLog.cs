using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZR.Model.Business
{
    /// <summary>
    /// ERROR收集
    /// </summary>
    [SugarTable("SAJET.P_COLLECTERROR_LOG")]
    public class PCollecterrorLog
    {
        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// StationName 
        /// </summary>
        private string stationname;

        [SugarColumn(ColumnName = "sTATION_NAME")]
        public string StationName { 
            get { return stationname.IsNullOrEmpty()?"N/A":stationname; } 
            set { stationname = value; }
        }

        /// <summary>
        /// Status 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// RequestParam 
        /// </summary>
        [SugarColumn(ColumnName = "rEQUEST_PARAM")]
        public string RequestParam { get; set; }

        /// <summary>
        /// ResponseResults 
        /// </summary>
        [SugarColumn(ColumnName = "rESPONSE_RESULTS")]
        public string ResponseResults { get; set; }
        /// <summary>
        /// ResErrcode 
        /// </summary>
        [SugarColumn(ColumnName = "rES_ERRCODE")]
        public string ResErrcode { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Line 
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// ClentIp 
        /// </summary>
        private string clentip;
        [SugarColumn(ColumnName = "cLENT_IP")]
        public string ClentIp {
            get { return clentip.IsNullOrEmpty() ? "N/A" : clentip; }
            set { clentip = value; }
        }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// ClientType 
        /// </summary>
        [SugarColumn(ColumnName = "cLIENT_TYPE")]
        public string ClientType { get; set; }
        /// <summary>
        /// EmpNo 
        /// </summary>
        private string empno;
        [SugarColumn(ColumnName = "eMP_NO")]
        public string EmpNo {
            get {return empno.IsNullOrEmpty()?"N/A":empno; }
            set {empno=value; } 
        }
        /// <summary>
        /// ProgramMent 
        /// </summary>
        private string programment;
        [SugarColumn(ColumnName = "pROGRAM_MENT")]
        public string ProgramMent {
            get {return programment.IsNullOrEmpty()?"N/A":programment; }
            set {programment= value; }
        }
        /// <summary>
        /// Errcode 
        /// </summary>
        public string Errcode { get; set; }
        /// <summary>
        /// Id 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// TicketId 
        /// </summary>
        [SugarColumn(ColumnName = "tICKET_ID")]
        public string TicketId { get; set; }
    }
}
