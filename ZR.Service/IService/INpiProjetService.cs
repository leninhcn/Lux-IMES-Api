using System;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Business;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Http;
using ZR.Model.System;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// NPI项目管理service接口
    /// </summary>
    public interface INpiProjetService : IBaseService<NpiProjet>
    {
        PagedInfo<NpiProjetDto> GetList(NpiProjetQueryDto parm);

        NpiProjet GetInfo(int Id);

        NpiProjet AddNpiProjet(NpiProjet parm);

        int UpdateNpiProjet(NpiProjet parm);

        (string, object, object) ImportNpiProjet(List<NpiProjet> list);

        public int GetMaxID();
        public int DeleteDataByIds(long[] operIds);

        DataTable GetStepById(int id);
        DataTable GetApprovalLogById(int id);

        DataTable GetStepInfoById(int id,string step);
        DataTable GetOrderInfoById(int id, string step);

        DataTable GetStationType();

        DataTable GetStationTypeConfig();
        public int AddRdItem(StepInfo step);

        public int AddOrderInfo(OrderInfo step);

        public string ConvertToHtmlTable(DataTable table);

        public string Getemails();


        Task<NpiProjetFile> SaveNpiFileToLocal(string rootPath,int id,string step, string fileName, string fileDir, string userName, IFormFile formFile);


    }
}
