using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Rework
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("P_REWORK_NO")]
    public class PReworkNo
    {


        /// <summary>
        /// Id 
        /// </summary>
        /// 
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public long Id { get; set; }


        /// <summary>
        /// ReworkNo 
        /// </summary>
        [SugarColumn(ColumnName = "rEWORK_NO")]
        public string ReworkNo { get; set; }

        /// <summary>
        /// Remark 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMPNO")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// Condition 
        /// </summary>
        public string Condition { get; set; }

    }




}
