using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.ProdDto;
using ZR.Model;

namespace ZR.Service.IService
{
    public interface ImntnPackSpecService
    {
        PagedInfo<ImesMpkspec> MntnPackSpeclist(string textData, int pageNum, int pageSize, string site);

        int MntnPackSpecInsert(ImesMpkspec imes);

        int MntnPackSpecUpdate(ImesMpkspec imes);

        int MntnPackSpecDelet(ImesMpkspec imes);

        object MntnPackSpeclistHt(string pkspecName, string id,string site);
    }
}
