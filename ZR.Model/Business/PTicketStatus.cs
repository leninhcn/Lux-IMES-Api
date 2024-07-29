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
    [SugarTable("SAJET.P_TICKET_TRAVEL")]
    public class PTicketTravel
    {
        /// <summary>
        /// Id 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// ProgramMent 
        /// </summary>
        [SugarColumn(ColumnName = "pROGRAM_MENT")]
        public string ProgramMent { get; set; }

        /// <summary>
        /// StationName 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_NAME")]
        public string StationName { get; set; }

        /// <summary>
        /// EmpNo 
        /// </summary>
        [SugarColumn(ColumnName = "eMP_NO")]
        public string EmpNo { get; set; }

        /// <summary>
        /// Status 
        /// </summary>
        public long? Status { get; set; }

        /// <summary>
        /// ErrorType 
        /// </summary>
        [SugarColumn(ColumnName = "eRROR_TYPE")]
        public long? ErrorType { get; set; }

        /// <summary>
        /// AssignEmp 
        /// </summary>
        [SugarColumn(ColumnName = "aSSIGN_EMP")]
        public string AssignEmp { get; set; }

        /// <summary>
        /// Mark 
        /// </summary>
        public string Mark { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

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
        /// ClentIp 
        /// </summary>
        [SugarColumn(ColumnName = "cLENT_IP")]
        public string ClentIp { get; set; }

    }
}