using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.ProdDto;
using ZR.Model;

namespace ZR.Service.IService
{
    public interface IMntnMaterialsService
    {

        PagedInfo<ImesMsnFeature> Materialsllist(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site);

        (string, object, object) MaterialsImportData(List<ImesMsnFeature> users, string site, string name);

        int MaterialsUpdate(ImesMsnFeature imesMpart);

        int MaterialsDelet(ImesMsnFeature imesMpart);

        Object MaterialsllistHt(int Id,string site);

        int MaterialInsert(ImesMsnFeature imesMsn);

        Object MaterialslIpnlist(string dateIpn, string dateIpnText, string site);

        Object MaterialslDescriptionlist(string dateIpn, string dateIpnText, string site);

        //-------------------------------分隔---------------------------------------------------------------------------
        PagedInfo<ImesMpartRoute> MntnPartRoutelist(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site);

        (string, object, object) MntnPartRouteImportData(List<ImesMpartRoute> users, string site, string name);

        int MntnPartRouteUpdate(ImesMpartRoute imesMpart);

        int MntnPartRouteDelet(ImesMpartRoute imesMpart);

        Object MntnPartRoutelistHt(int Id, string site);

        Object MntnPartRouteIpnlist(string ipn, string site);

        Object MntnPartRouteRoadNamelist(string routeName, string site);

        Object MntnPartRuleSetNameRulelist(string ruleSetName, string site);

        Object MntnPartPkspecNamelist(string pkspecName, string site);

        int MntnPartRouteInsert(ImesMpartRoute imes);
    }
}
