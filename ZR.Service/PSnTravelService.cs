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
using Microsoft.IdentityModel.Tokens;

namespace ZR.Service.Business
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IPSnTravelService), ServiceLifetime = LifeTime.Transient)]
    public class PSnTravelService : BaseService<PSnTravel>, IPSnTravelService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PSnTravelDto> GetList(PSnTravelQueryDto parm)
        {
          
            var predicate = Expressionable.Create<PSnTravel>();
            predicate.AndIF(!parm.WorkOrder.IsNullOrEmpty(),i=>i.WorkOrder.Contains(parm.WorkOrder));
            predicate.AndIF(!parm.StationType.IsNullOrEmpty(), i => i.StationType.Contains(parm.StationType));
            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<PSnTravel, PSnTravelDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PSnTravel GetInfo(int Id)
        {
            var response = Queryable()
                .Where(x => x.WorkOrder == Id.ToString())
                .First();

            return response;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PSnTravel AddPSnTravel(PSnTravel model)
        {
            return Context.Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdatePSnTravel(PSnTravel model)
        {
            //var response = Update(w => w.Id == model.Id, it => new PSnTravel()
            //{
            //    WorkOrder = model.WorkOrder,
            //    LocationNo = model.LocationNo,
            //    SerialNumber = model.SerialNumber,
            //    Ipn = model.Ipn,
            //    Version = model.Version,
            //    RouteName = model.RouteName,
            //    Line = model.Line,
            //    Stage = model.Stage,
            //    StationType = model.StationType,
            //    ClientType = model.ClientType,
            //    StationName = model.StationName,
            //    NextStationType = model.NextStationType,
            //    CurrentStatus = model.CurrentStatus,
            //    WorkFlag = model.WorkFlag,
            //    InStationtypeTime = model.InStationtypeTime,
            //    OutStationtypeTime = model.OutStationtypeTime,
            //    InLineTime = model.InLineTime,
            //    OutLineTime = model.OutLineTime,
            //    PalletNo = model.PalletNo,
            //    CartonNo = model.CartonNo,
            //    QcNo = model.QcNo,
            //    QcResult = model.QcResult,
            //    Customer = model.Customer,
            //    ReworkNo = model.ReworkNo,
            //    EmpNo = model.EmpNo,
            //    CustomerSn = model.CustomerSn,
            //    WipStationType = model.WipStationType,
            //    WipQty = model.WipQty,
            //    BoxNo = model.BoxNo,
            //    PanelNo = model.PanelNo,
            //    RcNo = model.RcNo,
            //    HoldFlag = model.HoldFlag,
            //    PassCnt = model.PassCnt,
            //    StateFlag = model.StateFlag,
            //    StateDesc = model.StateDesc,
            //    SnVersion = model.SnVersion,
            //    RpRoute = model.RpRoute,
            //    CreateTime = model.CreateTime,
            //    OPTION1 = model.OPTION1,
            //    OPTION2 = model.OPTION2,
            //    OPTION3 = model.OPTION3,
            //    OPTION4 = model.OPTION4,
            //    OPTION5 = model.OPTION5,
            //    SnCounter = model.SnCounter,
            //    MachineNo = model.MachineNo,
            //    ToolingNo = model.ToolingNo,
            //    CavityNo = model.CavityNo,
            //    ShippingNo = model.ShippingNo,
            //    WarehouseNo = model.WarehouseNo,
            //    Model = model.Model,
            //});
            //return response;
            return Update(model, true);
        }

    }
}