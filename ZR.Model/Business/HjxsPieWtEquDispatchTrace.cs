using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "SAJET.HJXS_Pie_WtEquDispatch_Trace")]
    public class HjxsPieWtEquDispatchTrace
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public string Id {  get; set; }

        /// <summary>
        /// 单据编号
        /// </summary>
        [SugarColumn(ColumnName = "BILLCODE")]
        public string BillCode { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [SugarColumn(ColumnName = "EMPNO")]
        public string EmpNo { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnName = "EMPNAME")]
        public string EmpName { get; set; }

        /// <summary>
        /// 进程名称
        /// </summary>
        [SugarColumn(ColumnName = "PROCESS_NAME")]
        public string ProcessName { get; set; }

        /// <summary>
        /// 批准类型
        /// </summary>
        [SugarColumn(ColumnName = "APPROVAL_TYPE")]
        public string ApprovalType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? CreateTime { get; set; }
    }
}
