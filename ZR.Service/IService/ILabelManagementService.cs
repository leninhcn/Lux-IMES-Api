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
    public interface ILabelManagementService : IBaseService<MLabelType>
    {
        string AddLabelFile(MLabelTemplateFile parm);
        string AddLabelType(MLabelType param);
        string AddPrintData(MPrintData param);
        string AddStationLabel(MStationtypeLabel param);
        string AddStationLabelParams(MStationtypeLabelParams param);
        string DeleteLabelType(MLabelType param);
        string DeletePrintData(MPrintData param);
        string DeleteStationtypeLabel(MStationtypeLabel param);
        string DeleteStationtypeLabelParam(MStationtypeLabelParams param);
        MLabelType GetInfoLabelType(string ID);
        MPrintData GetInfoprintdata(string ID);
        MStationtypeLabel GetInfostationtypelabel(string ID);
        MStationtypeLabelParams GetInfostationtypelabelParam(string ID);
        MLabelTemplateFile GetListLabelFile(string parm);
        PagedInfo<MLabelTypeDto> GetListlabeltype(MLabelTypeQueryDto parm);
        PagedInfo<MPrintDataDto> GetListprintdata(MPrintDataQueryDto parm);
        PagedInfo<MStationtypeLabelDto> GetListStationtypeLabel(MStationtypeLabelQueryDto parm);
        PagedInfo<MStationtypeLabelParamsDto> GetListStationtypeLabelParam(MStationtypeLabelParamsQueryDto parm);
        (string, object, object) ImportStationtypeLabel(List<MStationtypeLabel> labels, string site);
        string UpdateLabelType(MLabelType param);
        string UpdatePrintData(MPrintData param);
        string UpdateStationtypeLabel(MStationtypeLabel param);
        string UpdateStationtypeLabelParam(MStationtypeLabelParams param);
    }
}
