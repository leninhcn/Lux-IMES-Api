using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;

namespace ZR.Service.IService
{
    public interface IMntnWorkOrderService
    {
        string AddPWoBase(WoBase parm);
        PagedInfo<PWoBaseDto> GetWoBaseList(PWoBaseQueryDto parm);
        int UpdatePWoBase(WoBase model);
        int UpdatePWoBaseById(WoBase model);
        /**
         * PDA 查询接口
         * */
        List<WoBase> GetWoBaseListPda(PWoBaseQueryDto parm);
        PagedInfo<PWoBaseDto> GetWoBaseListNg(PWoBaseQueryDto parm);
        List<PWoBaseHt> GetWoBaseHistoryList(PWoBaseHTQueryDto parm);
        int UpdatePWoBaseEquipment(WoBase param);
        List<dynamic> GetWoStatusList(PWoBaseHTQueryDto parm);
    }
}
