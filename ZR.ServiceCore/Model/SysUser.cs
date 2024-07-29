using MiniExcelLibs.Attributes;

namespace ZR.Model.System
{
    namespace ZR.Model.Business
    {
        /// <summary>
        /// 
        /// </summary>
        [SugarTable(TableName = "SAJET.M_EMP")]
        public class MEmp
        {
            /// <summary>
            /// Id 
            /// </summary>
            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public long Id { get; set; }

            /// <summary>
            /// 工号 
            /// </summary>
            [SugarColumn(ColumnName = "EMP_NO")]
            public string EmpNo { get; set; }

            /// <summary>
            /// 姓名 
            /// </summary>
            [SugarColumn(ColumnName = "EMP_NAME")]
            public string EmpName { get; set; }

            /// <summary>
            /// 密码 
            /// </summary>
            public string Passwd { get; set; }

            /// <summary>
            /// 班别名称 
            /// </summary>
            [SugarColumn(ColumnName = "SHIFT_NAME")]
            public string ShiftName { get; set; }

            /// <summary>
            /// 厂区 
            /// </summary>
            public string Site { get; set; }

            /// <summary>
            /// 部门名称 
            /// </summary>
            [SugarColumn(ColumnName = "DEPT_NAME")]
            public string DeptName { get; set; }

            /// <summary>
            /// 最近一次密码修改时间 
            /// </summary>
            [SugarColumn(ColumnName = "CHANGE_PW_TIME")]
            public DateTime? ChangePwTime { get; set; }

            /// <summary>
            /// Remark 
            /// </summary>
            public string Remark { get; set; }

            /// <summary>
            /// CreateTime 
            /// </summary>
            [SugarColumn(ColumnName = "CREATE_TIME")]
            public string CreateTime { get; set; }

            /// <summary>
            /// CreateEmpno 
            /// </summary>
            [SugarColumn(ColumnName = "CREATE_EMPNO")]
            public DateTime? CreateEmpno { get; set; }

            /// <summary>
            /// 修改人工号 
            /// </summary>
            [SugarColumn(ColumnName = "UPDATE_EMPNO")]
            public string UpdateEmpno { get; set; }

            /// <summary>
            /// 修改时间 
            /// </summary>
            [SugarColumn(ColumnName = "UPDATE_TIME")]
            public DateTime? UpdateTime { get; set; }

            /// <summary>
            /// 有效状态 
            /// </summary>
            public string Enabled { get; set; }

            /// <summary>
            /// email 员工邮箱 
            /// </summary>
            public string OPTION1 { get; set; }

            /// <summary>
            /// OPTION2 
            /// </summary>
            public string OPTION2 { get; set; }

            /// <summary>
            /// OPTION3 
            /// </summary>
            public string OPTION3 { get; set; }

            /// <summary>
            /// OPTION4 
            /// </summary>
            public string OPTION4 { get; set; }

            /// <summary>
            /// OPTION5 
            /// </summary>
            public string OPTION5 { get; set; }
        }
    }

    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("SAJET.m_zr_user", "ZR用户表")]
    [Tenant("0")]
    public class ZrSysUser
    {
        /// <summary>
        /// 对应 M_EMP 的 Id 字段
        /// </summary>
        [SugarColumn(ColumnName = "MAIN_ID")]
        public long? MainId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [SugarColumn(ColumnName = "USERID", IsPrimaryKey = true, OracleSequenceName = "SAJET.zr_sys_user_id")]
        public long ZrUserId { get; set; }
        /// <summary>
        /// 登录用户名
        /// </summary>
        [SugarColumn(Length = 30, ColumnDescription = "用户账号", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string UserName { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        [SugarColumn(Length = 30, ColumnDescription = "用户昵称", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string NickName { get; set; }
        /// <summary>
        /// 用户类型（00系统用户）
        /// </summary>
        [SugarColumn(Length = 2, ColumnDescription = "用户类型（00系统用户）", DefaultValue = "00")]
        public string UserType { get; set; } = "00";
        //[SugarColumn(IsOnlyIgnoreInsert = true)]
        public string Avatar { get; set; }
        [SugarColumn(Length = 50, ColumnDescription = "用户邮箱")]
        public string Email { get; set; }

        [JsonIgnore]
        [ExcelIgnore]
        [SugarColumn(Length = 100, ColumnDescription = "密码", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phonenumber { get; set; }

        /// <summary>
        /// 用户性别（0男 1女 2未知）
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 帐号状态（0正常 1停用）
        /// </summary>
        [ExcelIgnore]
        [SugarColumn(DefaultValue = "0")]
        public int Status { get; set; }

        /// <summary>
        /// 删除标志（0代表存在 2代表删除）
        /// </summary>
        [SugarColumn(DefaultValue = "0")]
        public int DelFlag { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        [SugarColumn(IsOnlyIgnoreInsert = true)]
        public string LoginIP { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [SugarColumn(IsOnlyIgnoreInsert = true)]
        [ExcelColumn(Name = "登录日期", Format = "yyyy-MM-dd HH:mm:ss")]
        public DateTime? LoginDate { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        [SugarColumn(DefaultValue = "0")]
        public long DeptId { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
    }

    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("SAJET.M_EMP", "用户表")]
    [Tenant("0")]
    public class SysUser
    {
        public string Site { get; set; }

        /// <summary>
        /// M_EMP 的 Id 字段（用户id）
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "Id")]
        public long UserId { get; set; }

        /// <summary>
        /// 登录用户名 (EMP_NO)
        /// </summary>
        [SugarColumn(Length = 30, ColumnName = "EMP_NO", ColumnDescription = "用户账号", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string UserName { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        [SugarColumn(Length = 30, ColumnName = "EMP_NAME", ColumnDescription = "用户昵称", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string NickName { get; set; }

        [JsonIgnore]
        [ExcelIgnore]
        [SugarColumn(Length = 100, ColumnName = "Passwd", ColumnDescription = "密码", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string Password { get; set; }

        /// <summary>
        /// 班别名称 
        /// </summary>
        [SugarColumn(ColumnName = "SHIFT_NAME")]
        public string ShiftName { get; set; }

        /// <summary>
        /// 部门名称 
        /// </summary>
        [SugarColumn(ColumnName = "DEPT_NAME")]
        public string DeptName { get; set; }

        /// <summary>
        /// 最近一次密码修改时间 
        /// </summary>
        [SugarColumn(ColumnName = "CHANGE_PW_TIME")]
        public DateTime? ChangePwTime { get; set; }

        /// <summary>
        /// Remark 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_EMPNO")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 修改人工号 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// 修改时间 
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 有效状态 
        /// </summary>
        [ExcelIgnore]
        public string Enabled { get; set; }

        /// <summary>
        /// 帐号状态（0正常 1停用）
        /// </summary>
        [ExcelIgnore]
        [SugarColumn(IsIgnore = true)]
        public int Status { 
            get => Enabled == "Y" ? 0 : 1; 
            set => Enabled = value == 0 ? "Y" : "N";
        }

        /// <summary>
        /// email 员工邮箱 
        /// </summary>
        public string OPTION1 { get; set; }

        /// <summary>
        /// OPTION2 
        /// </summary>
        public string OPTION2 { get; set; }

        /// <summary>
        /// OPTION3 
        /// </summary>
        public string OPTION3 { get; set; }

        /// <summary>
        /// OPTION4 
        /// </summary>
        public string OPTION4 { get; set; }

        /// <summary>
        /// OPTION5 
        /// </summary>
        public string OPTION5 { get; set; }

        #region 表额外字段
        public bool IsAdmin()
        {
            return IsAdmin(UserId);
        }
        public static bool IsAdmin(long userId)
        {
            return 1 == userId;
        }

        /// <summary>
        /// 拥有角色个数
        /// </summary>
        //[SugarColumn(IsIgnore = true)]
        //public int RoleNum { get; set; }

        /// <summary>
        /// 部门Id（需要串 ZrSysUser 手动填入，或从前端接收）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public long DeptId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string Phonenumber { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string Email { get; set; }

        /// <summary>
        /// 用户性别（0男 1女 2未知）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int Sex { get; set; }

        /// <summary>
        /// 角色id集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [ExcelIgnore]
        public long[] RoleIds { get; set; }
        /// <summary>
        /// 岗位集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [ExcelIgnore]
        public int[] PostIds { get; set; }

        [SugarColumn(IsIgnore = true)]
        [ExcelIgnore]
        public List<SysRole> Roles { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string WelcomeMessage
        {
            get
            {
                int now = DateTime.Now.Hour;

                if (now > 0 && now <= 6)
                {
                    return "午夜好";
                }
                else if (now > 6 && now <= 11)
                {
                    return "早上好";
                }
                else if (now > 11 && now <= 14)
                {
                    return "中午好";
                }
                else if (now > 14 && now <= 18)
                {
                    return "下午好";
                }
                else
                {
                    return "晚上好";
                }
            }
        }
        [SugarColumn(IsIgnore = true)]
        public string WelcomeContent { get; set; }

        #endregion
    }
}
