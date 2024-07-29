using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Tooling
{
    public class ToolingDefectVo
    {
        public string DefectCode {  get; set; }
        public string DefectDesc2 {  get; set; }
        public string RecId { get; set; }
        public string RpStatus { get; set; }
    }

    public class ToolingSnShowDataVo
    {
        public string ToolingType { get; set; }

        public string ToolingLastMaintainTime {  get; set; }
        public List<ToolingDefectVo> toolingDefectVos { get; set; }

    }
}
