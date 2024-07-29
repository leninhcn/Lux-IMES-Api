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
    /// 查询type
    /// </summary>
    public class MBlockConfigTypeQueryDto : PagerInfo
    {
        public string ConfigTypeId { get; set; }
        public string Enabled { get; set; }
        public string ConfigTypeName { get; set; }

        public string Site {  get; set; }
    }

    /// <summary>
    /// 查询value
    /// </summary>
    public class MBlockConfigValueQueryDto : PagerInfo
    {
        public string ConfigTypeId { get; set; }
        public string Enabled { get; set; }
        public string ConfigName { get; set; }

        public string Site { get; set; }
    }
    /// <summary>
    /// 查询sql
    /// </summary>
    public class MBlockConfigProsqlQueryDto : PagerInfo
    {
        public string ConfigTypeId { get; set; }
        public string Enabled { get; set; }
        public string ConfigName { get; set; }

        public string Site { get; set; }
    }
    /// <summary>
    /// 输入输出type对象
    /// </summary>
    public class MBlockConfigTypeDto
    {
        public string ConfigTypeId { get; set; }

        public string ConfigTypeName { get; set; }

        public string ConfigTypeDesc { get; set; }

        public string NeederEmpno { get; set; }

        public string Enabled { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string UpdateEmpno { get; set; }

        public  string Site { get;set; }
    }
    /// <summary>
    /// 输入输出value
    /// </summary>
    public class MBlockConfigValueDto
    {
        public string ConfigTypeId { get; set; }
        public string ConfigId { get; set; }

        public string Enabled { get; set; }

        public string ConfigName { get; set; }

        public string ConfigDesc { get; set; }

        public string ConfigValue { get; set; }

        public string RouteName { get; set; }

        public string Line { get; set; }

        public string StationType { get; set; }

        public string StationName { get; set; }

        public string ClientId { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Site { get; set; }
    }
    /// <summary>
    /// 输入输出sql
    /// </summary>
    public class MBlockConfigProsqlDto
    {
        public string ConfigTypeId { get; set; }
        public string ConfigId { get; set; }
        public string ConfigName { get; set; }

        public DateTime? CreateTime { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? ConfigSeq { get; set; }

        public string ConfigDesc { get; set; }

        public string StationType { get; set; }

        public string CheckType { get; set; }

        public string ConfigType { get; set; }

        public string ConfigProsql { get; set; }

        public string Two { get; set; }

        public string Tsn { get; set; }

        public string Tline { get; set; }

        public string TstationType { get; set; }

        public string TstationName { get; set; }

        public string TrouteName { get; set; }

        public string Csn { get; set; }

        public string Cwo { get; set; }

        public string Tempno { get; set; }

        public string Enabled { get; set; }

        public string TcartonNo { get; set; }

        public string TpalletNo { get; set; }

        public string Tkpsn { get; set; }

        public string TPARAM1 { get; set; }

        public string TPARAM2 { get; set; }

        public string TPARAM3 { get; set; }

        public string UpdateEmpno { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string CreateEmpno { get; set; }

        public string Site {  get; set; }

    }
}
