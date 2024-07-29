using System.ComponentModel.DataAnnotations;
using MiniExcelLibs.Attributes;

namespace ZR.Model.Dto
{
    /// <summary>
    /// NPI项目管理查询对象
    /// </summary>
    public class NpiProjetQueryDto : PagerInfo 
    {
        public string NpiNo { get; set; }
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
        public string Ipn { get; set; }
        public string Apn { get; set; }
        public string ProjectTrialStage { get; set; }
        public string ProductType { get; set; }
        public string ProductLine { get; set; }
        public string RdManager { get; set; }
        public string NpiEngineer { get; set; }
        public string ProductVersion { get; set; }
        public DateTime? BeginCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
    }

    /// <summary>
    /// NPI项目管理输入输出对象
    /// </summary>
    public class NpiProjetDto
    {
        [ExcelColumn(Name = "Id")]
        [ExcelColumnName("Id")]
        public int? Id { get; set; }

      //  [Required(ErrorMessage = "NPINO不能为空")]
        [ExcelColumn(Name = "NPINO")]
        [ExcelColumnName("NPINO")]
        public string NpiNo { get; set; }

        [ExcelColumn(Name = "Option1")]
        [ExcelColumnName("项目描述")]
        public string Option1 { get; set; }

        [Required(ErrorMessage = "公司名称不能为空")]
        [ExcelColumn(Name = "公司名称")]
        [ExcelColumnName("公司名称")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "客户名称不能为空")]
        [ExcelColumn(Name = "客户名称")]
        [ExcelColumnName("客户名称")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "料号不能为空")]
        [ExcelColumn(Name = "料号")]
        [ExcelColumnName("料号")]
        public string Ipn { get; set; }

        [Required(ErrorMessage = "客户料号不能为空")]
        [ExcelColumn(Name = "客户料号")]
        [ExcelColumnName("客户料号")]
        public string Apn { get; set; }

        [Required(ErrorMessage = "项目试制阶段不能为空")]
        [ExcelColumn(Name = "项目试制阶段")]
        [ExcelColumnName("项目试制阶段")]
        public string ProjectTrialStage { get; set; }

        [Required(ErrorMessage = "产品类型不能为空")]
        [ExcelColumn(Name = "产品类型")]
        [ExcelColumnName("产品类型")]
        public string ProductType { get; set; }

        [Required(ErrorMessage = "产品线不能为空")]
        [ExcelColumn(Name = "产品线")]
        [ExcelColumnName("产品线")]
        public string ProductLine { get; set; }

        [Required(ErrorMessage = "研发项目经理不能为空")]
        [ExcelColumn(Name = "研发项目经理")]
        [ExcelColumnName("研发项目经理")]
        public string RdManager { get; set; }

        [Required(ErrorMessage = "NPI工程师不能为空")]
        [ExcelColumn(Name = "NPI工程师")]
        [ExcelColumnName("NPI工程师")]
        public string NpiEngineer { get; set; }

        [Required(ErrorMessage = "产品版本不能为空")]
        [ExcelColumn(Name = "产品版本")]
        [ExcelColumnName("产品版本")]
        public string ProductVersion { get; set; }

        [Required(ErrorMessage = "备注不能为空")]
        [ExcelColumn(Name = "备注")]
        [ExcelColumnName("备注")]
        public string ProjectRemark { get; set; }

        [ExcelColumn(Name = "更新时间", Format = "yyyy-MM-dd HH:mm:ss")]
        [ExcelColumnName("更新时间")]
        public DateTime? UpdateTime { get; set; }

        [ExcelColumn(Name = "更新人")]
        [ExcelColumnName("更新人")]
        public string UpdateEmpno { get; set; }

        [ExcelColumn(Name = "创建时间", Format = "yyyy-MM-dd HH:mm:ss")]
        [ExcelColumnName("创建时间")]
        public DateTime? CreateTime { get; set; }

        [ExcelColumn(Name = "创建人")]
        [ExcelColumnName("创建人")]
        public string CreateEmpno { get; set; }

        [ExcelIgnore]
        public string Enabled { get; set; }
    

        [ExcelIgnore]
        public string Option2 { get; set; }

        [ExcelIgnore]
        public string Option3 { get; set; }

        [ExcelIgnore]
        public string Option4 { get; set; }

        [ExcelIgnore]
        public string Option5 { get; set; }

        [ExcelColumn(Name = "Option6")]
        [ExcelColumnName("Option6")]
        public string Option6 { get; set; }



    }
}