namespace ZR.Model.Business
{
    [SugarTable("SAJET.M_MACHINE")]
    public class MMachine
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public long Id { get; set; }

        /// <summary>
        /// MachineCode
        /// </summary>
        [SugarColumn(ColumnName = "MACHINE_CODE")]
        public string MachineCode { get; set; }

        /// <summary>
        /// MachineDesc
        /// </summary>
        [SugarColumn(ColumnName = "MACHINE_DESC")]
        public string MachineDesc { get; set; }

        /// <summary>
        /// Machineloc
        /// </summary>
        [SugarColumn(ColumnName = "MACHINE_LOC")]
        public string Machineloc { get; set; }

        /// <summary>
        /// MaxUseCount
        /// </summary>
        [SugarColumn(ColumnName = "MAX_USED_COUNT")]
        public long MaxUseCount { get; set; }

        /// <summary>
        /// USED_COUNT
        /// </summary>
        [SugarColumn(ColumnName = "USED_COUNT")]
        public long UsedCount { get; set; }

        /// <summary>
        /// MachineType
        /// </summary>
        [SugarColumn(ColumnName = "MACHINE_TYPE")]
        public string MachineType { get; set; }

        /// <summary>
        /// UtilizationFlag
        /// </summary>
        [SugarColumn(ColumnName = "UTILIZATION_FLAG")]
        public string UtilizationFlag { get; set; }

        /// <summary>
        /// MachineSort
        /// </summary>
        [SugarColumn(ColumnName = "MACHINE_SORT")]
        public long MachineSort { get; set; }

        /// <summary>
        /// Line
        /// </summary>
        [SugarColumn(ColumnName = "LINE")]
        public string Line { get; set; }

        /// <summary>
        /// Stage
        /// </summary>
        [SugarColumn(ColumnName = "STAGE")]
        public string Stage { get; set; }

        /// <summary>
        /// StationType
        /// </summary>
        [SugarColumn(ColumnName = "STATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// StationName
        /// </summary>
        [SugarColumn(ColumnName = "STATION_NAME")]
        public string StationName { get; set; }


        /// <summary>
        /// StandradOutput
        /// </summary>
        [SugarColumn(ColumnName = "STANDARD_OUTPUT")]
        public long StandradOutput { get; set; }

        /// <summary>
        /// ExpectOutput
        /// </summary>
        [SugarColumn(ColumnName = "EXPECT_OUTPUT")]
        public long ExpectOutput { get; set; }

        /// <summary>
        /// CurrentCT
        /// </summary>
        [SugarColumn(ColumnName = "CURRENT_CT")]
        public long CurrentCT { get; set; }



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

        /// <summary>
        /// MACHINE_GROUP 
        /// </summary>
        [SugarColumn(ColumnName = "MACHINE_GROUP")]
        public string MachineGroup { get; set; }

        /// <summary>
        /// GROUP_ID 
        /// </summary>
        [SugarColumn(ColumnName = "GROUP_ID")]
        public long GroupId { get; set; }

        /// <summary>
        /// MachineStatus 
        /// </summary>
        [SugarColumn(ColumnName = "MACHINE_STATUS")]
        public string MachineStatus { get; set; }

        /// <summary>
        /// UpdateStatusTime 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_STATUS_TIME")]
        public string UpdateStatusTime { get; set; }

        /// <summary>
        /// ShiftCode 
        /// </summary>
        [SugarColumn(ColumnName = "SHIFT_CODE")]
        public string ShiftCode { get; set; }
    }
}
