using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.M_PART")]
    public class ImesMpart
    {

        /// <summary>
        /// Id 
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// Ipn 
        /// </summary>
        public string ipn { get; set; }

        /// <summary>
        /// Apn 
        /// </summary>
        public string apn { get; set; }

        /// <summary>
        /// PartType 
        /// </summary>
        [SugarColumn(ColumnName = "pART_TYPE")]
        public string partType { get; set; }

        /// <summary>
        /// SPEC1 
        /// </summary>
        public string spec1 { get; set; }

        /// <summary>
        /// SPEC2 
        /// </summary>
        public string spec2 { get; set; }

        /// <summary>
        /// Ppid 
        /// </summary>
        public string ppid { get; set; }

        /// <summary>
        /// Equivalentpn 
        /// </summary>
        public string equivalentpn { get; set; }

        /// <summary>
        /// Materialtype 
        /// </summary>
        public string materialtype { get; set; }

        /// <summary>
        /// PRODUCTDESCCONFIG1 
        /// </summary>
        public string productdescconfig1 { get; set; }

        /// <summary>
        /// PRODUCTDESCCONFIG2 
        /// </summary>
        public string productdescconfig2 { get; set; }

        /// <summary>
        /// PRODUCTDESCCOLOR1 
        /// </summary>
        public string productdesccolor1 { get; set; }

        /// <summary>
        /// PRODUCTDESCCOLOR2 
        /// </summary>
        public string productdesccolor2 { get; set; }

        /// <summary>
        /// Radio 
        /// </summary>
        public string radio { get; set; }

        /// <summary>
        /// Region 
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// Packingtype 
        /// </summary>
        public string packingtype { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string updateEmpno { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? updateTime { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMPNO")]
        public string createEmpno { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? createTime { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string enabled { get; set; }

        /// <summary>
        /// OPTION1 
        /// </summary>
        public string  option1 { get; set; }

        /// <summary>
        /// OPTION2 
        /// </summary>
        public string option2 { get; set; }

        /// <summary>
        /// OPTION3 
        /// </summary>
        public string option3 { get; set; }

        /// <summary>
        /// OPTION4 
        /// </summary>
        public string option4 { get; set; }

        /// <summary>
        /// Version 
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// Model 
        /// </summary>
        public string model { get; set; }

        /// <summary>
        /// ModelCustomer 
        /// </summary>
        [SugarColumn(ColumnName = "mODEL_CUSTOMER")]
        public string modelCustomer { get; set; }

        /// <summary>
        /// ModelNo 
        /// </summary>
        [SugarColumn(ColumnName = "mODEL_NO")]
        public string modelNo { get; set; }

        /// <summary>
        /// Eeee 
        /// </summary>
        public string eeee { get; set; }

        /// <summary>
        /// Config 
        /// </summary>
        public string config { get; set; }

        /// <summary>
        /// Vendor 
        /// </summary>
        public string vendor { get; set; }

        /// <summary>
        /// Mpn 
        /// </summary>
        public string mpn { get; set; }

        /// <summary>
        /// Plant 
        /// </summary>
        public string plant { get; set; }

        /// <summary>
        /// Backflush 
        /// </summary>
        public string backflush { get; set; }

        /// <summary>
        /// Calloff 
        /// </summary>
        public string calloff { get; set; }

        /// <summary>
        /// Procurement 
        /// </summary>
        public string procurement { get; set; }

        /// <summary>
        /// Procurementdesc 
        /// </summary>
        public string procurementdesc { get; set; }

        /// <summary>
        /// Type 
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Typedesc 
        /// </summary>
        public string typedesc { get; set; }

        /// <summary>
        /// Inhouseproduction 
        /// </summary>
        public long? inhouseproduction { get; set; }

        /// <summary>
        /// Plandeliveryday 
        /// </summary>
        public long? plandeliveryday { get; set; }

        /// <summary>
        /// Mrpcontrol 
        /// </summary>
        public string mrpcontrol { get; set; }

        /// <summary>
        /// Priceunit 
        /// </summary>
        public long? priceunit { get; set; }

        /// <summary>
        /// Price 
        /// </summary>
        public long? price { get; set; }

        /// <summary>
        /// Currency 
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// Pricefrom 
        /// </summary>
        public string pricefrom { get; set; }

        /// <summary>
        /// Profitcenter 
        /// </summary>
        public string profitcenter { get; set; }

        /// <summary>
        /// Materialgroup 
        /// </summary>
        public string materialgroup { get; set; }

        /// <summary>
        /// Category 
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// Rohs 
        /// </summary>
        public string rohs { get; set; }

        /// <summary>
        /// Dayns 
        /// </summary>
        public string dayns { get; set; }

        /// <summary>
        /// Assession 
        /// </summary>
        public string assession { get; set; }

        /// <summary>
        /// Customergroup 
        /// </summary>
        public string customergroup { get; set; }

        /// <summary>
        /// Division 
        /// </summary>
        public string division { get; set; }

        /// <summary>
        /// Deleteflag 
        /// </summary>
        public string deleteflag { get; set; }

        /// <summary>
        /// Generaldesc 
        /// </summary>
        public string generaldesc { get; set; }

        /// <summary>
        /// Bondedflag 
        /// </summary>
        public string bondedflag { get; set; }

        /// <summary>
        /// Holdflag 
        /// </summary>
        public string holdflag { get; set; }

        /// <summary>
        /// Modelfamily 
        /// </summary>
        public string modelfamily { get; set; }

        /// <summary>
        /// Customermodelname 
        /// </summary>
        public string customermodelname { get; set; }

        /// <summary>
        /// Labelcpn 
        /// </summary>
        public string labelcpn { get; set; }

        /// <summary>
        /// Upc 
        /// </summary>
        public string upc { get; set; }

        /// <summary>
        /// Ean 
        /// </summary>
        public string ean { get; set; }

        /// <summary>
        /// Jan 
        /// </summary>
        public string jan { get; set; }

        /// <summary>
        /// Lastupdate 
        /// </summary>
        public string lastupdate { get; set; }

        /// <summary>
        /// Prdindicate 
        /// </summary>
        public string prdindicate { get; set; }

        /// <summary>
        /// Baseunitmeasure 
        /// </summary>
        public string baseunitmeasure { get; set; }

        /// <summary>
        /// OPTION5 
        /// </summary>
        public string option5 { get; set; }

        /// <summary>
        /// OPTION6 
        /// </summary>
        public string option6 { get; set; }

        /// <summary>
        /// Allierevision 
        /// </summary>
        public string allierevision { get; set; }

        /// <summary>
        /// OPTION21 
        /// </summary>
        public string option21 { get; set; }

       /* /// <summary>
        /// MaterialClassify 
        /// </summary>
        [SugarColumn(ColumnName = "mATERIAL_CLASSIFY")]
        public string MaterialClassify { get; set; }
*/
        /// <summary>
        /// Site 
        /// </summary>
        public string site { get; set; }




    }
}
