using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.Business;

namespace ZR.Service.IService
{
    public interface ILabelRuleService: IBaseService<LabelRuleName>
    {
        public Task<List<LabelRuleName>> GetAllRuleName(string status);

        public Task<List<LabelRuleName>> GetAllRuleName(string status, string filter, string param);

        public Task<List<LabelRuleName>> GetRuleNameByName(string ruleName);

        public Task<bool> SaveRuleName(LabelRuleName model);

        public Task<bool> UpdateRuleName(LabelRuleName model);

        public Task<bool> UpdateRuleStatus(string ruleName, string uid, bool status);

        public Task<bool> DeleteRuleName(string ruleName, string uid);

        public Task<long> CopyRuleInfo(string ruleName, string copyRuleName, string uid);

        public Task<long> CopyRuleParam(string ruleName, string copyRuleName, string uid);

        public Task<List<LabelRuleParam>> GetRuleParamByName(string ruleName);

        public Task<long> DeleteRuleParam(string ruleName);

        public Task<long> SaveRuleParam(LabelRuleParam model);

        public Task<List<string>> GetFunName(string sFix);

        public Task<List<LabelRuleField>> GetAllRuleField(string fieldName);

        public Task<List<Label>> GetAllRuleType(string status);

        public Task<List<Label>> GetRuleTypeByName(string typeName);

        public Task<string> CheckTables(string strTableName, string strFiledName);

        public Task<List<Label>> GetRuleTypeByType(string status, string typeName);

        public Task<long> InsertRuleType(string strLabName, string strLabDesc, string strTableName, string strFiledName, string strSeqName, string strFileName, string uid);

        public Task<bool> UpdateRuleType(string strLabName, string strLabDesc, string strTableName, string strFiledName, string strSeqName, string strFileName, string uid);

        public Task<bool> UpdateRuleTypeStatus(string strLabName, bool status, string uid);

        public Task<bool> DeleteRuleType(string typeName, string uid);

        public Task<IList> GetWoByRule(string modelName, string funName);

        public Task<bool> SaveWoParam(string wo, string funName, string uid);

        public Task<string> GetSeqName(string ruleName);

        public Task<DataTable> GetSeq(string seqName, string ruleName);
    }
}
