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
    public interface IMntnActionBaseService : IBaseService<MActionJobTypeBase>
    {
        string DeleteJobGroup(MActionGroupBase param);
        string DeleteJobGroupLink(MActionGroupLink param);
        string DeleteJobID(MActionJobBase param);
        string DeleteJobLink(MActionJobLink param);
        string DeleteJobType(MActionJobTypeBase param);
        List<MActionGroupBaseDto> GetJobGroup(MActionGroupBaseQueryDto param);
        List<dynamic> GetJobGroupLinkDetail(MActionGroupBaseQueryDto param);
        List<MActionJobBaseDto> GetJobId(MActionJobBaseQueryDto param);
        List<MActionJobLinkDto> GetJobLink(MActionJobLinkQueryDto param);
        List<MActionJobTypeBaseDto> GetJobType(MActionJobTypeBaseQueryDto param);
        List<dynamic> GetJobTypelist(string site);
        List<MStationActionDto> GetStationAction(MStationActionQueryDto param);
        string InsertJobGroup(MActionGroupBase param);
        string InsertJobGroupLink(MActionGroupLink param);
        string InsertJobId(MActionJobBase param);
        string InsertJobLink(MActionJobLink param);
        string InsertJobType(MActionJobTypeBase param);
        string InsertOrUpdateStationAction(MStationAction param);
        string UpdateJobGroup(MActionGroupBase param);
        string UpdateJobGroupLink(MActionGroupLink param);
        string UpdateJobId(MActionJobBase param);
        string UpdateJobLink(MActionJobLink param);
        string UpdateJobType(MActionJobTypeBase param);
    }
}
