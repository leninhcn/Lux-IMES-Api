
namespace ZR.Model.Business
{
    /// <summary>
    /// 辅材上下线
    /// </summary>
    [SugarTable("SAJET.P_MATERIALS_STATUS")]
    public class PMaterialsStatus
    {
        /// <summary>
        /// 操作类型(记录上线还是下线) 
        /// </summary>
        [SugarColumn(ColumnName = "sCAN_TYPE")]
        public string ScanType { get; set; }

        /// <summary>
        /// 更新时间 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 生产料号 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// 上下线的辅材数量 
        /// </summary>
        [SugarColumn(ColumnName = "mATE_SUM")]
        public long? MateSum { get; set; }

        /// <summary>
        /// 设备编号

        /// </summary>
        [SugarColumn(ColumnName = "mACHINE_CODE")]
        public string MachineCode { get; set; }

        /// <summary>
        /// 设备名称 
        /// </summary>
        [SugarColumn(ColumnName = "mACHINE_TYPE")]
        public string MachineType { get; set; }


        /// <summary>
        /// 线别 
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// 编号 
        /// </summary>
     
        public long Id { get; set; }

        /// <summary>
        /// 区段 
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// 制程 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        ///// <summary>
        ///// 扫描出来的IPN  
        ///// </summary>
        //[SugarColumn(ColumnName = "sCAN_IPN")]
        //public string ScanIpn { get; set; }

        /// <summary>
        /// 批号 
        /// </summary>
        /// 
        public string Batchno { get; set; }

        [SugarColumn(ColumnName = "fC_CODE")]
        public string FcCode  { get; set; }

        /// <summary>
        /// 删除标志（0代表存在 1代表删除） 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// 厂区-租户 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// 创建人员工号 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMPNO")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// 更新人员工号 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// 创建时间 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 工单编号 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_ORDER")]
        public string WorkOrder { get; set; }

    }
}