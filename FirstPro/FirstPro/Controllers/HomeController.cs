using FirstPro.Models;
using FirstPro.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace FirstPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly Setting setting;

        public HomeController(IOptions<Setting> _setting)
        {
            setting = _setting.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RedirectUrl()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = string.
                        Format(setting.api);
                    var OnlineCustRequest = new OnlineCustRequest
                    {
                        code = setting.code,
                        data = new OnlineCustRequestData
                        {
                            username = setting.username,
                            password = setting.password,
                            TargetAction = setting.TargetAction,
                            Language = setting.Language,
                            Redirecturl = setting.Redirecturl
                        }
                    };
                    HttpContent c = new StringContent(JsonConvert.SerializeObject(OnlineCustRequest),
                        Encoding.UTF8, "application/json");

                    var response = client.PostAsync(url, c).Result;

                    string responseAsString = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<OnlineCustRequest>(responseAsString);

                    return Redirect(result.data.url);
                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult AppplyNow()
        {
            return RedirectUrl();
        }

    }
}