using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.ProdDto;
using ZR.Model;

namespace ZR.Service.IService
{
    public interface IPordCallPartExamineService
    {
        PagedInfo<ImesMCallPart> CallPartList(string enabled, string optionData, string textData, int pageNum, int pageSize, string site);
        object LineList(string site);
        object History(string id, string site);
        PagedInfo<IsmtMMaterialnfo> Ipnlist(string type, string enabled, string optionData, string textData, int pageNum, int pageSize);
        (string, object, object) CallPartImport(List<ImesMCallPart> imes, string site, string name);
        int CallPartabled(ImesMCallPart imesMCallPart, string site);
        int AuditingCallPartExamine(ImesMCallPart imesMCallPart, string updateEmp, string site);
        int InsertCallPartExamine(ImesMCallPart imesMCallPart);
        int UpdateCallPartExamine(ImesMCallPart imesMCallPart, string site, string updateEmp);
        int DeleteCallPartExamine(ImesMCallPart imesMCallPart, string site);
    }
}
