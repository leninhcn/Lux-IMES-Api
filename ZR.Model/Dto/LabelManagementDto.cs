using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.Quality;

namespace ZR.Model.Dto
{
    /// <summary>
    /// 查询labeltype对象
    /// </summary>
    public class MLabelTypeQueryDto : PagerInfo
    {
        public string Model { get; set; }
        public string Ipn { get; set; }
        public string LabelType { get; set; }
    }

    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MLabelTypeDto
    {
        public string Model { get; set; }

        public string PrinterName { get; set; }

        public string LabelTypeDesc { get; set; }

        public string LabelName { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        public string TypeFlag { get; set; }

        public string Ipn { get; set; }

        public string Plant { get; set; }

        public string Site { get; set; }

        public string Id { get; set; }
        public string LabelType { get; set; }
    }
    /// <summary>
    /// 查询printdata对象
    /// </summary>
    public class MPrintDataQueryDto : PagerInfo
    {
        public string DataType { get; set; }
        public string DataSql { get; set; }
    }

    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MPrintDataDto
    {
        public string DataType { get; set; }

        public string Id { get; set; }

        public string DataSql { get; set; }

        public string InputParam { get; set; }

        public string InputField { get; set; }

        public string OutputParam { get; set; }

        public string DataSql2 { get; set; }

        public string InputParam2 { get; set; }

        public string InputField2 { get; set; }

        public string OutputParam2 { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        public string Site { get; set; }

        public string DataOrder { get; set; }

    }

    /// <summary>
    /// 查询对象
    /// </summary>
    public class MStationtypeLabelQueryDto : PagerInfo
    {
        public string Model { get; set; }
        public string Ipn { get; set; }
        public string LabelType { get; set; }
        public string StationType { get; set; }
        public string Enabled { get; set; }
        public string Site { get; set; }
    }

    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MStationtypeLabelDto
    {
        public string PrinterName { get; set; }

        public string LabelParams { get; set; }

        public string LabelDlUrl { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        public string Plant { get; set; }

        public string Site { get; set; }

        [Required(ErrorMessage = "Id不能为空")]
        public string Id { get; set; }

        public string LabelDesc { get; set; }

        public string Model { get; set; }

        public string Ipn { get; set; }

        public string StationType { get; set; }

        public string LabelType { get; set; }

        public string LabelName { get; set; }

        public string LabelSrvIp { get; set; }
    }
    /// <summary>
    /// 查询对象
    /// </summary>
    public class MStationtypeLabelParamsQueryDto : PagerInfo
    {
        public string Model { get; set; }
        public string LabelType { get; set; }
        public string FieldName { get; set; }
        public string VarType { get; set; }

    }
    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MStationtypeLabelParamsDto
    {
        public string Model { get; set; }

        public string Id { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        public string Ipn { get; set; }

        public string LabelType { get; set; }

        public string VarName { get; set; }

        public string VarType { get; set; }

        public string FieldName { get; set; }

        public string Description { get; set; }

        public string UpdateEmpno { get; set; }

        public string Plant { get; set; }

        public string Site { get; set; }

        public DateTime? UpdateTime { get; set; }

    }
    /// <summary>
    /// 输入模版信息
    /// </summary>
    public class MLabelTemplate
    {
        public string LabelName { get; set; }
        public string TemplateFile { get; set; }

    }

    }
