using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.Quality;
using ZR.Model.Quality;
using ZR.Model;

namespace ZR.Service.Quality.IQualityService
{
    /// <summary>
    /// service接口
    /// </summary>
    public interface IPSnBlackListService : IBaseService<PSnBlackList>
    {
        PagedInfo<PSnBlackListDto> GetList(PSnBlackListQueryDto parm);

        PSnBlackList GetInfo(int Id);

        PSnBlackList AddPSnBlackList(PSnBlackList parm);

        int UpdatePSnBlackList(PSnBlackList parm);

    }






}
