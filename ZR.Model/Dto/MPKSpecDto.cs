using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class MPKSpecDto
    {
        public long Id { get; set; }

        public string PkspecName { get; set; }

        public long? PalletQty { get; set; }

        public long? PalletSnQty { get; set; }

        public long? CartonQty { get; set; }

        public long? CartonSnQty { get; set; }

        public long? BoxQty { get; set; }

        public long? BoxSnQty { get; set; }

        public string Enabled { get; set; }
    }
}
