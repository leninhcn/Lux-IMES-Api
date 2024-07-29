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
    [SugarTable("SAJET.M_DEFECT")]
    public class MDefect
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public long Id { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

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
        /// DefectLevel 
        /// </summary>
        [SugarColumn(ColumnName = "dEFECT_LEVEL")]
        public string DefectLevel { get; set; }

        /// <summary>
        /// DefectDesc 
        /// </summary>
        [SugarColumn(ColumnName = "dEFECT_DESC")]
        public string DefectDesc { get; set; }

        /// <summary>
        /// DefectDesc2 
        /// </summary>
        [SugarColumn(ColumnName = "dEFECT_DESC2")]
        public string DefectDesc2 { get; set; }

        /// <summary>
        /// DefectType 
        /// </summary>
        [SugarColumn(ColumnName = "dEFECT_TYPE")]
        public string DefectType { get; set; }

        /// <summary>
        /// CodeLevel 
        /// </summary>
        [SugarColumn(ColumnName = "cODE_LEVEL")]
        public string CodeLevel { get; set; }

        /// <summary>
        /// ParentDefectCode 
        /// </summary>
        [SugarColumn(ColumnName = "pARENT_DEFECT_CODE")]
        public string ParentDefectCode { get; set; }

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
        /// DefectCode 
        /// </summary>
        [SugarColumn(ColumnName = "dEFECT_CODE")]
        public string DefectCode { get; set; }

    }
}
