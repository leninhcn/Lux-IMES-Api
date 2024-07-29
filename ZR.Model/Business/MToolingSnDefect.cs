using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "ISMT.M_TOOLING_SN_DEFECT", TableDescription = "治具不良表")]
    public class MToolingSnDefect
    {
        /// <summary>
        /// RecId
        /// </summary>
        [SugarColumn(ColumnName = "REC_ID")]
        public long RecId {  get; set; }

        /// <summary>
        /// ToolingSn
        /// </summary>
        [SugarColumn(ColumnName = "TOOLING_SN")]
        public string ToolingSn { get; set; }

        /// <summary>
        /// RecTime
        /// </summary>
        [SugarColumn(ColumnName = "REC_TIME")]
        public DateTime RecTime { get; set; }

        /// <summary>
        /// DefectCode
        /// </summary>
        [SugarColumn(ColumnName = "DEFECT_CODE")]
        public string DefectCode { get; set; }

        /// <summary>
        /// TestEmpNo
        /// </summary>
        [SugarColumn(ColumnName = "TEST_EMPNO")]
        public string TestEmpNo { get; set; }

        /// <summary>
        /// RpStatus
        /// </summary>
        [SugarColumn(ColumnName = "RP_STATUS")]
        public string RpStatus { get; set; }

        /// <summary>
        /// ReceiveTime
        /// </summary>
        [SugarColumn(ColumnName = "RECEIVE_TIME")]
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// DefectQty
        /// </summary>
        [SugarColumn(ColumnName = "DEFECT_QTY")]
        public long DefectQty { get; set; }

        /// <summary>
        /// ReceiveProcess
        /// </summary>
        [SugarColumn(ColumnName = "RECEIVE_PROCESS")]
        public string ReceiveProcess { get; set; }

        /// <summary>
        /// ReceiveEmp
        /// </summary>
        [SugarColumn(ColumnName = "RECEIVE_EMP")]
        public string ReceiveEmp { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [SugarColumn(ColumnName = "LOCATION")]
        public string Location { get; set; }

        /// <summary>
        /// ErrPoint
        /// </summary>
        [SugarColumn(ColumnName = "ERR_POINT")]
        public long ErrPoint { get; set; }

        /// <summary>
        /// Site
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string Site { get; set; }
    }
}
