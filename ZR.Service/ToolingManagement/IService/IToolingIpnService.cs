using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;

namespace ZR.Service.ToolingManagement.IService
{
    public interface IToolingIpnService : IBaseService<MToolingIpn>
    {
        public List<MToolingIpn> GetInfoList(string site, long toolingid);

        public long AddInfo(MToolingIpn toolingIpn);

        public long AddHistory(long toolingId);
        
        public long DeleteInfo(long toolingId);


        public string CheckInfoNameUnique(MToolingIpn toolingipn);
    }
}
