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
    public interface IRepairReturnStaService
    {
        Task<ExecuteResult> AppendData(ModelInfo modelInfo, string userNo);
        Task<ExecuteResult> AppendDetailData(detailPartStation stations, string userNo);
        Task<ExecuteResult> Delete(string sId, string userNo);
        Task<ExecuteResult> DetailDelete(string sId, string userNo);
        Task<ExecuteResult> DetailDisabled(ModelInfo modelInfo, string userNo);
        Task<DataTable> DetailHistoryData(string sName);
        Task<ExecuteResult> Disable(ModelInfo modelInfo, string userNo);
        Task<DataTable> getDt(ModelInfo modelInfo);
        Task<DataTable> getModel();
        Task<DataTable> getStage(string sName);
        Task<DataTable> getStage(detailPartStation stations);
        Task<DataTable> HistoryData(string sName);
        Task<ExecuteResult> ModifyData(ModelInfo modelInfo, string userNo);
        Task<ExecuteResult> ModifyDetailData(detailPartStation stations, string userNo);
        Task<ExecuteResult> SaveExcel(detailPartStation stations, string userNo);
        Task<ExecuteResult> ShowData(returnStaData retData);
        Task<ExecuteResult> ShowDetailData(stationDetailData detailData);
    }
}
