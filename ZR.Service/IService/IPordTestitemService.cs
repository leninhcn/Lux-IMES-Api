using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.ProdDto;
using ZR.Model;

namespace ZR.Service.IService
{
    public interface IPordTestitemService
    {
        PagedInfo<ImesMTestItemType> TestList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site);
        int InsertTestItemType(ImesMTestItemType imesMTestItemType, string site, string updateEmp);
        int UpdateTestItemType(ImesMTestItemType imesMTestItemType, string site);
        int TestItemTypeabled(ImesMTestItemType imesMTestItemType, string site);
        int DeleteTestItemType(ImesMTestItemType imesMTestItemType);
        PagedInfo<ImesMTestItem> TestitemList(String enaBled, string itemTypeid, string optionData, string textData, int pageNum, int pageSize, string site);
        int TestItemabled(ImesMTestItem imesMTestItem, string site);
        int InsertTestItem(ImesMTestItem imesMTestItem, string site, string updateEmp);
        int UpdateTestItem(ImesMTestItem imesMTestItem, string site);
        int DeleteTestItem(ImesMTestItem imesMTestItem);
    }
}
