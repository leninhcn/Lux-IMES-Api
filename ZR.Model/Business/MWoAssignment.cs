using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "SAJET.M_WOASSIGNMENT")]
    public class MWoAssignment
    {
        /// <summary>
        /// 工单
        /// </summary>
        [SugarColumn(ColumnName = "WORK_ORDER")]
        public string WorkOrder {  get; set; }


        /// <summary>
        /// 设备组
        /// </summary>
        [SugarColumn(ColumnName = "MACHINE_GROUP")]
        public string MachineGroup { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        [SugarColumn(ColumnName = "MACHINE")]
        public string Machine { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        [SugarColumn(ColumnName = "STATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// ByMachineGroup
        /// </summary>
        [SugarColumn(ColumnName = "BY_MACHINE_GROUP")]
        public string ByMachineGroup { get; set; }

        /// <summary>
        /// AssignStatus
        /// </summary>
        [SugarColumn(ColumnName = "ASSIGNSTATUS")]
        public string AssignStatus { get; set; }

        /// <summary>
        /// Creator
        /// </summary>
        [SugarColumn(ColumnName = "CREATOR")]
        public string Creator { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [SugarColumn(ColumnName = "CREATETIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// LastUpdate
        /// </summary>
        [SugarColumn(ColumnName = "LASTUPDATE")]
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// LastUpdateUser
        /// </summary>
        [SugarColumn(ColumnName = "LASTUPDATEUSER")]
        public string LastUpdateUser { get; set; }


        /// <summary>
        /// Option1
        /// </summary>
        [SugarColumn(ColumnName = "OPTION1")]
        public string Option1 { get; set; }

        /// <summary>
        /// Option2
        /// </summary>
        [SugarColumn(ColumnName = "OPTION2")]
        public string Option2 { get; set; }

        /// <summary>
        /// Option3
        /// </summary>
        [SugarColumn(ColumnName = "OPTION3")]
        public string Option3 { get; set; }

        /// <summary>
        /// Option4
        /// </summary>
        [SugarColumn(ColumnName = "OPTION4")]
        public string Option4 { get; set; }

        /// <summary>
        /// Option5
        /// </summary>
        [SugarColumn(ColumnName = "OPTION5")]
        public string Option5 { get; set; }

        /// <summary>
        /// WoAssignStartDate
        /// </summary>
        [SugarColumn(ColumnName = "WO_ASSING_START_DATE")]
        public DateTime? WoAssignStartDate { get; set; }

        /// <summary>
        /// WoAssignEndDate
        /// </summary>
        [SugarColumn(ColumnName = "WO_ASSING_END_DATE")]
        public DateTime? WoAssignEndDate { get; set; }


    }
}
