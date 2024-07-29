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
    [SugarTable("SAJET.M_LABEL_TYPE")]
    public class MLabelType
    {
        /// <summary>
        /// Model 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// PrinterName 
        /// </summary>
        [SugarColumn(ColumnName = "pRINTER_NAME")]
        public string PrinterName { get; set; }

        /// <summary>
        /// LabelTypeDesc 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_TYPE_DESC")]
        public string LabelTypeDesc { get; set; }

        /// <summary>
        /// LabelName 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_NAME")]
        public string LabelName { get; set; }

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
        /// TypeFlag 
        /// </summary>
        [SugarColumn(ColumnName = "tYPE_FLAG")]
        public string TypeFlag { get; set; }

        /// <summary>
        /// Ipn 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// Plant 
        /// </summary>
        public string Plant { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string Id { get; set; }

        /// <summary>
        /// LabelType 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_TYPE")]
        public string LabelType { get; set; }

    }
}
