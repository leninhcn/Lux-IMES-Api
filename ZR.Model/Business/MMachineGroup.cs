using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable("SAJET.M_MACHINE_GROUP")]
    public class MMachineGroup
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public long Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [SugarColumn(ColumnName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [SugarColumn(ColumnName = "Description")]
        public string Description { get; set; }

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
        /// MES厂区 
        /// </summary>
        [SugarColumn(ColumnName = "SITE")]
        public string Site { get; set; }

        /// <summary>
        /// 站点类型 
        /// </summary>
        [SugarColumn(ColumnName = "STATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// 站点描述 
        /// </summary>
        [SugarColumn(ColumnName = "STATION_TYPE_DESC")]
        public string StationTypeDesc { get; set; }

    }
}
