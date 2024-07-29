
namespace ZR.Model.Business
{

    /// <summary>
    /// 不良品统计
    /// </summary>
    [SugarTable("SAJET.P_NG_DETAIL")]
    public class PNgDetail
    {
        /// <summary>
        /// 不良统计明细ID 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", OracleSequenceName = "SAJET.NG_MES", ColumnDescription = "主键")]
        public long Id { get; set; }

        /// <summary>
        /// 区段 
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// 生产料号 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// 工单目标产量 
        /// </summary>
        public long? Targetqty { get; set; }

        /// <summary>
        /// bz 
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 工站ID 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_ID")]
        public string StationId { get; set; }

        /// <summary>
        /// 工站名称 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_NAME")]
        public string StationName { get; set; }

        /// <summary>
        /// 工站类型 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// 单次抽检数量 
        /// </summary>
        [SugarColumn(ColumnName = "sAMPLING_INSPECTION")]
        public long? SamplingInspection { get; set; }

        /// <summary>
        /// 单次抽检数量不良品 
        /// </summary>
        [SugarColumn(ColumnName = "sAMPLING_INSPECTION_NG")]
        public long? SamplingInspectionNg { get; set; }

        /// <summary>
        /// 不良代码 
        /// </summary>
        [SugarColumn(ColumnName = "dEFECT_CODE")]
        public string DefectCode { get; set; }

        /// <summary>
        /// 不良描述 
        /// </summary>
        [SugarColumn(ColumnName = "bAD_DETAIL")]
        public string BadDetail { get; set; }

        /// <summary>
        /// 插架码 
        /// </summary>
        [SugarColumn(ColumnName = "iNSERTION_CODE")]
        public string InsertionCode { get; set; }

        /// <summary>
        /// 操作员 
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 处理措施 
        /// </summary>
        [SugarColumn(ColumnName = "hANDLING_MEASURES")]
        public string HandlingMeasures { get; set; }


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
        /// 更新时间 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 线别 
        /// </summary>
        [SugarColumn(ColumnName = "lINE_ID")]
        public string LineId { get; set; }

        /// <summary>
        /// 工单编号 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_ORDER")]
        public string WorkOrder { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        [SugarColumn(ColumnName = "mACHINE_CODE")]
        public string MachineCode { get; set; }

        [SugarColumn(ColumnName = "mACHINE_NO")]
        public string MachineNo { get; set; }
    }
}