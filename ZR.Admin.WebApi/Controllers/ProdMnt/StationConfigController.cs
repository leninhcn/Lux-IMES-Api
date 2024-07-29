using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Service;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.ProdMnt
{
    [Route("prodMnt/stationConfig/[action]")]
    [ApiController]
    public class StationConfigController : BaseController
    {
        readonly IStationConfigService service;
        public StationConfigController(IStationConfigService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetSites()
        {
            return Ok(await service.GetSites());
        }

        public class SitesReq
        {
            public string[] Sites { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> GetSitesWith([FromBody] SitesReq req)
        {
            return Ok(await service.GetSites(req.Sites));
        }

        public class StationReq
        {
            public string Site { get; set; }
            public string[] OpTypes { get; set; }
            public string[] ClientTypes { get; set; }
          
        }
        public class StationReqT
        {
            public string Site { get; set; }
            public string[] OpTypes { get; set; }
            public string[] ClientTypes { get; set; }
            public string StationName { get; set; }
        }


        [HttpPost]
        public async Task<IActionResult> GetStations([FromBody] StationReq req)
        {
            var site = req.Site;
            var opTypesArr = req.OpTypes.Select(x => x.ToUpper()).ToArray();
            var clientTypesArr = req.ClientTypes.Select(x => x.ToUpper()).ToArray();

            var stationInfo = await service.GetStationInfo(site, opTypesArr, clientTypesArr)
                .Select((a, b, c, d, e) => new
                {
                    b.Site,
                    b.Line,
                    c.Stage,
                    d.StationType,
                    a.StationName
                }).ToListAsync();

            if (stationInfo.Count == 0)
            {
                return Ok(new { msg = site + "未维护线别站点信息！请确认！" });
            }

            string preLine = "";
            string preStage = "";
            string preStationType = "";

            var treeNodes = new List<ElTreeNode>();

            foreach (var station in stationInfo)
            {
                var line = station.Line;
                var stage = station.Stage;
                var stationType = station.StationType;
                var stationName = station.StationName;

                if (preLine != line)
                {
                    var lineNode = new ElTreeNode { Label = line, IconIndex = 0 };
                    treeNodes.Add(lineNode);

                    lineNode.AddChild(new() { Label = stage, IconIndex = 1 })
                        .AddChild(new() { Label = stationType, IconIndex = 2 })
                        .AddChild(new() { Label = stationName, IconIndex = 3 });
                }
                else if (preStage != stage)
                {
                    var lineNode = treeNodes.LastOrDefault();

                    lineNode?.AddChild(new() { Label = stage, IconIndex = 1 })
                        .AddChild(new() { Label = stationType, IconIndex = 2 })
                        .AddChild(new() { Label = stationName, IconIndex = 3 });
                }
                else if (preStationType != stationType)
                {
                    var stageNode = treeNodes.LastOrDefault()?.Last;

                    stageNode?.AddChild(new() { Label = stationType, IconIndex = 2 })
                        .AddChild(new() { Label = stationName, IconIndex = 3 });
                }
                else
                {
                    var stationTypeNode = treeNodes.LastOrDefault()?.Last?.Last;
                    stationTypeNode?.AddChild(new() { Label = stationName, IconIndex = 3 });
                }
                preLine = line;
                preStage = stage;
                preStationType = stationType;
            }

            return Ok(treeNodes);
        }


        //[AllowAnonymousAttribute]
        //public async Task<IActionResult> GetStationsT([FromBody] StationReq req)
        //{



        //    var site = req.Site;
        //    var opTypesArr = req.OpTypes.Select(x => x.ToUpper()).ToArray();
        //    var clientTypesArr = req.ClientTypes.Select(x => x.ToUpper()).ToArray();

        //    var stationInfo = await service.GetStationInfo(site, opTypesArr, clientTypesArr)
        //        .Select((a, b, c, d, e) => new
        //        {
        //            b.Site,
        //            b.Line,
        //            c.Stage,
        //            d.StationType,
        //            a.StationName
        //        }).ToListAsync();

        //    if (stationInfo.Count == 0)
        //    {
        //        return Ok(new { msg = site + "未维护线别站点信息！请确认！" });
        //    }

        //    string preLine = "";
        //    string preStage = "";
        //    string preStationType = "";

        //    var treeNodes = new List<ElTreeNode>();

        //    foreach (var station in stationInfo)
        //    {
        //        var line = station.Line;
        //        var stage = station.Stage;
        //        var stationType = station.StationType;
        //        var stationName = station.StationName;

        //        if (preLine != line)
        //        {
        //            var lineNode = new ElTreeNode { Label = line, IconIndex = 0 };
        //            treeNodes.Add(lineNode);

        //            lineNode.AddChild(new() { Label = stage, IconIndex = 1 })
        //                .AddChild(new() { Label = stationType, IconIndex = 2 })
        //                .AddChild(new() { Label = stationName, IconIndex = 3 });
        //        }
        //        else if (preStage != stage)
        //        {
        //            var lineNode = treeNodes.LastOrDefault();

        //            lineNode?.AddChild(new() { Label = stage, IconIndex = 1 })
        //                .AddChild(new() { Label = stationType, IconIndex = 2 })
        //                .AddChild(new() { Label = stationName, IconIndex = 3 });
        //        }
        //        else if (preStationType != stationType)
        //        {
        //            var stageNode = treeNodes.LastOrDefault()?.Last;

        //            stageNode?.AddChild(new() { Label = stationType, IconIndex = 2 })
        //                .AddChild(new() { Label = stationName, IconIndex = 3 });
        //        }
        //        else
        //        {
        //            var stationTypeNode = treeNodes.LastOrDefault()?.Last?.Last;
        //            stationTypeNode?.AddChild(new() { Label = stationName, IconIndex = 3 });
        //        }
        //        preLine = line;
        //        preStage = stage;
        //        preStationType = stationType;
        //    }

        //    return Ok(treeNodes);
        //}


        /**
         * 反查站点等信息通过子节点
         */
        [HttpPost]
        [AllowAnonymousAttribute]
        public   async Task<IActionResult> NodeTree([FromBody] StationReqT req)
        {
            var site = req.Site;
            var opTypesArr = req.OpTypes.Select(x => x.ToUpper()).ToArray();
            var clientTypesArr = req.ClientTypes.Select(x => x.ToUpper()).ToArray();

            var stationInfo = await service.GetStationInfoBychildren(site, opTypesArr, clientTypesArr, req.StationName )
                .Select((a, b, c, d, e) => new
                {
                    b.Site,
                    b.Line,
                    c.Stage,
                    d.StationType,
                    a.StationName
                }).ToListAsync();

            if (stationInfo.Count == 1)
            {
                return SUCCESS(stationInfo);

            }
            return Ok(new { msg = site + "未维护线别站点信息！请确认！" });
        }



    }
}
