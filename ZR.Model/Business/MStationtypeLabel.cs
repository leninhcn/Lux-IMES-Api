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
    [SugarTable("SAJET.M_STATIONTYPE_LABEL")]
    public class MStationtypeLabel
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string Id { get; set; }
        /// <summary>
        /// LabelParams 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_PARAMS")]
        public string LabelParams { get; set; }

        /// <summary>
        /// LabelName 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_NAME")]
        public string LabelName { get; set; }

        /// <summary>
        /// PrinterName 
        /// </summary>
        [SugarColumn(ColumnName = "pRINTER_NAME")]
        public string PrinterName { get; set; }

        /// <summary>
        /// LabelSrvIp 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_SRV_IP")]
        public string LabelSrvIp { get; set; }

        /// <summary>
        /// LabelDlUrl 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_DL_URL")]
        public string LabelDlUrl { get; set; }

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
        /// Plant 
        /// </summary>
        public string Plant { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Model 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Ipn 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// LabelType 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_TYPE")]
        public string LabelType { get; set; }

        /// <summary>
        /// LabelDesc 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_DESC")]
        public string LabelDesc { get; set; }

    }
}