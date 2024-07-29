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
    [SugarTable("SAJET.M_BLOCK_CONFIG_TYPE")]
    public class MBlockConfigType
    {
        /// <summary>
        /// ConfigTypeId 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true,ColumnName = "cONFIG_TYPE_ID")]
        public string ConfigTypeId { get; set; }

        /// <summary>
        /// ConfigTypeName 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_TYPE_NAME")]
        public string ConfigTypeName { get; set; }

        /// <summary>
        /// ConfigTypeDesc 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_TYPE_DESC")]
        public string ConfigTypeDesc { get; set; }

        /// <summary>
        /// NeederEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "nEEDER_EMPNO")]
        public string NeederEmpno { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// 厂区
        /// </summary>
        public string Site { get; set; }

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
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

    }
}
