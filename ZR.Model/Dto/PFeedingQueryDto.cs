using System.ComponentModel.DataAnnotations;

namespace ZR.Model.Dto
{
    /// <summary>
    /// 上下料

    /// </summary>
    public class PFeedingQueryDto : PagerInfo
    {
        public string WorkOrder { get; set; }
        public string StationType { get; set; }

        public string Enabled { get; set; }

        public string Site { get; set; }
    }

    public class ResponstationInout
    {
        public ResponstationInout(string tres, string tmsg)
        {
            this.tres = tres;
            this.tmsg = tmsg;
        }

        public string tres { get; set; }
        public string tmsg { get; set; }
    }
    public class stationInout
    {
        public string Type { get; set; }
        public string ToolingNo { get; set; }
        public string MachineCode { get; set; }
        public string CreateEmpno { get; set; }
        public string Site { get; set; }
        public long OutPutQty { get; set; }
    }


    /// <summary>
    /// 上下料
    /// </summary>
    public class PFeedingDto
    {



        ///// <summary>
        ///// InputQty 
        ///// </summary>
        public long? InputQty { get; set; }

        public string LoadingStatus { get; set; }

        public string ScanType { get; set; }

        public string Stage { get; set; }

        public string StationType { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? Delflag { get; set; }

        public string Site { get; set; }

        public string ScanIpn { get; set; }

        public string CreateEmpno { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

      
        public long Id { get; set; }

        public string WorkOrder { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? Targetqty { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? QuantityAlreadyInvested { get; set; }

        public string MachineCode { get; set; }

        public string MachineType { get; set; }

        public string MachineLoc { get; set; }

        public string Batchno { get; set; }

        public string ItemIpn { get; set; }


        public string PartType { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? PartCount { get; set; }

        public string LineId { get; set; }



    }
}