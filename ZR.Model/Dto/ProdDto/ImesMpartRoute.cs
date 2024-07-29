using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_PART_ROUTE")]
    public class ImesMpartRoute
    {
        /// <summary>
        /// ID 
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public int id { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? updateTime { get; set; }

        /// <summary>
        /// CREATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }


        /// <summary>
        /// LINE 
        /// </summary>
        [SugarColumn(ColumnName = "LINE")]
        public string line { get; set; }

        /// <summary>
        /// IPN 
        /// </summary>
        [SugarColumn(ColumnName = "IPN")]
        public string ipn { get; set; }

        /// <summary>
        /// STAGE 
        /// </summary>
        [SugarColumn(ColumnName = "STAGE")]
        public string stage { get; set; }

        /// <summary>
        /// MODEL 
        /// </summary>
        [SugarColumn(ColumnName = "MODEL")]
        public string model { get; set; }

        /// <summary>
        /// ROUTE_NAME 
        /// </summary>
        [SugarColumn(ColumnName = "ROUTE_NAME")]
        public string routeName { get; set; }

        /// <summary>
        /// UPDATE_EMPNO 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string updateEmpno { get; set; }

        /// <summary>
        /// CREATE_EMPNO 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string createEmpno { get; set; }

        /// <summary>
        /// ENABLED 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }

        /// <summary>
        /// PACK_SPEC 
        /// </summary>
        [SugarColumn(ColumnName = "PACK_SPEC")]
        public string packSpec { get; set; }

        /// <summary>
        /// RULE_SET_NAME 
        /// </summary>
        [SugarColumn(ColumnName = "RULE_SET_NAME")]
        public string ruleSetName { get; set; }

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
