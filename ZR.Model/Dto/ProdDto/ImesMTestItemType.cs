using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.m_TEST_ITEM_TYPE")]
    public class ImesMTestItemType
    {
        [SugarColumn(ColumnName = "ITEM_TYPE_ID")]
        public int itemTypeid { get; set; }
        [SugarColumn(ColumnName = "ITEM_TYPE_NAME")]
        public string itemTypename { get; set; }
        [SugarColumn(ColumnName = "ITEM_TYPE_CODE")]
        public string itemTypecode { get; set; }
        [SugarColumn(ColumnName = "ITEM_TYPE_DESC")]
        public string itemTypedesc { get; set; }
        [SugarColumn(ColumnName = "UPDATE_EMP")]
        public string updateEmp { get; set; }
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime updateTime { get; set; }
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime createTime { get; set; }
        [SugarColumn(ColumnName = "ENABLED")]
        public string enaBled { get; set; }
        [SugarColumn(ColumnName = "SAMPLING_ID")]
        public int sampingId { get; set; }
        [SugarColumn(ColumnName = "ITEM_TYPE_DESC2")]
        public string itemTypedesc2 { get; set; }
        [SugarColumn(ColumnName = "MIN_INSP_QTY")]
        public int minInspqty { get; set; }
        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }
    }
}
