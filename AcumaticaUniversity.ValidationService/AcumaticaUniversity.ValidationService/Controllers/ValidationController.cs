using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AcumaticaUniversity.ValidationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post(ValidationDto validationDto)
        {
            using (var acumaticaRestClient = new AcumaticaRestClient())
            {
                var bills = acumaticaRestClient.GetBills();
                if (validationDto.LessonId == "6.1.2" && bills.Count > 1 && bills[1].Amount.value != 100)
                {
                    return BadRequest("Incorrect Total was entered");
                }

                return Ok();
            }
        }
    }

    public class ValidationDto
    {
        public string LessonId { get; set; }
    }

    public class AcumaticaRestClient : IDisposable
    {
        private HttpClient _httpClient;
        private string _acumaticaBaseUrl = "http://localhost/acumaticaerp";
        private string _username = "admin";
        private string _password = "Hack@2019";

        public AcumaticaRestClient()
        {
            Login();
        }

        public void Dispose()
        {
            Logout();
        }

        public List<dynamic> GetBills()
        {
            var httpResponseMessage = _httpClient.GetAsync($"{_acumaticaBaseUrl}/entity/default/17.200.001/Bill").GetAwaiter().GetResult();
            var result = httpResponseMessage.Content.ReadAsAsync<List<dynamic>>().GetAwaiter().GetResult();
            return result;
        }

        private void Login()
        {
            _httpClient = new HttpClient(new HttpClientHandler { UseCookies = true, CookieContainer = new CookieContainer() })
            {
                BaseAddress = new Uri($"{_acumaticaBaseUrl}/entity/Default/17.200.001"),
                DefaultRequestHeaders =
                {
                    Accept = {MediaTypeWithQualityHeaderValue.Parse("application/json")}
                }
            };

            var uri = $"{_acumaticaBaseUrl}/entity/auth/login";
            HttpResponseMessage res;

            try
            {
                res = _httpClient.PostAsJsonAsync(
                    uri, value: new
                    {
                        name = _username,
                        password = _password,
                    }).Result;
            }
            catch (Exception e)
            {
                Log.Error(e, "Login failed");
                throw new Exception("Login into Acumatica failed");
            }

            if (!res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                throw new Exception($"Login into Acumatica failed");
            }
        }

        private void Logout()
        {
            var uri = _acumaticaBaseUrl + "/entity/auth/logout";
            HttpResponseMessage res;
            try
            {
                res = _httpClient.PostAsync(uri, new ByteArrayContent(new byte[0])).Result;
            }
            catch (Exception e)
            {
                Log.Error(e, "Logout failed");
                throw new Exception("Logout failed");
            }

        }


    }
}
