using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;

namespace ZR.Service.ToolingManagement.IService
{
    public interface IToolingRepairService : IBaseService<MToolingSnDefect>
    {
        public ExecuteResult GetToolingSnInfo(string toolingSn, string site);

        public ExecuteResult GetToolingDefectInfo(string toolingSn, string site);

        public ExecuteResult GetDefect(string DefectCode, string site);

        public ExecuteResult InsertToolingSnDefect(string toolingSn, string DefectCode, string empNo, string site);

        public ExecuteResult GetReson(string reason, string site);

        public ExecuteResult GetToolingSnRepairData(string toolingSn, string reason, string site);

        public ExecuteResult ToolingSnRepair(string toolingSn, string reason, string site, string remark, string empNo);


        public ExecuteResult DeleteToolingSnDefectCode(string toolingSn, string defectCode);

        public ExecuteResult ToolingSnComplete(string toolingSn, string site, string empNo);

        public ExecuteResult CheckToolingSnScrap(string toolingSn, string site);

        public ExecuteResult ToolingSnScrap(string toolingSn, string empNo, string site);
    }
}
