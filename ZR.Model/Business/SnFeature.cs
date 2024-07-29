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
    [SugarTable("SAJET.M_SN_FEATURE")]
    public class SnFeature 
    {
        /// <summary>
        /// Id 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Ipn 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// SnFeature 
        /// </summary>
        [SugarColumn(ColumnName = "sN_FEATURE")]
        public string Feature { get; set; }

        /// <summary>
        /// PartType 
        /// </summary>
        [SugarColumn(ColumnName = "pART_TYPE")]
        public string PartType { get; set; }

        /// <summary>
        /// SnVendor 
        /// </summary>
        [SugarColumn(ColumnName = "sN_VENDOR")]
        public string SnVendor { get; set; }

        /// <summary>
        /// SnCode 
        /// </summary>
        [SugarColumn(ColumnName = "sN_CODE")]
        public string SnCode { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// SnLength 
        /// </summary>
        [SugarColumn(ColumnName = "sN_LENGTH")]
        public long? SnLength { get; set; }

        /// <summary>
        /// MesSpec 
        /// </summary>
        [SugarColumn(ColumnName = "mES_SPEC")]
        public string MesSpec { get; set; }

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
        /// SnCodePatition 
        /// </summary>
        [SugarColumn(ColumnName = "sN_CODE_PATITION")]
        public string SnCodePatition { get; set; }

    }
}
