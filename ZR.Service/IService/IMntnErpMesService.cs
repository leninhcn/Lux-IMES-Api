using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Dto.ProdDto;
using ZR.Model.System;

namespace ZR.Service.IService
{
    public interface IMntnErpMesService
    {
        PagedInfo<ImesMpartSpecErpMesMapping> ErpMeslist(string enaBled, string optionData, string textData, int pageNum, int pageSize,string site);
        
        (string, object, object) ImportErp(List<ImesMpartSpecErpMesMapping> users,string site,string name);
        
        Object ErpMeslModellist(string site);

        Object ErpMeslStageNamelist(string site);

        int ErpMesInsert(ImesMpartSpecErpMesMapping imesMpart);

        int ErpMesUpdate(ImesMpartSpecErpMesMapping imesMpart);

        int ErpMesDelet(ImesMpartSpecErpMesMapping imesMpart);

        Object ErpMeslistHt(int Id, string site);

        //-----------------分割线-------------------------------------------------------------------------------------


        PagedInfo<ImesMstationTypePartSpec> Stationlist(string enaBled, string optionData, string textData,string site, int pageNum, int pageSize);

        (string, object, object) StationImportData(List<ImesMstationTypePartSpec> users, string site, string name);

        Object StationStagelist(string site);

        Object StationTypelist(string stage, string stationType, string site);

        Object StationModellist(string site);

        Object StationBrandlist(string mesSpec, string stage, string site);

        int StationInsert(ImesMstationTypePartSpec imesMpart); 
        int StationDelete(ImesMstationTypePartSpec imesMpart);

        int StationtUpdate(ImesMstationTypePartSpec imesMpart);

        int StationCopy(ImesMstationTypePartSpec imesMpart);

        Object StationtlistHt(int Id, string site);

        Object StationtlistStage(string stationType, string site);
    }
}
