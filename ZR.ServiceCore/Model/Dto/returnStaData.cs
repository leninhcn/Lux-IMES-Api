using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.ServiceCore.Model.Dto
{
    public class returnStaData
    {
        /// <summary>
        /// 0:有效  1：无效
        /// </summary>
        public int Enabled { get; set; }
        public int SelectedIndex { get; set; }

        public string sFieldName { get; set; }

        public string sFilterText { get; set; }
    }

    public class  stationDetailData:  returnStaData
    {
        
        public string sId { get; set; }

        public int? detailSelectIndex { get; set; }

    }

    public class ModelInfo
    {
        string sn = "";
        public string SN
        {
            get { return sn; }
            set { sn = value; }
        }

        string model = "";
        public string MODEL
        {
            get { return model; }
            set { model = value; }
        }

        string category = "";
        public string CATEGORY
        {
            get { return category; }
            set { category = value; }
        }

        string total_material = "";
        public string TOTAL_MATERIAL
        {
            get { return total_material; }
            set { total_material = value; }
        }

        int kolkata;
        public int KOLKATA
        {
            get { return kolkata; }
            set { kolkata = value; }
        }

        string wo = "";
        public string WO
        {
            get { return wo; }
            set { wo = value; }
        }

        string id = "";
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        string sql = "";
        public string sSql
        {
            get { return sql; }
            set { sql = value; }
        }

        string eName = "";
        public string editName
        {
            get { return eName; }
            set { eName = value; }
        }

        string eDesc = "";
        public string editDesc
        {
            get { return eDesc; }
            set { eDesc = value; }
        }

        string enabled = "";
        public string Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        string type = "";
        /// <summary>
        /// APPEND MODIFY
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }

    public class DeleteData
    {
        public string sId { get; set; }
        public string sName { get; set; }
        public string sData { get; set; }
    }

    public class detailPartStation
    {
        public string sUpdateType { get; set; }
        public string sId { get; set; }
        public string returnStationType { get; set;}
        public string category { get; set; }
        public string unLinkmaterial { get; set; }
        public string kolkata { get; set; }
        public string model { get; set; }
        public string area { get; set; }
    }
}
