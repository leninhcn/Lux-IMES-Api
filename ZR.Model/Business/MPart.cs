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
    [SugarTable("SAJET.M_PART")]
    public class MPart
    {
        /// <summary>
        /// Id 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Ipn 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// Apn 
        /// </summary>
        public string Apn { get; set; }

        /// <summary>
        /// PartType 
        /// </summary>
        [SugarColumn(ColumnName = "pART_TYPE")]
        public string PartType { get; set; }

        /// <summary>
        /// SPEC1 
        /// </summary>
        public string SPEC1 { get; set; }

        /// <summary>
        /// SPEC2 
        /// </summary>
        public string SPEC2 { get; set; }

        /// <summary>
        /// Ppid 
        /// </summary>
        public string Ppid { get; set; }

        /// <summary>
        /// Equivalentpn 
        /// </summary>
        public string Equivalentpn { get; set; }

        /// <summary>
        /// Materialtype 
        /// </summary>
        public string Materialtype { get; set; }

        /// <summary>
        /// PRODUCTDESCCONFIG1 
        /// </summary>
        public string PRODUCTDESCCONFIG1 { get; set; }

        /// <summary>
        /// PRODUCTDESCCONFIG2 
        /// </summary>
        public string PRODUCTDESCCONFIG2 { get; set; }

        /// <summary>
        /// PRODUCTDESCCOLOR1 
        /// </summary>
        public string PRODUCTDESCCOLOR1 { get; set; }

        /// <summary>
        /// PRODUCTDESCCOLOR2 
        /// </summary>
        public string PRODUCTDESCCOLOR2 { get; set; }

        /// <summary>
        /// Radio 
        /// </summary>
        public string Radio { get; set; }

        /// <summary>
        /// Region 
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Packingtype 
        /// </summary>
        public string Packingtype { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

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
        /// OPTION1 
        /// </summary>
        public string OPTION1 { get; set; }

        /// <summary>
        /// OPTION2 
        /// </summary>
        public string OPTION2 { get; set; }

        /// <summary>
        /// OPTION3 
        /// </summary>
        public string OPTION3 { get; set; }

        /// <summary>
        /// OPTION4 
        /// </summary>
        public string OPTION4 { get; set; }

        /// <summary>
        /// Version 
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Model 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// ModelCustomer 
        /// </summary>
        [SugarColumn(ColumnName = "mODEL_CUSTOMER")]
        public string ModelCustomer { get; set; }

        /// <summary>
        /// ModelNo 
        /// </summary>
        [SugarColumn(ColumnName = "mODEL_NO")]
        public string ModelNo { get; set; }

        /// <summary>
        /// Eeee 
        /// </summary>
        public string Eeee { get; set; }

        /// <summary>
        /// Config 
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// Vendor 
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// Mpn 
        /// </summary>
        public string Mpn { get; set; }

        /// <summary>
        /// Plant 
        /// </summary>
        public string Plant { get; set; }

        /// <summary>
        /// Backflush 
        /// </summary>
        public string Backflush { get; set; }

        /// <summary>
        /// Calloff 
        /// </summary>
        public string Calloff { get; set; }

        /// <summary>
        /// Procurement 
        /// </summary>
        public string Procurement { get; set; }

        /// <summary>
        /// Procurementdesc 
        /// </summary>
        public string Procurementdesc { get; set; }

        /// <summary>
        /// Type 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Typedesc 
        /// </summary>
        public string Typedesc { get; set; }

        /// <summary>
        /// Inhouseproduction 
        /// </summary>
        public long? Inhouseproduction { get; set; }

        /// <summary>
        /// Plandeliveryday 
        /// </summary>
        public long? Plandeliveryday { get; set; }

        /// <summary>
        /// Mrpcontrol 
        /// </summary>
        public string Mrpcontrol { get; set; }

        /// <summary>
        /// Priceunit 
        /// </summary>
        public long? Priceunit { get; set; }

        /// <summary>
        /// Price 
        /// </summary>
        public long? Price { get; set; }

        /// <summary>
        /// Currency 
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Pricefrom 
        /// </summary>
        public string Pricefrom { get; set; }

        /// <summary>
        /// Profitcenter 
        /// </summary>
        public string Profitcenter { get; set; }

        /// <summary>
        /// Materialgroup 
        /// </summary>
        public string Materialgroup { get; set; }

        /// <summary>
        /// Category 
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Rohs 
        /// </summary>
        public string Rohs { get; set; }

        /// <summary>
        /// Dayns 
        /// </summary>
        public string Dayns { get; set; }

        /// <summary>
        /// Assession 
        /// </summary>
        public string Assession { get; set; }

        /// <summary>
        /// Customergroup 
        /// </summary>
        public string Customergroup { get; set; }

        /// <summary>
        /// Division 
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// Deleteflag 
        /// </summary>
        public string Deleteflag { get; set; }

        /// <summary>
        /// Generaldesc 
        /// </summary>
        public string Generaldesc { get; set; }

        /// <summary>
        /// Bondedflag 
        /// </summary>
        public string Bondedflag { get; set; }

        /// <summary>
        /// Holdflag 
        /// </summary>
        public string Holdflag { get; set; }

        /// <summary>
        /// Modelfamily 
        /// </summary>
        public string Modelfamily { get; set; }

        /// <summary>
        /// Customermodelname 
        /// </summary>
        public string Customermodelname { get; set; }

        /// <summary>
        /// Labelcpn 
        /// </summary>
        public string Labelcpn { get; set; }

        /// <summary>
        /// Upc 
        /// </summary>
        public string Upc { get; set; }

        /// <summary>
        /// Ean 
        /// </summary>
        public string Ean { get; set; }

        /// <summary>
        /// Jan 
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// Lastupdate 
        /// </summary>
        public string Lastupdate { get; set; }

        /// <summary>
        /// Prdindicate 
        /// </summary>
        public string Prdindicate { get; set; }

        /// <summary>
        /// Baseunitmeasure 
        /// </summary>
        public string Baseunitmeasure { get; set; }

        /// <summary>
        /// OPTION5 
        /// </summary>
        public string OPTION5 { get; set; }

        /// <summary>
        /// OPTION6 
        /// </summary>
        public string OPTION6 { get; set; }

        /// <summary>
        /// Allierevision 
        /// </summary>
        public string Allierevision { get; set; }

        /// <summary>
        /// OPTION21 
        /// </summary>
        public string OPTION21 { get; set; }
        /// <summary>
        /// MaterialClassify 
        /// </summary>
        [SugarColumn(ColumnName = "mATERIAL_CLASSIFY")]
        public string MaterialClassify { get; set; }
        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Progrp 
        /// </summary>
        [SugarColumn(ColumnName = "PROGRP")]
        public string Progrp { get; set; }

    }
}

