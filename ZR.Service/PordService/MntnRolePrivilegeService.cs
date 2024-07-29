using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Model;
using NLog.Filters;
using Org.BouncyCastle.Ocsp;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Dto.ProdDto;
using ZR.Repository;
using ZR.Service.IService;

namespace ZR.Service.PordService
{
    /// <summary>
    /// Service业务层处理
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    internal class MntnRolePrivilegeService : BaseService<SnStatus>, IMntnRolePrivilegeService
    {

        public PagedInfo<ImesMrole> RolePrivilegeList(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMrole>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "roleName", it => it.roleName.Contains(textData));
                exp.AndIF(optionData == "roleDesc", it => it.roleDesc.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
            }
            return Context.Queryable<ImesMrole>().Where(exp.ToExpression()).OrderBy(it => it.updateTime).ToPage(pager);
        }

        public string RolePrivilegeUpdate(ImesMrole imes)
        {
            string site = imes.site;
            int id = imes.id;
            /*string insertHt = $"insert into IMES.M_VENDOR_HT(select * from IMES.M_VENDOR  where ID = " + id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");*/
            imes.updateTime = DateTime.Now;
            int Updateable = Context.Updateable(imes).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            if (Updateable > 0)
            {
                return "修改成功！";
            }
            return "修改失败，联系资讯！";
        }

        public string RolePrivilegeInsert(ImesMrole imes)
        {
            imes.updateTime = DateTime.Now;
            imes.createTime = DateTime.Now;
            string site = imes.site;
            int count = Context.Queryable<ImesMrole>()
                .Where(it =>
                it.roleName == imes.roleName &&
                it.site == site)
                .Count();
            if (count > 0)
            {
                return imes.roleName + "已存在,新增失败!";
            }

            int MaxId = Context.Queryable<ImesMrole>().Max(it => it.id) + 1;
            imes.id = MaxId;
            int insertMaterial = Context.Insertable(imes).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            if (insertMaterial > 0)
            {
                /*string insertHt = $"insert into IMES.M_ROLE_HT(select * from IMES.M_ROLE  where ID = " + MaxId;
                if (site != null && site != "")
                {
                    insertHt = insertHt + " and site = '" + site + "'";
                }
                Context.Ado.SqlQuery<Object>(insertHt + ")");*/
                return "新增成功";
            }
            return "新增失败，检测数据是否有误！";

        }

        public List<ImesSprogramFunName> RolePrivilegeBindingList(int id, string textData, string site)
        {

            List<ImesMrolePrivilege> listImp = Context.Queryable<ImesMrolePrivilege>().Where(it => it.roleId == id).ToList();//
            List<ImesSprogramFunName> listIsfn = Context.Queryable<ImesSprogramFunName>().OrderBy(it => it.program).ToList();
            if (listImp.Count >0 && listIsfn.Count>0) { 
                for (int i = 0; i < listIsfn.Count; i++)
                {
                    listIsfn[i].status = 0;//用来做权限判断
                    for (int j = 0; j < listImp.Count; j++)
                    {
                        string gvFunName = listIsfn[i].function;
                        string dtFunName = listImp[j].fun;
                        if (gvFunName == dtFunName)
                        {
                            listIsfn[i].status = 1;
                        }
                    }

                }
            }
           return listIsfn;

        }

        public object RolePrivilegeBindingPermission(int id, string roleName, string authoritys, string updateUserNo,string idStr)
        {
            try { 
                string sql = string.Format(@"delete IMES.M_ROLE_PRIVILEGE where id ={0}", id);
                Context.Ado.SqlQuery<Object>(sql);
                if (!"".Equals(roleName)) { 
                    string insertsql = string.Format(
                        @"DECLARE  T_ID NUMBER; 
                            BEGIN FOR T IN (
                                SELECT {0} roleId, '{1}' ROLE_NAME, '{2}' AUTHORITYS, PROGRAM, FUNCTION, '{3}' CREATE_EMPNO FROM IMES.S_PROGRAM_FUN_NAME WHERE 
                                ID IN ({4}) AND ENABLED ='Y' 
                            ) LOOP  
                            SELECT 
                                CASE WHEN MAX(ID) IS NULL THEN 0 
                                ELSE MAX(ID) + 1 END INTO T_ID FROM IMES.M_ROLE_PRIVILEGE;
                                INSERT INTO IMES.M_ROLE_PRIVILEGE (ID,ROLE_ID,ROLE_NAME,AUTHORITYS,PROGRAM,FUN,CREATE_EMPNO)
                                VALUES(T_ID,T.roleId,T.ROLE_NAME,T.AUTHORITYS,T.PROGRAM,T.FUNCTION,T.CREATE_EMPNO); 
                            END LOOP; 
                            COMMIT; 
                           END;",id, roleName, authoritys, updateUserNo, idStr);
                    Context.Ado.SqlQuery<Object>(insertsql);
                    string sqlHt = @"INSERT INTO IMES.M_ROLE_PRIVILEGE_HT (SELECT * FROM IMES.M_ROLE_PRIVILEGE WHERE ROLE_ID =" + id;
                    Context.Ado.SqlQuery<Object>(sqlHt+")");//历史插入
                }
                return 1;
            }
            catch (Exception ex) {
                return 0;
            }
        }

        public object RolePrivilegeBindingPermissionHt(int id, string function)
        {
            string sqlHt = @"select * from IMES.M_ROLE_PRIVILEGE_HT where role_id=" + id+ " and FUN = '"+ function+ "' order by CREATE_TIME desc";
           return Context.Ado.SqlQuery<Object>(sqlHt);
        }

        public (List<ImesMroleReport>,List<ImesMdiyreport>) RolePrivilegeBindingReportPermission(int id)
        {
            List<ImesMdiyreport> listImpGrandpa = Context.Queryable<ImesMdiyreport>().ToList();
            List<ImesMroleReport> listIsfnt = Context.Queryable<ImesMroleReport>().Where(it=>it.roleId == id).ToList();
            for (int i = 0; i < listImpGrandpa.Count; i++)
            {
                listImpGrandpa[i].status = 0;//用来做权限判断
                for (int j = 0; j < listIsfnt.Count; j++)
                {
                    string gvFunName = listImpGrandpa[i].tkey;
                    string dtFunName = listIsfnt[j].fun;
                    if (gvFunName == dtFunName)
                    {
                        listImpGrandpa[i].status = 1;
                    }
                }

            }
            return (listIsfnt, listImpGrandpa);
        }

        public object RolePrivilegeBindingInsertReportPermission(int roleId, string roleName,string op, string idStr)
        {
            try {
                string sqlHtInsert = @"INSERT INTO  IMES.M_ROLE_REPORT_HT SELECT * FROM IMES.M_ROLE_REPORT WHERE ROLE_ID =" + roleId;
                Context.Ado.SqlQuery<int>(sqlHtInsert);

                Context.Deleteable<ImesMroleReport>().Where(it => it.roleId == roleId).ExecuteCommand();
                if (!"".Equals(idStr)) { 
                    string sqlInset = string.Format(@"declare  t_id number; begin for t in (select '{0}' role_id,'{1}' role_name,'{2}' authoritys,TTEXT,TKEY,'{3}' update_empno from imes.M_DIYReport where 
                                TKEY in ({4}) and TKEY not in (select fun from IMES.M_ROLE_REPORT where role_id = {5})) loop  select case when max(id) is null then 0 
                                else max(id) + 1 end into t_id from imes.M_ROLE_REPORT;insert into imes.M_ROLE_REPORT(id,role_id,role_name,authoritys,program,fun,update_empno)
                                values (t_id,t.role_id,t.role_name,t.authoritys,t.TTEXT,t.TKEY,t.update_empno); end loop; commit; end;", roleId, roleName, op, op, idStr, roleId);
                    Context.Ado.SqlQuery<int>(sqlInset);
                }
                return 1;
            }
            catch (Exception ex) {
                return 0;
            }
        }

        //---------------------------------------------------------------------------------------------------------
        public PagedInfo<ImesMemp> ImesMemplist(string enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {

            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMemp>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "empName", it => it.empName.Contains(textData));
                exp.AndIF(optionData == "empNo", it => it.empNo.Contains(textData));
                exp.AndIF(optionData == "shiftName", it => it.shiftName.Contains(textData));
                exp.AndIF(optionData == "deptName", it => it.deptName.Contains(textData));
                exp.AndIF(optionData == "remark", it => it.remark.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
                exp.AndIF(optionData == "option1", it => it.option1.ToString().Contains(textData));
            }
            return Context.Queryable<ImesMemp>().Where(exp.ToExpression()).OrderBy(it => it.createEmpno).ToPage(pager);

        }

        public int ImesMempInsert(ImesMemp imes)
        {
            string site = imes.site;
            int count = Context.Queryable<ImesMemp>()
                .Where(it => it.empNo == imes.empNo && it.site == imes.site)
                .Count();
            if (count >= 1)
            {
                throw new CustomException("该用户已存在");
            }
            imes.updateTime = DateTime.Now;
            imes.createEmpno = DateTime.Now;
            imes.id = Context.Queryable<ImesMemp>().Max(it => it.id) + 1;
            imes.passwd = passWordByEmpNo(imes.empNo);
            int insertErp = Context.Insertable(imes).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            string insertHt = $"insert into imes.m_emp_ht(select * from imes.m_emp where id= " + imes.id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            return insertErp;
        }

        public int ImesMempUpdate(ImesMemp imes)
        {
            var id = imes.id;
            string site = imes.site;
            string insertHt = $"insert into imes.m_emp_ht(select * from imes.m_emp where id= " + imes.id;
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");
            imes.updateTime = DateTime.Now;
            if (imes.status==1) {
                imes.passwd = passWordByEmpNo(imes.empNo);
            }
            
            return Context.Updateable(imes).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
        }

        public int ImesMempDelet(ImesMemp imes)
        {
            string site = imes.site;
            imes.updateTime = DateTime.Now;
            string insertHt = $"insert into imes.m_emp_ht(select * from imes.m_emp where id= " + imes.id;
            if (site != null && site != "")
            {
                insertHt = insertHt + " and site = '" + site + "'";
            }
            Context.Ado.SqlQuery<Object>(insertHt + ")");

            int det =Context.Deleteable<ImesMemp>().Where(it => it.id == imes.id && it.site == site).ExecuteCommand();
            DeleteAllEmpRoleByEmpID(imes.id.ToString(), "");
            return det;
        }

        public object ImesMemplistFactory()
        {
            return Context.Ado.SqlQuery<object>($"SELECT SITE FROM IMES.M_SITE WHERE ENABLED='Y'");
        }

        public object ImesMemplistBranch(string site)
        {
            return Context.Ado.SqlQuery<object>(string.Format(@"SELECT DEPT_NAME FROM IMES.M_DEPT WHERE SITE = '{0}'", site));
        }

        public List<ImesMrole> ImesMemplistRole(int id, string textData, string site)
        {
            List<ImesMroleEmp> listImp = Context.Queryable<ImesMroleEmp>().Where(it => it.empId == id).ToList();//
            var exp = Expressionable.Create<ImesMrole>().AndIF(textData != "" && textData != null, it => it.roleName.Contains(textData)).ToExpression();
            List<ImesMrole> listIsfn = Context.Queryable<ImesMrole>().Where(exp).OrderBy(it => it.roleName).ToList();
            if (listImp.Count > 0 && listIsfn.Count > 0)
            {
                for (int i = 0; i < listIsfn.Count; i++)
                {
                    listIsfn[i].status = 0;//用来做权限判断
                    for (int j = 0; j < listImp.Count; j++)
                    {
                        string gvRoleID = listIsfn[i].id.ToString();
                        string drRoeID = listImp[j].roleId.ToString();
                        if (gvRoleID == drRoeID)
                        {
                            listIsfn[i].status = 1;
                        }
                    }

                }
            }
            return listIsfn;
        }


        private string passWordByEmpNo(string empNo)
        {
            List<string> pwd = Context.Ado.SqlQuery<string>($"select imes.password.encrypt('" + empNo + "') enPassWord from dual");
            return pwd[0];
        }

        public object ImesMemplistHt(string empNo, string site)
        {
            return Context.Ado.SqlQuery<object>($"select * from imes.m_emp_ht where EMP_NO ='" + empNo + "' and SITE ='" + site + "'");
        }

        public object ImesMemplistRoleInsert(int empId, string empNo, string authoritys, string updateNo)
        {
            try {
                int det = Context.Deleteable<ImesMroleEmp>().Where(it => it.empId == empId).ExecuteCommand();

                string sql = string.Format(@"DECLARE  T_ID NUMBER; BEGIN FOR T IN (SELECT ID,ROLE_NAME,'{0}' EMP_ID,'{1}'EMP_NO,'{2}'UPDATE_EMPNO FROM IMES.M_ROLE WHERE 
                            ID IN ({3}) AND ID NOT IN (SELECT ID FROM IMES.M_ROLE_EMP WHERE EMP_ID = {4})) LOOP  SELECT CASE WHEN MAX(ID) IS NULL THEN 0 
                            ELSE MAX(ID) + 1 END INTO T_ID FROM IMES.M_ROLE_EMP;INSERT INTO IMES.M_ROLE_EMP (ID,ROLE_ID,ROLE_NAME,EMP_ID,EMP_NO,UPDATE_EMPNO)
                            VALUES(T_ID,T.ID,T.ROLE_NAME,T.EMP_ID,T.EMP_NO,T.UPDATE_EMPNO); END LOOP; COMMIT; END;"
                            , empId, empNo, updateNo, authoritys, empId);

                Context.Ado.SqlQuery<Object>(sql);
                return 1;
            }
            catch (Exception ex) {
                return 0;
            }
        }

        //复选全选掉接口
        public object DeleteAllEmpRoleByEmpID(string empId, string idStringB)
        {
            string sql = string.Empty;
            if (string.IsNullOrWhiteSpace(idStringB))
                sql = string.Format(@"DELETE IMES.M_ROLE_EMP WHERE EMP_ID ={0} ", empId);
            else
                sql = string.Format(@"DELETE IMES.M_ROLE_EMP WHERE EMP_ID ={0} AND ROLE_ID IN ({1})", empId, idStringB);
            return Context.Ado.SqlQuery<object>(sql);
        }

        public object ImesMemplistCopy(int empId, string empNo, string op)
        {
            /*string sql = @"select ID from IMES.M_EMP where emp_no ='" + empNo + "'";
            var id = Context.Ado.SqlQuery<int>(sql);//拿到开通员工的id*/
            List<ImesMemp> listImpIp = Context.Queryable<ImesMemp>().Where(it => it.empNo == empNo).ToList();
            int id = (int)listImpIp[0].id;
            List<ImesMroleEmp> listImp = Context.Queryable<ImesMroleEmp>().Where(it => it.empId == empId).ToList();//
            for (int i = 0; i < listImp.Count; i++) {
                var maxid = Context.Ado.SqlQuery<int>($"select max(id)+1 from IMES.M_ROLE_EMP");
                listImp[i].empId = id;
                listImp[i].updateEmpno = op;
                listImp[i].createEmpno = op;
                listImp[i].empNo = empNo;
                listImp[i].id  = Context.Queryable<ImesMroleEmp>().Max(it => it.id) + 1;
                Context.Insertable(listImp[i]).ExecuteCommand(); //都是参数化实现

            }

            return 1;
        }

    }

}
