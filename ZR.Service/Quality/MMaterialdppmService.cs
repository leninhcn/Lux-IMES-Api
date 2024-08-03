using Infrastructure.Attribute;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Dto.Quality;
using ZR.Model.Quality;
using ZR.Model;
using ZR.Service.Quality.IQualityService;
using ZR.Repository;
using System.Data;

namespace ZR.Service.Quality
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IMMaterialdppmService), ServiceLifetime = LifeTime.Transient)]
    public class MMaterialdppmService : BaseService<MMaterialdppm>, IMMaterialdppmService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MMaterialdppmDto> GetList(MMaterialdppmQueryDto parm)
        {
            var predicate = Expressionable.Create<MMaterialdppm>();

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<MMaterialdppm, MMaterialdppmDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MMaterialdppm GetInfo(int Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MMaterialdppm AddMMaterialdppm(MMaterialdppm model)
        {

            MMaterialdppm res = null;
            var rep = UseTran(() => {
                //1.查询获取数据表的总行数
                //2.更新model的ID
                //3.插入到表中
                var totalcount = Queryable().Count();
                model.Id = totalcount + 1;
                res = Context.Insertable(model).ExecuteReturnEntity();
            });
            return res;

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateMMaterialdppm(MMaterialdppm model)
        {
            //var response = Update(w => w.Id == model.Id, it => new MMaterialdppm()
            //{
            //    Id = model.Id,
            //    OPTION5 = model.OPTION5,
            //    Week = model.Week,
            //    Uploaddate = model.Uploaddate,
            //    Model = model.Model,
            //    SerialNumber = model.SerialNumber,
            //    Ngdescription = model.Ngdescription,
            //    Ngrate = model.Ngrate,
            //    Dppm = model.Dppm,
            //    Receivedate = model.Receivedate,
            //    Rmanumber = model.Rmanumber,
            //    Status = model.Status,
            //    Report = model.Report,
            //    Remark = model.Remark,
            //    UpdateEmpno = model.UpdateEmpno,
            //    UpdateTime = model.UpdateTime,
            //    CreateEmpno = model.CreateEmpno,
            //    CreateTime = model.CreateTime,
            //    Enabled = model.Enabled,
            //    OPTION1 = model.OPTION1,
            //    OPTION2 = model.OPTION2,
            //    OPTION3 = model.OPTION3,
            //    OPTION4 = model.OPTION4,
            //    Month = model.Month,
            //});
            //return response;
            return Update(model, true);
        }

        public DataTable GetModels()
        {
            var sql = "SELECT DISTINCT MODEL FROM SAJET.M_MODEL";
            return SqlQuery(sql);
        }
    }




}
