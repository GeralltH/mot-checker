using Microsoft.AspNetCore.Mvc;
using MOTChecker.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace MOTChecker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutMe()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult CheckMot(string registration)
        {
            GetRequest(registration);
            //GetRequest("wp70xfr");


            return View("Vehicle");
        }

        public async static void GetRequest(string registration)
        {
            var key = "fZi8YcjrZN1cGkQeZP7Uaa4rTxua8HovaswPuIno";

            using (HttpClient client = new HttpClient())
            {
                var baseUrl = "https://beta.check-mot.service.gov.uk/trade/vehicles/mot-tests?registration=";
                Uri uri = new Uri(baseUrl + registration);
                client.DefaultRequestHeaders.Add("x-api-key", key);

                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string JsonData = await content.ReadAsStringAsync();
                            LoadJson(JsonData);
                        }
                    }
                }
            }
        }

        public static void LoadJson(string results)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(results);
            //MemoryStream stream = new MemoryStream(byteArray);
            StreamReader reader = new StreamReader(new MemoryStream(byteArray));
            string text = reader.ReadToEnd();
            List<VehicleModel> vehicle = JsonConvert.DeserializeObject<List<VehicleModel>>(text);
            Console.WriteLine(vehicle[0].Make);
            Console.WriteLine(vehicle[0].Model);
            Console.WriteLine(vehicle[0].PrimaryColour);
            Console.WriteLine(vehicle[0].MotTests[0].expiryDate);
            Console.WriteLine(vehicle[0].MotTests[0].odometerValue + vehicle[0].MotTests[0].odometerUnit);
        }
    }
}