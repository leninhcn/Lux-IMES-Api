using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Dto.ProdDto;
using ZR.Model.System;
using static ZR.Model.Dto.ProdDto.ImesMmodel;

namespace ZR.Service.IService
{
    public interface IProdModelService
    {
        //ISugarQueryable<ImesMmodel> getModelData(string? enaBled, string? optionData, string? textData);

        PagedInfo<ImesMmodel> getModelData(string enaBled, string optionData, string textData, int pageNum, int pageSize,string site);
        int getModelInsert(ImesMmodel mmodel);
        int getModelUpdate(ImesMmodel mmodel);

        int PordModelDelete(ImesMmodel mmodel);

        Object ImesMmodelHtlist(int id, string model,string site);


        //---------------分割线-----------------------------------------------------------------------------------------------


        PagedInfo<MPartHtData> PordPartList(string enaBled, string optionData, string textData, int pageNum, int pageSize,string site);

        (string, object, object) PordPartImportData(List<MPartHtData> mPartHts, string site, string name);

        Object PlanList(string site);

        string PordPartInsert(MPartHtData mPartHt);

        string PordPartUpdate(MPartHtData mPartHt);

        int PordPartDelete(long id,string site, string updateEmpno);

        object PordPartHtlist(int id, string site);
    }
}
