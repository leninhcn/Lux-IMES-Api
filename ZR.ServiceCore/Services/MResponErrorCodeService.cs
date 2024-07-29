using Aliyun.OSS;
using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Infrastructure.Model;
using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using ZR.Common;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Dto.Quality;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Model.System.Vo;
using ZR.Repository;
using ZR.Service.System.IService;
using static ZR.Model.Dto.DataToWMS;

namespace ZR.Service.System
{
    /// <summary>
    /// 入库程式业务处理层
    /// </summary>
    [AppService(ServiceType = typeof(IMResponErrorCodeService), ServiceLifetime = LifeTime.Transient)]
    public class MResponErrorCodeService : BaseService<MResponseErrorcode>, IMResponErrorCodeService
    {
        /// <summary>
        /// 获取多语言报错信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<string> GetResponseMsg( ResponseErrorCodeDto parm)
        {
            var sqlstr = @" SELECT IMES.FN_GETRESPONSMSG(:LANE,:CODE,:PARAM) MSG from dual ";
            var resmsg = await Context.Ado.SqlQuerySingleAsync<string>(sqlstr, new List<SugarParameter> { new SugarParameter(":LANE", parm.Lang), new SugarParameter("@CODE", parm.ResponseErrorCode), new SugarParameter("@PARAM", parm.Param) });
            return resmsg;
        }
    }
}
