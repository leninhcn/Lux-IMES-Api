using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZR.Model.Business
{
    /// <summary>
    /// 责任类型
    /// </summary>
    [SugarTable("SAJET.M_DUTY")]
    public class MDuty
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public long Id { get; set; }

        /// <summary>
        /// 责任Code 
        /// </summary>
        [SugarColumn(ColumnName = "dUTY_CODE")]
        public string DutyCode { get; set; }

        /// <summary>
        /// 责任描述 
        /// </summary>
        [SugarColumn(ColumnName = "dUTY_DESC")]
        public string DutyDesc { get; set; }

        /// <summary>
        /// DutyDesc2 
        /// </summary>
        [SugarColumn(ColumnName = "dUTY_DESC2")]
        public string DutyDesc2 { get; set; }

        /// <summary>
        /// 状态
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
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

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
    }
}
