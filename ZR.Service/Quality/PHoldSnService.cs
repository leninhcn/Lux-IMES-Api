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
using Aliyun.OSS;
using System.Data;

namespace ZR.Service.Quality
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IPHoldSnService), ServiceLifetime = LifeTime.Transient)]
    public class PHoldSnService : BaseService<PHoldSn>, IPHoldSnService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PHoldSnDto> GetList(PHoldSnQueryDto parm)
        {
            var predicate = Expressionable.Create<PHoldSn>();
            var exp = predicate.AndIF(!string.IsNullOrEmpty(parm.Pn), h => h.Pn.Equals(parm.Pn.Trim()))
                     .AndIF(!string.IsNullOrEmpty(parm.Wo), h => h.Wo.Equals(parm.Wo.Trim()))
                     .AndIF(!string.IsNullOrEmpty(parm.Sn), h => h.Sn.Equals(parm.Sn.Trim()))
                     .AndIF(!string.IsNullOrEmpty(parm.Panel), h => h.Panel.Equals(parm.Panel.Trim()));
            var res = Queryable();
            var response = Queryable()
                .Where(exp.ToExpression())
                .ToPage<PHoldSn, PHoldSnDto>(parm);

            return response;
        }



        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PHoldSn GetInfo(int Id)
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
        public PHoldSn AddPHoldSn(PHoldSn model)
        {
            return Context.Insertable(model).ExecuteReturnEntity();
        }

        public PHoldSn AddPHoldSnByTran(PHoldSn model)
        {
            PHoldSn res = null;
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
        public int UpdatePHoldSn(PHoldSn model)
        {
            //var response = Update(w => w.Id == model.Id, it => new PHoldSn()
            //{
            //    Part = model.Part,
            //    PanelNo = model.PanelNo,
            //    Sn = model.Sn,
            //    Stage = model.Stage,
            //    StationType = model.StationType,
            //    HoldReason = model.HoldReason,
            //    UnholdReson = model.UnholdReson,
            //    HoldEmpno = model.HoldEmpno,
            //    HoldTime = model.HoldTime,
            //    UnholdEmpno = model.UnholdEmpno,
            //    UnholdTime = model.UnholdTime,
            //    CreateEmpno = model.CreateEmpno,
            //    CreateTime = model.CreateTime,
            //    Enabled = model.Enabled,
            //    Islms = model.Islms,
            //    StationName = model.StationName,
            //    MainSn = model.MainSn,
            //    Wo = model.Wo,
            //});
            //return response;
            return Update(model, true);
        }

        public int UpdatePHoldSns(PHoldSn[] parm)
        {
            int res = 0;
            var rep = UseTran(() => {
                
                parm.ToList().ForEach(p=> {
                    res += Update(p, true);
                });
;
            });

            return res;
        }
    }
}
