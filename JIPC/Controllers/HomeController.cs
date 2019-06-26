using JIPC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace JIPC.Controllers
{
    public class HomeController : Controller
    {
        string BaseUrl = "https://api.johnspizza.com/";

        public async Task<ActionResult> Index()
        {
            var AssignmentModel = new Assignment();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("v1/Employment/Programmers/Assignment");

                if (response.IsSuccessStatusCode)
                {
                    var Res = response.Content.ReadAsStringAsync().Result;
                    var SubRes = Res.Substring(1, Res.Length - 2);
                    byte[] newBytes = Convert.FromBase64String(SubRes);
                    AssignmentModel.DecodedString = Encoding.UTF8.GetString(newBytes);
                }
            }
                return View(AssignmentModel);
        }

        [HttpPost]
        public async Task<ActionResult> PostString(Assignment assignment)
        {
            var AssignmentModel = new Assignment();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var content = new StringContent(assignment.DecodedString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("v1/Employment/Programmers/Assignment?lastName=Klingelberg", content);

                if (response.IsSuccessStatusCode)
                {
                    AssignmentModel.IsSuccess = true;
                    return View("PostString");
                }
                else
                {
                    throw new Exception("Something went wrong");
                }
            }
        }
    }
}