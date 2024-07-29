using System;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Business;
using System.Collections.Generic;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 上下料
    /// </summary>
    public interface IPFeedingService : IBaseService<PFeeding>
    {
        PagedInfo<PFeedingDto> GetList(PFeedingQueryDto parm);

        PFeeding GetInfo(long Id);
        //工单上料
        int AddPFeeding(PFeeding parm);
        //上的物料判断
        int AddPFeedingPd(PFeeding parm);

        PWoCutting QueryPFeedingorder();

       int  GetPFeedingupdateorder(long id);

        int UpdatePFeeding(PFeeding parm);
        ResponstationInout stationInout(stationInout parm);

        ResponstationInout stationInoutAgv(stationInout parm);
    }
}
