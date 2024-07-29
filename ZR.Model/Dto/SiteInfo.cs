using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    [SugarTable("SAJET.M_SITE", "厂区维护表")]

    public class SiteInfo
    {

        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public int id { get; set; }

        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]
        public string site { get; set; }
        
       [SugarColumn(ColumnName = "SITE_CUSTOMER", IsPrimaryKey = true)]
        public string siteCustomer { get; set; }
         
       [SugarColumn(ColumnName = "SITE_DESC", IsPrimaryKey = true)]
        public string siteDesc { get; set; }

        [SugarColumn(ColumnName = "UPDATE_EMPNO", IsPrimaryKey = true)]
        public string updateEmpno { get; set; }

        [SugarColumn(ColumnName = "UPDATE_TIME", IsPrimaryKey = true)]
        public DateTime? updateTime { get; set; }

        [SugarColumn(ColumnName = "CREATE_EMPNO", IsPrimaryKey = true)]
        public string createEmpno { get; set; }

        [SugarColumn(ColumnName = "CREATE_TIME", IsPrimaryKey = true)]
        public DateTime? createTime { get; set; }

        [SugarColumn(ColumnName = "ENABLED", IsPrimaryKey = true)]
        public string enabled { get; set; }
        
        [SugarColumn(ColumnName = "PLANT", IsPrimaryKey = true)]
        public string plant { get; set; }



    }
}
