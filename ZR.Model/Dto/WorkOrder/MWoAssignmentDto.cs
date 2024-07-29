using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.WorkOrder
{
    public class MWoAssignmentDto
    { 
        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrder { get; set; }


        /// <summary>
        /// 设备
        /// </summary>
        public string Machine { get; set; }

        /// <summary>
        /// 派工计划开始时间
        /// </summary>
        public DateTime? WoAssignStartDate { get; set; }

        /// <summary>
        /// 派工计划终止时间
        /// </summary>
        public DateTime? WoAssignEndDate { get; set; }
    }
}
