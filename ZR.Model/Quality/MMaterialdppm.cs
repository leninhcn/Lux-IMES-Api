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
    [SugarTable("SAJET.M_MATERIALDPPM")]
    public class MMaterialdppm
    {



        /// <summary>
        /// Uploaddate 
        /// </summary>
        public string Uploaddate { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMPNO")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// SerialNumber 
        /// </summary>
        [SugarColumn(ColumnName = "sERIAL_NUMBER")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Ngdescription 
        /// </summary>
        public string Ngdescription { get; set; }

        /// <summary>
        /// Ngrate 
        /// </summary>
        public string Ngrate { get; set; }

        /// <summary>
        /// Dppm 
        /// </summary>
        public string Dppm { get; set; }

        /// <summary>
        /// Receivedate 
        /// </summary>
        public string Receivedate { get; set; }

        /// <summary>
        /// Rmanumber 
        /// </summary>
        public string Rmanumber { get; set; }

        /// <summary>
        /// Status 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Report 
        /// </summary>
        public string Report { get; set; }

        /// <summary>
        /// Remark 
        /// </summary>
        public string Remark { get; set; }

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
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public long Id { get; set; }

        /// <summary>
        /// Month 
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// Week 
        /// </summary>
        public string Week { get; set; }

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
        /// OPTION1 
        /// </summary>
        public string OPTION1 { get; set; }

        /// <summary>
        /// OPTION2 
        /// </summary>
        public string OPTION2 { get; set; }

        /// <summary>
        /// OPTION3 
        /// </summary>
        public string OPTION3 { get; set; }

        /// <summary>
        /// OPTION4 
        /// </summary>
        public string OPTION4 { get; set; }

        /// <summary>
        /// OPTION5 
        /// </summary>
        public string OPTION5 { get; set; }

        /// <summary>
        /// ReportFile 
        /// </summary>
        [SugarColumn(ColumnName = "rEPORT_FILE")]
        public byte[] ReportFile { get; set; }

        /// <summary>
        /// Model 
        /// </summary>
        public string Model { get; set; }

    }



}
