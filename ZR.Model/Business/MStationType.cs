using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZR.Model.Business
{


    /// <summary>
    /// 查询对象
    /// </summary>
    public class MStationTypeQueryDto : PagerInfo
    {
        //public string WorkOrder { get; set; }

        public string StationType { get; set; }

        public string Enabled { get; set; }

        public string Site { get; set; }

    }
    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MStationTypeDto
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string MaterialLoadFlag { get; set; }

        [Required(ErrorMessage = "StationtypeCustomer不能为空")]
        public string StationtypeCustomer { get; set; }

        public string OperateType { get; set; }

        public string ClientType { get; set; }

        public string StationTypeSeq { get; set; }

        [Required(ErrorMessage = "Stage不能为空")]
        public string Stage { get; set; }

        public string StationTypeDesc { get; set; }

        public string CustomerStationDesc { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Enabled { get; set; }

        public string DcCmd { get; set; }

        [Required(ErrorMessage = "Fpp不能为空")]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Fpp { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? CurrentCt { get; set; }

        [Required(ErrorMessage = "StationType不能为空")]
        public string StationType { get; set; }



    }
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("SAJET.M_STATION_TYPE")]
    public class MStationType
    {
        /// <summary>
        /// Id 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// StationtypeCustomer 
        /// </summary>
        [SugarColumn(ColumnName = "sTATIONTYPE_CUSTOMER")]
        public string StationtypeCustomer { get; set; }

        /// <summary>
        /// OperateType 
        /// </summary>
        [SugarColumn(ColumnName = "oPERATE_TYPE")]
        public string OperateType { get; set; }

        /// <summary>
        /// ClientType 
        /// </summary>
        [SugarColumn(ColumnName = "cLIENT_TYPE")]
        public string ClientType { get; set; }

        /// <summary>
        /// StationTypeSeq 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE_SEQ")]
        public string StationTypeSeq { get; set; }

        /// <summary>
        /// Stage 
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// StationTypeDesc 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE_DESC")]
        public string StationTypeDesc { get; set; }

        /// <summary>
        /// CustomerStationDesc 
        /// </summary>
        [SugarColumn(ColumnName = "cUSTOMER_STATION_DESC")]
        public string CustomerStationDesc { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMPNO")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// 厂区 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// DcCmd 
        /// </summary>
        [SugarColumn(ColumnName = "dC_CMD")]
        public string DcCmd { get; set; }

        /// <summary>
        /// Fpp 
        /// </summary>
        public long Fpp { get; set; }

        /// <summary>
        /// CurrentCt 
        /// </summary>
        [SugarColumn(ColumnName = "cURRENT_CT")]
        public long? CurrentCt { get; set; }

  

    }
}