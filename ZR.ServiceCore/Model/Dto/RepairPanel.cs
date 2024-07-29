using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.ServiceCore.Model.Dto
{
    public class RepairPanel
    {
        public string nextStationType { get; set; }
        public string panel { get; set; }
    }

    public class panelDto
    {
        public string LineName { get; set; }
        public string StageName { get; set; }
        public string StationType { get; set; }
        public string StationName { get; set; }
        public string panel { get; set; }
        public string cboSN { get; set; }
    }

    public class panelDefect
    {
        public string wo { get; set;}

        public string partNo { get; set; }

        public bool btnRepair { get; set; } = true;

        public string defLine { get; set; }
        public string defProcess { get; set; }
        public string defTerminal { get; set; }

        public List<LVDefect> lVDefects { get; set; }
    }
}
