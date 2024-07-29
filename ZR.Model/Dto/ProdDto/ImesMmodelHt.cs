using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.m_model_ht", "机种备份表")]
    public class ImesMmodelHt
    {
            [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
            public int ID { get; set; }

            [SugarColumn(ColumnName = "MODEL", IsPrimaryKey = true)]
            public string Model { get; set; }

            [SugarColumn(ColumnName = "MODEL_CUSTOMER", IsPrimaryKey = true)]
            public string ModelCustomer { get; set; }

            [SugarColumn(ColumnName = "MODEL_NO", IsPrimaryKey = true)]
            public string ModelNo { get; set; }

            [SugarColumn(ColumnName = "MODEL_DESC", IsPrimaryKey = true)]
            public string ModelDesc { get; set; }

            [SugarColumn(ColumnName = "UPDATE_EMPNO", IsPrimaryKey = true)]
            public string UpdateEmpno { get; set; }

            [SugarColumn(ColumnName = "UPDATE_TIME", IsPrimaryKey = true)]
            public DateTime? UpdateTime { get; set; }//时间

            [SugarColumn(ColumnName = "CREATE_EMPNO", IsPrimaryKey = true)]
            public string CreateEmpno { get; set; }

            [SugarColumn(ColumnName = "CREATE_TIME", IsPrimaryKey = true)]
            public DateTime? CreateTime { get; set; }//时间

            [SugarColumn(ColumnName = "ENABLED", IsPrimaryKey = true)]
            public string EnaBled { get; set; }

            [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]
            public string site { get; set; }


        
    }
}
