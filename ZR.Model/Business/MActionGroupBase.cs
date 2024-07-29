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
    [SugarTable("SAJET.M_ACTION_GROUP_BASE")]
    public class MActionGroupBase
    {
        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// GroupId 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true,ColumnName = "gROUP_ID")]
        public long? GroupId { get; set; }

        /// <summary>
        /// GroupName 
        /// </summary>
        [SugarColumn(ColumnName = "gROUP_NAME")]
        public string GroupName { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

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
        /// GroupDesc 
        /// </summary>
        [SugarColumn(ColumnName = "gROUP_DESC")]
        public string GroupDesc { get; set; }


        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

    }
}