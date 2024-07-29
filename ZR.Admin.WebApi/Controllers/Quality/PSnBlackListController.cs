using Microsoft.AspNetCore.Mvc;
using ZR.Admin.WebApi.Filters;
using ZR.Model.Dto.Quality;
using ZR.Model.Quality;
using ZR.Service.Quality.IQualityService;

namespace ZR.Admin.WebApi.Controllers.Quality
{

    /// <summary>
    /// 
    /// </summary>
    [Verify]
    [Route("quality/blacklist")]
    public class PSnBlackListController : BaseController
    {
        /// <summary>
        /// 接口
        /// </summary>
        private readonly IPSnBlackListService _PSnBlackListService;

        public PSnBlackListController(IPSnBlackListService PSnBlackListService)
        {
            _PSnBlackListService = PSnBlackListService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "quality:blacklist:list")]
        public IActionResult QueryPSnBlackList([FromQuery] PSnBlackListQueryDto parm)
        {
            var response = _PSnBlackListService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "quality:blacklist:query")]
        public IActionResult GetPSnBlackList(int Id)
        {
            var response = _PSnBlackListService.GetInfo(Id);

            var info = response.Adapt<PSnBlackList>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "quality:blacklist:add")]
        [Log(Title = "", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPSnBlackList([FromBody] PSnBlackListDto parm)
        {
            var modal = parm.Adapt<PSnBlackList>().ToCreate(HttpContext);

            var response = _PSnBlackListService.AddPSnBlackList(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "quality:blacklist:edit")]
        [Log(Title = "", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePSnBlackList([FromBody] PSnBlackListDto parm)
        {
            var modal = parm.Adapt<PSnBlackList>().ToUpdate(HttpContext);
            var response = _PSnBlackListService.UpdatePSnBlackList(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "quality:blacklist:delete")]
        [Log(Title = "", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePSnBlackList(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _PSnBlackListService.Delete(idsArr);

            return ToResponse(response);
        }




    }




}
