using System;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Business;
using System.Collections.Generic;
using System.Data;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 不良品统计service接口
    /// </summary>
    public interface IPNgDetailService : IBaseService<PNgDetail>
    {
        PagedInfo<PNgDetailDto> GetList(PNgDetailQueryDto parm);

        PNgDetail GetInfo(long Id);

        int AddPNgDetail(PNgDetail parm, List<PNgDetail> datalist);

        int UpdatePNgDetail(PNgDetail parm);

        int DelPNgDetail(PNgDetail parm);

        List<dynamic> ListMaChine(string site);
     
        List<dynamic> GetListstatistics(PNgDetailQueryDto parm);

        List<dynamic> QuerySnTravel(string workOrder);

        List<dynamic> SnTravelMacine(string Macine);
    }
}
