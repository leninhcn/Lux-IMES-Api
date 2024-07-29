using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.S_PROGRAM_FUN_NAME")]
    public class ImesSprogramFunName
    {

        [SugarColumn(ColumnName = "ID")]
        public int id { get; set; }

        [SugarColumn(ColumnName = "PROGRAM")]
        public string program { get; set; }

        [SugarColumn(ColumnName = "FUNCTION")]
        public string function { get; set; }

        [SugarColumn(ColumnName = "DLL_FILENAME")]
        public string dllFilename { get; set; }

        [SugarColumn(ColumnName = "FUN_PARAM")]
        public string funParam { get; set; }

        [SugarColumn(ColumnName = "FUN_TYPE")]
        public string funType { get; set; }

        [SugarColumn(ColumnName = "ENABLED")]
        public string enabled { get; set; }
        
        [SugarColumn(ColumnName = "FORM_NAME")]
        public string formName { get; set; }

        [SugarColumn(ColumnName = "FUN_IDX")]
        public int funIdx { get; set; }

        [SugarColumn(ColumnName = "FUN_TYPE_IDX")]//用来做权限判断
        public int funTypeIdx { get; set; }

        [SugarColumn(ColumnName = "FUN_TYPE_CN")]
        public string funTypeCn { get; set; }

        [SugarColumn(ColumnName = "FUN_CN")]
        public string funCn { get; set; }

        [SugarColumn(ColumnName = "FUN_DESC_CN")]
        public string funDescCn { get; set; }

        [SugarColumn(ColumnName = "CREATEUSER")]
        public string createuser { get; set; }

        [SugarColumn(ColumnName = "UPDATEUSER")]
        public string updateuser { get; set; }

        [SugarColumn(ColumnName = "CREATETIME")]
        public DateTime? createTime { get; set; }

        [SugarColumn(ColumnName = "UPDATETIME")]
        public DateTime? updateTime { get; set; }//时间


        [@SugarColumn(IsIgnore = true)]
        public int status {
            get; set;
        }

    }
}
