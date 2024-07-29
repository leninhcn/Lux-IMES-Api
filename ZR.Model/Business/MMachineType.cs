using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable("SAJET.M_MACHINE_TYPE")]
    public class MMachineType
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public long Id { get; set; }

        /// <summary>
        /// MachineTypeName
        /// </summary>
        [SugarColumn(ColumnName = "MACHINE_TYPE_NAME")]
        public string MachineTypeName { get; set; }

        /// <summary>
        /// MachineTypeDesc
        /// </summary>
        [SugarColumn(ColumnName = "MACHINE_TYPE_DESC")]
        public string MachineTypeDesc { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        [SugarColumn(ColumnName = "ENABLED")]
        public string Enabled { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string UpdateEmpNo { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string CreateEmpNo { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string Site { get; set; }
    }
}
