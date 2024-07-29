using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    /// <summary>
    /// 工单基础资料历史表
    /// </summary>
    [SugarTable("SAJET.P_WO_BASE_HT")]
    public class PWoBaseHt
    {
        /// <summary>
        /// WorkOrder 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_ORDER")]
        public string WorkOrder { get; set; }

        /// <summary>
        /// WoType 
        /// </summary>
        [SugarColumn(ColumnName = "wO_TYPE")]
        public string WoType { get; set; }

        /// <summary>
        /// Model 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Ipn 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// Version 
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// SPEC1 
        /// </summary>
        public string SPEC1 { get; set; }

        /// <summary>
        /// TargetQty 
        /// </summary>
        [SugarColumn(ColumnName = "tARGET_QTY")]
        public long? TargetQty { get; set; }

        /// <summary>
        /// InputQty 
        /// </summary>
        [SugarColumn(ColumnName = "iNPUT_QTY")]
        public long? InputQty { get; set; }

        /// <summary>
        /// OutputQty 
        /// </summary>
        [SugarColumn(ColumnName = "oUTPUT_QTY")]
        public long? OutputQty { get; set; }

        /// <summary>
        /// NgQty 
        /// </summary>
        [SugarColumn(ColumnName = "nG_QTY")]
        public long? NgQty { get; set; }

        /// <summary>
        /// ScrapQty 
        /// </summary>
        [SugarColumn(ColumnName = "sCRAP_QTY")]
        public long? ScrapQty { get; set; }

        /// <summary>
        /// WoCreateDate 
        /// </summary>
        [SugarColumn(ColumnName = "wO_CREATE_DATE")]
        public DateTime? WoCreateDate { get; set; }

        /// <summary>
        /// WoScheduleStartDate 
        /// </summary>
        [SugarColumn(ColumnName = "wO_SCHEDULE_START_DATE")]
        public DateTime? WoScheduleStartDate { get; set; }

        /// <summary>
        /// WoScheduleCloseDate 
        /// </summary>
        [SugarColumn(ColumnName = "wO_SCHEDULE_CLOSE_DATE")]
        public DateTime? WoScheduleCloseDate { get; set; }

        /// <summary>
        /// WoStartDate 
        /// </summary>
        [SugarColumn(ColumnName = "wO_START_DATE")]
        public DateTime? WoStartDate { get; set; }

        /// <summary>
        /// WoCloseDate 
        /// </summary>
        [SugarColumn(ColumnName = "wO_CLOSE_DATE")]
        public DateTime? WoCloseDate { get; set; }

        /// <summary>
        /// Line 
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// LineType 
        /// </summary>
        [SugarColumn(ColumnName = "lINE_TYPE")]
        public string LineType { get; set; }

        /// <summary>
        /// RouteName 
        /// </summary>
        [SugarColumn(ColumnName = "rOUTE_NAME")]
        public string RouteName { get; set; }

        /// <summary>
        /// StartStationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTART_STATION_TYPE")]
        public string StartStationType { get; set; }

        /// <summary>
        /// EndStationType 
        /// </summary>
        [SugarColumn(ColumnName = "eND_STATION_TYPE")]
        public string EndStationType { get; set; }

        /// <summary>
        /// SnRule 
        /// </summary>
        [SugarColumn(ColumnName = "sN_RULE")]
        public string SnRule { get; set; }

        /// <summary>
        /// CartonRule 
        /// </summary>
        [SugarColumn(ColumnName = "cARTON_RULE")]
        public string CartonRule { get; set; }

        /// <summary>
        /// BoxRule 
        /// </summary>
        [SugarColumn(ColumnName = "bOX_RULE")]
        public string BoxRule { get; set; }

        /// <summary>
        /// PalletRule 
        /// </summary>
        [SugarColumn(ColumnName = "pALLET_RULE")]
        public string PalletRule { get; set; }

        /// <summary>
        /// PkspecName 
        /// </summary>
        [SugarColumn(ColumnName = "pKSPEC_NAME")]
        public string PkspecName { get; set; }

        /// <summary>
        /// WorkFlag 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_FLAG")]
        public string WorkFlag { get; set; }

        /// <summary>
        /// WoStatus 
        /// </summary>
        [SugarColumn(ColumnName = "wO_STATUS")]
        public string WoStatus { get; set; }

        /// <summary>
        /// WoBuild 
        /// </summary>
        [SugarColumn(ColumnName = "wO_BUILD")]
        public string WoBuild { get; set; }

        /// <summary>
        /// WoConfig 
        /// </summary>
        [SugarColumn(ColumnName = "wO_CONFIG")]
        public string WoConfig { get; set; }

        /// <summary>
        /// WoPhsae 
        /// </summary>
        [SugarColumn(ColumnName = "wO_PHSAE")]
        public string WoPhsae { get; set; }

        /// <summary>
        /// WoVersion 
        /// </summary>
        [SugarColumn(ColumnName = "wO_VERSION")]
        public string WoVersion { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Remark 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// WarehouseNo 
        /// </summary>
        [SugarColumn(ColumnName = "wAREHOUSE_NO")]
        public string WarehouseNo { get; set; }

        /// <summary>
        /// WarehouseLocation 
        /// </summary>
        [SugarColumn(ColumnName = "wAREHOUSE_LOCATION")]
        public string WarehouseLocation { get; set; }

        /// <summary>
        /// DeptName 
        /// </summary>
        [SugarColumn(ColumnName = "dEPT_NAME")]
        public string DeptName { get; set; }

        /// <summary>
        /// PlantCode 
        /// </summary>
        [SugarColumn(ColumnName = "pLANT_CODE")]
        public string PlantCode { get; set; }

        /// <summary>
        /// Changeedflag 
        /// </summary>
        public string Changeedflag { get; set; }

        /// <summary>
        /// WorkCenter 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_CENTER")]
        public string WorkCenter { get; set; }

        /// <summary>
        /// WorkCenterPlant 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_CENTER_PLANT")]
        public string WorkCenterPlant { get; set; }

        /// <summary>
        /// Releasedate 
        /// </summary>
        public string Releasedate { get; set; }

        /// <summary>
        /// Seqid 
        /// </summary>
        public string Seqid { get; set; }

        /// <summary>
        /// Storageloc 
        /// </summary>
        public string Storageloc { get; set; }

        /// <summary>
        /// Priority 
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// Mfgpacktype 
        /// </summary>
        public string Mfgpacktype { get; set; }

        /// <summary>
        /// Customerdn 
        /// </summary>
        public string Customerdn { get; set; }

        /// <summary>
        /// Assignno 
        /// </summary>
        public string Assignno { get; set; }

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
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

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
        /// OPTION5 
        /// </summary>
        public string OPTION5 { get; set; }

        /// <summary>
        /// OPTION6 
        /// </summary>
        public string OPTION6 { get; set; }

        /// <summary>
        /// WoPurpose 
        /// </summary>
        [SugarColumn(ColumnName = "wO_PURPOSE")]
        public string WoPurpose { get; set; }

        /// <summary>
        /// RuleSetName 
        /// </summary>
        [SugarColumn(ColumnName = "rULE_SET_NAME")]
        public string RuleSetName { get; set; }

        /// <summary>
        /// MWo 
        /// </summary>
        [SugarColumn(ColumnName = "m_WO")]
        public string MWo { get; set; }

        /// <summary>
        /// BurninTime 
        /// </summary>
        [SugarColumn(ColumnName = "bURNIN_TIME")]
        public long? BurninTime { get; set; }

        /// <summary>
        /// CompanyName 
        /// </summary>
        [SugarColumn(ColumnName = "cOMPANY_NAME")]
        public string CompanyName { get; set; }

        /// <summary>
        /// EquipmentCode 
        /// </summary>
        [SugarColumn(ColumnName = "eQUIPMENT_CODE")]
        public string EquipmentCode { get; set; }

        /// <summary>
        /// RmaNo 
        /// </summary>
        [SugarColumn(ColumnName = "rMA_NO")]
        public string RmaNo { get; set; }

        /// <summary>
        /// RmaCustomer 
        /// </summary>
        [SugarColumn(ColumnName = "rMA_CUSTOMER")]
        public string RmaCustomer { get; set; }

        /// <summary>
        /// RmaAccountValue 
        /// </summary>
        [SugarColumn(ColumnName = "rMA_ACCOUNT_VALUE")]
        public string RmaAccountValue { get; set; }

        /// <summary>
        /// Plant 
        /// </summary>
        public string Plant { get; set; }

    }


}

