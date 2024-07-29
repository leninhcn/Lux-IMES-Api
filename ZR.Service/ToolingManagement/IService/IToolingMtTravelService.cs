using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Model.Dto.Tooling;

namespace ZR.Service.ToolingManagement.IService
{
    public interface IToolingMtTravelService : IBaseService<PToolingMtTravel>
    {
        public ImesMemp SelectEmpByNo(string empno, string site);

        public ExecuteResult GetDateATime(string toolingsn);


        public ExecuteResult GetDay(string time);

        public ExecuteResult CgToolingDEC(string Tooling);

        public ExecuteResult GetUsedCount(string Tooling);

        public ExecuteResult GetMAXCount(string Tooling);

        public ExecuteResult ChangeLStatus(string toolingsn, string status, string empno, string site);

        public ExecuteResult INSERTDATA( ToolingMtTravelDto dto);
        int UpdateInfo(MToolingSn toolingsn);
        Task<List<ToolingMtVo>> GetMaintainResult(string toolingSn);
    }

    
}
