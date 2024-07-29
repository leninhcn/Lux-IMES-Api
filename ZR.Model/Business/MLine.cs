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
    [SugarTable("SAJET.M_LINE")]
    public class MLine
    {
        /// <summary>
        /// Id 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Line 
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// LineCustomer 
        /// </summary>
        [SugarColumn(ColumnName = "lINE_CUSTOMER")]
        public string LineCustomer { get; set; }

        /// <summary>
        /// LineSap 
        /// </summary>
        [SugarColumn(ColumnName = "lINE_SAP")]
        public string LineSap { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// LineDesc 
        /// </summary>
        [SugarColumn(ColumnName = "lINE_DESC")]
        public string LineDesc { get; set; }

        /// <summary>
        /// LineType 
        /// </summary>
        [SugarColumn(ColumnName = "lINE_TYPE")]
        public string LineType { get; set; }

        /// <summary>
        /// LineLevel 
        /// </summary>
        [SugarColumn(ColumnName = "lINE_LEVEL")]
        public string LineLevel { get; set; }

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
        /// WorkCenter 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_CENTER")]
        public string WorkCenter { get; set; }
    }
}
