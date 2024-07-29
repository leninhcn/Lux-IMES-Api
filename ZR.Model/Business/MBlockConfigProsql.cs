using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZR.Model.Business
{
    /// <summary>
    /// 卡关配置动态SQL
    /// </summary>
    [SugarTable("SAJET.M_BLOCK_CONFIG_PROSQL")]
    public class MBlockConfigProsql
    {
        /// <summary>
        /// ConfigTypeId 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_TYPE_ID")]
        public string ConfigTypeId { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// ConfigName 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_NAME")]
        public string ConfigName { get; set; }

        /// <summary>
        /// ConfigSeq 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_SEQ")]
        public long? ConfigSeq { get; set; }

        /// <summary>
        /// ConfigDesc 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_DESC")]
        public string ConfigDesc { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// CheckType 
        /// </summary>
        [SugarColumn(ColumnName = "cHECK_TYPE")]
        public string CheckType { get; set; }

        /// <summary>
        /// ConfigType 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_TYPE")]
        public string ConfigType { get; set; }

        /// <summary>
        /// ConfigProsql 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_PROSQL")]
        public string ConfigProsql { get; set; }

        /// <summary>
        /// Two 
        /// </summary>
        public string Two { get; set; }

        /// <summary>
        /// Tsn 
        /// </summary>
        public string Tsn { get; set; }

        /// <summary>
        /// Tline 
        /// </summary>
        public string Tline { get; set; }

        /// <summary>
        /// TstationType 
        /// </summary>
        [SugarColumn(ColumnName = "tSTATION_TYPE")]
        public string TstationType { get; set; }

        /// <summary>
        /// TstationName 
        /// </summary>
        [SugarColumn(ColumnName = "tSTATION_NAME")]
        public string TstationName { get; set; }

        /// <summary>
        /// TrouteName 
        /// </summary>
        [SugarColumn(ColumnName = "tROUTE_NAME")]
        public string TrouteName { get; set; }

        /// <summary>
        /// Csn 
        /// </summary>
        public string Csn { get; set; }

        /// <summary>
        /// Cwo 
        /// </summary>
        public string Cwo { get; set; }

        /// <summary>
        /// Tempno 
        /// </summary>
        public string Tempno { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// TcartonNo 
        /// </summary>
        [SugarColumn(ColumnName = "tCARTON_NO")]
        public string TcartonNo { get; set; }

        /// <summary>
        /// TpalletNo 
        /// </summary>
        [SugarColumn(ColumnName = "tPALLET_NO")]
        public string TpalletNo { get; set; }

        /// <summary>
        /// Tkpsn 
        /// </summary>
        public string Tkpsn { get; set; }

        /// <summary>
        /// TPARAM1 
        /// </summary>
        public string TPARAM1 { get; set; }

        /// <summary>
        /// TPARAM2 
        /// </summary>
        public string TPARAM2 { get; set; }

        /// <summary>
        /// TPARAM3 
        /// </summary>
        public string TPARAM3 { get; set; }

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
        /// ConfigId 
        /// </summary>
        [SugarColumn(ColumnName = "cONFIG_ID", IsPrimaryKey = true, IsIdentity = false)]
        public string ConfigId { get; set; }

    }
}
