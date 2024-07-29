using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    [SugarTable(TableName = "SAJET.HJXS_PIE_WTEQUDISPATCHLIST_B")]
    public class HjxsPieWtEquDispatchListB
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
        public string Id { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "ROWNO")]
        public long RowNo { get; set; }

        /// <summary>
        /// Hid
        /// </summary>
        [SugarColumn(ColumnName = "HID")]
        public string Hid { get; set; }

        /// <summary>
        /// 线卡号
        /// </summary>
        [SugarColumn(ColumnName = "LINENO")]
        public string LineNo { get; set; }

        /// <summary>
        /// 线材特征
        /// </summary>
        [SugarColumn(ColumnName = "LINETYPE")]
        public string LineType { get; set; }

        /// <summary>
        /// 加工线长
        /// </summary>
        [SugarColumn(ColumnName = "LINEL")]
        public string LineL { get; set; }


        /// <summary>
        /// 线长公差
        /// </summary>
        [SugarColumn(ColumnName = "NETLENGTH")]
        public string NetLength { get; set; }

        /// <summary>
        /// Lengthen
        /// </summary>
        [SugarColumn(ColumnName = "LENGTHEN")]
        public string Lengthen { get; set; }

        /// <summary>
        /// 线材编码
        /// </summary>
        [SugarColumn(ColumnName = "WIREPRONUM")]
        public string WireProNum { get; set; }

        /// <summary>
        /// A端端子
        /// </summary>
        [SugarColumn(ColumnName = "TERMINALNOF")]
        public string TerminalNoF { get; set; }

        /// <summary>
        /// B端端子
        /// </summary>
        [SugarColumn(ColumnName = "TERMINALNOA")]
        public string TerminalNoA { get; set; }


        /// <summary>
        /// 产品型号
        /// </summary>
        [SugarColumn(ColumnName = "PRONUM")]
        public string ProNum { get; set; }

        /// <summary>
        /// 生产批次
        /// </summary>
        [SugarColumn(ColumnName = "PROLOT")]
        public string ProLot { get; set; }

        /// <summary>
        /// WoNum
        /// </summary>
        [SugarColumn(ColumnName = "WONUM")]
        public string WoNum { get; set; }

        /// <summary>
        /// WoExt
        /// </summary>
        [SugarColumn(ColumnName = "WOEXT")]
        public string WoExt { get; set; }

        /// <summary>
        /// 排产日期
        /// </summary>
        [SugarColumn(ColumnName = "PRODATE")]
        public string ProDate { get; set; }


        /// <summary>
        /// 线材来料批次
        /// </summary>
        [SugarColumn(ColumnName = "BATCHCODE")]
        public string BatchCode { get; set; }

        /// <summary>
        /// 排产产线
        /// </summary>
        [SugarColumn(ColumnName = "PROLINE")]
        public string ProLine { get; set; }

        /// <summary>
        /// 投入状态
        /// </summary>
        [SugarColumn(ColumnName = "FINISTATUE")]
        public string FiniStatue { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(ColumnName = "STARTTIME")]
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [SugarColumn(ColumnName = "ENDTIME")]
        public string EndTime { get; set; }


        /// <summary>
        /// 操作人
        /// </summary>
        [SugarColumn(ColumnName = "OPERATOR")]
        public string Operator { get; set; }

        /// <summary>
        /// 排产数量
        /// </summary>
        [SugarColumn(ColumnName = "PRODAYQTY")]
        public double ProDayQty { get; set; }

        /// <summary>
        /// 任务数量
        /// </summary>
        [SugarColumn(ColumnName = "WORKQTY")]
        public double WorkQty { get; set; }

        /// <summary>
        /// OfflineNo
        /// </summary>
        [SugarColumn(ColumnName = "OFFLINENO")]
        public string OfflineNo { get; set; }

        /// <summary>
        /// 线卡版本
        /// </summary>
        [SugarColumn(ColumnName = "OFFLINEVER")]
        public string Offlinever { get; set; }


        /// <summary>
        /// 客户型号
        /// </summary>
        [SugarColumn(ColumnName = "CUSPRONUM")]
        public string CusProNum { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [SugarColumn(ColumnName = "PRODESC")]
        public string ProDesc { get; set; }

        /// <summary>
        /// A端子批次
        /// </summary>
        [SugarColumn(ColumnName = "TERMINALBATCHA")]
        public string TerminalBatchA { get; set; }

        /// <summary>
        /// B端子批次
        /// </summary>
        [SugarColumn(ColumnName = "TERMINALBATCHB")]
        public string TerminalBatchB { get; set; }

        /// <summary>
        /// A模具编号
        /// </summary>
        [SugarColumn(ColumnName = "MODELCODEA")]
        public string ModelCodeA { get; set; }


        /// <summary>
        /// B模具编号
        /// </summary>
        [SugarColumn(ColumnName = "MODELCODEB")]
        public string ModelCodeB { get; set; }

        /// <summary>
        /// A模具条码
        /// </summary>
        [SugarColumn(ColumnName = "MODELBATCHA")]
        public string ModelBatchA { get; set; }

        /// <summary>
        /// B模具条码
        /// </summary>
        [SugarColumn(ColumnName = "MODELBATCHB")]
        public string ModelBatchB { get; set; }

        /// <summary>
        /// 发行编号
        /// </summary>
        [SugarColumn(ColumnName = "ISSUENUMBER")]
        public string IssueNumber { get; set; }

        /// <summary>
        /// 线材数量
        /// </summary>
        [SugarColumn(ColumnName = "CABLEQTY")]
        public double CableQty { get; set; }


        /// <summary>
        /// A端子数量
        /// </summary>
        [SugarColumn(ColumnName = "TERAQTY")]
        public double TerAQty { get; set; }

        /// <summary>
        /// B端子数量
        /// </summary>
        [SugarColumn(ColumnName = "TERBQTY")]
        public double TerBQty { get; set; }

        /// <summary>
        /// PProLot
        /// </summary>
        [SugarColumn(ColumnName = "PPROLOT")]
        public string PProLot { get; set; }

        /// <summary>
        /// 已扫数量
        /// </summary>
        [SugarColumn(ColumnName = "CABLEFINISHQTY")]
        public double CableFinishQty { get; set; }

        /// <summary>
        /// A已扫数量
        /// </summary>
        [SugarColumn(ColumnName = "TERAFINISHQTY")]
        public double TerAFinishQty { get; set; }

        /// <summary>
        /// B已扫数量
        /// </summary>
        [SugarColumn(ColumnName = "TERBFINISHQTY")]
        public double TerBFinishQty { get; set; }

        /// <summary>
        /// BarData
        /// </summary>
        [SugarColumn(ColumnName = "BARDATA")]
        public string BarData { get; set; }

        /// <summary>
        /// LabQty
        /// </summary>
        [SugarColumn(ColumnName = "LABQTY")]
        public long LabQty { get; set; }
    }
}
