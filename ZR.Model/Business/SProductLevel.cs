using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable("SAJET.S_PRODUCTLEVEL")]
    public class SProductLevel
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// ProdCode 
        /// </summary>
        [SugarColumn(ColumnName = "PROD_CODE")]
        public string ProdCode { get; set; }


        /// <summary>
        /// ProdName 
        /// </summary>
        [SugarColumn(ColumnName = "PROD_NAME")]
        public string ProdName { get; set; }
    }
}
