using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.WorkOrder;

namespace ZR.Model.Dto.Machine
{
    public class MachineGroupToWoAssign
    {

        public long machineGroupId {  get; set; }
        public string machineGroupName { get; set; }
        public string stationType { get; set; }
        public string description { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class MachineToWoAssign
    {
        /// <summary>
        /// 
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string machine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string stationType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long machineGroupId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string machineGroupName { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class WoAssignVo
    {
        /// <summary>
        /// 
        /// </summary>
        public List<WoAssignmentDto> pendingList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MachineToWoAssign> confirmList { get; set; }
        /// <summary>
        /// 派工计划开始日期
        /// </summary>
        public string startDate {  get; set; }
        /// <summary>
        /// 派工计划结束日期
        /// </summary>
        public string endDate { get; set; }

        /// <summary>
        /// 操作类型，Assign or Modify
        /// </summary>
        public string operate { get; set; }


    }
}
