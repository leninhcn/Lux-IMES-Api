using System;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Business;
using System.Collections.Generic;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 辅材上下线service接口
    /// </summary>
    public interface IPMaterialsStatusService : IBaseService<PMaterialsStatus>
    {
        PagedInfo<PMaterialsStatusDto> GetList(PMaterialsStatusQueryDto parm);

        PMaterialsStatus GetInfo(long MaterialsId);

        int AddPMaterialsStatus(PMaterialsStatus parm);

        int UpdatePMaterialsStatus(PMaterialsStatus parm);

    }
}
