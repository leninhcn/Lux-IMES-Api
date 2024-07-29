using System;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Business;
using System.Collections.Generic;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// service接口
    /// </summary>
    public interface IPSnTravelService : IBaseService<PSnTravel>
    {
        PagedInfo<PSnTravelDto> GetList(PSnTravelQueryDto parm);

        PSnTravel GetInfo(int Id);

        PSnTravel AddPSnTravel(PSnTravel parm);

        int UpdatePSnTravel(PSnTravel parm);

    }
}
