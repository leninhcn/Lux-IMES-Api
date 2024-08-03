using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
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
using ZR.Service.IService;
using ZR.Service.System.IService;

namespace ZR.Service
{
    /// <summary>
    /// 部门管理
    /// </summary>
    [AppService(ServiceType = typeof(IMntnDefectService), ServiceLifetime = LifeTime.Transient)]
    public class MntnDefectService : BaseService<MDefect>, IMntnDefectService
    {
        /// <summary>
        /// //获取所有不良code信息
        /// </summary>
        /// <param name="defect"></param>
        /// <returns></returns>
        public PagedInfo<MDefect> GetDefect(MntnDefect defect)
        {
            if(defect.Enabled=="ALL")
            {
                defect.Enabled = null;
            }
            var predicate = Expressionable.Create<MDefect>();
            predicate = predicate.AndIF (!defect.Enabled.IsNullOrEmpty(),it => it.Enabled == defect.Enabled);
            predicate = predicate.AndIF(defect.DefectCode != null, it => it.DefectCode.ToLower().StartsWith(defect.DefectCode.ToLower()));
            predicate = predicate.AndIF(defect.DefectDesc != null, it => it.DefectDesc.ToLower().StartsWith(defect.DefectDesc.ToLower()));
            predicate = predicate.And( it => it.Site == defect.Site);
            //PostService.GetPages(predicate.ToExpression(), pagerInfo, s => new { s.PostSort })
            var response = GetPages(predicate.ToExpression(), defect, s => new { s.CreateTime },OrderByType.Desc);
           // var response = GetPages((predicate.ToExpression());
            return response;
        }

        /// <summary>
        /// //获取所有不良code信息
        /// </summary>
        /// <param name="defect"></param>
        /// <returns></returns>
        public MDefect QueryDefect(MntnDefect defect)
        {
            //var predicate = Expressionable.Create<MDefect>();
            //predicate = predicate.AndIF(defect.Enabled.IfNotEmpty(), it => it.Enabled == defect.Enabled);
            //predicate = predicate.AndIF(defect.DefectCode != null, it => it.DefectCode.Contains(defect.DefectCode))
            //    .AndIF(defect.Id.IsNotEmpty(), it => it.Id == defect.Id);
            //var response = GetList(predicate.ToExpression());
            var response =Queryable().Where(x=>x.Id==defect.Id).First();
            return response;
        }

        /// <summary>
        /// GetModel
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public DataTable GetModel()
        {
          DataTable dt= SqlQuery("select MODEL FROM SAJET.M_MODEL ");
            //Context.SqlQueryable("select MODEL FROM SAJET.M_MODEL");
            //暂停，不返回model
            //if (dt.Rows.Count > 0)
            //{
            //    var bomInfos = new List<BomInfo>(dt.Rows.Count);
            //    foreach (DataRow dataRow in dt.Rows)
            //    {
            //        BomInfo bomInfo = new()
            //        {
            //            ItemCount = int.Parse(dataRow["ITEM_COUNT"].ToString()),
            //            ItemGroup = dataRow["ITEM_GROUP"].ToString(),
            //            ItemPartCode = dataRow["SN_FEATURE"].ToString(),
            //            ItemPartDesc = dataRow["MES_SPEC"].ToString(),
            //            ItemPartNo = dataRow["ITEM_IPN"].ToString(),
            //            ItemPartType = dataRow["PART_TYPE"].ToString(),
            //            MainPartNo = dataRow["IPN"].ToString(),
            //            Slot = dataRow["SLOT"].ToString(),
            //            ItemVersion = dataRow["ITEM_VERSION"].ToString(),
            //            ItemMpn = dataRow["MPN"].ToString(),
            //            IpnFinishCount = Convert.ToInt32(dataRow["COUNT"].ToString())
            //        };

            //        bomInfos.Add(bomInfo);
            //    }
            //}
            return dt;
        }

       public string CheckUnique(MDefect Defect)
        {
            return "0";
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="defect"></param>
        /// <returns></returns>
        public string Insert(MDefect defect)
        {        
            try
            {
                //检查是否有重复
                var predicate = Expressionable.Create<MDefect>();
                predicate = predicate.OrIF(defect.DefectCode.IsNotEmpty(), it => it.DefectCode == defect.DefectCode).OrIF(defect.DefectDesc.IsNotEmpty(), it => it.DefectCode == defect.DefectDesc);
                predicate = predicate.And(it=>it.Site==defect.Site);
            var response = GetList(predicate.ToExpression());
            if(response.Count()>0)
            {
                return "DefectCode和DefectCode 重复，请重新检查";
            }
            //获取ID
            //var oData = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);
            var tMax = new SugarParameter("T_MAXID",null,true);

             Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_GET_MAXID",
            new SugarParameter[]
            {
                    new SugarParameter("TFIELD", "ID"),
                    new SugarParameter("TTABLE", "SAJET.M_DEFECT"),
                new SugarParameter("TNUM", 8),
                     tRes,tMax
                });
            if(tRes.Value.ToString()=="OK")
            {
                defect.Id = Convert.ToInt64( tMax.Value.ToString());
            }
            else
            {
                return  "获取ID最大值失败，请检查 "+tRes.Value.ToString();
            }
           int i=  Add(defect);
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.M_DEFECT_HT select * FROM SAJET.M_DEFECT where ID = @ID", new List<SugarParameter>{ new SugarParameter("@ID", defect.Id ) });
            return i==1?"OK":"插入失败";
            }
            catch(Exception ex)
            {
                return "新增失败，请检查 "+ex.ToString();
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="defect"></param>
        /// <returns></returns>
        public string Update(MDefect defect)
        {   
            defect.UpdateTime = DateTime.Now;
            int result = Context.Updateable(defect).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.M_DEFECT_HT select * FROM SAJET.M_DEFECT where ID = @ID", new List<SugarParameter> { new SugarParameter("@ID", defect.Id) });
            return "OK";
        }
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="defect"></param>
        /// <returns></returns>
        public string UpdateStatus(MDefect defect)
        {
            
            //int result = Context.Updateable(defect).ExecuteCommand();
            Context.Ado.ExecuteCommand($" update SAJET.M_DEFECT set ENABLED=@enabled,UPDATE_TIME=SYSDATE,UPDATE_EMPNO=@EMPNO WHERE ID in {defect.Id}", new List<SugarParameter> {new SugarParameter("@enabled",defect.Enabled), new SugarParameter("@EMPNO", defect.UpdateEmpno) }
            );
            //备份
            Context.Ado.ExecuteCommand($"insert INTO SAJET.M_DEFECT_HT select * FROM SAJET.M_DEFECT where ID in {defect.Id}");
          
            return "OK";
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="defect"></param>
        /// <returns></returns>
        public string Delete(MDefect defect,string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0)
            {
                return "删除失败Id 不能为空";
            }
            ids = "(" + ids + ")";
            //int result = Context.Updateable(defect).ExecuteCommand();
            Context.Ado.ExecuteCommand($" update SAJET.M_DEFECT set UPDATE_TIME=SYSDATE,UPDATE_EMPNO=@EMPNO WHERE ID in {ids}", new List<SugarParameter> { new SugarParameter("@EMPNO",defect.UpdateEmpno)}
            );
            //备份
            Context.Ado.ExecuteCommand($"insert INTO SAJET.M_DEFECT_HT select * FROM SAJET.M_DEFECT where ID in {ids}" );
            //删除
            Context.Ado.ExecuteCommand($"delete IMES.M_DEFECT where ID in {ids}");
            return "OK";
        }
    }
}
