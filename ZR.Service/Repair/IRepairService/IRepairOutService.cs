using JinianNet.JNTemplate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;
using ZR.Model.Repair.Dto;

namespace ZR.Service.Repair.IRepairService
{
    public interface IRepairOutService
    {
        public Task<DataTable> selectSN(string sn, string type);

        public Task<DataTable> getDefect(string sn);

        public Task<DataTable> getRepaired(string sn);
        public Task<DataTable> getHold(string sn);
        public Task<string> CheckSN(string sn);
        public Task<string> RepairSn(RepairInDto typeSN, string sn, string _userNo);

        public Task<DataTable> checkstatus(string sn);

        public Task<string> RepairSnOut(RepairInDto repOut, string _userNo, string strProcess);
    }
}
