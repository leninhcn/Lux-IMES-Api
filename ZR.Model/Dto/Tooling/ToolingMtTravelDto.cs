using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Tooling
{
    public class ToolingMtTravelDto
    {
        
        public string ToolingSn { get; set; }

        /// <summary>
        /// UpdateEmpNo
        /// </summary>
        public string UpdateEmpNo { get; set; }

        /// <summary>
        /// Memo
        /// </summary>
        public string Memo { get; set; }


        /// <summary>
        /// UsedCount
        /// </summary>
        public long UsedCount { get; set; }

        /// <summary>
        /// MaintainItem
        /// </summary>
        public string MaintainItem { get; set; }

        /// <summary>
        /// MaintainResult
        /// </summary>
        public string MaintainResult { get; set; }

        /// <summary>
        /// TotalUsedCount
        /// </summary>
        public long TotalUsedCount { get; set; }

        /// <summary>
        /// Length
        /// </summary>
        public string Length { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// DamageDegree
        /// </summary>
        public string DamageDegree { get; set; }

        /// <summary>
        /// CleanDegree
        /// </summary>
        public string CleanDegree { get; set; }

        /// <summary>
        /// Rough
        /// </summary>
        public string Rough { get; set; }

        public string Scrape { get; set; }

        public string ViewCheck { get; set; }

        public char Enabled { get; set; }

        public string Site { get; set; }
    }
}
