using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.Quality;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static ZR.Model.Dto.DataToWMS;

namespace ZR.Model.Dto
{
    public class DataToWMS
    {
        public class HData
        {
            public Head HEAD { get; set; }
            public List<Data> DATA { get; set; }
        }
        public class Head
        {
            public string MES_PALLET_ID { get; set; }
            public string CARTON_ID { get; set; }
            public string PLANT { get; set; }
            public string PN { get; set; }

            public string UNIT { get; set; }
            public string LINE { get; set; }
            public string CQTY { get; set; }
            public string WAREHOUSE_CODE { get; set; }
            public string STATUS { get; set; }
            public string FULL_QTY { get; set; }
        }
        public class Data
        {
            public string USN { get; set; }
            public string CARTON_ID { get; set; }
            public string PLANT { get; set; }
            public string MO { get; set; }
            public string STATUS { get; set; }
            public string QTY { get; set; }
        }
        public class SMessage
        {
            public List<Msg> MSG { get; set; }
        }
        public class Msg
        {
            public string MSGTY { get; set; }
            public string MSGTX { get; set; }
            public string WMSID { get; set; }
        }
        public class CancelCARTONDATA
        {
            ////public string MES_PALLET_ID { get; set; }
            public string CARTON_ID { get; set; }
            //public string USN { get; set; }

            public string PLANT { get; set; }
        }
        public class CancelPALLETDATA
        {
            public string MES_PALLET_ID { get; set; }

            public string PLANT { get; set; }
        }
        public class CancelUSNDATA
        {
            public string USN { get; set; }

            public string PLANT { get; set; }
        }
        public class HoldModel
        {
            public string hold_id { get; set; } = "";
            public string plant_code { get; set; }
            public string warehouse_code { get; set; }
            public string sn { get; set; }
            public string origin_system { get; set; }
            public string hold_flag { get; set; }
            public string hold_reasoncode { get; set; }
            public string hold_reasondesc { get; set; }
            public string status { get; set; } = "";
            public string status_reasoncode { get; set; } = "";
            public string create_by { get; set; }


        }
        public class HoldModelMSG
        {
            public List<HoldModel> DATA { get; set; }
            public string WMSID { get; set; }
            public string MSGTY { get; set; }
            public string MSGTX { get; set; }
        }
    }
 
}
