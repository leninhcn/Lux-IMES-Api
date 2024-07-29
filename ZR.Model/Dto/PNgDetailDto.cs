using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using ZR.Model.Business;

namespace ZR.Model.Dto
{
    /// <summary>
    /// 不良品统计查询对象
    /// </summary>
    public class PNgDetailQueryDto : PagerInfo
    {

        public string WorkOrder { get; set; }
        public string StationType { get; set; }

        public string Enabled { get; set; }

        public string Site { get; set; }
    }

    /// <summary>
    /// 不良品统计输入输出对象
    /// </summary>
    public class PNgDetailDto
    {


        public string MachineCode { get; set; }

        public string MachineNo { get; set; }
        
        public List<PNgDetail> DataList { get; set; }   

        public long? Id { get; set; }

        [SugarColumn(InsertSql = "Y")]
        public string Enabled { get; set; }

        public string Stage { get; set; }

        public string Ipn { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? Targetqty { get; set; }

        public string StationId { get; set; }

        public string StationName { get; set; }

        public string StationType { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? SamplingInspection { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? SamplingInspectionNg { get; set; }

        public string DefectCode { get; set; }

        public string BadDetail { get; set; }

        public string InsertionCode { get; set; }

        public string Operator { get; set; }

        public string HandlingMeasures { get; set; }


        public string Site { get; set; }

        public string CreateEmpno { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string LineId { get; set; }

        public string WorkOrder { get; set; }

        public string REMARK { get; set; }

    }
}