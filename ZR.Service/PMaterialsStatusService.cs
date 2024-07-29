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
using Infrastructure;
using System.Data;
using System.Runtime.Serialization;
using ZR.Common;

namespace ZR.Service.Business
{
    /// <summary>
    /// 辅材上下线Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IPMaterialsStatusService), ServiceLifetime = LifeTime.Transient)]
    public class PMaterialsStatusService : BaseService<PMaterialsStatus>, IPMaterialsStatusService
    {
        /// <summary>
        /// 查询辅材上下线列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PMaterialsStatusDto> GetList(PMaterialsStatusQueryDto parm)
        {
            var predicate = Expressionable.Create<PMaterialsStatus>();
            predicate.And(i => i.Site == parm.Site);
            predicate.And(i => i.Enabled == "Y");
            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<PMaterialsStatus, PMaterialsStatusDto>(parm);
            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="MaterialsId"></param>
        /// <returns></returns>
        public PMaterialsStatus GetInfo(long MaterialsId)
        {
            var response = Queryable()
                .Where(x => x.Id == MaterialsId)
                .First();
                
            return response;
        }
        /**
     * 查询哪些物料可以进行上料
     */
        List<string> BomList(PMaterialsStatus model)
        {
            List<string> woBom = Context.Queryable<WoBom>()
           .Where(i => i.WorkOrder == model.WorkOrder)
           //.Where(i => i.StationType == model.StationType)
           .Where(i => i.Site == model.Site).Select(s => s.ItemIpn).ToList();
            if (woBom.Count <= 0)
            {
                throw new CustomException("此工单不允许上料:未查询到工单BOM");
            }
            return woBom;
        }
        /// <summary>
        /// 添加辅材上下线
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddPMaterialsStatus(PMaterialsStatus model)
        {

            var MACHINE = Context.Ado.SqlQuerySingle<dynamic>(
                  @"
                   SELECT M.MACHINE_CODE,M.MACHINE_DESC,M.MACHINE_TYPE,
                   G.STATION_TYPE,M.STAGE,M.LINE FROM IMES.M_MACHINE M
                   LEFT JOIN IMES.M_MACHINE_GROUP G ON (M.GROUP_ID=G.ID)
                   WHERE ROWNUM= 1 AND M.MACHINE_CODE =@MACHINE_CODE
                   AND M.SITE=@SITE AND M.ENABLED =@ENABLED",
                new List<SugarParameter> { 
                    new SugarParameter("@MACHINE_CODE", Tools.FormatStr(model.MachineCode)),
                    new SugarParameter("@SITE", model.Site),
                    new SugarParameter("@ENABLED",model.Enabled)
                });


            if (MACHINE == null)
            {
                throw new CustomException("上料失败，未查询到设备");
            }


            List<string> woBom = BomList(model);

            model.StationType = MACHINE.STATION_TYPE;
            model.Line = MACHINE.LINE;
            model.MachineType = MACHINE.MACHINE_TYPE;
            model.Stage = MACHINE.STAGE;

            //通过扫描的批次号查询查询发料记录表
            string sSQL = @"SELECT REEL_NO,REEL_QTY,KEEP_QTY, IPN,LOT_NO  From IMES.P_MATERIAL WHERE 
            REEL_NO= '" + Tools.FormatStr(model.Batchno) + "'  AND SITE = '" + model.Site + "' ";
            var MATERIAL = Context.Ado.SqlQuerySingle<dynamic>(sSQL);


            //如果有发料记录的话就直接进行上料,没有记录的话就查询特征码上料
            if (MATERIAL != null)
            {
                if (!woBom.Contains(MATERIAL.IPN))
                {
                    throw new CustomException("IPN不一致不允许上料");
                }
            }
            else
            {
                throw new CustomException("没有发料记录");
                //工单BOM物料的特征码
                //List<SnFeature> snFeature = Context.Queryable<SnFeature>().In(g => g.Ipn, woBom).ToList();
                ////匹配特征码,看工单BOM的特征码是否包含发料的IPN
                //if (!snFeature.Any(s => s.Ipn == %))
                //{
                //    throw new CustomException("特征码不匹配");
                //}
            }
            return Context.Insertable(model).ExecuteCommand();

        }

        /// <summary>
        /// 修改辅材上下线
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdatePMaterialsStatus(PMaterialsStatus model)
        {
            //var response = Update(w => w.MaterialsId == model.MaterialsId, it => new PMaterialsStatus()
            //{
            //    ScanType = model.ScanType,
            //    UpdateTime = model.UpdateTime,
            //    Ipn = model.Ipn,
            //    MateSum = model.MateSum,
            //    MachineCode = model.MachineCode,
            //    MachineName = model.MachineName,
            //    MachineLoc = model.MachineLoc,
            //    MachineStauts = model.MachineStauts,
            //    PartType = model.PartType,
            //    LineId = model.LineId,
            //    Stage = model.Stage,
            //    StationType = model.StationType,
            //    Delflag = model.Delflag,
            //    Site = model.Site,
            //    CreateEmpno = model.CreateEmpno,
            //    UpdateEmpno = model.UpdateEmpno,
            //    CreateTime = model.CreateTime,
            //    WorkOrder = model.WorkOrder,
            //});
            //return response;
            return Update(model, true);
        }

    }
}