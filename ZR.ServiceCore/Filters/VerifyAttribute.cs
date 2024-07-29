﻿using Infrastructure;
using Infrastructure.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ZR.Common;
using ZR.Service.System;

//本命名空间暂时先不改，改动比较大2023年9月2日
namespace ZR.Admin.WebApi.Filters
{
    /// <summary>
    /// 授权校验访问
    /// 如果跳过授权登录在Action 或controller加上 AllowAnonymousAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class VerifyAttribute : Attribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// 只判断token是否正确，不判断权限
        /// 如果需要判断权限的在Action上加上ApiActionPermission属性标识权限类别，ActionPermissionFilter作权限处理
        /// </summary>
        /// <param name="context"></param>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var noNeedCheck = false;
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                noNeedCheck = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                  .Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)));
            }

            if (noNeedCheck) return;

            string ip = HttpContextExtension.GetClientUserIp(context.HttpContext);
            string url = context.HttpContext.Request.Path;
            var isAuthed = context.HttpContext.User.Identity.IsAuthenticated;
            string osType = context.HttpContext.Request.Headers["os"];
            //使用jwt token校验2020-11-21
            TokenModel loginUser = await JwtUtil.GetLoginUser(context.HttpContext);
            if (loginUser != null)
            {
                var nowTime = DateTime.UtcNow;
                var jwtTs = loginUser.JwtExpire - nowTime;
                var sessionTs = loginUser.ExpireTime - nowTime;

                //Console.WriteLine($"jwt到期剩余：{ts.TotalMinutes}分,{ts.TotalSeconds}秒");

                if (jwtTs.TotalMinutes < 5)
                {
                    var newToken = JwtUtil.GenerateJwtToken(JwtUtil.AddClaims(loginUser));

                    //移动端不加下面这个获取不到自定义Header
                    if (osType != null)
                    {
                        context.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Refresh-Token");
                    }
                    context.HttpContext.Response.Headers.Add("X-Refresh-Token", newToken);
                }

                if(sessionTs.TotalMinutes < 5)
                {
                    var CK = loginUser.UserId.ToString();
                    await CacheService.SetUserToken(CK, loginUser);
                }
            }
            if (loginUser == null || !isAuthed)
            {
                string msg = $"请求访问[{url}]失败，无法访问系统资源";
                //logger.Info(msg);
                context.Result = new JsonResult(ApiResult.Error(ResultCode.DENY, msg));
            }
        }
    }
}
