using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Repair.Dto;

namespace ZR.Service.Repair.IRepairService
{
    public interface IRepairDelinkService
    {
       public Task<ExecuteResult> CheckSN(string sn);

       public Task<DataTable> getDetail(string sn);

       public Task<DataTable> getDefect(string sn);

       //public Task<DataTable> getKpsn(string sn);

        //public Task<DataTable> getPostUrl();

        //public Task<DataTable> getKpsnInfo(string ipn);

       // public Task<ExecuteResult> RemoveKp(string station, string sn, string recid, string kpsn, string partno, string kpflag, string defect_data, string userNo);
        public Task<List<lvkp>> Show_KP(string sn);

        public Task<bool> Remove_KP(repariDel repari,string userNo);
    }
}
