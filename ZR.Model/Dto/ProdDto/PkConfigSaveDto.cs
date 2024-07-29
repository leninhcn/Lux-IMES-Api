using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    public class PkBlockConfig
    {
        public string CreateType { get; set; }
        public string PrintMethod { get; set; }
        public string PrintPort { get; set; }
        public bool PrintLabel { get; set; }
        public int? PrintQty { get; set; }
    }
    

    public class PkConfigSaveDto
    {
        public string Station { get; set; }
        public PkBlockConfig CsnConfig { get; set; }
        public PkBlockConfig InnerBoxConfig { get; set; }
        public PkBlockConfig BoxConfig { get; set; }
        public PkBlockConfig CartonConfig { get; set; }
        public PkBlockConfig PalletConfig { get; set; }

        public bool WeightCarton { get; set; }
        public string WeightPort { get; set; }
        public string PackingBase { get; set; }
        public string PackingAction { get; set; }
        public string RuleFunction { get; set; }
        public bool CapsLock { get; set; }
        public bool InputDefect { get; set; }
        public bool RemoveCsn { get; set; }
    }
}
