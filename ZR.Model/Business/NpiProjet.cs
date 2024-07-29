
namespace ZR.Model.Business
{
    /// <summary>
    /// NPI项目管理
    /// </summary>
    [SugarTable("SAJET.p_npi_projet")]
    public class NpiProjet
    {
        /// <summary>
        /// Id 
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// NPINO 
        /// </summary>
        [SugarColumn(ColumnName = "npi_no")]
        public string NpiNo { get; set; }

        /// <summary>
        /// 公司名称 
        /// </summary>
        [SugarColumn(ColumnName = "company_name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 客户名称 
        /// </summary>
        [SugarColumn(ColumnName = "customer_name")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 料号 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// 客户料号 
        /// </summary>
        public string Apn { get; set; }

        /// <summary>
        /// 项目试制阶段 
        /// </summary>
        [SugarColumn(ColumnName = "project_trial_stage")]
        public string ProjectTrialStage { get; set; }

        /// <summary>
        /// 产品类型 
        /// </summary>
        [SugarColumn(ColumnName = "product_type")]
        public string ProductType { get; set; }

        /// <summary>
        /// 产品线 
        /// </summary>
        [SugarColumn(ColumnName = "product_line")]
        public string ProductLine { get; set; }

        /// <summary>
        /// 研发项目经理 
        /// </summary>
        [SugarColumn(ColumnName = "rd_manager")]
        public string RdManager { get; set; }

        /// <summary>
        /// NPI工程师 
        /// </summary>
        [SugarColumn(ColumnName = "npi_engineer")]
        public string NpiEngineer { get; set; }

        /// <summary>
        /// 产品版本 
        /// </summary>
        [SugarColumn(ColumnName = "product_version")]
        public string ProductVersion { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        [SugarColumn(ColumnName = "project_remark")]
        public string ProjectRemark { get; set; }

        /// <summary>
        /// 更新时间 
        /// </summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 更新人 
        /// </summary>
        [SugarColumn(ColumnName = "update_empno")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// 创建时间 
        /// </summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建人 
        /// </summary>
        [SugarColumn(ColumnName = "create_empno")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// 是否可用 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// Option1 
        /// </summary>
        [SugarColumn(ColumnName = "option1")]
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

        /// <summary>
        /// Option6 
        /// </summary>
        public string Option6 { get; set; }

    }
    public class NpiProjetFile
    {
        public string FileName { get; set; }
        public string AccessUrl { get; set; }
        /// <summary>
        /// 文件存储地址 eg：/uploads/20220202
        /// </summary>
        public string FileUrl { get; set; }
    }
}