using System;
using SqlSugar;
using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using System.Linq;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using Infrastructure.Model;
using Microsoft.AspNetCore.Http;
using ZR.Model.System;
using Microsoft.Extensions.Options;
using ZR.Service.System.IService;
using ZR.Service.System;
using System.Security.Cryptography;
using System.Text;
using System.Security.Policy;
using Microsoft.VisualBasic.FileIO;

namespace ZR.Service.Business
{
    /// <summary>
    /// NPI项目管理Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(INpiProjetService), ServiceLifetime = LifeTime.Transient)]
    public class NpiProjetService : BaseService<NpiProjet>, INpiProjetService
    {
        DbProvidernew db = new DbProvidernew();
        private OptionsSetting OptionsSetting;
        public NpiProjetService( IOptions<OptionsSetting> options)
        {
            OptionsSetting = options.Value;
        }
        /// <summary>
        /// 查询NPI项目管理列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<NpiProjetDto> GetList(NpiProjetQueryDto parm)
        {
            var predicate = Expressionable.Create<NpiProjet>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.NpiNo), it => it.NpiNo.Contains(parm.NpiNo));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.CompanyName), it => it.CompanyName.Contains(parm.CompanyName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.CustomerName), it => it.CustomerName.Contains(parm.CustomerName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.Ipn), it => it.Ipn.Contains(parm.Ipn));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.Apn), it => it.Apn.Contains(parm.Apn));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProjectTrialStage), it => it.ProjectTrialStage.Contains(parm.ProjectTrialStage));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProductType), it => it.ProductType.Contains(parm.ProductType));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProductLine), it => it.ProductLine.Contains(parm.ProductLine));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.RdManager), it => it.RdManager.Contains(parm.RdManager));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.NpiEngineer), it => it.NpiEngineer.Contains(parm.NpiEngineer));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProductVersion), it => it.ProductVersion.Contains(parm.ProductVersion));
            // predicate = predicate.AndIF(parm.BeginCreateTime == null, it => it.CreateTime >= DateTime.Now.ToShortDateString().ParseToDateTime());
            predicate = predicate.AndIF(parm.BeginCreateTime != null, it => it.CreateTime >= parm.BeginCreateTime);
            predicate = predicate.AndIF(parm.EndCreateTime != null, it => it.CreateTime <= parm.EndCreateTime);
            var response = Queryable()
                .OrderBy("Id desc")
                .Where(predicate.ToExpression())
                .ToPage<NpiProjet, NpiProjetDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public NpiProjet GetInfo(int Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        [SugarTable("SAJET.P_NPI_FLOWSTEP")]
        public class FlowStep
        {
            public int ID { get; set; }
            public string NPI_NO { get; set; }
            public int SEQ { get; set; }
            public string STEP { get; set; }
            public string STATUS { get; set; }
            public string COLOR { get; set; }
            public string ADVICE { get; set; }
            public string URL { get; set; }
            public Nullable<DateTime> UPDATE_TIME { get; set; }
            public string UPDATE_EMPNO { get; set; }
        }

        /// <summary>
        /// 添加NPI项目管理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public NpiProjet AddNpiProjet(NpiProjet model)
        {
            //曾加流程对应表单
            // 执行多条 INSERT 语句
            List<FlowStep> flowSteps = new List<FlowStep>
                {
                    new FlowStep { ID = (int)model.Id, NPI_NO = model.NpiNo, SEQ = 1, STEP = "启动", STATUS = "已完成", COLOR = "blue",UPDATE_TIME=DateTime.Now, UPDATE_EMPNO = model.CreateEmpno  },
                    new FlowStep {ID = (int)model.Id, NPI_NO =  model.NpiNo, SEQ = 2, STEP = "研发试制准备", STATUS = "进行中", COLOR = "#50f637",UPDATE_TIME=null  },
                     new FlowStep {ID = (int)model.Id, NPI_NO =  model.NpiNo, SEQ = 3, STEP = "工厂试制准备PE", STATUS = "未开始", COLOR = "blue",UPDATE_TIME=null  },
                    new FlowStep {ID = (int)model.Id, NPI_NO =  model.NpiNo, SEQ = 4, STEP = "工厂试制准备TE", STATUS = "未开始", COLOR = "blue",UPDATE_TIME=null  },
                    new FlowStep {ID = (int)model.Id, NPI_NO =  model.NpiNo, SEQ = 5, STEP = "工厂试制准备QE", STATUS = "未开始", COLOR = "blue",UPDATE_TIME=null  },
                    new FlowStep {ID = (int)model.Id, NPI_NO =  model.NpiNo, SEQ = 6, STEP = "工厂试制准备IE", STATUS = "未开始", COLOR = "blue",UPDATE_TIME=null  },
                    new FlowStep {ID = (int)model.Id, NPI_NO =  model.NpiNo, SEQ = 7, STEP = "工厂试制准备ME", STATUS = "未开始", COLOR = "blue",UPDATE_TIME=null  },
                    new FlowStep {ID = (int)model.Id, NPI_NO =  model.NpiNo, SEQ = 8, STEP = "试制准备完成", STATUS = "未开始", COLOR = "blue",UPDATE_TIME=null  },
                     new FlowStep {ID = (int)model.Id, NPI_NO =  model.NpiNo, SEQ = 9, STEP = "试制任务令PO释放", STATUS = "未开始", COLOR = "blue",UPDATE_TIME=null },
                new FlowStep {ID = (int)model.Id, NPI_NO =  model.NpiNo, SEQ = 10, STEP = "上传试制报告", STATUS = "未开始", COLOR = "blue",UPDATE_TIME=null  }
                    // ... 更多Person对象
                };
            foreach (var flowStep in flowSteps)
            {
                Context.Insertable(flowStep).ExecuteCommand();
            }


            return Context.Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改NPI项目管理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateNpiProjet(NpiProjet model)
        {
            var response = Update(w => w.Id == model.Id, it => new NpiProjet()
            {
                //NpiNo = model.NpiNo,
                CompanyName = model.CompanyName,
                CustomerName = model.CustomerName,
                Ipn = model.Ipn,
                Apn = model.Apn,
                ProjectTrialStage = model.ProjectTrialStage,
                ProductType = model.ProductType,
                ProductLine = model.ProductLine,
                RdManager = model.RdManager,
                NpiEngineer = model.NpiEngineer,
                ProductVersion = model.ProductVersion,
                ProjectRemark = model.ProjectRemark,
                UpdateTime = model.UpdateTime,
                UpdateEmpno = model.UpdateEmpno,
                Option1=model.Option1
            });
            return response;
            //return Update(model, true);
        }
        /// <summary>
        /// 清空NPI项目管理
        /// </summary>
        /// <returns></returns>
        public bool TruncateNpiProjet()
        {
            var newTableName = $"p_npi_projet_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("SAJET.p_npi_projet", newTableName);
            }

            return Truncate();
        }
        /// <summary>
        /// 导入NPI项目管理
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportNpiProjet(List<NpiProjet> list)
        {
            var x = Context.Storageable(list)
                .SplitInsert(it => !it.Any())
                //.SplitError(x => x.Item.NpiNo.IsEmpty(), "NPINO不能为空")
                .SplitError(x => x.Item.CompanyName.IsEmpty(), "公司名称不能为空")
                .SplitError(x => x.Item.CustomerName.IsEmpty(), "客户名称不能为空")
                .SplitError(x => x.Item.Ipn.IsEmpty(), "料号不能为空")
                .SplitError(x => x.Item.Apn.IsEmpty(), "客户料号不能为空")
                .SplitError(x => x.Item.ProjectTrialStage.IsEmpty(), "项目试制阶段不能为空")
                .SplitError(x => x.Item.ProductType.IsEmpty(), "产品类型不能为空")
                .SplitError(x => x.Item.ProductLine.IsEmpty(), "产品线不能为空")
                .SplitError(x => x.Item.RdManager.IsEmpty(), "研发项目经理不能为空")
                .SplitError(x => x.Item.NpiEngineer.IsEmpty(), "NPI工程师不能为空")
                .SplitError(x => x.Item.ProductVersion.IsEmpty(), "产品版本不能为空")
                .SplitError(x => x.Item.ProjectRemark.IsEmpty(), "备注不能为空")
                //.WhereColumns(it => it.UserName)//如果不是主键可以这样实现（多字段it=>new{it.x1,it.x2}）
                .ToStorage();
            var result = x.AsInsertable.ExecuteCommand();//插入可插入部分;

            string msg = $"插入{x.InsertList.Count} 更新{x.UpdateList.Count} 错误数据{x.ErrorList.Count} 不计算数据{x.IgnoreList.Count} 删除数据{x.DeleteList.Count} 总共{x.TotalList.Count}";
            Console.WriteLine(msg);

            //输出错误信息               
            foreach (var item in x.ErrorList)
            {
                Console.WriteLine("错误" + item.StorageMessage);
            }
            foreach (var item in x.IgnoreList)
            {
                Console.WriteLine("忽略" + item.StorageMessage);
            }

            return (msg, x.ErrorList, x.IgnoreList);
        }

        public int GetMaxID()
        {

            var _TFIELD = new SugarParameter("@TFIELD", "ID");//参数
            var _TTABLE = new SugarParameter("@TTABLE", "SAJET.p_npi_projet"); //参数
            var _TNUM = new SugarParameter("@TNUM", "8"); //参数
                                                          //var _TRES = new SugarParameter("@TRES", null, typeof(string), ParameterDirection.ReturnValue); //参数
                                                          //var _T_MAXID = new SugarParameter("@T_MAXID", null, typeof(string), ParameterDirection.ReturnValue); //返回参数
            var _TRES = new SugarParameter("@TRES", null, true); //参数
            var _T_MAXID = new SugarParameter("@T_MAXID", null, true); //返回

            //   _TRES.Direction = ParameterDirection.Output;//指定类型为输数类型
            //TRES.IsRefCursor = true;                                                //  aCur_tempp.IsRefCursor = false;// 表示返回游标
            //   _T_MAXID.Direction = ParameterDirection.Output;//指定类型为输数类型
            //T_MAXID.IsRefCursor = true;

            var dt = db.GetSugarDbContext().Ado.UseStoredProcedure().GetDataSetAll("SAJET.sp_GET_MAXID", _TFIELD, _TTABLE, _TNUM, _TRES, _T_MAXID);
            int maxid = Convert.ToInt32(_T_MAXID.Value);
            return maxid;
        }

        /// <summary>
        /// 批量删除系统操作
        /// </summary>
        /// <param name="operIds">需要删除的操作ID</param>
        /// <returns>结果</returns>
        public int DeleteDataByIds(long[] operIds)
        {
            //return Context.Deleteable<MLine>().In(operIds).ExecuteCommand();

            //插入历史表
            //例2: 同实体不同表插入    
            Context.Queryable<NpiProjet>().Where("Id in (@Id) ", new { Id = operIds }).IntoTable<MLinePlan>("SAJET.p_npi_projet_ht");
            //.IgnoreColumns(it=>it.Id) 如果是自增可以忽略，不过ID就不一样了

            Context.Deleteable<FlowStep>().Where("Id in (@Id) ", new { Id = operIds }).ExecuteCommand();

            //批量删除不按主键
            return Context.Deleteable<NpiProjet>().Where("Id in (@Id) ", new { Id = operIds }).ExecuteCommand();

        }

        public DataTable GetStepById(int id)
        {
            DataTable dt = new DataTable();
            string sql = @"select B.SEQ,B.STEP,B.UPDATE_TIME time,B.STATUS,decode(B.STATUS,'已完成','true','false') done , B.COLOR,B.ADVICE,c.NICKNAME||'/'||B.UPDATE_EMPNO emp,B.URL  from IMES.P_NPI_PROJET a,IMES.P_NPI_FLOWSTEP b,IMES.M_ZR_USER c
                where A.ID=B.ID(+) and A.ID='" + id + "' and b.UPDATE_EMPNO=c.USERNAME(+) order by to_number(seq) asc";
            //var seqs = GetSugarDbContext(dbName).Ado.SqlQuery<OracleSeq>(sql);


            dt = db.GetSugarDbContext().Ado.GetDataTable(sql);
            return dt;
        }
        public DataTable GetApprovalLogById(int id)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT A.STEP,A.STATUS,c.NICKNAME||'/'||A.UPDATE_EMPNO UPDATE_EMPNO,A.UPDATE_TIME,A.ADVICE  FROM IMES.P_NPI_FLOWSTEP A  ,IMES.M_ZR_USER c
                       WHERE A.ID='" + id + "' and a.UPDATE_EMPNO=c.USERNAME(+) order by to_number(seq) asc";
            //var seqs = GetSugarDbContext(dbName).Ado.SqlQuery<OracleSeq>(sql);


            dt = db.GetSugarDbContext().Ado.GetDataTable(sql);
            return dt;
        }
        public DataTable GetStepInfoById(int id, string step)
        {
            DataTable dt = new DataTable();
            string sql = @"select  distinct a.NPI_NO, a.ADVICE ,
                              DECODE(a.STATUS ,'进行中','true','已完成','true','false' ) STATUS,
                            DECODE(a.STATUS ,'进行中','false','true' ) COMMIT,a.URL,a.URL,a.NICKNAME||'/'||a.UPDATE_EMPNO UPDATE_EMPNO,a.update_time,
                            e.REMARKS,e.RESULT, e.DICTSORT ,e.DICTLABEL  name from (
                           select a.*,b.nickname   from IMES.P_NPI_FLOWSTEP a,IMES.M_ZR_USER b  where 
                              a.STEP=@STEP and a.id=@ID and a.UPDATE_EMPNO=b.username(+) ) a
                               left join                              
                              (select b.ID,a.DICTNAME, b.REMARKS,b.RESULT, a.DICTSORT ,a.DICTLABEL from (
                            select c.*,d.DICTNAME from IMES.M_ZR_DICT_DATA c,IMES.M_ZR_DICT_TYPE d
                            where d.DICTNAME=@STEP
                              and c.DICTTYPE=d.DICTTYPE) a,
                              (select * from  imes.P_NPI_STEPINFO where step= @STEP and id=@ID )b
                              where a.DICTNAME=b.step(+)
                              and a.DICTLABEL=b.option1(+)) e
                              on 1=1
                             order by  e.DICTSORT ";
            //var seqs = GetSugarDbContext(dbName).Ado.SqlQuery<OracleSeq>(sql);
            // 定义参数
            var parameters = new SugarParameter[]
            {
            new SugarParameter("@STEP", step),
            new SugarParameter("@ID", id),
            };

            dt = db.GetSugarDbContext().Ado.GetDataTable(sql, parameters);
            return dt;
        }
        public DataTable GetOrderInfoById(int id,string step)
        {
            DataTable dt=new DataTable();
            string sql = @"select distinct rownum seq, a.*,B.NPI_NO, b.ADVICE ,DECODE(b.STATUS ,'进行中','true','已完成','true','false' ) STATUS,
                            DECODE(b.STATUS ,'进行中','false','true' ) COMMIT,b.URL,c.NICKNAME||'/'||b.UPDATE_EMPNO UPDATE_EMPNO,b.update_time
                             from imes.p_npi_order a,IMES.P_NPI_FLOWSTEP b,IMES.M_ZR_USER c
                        where a.id(+)=B.ID
                          and b.UPDATE_EMPNO=c.USERNAME(+)
                        and b.id= @ID
                        and B.STEP=@STEP ";
            //var seqs = GetSugarDbContext(dbName).Ado.SqlQuery<OracleSeq>(sql);
            var parameters = new SugarParameter[]
           {
            new SugarParameter("@STEP", step),
            new SugarParameter("@ID", id),
           };

            dt = db.GetSugarDbContext().Ado.GetDataTable(sql, parameters);
            return dt;
        }

        public DataTable GetStationType()
        {
            DataTable dt = new DataTable();
            string sql = @"select id KEY,station_type label from IMES.M_STATION_TYPE ";


            dt = db.GetSugarDbContext().Ado.GetDataTable(sql);
            return dt;
        }

        public DataTable GetStationTypeConfig()
        {
            DataTable dt = new DataTable();
            string sql = @"select  distinct  b.CONFIG_NAME  key 
                  from IMES.M_BLOCK_CONFIG_TYPE a,IMES.M_BLOCK_CONFIG_VALUE b
                    where a.CONFIG_TYPE_NAME='WIP_SEQ'
                    and a.CONFIG_TYPE_ID = b.CONFIG_TYPE_ID ";


            dt = db.GetSugarDbContext().Ado.GetDataTable(sql);
            return dt;
        }

        public int AddRdItem(StepInfo modal)
        {
            //更新状态表为已完成
            //插入step info 内容    
           
            for (int i = 0; i < modal.subItems.Count; i++)
            {
                string sql= @"INSERT INTO IMES.P_NPI_STEPINFO
                            (ID, STEP, SORT, RESULT, REMARKS, UPDATE_EMPNO,OPTION1)
                            VALUES(@ID, @STEP, @SORT, @RESULT, @REMARKS, @UPDATE_EMPNO,@OPTION1 )";
                var parameters = new SugarParameter[]
                {
                  new SugarParameter("@ID", modal.id),
                  new SugarParameter("@STEP", modal.step),
                  new SugarParameter("@SORT", modal.subItems[i].dictsort),
                  new SugarParameter("@RESULT",modal.subItems[i].result),
                  new SugarParameter("@REMARKS", modal.subItems[i].remarks),
                  new SugarParameter("@UPDATE_EMPNO", modal.update_empno),
                  new SugarParameter("@OPTION1", modal.subItems[i].name),
                 };
                db.GetSugarDbContext().Ado.ExecuteCommand(sql, parameters);
            }
            string  sql1 = @"update IMES.P_NPI_FLOWSTEP set status='已完成' ,ADVICE=@ADVICE ,color='bule',update_time=@update_time ,update_empno=@update_empno where id=@id and step=@step";
            var parameters1 = new SugarParameter[]
           {
            new SugarParameter("@update_time", DateTime.Now),
            new SugarParameter("@ADVICE", modal.advice),
            new SugarParameter("@update_empno", modal.update_empno),
            new SugarParameter("@id", modal.id),
            new SugarParameter("@step",  modal.step),
           };
            db.GetSugarDbContext().Ado.ExecuteCommand(sql1, parameters1);

            sql1 = @"update IMES.P_NPI_FLOWSTEP set status='进行中', color='#50f637'   where  id=@id
                      and seq in (select seq+1 from IMES.P_NPI_FLOWSTEP where id=@id and step=@step )";
            parameters1 = new SugarParameter[]
          {
            new SugarParameter("@id", modal.id),
            new SugarParameter("@step",  modal.step),
          };
            return db.GetSugarDbContext().Ado.ExecuteCommand(sql1, parameters1);
            //批量删除不按主键
            

        }

        // 条件：例如，我们想要将Column2的值大于1的行着色为黄色
        Func<DataRow, string> getStatusColor = row =>
        {
            string status = row["STATUS"]?.ToString();
            return status == "已完成" ? "background-color: green;" :
                   status == "进行中" ? "background-color: yellow;" :
                   ""; // 默认不设置颜色，或者设置为其他颜色
        };
        public string ConvertToHtmlTable(DataTable table)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<table border='1'>");
            html.AppendLine("<tr>");

            // 添加表头
            foreach (DataColumn column in table.Columns)
            {
                string headerText = column.ColumnName; // 默认使用列名作为表头

                // 根据列名自定义表头文本
                if (column.ColumnName == "STATUS")
                {
                    headerText = "状态";
                }
                else if (column.ColumnName == "STEP")
                {
                    headerText = "节点";
                }
                else if (column.ColumnName == "UPDATE_EMPNO")
                {
                    headerText = "提交人员";
                }
                else if (column.ColumnName == "UPDATE_TIME")
                {
                    headerText = "提交时间";
                }
                else if (column.ColumnName == "ADVICE")
                {
                    headerText = "意见";
                }

                // 添加表头到HTML字符串中
                html.AppendLine("<th>" + headerText + "</th>");
            }

            html.AppendLine("</tr>");

            // 添加表数据
            foreach (DataRow row in table.Rows)
            {
                html.AppendLine("<tr>");
                foreach (var item in row.ItemArray)
                {
                    // 根据条件设置单元格的背景颜色
                    string cellColor = getStatusColor(row);
                    html.AppendLine("<td style='" + cellColor + "'>" + item.ToString() + "</td>");
                }
                html.AppendLine("</tr>");
            }

            html.AppendLine("</table>");
            html.AppendLine("<p>链接: <a href='" + "http://10.57.7.48:8887/business/NPI" + "'>Click Here</a></p>"); // 添加链接
            return html.ToString();
        }

        public int AddOrderInfo(OrderInfo modal)
        {
            //更新状态表为已完成
            //插入step info 内容    
            if (modal.step== "试制任务令PO释放")
            {
                for (int i = 0; i < modal.orders.Count; i++)
                {
                    string sql = @"INSERT INTO IMES.P_NPI_ORDER (ID,IPN,PLAN_QTY,LOT,WORK_ORDER,PO,ACTUAL_QTY,CREATE_EMPNO)
                               VALUES (@ID, @IPN, @PLAN_QTY, @LOT, @WORK_ORDER, @PO, @ACTUAL_QTY, @CREATE_EMPNO)";
                    var parameters = new SugarParameter[]
                    {
                  new SugarParameter("@ID", modal.id),
                  new SugarParameter("@IPN", modal.orders[i].ipn),
                  new SugarParameter("@PLAN_QTY", modal.orders[i].plan_qty),
                  new SugarParameter("@LOT",modal.orders[i].lot),
                  new SugarParameter("@WORK_ORDER", modal.orders[i].work_order),
                  new SugarParameter("@PO", modal.orders[i].po),
                  new SugarParameter("@ACTUAL_QTY", modal.orders[i].actual_qty),
                  new SugarParameter("@CREATE_EMPNO", modal.update_empno),
                     };
                    db.GetSugarDbContext().Ado.ExecuteCommand(sql, parameters);
                }
            }
            string sql1 = @"update IMES.P_NPI_FLOWSTEP set status='已完成' ,ADVICE=@ADVICE ,color='bule',update_time=@update_time ,update_empno=@update_empno where id=@id and step=@step";
            var parameters1 = new SugarParameter[]
           {
            new SugarParameter("@update_time", DateTime.Now),
            new SugarParameter("@ADVICE", modal.advice),
            new SugarParameter("@update_empno", modal.update_empno),
            new SugarParameter("@id", modal.id),
            new SugarParameter("@step",  modal.step),
           };
            db.GetSugarDbContext().Ado.ExecuteCommand(sql1, parameters1);

            sql1 = @"update IMES.P_NPI_FLOWSTEP set status='进行中', color='#50f637'   where  id=@id
                      and seq in (select seq+1 from IMES.P_NPI_FLOWSTEP where id=@id and step=@step )";
            parameters1 = new SugarParameter[]
          {
            new SugarParameter("@id", modal.id),
            new SugarParameter("@step",  modal.step),
          };
            return db.GetSugarDbContext().Ado.ExecuteCommand(sql1, parameters1);
            //批量删除不按主键


        }
        public string HashFileName(string str = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = Guid.NewGuid().ToString();
            }
            return BitConverter.ToString(MD5.HashData(Encoding.Default.GetBytes(str)), 4, 8).Replace("-", "");
        }

        /// <summary>
        /// 存储本地
        /// </summary>
        /// <param name="fileDir">存储文件夹</param>
        /// <param name="rootPath">存储根目录</param>
        /// <param name="fileName">自定文件名</param>
        /// <param name="formFile">上传的文件流</param>
        /// <param name="userName"></param>
        /// <param name="id">上传的文件流</param>
        /// <param name="step"></param>
        /// <returns></returns>
        public async Task<NpiProjetFile> SaveNpiFileToLocal(string rootPath, int id, string step, string fileName, string fileDir, string userName, IFormFile formFile)
        {
            string fileExt = Path.GetExtension(formFile.FileName);
            fileName = (fileName.IsEmpty() ? HashFileName() : fileName) + fileExt;

            string filePath = fileDir;
            //filePath += "\\palos\\ST0000\\A01\\01"; 
            string finalFilePath = Path.Combine(rootPath, filePath, fileName);
            double fileSize = Math.Round(formFile.Length / 1024.0, 2);

            if (!Directory.Exists(Path.GetDirectoryName(finalFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(finalFilePath));
            }

            using (var stream = new FileStream(finalFilePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            string uploadUrl = OptionsSetting.Upload.UploadUrl;
            string accessPath = string.Concat(uploadUrl, "/", filePath.Replace("\\", "/"), "/", fileName);

            string sql = @"update IMES.P_NPI_FLOWSTEP set url=url||@accessPath||';' where id=@id and step=@step ";
            var parameters = new SugarParameter[]
            {
            new SugarParameter("@accessPath", accessPath),
            new SugarParameter("@id", id),
            new SugarParameter("@step", step),
             };
            db.GetSugarDbContext().Ado.ExecuteCommand(sql, parameters);
            //SysFile file = new(formFile.FileName, fileName, fileExt, fileSize + "kb", filePath, userName)
            //{
            //    StoreType = (int)StoreType.LOCAL,
            //    FileType = formFile.ContentType,
            //    FileUrl = finalFilePath.Replace("\\", "/"),
            //    AccessUrl = accessPath
            //};
            //file.Id = await InsertFile(file);
            NpiProjetFile file = new NpiProjetFile
            {
                FileName = fileName,
                FileUrl = finalFilePath.Replace("\\", "/"),
                AccessUrl = accessPath
            };
            return file;
        }


        public string Getemails()
        {
            DataTable dt = new DataTable();
            //表调整
            /*string sql = @"select distinct  a.EMAIL from imes.SYS_USER a,
                            imes.SYS_USER_ROLE b,imes.SYS_ROLE c
                            where a.USERID=b.USER_ID
                            and b.ROLE_ID=c.ROLEID
                            and c.ROLENAME in ('试制任务令PO释放',
                            'NPI项目管理',
                            '研发试制准备-RD',
                            '工厂试制准备PE',
                            '工厂试制准备QE',
                            '试制准备完成',
                            '上传试制报告',
                            '工厂试制准备ME','工厂试制准备IE',
                            'NPI-查询') order by a.EMAIL";*/
            string sql = @"select distinct  a.EMAIL from imes.M_ZR_USER a,
                            imes.M_ZR_USER_ROLE b,imes.M_ZR_ROLE c
                            where a.MAIN_ID=b.USER_ID
                            and b.ROLE_ID=c.ROLEID
                            and c.ROLENAME in ('试制任务令PO释放',
                            'NPI项目管理',
                            '研发试制准备-RD',
                            '工厂试制准备PE',
                            '工厂试制准备QE',
                            '试制准备完成',
                            '上传试制报告',
                            '工厂试制准备ME','工厂试制准备IE',
                            'NPI-查询') order by a.EMAIL";

            dt = db.GetSugarDbContext().Ado.GetDataTable(sql);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                // 追加列的值到StringBuilder，并加上逗号
                sb.Append(row["EMAIL"].ToString());
                sb.Append(",");
            }

            // 移除最后一个逗号（如果有的话）
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
    }
}