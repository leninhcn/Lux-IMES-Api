using Infrastructure;
using Infrastructure.Attribute;
using Microsoft.IdentityModel.Tokens;
using SqlSugar.Extensions;
using System.Drawing.Printing;
using ZR.Common;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using ZR.Service.IService;
namespace ZR.Service.Business
{



    /// <summary>
    /// 上下料
    ///
    /// </summary>
    [AppService(ServiceType = typeof(IPFeedingService), ServiceLifetime = LifeTime.Transient)]
    public class PFeedingService : BaseService<PFeeding>, IPFeedingService
    {
        private readonly IMntnWorkOrderService _mntnWorkOrderService;

        public PFeedingService(IMntnWorkOrderService mntnWorkOrderService)
        {
            _mntnWorkOrderService = mntnWorkOrderService;
        }

        /// <summary>
        /// 查询上下料
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PFeedingDto> GetList(PFeedingQueryDto parm)
        {
            var predicate = Expressionable.Create<PFeeding>();
            predicate.AndIF(!parm.WorkOrder.IsNullOrEmpty(), i => i.WorkOrder.Contains(parm.WorkOrder));
            predicate.AndIF(!parm.StationType.IsNullOrEmpty(), i => i.StationType.Contains(parm.StationType));
            predicate.And(i => i.Site == parm.Site);
            predicate.And(i => i.Enabled == "Y");
            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<PFeeding, PFeedingDto>(parm);
            foreach (var item in response.Result)
            {
                WoBase WoBase = Context.Queryable<WoBase>().Where(i => i.WorkOrder == item.WorkOrder).First();
                if (WoBase != null)
                {

                    item.InputQty = WoBase.InputQty;
                }
            }

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PFeeding GetInfo(long Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加上下料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddPFeeding(PFeeding model)
        {
            //处理扫描的数据有空格,换行等格式
            model.MachineCode = Tools.FormatStr(model.MachineCode);
            model.Batchno = Tools.FormatStr(model.Batchno);
          

            var MACHINE = Context.Ado.SqlQuerySingle<dynamic>(
                      @"
                       SELECT M.MACHINE_CODE,M.MACHINE_DESC,M.MACHINE_TYPE,
                       G.STATION_TYPE,M.STAGE,M.LINE FROM IMES.M_MACHINE M
                       LEFT JOIN IMES.M_MACHINE_GROUP G ON (M.GROUP_ID=G.ID)
                       WHERE ROWNUM= 1 AND M.MACHINE_CODE =@MACHINE_CODE
                       AND M.SITE=@SITE AND M.ENABLED =@ENABLED",
            new List<SugarParameter> {
                        new SugarParameter("@MACHINE_CODE", model.MachineCode),
                        new SugarParameter("@SITE", model.Site),
                        new SugarParameter("@ENABLED",model.Enabled)
            });

            if (MACHINE == null)
            {
                throw new CustomException("上料失败，未查询到设备");
            }

            var expandoMACHINE = MACHINE as IDictionary<string, object>;

            string machineCode = expandoMACHINE.ContainsKey("MACHINE_CODE") ? expandoMACHINE["MACHINE_CODE"].ToString() : null;
            string stationType = expandoMACHINE.ContainsKey("STATION_TYPE") ? expandoMACHINE["STATION_TYPE"].ToString() : null;

            PFeeding pFeeding = Context.Queryable<PFeeding>()
            .Where(i => i.MachineCode == machineCode)
            .Where(i => i.WorkOrder == model.WorkOrder)
              .Where(i => i.StationType == stationType)
               .Where(i => i.Enabled == "Y")
                .Where(i => i.Batchno == Tools.FormatStr(model.Batchno))
            .First();

            //如果已经上过料的话就启用状态,重新添加数据
            if (pFeeding != null) {
                pFeeding.Enabled = "N";
                Context.Updateable(pFeeding).UpdateColumns(it => new { it.Enabled}).Where(it => it.Id == pFeeding.Id).ExecuteCommand();
            }



            //通过制程和工单号查询允许上哪些物料
            // List<string> woBom = BomList(model);

            //通过扫描的批次号查询查询发料记录表
            //string sSQL = @"SELECT REEL_NO,REEL_QTY,KEEP_QTY, IPN,LOT_NO  From IMES.P_MATERIAL WHERE 
            //REEL_NO= '" + Tools.FormatStr(model.Batchno) + "'  AND SITE = '" + model.Site + "' ";
            //var MATERIAL = Context.Ado.SqlQuerySingle<dynamic>(sSQL);

            ////如果有发料记录的话就直接进行上料,没有记录的话就查询特征码上料
            //if (MATERIAL != null)
            //{
            //    if (!woBom.Contains(MATERIAL.IPN))
            //    {
            //        throw new CustomException("IPN不一致不允许上料");
            //    }
            //}
            //else {
            //    throw new CustomException("没有发料记录");
            //}

            //long KEEP_QTY = MATERIAL.KEEP_QTY != null ? (long)MATERIAL.KEEP_QTY : 0;
            long KEEP_QTY = 0;

            ////(累计投入数量+产出OK数量)+当前投入数量>工单目标产量 数量就有误
           WoBase WoBase = Context.Queryable<WoBase>().Where(i => i.WorkOrder == model.WorkOrder).First();
            ////工单目标产量
            //long? targetQty = WoBase.TargetQty;
            ////工单目前已投入数量
            //long? inputQty = WoBase.InputQty;
            ////目前已经产出OK数量
            //long? outputQty = WoBase.OutputQty;
            ////当前投入数量/批次里面的
            //long? partCount = KEEP_QTY;
            //if (inputQty + outputQty + partCount > targetQty)
            //{
            //    throw new CustomException("投入数量有误");
            //}

            model.ScanType = "上料";
            model.PartCount = KEEP_QTY;
            model.StationType = MACHINE.STATION_TYPE;
            model.MachineTYPE = MACHINE.MACHINE_TYPE;
            model.Line = MACHINE.LINE;
            model.Stage = MACHINE.STAGE;
            model.Batchno = model.Batchno;
            model.MachineCode = model.MachineCode;
            int res = Context.Insertable(model).ExecuteCommand();

            //存在就不添加PWoCutting表
            PWoCutting PWoCuttingY = Context.Queryable<PWoCutting>()
                .Where(i => i.WorkOrder == model.WorkOrder)
                .Where(i => i.Enabled == "Y")
                .First();
            if (PWoCuttingY != null) return res;

            List<SugarParameter> sugarParameters = new List<SugarParameter> { 
                   new SugarParameter("@WORKORDER",WoBase.WorkOrder),
                   new SugarParameter("@RESULT",null,true),
                   new SugarParameter("@RES",null,true),
            };

            PWoCutting pWoCutting = new PWoCutting();

            try
            {
                Context.Ado.UseStoredProcedure().ExecuteCommand("SAJET.P_PROPORTION", sugarParameters);
                var RESULT = sugarParameters[1].Value;
                var RES = sugarParameters[2].Value;
                if ("OK".Equals(RES.ToString()))
                {
                    pWoCutting.BomRatio = RESULT.ObjToDecimal();
                }
                else {
                    pWoCutting.BomRatio = 1.1M;
                }
                pWoCutting.TargetQty = WoBase.TargetQty;
                pWoCutting.MaxQty = Math.Ceiling(pWoCutting.TargetQty * pWoCutting.BomRatio);
                pWoCutting.MaxShelfQty = Math.Ceiling(pWoCutting.MaxQty / 60);
            }
            catch (Exception e)
            {
                //错误的话记录日志
                Console.WriteLine(e.Message);
            }
            pWoCutting.Ipn = WoBase.Ipn;
            pWoCutting.WorkOrder = WoBase.WorkOrder;
            pWoCutting.Model = WoBase.Model;
            pWoCutting.OutputQty = 0;
            pWoCutting.Closed = "N";
            pWoCutting.Enabled = "Y";
            pWoCutting.CreateEmpno = model.CreateEmpno;
            pWoCutting.CreateTime = model.CreateTime;
            return Context.Insertable(pWoCutting).ExecuteCommand();
        }

        /**
         * 查询哪些物料可以进行上料
         */
        List<string> BomList(PFeeding model)
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
        /// 修改上下料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdatePFeeding(PFeeding model)
        {
            return Update(model, true);
        }

        /**
         * 投入产出
         * */
        public ResponstationInout stationInout(stationInout parm)
        {
            parm.MachineCode = Tools.FormatStr(parm.MachineCode);
            parm.ToolingNo = Tools.FormatStr(parm.ToolingNo);
           
            if (parm.MachineCode.IsNullOrEmpty())
            {
                return new ResponstationInout("NG", "请扫描设备!");
            }
            if( parm.ToolingNo.IsNullOrEmpty())
            {
                return new ResponstationInout("NG", "请扫描载具!");
            }


            var parameters = new List<SugarParameter>
             {
             new SugarParameter("@t_type", parm.Type=="投入"?parm.Type="input":parm.Type="output"),
             new SugarParameter("@t_tooling_no", parm.ToolingNo),
             new SugarParameter("@t_machine_code", parm.MachineCode),
             new SugarParameter("@t_emp", parm.CreateEmpno),
             new SugarParameter("@t_outputqty", parm.OutPutQty),
             new SugarParameter("@t_site",parm.Site), // 设置为输出参数
             new SugarParameter("@tres", null, true), // 设置为输出参数
             new SugarParameter("@tmsg", null, true) // 设置为输出参数
            };


            Context.Ado.UseStoredProcedure().ExecuteCommand("SAJET.SP_STATION_INOUT", parameters);

            // 获取输出参数值
            var tres = parameters[6].Value;
            var tmsg = parameters[7].Value;

            return new ResponstationInout(
            tres != null ? tres.ToString() : "",
            tmsg != null ? tmsg.ToString() : "");

        }


            
        public ResponstationInout stationInoutAgv(stationInout parm)
        {
            parm.MachineCode = Tools.FormatStr(parm.MachineCode);
            parm.ToolingNo = Tools.FormatStr(parm.ToolingNo);

            if (parm.MachineCode.IsNullOrEmpty())
            {
                return new ResponstationInout("NG", "请扫描设备!");
            }
            if (parm.ToolingNo.IsNullOrEmpty())
            {
                return new ResponstationInout("NG", "请扫描载具!");
            }


            var parameters = new List<SugarParameter>
             {
             new SugarParameter("@t_type",parm.Type),
             new SugarParameter("@t_tooling_no",parm.ToolingNo),
             new SugarParameter("@t_machine_code", parm.MachineCode),
             new SugarParameter("@t_emp", parm.CreateEmpno),
             new SugarParameter("@T_OUTPUTQTY", parm.OutPutQty),
             new SugarParameter("@t_site",parm.Site), // 设置为输出参数
             new SugarParameter("@tres", null, true), // 设置为输出参数
             new SugarParameter("@tmsg", null, true) // 设置为输出参数
            };


            Context.Ado.UseStoredProcedure().ExecuteCommand("SAJET.SP_STATION_INOUT", parameters);

            // 获取输出参数值
            var tres = parameters[5].Value;
            var tmsg = parameters[6].Value;

            return new ResponstationInout(
            tres != null ? tres.ToString() : "",
            tmsg != null ? tmsg.ToString() : "");

        }

        //暂时还没用到
        public int AddPFeedingPd(PFeeding model)
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

            //卡控这个设备允许哪些物料上料
            var okIpn = @"SELECT DISTINCT M.IPN,M.ID,M.ITEM_IPN,STATION_TYPE
            FROM IMES.M_BILL_MATERIAL M
            LEFT JOIN  IMES.P_WO_BASE B ON(M.IPN = B.IPN)
            WHERE M.STATION_TYPE = "+ MACHINE.STATION_TYPE + "";
            // var list = Context.Ado.Queryable<dynamic>(okIpn).ToList();
            List<Object> option = Context.Ado.SqlQuery<Object>(okIpn).ToList();
            if (option == null) {
                throw new CustomException("该设备没有配置物料");
            }

            //通过制程和工单号查询允许上哪些物料
            List<string> woBom = BomList(model);

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
            }

            return 1;
        }

        public PWoCutting QueryPFeedingorder()
        {

            string sql = @" SELECT* FROM(SELECT ROWNUM AS RN, ID, WORK_ORDER, IPN, MODEL, TARGET_QTY, MAX_QTY, OUTPUT_QTY FROM IMES.P_WO_CUTTING
WHERE CLOSED= 'N' AND ENABLED = 'Y' ORDER BY ID) WHERE RN = 1";



            PWoCutting sugarQueryable = Context.SqlQueryable<PWoCutting>(sql.ToString()).First();

            return sugarQueryable;
        }

        public int GetPFeedingupdateorder(long id)
        {
            PWoCutting pWoCutting = new PWoCutting();
            pWoCutting.Id = id;
            pWoCutting.Closed = "Y";
            return Context.Updateable(pWoCutting).UpdateColumns(it => new { it.Closed}).Where(it=>it.Id==id).ExecuteCommand();
          
        }

    }
}