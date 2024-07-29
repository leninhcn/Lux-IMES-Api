using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.Quality;
using ZR.Model.Quality;

namespace ZR.Service.Quality.IQualityService
{
    public interface IHoldOptionsService : IBaseService<PHoldOptionsDto>
    {
        Dictionary<string, List<PHoldOptionsDto>> GetPHoldOptionsDto();
    }
}
