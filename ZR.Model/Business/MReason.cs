using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZR.Model.Business
{
    /// <summary>
    /// 不良原因
    /// </summary>
    [SugarTable("SAJET.M_REASON")]
    public class MReason
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public long Id { get; set; }

        /// <summary>
        /// 不良原因代码
        /// </summary>
        [SugarColumn(ColumnName = "rEASON_CODE")]
        public string ReasonCode { get; set; }

        /// <summary>
        /// ?????? 
        /// </summary>
        [SugarColumn(ColumnName = "rEASON_LEVEL")]
        public string ReasonLevel { get; set; }

        /// <summary>
        /// 不良原因描述1
        /// </summary>
        [SugarColumn(ColumnName = "rEASON_DESC")]
        public string ReasonDesc { get; set; }

        /// <summary>
        /// 不良原因描述2
        /// </summary>
        [SugarColumn(ColumnName = "rEASON_DESC2")]
        public string ReasonDesc2 { get; set; }

        /// <summary>
        /// 父不良原因ID
        /// </summary>
        [SugarColumn(ColumnName = "pARENT_REASON_ID")]
        public long? ParentReasonId { get; set; }

        /// <summary>
        /// 不良原因等级
        /// </summary>
        [SugarColumn(ColumnName = "cODE_LEVEL")]
        public string CodeLevel { get; set; }

        /// <summary>
        /// UpdateEmpno 更新人
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
        /// Enabled 状态
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// ReasonType 类型
        /// </summary>
        [SugarColumn(ColumnName = "rEASON_TYPE")]
        public string ReasonType { get; set; }

        /// <summary>
        /// OPTION1 扩展栏位
        /// </summary>
        public string OPTION1 { get; set; }

        /// <summary>
        /// OPTION2 扩展栏位
        /// </summary>
        public string OPTION2 { get; set; }

        /// <summary>
        /// Site厂区信息
        /// </summary>
        public string Site { get; set; }
    }
}
