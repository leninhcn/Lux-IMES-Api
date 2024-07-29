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

namespace ZR.Service.Business
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IMStationTypeService), ServiceLifetime = LifeTime.Transient)]
    public class MStationTypeService : BaseService<MStationType>, IMStationTypeService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MStationTypeDto> GetList(MStationTypeQueryDto parm)
        {
            var predicate = Expressionable.Create<MStationType>();
            predicate.AndIF(!parm.StationType.IsEmpty(),i => i.StationType.Contains(parm.StationType));
            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<MStationType, MStationTypeDto>(parm);
            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MStationType GetInfo(int Id)
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
        public MStationType AddMStationType(MStationType model)
        {
            return Context.Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateMStationType(MStationType model)
        {
            //var response = Update(w => w.Id == model.Id, it => new MStationType()
            //{
            //    Id = model.Id,
            //    MaterialLoadFlag = model.MaterialLoadFlag,
            //    StationtypeCustomer = model.StationtypeCustomer,
            //    OperateType = model.OperateType,
            //    ClientType = model.ClientType,
            //    StationTypeSeq = model.StationTypeSeq,
            //    Stage = model.Stage,
            //    StationTypeDesc = model.StationTypeDesc,
            //    CustomerStationDesc = model.CustomerStationDesc,
            //    UpdateEmpno = model.UpdateEmpno,
            //    UpdateTime = model.UpdateTime,
            //    CreateEmpno = model.CreateEmpno,
            //    CreateTime = model.CreateTime,
            //    Enabled = model.Enabled,
            //    DcCmd = model.DcCmd,
            //    Fpp = model.Fpp,
            //    CurrentCt = model.CurrentCt,
            //    StationType = model.StationType,
            //});
            //return response;
            return Update(model, true);
        }

    }
}