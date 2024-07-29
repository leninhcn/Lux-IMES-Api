using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Service.Repair.IRepairService
{
    public interface IRepairPanelService
    {
        Task<ExecuteResult> geInfoBySN(string sn);
        Task<ExecuteResult> getNextStationType(string panel);
        Task<ExecuteResult> getPanelByStationType(string stationType);
        Task<ExecuteResult> getSNByPanel(RepairPanel panel);
        Task<bool> repairPanel(panelDto currStation, string sNextStationType);
        Task<DataTable> ShowDefect(string sn, DateTime dtTime);
    }
}
