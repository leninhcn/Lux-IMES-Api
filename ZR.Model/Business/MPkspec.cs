using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("SAJET.M_PKSPEC")]
    public class MPkspec
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public long Id { get; set; }
        /// <summary>
        /// 包装规则名称 
        /// </summary>
        [SugarColumn(ColumnName = "pKSPEC_NAME")]
        public string PkspecName { get; set; }

        /// <summary>
        /// 栈板可装的箱数 
        /// </summary>
        [SugarColumn(ColumnName = "pALLET_QTY")]
        public long? PalletQty { get; set; }

        /// <summary>
        /// 箱可装的彩盒数量 
        /// </summary>
        [SugarColumn(ColumnName = "cARTON_QTY")]
        public long? CartonQty { get; set; }

        /// <summary>
        /// 彩盒可装的机台数量 
        /// </summary>
        [SugarColumn(ColumnName = "bOX_QTY")]
        public long? BoxQty { get; set; }

        /// <summary>
        /// InnerBoxQty 
        /// </summary>
        [SugarColumn(ColumnName = "iNNER_BOX_QTY")]
        public long? InnerBoxQty { get; set; }

        /// <summary>
        /// 资料更新人员工号 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

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
        /// 资料是否有效 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// 资料更新时间 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

    }
}
