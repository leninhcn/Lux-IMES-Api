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
    [SugarTable("SAJET.M_LABEL")]
    public class Label
    {
        /// <summary>
        /// LabelName 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_NAME")]
        public string LabelName { get; set; }

        /// <summary>
        /// LabelDesc 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_DESC")]
        public string LabelDesc { get; set; }

        /// <summary>
        /// FieldName 
        /// </summary>
        [SugarColumn(ColumnName = "fIELD_NAME")]
        public string FieldName { get; set; }

        /// <summary>
        /// SeqName 
        /// </summary>
        [SugarColumn(ColumnName = "sEQ_NAME")]
        public string SeqName { get; set; }

        /// <summary>
        /// FileName 
        /// </summary>
        [SugarColumn(ColumnName = "fILE_NAME")]
        public string FileName { get; set; }

        /// <summary>
        /// Type 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// QtyField 
        /// </summary>
        [SugarColumn(ColumnName = "qTY_FIELD")]
        public string QtyField { get; set; }

        /// <summary>
        /// Function 
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// SubFlag 
        /// </summary>
        [SugarColumn(ColumnName = "sUB_FLAG")]
        public long? SubFlag { get; set; }

        /// <summary>
        /// RevokeSp 
        /// </summary>
        [SugarColumn(ColumnName = "rEVOKE_SP")]
        public string RevokeSp { get; set; }

        /// <summary>
        /// GroupFlag 
        /// </summary>
        [SugarColumn(ColumnName = "gROUP_FLAG")]
        public string GroupFlag { get; set; }

        /// <summary>
        /// PartFieldName 
        /// </summary>
        [SugarColumn(ColumnName = "pART_FIELD_NAME")]
        public string PartFieldName { get; set; }

        /// <summary>
        /// ReprintField 
        /// </summary>
        [SugarColumn(ColumnName = "rEPRINT_FIELD")]
        public string ReprintField { get; set; }

        /// <summary>
        /// ReprintSql 
        /// </summary>
        [SugarColumn(ColumnName = "rEPRINT_SQL")]
        public string ReprintSql { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// CreateEmp 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMP")]
        public string CreateEmp { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// UpdateEmp 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMP")]
        public string UpdateEmp { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// TableName 
        /// </summary>
        [SugarColumn(ColumnName = "tABLE_NAME")]
        public string TableName { get; set; }

    }

    [SugarTable("SAJET.M_LABEL_HT")]
    public class LabelHis
    {
        /// <summary>
        /// LabelName 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_NAME")]
        public string LabelName { get; set; }

        /// <summary>
        /// LabelDesc 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_DESC")]
        public string LabelDesc { get; set; }

        /// <summary>
        /// FieldName 
        /// </summary>
        [SugarColumn(ColumnName = "fIELD_NAME")]
        public string FieldName { get; set; }

        /// <summary>
        /// SeqName 
        /// </summary>
        [SugarColumn(ColumnName = "sEQ_NAME")]
        public string SeqName { get; set; }

        /// <summary>
        /// FileName 
        /// </summary>
        [SugarColumn(ColumnName = "fILE_NAME")]
        public string FileName { get; set; }

        /// <summary>
        /// Type 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// QtyField 
        /// </summary>
        [SugarColumn(ColumnName = "qTY_FIELD")]
        public string QtyField { get; set; }

        /// <summary>
        /// Function 
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// SubFlag 
        /// </summary>
        [SugarColumn(ColumnName = "sUB_FLAG")]
        public long? SubFlag { get; set; }

        /// <summary>
        /// RevokeSp 
        /// </summary>
        [SugarColumn(ColumnName = "rEVOKE_SP")]
        public string RevokeSp { get; set; }

        /// <summary>
        /// GroupFlag 
        /// </summary>
        [SugarColumn(ColumnName = "gROUP_FLAG")]
        public string GroupFlag { get; set; }

        /// <summary>
        /// PartFieldName 
        /// </summary>
        [SugarColumn(ColumnName = "pART_FIELD_NAME")]
        public string PartFieldName { get; set; }

        /// <summary>
        /// ReprintField 
        /// </summary>
        [SugarColumn(ColumnName = "rEPRINT_FIELD")]
        public string ReprintField { get; set; }

        /// <summary>
        /// ReprintSql 
        /// </summary>
        [SugarColumn(ColumnName = "rEPRINT_SQL")]
        public string ReprintSql { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// CreateEmp 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMP")]
        public string CreateEmp { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// UpdateEmp 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMP")]
        public string UpdateEmp { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// TableName 
        /// </summary>
        [SugarColumn(ColumnName = "tABLE_NAME")]
        public string TableName { get; set; }

    }
}