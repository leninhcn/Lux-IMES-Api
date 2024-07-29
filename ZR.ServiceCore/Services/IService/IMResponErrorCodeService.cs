using System.Collections.Generic;
using System.Data;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Model.System.Vo;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model;

namespace ZR.Service.System.IService
{
    /// <summary>
    /// 报错返回ErrorCode
    /// </summary>
    public interface IMResponErrorCodeService : IBaseService<MResponseErrorcode>
    {
        Task<string> GetResponseMsg(ResponseErrorCodeDto parm);
    }
}
