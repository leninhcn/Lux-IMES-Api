using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Infrastructure.Model;
using ZR.Model.Business;
using ZR.Service.Repair.IRepairService;
using ZR.ServiceCore.Model.Dto;

namespace ZR.Service.Repair
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class RepairPanelService : BaseService<SnStatus>, IRepairPanelService
    {
        public async Task<ExecuteResult> getSNByPanel(RepairPanel panel)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sSQL = @"select  /*+RULE*/SERIAL_NUMBER, OUT_STATIONTYPE_TIME  FROM SAJET.P_SN_STATUS a
where A.PANEL_NO = @PANEL_NO  AND A.WORK_FLAG = '0' AND A.CURRENT_STATUS= '1'
AND EXISTS (SELECT * FROM SAJET.m_ROUTE_DETAIL WHERE NEXT_STATION_TYPE = @NEXT_STATION_TYPE and STATION_TYPE = a.STATION_TYPE and ROUTE_NAME = a.ROUTE_NAME)";

                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sSQL, new List<SugarParameter>
                    {   new SugarParameter("@PANEL_NO", panel.panel),
                        new SugarParameter("@NEXT_STATION_TYPE", panel.nextStationType)});
                exeRes.Anything = dtTemp;
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }


        public async Task<ExecuteResult> getPanelByStationType(string stationType)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sSQL = string.Format(@"select  /*+RULE*/DISTINCT(A.PANEL_NO) FROM SAJET.P_SN_STATUS a
                    where  A.PANEL_NO <> 'N/A'
                    AND A.WORK_FLAG = '0'
                    AND A.CURRENT_STATUS= '1'
                    AND EXISTS 
                        (SELECT * FROM SAJET.m_ROUTE_DETAIL WHERE NEXT_STATION_TYPE = '{0}' and STATION_TYPE = a.STATION_TYPE and ROUTE_NAME = a.ROUTE_NAME)", stationType);
             
                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sSQL);
                exeRes.Anything = dtTemp;
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<ExecuteResult> geInfoBySN(string sn)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sSQL = @"select A.SERIAL_NUMBER,A.WORK_ORDER, b.ipn, a.OUT_STATIONTYPE_TIME
                    FROM SAJET.P_SN_STATUS a, imes.m_part b
                    where A.SERIAL_NUMBER =@SERIAL_NUMBER and a.IPN = b.IPN and ROWNUM = 1";

                DataTable dtTemp = await Context.Ado.GetDataTableAsync(sSQL, new List<SugarParameter>
                    {   new SugarParameter("@SERIAL_NUMBER", sn) });
                exeRes.Anything = dtTemp;
                exeRes.Status = true;
            }
            catch (Exception ex)
            {
                exeRes.Message = "Error:" + ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public async Task<DataTable> ShowDefect(string sn,DateTime dtTime)
        {
           string sSQL = @"select A.RECID,B.DEFECT_CODE,B.DEFECT_DESC,a.line,a.station_name, A.RP_STATUS , a.STATION_TYPE  ,A.LOCATION 
                            FROM SAJET.P_SN_DEFECT a , 
                                imes.m_defect b
                            where a.SERIAL_NUMBER = @SERIAL_NUMBER 
                            and a.REC_TIME >= @STIME 
                            and a.DEFECT_CODE = b.DEFECT_CODE(+)";
            DataTable dtTemp = await Context.Ado.GetDataTableAsync(sSQL, new List<SugarParameter>
                    {   new SugarParameter("@SERIAL_NUMBER", sn),
                        new SugarParameter("@STIME", dtTime)});

            return dtTemp;
        }

        public async Task<ExecuteResult> getNextStationType(string panel)
        {
            string sSQL = string.Format(@"select a.NEXT_STATION_TYPE  FROM SAJET.P_SN_STATUS a where a.PANEL_NO ='{0}'  AND a.NEXT_STATION_TYPE <> '0' and rownum = 1", panel);

            ExecuteResult exeRes = new ExecuteResult();
            DataTable dtTemp = await Context.Ado.GetDataTableAsync(sSQL);
            if (dtTemp == null || dtTemp.Rows.Count == 0)
            {
                exeRes.Status = false;
                exeRes.Message = "没有查询到 NEXT_STATION_TYPE";
            }
            else {
                exeRes.Status = true;
                exeRes.Anything = dtTemp.Rows[0]["NEXT_STATION_TYPE"].ToString();
            }
            return exeRes;
        }


        public  async Task<bool> repairPanel(panelDto currStation,string sNextStationType)
        {
           
            string sSQL = string.Format(@"update SAJET.P_SN_STATUS a set a.LINE =@line, a.STAGE = @stage, a.STATION_TYPE = @station_type, a.STATION_NAME = @station_name, 
                                             a.WIP_STATION_TYPE =@WIP_STATION_TYPE, a.NEXT_STATION_TYPE = @NEXT_STATION_TYPE
                                          where a.PANEL_NO = @PANEL_NO");

            var affected = await Context.Ado.ExecuteCommandAsync(sSQL, new List<SugarParameter>
            {
                new SugarParameter("@line", currStation.LineName),
                new SugarParameter("@stage", currStation.StageName),
                new SugarParameter("@station_type", currStation.StationType),
                new SugarParameter("@station_name",  currStation.StationName),
                new SugarParameter("@WIP_STATION_TYPE",  sNextStationType),
                new SugarParameter("@NEXT_STATION_TYPE",  sNextStationType),
                new SugarParameter("@PANEL_NO",  currStation.panel),
            });

            return affected > 0;
        }
    }
}
