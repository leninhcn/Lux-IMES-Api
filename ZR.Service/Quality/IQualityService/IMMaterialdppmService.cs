using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.Quality;
using ZR.Model.Quality;
using ZR.Model;
using System.Data;

namespace ZR.Service.Quality.IQualityService
{

    /// <summary>
    /// service接口
    /// </summary>
    public interface IMMaterialdppmService : IBaseService<MMaterialdppm>
    {
        PagedInfo<MMaterialdppmDto> GetList(MMaterialdppmQueryDto parm);

        MMaterialdppm GetInfo(int Id);

        DataTable GetModels();
        MMaterialdppm AddMMaterialdppm(MMaterialdppm parm);

        int UpdateMMaterialdppm(MMaterialdppm parm);

    }



}
