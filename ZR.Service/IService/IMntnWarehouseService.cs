using System.Collections.Generic;
using System.Data;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Model.System.Vo;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model;

namespace ZR.Service.IService
{
    /// <summary>
    /// 责任类型service接口
    /// </summary>
    public interface IMntnWarehouseService : IBaseService<MWarehouse>
    {
       Resultinfo<string> AddLocation(MLocation model);
        Resultinfo<string> AddWareHouse(MWarehouse model);
        MLocation GetInfoLocation(MLocationQueryDto param);
        MWarehouse GetInfoWareHouse(string ID, string site);
        Resultinfo<PagedInfo<MLocationDto>> GetListLocation(MLocationQueryDto parm);
        PagedInfo<MWarehouseDto> GetListWareHouse(MWarehouseQueryDto parm);
       Resultinfo<string> UpdateLocal(MLocation model);
        Resultinfo<string> UpdateWareHouse(MWarehouse model);
    }
}
