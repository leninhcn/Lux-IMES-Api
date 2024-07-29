using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;
using ZR.Model.Dto;

namespace ZR.Service.IService
{
    public interface IMntnWoBomLinkService
    {
        int AddPWoBom(WoBom model);
        int DeletePWoBom(WoBom model);
        int DeletePWoBomByWo(WoBom model);
        WoBom GetInfo(WoBom model);
        Task<List<dynamic>> GetWoBase(PWoBomQueryDto param);
        List<PWoBomDto> GetWoBom(PWoBomQueryDto param);
        (string, object, object) ImportWoBom(List<WoBom> users, string site);
        int UpdatePWoBom(WoBom model);
        string UpdatePWoBomByWO(List<WoBom> param, WoBom swobom);
    }
}
