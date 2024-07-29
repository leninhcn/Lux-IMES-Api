using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("SAJET.M_LABEL_TEMPLATE_FILE")]
    public class MLabelTemplateFile
    {
        /// <summary>
        /// LabelName 
        /// </summary>
        [SugarColumn(ColumnName = "lABEL_NAME")]
        public string LabelName { get; set; }

        /// <summary>
        /// TemplateFile 二进制数据
        /// </summary>
        [SugarColumn(ColumnName = "tEMPLATE_FILE")]
        public byte[] TemplateFile { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// CreateEmp 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMP")]
        public string CreateEmp { get; set; }

        /// <summary>
        /// FileContent 
        /// </summary>
        [SugarColumn(ColumnName = "fILE_CONTENT")]
        public string FileContent { get; set; }

    }
}