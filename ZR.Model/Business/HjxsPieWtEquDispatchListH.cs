using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "SAJET.HJXS_PIE_WTEQUDISPATCHLIST_H")]
    public class HjxsPieWtEquDiSpatchListH
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(ColumnName = "ID",IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// Creator
        /// </summary>
        [SugarColumn(ColumnName = "CREATOR")]
        public string Creator { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        [SugarColumn(ColumnName = "CREATETIME")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// VbillCode 单据编号
        /// </summary>
        [SugarColumn(ColumnName = "VBILLCODE")]
        public string VbillCode { get; set;}
        /// <summary>
        /// VcardDate 单据日期
        /// </summary>
        [SugarColumn(ColumnName = "VBILLDATE")]
        public DateTime? VbillDate { get; set;}
        /// <summary>
        /// ProDept 车间
        /// </summary>
        [SugarColumn(ColumnName = "PRODEPT")]
        public string ProDept { get;set;}
        /// <summary>
        /// EquCode 设备编号
        /// </summary>
        [SugarColumn(ColumnName = "EQUCODE")]
        public string EquCode { get; set; }
        /// <summary>
        /// EquType 设备类别
        /// </summary>
        [SugarColumn(ColumnName = "EQUTYPE")]
        public string EquType { get; set; }
        /// <summary>
        /// OpWorker
        /// </summary>
        [SugarColumn(ColumnName = "OPWORKER")]
        public string OpWorker { get; set; }
        /// <summary>
        /// Sender
        /// </summary>
        [SugarColumn(ColumnName = "SENDER")]
        public string Sender { get; set; }
        /// <summary>
        /// SendTime
        /// </summary>
        [SugarColumn(ColumnName = "SENDTIME")]
        public string SendTime { get; set; }
        /// <summary>
        /// FinishTime
        /// </summary>
        [SugarColumn(ColumnName = "FINISHTIME")]
        public string FinishTime { get; set; }
        /// <summary>
        /// Finisher
        /// </summary>
        [SugarColumn(ColumnName = "FINISHER")]
        public string Finisher { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        [SugarColumn(ColumnName = "REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// WfState 当前状态
        /// </summary>
        [SugarColumn(ColumnName = "WFSTATE")]
        public string WfState { get; set; }
        /// <summary>
        /// WfUserName 当前办理人
        /// </summary>
        [SugarColumn(ColumnName = "WFUSERNAME")]
        public string WfUserName { get; set; }
        /// <summary>
        /// WfIsFinish
        /// </summary>
        [SugarColumn(ColumnName = "WFISFINISH")]
        public string WfIsFinish { get; set; }
        /// <summary>
        /// WfIsStart
        /// </summary>
        [SugarColumn(ColumnName = "WFISSTART")]
        public string WfIsStart { get; set; }
        /// <summary>
        /// WfAllUser
        /// </summary>
        [SugarColumn(ColumnName = "WFALLUSER")]
        public string WfAllUser { get; set; }
        /// <summary>
        /// WfOption
        /// </summary>
        [SugarColumn(ColumnName = "WFOPINION")]
        public string WfOption { get; set; }
        /// <summary>
        /// WfRunid
        /// </summary>
        [SugarColumn(ColumnName = "WFRUNID")]
        public string WfRunid { get; set; }
        /// <summary>
        /// WfFlowid
        /// </summary>
        [SugarColumn(ColumnName = "WFFLOWID")]
        public string WfFlowid { get; set; }
        /// <summary>
        /// WfPrcsid
        /// </summary>
        [SugarColumn(ColumnName = "WFPRCSID")]
        public string WfPrcsid { get; set; }
        /// <summary>
        /// CabQty
        /// </summary>
        [SugarColumn(ColumnName = "CABQTY")]
        public long CabQty { get; set; }
        /// <summary>
        /// ProNum 产品型号
        /// </summary>
        [SugarColumn(ColumnName = "PRONUM")]
        public string ProNum { get; set; }
        /// <summary>
        /// ProLot 生产批次
        /// </summary>
        [SugarColumn(ColumnName = "PROLOT")]
        public string ProLot { get; set; }
        /// <summary>
        /// 生产令
        /// </summary>
        [SugarColumn(ColumnName = "WONUM")]
        public string WoNum { get; set; }
        /// <summary>
        /// 排产日期
        /// </summary>
        [SugarColumn(ColumnName = "PRODATE")]
        public string ProDate { get; set; }
        /// <summary>
        /// CusProNum
        /// </summary>
        [SugarColumn(ColumnName = "CUSPRONUM")]
        public string CusProNum { get; set; }
        /// <summary>
        /// 型号描述
        /// </summary>
        [SugarColumn(ColumnName = "PRODESCR")]
        public string ProDescr { get; set; }
        /// <summary>
        /// CusNum
        /// </summary>
        [SugarColumn(ColumnName = "CUSNUM")]
        public string CusNum { get; set; }
        /// <summary>
        /// LotQty
        /// </summary>
        [SugarColumn(ColumnName = "LOTQTY")]
        public string LotQty { get; set; }
        /// <summary>
        /// PrintCode
        /// </summary>
        [SugarColumn(ColumnName = "PRINTCODE")]
        public string PrintCode { get; set; }
        /// <summary>
        /// PrintName
        /// </summary>
        [SugarColumn(ColumnName = "PRINTNAME")]
        public string PrintName { get; set; }

    }
}
