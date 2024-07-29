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
    /// 入库程式service接口
    /// </summary>
    public interface IWareHousingService : IBaseService<PWoStockInWmsData>
    {
        Task<(bool, string, List<WareHousingResultDto>)> WareHousingInData(WareHousingDto parm);
    }
}
