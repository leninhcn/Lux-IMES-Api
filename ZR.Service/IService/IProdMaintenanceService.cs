using JinianNet.JNTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;

namespace ZR.Service.IService
{
    public interface IProdMaintenanceService
    {
        Object Verification(string reelNo, string site);
        int Validate(string site, string ipn, string reelNo, string inscription, long updateUserid);
        PagedInfo<ImesMpartInscription> MaintenanceList(string enaBled, string textData, string optionData, int pageNum, int pageSize, string site);
        Object Maintenancepart(string ipn, string site); 
        
        //ISugarQueryable<SnStatus> ProdMaintenance(bool Filter, string sFieldName, string sFieldText, int Enable);
        PagedInfo<ImesMpartInscription> ShowData(string enaBled, string textData, string optionData, int pageNum, int pageSize, string site);
        int MaintenanceInsert(ImesMpartInscription imesMpartInscription);
        int MaintenanceUpdate(ImesMpartInscription imesMpartInscription);
        int MaintenanceDelete(ImesMpartInscription imesMpartInscription);
        List<ImesMpartInscription> History(string id, string site);

    }
}
