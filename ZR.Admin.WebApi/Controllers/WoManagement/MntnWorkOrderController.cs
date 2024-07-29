using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using ZR.Model.Business;
using ZR.Model.Dto;
using ZR.Service;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.WoManagement
{
    [Route("womanagement/mntnwoworkder")]
    [ApiController]
    public class MntnWorkOrderController : BaseController
    {
        private readonly IMntnWorkOrderService _mntnWorkOrderService;
        private readonly IMntnWoBomLinkService _mntnWoBomLinkService;
        public MntnWorkOrderController(IMntnWorkOrderService mntnWorkOrderService, IMntnWoBomLinkService mntnWoBomLinkService)
        {
            _mntnWorkOrderService = mntnWorkOrderService;
            _mntnWoBomLinkService = mntnWoBomLinkService;
        }
        [HttpGet("getall")]
        public IActionResult GetWoBaseList([FromQuery]PWoBaseQueryDto  parm)
        {
            parm.site=HttpContext.GetSite();
            var data =  _mntnWorkOrderService.GetWoBaseList(parm);
            return  SUCCESS(data, TIME_FORMAT_FULL);
        }
        [HttpGet("getwobasehistory")]
        public IActionResult GetWoBaseHistory([FromQuery] PWoBaseHTQueryDto parm)
        {
            parm.Site=HttpContext.GetSite();
            var data = _mntnWorkOrderService.GetWoBaseHistoryList(parm);
            return SUCCESS(data, TIME_FORMAT_FULL);
        }
        [HttpGet("getwostatuslog")]
        public IActionResult GetWoStatusLog([FromQuery] PWoBaseHTQueryDto parm)
        {
            parm.Site = HttpContext.GetSite();
            var data = _mntnWorkOrderService.GetWoStatusList(parm);
            return SUCCESS(data, TIME_FORMAT_FULL);
        }

        [HttpGet("getallToNg")]
        public IActionResult GetWoBaseListNg([FromQuery] PWoBaseQueryDto parm)
        {
            //parm.site=HttpContext.GetSite();
            var data = _mntnWorkOrderService.GetWoBaseListNg(parm);
            return SUCCESS(data, TIME_FORMAT_FULL);
        }

        [HttpGet("getwobomtree")]
        public IActionResult GetWoTree([FromQuery] PWoBomQueryDto parm)
        {
            parm.site=HttpContext.GetSite();
            var data = _mntnWoBomLinkService.GetWoBom(parm);
            var treeNodes = new List<ElTreeNodeWobom>();

            foreach (var item in data)
            {
                var id=item.Id;
                var ipn = item.Ipn;
                var stationType = item.StationType;
                var ItemIpn = item.ItemIpn;
                var ItemCount = item.ItemCount;
                var ItemGroup = item.ItemGroup;
                var vsersion= item.Version;
                var ItemSpec1= item.ItemSpec1;

                if (ItemGroup =="0")
                {
                    var lineNode = new ElTreeNodeWobom { ID=id, Label = ItemIpn, StationType = stationType,ItemCount=ItemCount,ItemGroup=ItemGroup,Version = vsersion,SPEC1=ItemSpec1 };
                    treeNodes.Add(lineNode);
                }
                else 
                {
                   var nodeone= treeNodes.Find(x => x.ItemGroup==ItemGroup);
                    if (nodeone!=null)
                    {
                        nodeone?.AddChild(new() { ID = id, Label = ItemIpn, StationType = stationType, ItemCount = ItemCount, ItemGroup = ItemGroup, Version = vsersion, SPEC1 = ItemSpec1 });
                    }
                    else
                    {
                        var lineNode = new ElTreeNodeWobom { ID = id, Label = ItemIpn, StationType = stationType, ItemCount = ItemCount, ItemGroup = ItemGroup, Version = vsersion, SPEC1 = ItemSpec1 };
                        treeNodes.Add(lineNode);
                    } 
                }
            }
            return SUCCESS(treeNodes, TIME_FORMAT_FULL);
        }

        [HttpPost("updateaddwobombywobase")]
        public IActionResult UpdateWoBomByWObase([FromBody] List<WoBom> parm)
        {
            WoBom swobom = new WoBom()
            {
                Site = HttpContext.GetSite(),
                UpdateEmpno =HttpContext.GetName(),
                UpdateTime=DateTime.Now
           };
            var data = _mntnWoBomLinkService.UpdatePWoBomByWO(parm,swobom);
            return ToResponse(new ApiResult(data == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, data == "OK" ? "sucess" : data, data));
        }
        [HttpPost("deletewobombywobase")]
        public IActionResult DeleteWoBomByWObase([FromBody] WoBom parm)
        {
            parm.Site = HttpContext.GetSite();
            parm.UpdateEmpno = HttpContext.GetName();
            var data = _mntnWoBomLinkService.DeletePWoBomByWo(parm);
            return ToResponse(new ApiResult((int)ResultCode.SUCCESS,"sucess" , data));
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        //[ActionPermissionFilter(Permission = "business:pwobase:add")]
        [Log(Title = "", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPWoBase([FromBody] WoBase parm)
        {
            //var modal = parm.Adapt<WoBase>().ToCreate(HttpContext);
            parm.Site=HttpContext.GetSite();
            var resultinfo = new ResultInfo();
             resultinfo.Result = _mntnWorkOrderService.AddPWoBase(parm);
            return ToResponse(new ApiResult(resultinfo.Result == "OK" ? (int)ResultCode.SUCCESS : (int)ResultCode.CUSTOM_ERROR, resultinfo.Result == "OK" ? "sucess" : resultinfo.Result, resultinfo.Result));
            //return SUCCESS(response);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPost("update")]
        //[ActionPermissionFilter(Permission = "business:pwobase:edit")]
        [Log(Title = "", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePWoBase([FromBody] WoBase parm)
        {
            //var modal = parm.Adapt<PWoBase>().ToUpdate(HttpContext);
            parm.Site = HttpContext.GetSite();
            parm.UpdateEmpno = HttpContext.GetName();
            parm.UpdateTime = DateTime.Now;
            var response = _mntnWorkOrderService.UpdatePWoBase(parm);
            return ToResponse(response);
        }
        /// <summary>
        /// 更新绑定设备
        /// </summary>
        /// <returns></returns>
        [HttpPost("updateequipment")]
        //[ActionPermissionFilter(Permission = "business:pwobase:edit")]
        [Log(Title = "", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePWoBaseEquipment([FromBody] WoBase parm)
        {
            //var modal = parm.Adapt<PWoBase>().ToUpdate(HttpContext);
            parm.Site = HttpContext.GetSite();
            parm.UpdateEmpno = HttpContext.GetName();
            parm.UpdateTime = DateTime.Now;
            var response = _mntnWorkOrderService.UpdatePWoBaseEquipment(parm);
            return ToResponse(response);
        }
        /// <summary>
        /// 用户导出
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("export")]
        [Log(Title = "用户导出", BusinessType = BusinessType.EXPORT)]
        // [ActionPermissionFilter(Permission = "business:pwobase:export")]
        public IActionResult WoBserExport([FromQuery] PWoBaseQueryDto parm)
        {
            parm.site = HttpContext.GetSite();
            var list = _mntnWorkOrderService.GetWoBaseList(parm);

            var result = ExportExcelMini(list.Result, "WoBse", "工单");
            return ExportExcel(result.Item2, result.Item1);
        }
    }
}
