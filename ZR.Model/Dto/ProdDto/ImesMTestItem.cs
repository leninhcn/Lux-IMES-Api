using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.m_TEST_ITEM")]

    public class ImesMTestItem
    {
        [SugarColumn(ColumnName = "ITEM_TYPE_ID")]
        public int itemTypeid { get; set; }
        [SugarColumn(ColumnName = "ITEM_ID")]
        public int itemId { get; set; }
        [SugarColumn(ColumnName = "ITEM_NAME")]
        public string itemName { get; set; }
        [SugarColumn(ColumnName = "ITEM_CODE")]
        public string itemCode { get; set; }
        [SugarColumn(ColumnName = "ITEM_DESC")]
        public string itemDesc { get; set; }
        [SugarColumn(ColumnName = "UPDATE_EMP")]
        public string updateEmp { get; set; }
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime updateTime { get; set; }
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime createTime { get; set; }
        [SugarColumn(ColumnName = "ENABLED")]
        public string enaBled { get; set; }
        [SugarColumn(ColumnName = "HAS_VALUE")]
        public string hasValue { get; set; }
        [SugarColumn(ColumnName = "ITEM_DESC2")]
        public string itemDesc2 { get; set; }
        [SugarColumn(ColumnName = "VALUE_TYPE")]
        public string valueType { get; set; }
        [SugarColumn(ColumnName = "SITE")]
        public string site { get; set; }
    }
}
