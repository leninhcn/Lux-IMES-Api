//using System.ComponentModel.DataAnnotations;

//namespace ZR.Model.Dto
//{

//    /// <summary>
//    /// 输入输出对象
//    /// </summary>
//    public class MStationTypeDto
//    {
//        [JsonConverter(typeof(ValueToStringConverter))]
//        public long Id { get; set; }

//        public string MaterialLoadFlag { get; set; }

//        [Required(ErrorMessage = "StationtypeCustomer不能为空")]
//        public string StationtypeCustomer { get; set; }

//        public string OperateType { get; set; }

//        public string ClientType { get; set; }

//        public string StationTypeSeq { get; set; }

//        [Required(ErrorMessage = "Stage不能为空")]
//        public string Stage { get; set; }

//        public string StationTypeDesc { get; set; }

//        public string CustomerStationDesc { get; set; }

//        public string UpdateEmpno { get; set; }

//        public DateTime? UpdateTime { get; set; }

//        public string CreateEmpno { get; set; }

//        public DateTime? CreateTime { get; set; }

//        public string Enabled { get; set; }

//        public string DcCmd { get; set; }

//        [Required(ErrorMessage = "Fpp不能为空")]
//        [JsonConverter(typeof(ValueToStringConverter))]
//        public long Fpp { get; set; }

//        [JsonConverter(typeof(ValueToStringConverter))]
//        public long? CurrentCt { get; set; }

//        [Required(ErrorMessage = "StationType不能为空")]
//        public string StationType { get; set; }



//    }
//}