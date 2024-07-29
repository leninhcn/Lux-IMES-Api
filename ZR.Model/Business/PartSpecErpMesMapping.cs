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
    [SugarTable("SAJET.M_PART_SPEC_ERP_MES_MAPPING")]
    public class PartSpecErpMesMapping
    {
        /// <summary>
        /// Id 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// MesSpec 
        /// </summary>
        [SugarColumn(ColumnName = "mES_SPEC")]
        public string MesSpec { get; set; }

        /// <summary>
        /// ErpSpec 
        /// </summary>
        [SugarColumn(ColumnName = "eRP_SPEC")]
        public string ErpSpec { get; set; }

        /// <summary>
        /// Model 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Category 
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// CustomerSpec 
        /// </summary>
        [SugarColumn(ColumnName = "cUSTOMER_SPEC")]
        public string CustomerSpec { get; set; }

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
        /// Stage 
        /// </summary>
        public string Stage { get; set; }

    }
}
