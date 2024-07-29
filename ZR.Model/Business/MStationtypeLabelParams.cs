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
    [SugarTable("SAJET.M_STATIONTYPE_LABEL_PARAMS")]
    public class MStationtypeLabelParams
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string Id { get; set; }
        /// <summary>
        /// Model 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Ipn 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// LabelType 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_TYPE")]
        public string LabelType { get; set; }

        /// <summary>
        /// VarName 
        /// </summary>
        [SugarColumn(ColumnName = "vAR_NAME")]
        public string VarName { get; set; }

        /// <summary>
        /// VarType 
        /// </summary>
        [SugarColumn(ColumnName = "vAR_TYPE")]
        public string VarType { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// FieldName 
        /// </summary>
        [SugarColumn(ColumnName = "fIELD_NAME")]
        public string FieldName { get; set; }

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
        /// Plant 
        /// </summary>
        public string Plant { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

    }
}