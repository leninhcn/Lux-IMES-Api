using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.Quality;

namespace ZR.Model.Dto
{
    public class WareHousingDto
    {
        public string Site { get; set; }
        public string Cmbtype { get; set; }
        public string Warehousecode { get; set; }
        public string Locationcode { get; set; }
        public string Inputtype { get; set; }
        public string Qty { get; set; }
        public bool Sendwms { get; set; }
        public bool Checkwarelocation {  get; set; }
        public string Ipn { get; set; }
        public string Mark   { get; set; }
        public string Inputdata { get;set; }
        public string Stationname { get;set; }
        public string UpdateEmpno { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
    public class WareHousingResultDto
    {
        public string WORK_ORDER { get; set; }
        public string PART_NO { get; set; }
        public string PALLET_NO { get; set; }
        public string CARTON_NO { get; set; }
        public string SERIAL_NUMBER { get; set; }
        public string CUSTOMER_SN { get; set; }
        public string QTY { get; set; }
        public string FIXED_QTY { get; set; }
        public string SN_UNIT { get; set; }
    }
}
