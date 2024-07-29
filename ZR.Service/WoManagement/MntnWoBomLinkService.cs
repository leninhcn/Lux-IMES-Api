using Aliyun.OSS;
using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Mapster;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Common;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.System;
using ZR.Repository;
using ZR.Service.IService;

namespace ZR.Service.WoManagement
{
    [AppService(ServiceType =typeof(IMntnWoBomLinkService), ServiceLifetime =LifeTime.Transient)]
    public  class MntnWoBomLinkService : BaseService<WoBom>,IMntnWoBomLinkService
    {
        public async Task<List<dynamic>> GetWoBase(PWoBomQueryDto param)
        {
            // var wobase=await Context.Queryable<WoBase>().Where(a=>a.Site==site&&a.WorkOrder==wo).ToListAsync();
            string sql = @"SELECT A.*, IMES.SJ_WOSTATUS_RESULT (A.WO_STATUS) WOSTATUS FROM  IMES.P_WO_BASE A where A.WORK_ORDER=@wo and A.SITE= @site";
           return await Context.Ado.SqlQueryAsync<dynamic>(sql,new List<SugarParameter> { new SugarParameter ("@wo", param.WorkOrder),new SugarParameter("@site", param.site) });
        }

        public List<PWoBomDto> GetWoBom(PWoBomQueryDto param)
        {
            return Queryable().Where(it => it.Site == param.site && it.WorkOrder == param.WorkOrder).ToDto<WoBom, PWoBomDto>();
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddPWoBom(WoBom model)
        {
            return Context.Insertable(model).IgnoreColumns(ignoreNullColumn:true).ExecuteCommand();
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WoBom GetInfo(WoBom model)
        {
            var response = Queryable()
                .Where(x => x.WorkOrder == model.WorkOrder&&x.Ipn==model.Ipn&&x.ItemIpn==model.ItemIpn&&x.StationType==model.StationType && x.Site == model.Site)
                .First();
            return response;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdatePWoBom(WoBom model)
        {
            //备份
            Context.Ado.ExecuteCommand("insert into IMES.P_WO_BOM_HT select * from IMES.P_WO_BOM where ID = @ID", new List<SugarParameter>{ new SugarParameter("@ID", model.Id ) });
            var response = Context.Updateable(model).UpdateColumns(it => new { it.ItemGroup, it.ItemCount, it.StationType,it.UpdateEmpno,it.UpdateTime }).WhereColumns(a => new { a.Id}).ExecuteCommand();
            return response;
        }
        /// <summary>
        /// 工单维护新增修改工单BOM
        /// </summary>
        /// <param name="param"></param>
        /// <param name="swobom"></param>
        /// <returns></returns>
        public string UpdatePWoBomByWO(List<WoBom> param,WoBom swobom)
        {
            foreach (var item in param)
            {
                if(item.ItemIpn.IsNullOrEmpty())
                {
                    return "子料号(ItemIpn)不能为空 " + item.ItemIpn;
                }
                if (item.Id.IsNullOrEmpty())
                {
                    if (Context.Queryable<WoBom>().Where(it => it.WorkOrder == item.WorkOrder && it.ItemIpn == item.ItemIpn && it.Site == item.Site).Count() > 0)
                    {
                        return "子料号已存在 " + item.ItemIpn;
                    }
                }
            }
            var sitemgroup = "0";
            if(param.Count !=1)
              {// 1 获取id不为空的数据，如果itemgroup为0 则重新获取最大值，
               var frist= param.First(it=>it.Id !=null && it.Id != string.Empty);
                WoBom paramw =new WoBom();
                paramw.WorkOrder=frist.WorkOrder;
                paramw.ItemIpn= frist.ItemIpn;
              
                    var sqlfrist = Context.Queryable<WoBom>().Where(x => x.WorkOrder == paramw.WorkOrder && x.ItemIpn == paramw.ItemIpn).ToList();
                    var response = sqlfrist;
                if (sqlfrist[0].ItemGroup == "0")
                {
                    sitemgroup = Context.Ado.SqlQuery<string>(@"Select NVL(MAX(ITEM_GROUP),0)+1 ITEM_GROUP from IMES.P_WO_BOM where work_order=@wo", new List<SugarParameter> { new SugarParameter("@wo", param[0].WorkOrder) })[0];
                }
                else
                {
                    sitemgroup = sqlfrist[0].ItemGroup;
                }
            }
           foreach(var item in param)
            {   item.Site=swobom.Site;
                item.UpdateEmpno = swobom.UpdateEmpno;
                item.UpdateTime = swobom.UpdateTime;
                item.ItemGroup = sitemgroup;
                if(item.Id.IsNullOrEmpty())
                {
                   Context.Insertable(item).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                }
                else
                {
                    //备份
                    Context.Ado.ExecuteCommand("insert into IMES.P_WO_BOM_HT select * from IMES.P_WO_BOM where ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", item.Id) });
                    var response = Context.Updateable(item).UpdateColumns(it => new { it.ItemGroup, it.UpdateEmpno, it.UpdateTime }).WhereColumns(a => new { a.Id }).ExecuteCommand();
                }
            }
           
            return "OK";
        }
        /// <summary>
        /// 删除_工单bom使用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int DeletePWoBomByWo(WoBom model)
        {
            if(model.ItemGroup=="0")
            {
                //备份
                Context.Ado.ExecuteCommand("insert into IMES.P_WO_BOM_HT select * from IMES.P_WO_BOM where ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", model.Id) });
                var response = Context.Deleteable(model).Where(it=>it.Id == model.Id).ExecuteCommand();
                return response;
            }
            else
            {
                var result =Context.Queryable<WoBom>().Where(it=>it.WorkOrder==model.WorkOrder && it.ItemGroup==model.ItemGroup).Count();
                if(result > 2)
                {
                    //大于2笔自接删除
                    Context.Ado.ExecuteCommand("insert into IMES.P_WO_BOM_HT select * from IMES.P_WO_BOM where ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", model.Id) });
                    var response = Context.Deleteable(model).Where(it => it.Id == model.Id).ExecuteCommand();
                    return response;
                }
                else
                {
                    //等于2笔，剩余的group改为0
                    Context.Ado.ExecuteCommand("insert into IMES.P_WO_BOM_HT select * from IMES.P_WO_BOM where ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", model.Id) });
                    var response = Context.Deleteable(model).Where(it => it.Id == model.Id).ExecuteCommand();
                    Context.Ado.ExecuteCommand("insert into IMES.P_WO_BOM_HT select * from IMES.P_WO_BOM where WORK_ORDER = :pwo AND ITEM_GROUP = :pgroup AND SITE = :site", new List<SugarParameter> { new SugarParameter(":pwo", model.WorkOrder), new SugarParameter(":pgroup", model.ItemGroup), new SugarParameter(":site", model.Site) });
                    Context.Ado.ExecuteCommand("update IMES.P_WO_BOM set ITEM_GROUP = 0 ,UPDATE_TIME = sysdate,UPDATE_EMPNO = :pemp WHERE WORK_ORDER = :pwo AND ITEM_GROUP = :pgroup AND SITE = :site",new List<SugarParameter> { new SugarParameter(":pemp",model.UpdateEmpno),new SugarParameter(":pwo",model.WorkOrder),new SugarParameter(":pgroup", model.ItemGroup),new SugarParameter(":site",model.Site)});
                    return response;
                }
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int DeletePWoBom(WoBom model)
        {
            //备份
            Context.Ado.ExecuteCommand("insert into IMES.P_WO_BOM_HT select * from IMES.P_WO_BOM where ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", model.Id) });
            var response = Context.Deleteable(model).ExecuteCommand();
            return response;
        }
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="woboms"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public (string, object, object) ImportWoBom(List<WoBom> woboms,string site)
        {
            var tempwo = ""
;            //只允许单笔工单维护
            
            if (woboms.Select(x=>x.WorkOrder).Distinct().ToList().Count !=1)
            {
                return ("工单有重复，请单笔工单维护", "", "");
            }
            string sqlchecwoipn = @"SELECT work_order FROM  IMES.P_WO_BASE A where A.WORK_ORDER=@wo and a.ipn=@ipn and A.SITE= @site";
            string sqlchecitemipn = @"SELECT IPN FROM  IMES.M_PART A where A.ipn=@ipn and A.SITE= @site";
            string ssqlht = @"INSERT INTO IMES.P_WO_BOM_ht SELECT * FROM IMES.P_WO_BOM WHERE work_order = @wo";
            string sqlclear = @"DELETE  FROM IMES.P_WO_BOM  WHERE WORK_ORDER= @wo";
            string sqlcheckstationtype = @"SELECT STATION_TYPE FROM  IMES.M_STATION_TYPE A where A.STATION_TYPE=@stationtype and A.SITE= @site";
            foreach (var wobom in woboms)
            {
                tempwo= wobom.WorkOrder;
                PWoBomQueryDto woBomQueryDto = new PWoBomQueryDto();
                woBomQueryDto.WorkOrder = wobom.WorkOrder;
                woBomQueryDto.site=site;
                var res = GetWoBase(woBomQueryDto);
                if(res is null)
                {
                    return ("工单："+wobom.WorkOrder+"不存在", "", "");
                }
                //检查工单和工单料号
               var resa= Context.Ado.SqlQuery<string>(sqlchecwoipn,
                new List<SugarParameter>
            {
                    new SugarParameter("@wo",wobom.WorkOrder),
                    new SugarParameter("@ipn",wobom.Ipn),
                    new SugarParameter("@site",site)
            });
                if (resa.Count() ==0)
                {
                    return ("工单：" + wobom.WorkOrder + "料号："+wobom.Ipn+"不匹配", "", "");
                }
                //检查子料号
                var resb = Context.Ado.SqlQuery<string>(sqlchecitemipn,
                new List<SugarParameter>
            {
                    new SugarParameter("@ipn",wobom.Ipn),
                    new SugarParameter("@site",site)
            });
                if (resb.Count() == 0)
                {
                    return ("子料号：" + wobom.Ipn + "不存在", "", "");
                }
                //检查数量是否为数字
                if (wobom.ItemCount.IsNullOrZero())
                {
                    return ("ItemCount必须为数字", "", "");
                }
                //检查站点类型
                if(!wobom.StationType.IsNullOrEmpty())
                {
                    var resd = Context.Ado.SqlQuery<string>(sqlcheckstationtype,
                new List<SugarParameter>
            {
                    new SugarParameter("@stationtype",wobom.StationType),
                    new SugarParameter("@site",site)
            });
                    if (resd.Count() == 0)
                    {
                        return ("站点制程：" + wobom.StationType + "不存在", "", "");
                    }
                }

            }
            woboms.ForEach(x =>
            {
                x.Site = site;
                x.Id = Guid.NewGuid().ToString("N").ToUpper();
            });
            Context.Ado.ExecuteCommand(ssqlht, new List<SugarParameter> { new SugarParameter("@wo", tempwo) });
            Context.Ado.ExecuteCommand(sqlclear, new List<SugarParameter> { new SugarParameter("@wo", tempwo) });
            //Context.Insertable(woboms).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            //var x = Context.Storageable(woboms)
            //   .SplitInsert(it => !it.Any())
            //   .ToStorage();
            //var result = x.AsInsertable.IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();//插入可插入部分;
            woboms.ForEach(x =>
            {
                Context.Insertable(x).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            });
            return ("导入OK", "", "");
        }
    }
}
