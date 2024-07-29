using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZR.Model.Business
{
    /// <summary>
    /// 返回报错errorcode
    /// </summary>
    [SugarTable("SAJET.M_RESPONSE_ERRORCODE")]
    public class MResponseErrorcode
    {
        /// <summary>
        /// Code 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string Code { get; set; }

        /// <summary>
        /// ZhCn 
        /// </summary>
        [SugarColumn(ColumnName = "zH_CN")]
        public string ZhCn { get; set; }

        /// <summary>
        /// En 
        /// </summary>
        public string En { get; set; }

        /// <summary>
        /// ZhTw 
        /// </summary>
        [SugarColumn(ColumnName = "zH_TW")]
        public string ZhTw { get; set; }

        /// <summary>
        /// Vn 
        /// </summary>
        public string Vn { get; set; }

        /// <summary>
        /// OPTION1 
        /// </summary>
        public string OPTION1 { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// OPTION3 
        /// </summary>
        public string OPTION3 { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMPNO")]
        public string CreateEmpno { get; set; }

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
        /// OPTION2 
        /// </summary>
        public string OPTION2 { get; set; }

    }
}
