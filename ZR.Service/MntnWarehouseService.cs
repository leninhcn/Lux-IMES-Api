using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Infrastructure.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using ZR.Common;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Model.System.Vo;
using ZR.Repository;
using ZR.Service.IService;
using ZR.Service.System.IService;

namespace ZR.Service
{
    /// <summary>
    /// 仓储信息业务处理层
    /// </summary>
    [AppService(ServiceType = typeof(IMntnWarehouseService), ServiceLifetime = LifeTime.Transient)]
    public class MntnWarehouseService : BaseService<MWarehouse>, IMntnWarehouseService
    {
        private readonly IMResponErrorCodeService _responErrorCodeService;
        public MntnWarehouseService(IMResponErrorCodeService responErrorCodeService)
        {
            _responErrorCodeService = responErrorCodeService;
        }

        /// <summary>
        /// //获取所有仓储信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MWarehouseDto> GetListWareHouse(MWarehouseQueryDto parm)
        {
            if(parm.Enabled=="ALL")
            { 
                parm.Enabled = null;
            }
            var predicate = Expressionable.Create<MWarehouse>();
            predicate = predicate.AndIF (!parm.Enabled.IsNullOrEmpty(),it => it.Enabled == parm.Enabled);
            predicate = predicate.AndIF(!parm.WarehouseCode.IsNullOrEmpty(), it => it.WarehouseCode==parm.WarehouseCode).AndIF(!parm.WarehouseName.IsNullOrEmpty(),it=>it.WarehouseName==parm.WarehouseName).AndIF(!parm.WarehouseType.IsNullOrEmpty(),it=>it.WarehouseType==parm.WarehouseType);
            predicate = predicate.And(it => it.Site == parm.site);
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            // var response1 = GetPages(predicate.ToExpression(), parm, s => new { s.CreateTime });
            // var response = GetPages((predicate.ToExpression());
            //排序
            parm.Sort = "CreateTime";
            parm.SortType= "desc";
            var response = Queryable().Where(predicate.ToExpression()).ToPage<MWarehouse, MWarehouseDto>(parm);
            return response;
        }

        /// <summary>
        /// 获取仓储详情
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public MWarehouse GetInfoWareHouse(string ID,string site)
        {
            var response = Queryable().Where(x => x.WarehouseCode == ID && x.Site==site).First();
            return response;
        }

        /// <summary>
        /// 新增仓储信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Resultinfo<string> AddWareHouse(MWarehouse model)
        {
            Resultinfo<string> resultinfo = new Resultinfo<string>();
            try
            {
                //检查是否有重复
                var predicate = Expressionable.Create<MWarehouse>();
            predicate.AndIF(!model.WarehouseCode.IsNullOrEmpty(), it => it.WarehouseCode == model.WarehouseCode);
            var response = GetList(predicate.ToExpression());
               if (response.Count() > 0)
              {
                    //return "WarehouseCode 重复，请重新检查";
                    resultinfo.Result = "IMESERR005";
                    resultinfo.ResErrCodeParam = model.WarehouseCode;
                    resultinfo.ErrCode = "AddWareHouse:1";
                    //resultinfo.data = await _responErrorCodeService.GetResponseMsg(new ResponseErrorCodeDto { Lang=model.Lang,ResponseErrorCode=resultinfo.ResErrCode,Site=model.Site});
                    return resultinfo;
                }           
                int i = Context.Insertable(model).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert into IMES.M_WAREHOUSE_HT SELECT * FROM IMES.M_WAREHOUSE WHERE WAREHOUSE_CODE = @code AND SITE=@site", new List<SugarParameter>{ new SugarParameter("@code", model.WarehouseCode ),new SugarParameter("@site",model.Site) });
                if (i == 0)
                {
                    resultinfo.Result = "IMESERR004";
                    resultinfo.ErrCode = "AddWareHouse:2";
                } 
                return resultinfo;
            }
            catch(Exception ex)
            {
                resultinfo.Result = "新增失败，请检查 " + ex.ToString();
                resultinfo.ErrCode = "AddWareHouse:3";
                return resultinfo;
            }
        }
        /// <summary>
        /// 修改仓储信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Resultinfo<string> UpdateWareHouse(MWarehouse model)
        {
            Resultinfo<string> resultinfo = new Resultinfo<string>();
            model.UpdateTime = DateTime.Now;
            int result = Context.Updateable(model).UpdateColumns(it=>new { it.WarehouseName, it.WarehouseType,it.Enabled,it.UpdateTime, it.UpdateEmp }).WhereColumns(it => new {it.WarehouseCode,it.Site}).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert into IMES.M_WAREHOUSE_HT SELECT * FROM IMES.M_WAREHOUSE WHERE WAREHOUSE_CODE = @code AND SITE=@site", new List<SugarParameter> { new SugarParameter("@code", model.WarehouseCode), new SugarParameter("@site", model.Site) });
            if(result==0)
            {
                resultinfo.Result = "IMESERR004";
                resultinfo.ErrCode = "UpdateWareHouse:1";
            }
            return resultinfo;
        }
        /// <summary>
        /// 获取储位信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public Resultinfo< PagedInfo<MLocationDto>> GetListLocation(MLocationQueryDto parm)
        {
            if (parm.Enabled == "ALL")
            {
                parm.Enabled = null;
            }
            var predicate = Expressionable.Create<MLocation>();
            predicate = predicate.AndIF(!parm.Enabled.IsNullOrEmpty(), it => it.Enabled == parm.Enabled);
            predicate = predicate.AndIF(!parm.WarehouseCode.IsNullOrEmpty(), it => it.WarehouseCode == parm.WarehouseCode).AndIF(!parm.LocationCode.IsNullOrEmpty(), it => it.LocationCode == parm.LocationCode).AndIF(!parm.LocationName.IsNullOrEmpty(), it => it.LocationName == parm.LocationName).AndIF(!parm.CurrentStatus.IsNullOrEmpty(),it=>it.CurrentStatus==parm.CurrentStatus);
            predicate = predicate.And(it => it.Site == parm.site);
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            // var response1 = GetPages(predicate.ToExpression(), parm, s => new { s.CreateTime });
            // var response = GetPages((predicate.ToExpression());
            //排序
            parm.Sort = "CreateTime";
            parm.SortType = "desc";
            var response = Context.Queryable<MLocation>().Where(predicate.ToExpression()).ToPage<MLocation, MLocationDto>(parm);
            return new Resultinfo<PagedInfo<MLocationDto>> { Data=response};
        }

        /// <summary>
        /// 获取储位详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public MLocation GetInfoLocation(MLocationQueryDto param)
        {
            var response = Context.Queryable<MLocation>().Where(x => x.WarehouseCode == param.WarehouseCode && x.Site == param.site && x.LocationCode==param.LocationCode).First();
            return response;
        }

        /// <summary>
        /// 新增储位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Resultinfo<string> AddLocation(MLocation model)
        {
            Resultinfo<string> resultinfo = new();
            try
            {
                //检查是否有重复
                var response = Context.Queryable<MLocation>().Where(it=>it.WarehouseCode==model.WarehouseCode && it.LocationCode==model.LocationCode && it.Site==model.Site).ToList();
                if (response.Count > 0)
                {
                    //return "LocationCode 重复，请重新检查";
                    resultinfo.Result = "IMESERR006";
                    resultinfo.ResErrCodeParam=model.LocationCode;
                    resultinfo.ErrCode = "AddLocation:1";
                    return resultinfo;

                }
                int i = Context.Insertable(model).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                //备份
                Context.Ado.ExecuteCommand("insert into IMES.M_LOCATION_HT SELECT * FROM IMES.M_LOCATION WHERE WAREHOUSE_CODE = @code and LOCATION_CODE =@lcode AND SITE=@site", new List<SugarParameter> { new SugarParameter("@code", model.WarehouseCode),new SugarParameter("@lcode",model.LocationCode), new SugarParameter("@site", model.Site) });
                if (i == 0)
                {
                    //return i == 1 ? "OK" : "插入失败";
                    resultinfo.Result = "IMESERR004";
                    resultinfo.ErrCode = "AddLocation:2";
                }
                return resultinfo;
            }
            catch (Exception ex)
            {
                //return "新增失败，请检查 " + ex.ToString();
                resultinfo.Result = "新增失败，请检查 " + ex.ToString();
                resultinfo.ErrCode = "AddLocation:3";
                return resultinfo;
            }
        }
        /// <summary>
        /// 修改储位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Resultinfo<string> UpdateLocal(MLocation model)
        {
            Resultinfo<string> resultinfo = new();
            model.UpdateTime = DateTime.Now;
            int result = Context.Updateable(model).UpdateColumns(it => new { it.LocationCode, it.LocationName,it.LocationType, it.Enabled,it.UpdateTime, it.UpdateEmp }).WhereColumns(it => new { it.WarehouseCode,it.LocationCode, it.Site }).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert into IMES.M_LOCATION_HT SELECT * FROM IMES.M_LOCATION WHERE WAREHOUSE_CODE = @code AND LOCATION_CODE =@lcode AND SITE=@site", new List<SugarParameter> { new SugarParameter("@code", model.WarehouseCode), new SugarParameter("@lcode", model.LocationCode), new SugarParameter("@site", model.Site) });
            //return "OK";
            if (result == 0)
            {
                resultinfo.Result = "IMESERR004";
                resultinfo.ErrCode = "UpdateLocal:1";
            }
            return resultinfo;
        }
    }
}
