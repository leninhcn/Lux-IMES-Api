
namespace ZR.Model.Business
{
    /// <summary>
    /// 生产计划维护
    /// </summary>
    [SugarTable("SAJET.m_line_plan")]
    public class MLinePlan
    {
        /// <summary>
        /// ID 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 线别 
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// 料号 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// 机种 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 工单 
        /// </summary>
        [SugarColumn(ColumnName = "work_order")]
        public string WorkOrder { get; set; }

        /// <summary>
        /// 数量 
        /// </summary>
        public string Qty { get; set; }

        /// <summary>
        /// 面别 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// 上线日期 
        /// </summary>
        [SugarColumn(ColumnName = "online_date")]
        public DateTime? OnlineDate { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人 
        /// </summary>
        [SugarColumn(ColumnName = "create_emp")]
        public string CreateEmp { get; set; }

        /// <summary>
        /// 创建时间 
        /// </summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新人 
        /// </summary>
        [SugarColumn(ColumnName = "updated_emp")]
        public string UpdatedEmp { get; set; }

        /// <summary>
        /// 更新时间 
        /// </summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// Option1 
        /// </summary>
        public string Option1 { get; set; }

        /// <summary>
        /// Option2 
        /// </summary>
        public string Option2 { get; set; }

        /// <summary>
        /// Option3 
        /// </summary>
        public string Option3 { get; set; }

        /// <summary>
        /// Option4 
        /// </summary>
        public string Option4 { get; set; }

        /// <summary>
        /// Option5 
        /// </summary>
        public string Option5 { get; set; }

    }
}