using Infrastructure;
using Infrastructure.Attribute;
using IPTools.Core;
using System.Collections;
using System.Collections.Generic;
using ZR.Common;
using ZR.Model;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Repository;
using ZR.Service.System.IService;

namespace ZR.Service
{
    /// <summary>
    /// 系统用户
    /// </summary>
    [AppService(ServiceType = typeof(ISysUserService), ServiceLifetime = LifeTime.Transient)]
    public class SysUserService : BaseService<SysUser>, ISysUserService
    {
        private readonly ISysRoleService RoleService;
        private readonly ISysUserRoleService UserRoleService;
        private readonly ISysUserPostService UserPostService;

        public SysUserService(
            ISysRoleService sysRoleService,
            ISysUserRoleService userRoleService,
            ISysUserPostService userPostService)
        {
            RoleService = sysRoleService;
            UserRoleService = userRoleService;
            UserPostService = userPostService;
        }

        /// <summary>
        /// 根据条件分页查询用户列表
        /// </summary>
        /// <returns></returns>
        public PagedInfo<SysUser> SelectUserList(SysUserQueryDto user, PagerInfo pager)
        {
            var senable="N";
            if(user.Status==0)
            {
                senable = "Y";
            }
            else if(user.Status==1)
            {
                senable="N";
            }
            var exp = Expressionable.Create<SysUser, ZrSysUser>();
            exp.AndIF(!string.IsNullOrEmpty(user.UserName), (u, u2) => u.UserName.Contains(user.UserName));
            exp.AndIF(user.UserId > 0, (u, u2) => u.UserId == user.UserId);
            exp.AndIF(user.Status != -1, (u, u2) => u.Enabled == senable);
            exp.AndIF(user.BeginTime != DateTime.MinValue && user.BeginTime != null, (u, u2) => u.CreateTime >= user.BeginTime);
            exp.AndIF(user.EndTime != DateTime.MinValue && user.EndTime != null, (u, u2) => u.CreateTime <= user.EndTime);
            //exp.AndIF(!user.Phonenumber.IsEmpty(), u => u.Phonenumber == user.Phonenumber);
            exp.And((u, u2) => u2.MainId == null || u2.DelFlag == 0);

            if (user.DeptId != 0)
            {
                var allChildDepts = Context.Queryable<SysDept>().ToChildList(it => it.ParentId, user.DeptId);

                exp.And((u, u2) => allChildDepts.Select(f => f.DeptId).ToList().Contains(u2.DeptId));
            }
            var query = Context.Queryable<SysUser>()
                .LeftJoin<ZrSysUser>((u, u2) => u.UserId == u2.MainId)
                .LeftJoin<SysDept>((u, u2, dept) => u2.DeptId == dept.DeptId)
                .Where(exp.ToExpression())
                .Select((u, u2, dept) => new SysUser
                {
                    UserId = u.UserId.SelectAll(),
                    DeptName = dept.DeptName,
                    Phonenumber = u2.Phonenumber,
                    Email = u2.Email
                });

            return query.ToPage(pager);
        }

        /// <summary>
        /// 通过用户ID查询用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public SysUser SelectUserById(long userId)
        {
            var user = Queryable().Filter(null, true).WithCache(60 * 5)
                .Where(f => f.UserId == userId).First();
            //补充zr表资料
            EnsureZrUser(user.UserId, user.UserName, user.NickName);
            var zruser=Context.Queryable<ZrSysUser>().Where(it=>it.MainId==user.UserId).First();
            user.Email = zruser.Email;
            user.Phonenumber = zruser.Phonenumber;
            user.DeptId = Context.Queryable<ZrSysUser>().Where(x => x.MainId == userId)
                .Select(x => x.DeptId).First();

            if (user != null && user.UserId > 0)
            {
                user.Roles = RoleService.SelectUserRoleListByUserId(userId);
                user.RoleIds = user.Roles.Select(x => x.RoleId).ToArray();
            }
            return user;
        }

        /// <summary>
        /// 校验用户名称是否唯一
        /// </summary>
        /// <param name="site"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string CheckUserNameUnique(string site, string userName)
        {
            int count = Count(it => it.Site == site && it.UserName == userName);
            if (count > 0)
            {
                return UserConstants.NOT_UNIQUE;
            }
            return UserConstants.UNIQUE;
        }

        string EncryptedPassword(string password)
        {
            return Context.Ado.GetString($"select imes.password.encrypt('{password}') from dual");
        }

        /// <summary>
        /// 新增保存用户信息
        /// </summary>
        /// <param name="sysUser"></param>
        /// <returns></returns>
        public SysUser InsertUser(SysUser sysUser)
        {
            sysUser.Password = EncryptedPassword(sysUser.Password);
           
           sysUser.UserId = Queryable().Max(x => x.UserId) + 1;
           Insertable(sysUser).ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.M_EMP_HT SELECT * FROM SAJET.M_EMP  WHERE  ID = @ID ",new List<SugarParameter> { new SugarParameter("@ID",sysUser.UserId)});
              
            Context.Insertable(new ZrSysUser { 
                MainId = sysUser.UserId,
                UserName = sysUser.UserName,
                NickName = sysUser.NickName,
                DeptId = sysUser.DeptId,
                Phonenumber = sysUser.Phonenumber,
                Sex = sysUser.Sex,
                Email = sysUser.Email,
            }).ExecuteReturnIdentity();
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.M_ZR_USER_HT SELECT * FROM SAJET.M_ZR_USER  WHERE MAIN_ID = @ID ", new List<SugarParameter> { new SugarParameter("@ID", sysUser.UserId) });
            //新增用户角色信息
            UserRoleService.InsertUserRole(sysUser);
            //新增用户岗位信息
            UserPostService.InsertUserPost(sysUser);
            return sysUser;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int UpdateUser(SysUser user)
        {
            var roleIds = RoleService.SelectUserRoles(user.UserId);
            var diffArr = roleIds.Where(c => !((IList)user.RoleIds).Contains(c)).ToArray();
            var diffArr2 = user.RoleIds.Where(c => !((IList)roleIds).Contains(c)).ToArray();
            bool result = UseTran2(() =>
            {
                if (diffArr.Length > 0 || diffArr2.Length > 0)
                {
                    //删除用户与角色关联
                    UserRoleService.DeleteUserRoleByUserId((int)user.UserId);
                    //新增用户与角色关联
                    UserRoleService.InsertUserRole(user);
                }
                // 删除用户与岗位关联
                UserPostService.Delete(user.UserId);
                // 新增用户与岗位管理
                UserPostService.InsertUserPost(user);
                SysUserDto zruser = new SysUserDto();
                zruser.Phonenumber = user.Phonenumber;
                zruser.Email = user.Email;
                ChangeUser(user, zruser);
            });
            return result ? 1 : 0;
        }

        public int ChangeUser(SysUser user, SysUserDto dto)
        {
            user.UpdateTime = DateTime.Now;
            var ret = 0;

            UseTran2(() =>
            {
                ret += Update(user, t => new
                {
                    t.NickName,
                    t.DeptName,
                    t.Status,
                    t.PostIds,
                    t.Remark,
                    t.UpdateEmpno,
                    t.UpdateTime
                }, true);

                if(dto != null)
                {
                    EnsureZrUser(user.UserId, user.UserName, user.NickName);
                    ret += Context.Updateable<ZrSysUser>().SetColumns(_ => new ZrSysUser
                    {
                        Email = dto.Email,
                        Phonenumber = dto.Phonenumber,
                        Sex = dto.Sex,
                    }).Where(x => x.MainId == user.UserId)
                    .ExecuteCommand();
                }
            });
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.M_EMP_HT SELECT * FROM SAJET.M_EMP  WHERE  ID = @ID ", new List<SugarParameter> { new SugarParameter("@ID", user.UserId) });
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.M_ZR_USER_HT SELECT * FROM SAJET.M_ZR_USER  WHERE  MAIN_ID = @ID ", new List<SugarParameter> { new SugarParameter("@ID", user.UserId) });
            return ret;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int ResetPwd(long userid, string password)
        {
            password = EncryptedPassword(password);
            return Update(new SysUser() { UserId = userid, Password = password }, it => new { it.Password }, f => f.UserId == userid);
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int ChangeUserStatus(SysUser user)
        {
            CheckUserAllowed(user);
            return Update(user, it => new { it.Enabled }, f => f.UserId == user.UserId);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int DeleteUser(long userid, string updateemp)
        {
            CheckUserAllowed(new SysUser() { UserId = userid });
            //删除用户与角色关联
            UserRoleService.DeleteUserRoleByUserId((int)userid);
            // 删除用户与岗位关联
            UserPostService.Delete(userid);
            //更新
            Context.Updateable<SysUser>().SetColumns(_ => new SysUser
            {
                UpdateTime=DateTime.Now,
                UpdateEmpno=updateemp
            })
                .Where(x => x.UserId == userid)
                .ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.M_EMP_HT SELECT * FROM SAJET.M_EMP  WHERE  ID = @ID ", new List<SugarParameter> { new SugarParameter("@ID", userid) });
            //删除
            var i =  Context.Ado.ExecuteCommand("delete IMES.M_EMP  WHERE  ID = @ID ", new List<SugarParameter> { new SugarParameter("@ID", userid) });
           //更新
            Context.Updateable<ZrSysUser>().SetColumns(_ => new ZrSysUser
            {
                DelFlag = 2
            })
                .Where(x => x.MainId == userid)
                .ExecuteCommand();
            //备份
            Context.Ado.ExecuteCommand("insert INTO SAJET.M_ZR_USER_HT SELECT * FROM SAJET.M_ZR_USER  WHERE  MAIN_ID = @ID ", new List<SugarParameter> { new SugarParameter("@ID", userid) });
            //备份
            Context.Ado.ExecuteCommand("delete IMES.M_ZR_USER  WHERE  MAIN_ID = @ID ", new List<SugarParameter> { new SugarParameter("@ID", userid) });
            return i;
        }

        void EnsureZrUser(long userId, string userName = null, string nickName = null)
        {
            if (Context.Queryable<ZrSysUser>().Where(x => x.MainId == userId).Any()) return;

            Context.Insertable(new ZrSysUser
            {
                MainId = userId,
                UserName = userName,
                NickName = nickName,
                DeptId = 0,
                DelFlag = 0,
            }).ExecuteReturnIdentity();
        }

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int UpdatePhoto(ZrSysUser user)
        {
            EnsureZrUser(user.MainId.Value);
            return Context.Updateable<ZrSysUser>()
                .SetColumns(_ => new ZrSysUser { Avatar = user.Avatar })
                .Where(x => x.MainId == user.MainId)
                .ExecuteCommand();
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public SysUser Register(RegisterDto dto)
        {
            if (!Tools.PasswordStrength(dto.Password))
            {
                throw new CustomException("密码强度不符合要求");
            }
            if (!Tools.CheckUserName(dto.Username))
            {
                throw new CustomException("用户名不符合要求");
            }
            //密码md5
            //string password = NETCore.Encrypt.EncryptProvider.Md5(dto.Password);
            var password = EncryptedPassword(dto.Password);

            var ip_info = IpTool.Search(dto.UserIP);
            SysUser user = new()
            {
                Site = dto.Site,
                CreateTime = DateTime.Now,
                UserName = dto.Username,
                NickName = dto.Username,
                Password = password,
                Status = 0,
                DeptName = "",
                Remark = "用户注册",
            };
            if (UserConstants.NOT_UNIQUE.Equals(CheckUserNameUnique(dto.Site, dto.Username)))
            {
                throw new CustomException($"保存用户{dto.Username}失败，注册账号已存在");
            }

            while (true)
            {
                try
                {
                    user.UserId = Queryable().Max(x => x.UserId) + 1;
                    Insertable(user);
                    break;
                }
                catch { continue; }
            }
            //user.UserId = Insertable(user).ExecuteReturnIdentity();

            Context.Insertable(new ZrSysUser
            {
                MainId = user.UserId,
                UserName = user.UserName,
                NickName = user.NickName,
                DeptId = user.DeptId,
                Phonenumber = user.Phonenumber,
                Sex = user.Sex,
                Email = user.Email,
                Province = ip_info.Province,
                City = ip_info.City
            }).ExecuteReturnIdentity();

            return user;
        }

        /// <summary>
        /// 校验角色是否允许操作
        /// </summary>
        /// <param name="user"></param>
        public void CheckUserAllowed(SysUser user)
        {
            if (user.IsAdmin())
            {
                throw new CustomException("不允许操作超级管理员角色");
            }
        }

        /// <summary>
        /// 校验用户是否有数据权限
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="loginUserId"></param>
        public void CheckUserDataScope(long userid, long loginUserId)
        {
            if (!SysUser.IsAdmin(loginUserId))
            {
                SysUser user = new SysUser() { UserId = userid };

                //TODO 判断用户是否有数据权限
            }
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public (string, object, object) ImportUsers(List<SysUser> users)
        {
            users.ForEach(x =>
            {
                x.CreateTime = DateTime.Now;
                x.Enabled = "Y";
                //x.DelFlag = 0;
                x.Password = x.UserName;
                x.Remark = x.Remark.IsEmpty() ? "数据导入" : x.Remark;
            });
            var x = Context.Storageable(users)
                .SplitInsert(it => !it.Any())
                .SplitIgnore(it => it.Item.UserName == GlobalConstant.AdminRole)
                .SplitError(x => x.Item.UserName.IsEmpty(), "用户名不能为空")
                .SplitError(x => !Tools.CheckUserName(x.Item.UserName), "用户名不符合规范")
                .WhereColumns(it => it.UserName)//如果不是主键可以这样实现（多字段it=>new{it.x1,it.x2}）
                .ToStorage();
            var result = x.AsInsertable.ExecuteCommand();//插入可插入部分;

            string msg = string.Format(" 插入{0} 更新{1} 错误数据{2} 不计算数据{3} 删除数据{4} 总共{5}",
                               x.InsertList.Count,
                               x.UpdateList.Count,
                               x.ErrorList.Count,
                               x.IgnoreList.Count,
                               x.DeleteList.Count,
                               x.TotalList.Count);
            //输出统计                      
            Console.WriteLine(msg);

            //输出错误信息               
            foreach (var item in x.ErrorList)
            {
                Console.WriteLine("userName为" + item.Item.UserName + " : " + item.StorageMessage);
            }
            foreach (var item in x.IgnoreList)
            {
                Console.WriteLine("userName为" + item.Item.UserName + " : " + item.StorageMessage);
            }

            return (msg, x.ErrorList, x.IgnoreList);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录实体</param>
        /// <returns></returns>
        public SysUser Login(LoginBodyDto user)
        {
            SysUser sUser = Queryable().LeftJoin<ZrSysUser>((u, u2) => u.UserId == u2.MainId)
                .Where((u, u2) => u.Site == user.Site && u.UserName == user.Username
                && (u2.DelFlag == 0 || u2.MainId == null)
                && SqlFunc.MappingColumn<string>($"SAJET.password.compare('{user.Password}', passwd)") == "OK")
                .First();

            return sUser;
            
            //return GetFirst(it => it.UserName == user.Username && it.Password.ToLower() == user.Password.ToLower());
        }

        /// <summary>
        /// 修改登录信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void UpdateLoginInfo(LoginBodyDto user, long userId)
        {
            Context.Updateable<ZrSysUser>().SetColumns(_ => new ZrSysUser
            {
                LoginIP = user.LoginIP,
                LoginDate = DateTime.Now,
            })
                .Where(x => x.MainId == userId)
                .ExecuteCommand();
        }
    }
}
