using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("SAJET.M_LABEL_SERVER")]
    public class MLabelServer
    {
        /// <summary>
        /// ServerIp 
        /// </summary>
        [SugarColumn(ColumnName = "sERVER_IP")]
        public string ServerIp { get; set; }

        /// <summary>
        /// ServerUser 
        /// </summary>
        [SugarColumn(ColumnName = "sERVER_USER")]
        public string ServerUser { get; set; }

        /// <summary>
        /// ServerPassword 
        /// </summary>
        [SugarColumn(ColumnName = "sERVER_PASSWORD")]
        public string ServerPassword { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMPNO")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

    }
}