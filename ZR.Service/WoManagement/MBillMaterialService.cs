using System;
using SqlSugar;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using System.Linq;
using ZR.Common;

namespace ZR.Service.Business
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IMBillMaterialService), ServiceLifetime = LifeTime.Transient)]
    public class MBillMaterialService : BaseService<MBillMaterial>, IMBillMaterialService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MBillMaterialDto> GetList(MBillMaterialQueryDto parm)
        {
            var predicate = Expressionable.Create<MBillMaterial>();

            var response = Queryable()
                .Where(predicate.ToExpression()).OrderByDescending(it=>it.CreateTime)
                .ToPage<MBillMaterial, MBillMaterialDto>(parm);


            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MBillMaterial GetInfo(string Id)
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
        public MBillMaterial AddMBillMaterial(MBillMaterial model)
        {
            model.Id = Tools.GenerateLongUUID().ToString();
            return Context.Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateMBillMaterial(MBillMaterial model)
        {
            //var response = Update(w => w.Id == model.Id, it => new MBillMaterial()
            //{
            //    ItemVersion = model.ItemVersion,
            //    StationType = model.StationType,
            //    UpdateEmpno = model.UpdateEmpno,
            //    UpdateTime = model.UpdateTime,
            //    CreateTime = model.CreateTime,
            //    ItemCount = model.ItemCount,
            //    Ipn = model.Ipn,
            //    SPEC1 = model.SPEC1,
            //    Version = model.Version,
            //    ItemIpn = model.ItemIpn,
            //    ItemSpec1 = model.ItemSpec1,
            //    ItemGroup = model.ItemGroup,
            //    Site = model.Site,
            //});
            //return response;
            return Update(model, true);
        }

    }
}