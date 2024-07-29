using JinianNet.JNTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.ProdDto;
using ZR.Model;
using ZR.Model.Business;
using ZR.Service.IService;
using ZR.Repository;
using Infrastructure.Attribute;

namespace ZR.Service.PordService
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    internal class MntnVendorCustomerService : BaseService<SnStatus>, IMntnVendorCustomerService
    {

        public PagedInfo<ImesMvendor> VendorList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMvendor>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "vendorName", it => it.vendorName.Contains(textData));
            }
            return Context.Queryable<ImesMvendor>().Where(exp.ToExpression()).OrderBy(it => it.vendorName).ToPage(pager);
        }

        public (string, object, object) VendorImportData(List<ImesMvendor> imes, string site, string name)
        {
            try {
                int add = 0;
                int up = 0;
                int err = 0;
                imes.ForEach(x =>
                {
                    x.createTime = DateTime.Now;
                    x.updateTime = DateTime.Now;
                    x.createEmpno = name;
                    x.updateEmpno = name;
                    x.site = site;
                    if (x.id == 0)
                    {
                        x.id = Context.Queryable<ImesMvendor>().Max(it => it.id) + 1;
                        if (this.VendorInsert(x).Contains("失败"))
                        {
                            err++;
                        }
                        else
                        {
                            add++;
                        }

                    }
                    else
                    {
                        if (this.VendorUpdate(x).Contains("失败"))
                        {
                            err++;
                        }
                        else
                        {
                            up++;
                        }
                    }
                });

                string msg = string.Format(" 插入{0} 更新{1} 错误数据{2}  总共{5}",
                                   add,
                                   up,
                                   err,
                                   imes.Count);
                return (msg, "", "");
            }
            catch (Exception ex) { 
                return ("上传失败!", "", "");
            }
        }

        public string VendorUpdate(ImesMvendor imes)
        {
            string site = imes.site;
            int id = imes.id;
            string insertHt = $"insert into IMES.M_VENDOR_HT(select * from IMES.M_VENDOR  where ID = " + id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            imes.updateTime = DateTime.Now;
            int Updateable = Context.Updateable(imes).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            if (Updateable > 0)
            {
                return  "修改成功！";
            }
            return  "修改失败，联系资讯！";
        }

        public int VendorDelet(ImesMvendor imes)
        {
            int id = imes.id;
            string site = imes.site;
            imes.updateTime = new DateTime();
            Context.Updateable(imes).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            string insertHt = $"insert into IMES.M_VENDOR_HT(select * from IMES.M_VENDOR  where ID = " + id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");

            return Context.Deleteable<ImesMvendor>().Where(it => it.id == id && it.site == site).ExecuteCommand();

        }

        public object VendorListHt(int Id, string site)
        {
            string listHt = $"select * from IMES.M_VENDOR_HT  where ID = " + Id;
            if (site != null && site != "")
            {
                listHt = listHt + " and site = '" + site + "'";
            }
            return Context.Ado.SqlQuery<Object>(listHt);
        }

        public string VendorInsert(ImesMvendor imes)
        {
            imes.updateTime = DateTime.Now;
            imes.createTime = DateTime.Now;
            string site = imes.site;
            int count = Context.Queryable<ImesMvendor>()
                .Where(it =>
                it.vendorCode == imes.vendorCode &&
                it.site == site)
                .Count();
            if (count > 0)
            {
                return imes.vendorCode + "已存在,新增失败!";
            }

            int MaxId = Context.Queryable<ImesMvendor>().Max(it => it.id) + 1;
            imes.id = MaxId;
            int insertMaterial = Context.Insertable(imes).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertMaterial > 0)
            {
                string insertHt = $"insert into IMES.M_VENDOR_HT(select * from IMES.M_VENDOR  where ID = " + MaxId;
                if (site != null && site != "")
                {
                    insertHt = insertHt + " and site = '" + site + "'";
                }
                Context.Ado.SqlQuery<Object>(insertHt + ")");
                return "新增成功";
            }

            return "新增失败，检测数据是否有误！";

        }
        //----------------------------------------------------
        public PagedInfo<ImesmMcustomer> CustomerList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesmMcustomer>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "customerCode", it => it.customerCode.Contains(textData));
            }
            return Context.Queryable<ImesmMcustomer>().Where(exp.ToExpression()).OrderBy(it => it.customerCode).ToPage(pager);
        }

        public string CustomerInsert(ImesmMcustomer imes)
        {
            imes.updateTime = DateTime.Now;
            imes.createTime = DateTime.Now;
            string site = imes.site;
            int count = Context.Queryable<ImesmMcustomer>()
                .Where(it =>
                it.customerCode == imes.customerCode &&
                it.site == site)
                .Count();
            if (count > 0)
            {
                return "Vendor Code Already exists!";
            }

            int MaxId = Context.Queryable<ImesmMcustomer>().Max(it => it.id) + 1;
            imes.id = MaxId;
            int insertMaterial = Context.Insertable(imes).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertMaterial > 0)
            {
                /*string insertHt = $"insert into IMES.M_CUSTOMER_HT(select * from IMES.M_CUSTOMER  where ID = " + MaxId;
                if (site != null && site != "")
                {
                    insertHt = insertHt + " and site = '" + site + "'";
                }
                Context.Ado.SqlQuery<Object>(insertHt + ")");*/
                return "新增成功";
            }

            return "新增失败，检测数据是否有误！";

        }

        public string CustomerUpdate(ImesmMcustomer imes)
        {
            string site = imes.site;
            int id = imes.id;
            /*string insertHt = $"insert into IMES.M_CUSTOMER_HT(select * from IMES.M_CUSTOMER  where ID = " + id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");*/
            imes.updateTime = DateTime.Now;
            int Updateable = Context.Updateable(imes).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            if (Updateable > 0)
            {
                return "修改成功！";
            }
            return "修改失败，联系资讯！";
        }

        //-----------------------------------------------------------------------------------------------------------
        public PagedInfo<ImesMdept> MdeptList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMdept>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "deptCode", it => it.deptCode.Contains(textData));
                exp.AndIF(optionData == "deptName", it => it.deptName.Contains(textData));
                exp.AndIF(optionData == "deptDesc", it => it.deptDesc.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
                exp.AndIF(optionData == "createEmpno", it => it.createEmpno.Contains(textData));
            }
            return Context.Queryable<ImesMdept>().Where(exp.ToExpression()).OrderBy(it => it.updateTime).ToPage(pager);
        }

        public object MdeptListHt(int Id, string site)
        {
            string listHt = $"select * from IMES.M_DEPT_HT  where ID = " + Id;
            if (site != null && site != "")
            {
                listHt = listHt + " and site = '" + site + "'";
            }
            return Context.Ado.SqlQuery<Object>(listHt);
        }


        public string MdeptUpdate(ImesMdept imes)
        {
            string site = imes.site;
            int id = imes.id;
            string insertHt = $"insert into IMES.M_DEPT_HT(select * from IMES.M_DEPT  where ID = " + id + " and site = '" + site + "')";
            Context.Ado.SqlQuery<Object>(insertHt);
            imes.updateTime = DateTime.Now;
            int Updateable = Context.Updateable(imes).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            if (Updateable > 0)
            {
                return "修改成功！";
            }
            return "修改失败，联系资讯！";
        }


        public int MdeptDelet(ImesMdept imes)
        {
            int id = imes.id;
            string site = imes.site;
            imes.updateTime = new DateTime();
            Context.Updateable(imes).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            string insertHt = $"insert into IMES.M_DEPT_HT(select * from IMES.M_DEPT  where ID = " + id + " and site = '" + site + "')";
            Context.Ado.SqlQuery<Object>(insertHt );
            return Context.Deleteable<ImesMdept>().Where(it => it.id == id && it.site == site).ExecuteCommand();
        }


        public string MdeptInsert(ImesMdept imes)
        {
            imes.updateTime = DateTime.Now;
            imes.createTime = DateTime.Now;
            string site = imes.site;
            int count = Context.Queryable<ImesMdept>()
                .Where(it =>
                it.deptName == imes.deptName &&
                it.site == site)
                .Count();
            if (count > 0)
            {
                return "部门名称已存在,新增失败!";
            }

            int MaxId = Context.Queryable<ImesMdept>().Max(it => it.id) + 1;
            imes.id = MaxId;
            int insertMaterial = Context.Insertable(imes).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertMaterial > 0)
            {
                string insertHt = $"insert into IMES.M_DEPT_HT(select * from IMES.M_DEPT  where ID = " + MaxId+ " and site = '" + site + "')";
                Context.Ado.SqlQuery<Object>(insertHt);
                return "新增成功";
            }

            return "新增失败，检测数据是否有误！";

        }

        

        public Object MdeptListFactory()
        {
            string insertHt = $"SELECT ID,SITE FROM IMES.M_SITE WHERE ENABLED='Y'";
            return Context.Ado.SqlQuery<Object>(insertHt);
        }



    }
}
