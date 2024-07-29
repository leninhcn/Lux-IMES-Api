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
    [SugarTable("SAJET.M_ROUTE_DETAIL")]
    public class RouteDetail
    {
        /// <summary>
        /// RouteName 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false, ColumnName = "rOUTE_NAME")]
        public string RouteName { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string Site { get; set; }

        /// <summary>
        /// NextStationType 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false, ColumnName = "nEXT_STATION_TYPE")]
        public string NextStationType { get; set; }

        /// <summary>
        /// Result 
        /// </summary>
        public long? Result { get; set; }

        /// <summary>
        /// Seq 
        /// </summary>
        public long? Seq { get; set; }

        /// <summary>
        /// PdCode 
        /// </summary>
        [SugarColumn(ColumnName = "pD_CODE")]
        public string PdCode { get; set; }

        /// <summary>
        /// Necessary 
        /// </summary>
        public string Necessary { get; set; }

        /// <summary>
        /// Step 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public long Step { get; set; }

        /// <summary>
        /// DefaultInstationtype 
        /// </summary>
        [SugarColumn(ColumnName = "dEFAULT_INSTATIONTYPE")]
        public string DefaultInstationtype { get; set; }

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
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false, ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

    }
}
