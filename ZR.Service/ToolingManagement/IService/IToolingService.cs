using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;

namespace ZR.Service.ToolingManagement.IService
{
    public interface IToolingService : IBaseService<MTooling>
    {
        public PagedInfo<MTooling> GetInfoList(MesCommonQuery query, string site);

        public MTooling GetInfoById(long id);

        public string CheckInfoNameUnique(MTooling tooling);

        public long AddInfo(MTooling tooling);

        public int UpdateInfo(MTooling tooling);
        Task SwitchStatus(long id, string status, string empNo);
    }
}
