using Infrastructure.Attribute;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class RepairReturnStaService : BaseService<SnStatus>, IRepairReturnStaService
    {
        public async Task<ExecuteResult> ShowData(returnStaData retData)
        {
           // ShowData(int idx, int fIdx, string sFieldName, string fText)
            ExecuteResult exeRes = new ExecuteResult();
            string sName = string.Empty;
            try
            {
               string sSQL = "Select * from IMES.M_RETURNSTATION_HEADER " ;
                if (retData.Enabled == 0)
                    sSQL = sSQL + " where Enabled = 'Y' ";
                else if (retData.Enabled == 1)
                    sSQL = sSQL + " where Enabled = 'N' ";

                if (retData.SelectedIndex > -1 && retData.sFilterText != "")
                {
                    sName = retData.sFieldName;
                    if (retData.SelectedIndex <= 1)
                        sSQL = sSQL + " and ";
                    else
                        sSQL = sSQL + " where ";
                    sSQL = sSQL + sName + " like '%" + retData.sFilterText + "%'";
                }
                sSQL = sSQL + " order by CATEGORY ";

                exeRes.Anything = await Context.Ado.GetDataTableAsync(sSQL);
                exeRes.Status = true;

            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> ShowDetailData(stationDetailData detailData)
        {
             //   ShowDetailData(string sId, int idx, string sFieldName, int didx, string dFilter)
             ExecuteResult exeRes = new ExecuteResult();
            string sName = "";
            try
            {
               string sSQL = "Select a.* from IMES.M_RETURNSTATION_DETAIL a, IMES.M_RETURNSTATION_HEADER b " 
               + "where a.MODEL = b.MODEL(+) and a.CATEGORY=b.CATEGORY(+) and a.KOLKATA=b.KOLKATA(+) "
               + "and B.id = '" + detailData.sId + "' ";
                if (detailData.Enabled == 0)
                    sSQL = sSQL + " and a.Enabled = 'Y' ";
                else if (detailData.Enabled == 1)
                    sSQL = sSQL + " and a.Enabled = 'N' ";
                if (detailData.detailSelectIndex > -1 && detailData.sFilterText != "")
                {
                    sName = detailData.sFieldName;
                    sSQL = sSQL + " and " + "a." + sName + " like '" + detailData.sFilterText + "%'";
                }
                sSQL = sSQL + " order by RETURNSTATION_TYPE ";
                exeRes.Anything = await Context.Ado.GetDataTableAsync(sSQL);
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }

            return exeRes;
        }

        public async Task<DataTable> getModel()
        {
            string sSQL = " SELECT MODEL  FROM IMES.M_MODEL WHERE ENABLED='Y'   ORDER BY MODEL";
            DataTable dt = await Context.Ado.GetDataTableAsync(sSQL);
            return dt;
        }

        public async Task<DataTable> getDt(ModelInfo modelInfo)
        {
            string sSQL = @" Select * from IMES.M_RETURNSTATION_HEADER  Where model = '" + modelInfo.MODEL + "' and CATEGORY='" + modelInfo.CATEGORY + "' and KOLKATA=" + modelInfo.KOLKATA + " ";

            if (modelInfo.Type == "MODIFY")
            {
                sSQL += "and id<> '" + modelInfo.ID + "'";
            }
            DataTable dt = await Context.Ado.GetDataTableAsync(sSQL);
          
            return dt;
        }

        public async Task<ExecuteResult> AppendData(ModelInfo modelInfo,string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();

            exeRes  = await GetMaxID("SAJET.M_RETURNSTATION_HEADER", "ID", 8);
            if (!exeRes.Status)
            {
                return exeRes;
            }
            string sMaxID = (string)exeRes.Anything;

            try
            {
               string sSQL = @" Insert into IMES.M_RETURNSTATION_HEADER (id,MODEL,CATEGORY,TOTAL_MATERIAL,KOLKATA,ENABLED,UPDATE_EMPNO,UPDATE_TIME,CREATE_EMPNO) 
                    Values  (@id,@MODEL,@CATEGORY,@TOTAL_MATERIAL,@KOLKATA,'Y',@UPDATE_EMPNO,SYSDATE,@CREATE_EMPNO) ";
               
                var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@id", sMaxID),
                new SugarParameter("@MODEL", modelInfo.MODEL),
                new SugarParameter("@CATEGORY",modelInfo.CATEGORY),
                new SugarParameter("@TOTAL_MATERIAL", modelInfo.TOTAL_MATERIAL),
                new SugarParameter("@KOLKATA",modelInfo.KOLKATA),
                new SugarParameter("@UPDATE_EMPNO", userNo),
                new SugarParameter("@CREATE_EMPNO",userNo)
            });

                exeRes.Status = affected > 0;

                await CopyToHistory(sMaxID);
            }
            catch (Exception ex)
            {
                // utility.AddLogToServer(utility.GlobalCurrentFunctinName, utility.GlobalUserNo, LogLevel.Error, ex.Message);
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }
            return exeRes;

        }

        public async Task<ExecuteResult> AppendDetailData(detailPartStation stations,string userNo)
        {
            ExecuteResult exeRes =await GetMaxID("SAJET.M_RETURNSTATION_DETAIL", "ID", 8);
            if (!exeRes.Status)
            {
                return exeRes;
            }
            string sMaxID = (string)exeRes.Anything;
            try
            {
               string sSQL = @" Insert into IMES.M_RETURNSTATION_DETAIL  
               (id,MODEL,CATEGORY,RETURNSTATION_TYPE,UNLINKEDMATERIAL,AREA ,KOLKATA,ENABLED,UPDATE_EMPNO,UPDATE_TIME,CREATE_EMPNO)  
                Values    (@id,@MODEL,@CATEGORY,@RETURNSTATION_TYPE,@UNLINKEDMATERIAL,@AREA,@KOLKATA,'Y',@UPDATE_EMPNO,SYSDATE,@CREATE_EMPNO) ";

                var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
               {
                new SugarParameter("@id", sMaxID),
                new SugarParameter("@MODEL", stations.model),
                new SugarParameter("@CATEGORY",stations.category),
                new SugarParameter("@RETURNSTATION_TYPE", stations.returnStationType),
                new SugarParameter("@UNLINKEDMATERIAL", stations.unLinkmaterial),
                new SugarParameter("@AREA", stations.area),
                new SugarParameter("@KOLKATA",stations.kolkata),
                new SugarParameter("@UPDATE_EMPNO", userNo),
                new SugarParameter("@CREATE_EMPNO",userNo)
                 });

                exeRes.Status = affected > 0;

                await CopyToDetailHistory(sMaxID);
            }
            catch (Exception ex)
            {
                // utility.AddLogToServer(utility.GlobalCurrentFunctinName, utility.GlobalUserNo, LogLevel.Error, ex.Message);
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }

            return exeRes;
        }

        public async Task<bool> CopyToHistory(string sId)
        {
            string sSQL = @" Insert into IMES.M_RETURNSTATION_HEADER_HT
                         Select * from IMES.M_RETURNSTATION_HEADER  where ID  = '" + sId + "' ";
       
            var affected = await Context.Ado.ExecuteCommandAsync(sSQL);

            return affected > 0;
        }

        public async Task<bool> CopyToDetailHistory(string sId)
        {
            string sSQL = $@" Insert into IMES.M_RETURNSTATION_DETAIL_HT 
                         Select * from IMES.M_RETURNSTATION_DETAIL
                         where id = '{sId}'";

            var affected = await Context.Ado.ExecuteCommandAsync(sSQL);

            return affected > 0;
        }

        private async Task<ExecuteResult> GetMaxID(string sTable, string sField, int iIDLength)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                var sRes = new SugarParameter("TRES", null, true);
                var tMaxID = new SugarParameter("T_MAXID", null, true);

                await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_GET_MAXID",
                new SugarParameter[]
                    {
                    new SugarParameter("TFIELD", sField),
                    new SugarParameter("TTABLE", sTable),
                    new SugarParameter("TNUM", iIDLength.ToString()),
                    sRes,tMaxID
                    });


                exeRes.Anything = "0";
                if (sRes.Value.ToString() != "OK")
                {
                    exeRes.Status = false;
                    exeRes.Message = sRes.Value.ToString();
                }
                else
                {
                    exeRes.Status = true;
                    exeRes.Anything = tMaxID.Value.ToString();

                }
            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
                exeRes.Anything = "0";
            }
            return exeRes;
        }

        public async Task<ExecuteResult> ModifyData(ModelInfo modelInfo,string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
               string  sSQL = @" Update IMES.M_RETURNSTATION_HEADER     
                      set MODEL = @MODEL,CATEGORY = @CATEGORY,TOTAL_MATERIAL = @TOTAL_MATERIAL,KOLKATA = @KOLKATA ,
                       UPDATE_EMPNO = @UPDATE_EMPNO ,UPDATE_TIME = SYSDATE  where id = @id ";

                var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@MODEL", modelInfo.MODEL),
                new SugarParameter("@CATEGORY",modelInfo.CATEGORY),
                new SugarParameter("@TOTAL_MATERIAL", modelInfo.TOTAL_MATERIAL),
                new SugarParameter("@KOLKATA",modelInfo.KOLKATA),
                new SugarParameter("@UPDATE_EMPNO", userNo),
                new SugarParameter("@id", modelInfo.ID),
            });

                exeRes.Status = affected > 0;
                await CopyToHistory(modelInfo.ID);
            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> ModifyDetailData(detailPartStation stations, string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sSQL = @" Update IMES.M_RETURNSTATION_DETAIL set set MODEL = @MODEL,CATEGORY = @CATEGORY
                               ,RETURNSTATION_TYPE = @RETURNSTATION_TYPE
                               ,UNLINKEDMATERIAL = @UNLINKEDMATERIAL 
                               ,UPDATE_EMPNO = @UPDATE_USERID 
                               ,UPDATE_TIME = SYSDATE 
                               ,AREA=@AREA
                               ,KOLKATA=@KOLKATA 
                             where id=@id ";

                var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
                 {
                    new SugarParameter("@MODEL", stations.model),
                    new SugarParameter("@CATEGORY",stations.category),
                    new SugarParameter("@RETURNSTATION_TYPE", stations.returnStationType),
                    new SugarParameter("@UNLINKEDMATERIAL", stations.unLinkmaterial),
                    new SugarParameter("@UPDATE_USERID", userNo),
                    new SugarParameter("@AREA", stations.area),
                    new SugarParameter("@KOLKATA",stations.kolkata),
                    new SugarParameter("@id",stations.sId)
                  });

                exeRes.Status = affected > 0;
                await CopyToDetailHistory(stations.sId);
            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> Disable(ModelInfo modelInfo, string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sSQL = @" Update IMES.M_RETURNSTATION_HEADER set Enabled = '" + modelInfo.Enabled + "' ,UPDATE_EMPNO = '" + userNo + "' ,UPDATE_TIME = SYSDATE where  ID = '" + modelInfo.ID + "'";

                var affected = await Context.Ado.ExecuteCommandAsync(sSQL);

                exeRes.Status = affected > 0;

                await CopyToHistory(modelInfo.ID);
            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }

            return exeRes;
        }

        public async Task<ExecuteResult> DetailDisabled(ModelInfo modelInfo, string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sSQL = @" Update IMES.M_RETURNSTATION_DETAIL set Enabled = '" + modelInfo.Enabled + "' ,UPDATE_EMPNO = '" + userNo + "' ,UPDATE_TIME = SYSDATE where  ID = '" + modelInfo.ID + "'";

                var affected = await Context.Ado.ExecuteCommandAsync(sSQL);

                exeRes.Status = affected > 0;

                await CopyToDetailHistory(modelInfo.ID);
            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }

            return exeRes;
        }

        public async Task<DataTable> getStage(string sName)
        {
            string sSQL = " Select * from IMES.M_RETURNSTATION_DETAIL "
                    + " where CATEGORY = '" + sName + "'";
            DataTable dt = await Context.Ado.GetDataTableAsync(sSQL);

            return dt;
        }

        public async Task<DataTable> getStage(detailPartStation stations)
        {
            string sSQL = $@"Select * from IMES.M_RETURNSTATION_DETAIL  Where RETURNSTATION_TYPE = '{stations.returnStationType}' and CATEGORY='{stations.category}'
                                and UNLINKEDMATERIAL='{stations.unLinkmaterial}' and KOLKATA= {Int32.Parse(stations.kolkata)} and model='{stations.model}' and area='{stations.area}' ";

            DataTable dt = await Context.Ado.GetDataTableAsync(sSQL);
            
            return dt;
        }

        public async Task<ExecuteResult> Delete(string sId,string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sSQL = string.Format(@" Update IMES.M_RETURNSTATION_HEADER set Enabled = 'D'  ,UPDATE_EMPNO ='{0}',,UPDATE_TIME = SYSDATE ,where ID = '{1}' ", userNo, sId);

                var affected = await Context.Ado.ExecuteCommandAsync(sSQL);

                await CopyToHistory(sId);

                sSQL = string.Format(@" Delete IMES.M_RETURNSTATION_HEADER where ID = '{0}' ", sId);

                affected = await Context.Ado.ExecuteCommandAsync(sSQL);

                exeRes.Status = affected > 0;

            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> DetailDelete(string sId, string userNo)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sSQL = string.Format(@" Update IMES.M_RETURNSTATION_DETAIL set Enabled = 'D'  ,UPDATE_EMPNO ='{0}',,UPDATE_TIME = SYSDATE ,where ID = '{1}' ", userNo, sId);

                var affected = await Context.Ado.ExecuteCommandAsync(sSQL);

                await CopyToDetailHistory(sId);

                sSQL = string.Format(@" Delete IMES.M_RETURNSTATION_DETAIL where ID = '{0}' ", sId);

                affected = await Context.Ado.ExecuteCommandAsync(sSQL);

                exeRes.Status = affected > 0;

            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }
            return exeRes;
        }

        public async Task<DataTable> HistoryData(string sName)
        {
          string sSQL = string.Format(@"Select a.Stage,a.Stage_Desc,a.ENABLED,b.emp_name,a.UPDATE_TIME from IMES.M_RETURNSTATION_HEADER_HT a,IMES.M_EMP b 
                         Where a.ID='{0}' and a.UPDATE_EMPNO = b.emp_no(+) Order By a.Update_Time ",sName);
           
          DataTable dt = await Context.Ado.GetDataTableAsync(sSQL);

          return dt;

        }
        public async Task<DataTable> DetailHistoryData(string sName)
        {
           string sSQL = string.Format($@" Select a.STATION_TYPE,a.STATIONTYPE_CUSTOMER,a.OPERATE_TYPE,a.CLIENT_TYPE,c.Stage,a.ENABLED,b.emp_name,a.UPDATE_TIME 
                                          from IMES.M_RETURNSTATION_DETAIL_HT a,IMES.M_EMP b ,IMES.M_RETURNSTATION_HEADER c
                                          Where a.ID = '{sName}'  and a.UPDATE_EMPNO = b.emp_no(+) and a.STAGE = c.STAGE(+) 
                                           Order By a.Update_Time ");
            DataTable dt = await Context.Ado.GetDataTableAsync(sSQL);

            return dt;
        }

        public async Task<ExecuteResult> SaveExcel(detailPartStation stations,string userNo)
        {
            ExecuteResult exeRes =await GetMaxID("SAJET.M_RETURNSTATION_DETAIL", "ID", 8);

            if (!exeRes.Status)
            {
                return exeRes;
            }
            string sMaxID = (string)exeRes.Anything;
            try
            {
                string sSQL = @"MERGE INTO IMES.M_RETURNSTATION_DETAIL T1
                              USING (SELECT  @ID             AS ID,
                                             @MODEL    AS MODEL,
                                             @CATEGORY    AS CATEGORY,
                                             @RETURNSTATION_TYPE AS RETURNSTATION_TYPE,
                                             @UNLINKEDMATERIAL         AS UNLINKEDMATERIAL,
                                             @AREA         AS AREA,
                                             @KOLKATA         AS KOLKATA,
                                             @UPDATE_EMPNO  AS UPDATE_EMPNO,
                                             SYSDATE        AS UPDATE_TIME,
                                             'Y'            AS ENABLED,
                                             @CREATE_EMPNO AS CREATE_EMPNO
                                       FROM DUAL) T2
                                 ON (T1.MODEL=T2.MODEL AND T1.CATEGORY=T2.CATEGORY AND T1.KOLKATA=T2.KOLKATA )
                             WHEN MATCHED
                             THEN
                             UPDATE SET T1.RETURNSTATION_TYPE = T2.RETURNSTATION_TYPE,
                                        T1.UNLINKEDMATERIAL = T2.UNLINKEDMATERIAL,
                                        T1.AREA = T2.AREA,
                                        T1.UPDATE_EMPNO = T2.UPDATE_EMPNO,
                                        T1.UPDATE_TIME=SYSDATE
                             WHEN NOT MATCHED
                             THEN
                             INSERT     (T1.ID,T1.MODEL,T1.CATEGORY,T1.RETURNSTATION_TYPE,T1.UNLINKEDMATERIAL,T1.AREA,T1.KOLKATA,T1.UPDATE_EMPNO)
                                 VALUES (T2.ID,T2.MODEL,T2.CATEGORY,T2.RETURNSTATION_TYPE,T2.UNLINKEDMATERIAL,T2.AREA,T2.KOLKATA,T2.UPDATE_EMPNO)";

            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
               {
                new SugarParameter("@ID", sMaxID),
                new SugarParameter("@MODEL", stations.model),
                new SugarParameter("@CATEGORY",stations.category),
                new SugarParameter("@RETURNSTATION_TYPE", stations.returnStationType),
                new SugarParameter("@UNLINKEDMATERIAL", stations.unLinkmaterial),
                new SugarParameter("@AREA", stations.area),
                new SugarParameter("@KOLKATA",stations.kolkata),
                new SugarParameter("@UPDATE_EMPNO", userNo),
                new SugarParameter("@CREATE_EMPNO",userNo)
                 });

                await CopyToHistory(sMaxID);

                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Status = false;
                exeRes.Message = ex.Message;
            }
            return exeRes;
        }
    }

}
