using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.ProdDto;
using ZR.Model;

namespace ZR.Service.IService
{
    public interface IMntnVendorCustomerService
    {


        PagedInfo<ImesMvendor> VendorList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site);
       
        (string, object, object) VendorImportData(List<ImesMvendor> users, string site, string name);

        string VendorUpdate(ImesMvendor imes);

        int VendorDelet(ImesMvendor imes);

        Object VendorListHt(int Id, string site);

        string VendorInsert(ImesMvendor imes);

        //-----------------------------------------------------------------------

        PagedInfo<ImesmMcustomer> CustomerList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site);

        string CustomerInsert(ImesmMcustomer imes);

        string CustomerUpdate(ImesmMcustomer imes);

        //-------------------------------------------------------------------------
        PagedInfo<ImesMdept> MdeptList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site);

        Object MdeptListHt(int Id, string site);

        string MdeptUpdate(ImesMdept imes);

        int MdeptDelet(ImesMdept imes);

        string MdeptInsert(ImesMdept imes);

        Object MdeptListFactory();
    }
}
