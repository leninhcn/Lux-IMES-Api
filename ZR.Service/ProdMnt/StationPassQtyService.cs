using Infrastructure.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model.Dto.Quality;
using ZR.Repository;
using ZR.Service.IService;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace ZR.Service.ProdMnt
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class StationPassQtyService : BaseService<SnStatus>, IStationPassQtyService
    {
        public List<string> GetLineInfo(string site)
        {

            String sqlStr = $"select distinct b.line FROM SAJET.m_line a, imes.m_station b  where a.LINE = b.LINE ";

            if (site != null && site != "")
            {
                sqlStr += " and b.SITE ='" + site + "'";
            }
            sqlStr = sqlStr + " order by b.line  ";
            List<string> list = Context.Ado.SqlQuery<string>(sqlStr);
            return list;
        }
        public List<string>  GetStationTypeInfo(string line,string site)
        {
            Console.WriteLine(line, site);
            String sqlStr = $"SELECT DISTINCT b.STATION_TYPE FROM SAJET.m_station b WHERE 1=1  ";

            if (site != null && site != "")
            {
                sqlStr += " and b.SITE ='" + site+"'";
            }
            if (line != null && line != "")
            {
                sqlStr += " and b.LINE ='" + line + "'"; ;
            }
            sqlStr = sqlStr + "  order by b.STATION_TYPE  ";
            List<string> list = Context.Ado.SqlQuery<string>(sqlStr);
            return list;

        }
        public List<string> GetStationNameInfo(string line, string stationType, string site)
        {
            String sqlStr = $"select b.STATION_NAME from  imes.m_station b  where 1=1 ";

            if (line != null&& line !="")
            {
                sqlStr += " and b.LINE ='" + line + "'";
            }
            if (stationType != null && stationType !="" )
            {
                sqlStr += " and b.STATION_TYPE ='" + stationType + "'"; ;
            }
            if (site != null && site!="")
            {
                sqlStr += " and b.SITE ='" + site + "'";
            }
            sqlStr = sqlStr + "  order by b.STATION_NAME    ";
            List<string> list = Context.Ado.SqlQuery<string>(sqlStr);
            return list;

        }
        public PagedInfo<StationInfos>  GetStationInfo(string line, string stationType, string stationName, string site, int pageNum, int pageSize)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<StationInfos>();
            exp.AndIF(line != "" && line != null, it => it.line == line);
            exp.AndIF(stationType != "" && stationType != null, it => it.stationType == stationType);
            exp.AndIF(stationName != "" && stationName != null, it => it.stationName == stationName);
            exp.AndIF(site != "" && site != null, it => it.site == site);
            var expPagedInfo = Context.Queryable<StationInfos>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
            return expPagedInfo;

        }

        public int UpdateStationInfo(StationInfos stationInfos)
        {
           int updateStation =  Context.Updateable(stationInfos).UpdateColumns(it => new { it.maxQty, it.fallQty, it.passQty,it.updateEmpno,it.updateTime }).WhereColumns(it => new { it.id, it.site }).ExecuteCommand();
            if (updateStation>0) 
            {
                string sqlStr = $"insert INTO SAJET.m_station_ht(select * FROM SAJET.m_station  where id = '" + stationInfos.id + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
           return updateStation;
        }

        public PagedInfo<WeightFaiInfo> ShowData(String enaBled, string optionData, string textData, int pageNum, int pageSize, string site)
        {
            PagerInfo pager = new PagerInfo();
            pager.PageNum = pageNum;
            pager.PageSize = pageSize;
            var exp = Expressionable.Create<WeightFaiInfo>();
            exp.AndIF(site != "" && site != null, it => it.site == site);
            exp.AndIF(enaBled != "" && enaBled != null, it => it.enabled == enaBled);
            if (textData != null && textData != "")
            {
                exp.AndIF(optionData == "type", it => it.type.Contains(textData));
                exp.AndIF(optionData == "ipn", it => it.ipn.Contains(textData));
                exp.AndIF(optionData == "mpn", it => it.mpn.Contains(textData));
                exp.AndIF(optionData == "faiValue", it => it.faiValue.Contains(textData));
                exp.AndIF(optionData == "unit", it => it.unit.Contains(textData));
                exp.AndIF(optionData == "updateEmpno", it => it.updateEmpno.Contains(textData));
            }
           var expPagedInfo =  Context.Queryable<WeightFaiInfo>().Where(exp.ToExpression()).OrderBy(it => it.createTime).ToPage(pager);
            return expPagedInfo;
        }
        public int InsertWeightFai(WeightFaiInfo weightFaiInfo, string site)
        {
            int MaxId = Context.Queryable<WeightFaiInfo>().Max(it => it.id) + 1;
            weightFaiInfo.id = MaxId;
            weightFaiInfo.site = site;
            weightFaiInfo.enabled = "Y";
            weightFaiInfo.createTime = DateTime.Now;
            weightFaiInfo.updateTime = DateTime.Now;
            int insertErp = Context.Insertable(weightFaiInfo).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();

            if (insertErp > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_WEIGHT_FAI_ht(select * FROM SAJET.M_WEIGHT_FAI where id = " + MaxId;
                Context.Ado.SqlQuery<string>(sqlStr + ")");
                return 1;
            }
            return insertErp;
        }
        public int ExistIPN(string ipn)
        {
            String sqlStr = $"select IPN FROM SAJET.m_part  where 1=1 ";

            if (ipn != null && ipn != "")
            {
                sqlStr += " and IPN  ='" + ipn + "'";
            }
            int ipnCount = Context.Ado.SqlQuery<string>(sqlStr).Count;
            return ipnCount;
        }
        public int UpdateWeightFai(WeightFaiInfo weightFaiInfo, string site)
        {
            int updateStation = Context.Updateable(weightFaiInfo).UpdateColumns(it => new { it.type, it.mpn, it.ipn,it.faiValue,it.unit }).WhereColumns(it => new { it.id,site}).ExecuteCommand();
            if (updateStation > 0)
            {
                string sqlStr = $"insert INTO SAJET.M_WEIGHT_FAI_ht(select * FROM SAJET.M_WEIGHT_FAI  where id = '" + weightFaiInfo.id + "')";
                Context.Ado.SqlQuery<string>(sqlStr);
                return 1;
            }
            return updateStation;
        }
       
        public int UpdateWeightFaiState(WeightFaiInfo weightFaiInfo, string site)
        {
            if (weightFaiInfo.enabled == "Y")
            {
                int updateStation = Context.Updateable(weightFaiInfo).UpdateColumns(it => it.enabled == "N").WhereColumns(it =>it.id).ExecuteCommand();
                if (updateStation > 0)
                {
                    string sqlStr = $"insert INTO SAJET.M_WEIGHT_FAI_ht(select * FROM SAJET.M_WEIGHT_FAI  where id = '" + weightFaiInfo.id + "')";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return 1;
                }
            }
            else
            {
                int updateStation = Context.Updateable(weightFaiInfo).UpdateColumns(it => it.enabled == "Y").WhereColumns(it => it.id).ExecuteCommand();
                if (updateStation > 0)
                {
                    string sqlStr = $"insert INTO SAJET.M_WEIGHT_FAI_ht(select * FROM SAJET.M_WEIGHT_FAI   where id = '" + weightFaiInfo.id + "')";
                    Context.Ado.SqlQuery<string>(sqlStr);
                    return 1;
                }
            }         
            return 0;
        }

        public List<WeightFaiInfo> History(string id, string site)

        {
            String sqlStr = $"select *  FROM SAJET.M_WEIGHT_FAI_ht b  where 1=1 ";

            if (id != null && id != "")
            {
                sqlStr += " and b.id ='" + id + "'";
            }
            if (site != null && site != "")
            {
                sqlStr += " and b.SITE ='" + site + "'";
            }
            List<WeightFaiInfo> list = Context.Ado.SqlQuery<WeightFaiInfo>(sqlStr);

            return list;
        }

        public int DeleteWeightFai(int id, string site)
        {
            string sqlStr = $"insert INTO SAJET.M_WEIGHT_FAI_ht(select * FROM SAJET.M_WEIGHT_FAI  where id = " + id + ")";
            Context.Ado.SqlQuery<string>(sqlStr);            
            Console.WriteLine("id:"+id);
           return Context.Deleteable<WeightFaiInfo>().Where(it => it.id == id && it.site == site).ExecuteCommand();
        }
    }
}
