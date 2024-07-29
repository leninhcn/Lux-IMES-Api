using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Service.IService;
using ZR.Repository;
using Infrastructure.Enums;

namespace ZR.Service.PordService
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    internal class ProdCallPartService : BaseService<SnStatus>, IProdCallPartService
    {
        public PagedInfo<ImesMCallPart> CallPartList(string enabled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMCallPart>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enabled != "" && enabled != null, it => it.enabled == enabled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "partType", it => it.partType.Contains(textData));
                exp.AndIF(optionData == "shiftType", it => it.shiftType.Contains(textData));
                exp.AndIF(optionData == "ipn", it => it.ipn.Contains(textData));
                exp.AndIF(optionData == "qty", it => it.qty.ToString().Contains(textData));
                exp.AndIF(optionData == "line", it => it.line.Contains(textData));
                exp.AndIF(optionData == "createEmp", it => it.createEmp.Contains(textData));
                exp.AndIF(optionData == "createTime", it => it.createTime.ToString().Contains(textData));
                exp.AndIF(optionData == "option1", it => it.option1.Contains(textData));
                exp.AndIF(optionData == "option3", it => it.option3.ToString().Contains(textData));
            }
            var expPagedInfo = Context.Queryable<ImesMCallPart>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
            return expPagedInfo;
        }
        public PagedInfo<IsmtMMaterialnfo> Ipnlist(string type,string enabled, string optionData, string textData, int pageNum, int pageSize)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<IsmtMMaterialnfo>();
            exp.AndIF(type != "" && type != null, it => it.materialType == type);
            exp.AndIF(enabled != "" && enabled != null, it => it.enabled == enabled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "materialPn", it => it.materialPn.Contains(textData));
                exp.AndIF(optionData == "materialCname", it => it.materialCname.Contains(textData));
                exp.AndIF(optionData == "vendor", it => it.vendor.Contains(textData));
                exp.AndIF(optionData == "modelDesc", it => it.modelDesc.ToString().Contains(textData));
            }
            var expPagedInfo = Context.Queryable<IsmtMMaterialnfo>().Where(exp.ToExpression()).OrderBy(it => it.materialPn).ToPage(pager);
            return expPagedInfo;
        }
        public object History(string id, string site)
        {
            string sql = string.Format(@"select * from imes.M_CallPart where id = '" + id + "'and site ='"+ site + "'");
            return Context.Ado.SqlQuery<Object>(sql);
        }
        public object LineList(string site)
        {
            string sql = string.Format(@"SELECT LINE FROM IMES.M_LINE WHERE ENABLED='Y'and site = '"+site+"'");
            return Context.Ado.SqlQuery<Object>(sql);
        }
        //public object Ipnlist(string type, string site)
        //{
        //    string sql = string.Format(@"select MATERIAL_PN,MATERIAL_CNAME ,VENDOR ,MODEL_DESC  from ISMT.M_MATERIAL_INFO where ENABLED='Y' and MATERIAL_TYPE='" + type + "' order by MATERIAL_PN");
        //    return Context.Ado.SqlQuery<Object>(sql);ImesPwoBase
        //}
        public PagedInfo<ImesPwoBase> WorkOrder( string optionData, string textData, int pageNum, int pageSize,string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesPwoBase>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "workOrder", it => it.workOrder.Contains(textData));
                exp.AndIF(optionData == "woType", it => it.woType.Contains(textData));
                exp.AndIF(optionData == "model", it => it.model.Contains(textData));
                exp.AndIF(optionData == "ipn", it => it.ipn.ToString().Contains(textData));
                exp.AndIF(optionData == "targetQty", it => it.targetQty.ToString().Contains(textData));
                exp.AndIF(optionData == "woCreateDate", it => it.woCreateDate.ToString().Contains(textData));
            }
            var expPagedInfo = Context.Queryable<ImesPwoBase>().Where(exp.ToExpression()).OrderBy(it => it.woCreateDate).ToPage(pager);
            return expPagedInfo;
        }
        public (string, object, object) CallPartImport(List<ImesMCallPart> imes, string site, string name)
        {
            try
            {
                int addNum = 0;
                int errNum = 0;
                int upNum = 0;
                imes.ForEach(x =>
                {
                    x.createTime = DateTime.Now;
                    x.updateTime = DateTime.Now;
                    x.createEmp = name;
                    x.updateEmp = name;
                    x.site = site;
                    if (x.Id == "")
                    {
                        int MesInsert = this.InsertCallPart(x);
                        if (MesInsert > 0)
                        {
                            addNum++;
                        }
                        else
                        {
                            errNum++;
                        }

                    }
                    else
                    {
                        int insertErp = Context.Insertable(x).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                        if (insertErp > 0)
                        {
                            upNum++;
                        }
                        else
                        {
                            errNum++;
                        }
                    }
                });
                string msg = "总数:" + imes.Count;
                return (msg, " 添加:" + addNum, " 修改:" + upNum + " 失败:" + errNum);
            }
            catch (Exception e)
            {
                return ("上传失败，总数:", imes.Count + " 添加:0", " 修改:0");
            }
        }
        public int CallPartabled(ImesMCallPart imesMCallPart, string site)
        {
            if (imesMCallPart.enabled == "Y")
            {
                int updateCallPart = Context.Updateable(imesMCallPart).UpdateColumns(it => it.enabled == "N").WhereColumns(it => it.Id).ExecuteCommand();
                if (updateCallPart > 0)
                {
                    string sqlStr = $"insert into IMES.M_CallPart_HT(select * from IMES.M_CallPart  where id = '" + imesMCallPart.Id+ "')";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return 1;
                }
            }
            else
            {
                int updateCallPart = Context.Updateable(imesMCallPart).UpdateColumns(it => it.enabled == "Y").WhereColumns(it => it.Id).ExecuteCommand();
                if (updateCallPart > 0)
                {
                    string sqlStr = $"insert into IMES.M_CallPart_HT(select * from IMES.M_CallPart   where id = '" + imesMCallPart.Id + "')";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return 1;
                }
            }
            return 0;
        }
        public int InsertCallPart(ImesMCallPart imesMCallPart)
        {
            string site = imesMCallPart.site;
            imesMCallPart.createTime = DateTime.Now;
            imesMCallPart.updateTime = DateTime.Now;
            int insertErp = Context.Insertable(imesMCallPart).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            string sql = $"update  imes.M_CallPart set  id =(select sys_guid() from dual) where option1 = '" + imesMCallPart.option1+ "'";
            Context.Ado.SqlQuery<string>(sql);
            if (insertErp > 0)
            {
                string sqlStr1 = $"insert into imes.M_CallPart_HT(select * from imes.M_CallPart where option1 = '"+ imesMCallPart.option1+"'";
                Context.Ado.SqlQuery<string>(sqlStr1 + ")");
                return 1;
            }
            return 1;
          
        }
        public int UpdateCallPart(ImesMCallPart imesMCallPart, string site)
        {
            string sql = string.Format(@"select option2 from imes.M_CallPart where id = '" + imesMCallPart.Id + "'");
            string option2 = Context.Ado.SqlQuery<Object>(sql).ToString();
            if(option2 =="Y")
            {
                return 0;
            }
            int updateCallPart = Context.Updateable(imesMCallPart).UpdateColumns(it => new { it.ipn, it.qty, it.line, it.partType, it.shiftType, it.option1, it.option3 }).WhereColumns(it => new { it.Id, site }).ExecuteCommand();
            if (updateCallPart > 0)
            {
                string sqlStr = $"insert into IMES.M_CallPart_HT(select * from IMES.M_CallPart  where id = '" + imesMCallPart.Id + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return updateCallPart;
        }
        public int DeleteCallPart(ImesMCallPart imesMCallPart, string site)
        {
            string sql = string.Format(@"select option2 from imes.M_CallPart where id = '" + imesMCallPart.Id + "'");
            string option2 = Context.Ado.SqlQuery<Object>(sql).ToString();
            if (option2 == "Y")
            {
                return 0;
            }
            string id = imesMCallPart.Id;
            site = imesMCallPart.site;
            string insertHt = $"insert into IMES.M_CallPart_HT(select * from IMES.M_CallPart  where id = '" + id + "' and site = '" + site + "'";
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            return Context.Deleteable<ImesMCallPart>().Where(it => it.Id == id && it.site == site).ExecuteCommand();
        }
    }
}
