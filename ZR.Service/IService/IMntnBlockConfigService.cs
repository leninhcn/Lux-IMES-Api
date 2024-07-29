using System.Collections.Generic;
using System.Data;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Model.System.Vo;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Model;

namespace ZR.Service.IService
{
    /// <summary>
    /// 卡关配置服务接口
    /// </summary>
    public interface IMntnBlockConfigService 
    {
        //获取所有route信息分页
        List<Route> GetListRoute(string parm,string site);
        //获取所有Line信息分页
        List<MLine> GetListLine(string parm, string site);
        //获取所有StationType信息分页
        List<MStationType> GetListStationType(string parm,string site);
        //获取所有StationType信息分页
        List<Station> GetListStation(string parm,string site);
        //获取所有type信息分页
        PagedInfo<MBlockConfigTypeDto> GetListTypeFenye(MBlockConfigTypeQueryDto parm);
        //获取所有type信息
        List<MBlockConfigType> GetListType(MBlockConfigTypeQueryDto parm);
        //获取所有value信息
        List<MBlockConfigValue> GetListValue(MBlockConfigValueQueryDto parm);
        //获取所有信息
        List<MBlockConfigProsql> GetListSql(MBlockConfigProsqlQueryDto parm);
        //查询单个type详细信息
        MBlockConfigType GetInfoType(string Id);
        //查询单个value详细信息
        MBlockConfigValue GetInfoValue(string Id);
        //查询单个sql详细信息
        MBlockConfigProsql GetInfoSql(string Id);
        //新增type
        string AddType(MBlockConfigType parm);
        //新增value
        string AddValue(MBlockConfigValue parm);
        //新增sql
        string AddSql(MBlockConfigProsql parm);
        //修改type
        string UpdateType(MBlockConfigType parm);
        //修改value
        string UpdateValue(MBlockConfigValue parm);
        //修改sql
        string UpdateSql(MBlockConfigProsql parm);
        //删除类型
         string DeleteType(MBlockConfigTypeDto parm,string id);
        //删除配置
        string DeleteValue(MBlockConfigValueDto parm, string id);
        //删除SQL
        string DeleteSql(MBlockConfigProsqlDto parm, string id);
        List<MBlockConfigType> GetListSqlType(MBlockConfigTypeQueryDto parm);
    }
}
