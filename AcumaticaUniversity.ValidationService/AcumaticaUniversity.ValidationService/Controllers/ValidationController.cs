using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            using (var internalClient = new AcumaticaRestClient("http://localhost/acumaticaerp", "admin", "Hack@2019"))
            {
                var bills = internalClient.GetBills();
                if (validationDto.LessonId == "6.1.3" && bills[0].Amount.value != 30)
                {
                    return BadRequest("Incorrect Total was entered");
                }

                using (var externalClient = new AcumaticaRestClient("http://hackathon.acumatica.com/beta", "admin", "Password1!"))
                {
                    externalClient.Finish();
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

        public AcumaticaRestClient(string url, string username, string password)
        {
            this._acumaticaBaseUrl = url;
            this._username = username;
            this._password = password;
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

        public void Finish()
        {


            var body = @"
{

""EmployeeID"": {

""value"": ""EP00000002""

},

""Training"": [

{

""BAccountID"": {

""value"": 2892

},

""LessonCD"": {

""value"": ""Lesson6""

},

""LessonStep"": [

{

""BAccountID"": {

""value"": 2892

},

""LessonID"": {

""value"": 5

},

""LineNbr"": {

""value"": 2

},

""Completed"": {

""value"": true

}

}]

}]

}";
            var deserializeObject = JsonConvert.DeserializeObject(body);
            var httpResponseMessage = _httpClient.PutAsJsonAsync($"{_acumaticaBaseUrl}/entity/ETA/18.200.002/Employee?$expand=Training,Training/LessonStep", deserializeObject).GetAwaiter().GetResult();
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
