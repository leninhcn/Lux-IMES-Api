using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_CUSTOMER", "客户表")]
    public class ImesmMcustomer
    {
        [SugarColumn(ColumnName = "ID")]
        public int id { get; set; }

        [SugarColumn(ColumnName = "CUSTOMER_CODE")]
        public string customerCode { get; set; }

        [SugarColumn(ColumnName = "CUSTOMER_NAME")]
        public string customerName { get; set; }

        [SugarColumn(ColumnName = "CUSTOMER_ADDR")]
        public string customerAddr { get; set; }

        [SugarColumn(ColumnName = "CUSTOMER_TEL")]
        public string customerTel { get; set; }

        [SugarColumn(ColumnName = "CUSTOMER_DESC")]
        public string customerDesc { get; set; }

        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string updateEmpno { get; set; }

        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public string createEmpno { get; set; }

        [SugarColumn(ColumnName = "site")]
        public string site { get; set; }

        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime? createTime { get; set; }

        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? updateTime { get; set; }//时间

    }
}
