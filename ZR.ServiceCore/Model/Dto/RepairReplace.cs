using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.ServiceCore.Model.Dto
{
    public class RepairReplace
    {
        public string sn { get; set; }
        public string kpsn { get; set; }    
        public List<NewKeyPart> liKpNew { get; set; }
    }

    public class NewKeyPart
    {
        public string partType { get; set; }
        public string inputValue { get; set; }
        public string rid { get; set; }
        public string lc { get; set; }
        public string dc { get; set; }
    }

    public class RepReplaceDef
    {
        public string defectCode { get; set; }
        public List<defectDes> LVEC{ get; set; }
    }

    public class defectDes
    {
        public string defCode { get; set; }
        public string defDes { get; set; }
    }

    public class defReplaceKp
    {
        public string stationName { get; set; }
        public string sn { get; set; }
        public string defRecid { get; set; }
        public string kpsn { get; set; }
        public string remark { get; set;}

        public bool rdbtnYes { get; set; } = true;

        public List<LVKP> lvkp { get; set; }

        public List<NewKeyPart> liKpNew { get; set; }

        public List<defectDes> LVEC { get; set; }

    }

    public class LVKP
    {
        public string kpsn { get; set; }
        public string sn { get; set; }
        public string itemGroup { get; set; }
        public string customerSN { get; set; }
        public string workOrder { get; set; }
        public string stationType { get; set; }
        public string partType { get; set; }
    }
}
