using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.WorkOrder
{
    public class EquipmentWoFilter
    {
        /// <summary>
        /// VBILLCODE[单据编号]、
        /// VBILLDATE[单据日期]、
        /// PRODEPT[车间]、
        /// EQUCODE[设备编号]、
        /// EQUTYPE[设备类别]、
        /// PRONUM[产品型号]、
        /// PROLOT[生产批次]、
        /// WFSTATE[当前状态]、
        /// WFUSERNAME[当前办理人]
        /// </summary>
        public string filterField { get; set; }
        /// <summary>
        /// 默认 null
        /// </summary>
        public string filterValue { get; set; }
        /// <summary>
        /// 格式yyyy-mm-dd 2024-06-01
        /// </summary>
        public string startDate { get; set; }
        /// <summary>
        /// 格式yyyy-mm-dd 2024-07-15
        /// </summary>
        public string endDate { get; set; }

    }
}
