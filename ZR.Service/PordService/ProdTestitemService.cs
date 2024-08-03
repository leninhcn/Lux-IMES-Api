using Infrastructure.Attribute;
using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZR.Infrastructure.Model;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Model.Quality;
using ZR.Repository;
using ZR.Service.IService;

namespace ZR.Service.PordService
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    internal class ProdTestitemService : BaseService<SnStatus>, IPordTestitemService
    {
        public PagedInfo<ImesMTestItemType> TestList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMTestItemType>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enaBled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "ITEM_TYPE_CODE", it => it.itemTypecode.Contains(textData));
                exp.AndIF(optionData == "ITEM_TYPE_DESC", it => it.itemTypedesc.Contains(textData));
                exp.AndIF(optionData == "ITEM_TYPE_DESC2", it => it.itemTypedesc2.Contains(textData));
                exp.AndIF(optionData == "MIN_INSP_QTY", it => it.minInspqty.ToString().Contains(textData));
                exp.AndIF(optionData == "ITEM_TYPE_NAME", it => it.itemTypename.Contains(textData));
            }
            var expPagedInfo = Context.Queryable<ImesMTestItemType>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
            return expPagedInfo;
        }
        public int InsertTestItemType(ImesMTestItemType imesMTestItemType, string site, string updateEmp)
        {
            //string sqlStr = $"SELECT MAX(ID) AS MaxID FROM SAJET.M_WEIGHT_FAI";
            int MaxId = Context.Queryable<ImesMTestItemType>().Max(it => it.itemTypeid) + 1;
            imesMTestItemType.itemTypeid = MaxId;
            imesMTestItemType.site = site;
            imesMTestItemType.updateEmp = updateEmp;
            imesMTestItemType.createTime = DateTime.Now;
            imesMTestItemType.updateTime = DateTime.Now;
            int insertErp = Context.Insertable(imesMTestItemType).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertErp > 0)
            {
                string sqlStr1 = $"insert INTO SAJET.m_TEST_ITEM_TYPE_HT(select * FROM SAJET.m_TEST_ITEM_TYPE where item_type_id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr1 + ")");
                return 1;
            }
            return insertErp;
        }
        public int InsertTestItem(ImesMTestItem imesMTestItem, string site, string updateEmp)
        {
            //string sqlStr = $"SELECT MAX(ID) AS MaxID FROM SAJET.M_WEIGHT_FAI";
            int MaxId = Context.Queryable<ImesMTestItem>().Max(it => it.itemId) + 1;
            imesMTestItem.itemId = MaxId;
            imesMTestItem.site = site;
            imesMTestItem.updateEmp = updateEmp;
            imesMTestItem.createTime = DateTime.Now;
            imesMTestItem.updateTime = DateTime.Now;
            int insertErp = Context.Insertable(imesMTestItem).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertErp > 0)
            {
                string sqlStr1 = $"insert INTO SAJET.M_TEST_ITEM_HT(select * FROM SAJET.M_TEST_ITEM where item_type_id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr1 + ")");
                return 1;
            }
            return insertErp;
        }
        public int UpdateTestItemType(ImesMTestItemType imesMTestItemType, string site)
        {
            int updateTestItemType = Context.Updateable(imesMTestItemType).UpdateColumns(it => new { it.itemTypecode, it.itemTypename,it.itemTypedesc,it.itemTypedesc2,it.minInspqty }).WhereColumns(it => new { it.itemTypeid, site }).ExecuteCommand();
            if (updateTestItemType > 0)
            {
                string sqlStr = $"insert INTO SAJET.m_TEST_ITEM_TYPE_HT(select * FROM SAJET.m_TEST_ITEM_TYPE  where item_type_id = '" + imesMTestItemType.itemTypeid + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return updateTestItemType;
        }
        public int UpdateTestItem(ImesMTestItem imesMTestItem, string site)
        {
            int updateTestItemType = Context.Updateable(imesMTestItem).UpdateColumns(it => new { it.itemCode, it.itemName, it.itemDesc, it.itemDesc2, it.hasValue }).WhereColumns(it => new { it.itemId, site }).ExecuteCommand();
            if (updateTestItemType > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_TEST_ITEM_HT(select * FROM SAJET.M_TEST_ITEM  where item_type_id = '" + imesMTestItem.itemTypeid + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return updateTestItemType;
        }
        public int TestItemTypeabled(ImesMTestItemType imesMTestItemType, string site)
        {
            if (imesMTestItemType.enaBled == "Y")
            {
                int updateTestItemType = Context.Updateable(imesMTestItemType).UpdateColumns(it => it.enaBled == "N").WhereColumns(it => it.itemTypeid).ExecuteCommand();
                if (updateTestItemType > 0)
                {
                    string sqlStr = $"insert INTO SAJET.m_TEST_ITEM_TYPE_HT(select * FROM SAJET.m_TEST_ITEM_TYPE  where item_type_id = '" + imesMTestItemType.itemTypeid + "')";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return 1;
                }
            }
            else
            {
                int updateTestItemType = Context.Updateable(imesMTestItemType).UpdateColumns(it => it.enaBled == "Y").WhereColumns(it => it.itemTypeid).ExecuteCommand();
                if (updateTestItemType > 0)
                {
                    string sqlStr = $"insert INTO SAJET.m_TEST_ITEM_TYPE_HT(select * FROM SAJET.m_TEST_ITEM_TYPE   where item_type_id = '" + imesMTestItemType.itemTypeid + "')";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return 1;
                }
            }
            return 0;
        }
        public int DeleteTestItemType(ImesMTestItemType imesMTestItemType)
        {
            int id = imesMTestItemType.itemTypeid;
            string site = imesMTestItemType.site;
            string insertHt = $"insert INTO SAJET.m_TEST_ITEM_TYPE_HT(select * FROM SAJET.m_TEST_ITEM_TYPE  where item_type_id = " + id + " and site = '" + site + "'";
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            return Context.Deleteable<ImesMTestItemType>().Where(it => it.itemTypeid == id && it.site == site).ExecuteCommand();
        }
        public int DeleteTestItem(ImesMTestItem ImesMTestItem)
        {
            int id = ImesMTestItem.itemId;
            string site = ImesMTestItem.site;
            string insertHt = $"insert INTO SAJET.M_TEST_ITEM_HT(select * FROM SAJET.M_TEST_ITEM  where item_id = " + id + " and site = '" + site + "'";
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            return Context.Deleteable<ImesMTestItem>().Where(it => it.itemId == id && it.site == site).ExecuteCommand();
        }
        public PagedInfo<ImesMTestItem> TestitemList(String enaBled, string itemTypeid, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMTestItem>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(itemTypeid != "" && itemTypeid != null, it => it.itemTypeid.ToString() == itemTypeid);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enaBled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "itemCode", it => it.itemCode.Contains(textData));
                exp.AndIF(optionData == "itemDesc", it => it.itemDesc.Contains(textData));
                exp.AndIF(optionData == "itemDesc2", it => it.itemDesc2.Contains(textData));
                exp.AndIF(optionData == "itemName", it => it.itemName.Contains(textData));
                exp.AndIF(optionData == "hasValue", it => it.hasValue.Contains(textData));
                exp.AndIF(optionData == "valueType", it => it.valueType.Contains(textData));
            }
            var expPagedInfo = Context.Queryable<ImesMTestItem>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
            return expPagedInfo;
        }
        public int TestItemabled(ImesMTestItem imesMTestItem, string site)
        {
            if (imesMTestItem.enaBled == "Y")
            {
                int updateTestItemType = Context.Updateable(imesMTestItem).UpdateColumns(it => it.enaBled == "N").WhereColumns(it => it.itemId).ExecuteCommand();
                if (updateTestItemType > 0)
                {
                    string sqlStr = $"insert INTO SAJET.M_TEST_ITEM_HT(select * FROM SAJET.M_TEST_ITEM  where item_type_id = '" + imesMTestItem.itemId + "')";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return 1;
                }
            }
            else
            {
                int updateTestItemType = Context.Updateable(imesMTestItem).UpdateColumns(it => it.enaBled == "Y").WhereColumns(it => it.itemTypeid).ExecuteCommand();
                if (updateTestItemType > 0)
                {
                    string sqlStr = $"insert INTO SAJET.M_TEST_ITEM_HT(select * FROM SAJET.IMES.M_TEST_ITEM  where item_type_id = '" + imesMTestItem.itemId + "')";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return 1;
                }
            }
            return 0;
        }
    }
}
