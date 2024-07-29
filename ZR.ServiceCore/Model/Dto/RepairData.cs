using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Repair.Dto;
using static System.Net.Mime.MediaTypeNames;

namespace ZR.ServiceCore.Model.Dto
{

    public class RepairDataDto
    {
     public repairRes repairRes { get; set; }
      public string gSn { get; set; }
      public string reasonCode { get; set; }
      public string reasonType { get; set; }
      public string remark{ get; set; }

      public string casetype { get; set; }

      public string comStation { get; set; }
      public string defectsSym { get; set; }

      public  List<RepDetailData> repLiData { get; set; }
    }

    public class  repairRes: RepairInDto
    {
        public string wo { get; set; }
        public string partNo { get; set; }

        public string defectRecID { get; set; }
    }

    public class RepDetailData: RepairData
    {
        public string oldcsn { get; set; }
        public string newcsn { get; set; }
        public string location { get; set; }
        public string action { get; set; }

        public string oldcpn { get; set; }
        public string dateCode { get; set; }
        public string lotCode { get; set; }
        public string vendor { get; set; }

        public string oldreel { get; set; }
        public string remark { get; set; }

        public string remark1 { get; set; }
    }

    public class RepairData
    {
        public string  newCPN { get; set; }
        public string  dateCode1 { get; set; }
        public string lotCode1 { get; set; }
        public string  vendor1 { get; set; }
        public string  newReel { get; set; }

    }

    public class resRepairInfo
    {
        public string reasonCode { get; set; }
        public string reasonDesc { get; set; }
        public string reasonDesc1 { get; set; }
    }

    public class RepairInfo
    {
        private string sn;
        private string casetype;
        private string station;
        private string remark;
        private string defectsSym;
        private List<RepairDetail> repairDetails;

        public string Sn
        {
            get
            {
                return sn;
            }

            set
            {
                sn = value;
            }
        }

        public string Casetype
        {
            get
            {
                return casetype;
            }

            set
            {
                casetype = value;
            }
        }

        public string Station
        {
            get
            {
                return station;
            }

            set
            {
                station = value;
            }
        }

        public string Remark
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
            }
        }

        public string DefectsSym
        {
            get
            {
                return defectsSym;
            }

            set
            {
                defectsSym = value;
            }
        }

        public List<RepairDetail> RepairDetails
        {
            get
            {
                return repairDetails;
            }

            set
            {
                repairDetails = value;
            }
        }
    }

    public class RepairDetail
    {
        private string action;
        private string location;

        private string old_csn;
        private string new_csn;

        private string old_cpn;
        private string old_reel;
        private string vendor;
        private string datecode;
        private string lotcode;

        private string new_cpn;
        private string new_reel;
        private string vendor1;
        private string datecode1;
        private string lotcode1;

        private string remark;
        private string remark1;

        public string Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value;
            }
        }
        public string Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }
        public string Old_Csn
        {
            get
            {
                return old_csn;
            }

            set
            {
                old_csn = value;
            }
        }

        public string New_Csn
        {
            get
            {
                return new_csn;
            }

            set
            {
                new_csn = value;
            }
        }

        public string Old_Cpn
        {
            get
            {
                return old_cpn;
            }

            set
            {
                old_cpn = value;
            }
        }

        public string Old_Reel
        {
            get
            {
                return old_reel;
            }

            set
            {
                old_reel = value;
            }
        }


        public string Vendor
        {
            get
            {
                return vendor;
            }

            set
            {
                vendor = value;
            }
        }

        public string Datecode
        {
            get
            {
                return datecode;
            }

            set
            {
                datecode = value;
            }
        }

        public string Lotcode
        {
            get
            {
                return lotcode;
            }

            set
            {
                lotcode = value;
            }
        }
        public string New_Cpn
        {
            get
            {
                return new_cpn;
            }

            set
            {
                new_cpn = value;
            }
        }

        public string New_Reel
        {
            get
            {
                return new_reel;
            }

            set
            {
                new_reel = value;
            }
        }

        public string Vendor1
        {
            get
            {
                return vendor1;
            }

            set
            {
                vendor1 = value;
            }
        }

        public string Datecode1
        {
            get
            {
                return datecode1;
            }

            set
            {
                datecode1 = value;
            }
        }

        public string Lotcode1
        {
            get
            {
                return lotcode1;
            }

            set
            {
                lotcode1 = value;
            }
        }


        public string Remark
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
            }
        }
        public string Remark1
        {
            get
            {
                return remark1;
            }

            set
            {
                remark1 = value;
            }
        }

    }


    public class RepairRemoveCHK
    {
        public string gSn { get; set; }
        public string repairEmpNo { get; set; }
        public List<LVDefect> lvDef { get; set; }
    }
}
