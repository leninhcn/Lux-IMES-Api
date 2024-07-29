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
    /// 查询IPN料号
    /// </summary>
    public class MesGetPart 
    {
        public string Model { get; set; }
        public string Ipn { get; set; }
        public string Site {  get; set; }
    }
    /// <summary>
    /// 检查warehouse
    /// </summary>
    public class MesCheckWareHouse
    {
        public string WarehouseCode { get; set; }
        public string LocationCode { get; set; }
        public string Site { get; set; }
    }
    /// <summary>
    /// 获取EMP
    /// </summary>
    public class MesGetEMP
    {
        public string EMP_NAME { get; set; }
        public string EMP_NO { get; set; }
        public string Site { get; set; }
    }
    /// <summary>
    /// 输入输出机种model
    /// </summary>
    public class MesGetModel
    {
        public string Model { get; set; }
        public string Site { get; set; }

    }
    /// <summary>
    /// 输入输出打印变量名
    /// </summary>
    public class MesGetPrintField
    {
        public string DataType { get; set; }
        public string Site { get; set; }
    }

    /// <summary>
    /// 输入输出labeltype
    /// </summary>
    public class MesGetLabelType
    {
        public string Id { get; set; }
        public string Model { get; set; }
        public string Ipn { get; set; }
        public string LabelType { get; set; }
        public string Site { get; set; }
    }
    /// <summary>
    /// 输入输出labeltypebase
    /// </summary>
    public class MesGetLabelTypeBase
    {
        public string TypeName { get; set; }
        public string Site { get; set; }
    }
    /// <summary>
    /// 输入输出labelstationtype
    /// </summary>
    public class MesGetLabelStationType
    {
        public string StationType { get; set; }
        public string Site { get; set; }
    }
    /// <summary>
    /// 输入查询SN信息
    /// </summary>
    public class MesGetSNInfo
    {
        public string WorkOrder { get; set; }
        public string SerialNumber { get; set; }
        public string PanelNo { get; set; }
        public string CartonNo { get; set; }
        public string PalletNo { get; set; }
        public string Site { get; set; }
    }
}
