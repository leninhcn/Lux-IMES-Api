using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public class PWoBaseQueryDto : PagerInfo
    {
        public string WoStatus {  get; set; }
        public string Select { get; set; }
        public string Value { get; set; }
        public string Site {  get; set; }
    }

    public class MesAgvDto 
    {
        public string MachineCode { get; set; }
        public string ToolingNo { get; set; }
        public string Type { get; set; }
        public string Site { get; set; }
        public string EmpNo { get; set; }
    }

    /// <summary>
    /// 历史资料查询对象
    /// </summary>
    public class PWoBaseHTQueryDto 
    {
        public string WorkOrder { get; set; }
        public string Site { get; set; }
    }
    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class PWoBaseDto
    {
        public string WarehouseNo { get; set; }

        public string WarehouseLocation { get; set; }

        public string DeptName { get; set; }

        public string PlantCode { get; set; }

        public string Changeedflag { get; set; }

        public string WorkCenter { get; set; }

        public string WorkCenterPlant { get; set; }

        public string Releasedate { get; set; }

        public string Seqid { get; set; }

        public string Storageloc { get; set; }

        public string Priority { get; set; }

        public string Mfgpacktype { get; set; }

        public string Customerdn { get; set; }

        public string Assignno { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public string OPTION1 { get; set; }

        public string OPTION2 { get; set; }

        public string OPTION3 { get; set; }

        public string OPTION4 { get; set; }

        public string OPTION5 { get; set; }

        public string OPTION6 { get; set; }

        public string WoPurpose { get; set; }

        public string RuleSetName { get; set; }

        public string WorkOrder { get; set; }

        public string WoType { get; set; }

        public string Model { get; set; }

        public string Ipn { get; set; }

        public string Version { get; set; }

        public string SPEC1 { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? TargetQty { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? InputQty { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? OutputQty { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? NgQty { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? ScrapQty { get; set; }

        public DateTime? WoCreateDate { get; set; }

        public DateTime? WoScheduleStartDate { get; set; }

        public DateTime? WoScheduleCloseDate { get; set; }

        public DateTime? WoStartDate { get; set; }

        public DateTime? WoCloseDate { get; set; }

        public string Line { get; set; }

        public string LineType { get; set; }

        public string RouteName { get; set; }

        public string StartStationType { get; set; }

        public string EndStationType { get; set; }

        public string SnRule { get; set; }

        public string CartonRule { get; set; }

        public string BoxRule { get; set; }

        public string PalletRule { get; set; }

        public string PkspecName { get; set; }

        public string WorkFlag { get; set; }

        public string WoStatus { get; set; }

        public string WoBuild { get; set; }

        public string WoConfig { get; set; }

        public string WoPhsae { get; set; }

        public string WoVersion { get; set; }

        public string Site { get; set; }

        public string Remark { get; set; }

        public string MWo { get; set; }

        public long? BurninTime { get; set; }

        public string CompanyName { get; set; }

        public string EquipmentCode { get; set; }

        public string RmaNo { get; set; }

        public string RmaCustomer { get; set; }

        public string RmaAccountValue { get; set; }

        public string Plant { get; set; }



    }
}
