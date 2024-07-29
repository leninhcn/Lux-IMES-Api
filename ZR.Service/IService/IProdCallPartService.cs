using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.ProdDto;
using ZR.Model;

namespace ZR.Service.IService
{
    public interface IProdCallPartService
    {
        PagedInfo<ImesMCallPart> CallPartList(string enabled, string optionData, string textData, int pageNum, int pageSize, string site);
        object LineList(string site);
        object History(string id, string site);
        PagedInfo<IsmtMMaterialnfo> Ipnlist(string type, string enabled, string optionData, string textData, int pageNum, int pageSize );
        (string, object, object) CallPartImport(List<ImesMCallPart> imes, string site, string name);
        PagedInfo<ImesPwoBase> WorkOrder(string optionData, string textData, int pageNum, int pageSize, string site);
        int CallPartabled(ImesMCallPart imesMCallPart, string site);
        int InsertCallPart(ImesMCallPart imesMCallPart);
        int UpdateCallPart(ImesMCallPart imesMCallPart, string site);
        int DeleteCallPart(ImesMCallPart imesMCallPart, string site);
    }
}
