using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "SAJET.S_BASE" )]
    public class SBase
    {
        /// <summary>
        /// Program
        /// </summary>
        [SugarColumn(ColumnName = "PROGRAM")]
        public string Program {  get; set; }

        /// <summary>
        /// ParamName
        /// </summary>
        [SugarColumn(ColumnName = "PARAM_NAME")]
        public string ParamName {  get; set; }

        /// <summary>
        /// ParamValue
        /// </summary>
        [SugarColumn(ColumnName = "PARAM_VALUE")]
        public string ParamValue { get; set; }

    }
}
