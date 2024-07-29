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
    public interface IMStationTypeService : IBaseService<MStationType>
    {
        PagedInfo<MStationTypeDto> GetList(MStationTypeQueryDto parm);

        MStationType GetInfo(int Id);

        MStationType AddMStationType(MStationType parm);

        int UpdateMStationType(MStationType parm);

    }
}
