using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.m_part_Inscription", "碑文表")]
    public class ImesMpartInscription
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public int id { get; set; }

        /// <summary>
        /// Ipn 
        /// </summary>
        [SugarColumn(ColumnName = "IPN")]
        public string ipn { get; set; }
        /// <summary>
        /// INSCRIPTION 
        /// </summary>
        [SugarColumn(ColumnName = "INSCRIPTION")]
        public string inscription { get; set; }
        /// <summary>
        /// ENABLED 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }
        /// <summary>
        /// UPDATE_TIME 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? updateTime { get; set; }
        ///<summary>
        ///UPDATE_EMPNO
        ///<summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string updateEmpno { get; set; }
        ///<summary>
        ///CREATE_TIME
        ///<summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }
        ///<summary>
        ///CREATE_EMPNO
        ///<summary>
        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string createEmpno { get; set; }
        ///<summary>
        ///OPTIONS
        ///<summary>
        [SugarColumn(ColumnName = "OPTIONS")]
        public string options { get; set; }
        ///<summary>
        ///PIC
        ///<summary>
        [SugarColumn(ColumnName = "PIC")]
        public SqlBinary pic { get; set;}
        [SugarColumn(ColumnName = "PLANT")]
        public string plant { get; set;}
        [SugarColumn(ColumnName = "Site")]
        public string site { get; set; }
    }
}
