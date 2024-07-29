using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{

        [SugarTable("SAJET.M_EMP")]
        public class ImesMemp
        {
            /// <summary>
            /// Id 
            /// </summary>
            [SugarColumn(ColumnName = "ID")]
            public long id { get; set; }

            /// <summary>
            /// 工号 
            /// </summary>
            [SugarColumn(ColumnName = "EMP_NO")]
            public string empNo { get; set; }

            /// <summary>
            /// 姓名 
            /// </summary>
            [SugarColumn(ColumnName = "EMP_NAME")]
            public string empName { get; set; }

            /// <summary>
            /// 密码 
            /// </summary>
            public string passwd { get; set; }

            /// <summary>
            /// 班别名称 
            /// </summary>
            [SugarColumn(ColumnName = "SHIFT_NAME")]
            public string shiftName { get; set; }

            /// <summary>
            /// 厂区 
            /// </summary>
            public string site { get; set; }

            /// <summary>
            /// 部门名称 
            /// </summary>
            [SugarColumn(ColumnName = "DEPT_NAME")]
            public string deptName { get; set; }

            /// <summary>
            /// 最近一次密码修改时间 
            /// </summary>
            [SugarColumn(ColumnName = "CHANGE_PW_TIME")]
            public DateTime? changePwTime { get; set; }

            /// <summary>
            /// Remark 
            /// </summary>
            [SugarColumn(ColumnName = "REMARK")]
            public string remark { get; set; }

            /// <summary>
            /// CreateTime 
            /// </summary>
            [SugarColumn(ColumnName = "CREATE_TIME")]
            public string createTime { get; set; }

            /// <summary>
            /// CreateEmpno 
            /// </summary>
            [SugarColumn(ColumnName = "CREATE_EMPNO")]
            public DateTime? createEmpno { get; set; }

            /// <summary>
            /// 修改人工号 
            /// </summary>
            [SugarColumn(ColumnName = "UPDATE_EMPNO")]
            public string updateEmpno { get; set; }

            /// <summary>
            /// 修改时间 
            /// </summary>
            [SugarColumn(ColumnName = "UPDATE_TIME")]
            public DateTime? updateTime { get; set; }

            /// <summary>
            /// 有效状态 
            /// </summary>
            [SugarColumn(ColumnName = "ENABLED")]
            public string enabled { get; set; }

            /// <summary>
            /// email 员工邮箱 
            /// </summary>
            [SugarColumn(ColumnName = "OPTION1")]
            public string option1 { get; set; }

            /// <summary>
            /// OPTION2 
            /// </summary>
            [SugarColumn(ColumnName = "OPTION2")]
            public string option2 { get; set; }

            /// <summary>
            /// OPTION3 
            /// </summary>
            [SugarColumn(ColumnName = "OPTION3")]
            public string option3 { get; set; }

            /// <summary>
            /// OPTION4 
            /// </summary>
            [SugarColumn(ColumnName = "OPTION4")]
            public string option4 { get; set; }

            /// <summary>
            /// OPTION5 
            /// </summary>
            [SugarColumn(ColumnName = "OPTION5")]
            public string option5 { get; set; }


            /// <summary>
            /// 该字段实体类不存在 
            /// </summary>
            [@SugarColumn(IsIgnore = true)]
            public int status
            {
                get; set;
            }
            /// <summary>
            /// 该字段实体类不存在 
            /// </summary>
            [@SugarColumn(IsIgnore = true)]
            public string authoritys
            {
                get; set;
            }
        }
    
}


