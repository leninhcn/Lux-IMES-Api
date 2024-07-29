using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.WorkOrder
{
    public class WoAssignmentFilter
    {
        /// <summary>
        /// 过滤条件 (WorkOrder,Ipn,StationType,TopWorkOrder,WoScheduleStartDate,WoScheduleCloseDate,WoAssingStartDate,WoAssingEndDate 之一)
        /// </summary>
        public string filterField {  get; set; }
        /// <summary>
        /// 筛选值（当过滤条件 为(WorkOrder,Ipn,StationType,TopWorkOrder）之一时，不能为空)
        /// </summary>
        public string filterValue { get; set; }
        /// <summary>
        /// 起始时间(格式：yyyy-MM-dd) 当过滤条件为（WoScheduleStartDate,WoScheduleCloseDate,WoAssingStartDate,WoAssingEndDate），不能为空
        /// </summary>
        public DateTime? startDate { get; set; }
        /// <summary>
        /// 终止时间(格式：yyyy-MM-dd) 当过滤条件为（WoScheduleStartDate,WoScheduleCloseDate,WoAssingStartDate,WoAssingEndDate），不能为空
        /// </summary>
        public DateTime? endDate { get; set; }

        /// <summary>
        /// AssignStatus 值只有三种1:ALL 2:Y 3:N
        /// </summary>
        public string AssignStatus { get; set;}

        /// <summary>
        /// WO_CREATE_DATE 工单创建日期 起始时间(格式：yyyy-MM-dd)
        /// </summary>
        public DateTime WoCreateDateStart { get; set; }
        /// <summary>
        /// WO_CREATE_DATE 工单创建日期 终止时间(格式：yyyy-MM-dd)
        /// </summary>
        public DateTime WoCreateDateEnd { get; set; }


        public WoAssignmentFilter()
        {
            this.filterField = "WorkOrder";
            this.filterValue = "";
            this.AssignStatus = "Y";
            this.WoCreateDateStart = DateTime.Now.AddMonths(-1);
            this.WoCreateDateEnd = DateTime.Now;
        }
    }
}
