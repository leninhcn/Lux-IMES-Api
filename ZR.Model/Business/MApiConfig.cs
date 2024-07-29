using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "SAJET.M_APICONFIG")]
    public class MApiConfig
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(ColumnName ="ID")]
        public int Id {  get; set; }

        /// <summary>
        /// ApiCode
        /// </summary>
        [SugarColumn(ColumnName = "API_CODE")]
        public string ApiCode {  get; set; }

        /// <summary>
        /// ApiUrl
        /// </summary>
        [SugarColumn(ColumnName = "API_URL")]
        public string ApiUrl { get; set; }

        /// <summary>
        /// Tcomment
        /// </summary>
        [SugarColumn(ColumnName = "TCOMMENT")]
        public string Tcomment { get; set; }

        /// <summary>
        /// IsOpenApi
        /// </summary>
        [SugarColumn(ColumnName = "IS_OPENAPI")]
        public string IsOpenApi { get; set; }

        /// <summary>
        /// AppCode
        /// </summary>
        [SugarColumn(ColumnName = "APP_CODE")]
        public string AppCode { get; set; }

        /// <summary>
        /// TopicCode
        /// </summary>
        [SugarColumn(ColumnName = "TOPIC_CODE")]
        public string TopicCode { get; set; }

        /// <summary>
        /// AK
        /// </summary>
        [SugarColumn(ColumnName = "AK")]
        public string AK { get; set; }

        /// <summary>
        /// SK
        /// </summary>
        [SugarColumn(ColumnName = "SK")]
        public string SK { get; set; }

        /// <summary>
        /// Refetch
        /// </summary>
        [SugarColumn(ColumnName = "REFETCH")]
        public string Refetch { get; set; }

        /// <summary>
        /// AuthType
        /// </summary>
        [SugarColumn(ColumnName = "AUTH_TYPE")]
        public string AuthType { get; set; }

        /// <summary>
        /// EapApi
        /// </summary>
        [SugarColumn(ColumnName = "REFAPI")]
        public string EapApi { get; set; }
    }
}
