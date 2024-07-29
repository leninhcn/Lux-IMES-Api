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
    [SugarTable("SAJET.M_OPERATION_STEP")]
    public class MOperationStep
    {
        /// <summary>
        /// Id 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Model 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// StepSeq 
        /// </summary>
        [SugarColumn(ColumnName = "sTEP_SEQ")]
        public long StepSeq { get; set; }

        /// <summary>
        /// StepInput 
        /// </summary>
        [SugarColumn(ColumnName = "sTEP_INPUT")]
        public string StepInput { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

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
        /// StepDesc 
        /// </summary>
        [SugarColumn(ColumnName = "sTEP_DESC")]
        public string StepDesc { get; set; }

    }
}
