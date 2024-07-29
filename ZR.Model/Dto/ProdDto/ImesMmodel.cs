using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.m_model","机种表")]
    public class ImesMmodel
    {
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public int ID { get; set; }

        [SugarColumn(ColumnName = "MODEL", IsPrimaryKey = false)]
        public string Model { get; set; }

        [SugarColumn(ColumnName = "MODEL_CUSTOMER", IsPrimaryKey = false)]
        public string ModelCustomer { get; set; }

        [SugarColumn(ColumnName = "MODEL_NO", IsPrimaryKey = false)]
        public string ModelNo { get; set; }

        [SugarColumn(ColumnName = "MODEL_DESC", IsPrimaryKey = false)]
        public string ModelDesc { get; set; }

        [SugarColumn(ColumnName = "UPDATE_EMPNO", IsPrimaryKey = false)]
        public string UpdateEmpno { get; set; }

        [SugarColumn(ColumnName = "UPDATE_TIME", IsPrimaryKey = false)]
        public DateTime? UpdateTime { get; set; }//时间

        [SugarColumn(ColumnName = "CREATE_EMPNO", IsPrimaryKey = false)]
        public string CreateEmpno { get; set; }

        [SugarColumn(ColumnName = "CREATE_TIME", IsPrimaryKey = false)]
        public DateTime? CreateTime { get; set; }//时间

        [SugarColumn(ColumnName = "ENABLED", IsPrimaryKey = false)]
        public string EnaBled { get; set; }

        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = false)]
        public string site { get; set; }

    }
}
