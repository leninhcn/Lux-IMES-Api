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
    public interface IMntnDefectService : IBaseService<MDefect>
    {
        //获取所有不良code信息
        PagedInfo<MDefect> GetDefect(MntnDefect Defect);
        //查询不良信息
        MDefect QueryDefect(MntnDefect Defect);
        //查询所有的机种信息
        DataTable GetModel();
        string CheckUnique(MDefect Defect);
        string Insert(MDefect Defect);
        string Update(MDefect Defect);
        string Delete(MDefect Defect,string id);
        string UpdateStatus(MDefect defect);
    }
}
