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
namespace ZR.Service.Quality
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IPSnBlackListService), ServiceLifetime = LifeTime.Transient)]
    public class PSnBlackListService : BaseService<PSnBlackList>, IPSnBlackListService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PSnBlackListDto> GetList(PSnBlackListQueryDto parm)
        {
            var predicate = Expressionable.Create<PSnBlackList>();
            var exp = predicate.AndIF(!string.IsNullOrEmpty(parm.Kpsn), black=> black.Kpsn.Equals(parm.Kpsn))
                               .AndIF(!string.IsNullOrEmpty(parm.BlackType),black=>black.BlackType.Equals(parm.BlackType))
                               .AndIF(!string.IsNullOrEmpty(parm.VendorCode),black=>black.VendorCode.Equals(parm.VendorCode))
                               .AndIF(!string.IsNullOrEmpty(parm.DefectCode),black=>black.DefectCode.Equals(parm.DefectCode));
            var response = Queryable()
                .Where(exp.ToExpression())
                .ToPage<PSnBlackList, PSnBlackListDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PSnBlackList GetInfo(int Id)
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
        public PSnBlackList AddPSnBlackList(PSnBlackList model)
        {
            PSnBlackList res = null;
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
        public int UpdatePSnBlackList(PSnBlackList model)
        {
            //var response = Update(w => w.Id == model.Id, it => new PSnBlackList()
            //{
            //    Kpsn = model.Kpsn,
            //    VendorCode = model.VendorCode,
            //    DefectCode = model.DefectCode,
            //    CreateUser = model.CreateUser,
            //    CreateTime = model.CreateTime,
            //    StationId = model.StationId,
            //    Message = model.Message,
            //    Line = model.Line,
            //    UpdateUser = model.UpdateUser,
            //    UpdateTime = model.UpdateTime,
            //    Enabled = model.Enabled,
            //    BlackType = model.BlackType,
            //    StationType = model.StationType,
            //    TestStationName = model.TestStationName,
            //});
            //return response;
            return Update(model, true);
        }

    }




}
