using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "SAJET.M_DATASYNC_LOG")]
    public class MDatasyncLog
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// TableName
        /// </summary>
        [SugarColumn(ColumnName = "TABLE_NAME")]
        public string TableName { get; set; }

        /// <summary>
        /// Json
        /// </summary>
        [SugarColumn(ColumnName = "JSON")]
        public string Json { get; set; }

        /// <summary>
        /// Bres
        /// </summary>
        [SugarColumn(ColumnName = "BRES")]
        public string Bres { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        [SugarColumn(ColumnName = "RESULT")]
        public string Result { get; set; }

        /// <summary>
        /// SendTime
        /// </summary>
        [SugarColumn(ColumnName = "SEND_TIME")]
        public DateTime? SendTime { get; set; }

        /// <summary>
        /// ApiUrl
        /// </summary>
        [SugarColumn(ColumnName = "API_URL")]
        public string ApiUrl { get; set; }

        /// <summary>
        /// Otype
        /// </summary>
        [SugarColumn(ColumnName = "OTYPE")]
        public string Otype { get; set; }

        /// <summary>
        /// SourceId
        /// </summary>
        [SugarColumn(ColumnName = "SOURCE_ID")]
        public long SourceId { get; set; }

        /// <summary>
        /// Refetch
        /// </summary>
        [SugarColumn(ColumnName = "REFETCH")]
        public string Refetch { get; set; }

        /// <summary>
        /// PkName
        /// </summary>
        [SugarColumn(ColumnName = "PK_NAME")]
        public string PkName { get; set; }

        /// <summary>
        /// ErrorMsg
        /// </summary>
        [SugarColumn(ColumnName = "ERROR_MSG")]
        public string ErrorMsg { get; set; }

        /// <summary>
        /// AppKey
        /// </summary>
        [SugarColumn(ColumnName = "APPKEY")]
        public string AppKey { get; set; }

        /// <summary>
        /// TopicCode
        /// </summary>
        [SugarColumn(ColumnName = "TOPIC_CODE")]
        public string TopicCode { get; set; }


        /// <summary>
        /// Action
        /// </summary>
        [SugarColumn(ColumnName = "ACTION")]
        public long Action { get; set; }

        /// <summary>
        /// ApiPath
        /// </summary>
        [SugarColumn(ColumnName = "API_PATH")]
        public string ApiPath { get; set; }

        /// <summary>
        /// FailTime
        /// </summary>
        [SugarColumn(ColumnName = "FAILTIME")]
        public string FailTime { get; set; }

        /// <summary>
        /// IsResend
        /// </summary>
        [SugarColumn(ColumnName = "ISRESEND")]
        public int IsResend { get; set; }
    }
}
