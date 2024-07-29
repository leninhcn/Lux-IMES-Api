using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Tooling
{
    public class MToolingTypeDto
    {
        public long Id { get; set; }

        public string ToolingType { get; set; }

        public string ToolingTypeDesc { get; set; }

        public string Options { get; set; }

        public long LocationQty { get; set; }
    }
}
