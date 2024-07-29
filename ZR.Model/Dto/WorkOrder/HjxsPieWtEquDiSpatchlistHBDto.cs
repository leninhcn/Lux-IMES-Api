using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;

namespace ZR.Model.Dto.WorkOrder
{
    public class HjxsPieWtEquDiSpatchlistHBDto
    {
        /// <summary>
        /// APPEND[新增]
        /// NEXT[下一步]
        /// REJECT[驳回]
        /// </summary>
        public string sUpdateType { get; set; } = "APPEND";

        /// <summary>
        /// 单据信息
        /// </summary>
        public HjxsPieWtEquDiSpatchListH diSpatchlistH { get; set; }

        /// <summary>
        /// 派工任务清单
        /// </summary>
        public List<HjxsPieWtEquDispatchListB> dispatchListBs { get; set; }

        /// <summary>
        /// 输入的批次清单
        /// </summary>
        public List<HjxsPieWtScanLableData> scanLableDatas { get; set; }
    }

    public class InputScanDto
    {
        /// <summary>
        /// 扫描的旧条码（以[]或$进行分割且数组长度不小于8）
        /// 以 [] 分割
        /// [0:发料单号][1:物料批次号][2:][3:][4:][5:生产批次][6:产品型号][7:][8:][9:标贴数量][10:标贴页码]
        /// 以 $ 分割
        /// 0:发料单号$1:供应商代码$2:$3:物料编号$4:$5:生产批号$6:生产日期$7:标贴数量$8:$9:BIN号$10:
        /// </summary>
        public string inputScan { get; set; }

        //public HjxsPieWtEquDiSpatchListH diSpatchlistH { get; set; }

        /// <summary>
        /// 派工清单
        /// </summary>
        public List<HjxsPieWtEquDispatchListB> dispatchListBs { get; set; }

    }

    public class InputScanOut
    {
        public List<HjxsPieWtEquDispatchListB> hjxsPieWtEqus { get; set; }
        public HjxsPieWtScanLableData hjxsPieWtScan { get; set; }


    }
}
