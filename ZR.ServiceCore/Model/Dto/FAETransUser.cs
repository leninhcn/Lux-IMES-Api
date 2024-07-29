using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.ServiceCore.Model.Dto
{
    public class FAETransUser
    {
        public string sFieldName { get; set; }
        public string sFieldText { get; set; }
        public int Enable { get; set; }
    }

    public class FAETransInfo
    {
        /// <summary>
        /// APPEND MODIFY  DELETE DISABLE ENABLE
        /// </summary>
        public string Type { get; set; }
        public string ID { get; set; }
        //public int Idx { get; set; }
        //public string FilterName { get; set; }
        //public string FiledName { get; set; }
        public string Enabled { get; set; }
        //public string sSql { get; set; }

        public string EMP_NO { get; set; }
        public string EMP_NAME { get; set; }
        public string LAB { get; set; }
        public string PHONE_NO { get; set; }
    }
}
