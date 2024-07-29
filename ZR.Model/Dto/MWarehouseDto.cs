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
    /// 查询对象
    /// </summary>
    public class MWarehouseQueryDto : PagerInfo
    {
        public string WarehouseCode { get; set; }

        public string WarehouseName { get; set; }

        public string WarehouseType { get; set; }

        public string Enabled { get; set; }

    }

    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MWarehouseDto
    {
        public string WarehouseCode { get; set; }

        public string WarehouseName { get; set; }

        public string WarehouseType { get; set; }

        public string Enabled { get; set; }

        public string Site { get; set; }

        public string CreateEmp { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string UpdateEmp { get; set; }

        public DateTime? CreateTime { get; set; }
    }
    /// <summary>
    /// 查询对象
    /// </summary>
    public class MLocationQueryDto : PagerInfo
    {
        public string Enabled { get; set; }
        public string WarehouseCode { get; set; }
        public string LocationName { get; set; }
        public string LocationCode { get; set; }
        public string CurrentStatus { get; set; }
    }

    /// <summary>
    /// 输入输出对象
    /// </summary>
    public class MLocationDto
    {
        public string Enabled { get; set; }

        public DateTime? CreateTime { get; set; }

        public string CreateEmp { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string WarehouseCode { get; set; }

        public string Site { get; set; }

        public string LocationName { get; set; }

        public string LocationType { get; set; }

        public string UpdateEmp { get; set; }

        public string CurrentStatus { get; set; }
        public string LocationCode { get; set; }
    }
}
