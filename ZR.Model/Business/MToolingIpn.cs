using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable("SAJET.M_TOOLING_IPN")]
    public class MToolingIpn
    {
        /// <summary>
        /// TOOLING_ID
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_ID")]
        public long ToolingId { get; set; }

        /// <summary>
        /// IPN
        /// </summary>
        [SugarColumn(ColumnName = "IPN")]
        public string Ipn { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string UpdateEmpNo { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string CreateEmpNo { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public string CreateTime { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public char Enabled { get; set; }

        /// <summary>
        /// Plant 
        /// </summary>
        [SugarColumn(ColumnName = "PLANT")]
        public string Plant { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string Site { get; set; }
    }
}
