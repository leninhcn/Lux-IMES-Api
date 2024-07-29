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
    public interface IMDutyService : IBaseService<MDuty>
    {
        //获取所有信息
        PagedInfo<MDutyDto> GetList(MDutyQueryDto parm);
        //查询单个详细信息
        MDuty GetInfo(long Id);
        //新增
        string AddMReason(MDuty parm);
        //修改
        string UpdateMReason(MDuty parm);
        //删除
        string DeleteMReason(MDutyDto parm,string id);
        string UpdateMReasonStatus(MDuty model);
    }
}
