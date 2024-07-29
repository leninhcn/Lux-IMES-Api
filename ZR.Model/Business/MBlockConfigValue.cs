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
    [SugarTable("SAJET.M_BLOCK_CONFIG_VALUE")]
    public class MBlockConfigValue
    {
        /// <summary>
        /// ConfigTypeId 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_TYPE_ID")]
        public string ConfigTypeId { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// ConfigName 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_NAME")]
        public string ConfigName { get; set; }

        /// <summary>
        /// ConfigDesc 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_DESC")]
        public string ConfigDesc { get; set; }

        /// <summary>
        /// ConfigValue 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_VALUE")]
        public string ConfigValue { get; set; }

        /// <summary>
        /// RouteName 
        /// </summary>
        [SugarColumn(ColumnName = "rOUTE_NAME")]
        public string RouteName { get; set; }

        /// <summary>
        /// Line 
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// StationName 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_NAME")]
        public string StationName { get; set; }

        /// <summary>
        /// ClientId 
        /// </summary>
        [SugarColumn(ColumnName = "cLIENT_ID")]
        public string ClientId { get; set; }

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
        /// ConfigId 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "cONFIG_ID")]
        public string ConfigId { get; set; }

    }
}
