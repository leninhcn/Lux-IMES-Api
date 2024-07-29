using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_SN_FEATURE", "料号特征码管理查询表")]
    public class ImesMsnFeature
    {
        /// <summary>
        /// ID 
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public int id { get; set; }

        /// <summary>
        /// IPN 
        /// </summary>
        [SugarColumn(ColumnName = "IPN")]
        public string ipn { get; set; }

        /// <summary>
        /// SN_FEATURE 
        /// </summary>
        [SugarColumn(ColumnName = "SN_FEATURE")]
        public string snFeature { get; set; }

        /// <summary>
        /// PART_TYPE 
        /// </summary>
        [SugarColumn(ColumnName = "PART_TYPE")]
        public string partType { get; set; }

        /// <summary>
        /// SN_VENDOR 
        /// </summary>
        [SugarColumn(ColumnName = "SN_VENDOR")]
        public string snVendor { get; set; }

        /// <summary>
        /// SN_CODE 
        /// </summary>
        [SugarColumn(ColumnName = "SN_CODE")]
        public string snCode { get; set; }

        /// <summary>
        /// SN_CODE_PATITION 
        /// </summary>
        [SugarColumn(ColumnName = "SN_CODE_PATITION")]
        public string snCodePatition { get; set; }


        /// <summary>
        /// SN_LENGTH 
        /// </summary>
        [SugarColumn(ColumnName = "SN_LENGTH")]
        public int snLength { get; set; }


        /// <summary>
        /// MES_SPEC 
        /// </summary>
        [SugarColumn(ColumnName = "MES_SPEC")]
        public string mesSpec { get; set; }

        /// <summary>
        /// UPDATE_EMPNO 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string updateEmpno { get; set; }

        /// <summary>
        /// UPDATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? updateTime { get; set; }

        /// <summary>
        /// CREATE_EMPNO 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string createEmpno { get; set; }

        /// <summary>
        /// CREATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }

        /// <summary>
        /// ENABLED 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }

        /// <summary>
        /// PLANT 
        /// </summary>
        [SugarColumn(ColumnName = "PLANT")]
        public string plant { get; set; }

        /// <summary>
        /// SITE 
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }
    }
}
