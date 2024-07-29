using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Dynamic;
using System.Text.Json;
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
    [Route("quality/dppm")]
    public class MMaterialdppmController : BaseController
    {
        /// <summary>
        /// 接口
        /// </summary>
        private readonly IMMaterialdppmService _MMaterialdppmService;

        public MMaterialdppmController(IMMaterialdppmService MMaterialdppmService)
        {
            _MMaterialdppmService = MMaterialdppmService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "quality:dppm:list")]
        public IActionResult QueryMMaterialdppm([FromQuery] MMaterialdppmQueryDto parm)
        {
            var response = _MMaterialdppmService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询机种列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("models")]
        [ActionPermissionFilter(Permission = "quality:dppm:list")]
        public IActionResult QueryModels([FromQuery] MMaterialdppmQueryDto parm)
        {
            var response = _MMaterialdppmService.GetModels();
            //datatable转成行对象数组
            var res = response.AsEnumerable().Select(r => {
                var cols = response.Columns;
                var exobj = new ExpandoObject();
                for (int i = 0; i < r.ItemArray.Length; i++)
                {
                    exobj.TryAdd(cols[i].ColumnName, r.ItemArray[i].ToString());
                }
                return exobj;
            });
            return SUCCESS(res);
        }



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "quality:dppm:query")]
        public IActionResult GetMMaterialdppm(int Id)
        {
            var response = _MMaterialdppmService.GetInfo(Id);

            var info = response.Adapt<MMaterialdppm>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "quality:dppm:add")]
        [Log(Title = "", BusinessType = BusinessType.INSERT)]
        public IActionResult AddMMaterialdppm([FromBody] MMaterialdppmDto parm)
        {
            var modal = parm.Adapt<MMaterialdppm>().ToCreate(HttpContext);

            var response = _MMaterialdppmService.AddMMaterialdppm(modal);

            return SUCCESS(response);
        }



        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        [ActionPermissionFilter(Permission = "quality:dppm:add")]
        [Log(Title = "", BusinessType = BusinessType.INSERT)]
        public IActionResult UploadFile([FromForm] UploadFileDto parm)
        {
            var file = parm.File;
            var ms = new MemoryStream();
            file.CopyTo(ms);
            var model = new MMaterialdppm();
            model.Id =parm.Id;
            model.Report =file.FileName;
            model.ReportFile = ms.ToArray();
            var response = model;
            if (parm.Id == 0)
            {
                response = _MMaterialdppmService.AddMMaterialdppm(model);
            }
            if (parm.Id != 0)
            {
                _MMaterialdppmService.UpdateMMaterialdppm(model);
                response = model;
            }

            return SUCCESS(response);
        }



        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "quality:dppm:edit")]
        [Log(Title = "", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateMMaterialdppm([FromBody] MMaterialdppmDto parm)
        {
            var modal = parm.Adapt<MMaterialdppm>().ToUpdate(HttpContext);
            var response = _MMaterialdppmService.UpdateMMaterialdppm(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "quality:dppm:delete")]
        [Log(Title = "", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteMMaterialdppm(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _MMaterialdppmService.Delete(idsArr);

            return ToResponse(response);
        }




    }




}
