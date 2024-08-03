using Infrastructure.Attribute;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZR.Infrastructure.Model;
using System.Drawing;
using System.IO;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto.ProdDto;
using ZR.Model.Quality;
using ZR.Repository;
using ZR.Service.IService;
using Org.BouncyCastle.Pqc.Crypto.Picnic;



namespace ZR.Service.PordService
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    internal class ProdMaintenanceService : BaseService<SnStatus>, IProdMaintenanceService
    {
        public PagedInfo<ImesMpartInscription> ShowData(String enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {

            /*Expression<Func<WeightFaiInfo, bool>> exp = Expressionable.Create<WeightFaiInfo>()
           .AndIF(site != "" && site != null, it => it.Site == site)
           .AndIF(enaBled != "" && enaBled != null && enaBled=="Y", it => it.Eabled == "Y")
           .AndIF(enaBled != "" && enaBled != null && enaBled == "N", it => it.Eabled == "N")
           .AndIF(optionData != "" && optionData != null, it =>SqlFunc.MappingColumn<string>(optionData).Contains(textData))
           .ToExpression();
            return Context.Queryable<WeightFaiInfo>().Where(exp);*/

            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMpartInscription>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "ipn", it => it.ipn.Contains(textData));
                exp.AndIF(optionData == "inscription", it => it.inscription.Contains(textData));
                exp.AndIF(optionData == "options", it => it.options.Contains(textData));
                exp.AndIF(optionData == "pic", it => it.pic.ToString().Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
            }
            var expPagedInfo = Context.Queryable<ImesMpartInscription>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
            return expPagedInfo;
        }

        public PagedInfo<ImesMpartInscription> MaintenanceList(String enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<ImesMpartInscription>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "ipn", it => it.ipn.Contains(textData));
            }
            var expPagedInfo = Context.Queryable<ImesMpartInscription>().Where(exp.ToExpression()).OrderBy(it => it.ipn).ToPage(pager);
            return expPagedInfo;
        }
        public List<ImesMpartInscription> History(string id, string site)
        {
            String sqlStr = $"select *  FROM SAJET.m_part_Inscription_ht b  where 1=1 ";

            if (id != null && id != "")
            {
                sqlStr += " and b.id ='" + id + "'";
            }
            if (site != null && site != "")
            {
                sqlStr += " and b.SITE ='" + site + "'";
            }
            List<ImesMpartInscription> list = Context.Ado.SqlQuery<ImesMpartInscription>(sqlStr);

            return list;
        }

        public object Maintenancepart(string ipn, string site)
        {
            string sql = string.Format(@"select id,ipn,apn,spec1,spec2 FROM SAJET.m_part where  SITE = '" + site + "'");
            if (!string.IsNullOrWhiteSpace(ipn))
                sql = sql + " AND ipn LIKE '%" + ipn + "%'";
            return Context.Ado.SqlQuery<Object>(sql);
        }
        public object Verification(string reelNo, string site)
        {
            string sql = string.Format(@"select IPN FROM SAJET.p_material where site = '" + site + "'");
            if (!string.IsNullOrWhiteSpace(reelNo))
                sql = sql + " AND REEL_NO = '" + reelNo + "'";
            return Context.Ado.SqlQuery<Object>(sql);
        }
        public int Validate(string ipn, string reelNo, string inscription, string site, long updateUserid)
        {
            string sql = string.Format(@"select * FROM SAJET.m_part_Inscription where site = '" + site + "' and  inscription = '" + inscription + "'and IPN= '" + ipn + "'");
            int count = Context.Ado.SqlQuery<Object>(sql).Count;
            string insertHt = $"Insert INTO SAJET.p_material_HT (Select * FROM SAJET.p_material  where REEL_NO='" + reelNo + "')";
            Context.Ado.SqlQuery<Object>(insertHt);
            string inscriptionVerification;
            if (count >= 1)
            {
                inscriptionVerification = "P";
                string sql1 = string.Format(@"update SAJET.p_material set INSCRIPTION_VERIFICATION ='" + inscriptionVerification + "',update_time = sysdate,UPDATE_USERID ='" + updateUserid + "'  where REEL_NO='" + reelNo + "'");
                Context.Ado.SqlQuery<object>(sql1);
                return 1;
            }
            else
            {
                inscriptionVerification = "F";
                string sql1 = string.Format(@"update SAJET.p_material set INSCRIPTION_VERIFICATION ='" + inscriptionVerification + "',update_time = sysdate,UPDATE_USERID ='" + updateUserid + "'  where REEL_NO='" + reelNo + "'");
                Context.Ado.SqlQuery<object>(sql1);
                return 2;
            }

        }

        public int MaintenanceInsert(ImesMpartInscription imesMpartInscription)
        {
            int count = Context.Queryable<ImesMpartInscription>().IgnoreColumns(it => it.inscription).Where(it => it.inscription == imesMpartInscription.inscription && it.ipn == imesMpartInscription.ipn).Count();
            if (count > 0)
            {
                return 0;
            }
            //FileStream fs = new FileStream(dialog.FileName, FileMode.Open);//将图片文件存在文件流中
            //long fslength = fs.Length;//流长度
            //byte[] bytearr = new byte[(int)fslength];//定义二进制数组
            //fs.Read(bytearr, 0, (int)fslength);//将流中字节写入二进制数组中
            //fs.Close();//关闭流
            //var ms = new MemoryStream(bytearr);
            //ms.Dispose();
            //ms.Close();
            //imesMpartInscription.pic = bytearr;
            string site = imesMpartInscription.site;
            int id = Context.Queryable<ImesMpartInscription>().Max(it => it.id) + 1;
            imesMpartInscription.id = id;
            imesMpartInscription.createTime = DateTime.Now;
            int insertErp = Context.Insertable(imesMpartInscription).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
            string insertHt = $"Insert INTO SAJET.m_part_Inscription_HT (Select * FROM SAJET.m_part_Inscription where id = " + id + ")";
            Context.Ado.SqlQuery<Object>(insertHt);
            return insertErp;
        }

        public int MaintenanceUpdate(ImesMpartInscription imesMpartInscription)
        {
            int id = imesMpartInscription.id;
            int Updateable = Context.Updateable(imesMpartInscription).IgnoreColumns(ignoreAllNullColumns: true).WhereColumns(it => new { id }).ExecuteCommand();
            string insertHt = $"Insert INTO SAJET.m_part_Inscription_HT (Select * FROM SAJET.m_part_Inscription where id = " + id + ")";
            Context.Ado.SqlQuery<Object>(insertHt);
            return Updateable;
        }

        public int MaintenanceDelete(ImesMpartInscription imesMpartInscription)
        {
            int id = imesMpartInscription.id;
            int updateStation = Context.Deleteable<ImesMpartInscription>().In(it => it.id, id).ExecuteCommand();
            string insertHt = $"Insert INTO SAJET.m_part_Inscription_HT (Select * FROM SAJET.m_part_Inscription where id = " + id + ")";
            Context.Ado.SqlQuery<string>(insertHt);
            return 1;
        }
        //public int VerificationInsert(Object json)
        //{
        //}


    }
}
