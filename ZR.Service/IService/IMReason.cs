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
    /// 不良原因service接口
    /// </summary>
    public interface IMReasonService : IBaseService<MReason>
    {
        //获取所有不良原因信息
        PagedInfo<MReasonDto> GetList(MReasonQueryDto parm);
        //查询单个详细信息
        MReason GetInfo(long Id);
        //新增
        ResultInfo AddMReason(MReason parm);
        //修改
        string UpdateMReason(MReason parm);
        //删除
        string DeleteMReason(MReasonDto parm,string id);
        string UpdateMReasonStatus(MReason model);
    }
}
