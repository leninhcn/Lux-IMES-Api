using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Cache;
using System.Net;
using System.Text;
using ZR.Service.IService;

namespace ZR.Admin.WebApi.Controllers.ProdMnt
{
    [Route("prodMnt/prodVI/[action]")]
    [ApiController]
    public class ProdVIController : BaseController
    {
        readonly IProdVIService service;
        public ProdVIController(IProdVIService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetMarinaStation()
        {
            return Ok(new { yes = await service.GetMarinaStation() });
        }

        async Task<string> ForwardMarinaCheck(string sn, string station)
        {
            string Res = "";
            var _ResEncoding = Encoding.UTF8;
            Encoding encoding = Encoding.UTF8;
            string selectedItem = "POST";
            string text = "";

            //QA:172.30.70.62  产线:10.42.24.62
            string IP = "";
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    IP = _IPAddress.ToString();
                }
            }
            if (IP.Substring(0, 5) == "10.42")
            {
                text = "http://172.30.70.160/MABCATA01/Bobcat.aspx";
                //text = "http://10.42.24.62/MABCATA01/Bobcat.aspx";
            }
            else
            {
                text = "http://172.30.70.160/MABCATA01/Bobcat.aspx";
                //text = "http://172.30.70.62/MABCATA01/Bobcat.aspx";
            }
            string s = "c=QUERY_RECORD&sn=" + sn + "&tsid=" + station + "&p=MARINA_SERVER_CHECK";
            string str4 = "application/x-www-form-urlencoded";

            try
            {
                HttpWebRequest request = WebRequest.Create(text) as HttpWebRequest;
                request.Method = selectedItem;
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                if (0 == string.Compare("POST", selectedItem))
                {
                    byte[] bytes = encoding.GetBytes(s);
                    request.ContentType = str4;
                    request.ContentLength = bytes.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                try
                {
                    Encoding encoding2 = null;
                    encoding2 = _ResEncoding;
                    Debug.WriteLine(encoding2);
                    using Stream stream2 = response.GetResponseStream();
                    using StreamReader reader = new(stream2, encoding2);
                    Res = await reader.ReadToEndAsync();
                }
                finally
                {
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                Res = ex + Res;
            }
            return Res;
        }

        [HttpGet]
        public async Task<IActionResult> MarinaCheck(string sn, string station)
        {
            return Ok(new { data = await ForwardMarinaCheck(sn, station) });
        }

        [HttpGet]
        public async Task<IActionResult> CheckErrorCode(string errorCode)
        {
            return Ok(new { yes = await service.CheckErrorCode(errorCode) });
        }
    }
}
