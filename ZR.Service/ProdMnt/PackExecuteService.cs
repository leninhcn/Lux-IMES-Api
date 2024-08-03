using Infrastructure.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging.Abstractions;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Dto.ProdDto;
using ZR.Service.IService;

namespace ZR.Service
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class PackExecuteService : BaseService<SnStatus>, IPackExecuteService
    {
        public async Task<DataTable> GetOptionData(string station)
        {
            var sql = @"SELECT * FROM SAJET.m_MODULE_PARAM WHERE MODULE_NAME = 'PACKING' and FUNCTION_NAME = 'Work Station Configuration' and PARAME_NAME = @station";
            return await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter>
            {
                new SugarParameter("@station", station),
            });
        }

        public async Task<DataTable> GetPackingAction()
        {
            return await Context.Ado.GetDataTableAsync("select param_value FROM SAJET.s_base where param_name = 'Packing Action'");
        }

        public async Task<string> GetWorkOrderBySn(string input)
        {
            if (input == "N/A") return input;
            var wo = await Context.Queryable<SnStatus>()
                .Where(x => x.SerialNumber == input)
                .Select(x => x.WorkOrder)
                .SingleAsync();

            wo ??= await Context.Queryable<SnStatus>()
                    .Where(x => x.CustomerSn == input)
                    .Select(x => x.WorkOrder)
                    .SingleAsync();

            return wo ?? input;
        }

        public async Task<(bool, string)> CheckWo(string wo, string station)
        {
            var oMsg = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_CHK_WO",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    oMsg, tRes
                });

            var msg = tRes.Value.ToString();
            return (msg == "OK", tRes.Value.ToString());
        }

        public async Task<WoBase> GetWoInfo(string wo)
        {
            return await Context.Queryable<WoBase>().Where(x => x.WorkOrder == wo).SingleAsync();
        }

        public async Task<MPKSpecDto> GetPkSpec(string wo)
        {
            var packSpec = await Context.Queryable<WoBase, MPkspec>((a, b) => a.PkspecName == b.PkspecName)
                .Where((a, b) => a.WorkOrder == wo)
                .Select((a, b) => new MPKSpecDto
                {
                    Id = b.Id,
                    PalletQty = b.PalletQty,
                    CartonQty = b.CartonQty,
                    BoxQty = b.BoxQty,
                    Enabled = b.Enabled,
                }).SingleAsync();


            packSpec.BoxSnQty = packSpec.BoxQty;
            if (packSpec.BoxQty <= 0)
            {
                packSpec.CartonSnQty = packSpec.CartonQty;
            }
            else
            {
                packSpec.CartonSnQty = packSpec.BoxQty * packSpec.CartonQty;
            }

            if (packSpec.CartonQty <= 0)
            {
                packSpec.PalletSnQty = packSpec.PalletQty;
            }
            else
            {
                packSpec.PalletSnQty = packSpec.CartonSnQty * packSpec.PalletQty;
            }

            return packSpec;
        }

        public async Task<List<StationLabelDto>> GetLabelList(string wo, string stationType)
        {
            return await Context.Queryable<SnStatus, MStationtypeLabel>((a, b) => a.Ipn == b.Ipn)
                .Where((a, b) => SqlFunc.Subqueryable<SnStatus>().Where(c => c.WorkOrder == wo && a.SerialNumber == c.SerialNumber).Any() && b.StationType == stationType)
                .OrderBy((_, b) => b.LabelName)
                .Select((a, b) => new StationLabelDto
                {
                    Ipn = a.Ipn,
                    LabelName = b.LabelName,
                    LabelType = b.LabelType,
                    LabelSrvIp = b.LabelSrvIp,
                    LabelDlUrl = b.LabelDlUrl,
                })
                .ToListAsync();
        }

        public async Task<MLabelServer> GetLabelServerInfo(StationLabelDto label)
        {
            return await Context.Queryable<MLabelServer>()
                .Where(a => a.ServerIp == label.LabelSrvIp)
                .FirstAsync();
        }

        public async Task<string> GetUnfinishPallet(string type, string typeValue, string station, string pkSpecName)
        {
            var ex = Expressionable.Create<PPackPallet>()
                .AndIF(type == "Work Order", a => a.WorkOrder == typeValue)
                .AndIF(type != "Work Order", a => a.Ipn == typeValue)
                .And(a => a.StationName == station && a.CloseFlag == "N" && a.PkspecName == pkSpecName);

            return await Context.Queryable<PPackPallet>()
                .Where(ex.ToExpression())
                .Select(a => a.PalletNo)
                .SingleAsync();
        }

        public async Task<string> GetUnfinishCarton(string type, string typeValue, string station, string pkSpecName)
        {
            var sql = "SELECT CARTON_NO FROM SAJET.p_PACK_CARTON ";
            if (type == "Work Order")
            {
                sql = sql + " Where WORK_ORDER = '" + typeValue + "' ";
            }
            else //sType == "Part No"
            {
                sql = sql + " Where IPN = '" + typeValue + "' ";
            }

            sql = sql + " AND station_name = '" + station + "' "
                        + " AND CLOSE_FLAG = 'N' "
                        + " AND PKSPEC_NAME = '" + pkSpecName + "' ";

            var dt = await Context.Ado.GetDataTableAsync(sql);
            return dt.Rows[0][0].ToString();
        }

        public async Task<string> GetUnfinishBox(string type, string typeValue, string station, string pkSpecName)
        {
            var sql = "SELECT BOX_NO FROM SAJET.p_PACK_BOX ";
            if (type == "Work Order")
            {
                sql = sql + " Where WORK_ORDER = '" + typeValue + "' ";
            }
            else //sType == "Part No"
            {
                sql = sql + " Where IPN = '" + typeValue + "' ";
            }

            sql = sql + " AND station_name = '" + station + "' "
                        + " AND CLOSE_FLAG = 'N' "
                        + " AND PKSPEC_NAME = '" + pkSpecName + "' ";

            var dt = await Context.Ado.GetDataTableAsync(sql);
            return dt.Rows[0][0].ToString();
        }

        public async Task<(int, int)> GetPackQty(string type, string packNo, string wo = "")
        {
            if (type == "Pallet")
            {
                var sql = @"SELECT  COUNT(DISTINCT A.CARTON_NO) COUNT FROM SAJET.p_SN_STATUS A ,imes.p_PACK_CARTON B  WHERE A.PALLET_NO <>'N/A' AND A.PALLET_NO =@PALLET_NO AND A.CARTON_NO = B.CARTON_NO  and B.CLOSE_FLAG='Y'";

                var dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> { new SugarParameter("@PALLET_NO", packNo) });

                var packQty = int.Parse(dt.Rows[0][0].ToString());

                sql = @"SELECT count(SERIAL_NUMBER) num　FROM SAJET.p_SN_STATUS T WHERE T.PALLET_NO= @PALLET_NO AND T.PALLET_NO<>'N/A'";

                dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> { new SugarParameter("@PALLET_NO", packNo) });

                var snQty = int.Parse(dt.Rows[0][0].ToString());

                return (packQty, snQty);
            }
            else if (type == "Carton")
            {
                var sql = string.IsNullOrEmpty(wo) ? @"SELECT COUNT(SERIAL_NUMBER)  FROM SAJET.p_SN_STATUS  WHERE CARTON_NO<>'N/A' AND CARTON_NO = @CARTON_NO"
                : @"SELECT count( distinct A.BOX_NO) qty   FROM SAJET.p_SN_STATUS A ,imes.p_PACK_BOX B  WHERE a.CARTON_NO<>'N/A' AND a.CARTON_NO=@CARTON_NO AND A.BOX_NO = B.BOX_NO  AND B.CLOSE_FLAG = 'Y' AND A.WORK_ORDER=@WORK_ORDER";

                var qtyParam = new List<SugarParameter> { new SugarParameter("@CARTON_NO", packNo) };
                if (!string.IsNullOrEmpty(wo))
                {
                    qtyParam.Add(new SugarParameter("@WORK_ORDER", wo));
                }

                var dt = await Context.Ado.GetDataTableAsync(sql, qtyParam);

                var packQty = int.Parse(dt.Rows[0][0].ToString());

                sql = @"SELECT COUNT(SERIAL_NUMBER) NUM FROM SAJET.p_SN_STATUS WHERE  CARTON_NO<>'N/A' AND CARTON_NO = @CARTON_NO";

                dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> { new SugarParameter("@CARTON_NO", packNo) });

                var snQty = int.Parse(dt.Rows[0][0].ToString());

                return (packQty, snQty);
            }
            else if (type == "Box")
            {
                var sql = @"SELECT  COUNT(SERIAL_NUMBER) COUNT FROM SAJET.p_SN_STATUS WHERE BOX_NO = @box ";

                var dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> { new SugarParameter("@box", packNo) });

                var packQty = int.Parse(dt.Rows[0][0].ToString());

                return (packQty, 0);
            }
            else return (0, 0);
        }

        async Task<DataTable> GetWoParam(string wo, string labelType)
        {
            string sql = @"SELECT C.*
                FROM SAJET.P_WO_BASE A, IMES.M_RULE_SET_DETAIL B, IMES.M_RULE_PARAM C
                WHERE A.RULE_SET_NAME = B.RULE_SET_NAME
                AND B.RULE_NAME = C.RULE_NAME
                AND A.WORK_ORDER = @WO
                AND B.RULE_TYPE = @label_Type";

            var dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> {
                new SugarParameter("@WO", wo),
                new SugarParameter("@label_Type", labelType.ToUpper()),
            });

            return dt;
        }

        async Task<DataTable> GetWoData(string wo)
        {
            string sql = @"SELECT A.START_STATION_TYPE, A.END_STATION_TYPE, A.*, B.*
            FROM SAJET.P_WO_BASE A
            LEFT JOIN IMES.M_PART_SEMI_FG B
            ON A.IPN = B.IPN
            WHERE A.WORK_ORDER = @WO";

            var dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> {
                new SugarParameter("@WO", wo),
            });

            return dt;
        }

        async Task<DataTable> GetSysDual(string text1, string text2)
        {
            string sql = "  SELECT @1, (@2) FUNDATA FROM DUAL ";

            var dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> {
                new SugarParameter("@1", text1),
                new SugarParameter("@2", text2),
            });

            return dt;
        }

        async Task<DataTable> GetSequenceLastNum(string sequencesName)
        {
            string sql = @"SELECT LAST_NUMBER
                FROM ALL_SEQUENCES
                WHERE SEQUENCE_NAME =@SeqName
                AND SEQUENCE_OWNER = 'IMES' ";

            var dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> {
                new SugarParameter("@SeqName", sequencesName),
            });

            return dt;
        }

        async Task<DataTable> GetUserObject(string seq)
        {
            string sql = " SELECT * FROM ALL_OBJECTS WHERE OBJECT_TYPE = 'SEQUENCE'  AND OBJECT_NAME = '" + seq + "' AND  OWNER='IMES' ";

            var dt = await Context.Ado.GetDataTableAsync(sql);

            return dt;
        }

        class RecurValue<T>
        {
            readonly Lazy<List<RecurValue<T>>> _subItems = new();
            public List<RecurValue<T>> SubItems => _subItems.Value;
            public T Value { get; set; }

            RecurValue(T value) => this.Value = value;

            public static implicit operator RecurValue<T>(T value) => new(value);
        }

        public async Task<(bool, string[], object[])> Get_RuleData(string sLabelType, string sWO)
        {
            string[] sParam = null;
            object[] objRuleData = null;

            string text = "";
            string text2 = "";
            string text3 = "";
            string text4 = "";
            string text5 = "";
            string text6 = "N";
            var listView = new List<RecurValue<string>>();
            var listView2 = new List<RecurValue<string>>();
            var listView3 = new List<RecurValue<string>>();
            var dtTemp = await GetWoParam(sWO, sLabelType);
            if (dtTemp.Rows.Count == 0)
            {
                if (!(sLabelType.ToUpper() == "CSN"))
                {
                    return (false, sParam, objRuleData);
                }

                dtTemp = await GetWoParam(sWO, sLabelType);
                if (dtTemp.Rows.Count == 0)
                {
                    return (false, sParam, objRuleData);
                }

                sLabelType = "Customer SN";
            }

            for (int i = 0; i <= dtTemp.Rows.Count - 1; i++)
            {
                string text7 = dtTemp.Rows[i]["PARAME_NAME"].ToString();
                string text8 = dtTemp.Rows[i]["PARAME_ITEM"].ToString();
                string text9 = dtTemp.Rows[i]["PARAME_VALUE"].ToString();
                if (text7 == sLabelType.ToUpper() + " Code")
                {
                    switch (text8)
                    {
                        case "Code":
                            text = text9;
                            break;
                        case "Default":
                            text2 = text9;
                            break;
                        case "Code Type":
                            text3 = text9;
                            break;
                    }

                    continue;
                }

                string text10 = text7;
                if (text10 != null)
                {
                    switch (text10)
                    {
                        case "Month User Define":
                            listView.Add("m");
                            listView.Last().SubItems.Add(text9);
                            continue;
                        case "Day User Define":
                            listView.Add("d");
                            listView.Last().SubItems.Add(text9);
                            continue;
                        case "Week User Define":
                            listView.Add("w");
                            listView.Last().SubItems.Add(text9);
                            continue;
                        case "Day of Week User Define":
                            listView.Add("k");
                            listView.Last().SubItems.Add(text9);
                            continue;
                        case "Check Sum":
                            text4 = text9;
                            continue;
                        case "Reset Sequence":
                            text6 = ((!(text8 == "1")) ? "N" : "Y");
                            text5 = text9;
                            continue;
                    }
                }

                if (text7.IndexOf("Digit Type & Field") != -1)
                {
                    listView3.Add(text7.Substring(0, 1));
                    listView3.Last().SubItems.Add(text8);
                    listView3.Last().SubItems.Add(text9);
                    dtTemp = await GetWoData(sWO);
                    string text11 = "N/A";
                    if (text8 != "N/A")
                    {
                        text11 = dtTemp.Rows[0][text8].ToString();
                    }

                    dtTemp = await GetSysDual(text9, text11);
                    string text12 = dtTemp.Rows[0]["fundata"].ToString();
                    listView3.Last().SubItems.Add(text12);
                }
                else if (text7 == sLabelType.ToUpper() + " User Define")
                {
                    listView2.Add(text8);
                    listView2.Last().SubItems.Add(text9);
                }
            }

            Array.Clear(objRuleData, 0, objRuleData.Length);
            Array.Resize(ref objRuleData, 9);
            Array.Clear(sParam, 0, sParam.Length);
            Array.Resize(ref sParam, 9);
            sParam[0] = "Code";
            objRuleData[0] = text;
            sParam[1] = "Default";
            objRuleData[1] = text2;
            sParam[2] = "Code Type";
            objRuleData[2] = text3;
            sParam[3] = "User DayCode";
            objRuleData[3] = listView;
            sParam[4] = "User Seq";
            objRuleData[4] = listView2;
            sParam[5] = "User Function";
            objRuleData[5] = listView3;
            sParam[6] = "Check Sum";
            objRuleData[6] = text4;
            sParam[7] = "Reset Sequence";
            objRuleData[7] = text6;
            sParam[8] = "Reset Cycle";
            objRuleData[8] = text5;
            return (true, sParam, objRuleData);
        }

        public async Task<string> Reset_Sequence(string sSeqName, string sCode, string sResetCycle, string[] sDayValue, string sMark, bool b_SeqExist)
        {
            string text = sDayValue[0];
            string text2 = sDayValue[1];
            string text3 = sDayValue[2];
            string text4 = sDayValue[3];
            string text5 = sDayValue[4];
            string text6 = sDayValue[5];
            string text7 = sDayValue[6];
            string text8 = sDayValue[7];
            bool flag = false;
            if (sResetCycle != null)
            {
                switch (sResetCycle)
                {
                    case "3":
                        if (sMark == "" || sMark != text5)
                        {
                            sMark = text5;
                            flag = true;
                        }

                        break;
                    case "2":
                        if (sCode.ToUpper().IndexOf("M") > -1)
                        {
                            if (sMark == "" || sMark != text2)
                            {
                                sMark = text2;
                                flag = true;
                            }
                        }
                        else if (sMark == "" || sMark != text8)
                        {
                            sMark = text8;
                            flag = true;
                        }

                        break;
                    case "1":
                        if (sCode.ToUpper().IndexOf("W") > -1)
                        {
                            if (sMark == "" || sMark != text4)
                            {
                                sMark = text4;
                                flag = true;
                            }
                        }
                        else if (sMark == "" || sMark != text7)
                        {
                            sMark = text7;
                            flag = true;
                        }

                        break;
                    case "0":
                        if (sCode.ToUpper().IndexOf("D") > -1)
                        {
                            if (sMark == "" || sMark != text)
                            {
                                sMark = text;
                                flag = true;
                            }
                        }
                        else if (sCode.ToUpper().IndexOf("K") > -1)
                        {
                            if (sMark == "" || sMark != text3)
                            {
                                sMark = text3;
                                flag = true;
                            }
                        }
                        else if (sMark == "" || sMark != text6)
                        {
                            sMark = text6;
                            flag = true;
                        }

                        break;
                }
            }

            if (b_SeqExist && flag)
            {
                string sQL = "Drop Sequence " + sSeqName;
                await Context.Ado.ExecuteCommandAsync(sQL);
            }

            return sMark;
        }

        static string SeqTran(int iSeq, string sRuleCode, string sSeqText, string g_sCarry, List<RecurValue<string>> LVUserSeq)
        {
            const string g_sCarry16 = "0123456789ABCDEF";

            string text = "";
            string text2 = "";
            for (int i = 0; i <= sRuleCode.Length - 1; i++)
            {
                if (sSeqText.IndexOf(sRuleCode[i]) > -1)
                {
                    text += sRuleCode[i];
                }
            }

            int num = iSeq;
            for (int num2 = text.Length - 1; num2 >= 0; num2--)
            {
                if (text[num2].ToString() == "S")
                {
                    sRuleCode = ((!(g_sCarry == "16")) ? g_sCarry16.Substring(0, 10) : g_sCarry16);
                }
                else
                {
                    for (int j = 0; j <= LVUserSeq.Count - 1; j++)
                    {
                        if (text[num2].ToString() == LVUserSeq[j].Value)
                        {
                            sRuleCode = LVUserSeq[j].SubItems[1].Value;
                            break;
                        }
                    }
                }

                if (num != 0)
                {
                    int num3 = num % sRuleCode.Length;
                    num /= sRuleCode.Length;
                    text2 = ((num3 != 0) ? (sRuleCode.Substring(num3, 1) + text2) : (sRuleCode.Substring(0, 1) + text2));
                }
                else
                {
                    text2 = ((!(text[num2].ToString() == "S")) ? (sRuleCode.Substring(0, 1) + text2) : ("0" + text2));
                }
            }

            if (num > 0)
            {
                return "-1";
            }

            return text2;
        }

        int SeqCode(string sStart, string sRuleCode, string sSeqText, string g_sCarry, List<RecurValue<string>> LVUserSeq)
        {
            const string g_sCarry16 = "0123456789ABCDEF";

            int num = 0;
            string text = "";
            for (int i = 0; i <= sRuleCode.Length - 1; i++)
            {
                if (sSeqText.IndexOf(sRuleCode[i]) > -1)
                {
                    text += sRuleCode[i];
                }
            }

            for (int j = 0; j <= sStart.Length - 1; j++)
            {
                if (text[j].ToString() == "S")
                {
                    sRuleCode = ((!(g_sCarry == "16")) ? g_sCarry16.Substring(0, 10) : g_sCarry16);
                }
                else
                {
                    for (int k = 0; k <= LVUserSeq.Count - 1; k++)
                    {
                        if (text[j].ToString() == LVUserSeq[k].Value)
                        {
                            sRuleCode = LVUserSeq[k].SubItems[1].Value;
                            break;
                        }
                    }
                }

                int length = sRuleCode.Length;
                num = num * length + sRuleCode.IndexOf(sStart[j]);
            }

            return num;
        }

        async Task<bool> Create_Rule_Seq(string sSeqName, string sStartSeq, string sRuleCode, string sSeqText, string g_sCarry, List<RecurValue<string>> LVUserSeq)
        {
            string text = "0";
            string text2 = "";
            var dtTemp = await GetUserObject(sSeqName);

            if (dtTemp.Rows.Count == 0)
            {
                int num = SeqCode(sStartSeq, sRuleCode, sSeqText, g_sCarry, LVUserSeq);
                string text3 = Convert.ToString(num);
                string seq_MaxValue = GetSeq_MaxValue(sRuleCode, g_sCarry, LVUserSeq);
                text2 = string.Concat("CREATE SEQUENCE ", "SAJET." + sSeqName, " INCREMENT BY 1 START WITH ", text3, " minvalue ", text, " MAXVALUE ", seq_MaxValue, " CYCLE NOCACHE ORDER ");
                await Context.Ado.ExecuteCommandAsync(text2);
                text2 = "GRANT SELECT ON " + sSeqName + " TO SYS_USER";
                await Context.Ado.ExecuteCommandAsync(text2);
            }

            return true;
        }

        string GetSeq_MaxValue(string sRuleCode, string g_sCarry, List<RecurValue<string>> LVUserSeq)
        {
            int num = 1;
            for (int i = 0; i <= sRuleCode.Length - 1; i++)
            {
                if (sRuleCode[i].ToString() == "S")
                {
                    num = ((!(g_sCarry == "16")) ? (num * 10) : (num * 16));
                    continue;
                }

                for (int j = 0; j <= LVUserSeq.Count - 1; j++)
                {
                    if (sRuleCode[i].ToString() == LVUserSeq[j].Value)
                    {
                        num *= LVUserSeq[j].SubItems[1].Value.Length;
                        break;
                    }
                }
            }

            return Convert.ToString(num - 1);
        }

        public async Task<(bool, string, string)> Create_NewNo(string sSeqName, string sResetMark, string[] sParam, object[] objData)
        {
            string sNewNo = null;
            var listView = new List<RecurValue<string>>();
            var listView2 = new List<RecurValue<string>>();
            var listView3 = new List<RecurValue<string>>();
            string text = objData[Array.IndexOf(sParam, "Default")].ToString();
            string text2 = objData[Array.IndexOf(sParam, "Code")].ToString();
            string g_sCarry = objData[Array.IndexOf(sParam, "Code Type")].ToString();
            listView = (List<RecurValue<string>>)objData[Array.IndexOf(sParam, "User DayCode")];
            listView2 = (List<RecurValue<string>>)objData[Array.IndexOf(sParam, "User Seq")];
            listView3 = (List<RecurValue<string>>)objData[Array.IndexOf(sParam, "User Function")];
            string text3 = objData[Array.IndexOf(sParam, "Check Sum")].ToString();
            string text4 = objData[Array.IndexOf(sParam, "Reset Sequence")].ToString();
            string sResetCycle = objData[Array.IndexOf(sParam, "Reset Cycle")].ToString();
            string[] array = new string[1] { "" };
            string[] array2 = new string[1] { "" };
            string[] array3 = new string[1] { "" };
            string[] array4 = new string[1] { "" };
            for (int i = 0; i <= listView.Count - 1; i++)
            {
                string text5 = listView[i].SubItems[1].Value;
                switch (listView[i].Value)
                {
                    case "m":
                        array = text5.Split(',');
                        break;
                    case "d":
                        array2 = text5.Split(',');
                        break;
                    case "w":
                        array3 = text5.Split(',');
                        break;
                    case "k":
                        array4 = text5.Split(',');
                        break;
                }
            }

            var sql = " Select TO_CHAR(SYSDATE,'YYYY/MM/DD/IW/DDD/D') YMD, sysdate From DUAL ";
            if (text2.ToUpper().IndexOf("W") != -1 && text2.ToUpper().IndexOf("M") == -1 && text2.ToUpper().IndexOf("D") == -1)
            {
                sql = " Select TO_CHAR(SYSDATE,'IYYY/MM/DD/IW/DDD/D') YMD, sysdate From DUAL ";
            }

            var dtTemp = await Context.Ado.GetDataTableAsync(sql);
            string text6 = dtTemp.Rows[0]["YMD"].ToString();
            string text7 = dtTemp.Rows[0]["sysdate"].ToString();
            string text8 = text6.Substring(0, 4);
            string text9 = text6.Substring(5, 2);
            string text10 = text6.Substring(8, 2);
            string text11 = text6.Substring(11, 2);
            string text12 = text6.Substring(14, 3);
            string text13 = text6.Substring(18, 1);
            string text14 = "";
            if (array[0].ToString() != "" && array.Length >= Convert.ToInt32(text9))
            {
                text14 = array[Convert.ToInt32(text9) - 1].ToString();
            }

            string text15 = "";
            if (array2[0].ToString() != "" && array2.Length >= Convert.ToInt32(text10))
            {
                text15 = array2[Convert.ToInt32(text10) - 1].ToString();
            }

            string text16 = "";
            if (array3[0].ToString() != "" && array3.Length >= Convert.ToInt32(text11))
            {
                text16 = array3[Convert.ToInt32(text11) - 1].ToString();
            }

            string text17 = "";
            if (array4[0].ToString() != "" && array4.Length >= Convert.ToInt32(text13))
            {
                text17 = array4[Convert.ToInt32(text13) - 1].ToString();
            }

            string[] sDayValue = new string[8] { text10, text9, text13, text11, text8, text15, text16, text14 };
            string text18 = "";
            for (int num = text2.Length - 1; num >= 0; num--)
            {
                string value = "0";
                if (listView3.Count > 0)
                {
                    var item = listView3.Find(x => x.Value == text2[num].ToString());
                    int num2 = listView3.IndexOf(item);
                    if (num2 != -1)
                    {
                        string text19 = listView3[num2].SubItems[3].Value;
                        if (text19.Length > 0)
                        {
                            value = text19[text19.Length - 1].ToString();
                            try
                            {
                                Convert.ToInt32(value);
                                text18 = "9" + text18;
                            }
                            catch
                            {
                                text18 = "L" + text18;
                            }

                            text19 = text19.Substring(0, text19.Length - 1);
                            listView3[num2].SubItems[3].Value = text19;
                        }
                        else
                        {
                            value = "0";
                            text18 = "9" + text18;
                        }

                        text = text.Remove(num, 1);
                        text = text.Insert(num, value);
                        continue;
                    }
                }

                switch (text2[num].ToString())
                {
                    case "Y":
                        text18 = "0" + text18;
                        if (text8.Length > 0)
                        {
                            value = text8[text8.Length - 1].ToString();
                            text8 = text8.Substring(0, text8.Length - 1);
                        }

                        break;
                    case "M":
                        text18 = "0" + text18;
                        if (text9.Length > 0)
                        {
                            value = text9[text9.Length - 1].ToString();
                            text9 = text9.Substring(0, text9.Length - 1);
                        }

                        break;
                    case "D":
                        text18 = "0" + text18;
                        if (text10.Length > 0)
                        {
                            value = text10[text10.Length - 1].ToString();
                            text10 = text10.Substring(0, text10.Length - 1);
                        }

                        break;
                    case "W":
                        text18 = "0" + text18;
                        if (text11.Length > 0)
                        {
                            value = text11[text11.Length - 1].ToString();
                            text11 = text11.Substring(0, text10.Length - 1);
                        }

                        break;
                    case "F":
                        text18 = "0" + text18;
                        if (text12.Length > 0)
                        {
                            value = text12[text12.Length - 1].ToString();
                            text12 = text12.Substring(0, text12.Length - 1);
                        }

                        break;
                    case "K":
                        text18 = "0" + text18;
                        if (text13.Length > 0)
                        {
                            value = text13[text13.Length - 1].ToString();
                            text13 = text13.Substring(0, text13.Length - 1);
                        }

                        break;
                    case "m":
                        text18 = "A" + text18;
                        if (text14.Length > 0)
                        {
                            value = text14[text14.Length - 1].ToString();
                            text14 = text14.Substring(0, text14.Length - 1);
                        }

                        break;
                    case "d":
                        text18 = "A" + text18;
                        if (text15.Length > 0)
                        {
                            value = text15[text15.Length - 1].ToString();
                            text15 = text15.Substring(0, text15.Length - 1);
                        }

                        break;
                    case "w":
                        text18 = "A" + text18;
                        if (text16.Length > 0)
                        {
                            value = text16[text16.Length - 1].ToString();
                            text16 = text16.Substring(0, text16.Length - 1);
                        }

                        break;
                    case "k":
                        text18 = "A" + text18;
                        if (text17.Length > 0)
                        {
                            value = text17[text17.Length - 1].ToString();
                            text17 = text17.Substring(0, text17.Length - 1);
                        }

                        break;
                    case "L":
                        text18 = "L" + text18;
                        value = text[num].ToString();
                        break;
                    case "C":
                        text18 = "C" + text18;
                        value = text[num].ToString();
                        break;
                    case "9":
                        text18 = "9" + text18;
                        value = text[num].ToString();
                        break;
                    default:
                        text18 = "L" + text18;
                        value = text[num].ToString();
                        break;
                }

                text = text.Remove(num, 1);
                text = text.Insert(num, value);
            }

            string text20 = "S";
            for (int j = 0; j <= listView2.Count - 1; j++)
            {
                text20 += listView2[j].Value;
            }

            dtTemp = await GetSequenceLastNum(sSeqName.ToUpper());
            if (dtTemp.Rows.Count == 0)
            {
                sResetMark = await Reset_Sequence(sSeqName, text2, sResetCycle, sDayValue, sResetMark, b_SeqExist: false);
            }
            else if (text4 == "Y")
            {
                sResetMark = await Reset_Sequence(sSeqName, text2, sResetCycle, sDayValue, sResetMark, b_SeqExist: true);
            }

            string sStartSeq = SeqTran(1, text2, text20, g_sCarry, listView2);
            await Create_Rule_Seq(sSeqName, sStartSeq, text2, text20, g_sCarry, listView2);

            sql = "SELECT IMES." + sSeqName + ".NEXTVAL SNID FROM DUAL ";
            dtTemp = await Context.Ado.GetDataTableAsync(sql);

            int iSeq = Convert.ToInt32(dtTemp.Rows[0]["SNID"].ToString());
            sql = "alter sequence IMES." + sSeqName + " INCREMENT BY 1 ";
            await Context.Ado.ExecuteCommandAsync(sql);

            string text21 = SeqTran(iSeq, text2, text20, g_sCarry, listView2);
            int num3 = 0;
            for (int k = 0; k <= text2.Length - 1; k++)
            {
                if (text20.IndexOf(text2[k]) > -1)
                {
                    text = text.Remove(k, 1);
                    text = text.Insert(k, text21[num3].ToString());
                    num3++;
                }
            }

            if (text2.IndexOf("X") != -1)
            {
                dtTemp = await GetSysDual(text3, text);
                text = dtTemp.Rows[0]["FUNDATA"].ToString();
            }

            sNewNo = text;
            return (true, sResetMark, sNewNo);
        }

        public async Task<(bool, string, string)> GetNewNo(string type, string wo, long uid)
        {
            var msg = string.Empty;
            var newNo = string.Empty;

            string sql = string.Empty;
            string sField = "";
            switch (type)
            {
                case "Pallet":
                    sField = "Pallet No";
                    sql = $@"SELECT B.RULE_NAME,B.RULE_DESC FROM SAJET.P_WO_BASE A
            INNER JOIN IMES.M_RULE_SET_DETAIL B ON A.RULE_SET_NAME = B.RULE_SET_NAME
            WHERE WORK_ORDER='" + wo + "' " +
                        "AND B.RULE_TYPE = 'PALLET NO'";
                    break;
                case "Carton":
                    sField = "CARTON NO";
                    sql = $@"SELECT B.RULE_NAME,B.RULE_DESC FROM SAJET.P_WO_BASE A
            INNER JOIN IMES.M_RULE_SET_DETAIL B ON A.RULE_SET_NAME = B.RULE_SET_NAME
            WHERE WORK_ORDER='" + wo + "' " +
                        "AND B.RULE_TYPE = 'CARTON NO'";
                    break;
                case "Box":
                    sField = "Box No";
                    sql = $@"SELECT B.RULE_NAME,B.RULE_DESC FROM SAJET.P_WO_BASE A
            INNER JOIN IMES.M_RULE_SET_DETAIL B ON A.RULE_SET_NAME = B.RULE_SET_NAME
            WHERE WORK_ORDER='" + wo + "' " +
                        "AND B.RULE_TYPE = 'BOX NO'";
                    break;
                case "CSN":
                    sField = "CUSTOMER SN";
                    sql = $@"SELECT B.RULE_NAME,B.RULE_DESC FROM SAJET.P_WO_BASE A
            INNER JOIN IMES.M_RULE_SET_DETAIL B ON A.RULE_SET_NAME = B.RULE_SET_NAME
            WHERE WORK_ORDER='" + wo + "' " +
                        "AND B.RULE_TYPE = 'CUSTOMER SN'";
                    break;
            }

            var dt = await Context.Ado.GetDataTableAsync(sql);

            if (dt.Rows.Count == 0 || string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
            {
                msg = " Rule is not found : " + type;
                return (false, msg, newNo);
            }
            else
            {
                string sRuleName = dt.Rows[0]["RULE_NAME"].ToString();
                string sSeqFix = "";
                sql = "SELECT SEQ_NAME FROM SAJET.M_label where Upper(label_name) ='" + sField.ToUpper() + "'";

                dt = await Context.Ado.GetDataTableAsync(sql);
                if (dt.Rows.Count > 0)
                    sSeqFix = dt.Rows[0]["SEQ_NAME"].ToString();

                string sSeqName = sSeqFix + sRuleName;
                object[] objData = new object[3];
                string[] sParam = new string[1];

                (var bRuleExist, sParam, objData) = await Get_RuleData(sField, wo);
                var LVFun = (List<RecurValue<string>>)objData[Array.IndexOf(sParam, "User Function")];
                for (int i = 0; i <= LVFun.Count - 1; i++)
                {
                    string sFun_Field = LVFun[i].SubItems[1].Value;
                    string sFun_Name = LVFun[i].SubItems[2].Value;
                    string sData = "N/A";
                    if (sFun_Field != "N/A")
                        sData = wo;
                    sql = " select " + sFun_Name + "('" + sData + "') fundata from dual ";
                    dt = await Context.Ado.GetDataTableAsync(sql);
                    string sValue = dt.Rows[0]["fundata"].ToString();
                    LVFun[i].SubItems.Add(sValue);
                }
                string sResetMark = "";
                sql = "Select PARAME_VALUE FROM SAJET.M_MODULE_PARAM Where UPPER(MODULE_NAME) = '" + sSeqFix.ToUpper() + "' "
                      + "and FUNCTION_NAME = '" + sRuleName + "' and PARAME_NAME = 'Reset Sequence Mark' ";
                dt = await Context.Ado.GetDataTableAsync(sql);
                if (dt.Rows.Count > 0)
                    sResetMark = dt.Rows[0]["PARAME_VALUE"].ToString();

                string sInputNo = "";
                if (bRuleExist)
                {
                    (_, sResetMark, sInputNo) = await Create_NewNo(sSeqName, sResetMark, sParam, objData);
                }

                newNo = sInputNo;
                sql = "Select rowid||'' AS ID FROM SAJET.M_MODULE_PARAM Where UPPER(MODULE_NAME) = '" + sSeqFix.ToUpper() + "' "
                     + "and FUNCTION_NAME = '" + sRuleName + "' and PARAME_NAME = 'Reset Sequence Mark' ";
                dt = await Context.Ado.GetDataTableAsync(sql);
                if (dt.Rows.Count == 0)
                {
                    sql = " Insert INTO SAJET.M_MODULE_PARAM (MODULE_NAME,FUNCTION_NAME,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,UPDATE_USERID ) "
                          + " Values ('" + sSeqFix.ToUpper() + "','" + sRuleName + "','Reset Sequence Mark','" + sRuleName + "','" + sResetMark + "','" + uid + "')";
                    await Context.Ado.ExecuteCommandAsync(sql);
                }
                else
                {
                    string sRowid = dt.Rows[0]["ID"].ToString();
                    sql = "update SAJET.M_MODULE_PARAM set parame_value = '" + sResetMark + "' where rowid = '" + sRowid + "' ";
                    await Context.Ado.ExecuteCommandAsync(sql);
                }
                return (true, msg, newNo);
            }
        }

        async Task<bool> Check_Exist(string sType, string sValue)
        {
            string sTable = "", sField = "";
            switch (sType.ToUpper())
            {
                case "PALLET":
                    sTable = "SAJET.P_PACK_PALLET";
                    sField = "PALLET_NO";
                    break;
                case "CARTON":
                    sTable = "SAJET.P_PACK_CARTON";
                    sField = "CARTON_NO";
                    break;
                case "BOX":
                    sTable = "SAJET.P_PACK_BOX";
                    sField = "BOX_NO";
                    break;
                case "INNERBOX":
                    sTable = "SAJET.P_PACK_INNERBOX";
                    sField = "INNERBOX_NO";
                    break;
                case "CSN":
                    sTable = "SAJET.P_SN_STATUS";
                    sField = "CUSTOMER_SN";
                    break;
            }
            string sql = " SELECT " + sField + " FROM " + sTable + " WHERE " + sField + " = '" + sValue + "' and rownum = 1 ";

            var dt = await Context.Ado.GetDataTableAsync(sql);

            return dt.Rows.Count == 0;
        }

        public async Task<(string, string)> Get_NextNewNo(string type, string wo, string sNewNo, long uid)
        {
            var sMsg = string.Empty;
            string sStart = "";
            string sEnd = "";

            while (!await Check_Exist(type, sNewNo))
            {
                if (sStart == "")
                    sStart = sNewNo;
                sEnd = sNewNo;

                (var ok, sMsg, sNewNo) = await GetNewNo(type, wo, uid);
                if (!ok)
                    return (sMsg, sNewNo);
            }
            if (sStart != "")
            {
                if (sStart == sEnd)
                    sMsg = type + "Duplicate" + sStart;
                else
                    sMsg = type + "Duplicate" + sStart + " ~ " + sEnd;
            }
            return (sMsg, sNewNo);
        }

        public async Task Append_PackNo(string type, string wo, string ipn, string station, string specName, string packNo, string userNo)
        {
            if (packNo == "N/A")
                return;
            string sTable = "", sField = "";
            switch (type)
            {
                case "Pallet":
                    sTable = "SAJET.P_PACK_PALLET";
                    sField = "PALLET_NO";
                    break;
                case "Carton":
                    sTable = "SAJET.P_PACK_CARTON";
                    sField = "CARTON_NO";
                    break;
                case "Box":
                    sTable = "SAJET.P_PACK_BOX";
                    sField = "BOX_NO";
                    break;
            }
            var sql = " SELECT " + sField + " From " + sTable + " Where " + sField + " = '" + packNo + "' and rownum=1";
            DataTable dataTable = await Context.Ado.GetDataTableAsync(sql);
            if (dataTable.Rows.Count == 0)
            {
                sql = " INSERT INTO " + sTable + " (" + sField + ",WORK_ORDER,IPN,CLOSE_FLAG,STATION_NAME,CREATE_EMPNO,PKSPEC_NAME) "
                     + " VALUES " + "('" + packNo + "','" + wo + "','" + ipn + "','N','" + station + "','" + userNo + "','" + specName + "')";
                await Context.Ado.ExecuteCommandAsync(sql);
            }
        }

        public async Task<(bool, string, string)> CreateNewPack(string type, string wo, string ipn, string specName, string station, long uid, string userNo)
        {
            var (ok, msg, newNo) = await GetNewNo(type, wo, uid);
            if (!ok)
                return (false, msg, null);
            (msg, newNo) = await Get_NextNewNo(type, wo, newNo, uid);
            await Append_PackNo(type, wo, ipn, station, specName, newNo, userNo);

            return (true, msg, newNo);
        }

        public async Task<string> GetSn(string input)
        {
            var sn = await Context.Queryable<SnStatus>()
                .Where(x => x.SerialNumber == input)
                .Select(x => x.SerialNumber)
                .SingleAsync();

            if (sn is null)
            {
                sn = await Context.Queryable<SnStatus>()
                    .Where(x => x.CustomerSn == input && x.CustomerSn != "N/A")
                    .Select(x => x.SerialNumber)
                    .SingleAsync();
            }

            return sn;
        }

        public async Task<bool> CheckDefectcode(string input)
        {
            return await Context.Queryable<MDefect>()
                .Where(x => x.DefectCode == input && x.Enabled == "Y")
                .AnyAsync();
        }

        public async Task<(string, string, string)> GetOldPackNoBySn(string sn)
        {
            var r = await Context.Queryable<SnStatus>()
                .Where(x => x.SerialNumber == sn)
                .Select(x => new
                {
                    x.BoxNo,
                    x.CartonNo,
                    x.PalletNo,
                })
                .SingleAsync();

            return (r.BoxNo, r.CartonNo, r.PalletNo);
        }

        public async Task<(bool, string)> CheckSnBefore(string station, string wo, string sn, string empNo)
        {
            var oData = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_CHKBEFORE",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_EMP", empNo),
                    oData, tRes
                });

            var msg = tRes.Value.ToString();
            return (msg == "OK", msg);
        }

        public async Task<(bool, string)> CheckRoute(string station, string sn)
        {
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_CHECK_ROUTE",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_SN", sn),
                    tRes
                });

            var msg = tRes.Value.ToString();
            return (msg == "OK", msg);
        }

        public async Task<(bool, string)> CheckStationSN(string station, string wo, string sn, string empNo)
        {
            var oData = new SugarParameter("O_MSG", null, true);
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_STATION_CHKSN",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_EMP", empNo),
                    oData, tRes
                });

            var msg = tRes.Value.ToString();
            var ok = msg == "OK";
            return (ok, ok ? oData.Value.ToString() : msg);
        }

        public async Task<(bool, string)> CheckMix(string station, string wo, string sn,
            string box, string carton, string pallet, string empNo)
        {
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_CANTAINER_MIX_SN_CHECK",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_WO", wo),
                    new SugarParameter("T_SN", sn),
                    new SugarParameter("T_BOX_NO", box),
                    new SugarParameter("T_CARTON_NO", carton),
                    new SugarParameter("T_PALLET_NO", pallet),
                    new SugarParameter("T_EMPNO", empNo),
                    tRes
                });

            var msg = tRes.Value.ToString();
            return (msg == "OK", msg);
        }

        public async Task<(bool, string)> PackingRepackGo(string station, string pkAction, string packNo, string empNo)
        {
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_PACKING_REPACK_GO",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("T_EMPNO", empNo),
                    new SugarParameter("T_PACK_ACTION", pkAction),
                    new SugarParameter("T_PACK_VALUE", packNo),
                    tRes
                });

            var msg = tRes.Value.ToString();
            return (msg == "OK", msg);
        }

        public async Task<bool> PackIsClosed(string type, string packNo)
        {
            var sql = "";
            switch (type)
            {
                case "Box": sql = "SELECT CLOSE_FLAG FROM SAJET.p_PACK_Box WHERE  @1<>'N/A' AND BOX_NO = @1"; break;
                case "Carton": sql = "SELECT CLOSE_FLAG FROM SAJET.p_PACK_CARTON   WHERE @1<>'N/A' AND CARTON_NO =@1"; break;
                case "Pallet": sql = "SELECT CLOSE_FLAG FROM SAJET.p_PACK_PALLET WHERE  @1<>'N/A' AND PALLET_NO = @1"; break;
            }

            var dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter>
            {
                new ("@1", packNo)
            });

            if (dt.Rows.Count > 0)
            {
                var closeFlag = dt.Rows[0]["CLOSE_FLAG"].ToString();
                return closeFlag == "Y";
            }
            return false;
        }

        public async Task<(bool, string)> CheckPackInfoByCarton(string pallet, string carton)
        {
            var sql = @"SELECT BOX_NO,CARTON_NO,PALLET_NO
                 FROM SAJET.p_SN_STATUS 
                 Where CARTON_NO = @CARTON_NO and rownum = 1 ";

            var dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter>
            {
                new ("@CARTON_NO", carton)
            });

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["PALLET_NO"].ToString() != "N/A")
                {
                    if (pallet != dt.Rows[0]["PALLET_NO"].ToString())
                    {
                        var msg = "Carton in other Pallet" + " (" + dt.Rows[0]["PALLET_NO"].ToString() + ")";
                        return (false, msg);
                    }
                }
            }

            return (true, null);
        }

        public async Task<(bool, string)> CheckPackInfoByBox(string box, string carton)
        {
            var sql = @"SELECT BOX_NO,CARTON_NO,PALLET_NO
                 FROM SAJET.p_SN_STATUS 
                 Where BOX_NO = @BOX_NO and rownum = 1 ";

            var dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter>
            {
                new ("@BOX_NO", box)
            });

            if (dt.Rows.Count > 0)
            {
                if (carton != dt.Rows[0]["CARTON_NO"].ToString())
                {
                    var msg = "Box in other Carton" + " (" + dt.Rows[0]["CARTON_NO"].ToString() + ")";
                    return (false, msg);
                }
            }

            return (true, null);
        }

        public async Task<(bool, string)> PackingGo(string station, string pkAction, string sn, string csn, string box, string carton, string pallet, string empNo)
        {
            var tRes = new SugarParameter("TRES", null, true);

            await Context.Ado.UseStoredProcedure().ExecuteCommandAsync("SAJET.SP_MES_PACKING_GO_NEW",
                new SugarParameter[]
                {
                    new SugarParameter("T_STATIONNAME", station),
                    new SugarParameter("TSN", sn),
                    new SugarParameter("TEMP", empNo),
                    new SugarParameter("PACKACTION", pkAction),
                    new SugarParameter("PALLETNO", pallet),
                    new SugarParameter("CARTONNO", carton),
                    new SugarParameter("BOXNO", box),
                    new SugarParameter("CUSTOMERSN", csn),
                    tRes
                });

            var msg = tRes.Value.ToString();
            return (msg == "OK", msg);
        }

        string GetCode128B(string barcode)
        {
            int num = 104;
            for (int i = 0; i < barcode.Length; i++)
            {
                num = ((barcode[i] < ' ') ? (num + (barcode[i] + 64) * (i + 1)) : (num + (barcode[i] - 32) * (i + 1)));
            }

            num %= 103;
            num = ((num >= 95) ? (num + 100) : (num + 32));
            return Convert.ToChar(204) + barcode.ToString() + Convert.ToChar(num) + Convert.ToChar(206);
        }

        public async Task<(string, Dictionary<string, string>)> GetLabelVarsPrintData(string inputData, string labelType)
        {
            var dictionary = new Dictionary<string, string>();
            string sql = "SELECT  DATA_TYPE,DATA_ORDER,DATA_SQL,INPUT_PARAM,INPUT_FIELD FROM SAJET.M_PRINT_DATA   WHERE DATA_TYPE = @DATATYPE AND ENABLED='Y' ORDER BY DATA_ORDER";

            var dataTable = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> {
                new("@DATATYPE", labelType),
            });

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var oldValue = row["INPUT_PARAM"].ToString();
                    var sql2 = row["DATA_SQL"].ToString().Replace(oldValue, " '" + inputData + "' ");

                    var dataTable2 = await Context.Ado.GetDataTableAsync(sql2);

                    if (dataTable2.Rows.Count > 0)
                    {
                        foreach (DataColumn dc2 in dataTable2.Columns)
                        {
                            if (dictionary.Keys.Any(q => q.ToUpper() == dc2.ColumnName.ToUpper()))
                            {
                                dictionary[dc2.ColumnName] = dataTable2.Rows[0][dc2.ColumnName].ToString();
                            }
                            else
                            {
                                dictionary.Add(dc2.ColumnName, dataTable2.Rows[0][dc2.ColumnName].ToString());
                            }
                        }

                        continue;
                    }

                    foreach (DataColumn dc in dataTable2.Columns)
                    {
                        if (!dictionary.Keys.Any(q => q.ToUpper() == dc.ColumnName.ToUpper()))
                        {
                            dictionary.Add(dc.ColumnName, "");
                        }
                    }
                }

                sql = "SELECT VAR_NAME,FIELD_NAME,VAR_TYPE FROM SAJET.M_STATIONTYPE_LABEL_PARAMS WHERE LABEL_TYPE=@LABEL_TYPE ";

                dataTable = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> {
                    new("@LABEL_TYPE", labelType),
                });

                if (dataTable.Rows.Count > 0)
                {
                    var dictionary2 = new Dictionary<string, string>();
                    foreach (DataRow rowitem in dataTable.Rows)
                    {
                        if (dictionary.Any((KeyValuePair<string, string> q) => q.Key == rowitem["FIELD_NAME"].ToString()))
                        {
                            string text = dictionary.First((KeyValuePair<string, string> q) => q.Key == rowitem["FIELD_NAME"].ToString()).Value;
                            if (rowitem["VAR_TYPE"].ToString().ToUpper() == "CODE128")
                            {
                                text = GetCode128B(text);
                            }

                            dictionary2.Add(rowitem["VAR_NAME"].ToString(), text);
                        }
                        else
                        {
                            dictionary2.Add(rowitem["VAR_NAME"].ToString(), "");
                        }
                    }

                    return (null, dictionary2);
                }
                return (null, dictionary);
            }

            return ("Not found DataType:" + labelType + " M_STATIONTYPE_LABEL_PARAMS", null);
        }

        public async Task<bool> CheckPackNo(string packNo, string type)
        {
            var exp = Expressionable.Create<SnStatus>()
                .AndIF(type == "Carton", x => x.CartonNo == packNo)
                .AndIF(type == "Pallet", x => x.PalletNo == packNo)
                .AndIF(type == "Box", x => x.BoxNo == packNo);

            return await Context.Queryable<SnStatus>()
                .Where(exp.ToExpression())
                .AnyAsync();
        }

        public async Task<bool> CheckPackNoByPack(string packNo, string type)
        {
            var sql = "";

            switch(type)
            {
                case "Box": sql = "SELECT BOX_NO FROM SAJET.p_PACK_BOX WHERE BOX_NO = @1";
                    break;
                case "Carton": sql = "SELECT CARTON_NO FROM SAJET.p_PACK_CARTON WHERE CARTON_NO = @1";
                    break;
                case "Pallet":
                    sql = "SELECT PALLET_NO FROM SAJET.p_PACK_PALLET  WHERE PALLET_NO = @1";
                    break;
            }

            var dt = await Context.Ado.GetDataTableAsync(sql, new List<SugarParameter> {
                new("@1", packNo),
            });

            return dt.Rows.Count > 0;
        }

        public async Task DeletePackNoByPack(string packNo, string type)
        {
            var sql = "";

            switch (type)
            {
                case "Box":
                    sql = "DELETE FROM SAJET.p_PACK_BOX WHERE BOX_NO = @1";
                    break;
                case "Carton":
                    sql = "DELETE FROM SAJET.p_PACK_CARTON WHERE CARTON_NO = @1";
                    break;
                case "Pallet":
                    sql = "DELETE FROM SAJET.p_PACK_PALLET  WHERE PALLET_NO = @1";
                    break;
            }

            await Context.Ado.ExecuteCommandAsync(sql, new List<SugarParameter> {
                new("@1", packNo),
            });
        }

        public async Task ClosePackNoByPack(string pack, string type)
        {
            var sql = "";

            switch (type)
            {
                case "Box":
                    sql = "update SAJET.p_PACK_BOX  SET CLOSE_FLAG = 'Y' WHERE BOX_NO = @1";
                    break;
                case "Carton":
                    sql = "update SAJET.p_PACK_CARTON  SET CLOSE_FLAG = 'Y' WHERE CARTON_NO = @1";
                    break;
                case "Pallet":
                    sql = "update SAJET.p_PACK_PALLET  SET CLOSE_FLAG = 'Y' WHERE PALLET_NO = @1";
                    break;
            }
            await Context.Ado.ExecuteCommandAsync(sql, new List<SugarParameter> {
                new("@1", pack),
            });
        }

        public async Task SavePackForceClose(string packNo, string packType, string userNo)
        {
            var sql = @"INSERT INTO SAJET.p_PACK_FORCECLOSE (PACK_NO, PACK_TYPE, CREATE_EMPNO,CREATE_TIME) VALUES (@1 ,:2 ,:3 ,SYSDATE) ";
            await Context.Ado.ExecuteCommandAsync(sql, new List<SugarParameter> {
                new("@1", packNo),
                new("@2", packType),
                new("@3", userNo),
            });
        }

        async Task<string> GetSysBaseData(string paramName)
        {
            return await Context.Queryable<SBase>()
                .Where(x => x.Program.ToUpper() == "ProdManagement".ToUpper() 
                    && x.ParamName.ToUpper() == paramName.ToUpper())
                .Select(x => x.ParamValue)
                .SingleAsync();
        }

        async Task<List<string>> GetRuleFunctionList()
        {
            var sql = @" select owner || '.' || object_name object_name  from ALL_OBJECTS  where object_type = 'FUNCTION'  and substr(object_name, 1, 3) = 'PK_'";

            var dt = await Context.Ado.GetDataTableAsync(sql);
            var list = new List<string>(dt.Rows.Count);

            foreach(DataRow row in dt.Rows)
            {
                list.Add(row["object_name"].ToString());
            }

            return list;
        }

        public async Task<object> GetPkConfigData()
        {
            var pkBaseRaw = await GetSysBaseData("Packing Base");
            var pkActionRaw = await GetSysBaseData("Packing Action");
            var pkPortRaw = await GetSysBaseData("Packing Print Port");
            var ruleFunctions = await GetRuleFunctionList();

            return new
            {
                packingBases = pkBaseRaw.TrimEnd(',').Split(','),
                packingActions = pkActionRaw.TrimEnd(',').Split(','),
                printPorts = pkPortRaw.TrimEnd(',').Split(','),
                ruleFunctions,
            };
        }

        public async Task<List<ModuleParam>> GetModuleParamList(string station)
        {
            return await Context.Queryable<ModuleParam>()
                .Where(x => x.ModuleName == "PACKING" && x.Enabled == "Y"
                && x.FunctionName == "Work Station Configuration"
                && x.ParameName == station)
                .OrderBy(x => x.ParameItem)
                .ToListAsync();
        }

        async Task DeleteModuleParamInfo(string station)
        {
            var sql = @"Delete FROM SAJET.M_MODULE_PARAM 
                 Where MODULE_NAME = 'PACKING' 
                 and FUNCTION_NAME = 'Work Station Configuration'  
                 and PARAME_NAME = @1";
            await Context.Ado.ExecuteCommandAsync(sql, new List<SugarParameter>
            {
                new("@1", station),
            });
        }

        async Task SaveModuleParamItem(string station, string key, string value, string uid)
        {
            if (string.IsNullOrEmpty(value)) return;
            await Context.Insertable(new ModuleParam
            {
                ModuleName = "PACKING",
                FunctionName = "Work Station Configuration",
                ParameName = station,
                ParameItem = key,
                ParameValue = value,
                UpdateUserid = uid,
                Enabled = "Y",
                UpdateTime = DateTime.Now,
            })
            .ExecuteCommandAsync();
        }

        public async Task SavePkConfig(PkConfigSaveDto config, string uid)
        {
            var station = config.Station;
            await DeleteModuleParamInfo(station);

            var c = config.CsnConfig;
            //csn
            await SaveModuleParamItem(station, "CSN", c.CreateType, uid);
            await SaveModuleParamItem(station, "Print CSN Label", c.PrintLabel ? "Y" : "N", uid);
            await SaveModuleParamItem(station, "Print CSN Label Method", c.PrintMethod, uid);
            await SaveModuleParamItem(station, "Print CSN Label Qty", c.PrintQty.ToString(), uid);
            await SaveModuleParamItem(station, "Print CSN Label Port", c.PrintPort, uid);

            c = config.InnerBoxConfig;
            //inner
            await SaveModuleParamItem(station, "InnerBox", c.CreateType, uid);
            await SaveModuleParamItem(station, "Print InnerBox Label", c.PrintLabel ? "Y" : "N", uid);
            await SaveModuleParamItem(station, "Print InnerBox Label Method", c.PrintMethod, uid);
            await SaveModuleParamItem(station, "Print InnerBox Label Qty", c.PrintQty.ToString(), uid);
            await SaveModuleParamItem(station, "Print InnerBox Label Port", c.PrintPort, uid);

            c = config.BoxConfig;
            //box
            await SaveModuleParamItem(station, "Box", c.CreateType, uid);
            await SaveModuleParamItem(station, "Print Box Label", c.PrintLabel ? "Y" : "N", uid);
            await SaveModuleParamItem(station, "Print Box Label Method", c.PrintMethod, uid);
            await SaveModuleParamItem(station, "Print Box Label Qty", c.PrintQty.ToString(), uid);
            await SaveModuleParamItem(station, "Print Box Label Port", c.PrintPort, uid);

            c = config.CartonConfig;
            //carton
            await SaveModuleParamItem(station, "Carton", c.CreateType, uid);
            await SaveModuleParamItem(station, "Print Carton Label", c.PrintLabel ? "Y" : "N", uid);
            await SaveModuleParamItem(station, "Print Carton Label Method", c.PrintMethod, uid);
            await SaveModuleParamItem(station, "Print Carton Label Qty", c.PrintQty.ToString(), uid);
            await SaveModuleParamItem(station, "Print Carton Label Port", c.PrintPort, uid);

            c = config.PalletConfig;
            //pallet
            await SaveModuleParamItem(station, "Pallet", c.CreateType, uid);
            await SaveModuleParamItem(station, "Print Pallet Label", c.PrintLabel ? "Y" : "N", uid);
            await SaveModuleParamItem(station, "Print Pallet Label Method", c.PrintMethod, uid);
            await SaveModuleParamItem(station, "Print Pallet Label Qty", c.PrintQty.ToString(), uid);
            await SaveModuleParamItem(station, "Print Pallet Label Port", c.PrintPort, uid);

            await SaveModuleParamItem(station, "Packing Base", config.PackingBase, uid);
            await SaveModuleParamItem(station, "Packing Action", config.PackingAction, uid);
            await SaveModuleParamItem(station, "Input Error Code", config.InputDefect ? "Y" : "N", uid);

            await SaveModuleParamItem(station, "Check Rule by Function", config.RuleFunction, uid);

            await SaveModuleParamItem(station, "Caps Lock", config.CapsLock ? "Y" : "N", uid);
            await SaveModuleParamItem(station, "Remove Customer SN", config.RemoveCsn ? "Y" : "N", uid);
            await SaveModuleParamItem(station, "Weight Carton", config.WeightCarton ? "Y" : "N", uid);
            await SaveModuleParamItem(station, "Weight COM Port", config.WeightPort, uid);
        }
    }
}
