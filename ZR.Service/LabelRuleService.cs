using Infrastructure;
using Infrastructure.Attribute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;
using ZR.Service.IService;

namespace ZR.Service
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    internal class LabelRuleService : BaseService<LabelRuleName>, ILabelRuleService
    {
        public async Task<List<LabelRuleName>> GetAllRuleName(string status) =>
            await Queryable().WhereIF(!string.IsNullOrEmpty(status), l => l.Enabled == status).ToListAsync();

        public async Task<List<LabelRuleName>> GetAllRuleName(string status, string filter, string param) {
            var exp = Expressionable.Create<LabelRuleName>().AndIF(!string.IsNullOrEmpty(status), l => l.Enabled == status)
                .AndIF(!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(param), 
                l => SqlFunc.MappingColumn<string>(filter).Contains(param)).ToExpression();
            
            return await Queryable().Where(exp).ToListAsync();
        }

        public async Task<List<LabelRuleName>> GetRuleNameByName(string ruleName)
        {
            return await Queryable().Where(l=>l.RuleName == ruleName).Select(l => new LabelRuleName
            {
                RuleName = l.RuleName,
            }).ToListAsync();
        }

        public async Task<bool> SaveRuleName(LabelRuleName model)
        {
            return await InsertAsync(model);
        }

        public async Task<bool> UpdateRuleName(LabelRuleName model)
        {
            return await UpdateAsync(model);
        }

        public async Task<bool> UpdateRuleStatus(string ruleName, string uid, bool status)
        {
            var r = await UpdateAsync(l => new LabelRuleName
            { Enabled = status ? "Y" : "N", UpdateEmpno = uid, UpdateTime = DateTime.Now },
            l => l.RuleName == ruleName);

            if (!r) return false;

            r = await Context.Queryable<LabelRuleName>()
                .Where(l => l.RuleName == ruleName)
                .Select(l => new LabelRuleNameHis
                {
                    Id = l.Id,
                    RuleType = l.RuleType,
                    RuleName = l.RuleName,
                    RuleDesc = l.RuleDesc,
                    GroupQty = l.GroupQty,
                    SafetyStock = l.SafetyStock,
                    Enabled = l.Enabled,
                    CreateEmpno = l.CreateEmpno,
                    CreateTime = l.CreateTime,
                    UpdateEmpno = l.UpdateEmpno,
                    UpdateTime = l.UpdateTime,
                })
                .IntoTableAsync<LabelRuleNameHis>()
                > 0;

            return r;
        }

        public async Task<bool> DeleteRuleName(string ruleName, string uid)
        {
            var r = await Context.Ado.UseTranAsync(async delegate
            {
                await UpdateAsync(l => new LabelRuleName
                { Enabled = "D", UpdateEmpno = uid, UpdateTime = DateTime.Now },
                l => l.RuleName == ruleName);

                await Context.Queryable<LabelRuleName>()
                .Where(l => l.RuleName == ruleName)
                .IntoTableAsync<LabelRuleName>("M_RULE_NAME_HT");

                await Context.Updateable<LabelRuleParam>().SetColumns(rp => new LabelRuleParam
                { Enabled = "D", UpdateEmpno = uid, UpdateTime = DateTime.Now })
                .Where(rp => rp.RuleName == ruleName).ExecuteCommandAsync();

                await Context.Queryable<LabelRuleParam>()
                .Where(rp=>rp.RuleName == ruleName)
                .IntoTableAsync<LabelRuleParam>("M_RULE_PARAM_HT");

                await Deleteable().Where(l => l.RuleName == ruleName).ExecuteCommandAsync();
                await Context.Deleteable<LabelRuleParam>().Where(rp => rp.RuleName == ruleName).ExecuteCommandAsync();
            });

            return r.IsSuccess;
        }

        public async Task<long> CopyRuleInfo(string ruleName, string copyRuleName, string uid)
        {
            return await Queryable().Where(l => l.RuleName == copyRuleName).Select(l => new LabelRuleName
            {
                RuleType = l.RuleType,
                RuleName = ruleName,
                RuleDesc= l.RuleDesc,
                GroupQty = l.GroupQty,
                SafetyStock = l.SafetyStock,
                Enabled = "Y",
                CreateEmpno = uid,
                CreateTime = DateTime.Now,
                UpdateEmpno = uid,
                UpdateTime = DateTime.Now,
            }).IntoTableAsync<LabelRuleName>();
        }

        public async Task<long> CopyRuleParam(string ruleName, string copyRuleName, string uid)
        {
            return await Context.Queryable<LabelRuleParam>().Where(rp=>rp.RuleName == copyRuleName).Select(rp => new LabelRuleParam 
            { 
                RuleType = rp.RuleType,
                RuleName = ruleName,
                ParameName = rp.ParameName,
                ParameItem = rp.ParameItem,
                ParameValue = rp.ParameValue,
                CreateEmpno = uid,
                CreateTime = DateTime.Now,
                UpdateEmpno = uid,
                UpdateTime = DateTime.Now,
                Enabled="Y",
            }).IntoTableAsync<LabelRuleParam>();
        }

        public async Task<List<LabelRuleParam>> GetRuleParamByName(string ruleName)
        {
            return await Context.Queryable<LabelRuleParam>().Where(rp => rp.RuleName == ruleName).ToListAsync();
        }

        public async Task<long> DeleteRuleParam(string ruleName)
        {
            return await Context.Deleteable<LabelRuleParam>().Where(rp => rp.RuleName == ruleName).ExecuteCommandAsync();
        }

        public async Task<long> SaveRuleParam(LabelRuleParam model)
        {
            return await Context.Insertable(model).ExecuteCommandAsync();
        }

        public async Task<List<string>> GetFunName(string sFix)
        {
            string sql = @"select owner || '.' || object_name object_name from all_objects where object_type='FUNCTION'
                        and owner='MES_DEV' and substr(object_name,1,length('" + sFix + "'))='" + sFix + "' ";

            return await Context.Ado.SqlQueryAsync<string>(sql);
        }

        public async Task<List<LabelRuleField>> GetAllRuleField(string fieldName)
        {
            return await Context.Queryable<LabelRuleField>()
                .WhereIF(!string.IsNullOrWhiteSpace(fieldName), f => f.FieldName == fieldName)
                .OrderBy(f => f.FieldName).ToListAsync();
        }

        public async Task<List<Label>> GetAllRuleType(string status)
        {
            return await Context.Queryable<Label>().WhereIF(!string.IsNullOrEmpty(status), l=>l.Enabled == status)
                .ToListAsync();
        }

        public async Task<List<Label>> GetRuleTypeByName(string typeName)
        {
            return await Context.Queryable<Label>().Where(l => l.LabelName == typeName)
                .ToListAsync();
        }

        public async Task<string> CheckTables(string strTableName, string strFiledName)
        {
            var sql = string.Format("select {0} from {1}", strFiledName, strTableName);
            return await Context.Ado.SqlQuerySingleAsync<string>(sql);
        }

        public async Task<List<Label>> GetRuleTypeByType(string status, string typeName)
        {
            var whereExp = Expressionable.Create<Label>().And(l => l.LabelName.Contains(typeName))
                .AndIF(!string.IsNullOrEmpty(status), l => l.Enabled == status)
                .ToExpression();

            return await Context.Queryable<Label>().Where(whereExp).ToListAsync();
        }

        public async Task<long> InsertRuleType(string strLabName, string strLabDesc, string strTableName, string strFiledName, string strSeqName, string strFileName, string uid)
        {
            return await 
            Context.Insertable(new Label
            {
                LabelName = strLabName,
                LabelDesc = strLabDesc,
                TableName = strTableName,
                FieldName = strFiledName,
                SeqName = strSeqName,
                FileName = strFileName,
                CreateEmp = uid,
                UpdateEmp = uid,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
            }).ExecuteCommandAsync();
        }

        public async Task<bool> UpdateRuleType(string strLabName, string strLabDesc, string strTableName, string strFiledName, string strSeqName, string strFileName, string uid)
        {
            var r = await Context.Ado.UseTranAsync(async delegate
            {
                await Context.Queryable<Label>().Where(l => l.LabelName == strLabName).IntoTableAsync<LabelHis>();

                await Context.Updateable<Label>().SetColumns(_ => new Label
                {
                    LabelDesc = strLabDesc,
                    TableName = strTableName,
                    FieldName = strFiledName,
                    SeqName = strSeqName,
                    FileName = strFileName,
                    UpdateEmp = uid,
                    UpdateTime = DateTime.Now,
                }).Where(l => l.LabelName == strLabName).ExecuteCommandAsync();
            });

            return r.IsSuccess;
        }

        public async Task<bool> UpdateRuleTypeStatus(string strLabName, bool status, string uid)
        {
            var r = await Context.Ado.UseTranAsync(async delegate
            {
                await Context.Queryable<Label>().Where(l => l.LabelName == strLabName).IntoTableAsync<LabelHis>();

                await Context.Updateable<Label>().SetColumns(_ => new Label
                {
                    Enabled = status ? "Y" : "N",
                    UpdateEmp = uid,
                    UpdateTime = DateTime.Now,
                }).Where(l => l.LabelName == strLabName).ExecuteCommandAsync();
            });

            return r.IsSuccess;
        }

        public async Task<bool> DeleteRuleType(string typeName, string uid)
        {
            var r = await Context.Ado.UseTranAsync(async delegate
            {
                await Context.Updateable<LabelRuleType>().SetColumns(_ => new LabelRuleType
                {
                    Enabled = "D",
                    UpdateEmpno = uid,
                    UpdateTime = DateTime.Now,
                }).Where(t=>t.RuleType == typeName).ExecuteCommandAsync();

                await Context.Queryable<LabelRuleType>().Where(t => t.RuleType == typeName).IntoTableAsync<LabelRuleTypeHis>();

                await Context.Deleteable<LabelRuleType>().Where(t=>t.RuleType != typeName).ExecuteCommandAsync();
            });

            return r.IsSuccess;
        }

        public async Task<IList> GetWoByRule(string modelName, string funName)
        {
            //await Context.Qu
            var bQ = Context.Queryable<WoParam>().Where(p => p.ModuleName == modelName && p.FunctionName == funName).GroupBy(p => p.WorkOrder).Select(p=>p.WorkOrder);

            var list  = await bQ.MergeTable()
                .InnerJoin<WoBase>((b, a) => b == a.WorkOrder)
                .InnerJoin<MPart>((b, a, c) => a.Ipn == c.Ipn)
                .Where((b, a) => a.WoStatus != "6")
                .Select((b, a, c) => new
                {
                    a.WorkOrder,
                    c.Ipn,
                    a.TargetQty,
                    a.InputQty,
                    a.OutputQty,
                }).ToListAsync();
            return list;
        }

        public async Task<bool> SaveWoParam(string wo, string funName, string uid)
        {
            var r = await Context.Ado.UseTranAsync(async delegate
            {
                await Context.Deleteable<WoParam>().Where(w => w.WorkOrder == wo
                && SqlFunc.Subqueryable<Label>()
                .Where(l => l.LabelName.ToUpper() + " RULE" == w.ModuleName).Any()).ExecuteCommandAsync();

                await Context.Queryable<ModuleParam, LabelRuleName, LabelRuleParam, Label>((a, b, d, c) =>
                a.ParameItem == b.RuleName
                && a.ParameName == c.LabelName + " Rule"
                && b.RuleType == c.LabelName.ToUpper()
                && b.RuleName == d.RuleName)
                .Where((a, b, d, c) => a.ModuleName == "W/O RULE" && a.FunctionName == funName && c.LabelName != "U")
                .Select((a,b,d,c)=> new WoParam
                {
                    WorkOrder = wo,
                    ModuleName = b.RuleType + " RULE",
                    FunctionName = b.RuleName,
                    ParameName = d.ParameName,
                    ParameItem = d.ParameItem,
                    ParameValue = d.ParameValue,
                    UpdateEmpno = uid,
                    UpdateTime = DateTime.Now,
                }).IntoTableAsync<WoParam>();
            });

            return r.IsSuccess;
        }

        public async Task<string> GetSeqName(string ruleName)
        {
            return await Context.Queryable<Label, LabelRuleParam>((a, b) => a.LabelName == b.RuleType)
                .Where((a, b) => b.RuleName == ruleName).Select(a => a.SeqName).FirstAsync();
        }

        public async Task<DataTable> GetSeq(string seqName, string ruleName)
        {
            //string sql = "SELECT SEQUENCE_NAME,MIN_VALUE,MAX_VALUE,INCREMENT_BY,LAST_NUMBER " +
            //    "FROM ALL_SEQUENCES WHERE SEQUENCE_NAME = :1";

            return await Context.Queryable<object>().AS("ALL_SEQUENCES").Where("SEQUENCE_NAME = @q1",
                new { q1 = seqName.Trim() + ruleName.Trim() })
                .Select("SEQUENCE_NAME,MIN_VALUE,MAX_VALUE,INCREMENT_BY,LAST_NUMBER").ToDataTableAsync();
        }
    }
}