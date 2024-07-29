using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto.Tooling;

namespace ZR.Service.ToolingManagement.IService
{
    public interface IToolingSnService : IBaseService<MToolingSn>
    {
        public PagedInfo<MToolingSn> GetInfoList(ToolingSnQuery query, string site);

        public MToolingSn GetInfoByToolingSnId(long toolingSnId);

        public MToolingSn GetInfoByToolingSn(string site ,string toolingSn);

        public string CheckInfoNameUnique(MToolingSn toolingsn);

        public long AddInfo(MToolingSn toolingsn);

        public int UpdateInfo(MToolingSn toolingsn);
        Task SwitchStatus(long id, string status, string empNo);
    }
}
