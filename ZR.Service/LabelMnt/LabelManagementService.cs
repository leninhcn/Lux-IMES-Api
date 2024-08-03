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
    /// 模版便签维护
    /// </summary>
    [AppService(ServiceType = typeof(ILabelManagementService), ServiceLifetime = LifeTime.Transient)]
    public class LabelManagementService : BaseService<MLabelType>, ILabelManagementService
    {
        /// <summary>
        /// //获取labeltype信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MLabelTypeDto> GetListlabeltype(MLabelTypeQueryDto parm)
        {
            var predicate = Expressionable.Create<MLabelType>();
            predicate = predicate.AndIF (!parm.Model.IsNullOrEmpty(),it => it.Model == parm.Model);
            predicate = predicate.AndIF(!parm.Ipn.IsNullOrEmpty(), it => it.Ipn == parm.Ipn);
            predicate = predicate.And(it => it.Site == parm.site);
            predicate = predicate.And(it => it.Enabled == "Y");
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            // var response1 = GetPages(predicate.ToExpression(), parm, s => new { s.CreateTime });
            // var response = GetPages((predicate.ToExpression());
            //排序
            parm.Sort = "CreateTime";
            parm.SortType= "desc";
            var response = Queryable().Where(predicate.ToExpression()).ToPage<MLabelType, MLabelTypeDto>(parm);
            return response;
        }
     
        /// <summary>
        /// 获取labeltype详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public MLabelType GetInfoLabelType(string ID)
        {
            var response = Queryable().Where(x => x.Id == ID).First();
            return response;
        }

        /// <summary>
        /// 新增labeltype待增加保存模板功能
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string AddLabelType(MLabelType param)
        {        
            try
            {
                if(param.Model is null)
                {
                    return "Model 不能为空";
                }
                if(param.LabelType is null)
                {
                    return "LabelType 不能为空";
                }
                if (param.LabelName is null)
                {
                    return "LabelName 不能为空";
                }
                //检查是否有重复
                var predicate = Expressionable.Create<MLabelType>();
            predicate.And( it => it.Model==param.Model && it.Ipn == param.Ipn && it.LabelType==param.LabelType && it.TypeFlag==param.TypeFlag);
            var response = GetList(predicate.ToExpression());
            if(response.Count>0)
            {
                return $"Label Type Data is Exists: LabelType:{param.LabelType},IPN:{param.Ipn},Type Flag:{param.TypeFlag}";
            }
                param.Id = Guid.NewGuid().ToString("N").ToUpper();
                int i = Context.Insertable(param).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            //备份
           // Context.Ado.ExecuteCommand("insert INTO SAJET.M_DUTY_HT select * FROM SAJET.M_DUTY where ID = @ID", new List<SugarParameter>{ new SugarParameter("@ID", model.Id ) });
            return i==1?"OK":"插入失败";
            //待增加保存模板功能
            }
            catch(Exception ex)
            {
                return "新增失败，请检查 "+ex.ToString();
            }
        }
        /// <summary>
        /// 修改labeltype
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string UpdateLabelType(MLabelType param)
        {   
            param.UpdateTime = DateTime.Now;
            //待补充保存模版
            int result = Context.Updateable(param).UpdateColumns(it=>new {it.Ipn,it.LabelTypeDesc,it.LabelName,it.PrinterName,it.TypeFlag,it.UpdateEmpno,it.UpdateTime}).ExecuteCommand();
            //更新其它相关表
            if(result >0)
            {
                var sql=@"update SAJET.M_STATIONTYPE_LABEL SET LABEL_NAME=@1,LABEL_DESC=@2,LABEL_PARAMS=@3,UPDATE_EMPNO=@4,UPDATE_TIME=SYSDATE WHERE LABEL_TYPE=@5 AND LABEL_NAME=@6 AND IPN=@7";
                Context.Ado.ExecuteCommand(sql, new List<SugarParameter> { new SugarParameter("@1", param.LabelName), new SugarParameter("@2", param.LabelTypeDesc), new SugarParameter("@3", param.TypeFlag), new SugarParameter("@4", param.UpdateEmpno), new SugarParameter("@5", param.LabelType), new SugarParameter("@6", param.LabelName), new SugarParameter("@7", param.Ipn) });
            }
            return "OK";
        }
        /// <summary>
        /// 删除labeltype
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string DeleteLabelType(MLabelType param)
        {
            int result = Context.Deleteable(param).ExecuteCommand();
            return "OK";
        }

        /// <summary>
        /// //获取printdata信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MPrintDataDto> GetListprintdata(MPrintDataQueryDto parm)
        {
            var predicate = Expressionable.Create<MPrintData>();
            predicate = predicate.AndIF(!parm.DataType.IsNullOrEmpty(), it => it.DataType == parm.DataType);
            predicate = predicate.AndIF(!parm.DataSql.IsNullOrEmpty(), it => it.DataSql.ToUpper().Contains(parm.DataSql.ToUpper()));
            predicate = predicate.And(it => it.Site == parm.site);
            predicate = predicate.And(it => it.Enabled == "Y");
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            // var response1 = GetPages(predicate.ToExpression(), parm, s => new { s.CreateTime });
            // var response = GetPages((predicate.ToExpression());
            //排序
            parm.Sort = "CreateTime";
            parm.SortType = "desc";
            var response = Context.Queryable<MPrintData>().Where(predicate.ToExpression()).ToPage<MPrintData, MPrintDataDto>(parm);
            return response;
        }

        /// <summary>
        /// 获取printdata详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public MPrintData GetInfoprintdata(string ID)
        {
            var response = Context.Queryable<MPrintData>().Where(x => x.Id == ID).First();
            return response;
        }
        /// <summary>
        /// 新增printdata
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string AddPrintData(MPrintData param)
        {
            try
            {
                //检查是否有重复
                var predicate = Expressionable.Create<MPrintData>();
                predicate.And(it => it.DataType == param.DataType && it.DataSql == param.DataSql);
                var response = Context.Queryable<MPrintData>().Where(predicate.ToExpression()).ToList();
                if (response.Count > 0)
                {
                    return $"Label Type Data is Exists: LabelType:{param.DataType},LabelSql:{param.DataSql}";
                }
                param.Id = Guid.NewGuid().ToString("N").ToUpper();
                int i = Context.Insertable(param).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                //备份
                // Context.Ado.ExecuteCommand("insert INTO SAJET.M_DUTY_HT select * FROM SAJET.M_DUTY where ID = @ID", new List<SugarParameter>{ new SugarParameter("@ID", model.Id ) });
                return i == 1 ? "OK" : "插入失败";
                //待增加保存模板功能
            }
            catch (Exception ex)
            {
                return "新增失败，请检查 " + ex.ToString();
            }
        }
        /// <summary>
        /// 修改printdata
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string UpdatePrintData(MPrintData param)
        {
            param.UpdateTime = DateTime.Now;
            //待补充保存模版
            int result = Context.Updateable(param).UpdateColumns(it => new { it.DataType, it.DataSql, it.InputParam, it.InputField, it.UpdateEmpno, it.UpdateTime,it.Enabled }).WhereColumns(it => new {it.Id}).ExecuteCommand();
            return "OK";
        }
        /// <summary>
        /// 删除printdata
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string DeletePrintData(MPrintData param)
        {
            int result = Context.Deleteable(param).ExecuteCommand();
            return "OK";
        }

        /// <summary>
        /// //获取stationtypelabel信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MStationtypeLabelDto> GetListStationtypeLabel(MStationtypeLabelQueryDto parm)
        {
            var predicate = Expressionable.Create<MStationtypeLabel>();
            predicate = predicate.AndIF(!parm.Model.IsNullOrEmpty(), it => it.Model == parm.Model);
            predicate = predicate.AndIF(!parm.Ipn.IsNullOrEmpty(), it => it.Ipn == parm.Ipn).AndIF(!parm.LabelType.IsNullOrEmpty(), it => it.LabelType == parm.LabelType).AndIF(!parm.StationType.IsNullOrEmpty(), it => it.StationType == parm.StationType);
            predicate = predicate.And(it => it.Site == parm.site);
            predicate = predicate.And(it => it.Enabled == "Y");
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            // var response1 = GetPages(predicate.ToExpression(), parm, s => new { s.CreateTime });
            // var response = GetPages((predicate.ToExpression());
            //排序
            parm.Sort = "CreateTime";
            parm.SortType = "desc";
            var response = Context.Queryable<MStationtypeLabel>().Where(predicate.ToExpression()).ToPage<MStationtypeLabel, MStationtypeLabelDto>(parm);
            return response;
        }

        /// <summary>
        /// 获取stationtypelabel详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public MStationtypeLabel GetInfostationtypelabel(string ID)
        {
            var response = Context.Queryable<MStationtypeLabel>().Where(x => x.Id == ID).First();
            return response;
        }
        /// <summary>
        /// 新增stationtypelabel
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string AddStationLabel(MStationtypeLabel param)
        {
            try
            {
                //检查是否有重复
                var predicate = Expressionable.Create<MStationtypeLabel>();
                predicate.And(it => it.Model==param.Model&&it.Ipn==param.Ipn&&it.StationType==param.StationType&&it.LabelType == param.LabelType && it.LabelParams == param.LabelParams);
                var response = Context.Queryable<MStationtypeLabel>().Where(predicate.ToExpression()).ToList();
                if (response.Count > 0)
                {
                    return $"Label Type Data is Exists:  Model:{param.Model} IPN:{param.Ipn} StationType:{param.StationType} LabelType:{param.LabelType} LabelParams:{param.LabelParams}";
                }
                param.Id = Guid.NewGuid().ToString("N").ToUpper();
                int i = Context.Insertable(param).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                //备份
                // Context.Ado.ExecuteCommand("insert INTO SAJET.M_DUTY_HT select * FROM SAJET.M_DUTY where ID = @ID", new List<SugarParameter>{ new SugarParameter("@ID", model.Id ) });
                return i == 1 ? "OK" : "插入失败";
                //待增加保存模板功能
            }
            catch (Exception ex)
            {
                return "新增失败，请检查 " + ex.ToString();
            }
        }
        /// <summary>
        /// 修改stationtypelabel
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string UpdateStationtypeLabel(MStationtypeLabel param)
        {
            param.UpdateTime = DateTime.Now;
            //待补充保存模版
            int result = Context.Updateable(param).UpdateColumns(it => new { it.LabelType, it.LabelName, it.LabelParams, it.LabelDesc,it.PrinterName,it.LabelSrvIp,it.LabelDlUrl, it.UpdateEmpno, it.UpdateTime, it.Enabled }).ExecuteCommand();
            return "OK";
        }
        /// <summary>
        /// 删除stationtypelabel
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string DeleteStationtypeLabel(MStationtypeLabel param)
        {
            int result = Context.Deleteable(param).ExecuteCommand();
            return "OK";
        }
        /// <summary>
        /// //获取stationtypelabelParam信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MStationtypeLabelParamsDto> GetListStationtypeLabelParam(MStationtypeLabelParamsQueryDto parm)
        {
            var predicate = Expressionable.Create<MStationtypeLabelParams>();
            predicate = predicate.AndIF(!parm.Model.IsNullOrEmpty(), it => it.Model == parm.Model);
            predicate = predicate.AndIF(!parm.LabelType.IsNullOrEmpty(), it => it.LabelType == parm.LabelType).AndIF(!parm.FieldName.IsNullOrEmpty(), it => it.FieldName == parm.FieldName).AndIF(!parm.VarType.IsNullOrEmpty(), it => it.VarType == parm.VarType);
            predicate = predicate.And(it => it.Site == parm.site);
            predicate = predicate.And(it => it.Enabled == "Y");
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            // var response1 = GetPages(predicate.ToExpression(), parm, s => new { s.CreateTime });
            // var response = GetPages((predicate.ToExpression());
            //排序
            parm.Sort = "CreateTime";
            parm.SortType = "desc";
            var response = Context.Queryable<MStationtypeLabelParams>().Where(predicate.ToExpression()).ToPage<MStationtypeLabelParams, MStationtypeLabelParamsDto>(parm);
            return response;
        }
        /// <summary>
        /// 获取stationtypelabelParam详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public MStationtypeLabelParams GetInfostationtypelabelParam(string ID)
        {
            var response = Context.Queryable<MStationtypeLabelParams>().Where(x => x.Id == ID).First();
            return response;
        }
        /// <summary>
        /// 新增stationtypelabelParam
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string AddStationLabelParams(MStationtypeLabelParams param)
        {
            try
            {
                //检查是否有重复
                var predicate = Expressionable.Create<MStationtypeLabelParams>();
                predicate.And(it => it.Model==param.Model && it.LabelType == param.LabelType && it.VarName == param.VarName);
                var response = Context.Queryable<MStationtypeLabelParams>().Where(predicate.ToExpression()).ToList();
                if (response.Count > 0)
                {
                    return $"Label Type Data is Exists: LabelType:{param.LabelType},LabelVarName:{param.VarName}";
                }
                param.Id=Guid.NewGuid().ToString("N").ToUpper();
                int i = Context.Insertable(param).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                //备份
                // Context.Ado.ExecuteCommand("insert INTO SAJET.M_DUTY_HT select * FROM SAJET.M_DUTY where ID = @ID", new List<SugarParameter>{ new SugarParameter("@ID", model.Id ) });
                return i == 1 ? "OK" : "插入失败";
                //待增加保存模板功能
            }
            catch (Exception ex)
            {
                return "新增失败，请检查 " + ex.ToString();
            }
        }
        /// <summary>
        /// 修改stationtypelabelparam
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string UpdateStationtypeLabelParam(MStationtypeLabelParams param)
        {
            param.UpdateTime = DateTime.Now;
            //待补充保存模版
            int result = Context.Updateable(param).UpdateColumns(it => new { it.VarName, it.VarType, it.FieldName, it.Description, it.UpdateEmpno, it.UpdateTime,it.Enabled }).ExecuteCommand();
            return "OK";
        }
        /// <summary>
        /// 删除stationtypelabelParam
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string DeleteStationtypeLabelParam(MStationtypeLabelParams param)
        {
            int result = Context.Deleteable(param).ExecuteCommand();
            return "OK";
        }
        /// <summary>
        /// 导入stationtypelabel数据
        /// </summary>
        /// <param name="labels"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public (string, object, object) ImportStationtypeLabel(List<MStationtypeLabel> labels, string site)
        {
            foreach (var label in labels)
            {
                //检查值不能为空
                if(label.Model.IsNullOrEmpty()&&label.LabelName.IsNullOrEmpty()&&label.LabelType.IsNullOrEmpty()&&label.Ipn.IsNullOrEmpty())
                {
                    return ("model，labelname,labeltype,ipn不能为空", "", "");
                }

                //检查是否有重复
                var predicate = Expressionable.Create<MStationtypeLabel>();
                predicate.And(it => it.Model == label.Model && it.Ipn == label.Ipn && it.StationType == label.StationType && it.LabelType == label.LabelType && it.LabelParams == label.LabelParams);
                var response = Context.Queryable<MStationtypeLabel>().Where(predicate.ToExpression()).ToList();
                if (response.Count > 0)
                {
                    return ($"Label Type Data is Exists:  Model:{label.Model} IPN:{label.Ipn} StationType:{label.StationType} LabelType:{label.LabelType} LabelParams:{label.LabelParams}","","");
                }

            }
            labels.ForEach(x =>
            {
                x.Site = site;
                x.Id = Guid.NewGuid().ToString("N").ToUpper();
            });
            //Context.Insertable(woboms).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            //var x = Context.Storageable(woboms)
            //   .SplitInsert(it => !it.Any())
            //   .ToStorage();
            //var result = x.AsInsertable.IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();//插入可插入部分;
            labels.ForEach(x =>
            {
                Context.Insertable(x).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            });
            return ("导入OK", "", "");
        }
        /// <summary>
        /// 新增打印模版列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public string AddLabelFile(MLabelTemplateFile parm)
        {
            int i = Context.Insertable(parm).IgnoreColumns(ignoreNullColumn:true).ExecuteCommand();
            return i == 1 ? "OK" : "插入失败";
        }
        /// <summary>
        /// 查询打印模版列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public MLabelTemplateFile GetListLabelFile(string parm)
        {
            var predicate = Context.Queryable<MLabelTemplateFile>().Where(it=>it.LabelName==parm).First();
            return predicate;
        }
    }
}
