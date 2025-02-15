using System.Collections.Generic;
using ZR.Model;
using ZR.Model.System;
using ZR.Model.System.Dto;

namespace ZR.Service.System.IService
{
    /// <summary>
    /// 通知公告表service接口
    ///
    /// @author zr
    /// @date 2021-12-15
    /// </summary>
    public interface ISysNoticeService : IBaseService<SysNotice>
    {
        List<SysNotice> GetSysNotices();

        PagedInfo<SysNotice> GetPageList(SysNoticeQueryDto parm);
    }
}
