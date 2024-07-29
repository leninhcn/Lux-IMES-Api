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
    public interface IToolingTypeService : IBaseService<MToolingType>
    {
        public PagedInfo<MToolingType> GetInfoList(MesCommonQuery query, string site);

        public MToolingType GetInfoById(long id);

        public string CheckInfoNameUnique(MToolingType type);

        public long AddInfo(MToolingType type);

        public int UpdateInfo(MToolingType type);

        public List<String> GetAllTypeList(string site);
        Task SwitchStatus(long id, string status, string empNo);
    }
}
