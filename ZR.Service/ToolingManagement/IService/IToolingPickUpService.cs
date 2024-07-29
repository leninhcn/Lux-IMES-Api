using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Model.Dto.Tooling;

namespace ZR.Service.ToolingManagement.IService
{
    public interface IToolingPickUpService : IBaseService<MToolingToolingSnVo>
    {
        public ImesMemp SelectEmpByNo(string empno, string site);

        public MToolingToolingSnVo selectToolingByToolingSn(string toolingsn, string site);


        public ExecuteResult ToolingPickUp(string toolingsn, string empno, string site);

        public ExecuteResult ToolingReturn(string toolingsn, string empno, string site);


        List<MLine> SelectLine(string site);

        bool CheckLine(string site,string line);

        List<MPart> SelectPart(string ipn, string site);


        public ExecuteResult ToolingPickLoad(string toolingsn, string line, string ipn, string empno, string site);

        public ExecuteResult ToolingPickUnload(string toolingsn, string line, string empno, string site);

        public ToolingPickLoadVo SelectToolingPickLoadIn(string line, string toolingSn, string site);

        public ToolingPickLoadVo SelectToolingPickLoadOut(string line, string toolingSn, string site);
        Task<List<ToolingPickupResultVo>> GetToolingInfo(string toolingSn);
    }
}
