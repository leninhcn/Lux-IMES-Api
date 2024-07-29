using Infrastructure.Attribute;
using JinianNet.JNTemplate;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto.WorkOrder;
using ZR.Service.WoManagement.IService;
using ZR.Repository;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Drawing.Drawing2D;
using Infrastructure.Enums;
using Aliyun.OSS;
using ZR.Model.Dto;
using ZR.Model.System.ZR.Model.Business;
using ZR.ServiceCore.Model.Dto;
using System.Security.Policy;

namespace ZR.Service.WoManagement
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IEquipmentWoService), ServiceLifetime = LifeTime.Transient)]
    public class EquipmentWoServiceImpl : BaseService<HjxsPieWtEquDiSpatchListH>, IEquipmentWoService
    {
        public PagedInfo<HjxsPieWtEquDiSpatchListH> ShowData(EquipmentWoFilter filter, PagerInfo pager, string empno)
        {
            string sSQL;

            if (empno.StartsWith("PED"))
            {
                sSQL = " CREATETIME >= to_date('" + filter.startDate + "','yyyy-MM-dd') and CREATETIME < to_date('" + filter.endDate + "','yyyy-MM-dd')+1 and EQUCODE='" + empno + "'";
            }
            else
            {
                sSQL = " CREATETIME >= to_date('" + filter.startDate + "','yyyy-MM-dd') and CREATETIME < to_date('" + filter.endDate + "','yyyy-MM-dd')+1 ";
            }

            if (filter.filterField != null && filter.filterField != "" && filter.filterValue != null && filter.filterValue != "")
            {
                string sName = filter.filterField;

                sSQL = sSQL + " AND " + sName + " like '" + filter.filterValue + "%'";
            }
            var exp = Context.Queryable<HjxsPieWtEquDiSpatchListH>().Where(sSQL).OrderBy(it => it.Id).ToPage(pager);

            return exp;
        }

        public List<HjxsPieWtEquDispatchListB> getDispatchList_b(string strID)
        {
            var resList =  
                Context.Queryable<HjxsPieWtEquDispatchListB>()
                .Where(it=>it.Hid == strID)
                .ToList();
            return resList;
        }

        public List<string> getChejian(string site)
        {
            string sSQL = $"select distinct LINE_SAP from IMES.M_LINE where Enabled='Y' and LINE_SAP is not null and site = '{site}'";

            var temp = Context.Ado.GetDataTable(sSQL);

            var lines = temp.AsEnumerable().Select(r => r["LINE_SAP"].ToString()).Distinct().ToList();
            return lines;
        }

        public DataTable GetEquipmentCode(string devType)
        {
            string sSQL = $@"Select a.DevCode As 设备编码
                    ,a.DEVTYPE AS 设备类型
		            ,a.DevDescrip As 设备名称
		            ,a.FIXCODE As 固定资产编码
		            ,a.FIXTYPE AS 固定资产类型
		            ,a.DEPARTCODEUSE AS 使用部门
		            ,a.STATUS AS 状态
		            ,a.VENDORCODE AS 供应商
		            ,a.LTNCODE
		            ,a.OEETYPE
		            ,a.DEVPACKSIZE 设备规格
                    From IMES.HJXS_dv_devCard a 
                    where (a.OeeType like '%{devType}%') order by a.DevCode";

            var temp = Context.Ado.GetDataTable(sSQL);

            return temp;

        }

        public DataTable CheckEquipmentCodeStatus (string DevCode)
        {
            string sSQL = $@"Select a.Status,b.UseFlag
                     From IMES.HJXS_dv_devcard a 
                     Join IMES.HJXS_dv_status b On a.Status=b.Status
                     Where a.DevCode= '{DevCode}' ";

            var temp = Context.Ado.GetDataTable(sSQL);

            return temp;

        }

        public DataTable GetProductLot(string Chejian)
        {
            string sSQL = $@"select A.WO_SCHEDULE_START_DATE 排产日期
                           ,A.WORK_SHOP 车间
                           ,A.LINE 产线
                           ,A.MODEL 客户
                           ,A.IPN 产品型号
                           ,A.WORK_ORDER 生产批次
                           ,'' 单位
                           ,A.TARGET_QTY 排产数量
                           ,'' 生产令
                           ,'' 令序号
                           ,'' 销售订单
                           ,'' 销售批号
                           ,'' 客户PO
                           ,'' 客户型号
                           ,A.WO_STATUS 排产状态
                           ,'' 产品描述
                    from IMES.P_WO_BASE A
                    --join IMES.HJXS_Gp_Pro_ProWorkshopLine B on A.LINE=B.PROLINE 
                    where A.WO_STATUS in('2','4') and A.WORK_SHOP like '%{Chejian}' order by A.WO_SCHEDULE_START_DATE desc";

            var temp = Context.Ado.GetDataTable(sSQL);

            return temp;
        }

        public bool ModuleAuth(string moduleName, string pAction, string site, string empno)
        {
            string sSql = @$"select b.* from imes.m_role_emp a
                        join imes.m_role_action b on a.role_id = b.role_id and b.enabled = 'Y'
                        join (
                        select b.id action_id from imes.s_program_fun_name a 
                        join imes.s_fun_action b on a.id = b.fun_id and b.action_name = '{pAction}' and b.enabled = 'Y' 
                        where a.function = '{moduleName}'
                        ) c
                        on b.action_id = c.action_id
                        where a.emp_no = '{empno}' and a.enabled = 'Y' ";

            var temp = Context.Ado.GetDataTable(sSql);

            return temp.Rows.Count > 0;
        }


        public DataTable getTraceAll(string billNum)
        {
            string sSql = $@"select EMPNO 操作人工号, EMPNAME 操作人姓名, PROCESS_NAME 操作流程, APPROVAL_TYPE 操作类型, CREATE_TIME 操作时间 from IMES.HJXS_Pie_WtEquDispatch_Trace where BILLCODE='{billNum}'";
            var temp = Context.Ado.GetDataTable(sSql);
            return temp;
        }

        public DataTable getWorkLine(string worknum)
        {
            string sSql = $@"select C.CUT_CARDNO 线卡号
                            ,A.WORK_ORDER 生产批次
                            ,A.IPN 产品型号
                            ,A.WO_SCHEDULE_START_DATE 排产日期
                            ,C.VERNO 线卡版本
                            ,A.LINE 产线
                            ,C.ITEMDESCRIPMT 线才特征
                            ,C.MAKESIZEL 加工线长
                            ,'' 生产令
                            ,A.TARGET_QTY 批次数量
                            ,C.QTY 线组数量
                            ,A.TARGET_QTY 任务数量
                            ,'' 客户型号
                            ,'' 型号描述
                            ,'1' 裁线序号
                            ,C.MAKESIZELTOLERANCE 线长公差
                            ,C.ITEMCODEMT 线材编码
                            ,A.TARGET_QTY*C.MAKESIZEL/1000 线材数量
                            ,C.ITEMCODEPRESSA 前端端子
                            ,A.TARGET_QTY A端子数量
                            ,C.ITEMCODEPRESSB 后端端子
                            ,A.TARGET_QTY B端子数量
                            ,C.APPLICABLEDIEPRESSA A模具编号
                            ,C.APPLICABLEDIEPRESSB B模具编号
                            ,C.LCPSNUM 发行编号
                            ,C.ECNCOUNT
                    from IMES.P_WO_BASE A 
                    --join IMES.HJXS_Gp_Pro_ProWorkshopLine B on A.LINE=B.PROLINE 
                    join IMES.HJXS_e_lcps C on C.ITEMCODE=A.IPN 
                    where A.WORK_ORDER='{worknum}' and ECNCOUNT=0 
                    and C.LCPSNUM not in(select ISSUENUMBER from IMES.HJXS_Pie_WtEquDispatchList_b where PROLOT='{worknum}') 
                    order by C.CUT_CARDNO";
            var temp = Context.Ado.GetDataTable(sSql);
            return temp;
        }


        public List<HjxsPieWtEquDispatchTrace> getTraceByType(string billNum, string strType)
        {
            var predicate = Expressionable.Create<HjxsPieWtEquDispatchTrace>();
            predicate.AndIF(!string.IsNullOrEmpty(strType), it => it.ApprovalType == strType);
            predicate.AndIF(!string.IsNullOrEmpty(billNum), it => it.BillCode == billNum);


            var response = Context.Queryable<HjxsPieWtEquDispatchTrace>()
                .Where(predicate.ToExpression()).OrderBy(it => it.CreateTime)
                .ToList();
            return response;
        }


        public string GetNoteNum(string pNoteType)
        {
            string sSQL = $@"Select IMES.FN_GET_NOTE_NUM('{pNoteType}') from dual";
            DataTable dtTemp = Context.Ado.GetDataTable(sSQL);
            return dtTemp.Rows[0][0].ToString();
        }

        public string GetGuID()
        {
            string sSQL = @"select rawtohex(sys_guid()) from dual";
            DataTable dtTemp = Context.Ado.GetDataTable(sSQL);
            return dtTemp.Rows[0][0].ToString();
        }

        public bool SaveDispatchList_h(HjxsPieWtEquDiSpatchListH dto, string empNo)
        {

            try
            {
                long insertErp = Context.Insertable(dto).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
                if (SaveDispatch_Trace(dto.VbillCode, "生产班长派工", "审批", empNo) == false)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool ModifyDispatchList_h(HjxsPieWtEquDiSpatchListH dto, string empNo, string sUpdateType)
        {
            try
            {
                string sSQL = $@" update IMES.HJXS_PIE_WTEQUDISPATCHLIST_H
                          set ProDept='{dto.ProDept}',
                              EquCode='{dto.EquCode}',
                              EquType='{dto.EquType}',
                              OpWorker='{empNo}',
                              Remark='{dto.Remark}',
                              CabQty='{dto.CabQty}',
                              ProNum='{dto.ProNum}',
                              ProLot='{dto.ProLot}',
                              CusProNum='{dto.CusProNum}',
                              LotQty='{dto.LotQty}',
                              PrintName='{dto.PrintName}',
                              WFState='{dto.WfState}',
                              WFUserName='{empNo}',
                              WFIsFinish='0',
                              WFIsStart='0'
                           WHERE ID = '{dto.Id}'";

                Context.Ado.SqlQuery<string>(sSQL);

                string sqlStr = $"INSERT INTO IMES.HJXS_Pie_WtEquDispatchList_h_HT(SELECT * FROM IMES.HJXS_PIE_WTEQUDISPATCHLIST_H WHERE ID = '{dto.Id}' )";
                Context.Ado.SqlQuery<string>(sqlStr);

                if (sUpdateType.Equals("NEXT"))
                {
                    SaveDispatch_Trace(dto.VbillCode, dto.WfState, "审批", empNo);
                }
                if (sUpdateType.Equals("REJECT"))
                {
                    SaveDispatch_Trace(dto.VbillCode, dto.WfState, "驳回", empNo);
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool SaveDispatch_Trace(string billCode, string process_name, string approval_type, string empNo)
        {
            try
            {
                string sqlStr = $@"Insert Into IMES.HJXS_Pie_WtEquDispatch_Trace(ID, BILLCODE, EMPNO, PROCESS_NAME, APPROVAL_TYPE)
                         values(rawtohex(sys_guid()),'{billCode}','{empNo}','{process_name}','{approval_type}')";
                Context.Ado.SqlQuery<string>(sqlStr);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool SaveDispatchList_B(HjxsPieWtEquDispatchListB dispatchListB)
        {
            try
            {
                long insertErp = Context.Insertable(dispatchListB).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool ModifyDispatchList_B(HjxsPieWtEquDispatchListB dispatchListB)
        {
            try
            {
                string sSQL = $@" update IMES.HJXS_Pie_WtEquDispatchList_b
                          set StartTime= '{dispatchListB.StartTime}',
                              EndTime='{dispatchListB.EndTime}',
                              Operator='{dispatchListB.Operator}',
                              FiniStatue='{dispatchListB.FiniStatue}',
                              BatchCode='{dispatchListB.BatchCode}',
                              TerminalBatchA='{dispatchListB.TerminalBatchA}',
                              TerminalBatchB='{dispatchListB.TerminalBatchB}',
                              ModelBatchA='{dispatchListB.ModelBatchA}',
                              ModelBatchB='{dispatchListB.ModelBatchB}',
                              CableFinishQty='{dispatchListB.CableFinishQty}',
                              TerAFinishQty='{dispatchListB.TerAFinishQty}',
                              TerBFinishQty='{dispatchListB.TerBFinishQty}'
                           WHERE ID = '{dispatchListB.Id}'";
                Context.Ado.SqlQuery<string>(sSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool ModifyScanLableData(HjxsPieWtScanLableData hjxsPieWtScanLableData)
        {

            try
            {
                string sqlStr = $@" update IMES.HJXS_Pie_WtScanLableData
                          set SCANQTY='{hjxsPieWtScanLableData.ScanQty}',
                              LEFTQTY='{hjxsPieWtScanLableData.LeftQty}',
                              NOTE='{hjxsPieWtScanLableData.Note}',
                              UPDATETIME=to_char(sysdate,'yyyy/mm/dd hh24:mi:ss')
                           WHERE ID = '{hjxsPieWtScanLableData.Id}' ";
                Context.Ado.SqlQuery<string>(sqlStr);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }



        public string CheckSix(string str)
        {
            if (str.Contains("[") && str.Split('[').Length > 8)
            {
                return "1";
            }
            else if (str.Contains("$") && str.Split('$').Length > 8)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public List<HjxsPieWtScanLableData> GetScanLableData(HjxsPieWtScanLableData sld)
        {

            var exp = Expressionable.Create<HjxsPieWtScanLableData>();
            exp.And(it => it.BillCode == sld.BillCode);
            exp.And(it => it.BatchCode == sld.BatchCode);
            exp.And(it => it.ProNum == sld.ProNum);
            exp.And(it => it.LabPage == sld.LabPage);

            var response = Context.Queryable<HjxsPieWtScanLableData>()
                .Where(exp.ToExpression())
                .ToList();
            return response;
        }

        public bool SaveScanLableData(HjxsPieWtScanLableData hjxsPieWtScanLableData)
        {
            long insertErp = Context.Insertable(hjxsPieWtScanLableData).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertErp > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
