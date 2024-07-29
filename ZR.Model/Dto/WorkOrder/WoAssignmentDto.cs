using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.WorkOrder
{
    public class WoAssignmentDto
    {
        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrder {  get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ipn { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public string StationType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StationTypeDesc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WorkShop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WoType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? WoScheduleStartDate {  get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? WoScheduleCloseDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? WoAssingStartDate {  get; set; }

        public DateTime? WoAssingEndDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AssignStatus {  get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TopWorkOrder {  get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Spec2 {  get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SoText { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SendAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Progrp {  get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProdName {  get; set; }
    }
}
