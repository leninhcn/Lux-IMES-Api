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
    public interface IMBillMaterialService : IBaseService<MBillMaterial>
    {
        PagedInfo<MBillMaterialDto> GetList(MBillMaterialQueryDto parm);

        MBillMaterial GetInfo(string Id);

        MBillMaterial AddMBillMaterial(MBillMaterial parm);

        int UpdateMBillMaterial(MBillMaterial parm);

    }
}
