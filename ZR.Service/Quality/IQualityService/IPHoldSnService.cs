using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Quality;
using ZR.Model;
using ZR.Model.Dto.Quality;

namespace ZR.Service.Quality.IQualityService
{
    /// <summary>
    /// service接口
    /// </summary>
    public interface IPHoldSnService : IBaseService<PHoldSn>
    {
        PagedInfo<PHoldSnDto> GetList(PHoldSnQueryDto parm);


        PHoldSn GetInfo(int Id);

        PHoldSn AddPHoldSn(PHoldSn parm);

        PHoldSn AddPHoldSnByTran(PHoldSn parm);
        int UpdatePHoldSn(PHoldSn parm);

        int UpdatePHoldSns(PHoldSn[] parm);

    }



}
