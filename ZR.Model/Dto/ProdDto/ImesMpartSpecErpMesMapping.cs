using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_PART_SPEC_ERP_MES_MAPPING", "品名对应关系表")]
    public class ImesMpartSpecErpMesMapping
    {
        /// <summary>
        /// ID 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID")]
        public int id { get; set; }

        /// <summary>
        /// MES_SPEC 
        /// </summary>
        [SugarColumn(ColumnName = "MES_SPEC")]
        public string mesSpec { get; set; }

        /// <summary>
        /// ERP_SPEC 
        /// </summary>
        [SugarColumn(ColumnName = "ERP_SPEC")]
        public string erpSpec { get; set; }

        /// <summary>
        /// MODEL 
        /// </summary>
        [SugarColumn(ColumnName = "MODEL")]
        public string model { get; set; }

        /// <summary>
        /// CATEGORY 
        /// </summary>
        [SugarColumn(ColumnName = "CATEGORY")]
        public string category { get; set; }

        /// <summary>
        /// STAGE 
        /// </summary>
        [SugarColumn(ColumnName = "STAGE")]
        public string stage { get; set; }

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
        /// ENABLESITED 
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }

        /// <summary>
        /// CUSTOMER_SPEC 
        /// </summary>
        [SugarColumn(ColumnName = "CUSTOMER_SPEC")]
        public string customerSpec { get; set; }
}
}
