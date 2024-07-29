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
    /// ERROR收集服务接口
    /// </summary>
    public interface ICollectErrorService 
    {
        //获取状态
        string Status();
        //新增
        string Add(PCollecterrorLog parm);
    }
}
