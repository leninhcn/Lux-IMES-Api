using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Model.Repair.Dto
{
    public class RepairInDto
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string lblType { get; set; }
        /// <summary>
        /// 业务字段  SN
        /// </summary>
        public string sn { get; set; }

        public string lineName { get; set; }
        public string lineType { get; set; }
        public string stageName { set; get; }
        public string stationType { get; set; }
        public string stationName { get; set; }
        public string apName { set; get; }

    }

    public class repariDto : RepairInDto
    {
        public string partNo { get; set; }
        public string gsSN { get; set; }

        public string repairEmpNo { get; set; }

        public List<LVDefect> lvdef { get; set; }
    }

    public class RepairBaseInfo
    {
        public string wo {  get; set; }
        public string ipn { get; set; }
        public string lineName { get; set; }
        public string lineType { get; set; }
        public string stationType { get; set; }
        public string stationName { get; set; }
        public string defectDesc2 { get; set; }
        public string woType { get; set; }
        public string rpStatus { get; set; }

    }

    public class   repariDel: RepairBaseInfo
    {
        public string partNo { get; set; }
        public string processTime { get; set; }

        public List<lvkp> lvkps { get; set; }
       
    }
    public class repairDef : RepairInDto
    {
        public string wo { get; set; }

        public string partNo { get; set; }
        public string sDefCode { get; set; }
        public string sLocation { get; set; }

        
    }

    public class repairExe : RepairBaseInfo
    {
        public string wo { get; set; }
        public string partNo { get; set; }
        public string routName { get; set; }
        public string CurrentStationType { get; set; }
        public string sOutTime { get; set; }
        public string sRepairTime { get; set; }

        public string sRouteStep { get; set; }

        public List<LVDefect> lvdef { get; set; }

    }

    public class lvkp
    {
        public string ITEM_IPN { get; set; }
        public string SPEC1 { get; set; }
        public string ITEM_SN { get; set; }
        public string ITEM_SN_CUSTOMER { get; set; }
        public string STATION_TYPE { get; set; }
    }

    public class DATAItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string VSN { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string PLANT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DATAItem> DATA { get; set; }
    }
}
