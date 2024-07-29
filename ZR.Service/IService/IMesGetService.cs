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
    /// MES通用服务接口
    /// </summary>
    public interface IMesGetService 
    {
        //获取所有route信息分页
        List<Route> GetListRoute(string parm,string site);
        //获取所有Line信息分页
        List<MLine> GetListLine(string parm, string site);
        //获取所有StationType信息分页
        List<MStationType> GetListStationType(string parm,string site);
        //获取所有StationType信息分页
        List<Station> GetListStation(string parm,string site);
        List<MPkspec> GetListPKSPEC(string parm, string site);
        List<MRuleSet> GetListRule(string parm, string site);
        List<dynamic> GetListWoType(string parm, string site);
        List<MDept> GetListDept(string parm, string site);
        List<MPart> GetListPart(string parm, string site);
        List<RouteDetail> GetInProcess(string parm, string site);
        List<RouteDetail> GetOutProcess(string parm, string site);
        List<string> GetModelByPart(string parm, string site);
        List<dynamic> GetListProcedures(string parm);
        List<dynamic> GetStationTree(string site);
        List<dynamic> GetStationTypeTree(string site);
        dynamic GetListModel(MesGetModel parm);
        dynamic GetListPartlabeltype(MesGetPart parm);
        dynamic GetListPrintField(MesGetPrintField parm);
        dynamic GetListLabelTypeByStation(MesGetLabelType parm);
        dynamic GetListLabelTypeBase(MesGetLabelTypeBase parm);
        dynamic GetListLabelStationType(MesGetLabelStationType parm);
        List<dynamic> GetSNInfo(MesGetSNInfo parm);
        List<dynamic> GetWoPanelInfo(MesGetSNInfo parm);
        List<MPart> GetPart(string parm, string site);
        Task<string> CheckWareHouse(MesCheckWareHouse parm);
        Task<dynamic> GetWareHouse(MesCheckWareHouse parm);
        Task<dynamic> GetEMP(MesGetEMP parm);
    }
}
