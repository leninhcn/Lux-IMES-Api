using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Rework;
using ZR.Model;
using System.Data;

namespace ZR.Service.Rework.IReworkService
{
    /// <summary>
    /// service接口
    /// </summary>
    public interface IPReworkNoService : IBaseService<PReworkNo>
    {
        //PagedInfo<PReworkNoDto> GetList(PReworkNoQueryDto parm);

        PReworkNo GetInfo(int Id);

        DataTable GetSpec(string parm);

        DataTable GetReworkno(string parm);

        DataTable GetStationOptions(string parm);

        DataTable GetRoute(string parm);

        DataTable PreCheck(string input, string inputtype, int isnewwo, string newwo, string tstation);
        PReworkNo AddPReworkNo(PReworkNo parm);

        int UpdatePReworkNo(PReworkNo parm);

        string ReworkExcute(Dictionary<string,object> parm);

    }




}
