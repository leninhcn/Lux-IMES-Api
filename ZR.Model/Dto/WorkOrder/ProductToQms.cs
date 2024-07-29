using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.WorkOrder
{
    public class ProductToQms
    {
        public string Werks { get; set; }
        public string LotNO { get; set; }
        public string ProdLine { get; set; }
        public string PlanProdDate { get; set; }
        public string WorkOrder { get; set; }
        public string MaterialNo { get; set; }
        public string Line { get; set; }
        public decimal PlanProdQty { get; set; }
        public string Zqty { get; set; }
        public string CallbackPostAPI { get; set; }
        public string MODIFYBY { get; set; }
        public string MODIFYTIME { get; set; }
        public string MODIFYBYNAME { get; set; }
        public string DRAWNUM { get; set; }
        public string DRAWNUM_VERSION { get; set; }
    }
}
