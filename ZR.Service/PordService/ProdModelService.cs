using Aliyun.OSS;
using Infrastructure;
using Infrastructure.Attribute;
using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using NLog.Filters;
using Org.BouncyCastle.Crypto;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Dto.ProdDto;
using ZR.Model.Quality;
using ZR.Model.System;
using ZR.Model.System.Generate;
using ZR.Repository;
using ZR.Service.IService;
using ZR.Service.Quality.IQualityService;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static System.Collections.Specialized.BitVector32;
using static ZR.Model.Dto.ProdDto.ImesMmodel;

namespace ZR.Service.PordService
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    internal class ProdModelService : BaseService<SnStatus>, IProdModelService
    {
        public PagedInfo<ImesMmodel> getModelData(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMmodel>()
            .AndIF(site != "" && site != null, it => it.site == site)
            .AndIF(enaBled != "" && enaBled != null, it => it.EnaBled == enaBled);

            if (textData!=null&&textData!="") {
                exp.AndIF(optionData == "MODEL", it => it.Model.Contains(textData));
                exp.AndIF(optionData == "MODEL_DESC", it => it.ModelDesc.Contains(textData));
                exp.AndIF(optionData == "UPDATE_TIME", it => it.UpdateTime.ToString().Contains(textData));
                exp.AndIF(optionData == "UPDATE_EMPNO", it => it.UpdateEmpno.Contains(textData));
                exp.AndIF(optionData == "MODEL_CUSTOMER", it => it.ModelCustomer.Contains(textData));
            }
            return Context.Queryable<ImesMmodel>().Where(exp.ToExpression()).OrderBy(it=>it.Model).ToPage(pager);
        }

        public int getModelInsert(ImesMmodel mmodel)
        {

            int id =  Context.Queryable<ImesMmodel>().Max(it => it.ID);
            mmodel.ID = id + 1;
            return Context.Insertable(mmodel).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
        }

        public int getModelUpdate(ImesMmodel mmodel)
        {
            // JSON转实体类
            ImesMmodelHt imes = JsonConvert.DeserializeObject<ImesMmodelHt>(JsonConvert.SerializeObject(mmodel));
            mmodel.UpdateTime = DateTime.Now;

            int result = Context.Insertable(imes).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand(); 
            if (result > 0) {
                return Context.Updateable(mmodel).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.ID,it.site }).ExecuteCommand();
            }
            return 0;
        }

        public Object ImesMmodelHtlist(int id , string model, string site)
        {
            var exp = Expressionable.Create<ImesMmodelHt>()
                .And(it => it.ID == id)
                //.And(it => it.Model == model)
                .AndIF(site != "" && site != null, it => it.site == site);
            return Context.Queryable<ImesMmodelHt>().Where(exp.ToExpression()).ToList();
        }

        public int PordModelDelete(ImesMmodel mmodel)
        {
            ImesMmodelHt imes = JsonConvert.DeserializeObject<ImesMmodelHt>(JsonConvert.SerializeObject(mmodel));
            mmodel.UpdateTime = DateTime.Now;
            Context.Updateable(mmodel).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.ID, it.site }).ExecuteCommand();

            int result = Context.Insertable(imes).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (result > 0)
            {
                string site = mmodel.site;
                var exp = Expressionable.Create<ImesMmodel>()
                .And(it => it.ID == mmodel.ID)
                .AndIF(site != "" && site != null, it => it.site == site);

                return Context.Deleteable<ImesMmodel>().Where(exp.ToExpression()).ExecuteCommand();
            }
            return 0;
        }


        //-------------分割线-------------------------------------------------------------------------------------------------


        public PagedInfo<MPartHtData> PordPartList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;

            var exp = Context.Queryable<ImesMpart>()
             .LeftJoin<ImesMqcSamplingDefault>((o, i) => o.ipn == i.ipn)
             .LeftJoin<ImesMqcSamplingPlan>((o, i, c) => i.samplingId == c.samplingId)
             .WhereIF(enaBled != null && enaBled != "", o => o.enabled == enaBled)
             .WhereIF(site != null && site != "", o => o.site == site);

            if (textData != null && textData != "")
            {
                exp.WhereIF(optionData == "ipn", o => o.ipn.Contains(textData));//62
                exp.WhereIF(optionData == "apn", o => o.apn.Contains(textData));
                exp.WhereIF(optionData == "partType", o => o.partType.Contains(textData));
                exp.WhereIF(optionData == "spec1", o => o.spec1.Contains(textData));
                exp.WhereIF(optionData == "spec2", o => o.spec2.Contains(textData));
                exp.WhereIF(optionData == "version", o => o.version.Contains(textData));
                exp.WhereIF(optionData == "model", o => o.model.Contains(textData));
                exp.WhereIF(optionData == "modelCustomer", o => o.modelCustomer.Contains(textData));
                exp.WhereIF(optionData == "modelNo", o => o.modelNo.Contains(textData));
                exp.WhereIF(optionData == "vendor", o => o.vendor.Contains(textData));
                exp.WhereIF(optionData == "updateEmpno", o => o.updateEmpno.Contains(textData));
                exp.WhereIF(optionData == "updateTime", o => o.updateTime.ToString().Contains(textData));
                exp.WhereIF(optionData == "option1", o => o.option1.Contains(textData));
            }

            exp.OrderBy(o => o.createTime, OrderByType.Desc)
             .Select((o, i, c) => 
             new MPartHtData { 
                 id = o.id.SelectAll(), samplingType = c.samplingType 
             }).ToPage(pager);

            return exp.OrderBy(o => o.createTime, OrderByType.Desc)
             .Select((o, i, c) =>
             new MPartHtData
             {
                 id = o.id.SelectAll(),
                 samplingType = c.samplingType
             }).ToPage(pager);
        }

        public object PlanList(string site)
        {

            string sql = @"Select SAMPLING_TYPE  From imes.m_QC_SAMPLING_PLAN Where ENABLED = 'Y' and site = '"+site+"'  Order By SAMPLING_TYPE";
            return Context.Ado.SqlQuery<Object>(sql);
        }

        public (string, object, object) PordPartImportData(List<MPartHtData> mPartHts, string site, string name)
        {
            int insert = 0;
            int update = 0;
            int fail = 0;
            mPartHts.ForEach(x =>
            {
                x.createTime = DateTime.Now;
                x.updateTime = DateTime.Now;
                x.createEmpno = name;
                x.updateEmpno = name;
                if (x.id == 0)
                {
                    x.id = Context.Queryable<ImesMpartSpecErpMesMapping>().Max(it => it.id) + 1;
                    var result = this.PordPartInsert(x);
                    if (result == "添加成功！")
                    {
                        insert ++;
                    }
                    else {
                        fail++;

                    }
                }
                else {
                    var result = this.PordPartUpdate(x);
                    if (result == "修改成功") 
                    { 
                        update ++;
                    }
                    else 
                    { 
                        fail++; 
                    }
                }

            });
            return ((" 添加:"+insert), (" 修改:"+update+ " 失败" + fail), (" 总共:"+ mPartHts));
        }

        public string PordPartInsert(MPartHtData mPartHt)
        {
            var ipn =  mPartHt.ipn;
            int count = Context.Queryable<ImesMpart>().Where(it => it.ipn == ipn).Count();
            if (count > 0) 
            {
                return ipn + " 数据已存在！";
            }
            var id = Context.Queryable<ImesMpart>().Max(it => it.id)+1;//获取最大ID
            mPartHt.id = id;

            //插入
            ImesMpart mpart = CopyTo<MPartHtData, ImesMpart>(mPartHt);
            int insertMpart = Context.Insertable(mpart).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertMpart > 0)
            {
                InsertPartSampling(ipn, mPartHt.samplingType, mPartHt.updateEmpno, mPartHt.site);
                Context.Ado.SqlQuery<Object>(@"insert into IMES.M_PART_ht (select * from IMES.M_PART a where a.ID =" + id + ")");
                return "添加成功！";
            }
            else {
                return "添加失败！";
            }
        }

        //使用映射实现两个实体类赋值
        public TTarget CopyTo<TSource, TTarget>(TSource source) where TTarget : new()
        {
            TTarget target = new TTarget();
            foreach (var prop in typeof(TSource).GetProperties())
            {
                var targetProp = typeof(TTarget).GetProperty(prop.Name);
                if (targetProp != null && targetProp.CanWrite && prop.CanRead)
                {
                    targetProp.SetValue(target, prop.GetValue(source, null), null);
                }
            }
            return target;
        }

        public string PordPartUpdate(MPartHtData mPartHt)
        {
            ImesMpart mpart = CopyTo<MPartHtData, ImesMpart>(mPartHt);
            string ipn = mPartHt.ipn;
            var id =  mPartHt.id;

            var site = mPartHt.site;
            int updateMpart = Context.Updateable(mpart).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.ipn, it.site }).ExecuteCommand();

            //Console.WriteLine(updateMpart + "--id:" + id + "-ipn:" + ipn + "--site:" + site);
            if (updateMpart > 0)
            {
                Context.Ado.SqlQuery<Object>(@"insert into IMES.M_PART_ht (select * from IMES.M_PART a where a.ID =" + id + ")");
            }
            else { 
                return "修改失败";
            }

            InsertPartSampling(ipn, mPartHt.samplingType, mPartHt.updateEmpno, site);
            return "修改成功";
        }

        public int PordPartDelete(long id, string site,string updateEmpno)
        {
            ImesMpart mpart = new ImesMpart();
            mpart.id = id;
            mpart.site = site;
            mpart.updateTime = DateTime.Now;
            mpart.updateEmpno = updateEmpno;
            Context.Updateable(mpart).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.ipn, it.site }).ExecuteCommand();
            Context.Ado.SqlQuery<Object>(@"insert into IMES.M_PART_ht (select * from IMES.M_PART a where a.ID =" + id + ")");
            return Context.Deleteable<ImesMpart>().Where(it => it.id == id && it.site==site).ExecuteCommand();
        }

        public object PordPartHtlist(int id, string site)
        {
           return Context.Ado.SqlQuery<Object>(@"SELECT * FROM IMES.M_PART_HT A WHERE A.ID = " + id + " and A.site = '" + site + "' ORDER BY A.UPDATE_TIME DESC");
        }

        public void InsertPartSampling(string ipn,string samplingPlan,string updateEmp,string site) {
            string sSQL = " Delete imes.m_QC_SAMPLING_DEFAULT where ipn = '" + ipn + "'";
            Context.Ado.SqlQuery<int>(sSQL);
            if (samplingPlan != null&& samplingPlan != "") 
            {
                List<ImesMqcSamplingPlan> list = Context.Queryable<ImesMqcSamplingPlan>().Where(it=>it.samplingType== samplingPlan && it.site == site).ToList();
                if (list.Count == 0) {
                    return;
                }
                string samplingId = list[0].samplingId;
                sSQL = " Insert Into imes.m_QC_SAMPLING_DEFAULT "
                     + " (ipn,SAMPLING_ID,UPDATE_emp) "
                     + " Values ('" + ipn + "','" + samplingId + "','" + updateEmp + "') ";

                Context.Ado.SqlQuery<int>(sSQL);
            }
        }
    }
}
