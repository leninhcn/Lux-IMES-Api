﻿using Aliyun.OSS;
using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Controllers;
using Infrastructure.Model;
using IPTools.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using NLog;
using SqlSugar;
using ZR.Model.Business;
using ZR.Model.System;
using ZR.Service.System;
using ZR.Service.System.IService;
using ZR.ServiceCore.Bascon;

namespace ZR.ServiceCore.Middleware
{
    public class GlobalActionMonitor : ActionFilterAttribute
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ISysOperLogService OperLogService;
        private readonly ICollectErrorService CollectErrorService;
        public GlobalActionMonitor(ISysOperLogService operLogService,ICollectErrorService _collectErrorService )
        {
            OperLogService = operLogService;
            CollectErrorService = _collectErrorService;
        }

        /// <summary>
        /// Action请求前
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string msg = string.Empty;
            var values = context.ModelState.Values;
            foreach (var item in values)
            {
                foreach (var err in item.Errors)
                {
                    if (!string.IsNullOrEmpty(msg))
                    {
                        msg += " | ";
                    }

                    msg += err.ErrorMessage;
                }
            }
            if (!string.IsNullOrEmpty(msg))
            {
                logger.Info($"请求参数错误,{msg}");
                ApiResult response = new((int)ResultCode.PARAM_ERROR, msg);
                
                context.Result = new JsonResult(response);
            }
            return base.OnActionExecutionAsync(context, next);
        }

        /// <summary>
        /// OnActionExecuted是在Action中的代码执行之后运行的方法。
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return;

            //获得注解信息
            LogAttribute logAttribute = GetLogAttribute(controllerActionDescriptor);
            if (logAttribute == null) return;

            try
            {
                //不记录报错信息和全部的请求信息，则直接返回
                if(CollectErrorService.Status()!="1" )
                {
                    return;
                }
                if(!logAttribute.IsSaveResponseDataError && !logAttribute.IsSaveResponseDataAll)
                {
                    return;
                }
                string method = context.HttpContext.Request.Method.ToUpper();
                // 获取当前的用户
                string userName = context.HttpContext.GetName() ?? context.HttpContext.Request.Headers["userName"];
                string jsonResult = string.Empty;
                if (context.Result is ContentResult result && result.ContentType == "application/json")
                {
                    jsonResult = result.Content.Replace("\r\n", "").Trim();
                }
                if (context.Result is JsonResult result2)
                {
                    jsonResult = result2.Value?.ToString();
                }
                //获取当前执行方法的类名
                //string className =  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                //获取当前成员的名称
                //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string controller = context.RouteData.Values["Controller"].ToString();
                string action = context.RouteData.Values["Action"].ToString();

                string ip = HttpContextExtension.GetClientUserIp(context.HttpContext);
                var ip_info = IpTool.Search(ip);

                SysOperLog sysOperLog = new()
                {
                    Status = 0,
                    OperName = userName,
                    OperIp = ip,
                    OperUrl = HttpContextExtension.GetRequestUrl(context.HttpContext),
                    RequestMethod = method,
                    JsonResult = jsonResult,
                    OperLocation = HttpContextExtension.GetIpInfo(ip),
                    Method = controller + "." + action + "()",
                    //Elapsed = _stopwatch.ElapsedMilliseconds,
                    OperTime = DateTime.Now,
                    OperParam = HttpContextExtension.GetRequestValue(context.HttpContext, method)
                };

                if (logAttribute != null)
                {
                    sysOperLog.Title = logAttribute?.Title;
                    sysOperLog.BusinessType = (int)logAttribute.BusinessType;
                    sysOperLog.OperParam = logAttribute.IsSaveRequestData ? sysOperLog.OperParam : "";
                    sysOperLog.JsonResult = logAttribute.IsSaveResponseData ? sysOperLog.JsonResult : "";
                }

                LogEventInfo ei = new(NLog.LogLevel.Info, "GlobalActionMonitor", "");

                ei.Properties["jsonResult"] = !HttpMethods.IsGet(method) ? jsonResult : "";
                ei.Properties["requestParam"] = sysOperLog.OperParam;
                ei.Properties["user"] = userName;
                logger.Log(ei);
                JObject dclass = (JObject)JsonConvert.DeserializeObject(jsonResult);
                var code = dclass["code"].ToString();
                var msg=dclass["msg"].ToString();
                
                //判断是否需要记录异常信息
                if (code != "200" && logAttribute.IsSaveResponseDataError)
                {
                    //获取errorcode
                    var er = (BaseController)context.Controller;
                    PCollecterrorLog pCollecterror = new PCollecterrorLog
                    {
                        Site = context.HttpContext.GetSite(),
                        ProgramMent = context.HttpContext.GetReferer(),
                        EmpNo = userName,
                        ClentIp = ip,
                        ClientType = "Backend",
                        Errcode = controller + "." + action + "()" + er.errorcode,
                        RequestParam = sysOperLog.OperParam,
                        ResponseResults = msg,
                        ResErrcode = ""
                    };
                    if (msg.StartsWith("SAJETERR"))
                    {
                        pCollecterror.ResErrcode = msg.Split(':')[0];
                    }
                    CollectErrorService.Add(pCollecterror);
                    var errorcode = er.errorcode;
                }
                if (logAttribute.IsSaveResponseDataAll)
                {
                    OperLogService.InsertOperlog(sysOperLog);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"记录操作日志出错了#{ex.Message}");
            }
        }

        private LogAttribute GetLogAttribute(ControllerActionDescriptor controllerActionDescriptor)
        {
            var attribute = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                .FirstOrDefault(a => a.GetType().Equals(typeof(LogAttribute)));

            return attribute as LogAttribute;
        }
    }
}
