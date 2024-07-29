using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.ProdDto;
using ZR.Model.System;

namespace ZR.Model.Business
{
    public interface IMntnlStandardTimeService
    {
        PagedInfo<imesMstandardtime> StandardTimelist(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site);

        (string, object, object) StandardTimeImport(List<imesMstandardtime> users, string site, string name);
        
        Object StandardTimelistHt(int Id, string site);


        int StandardTimeUpdate(imesMstandardtime imes);


        int StandardTimeDelet(imesMstandardtime imes);


        int StandardTimeCopy(imesMstandardtime imes);


        int StandardTimeInsert(imesMstandardtime imes);

        object FactoryStandardTimelist(string ipn,string site);

        object ModelStandardTimelist(string model, string site);

        object StationtypeStandardTimelist(string stationtype, string line, string site);

        object LineStandardTimelist(string site);

        List<SysUser> ModelNameStandardTimeList(string site);


    }
}
