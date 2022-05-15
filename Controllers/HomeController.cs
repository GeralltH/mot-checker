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
        public ActionResult Vehicle(string registration)
        {
            var GetVehicleData = GetRequest(registration);
            
            VehicleDTO vehicleData = GetVehicleData.Result;

            if(vehicleData != null)
            {
                return View(vehicleData);
            }
            else
            {
                return View("Error");
            }
        }

        public async static Task<VehicleDTO> GetRequest(string registration)
        {
            var key = "fZi8YcjrZN1cGkQeZP7Uaa4rTxua8HovaswPuIno";

            using (HttpClient client = new HttpClient())
            {
                var baseUri = "https://beta.check-mot.service.gov.uk/trade/vehicles/mot-tests?registration=";
                Uri uri = new Uri(baseUri + registration);
                client.DefaultRequestHeaders.Add("x-api-key", key);

                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string JsonData = await content.ReadAsStringAsync();
                            return LoadJson(JsonData);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static VehicleDTO LoadJson(string results)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(results);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader reader = new StreamReader(stream);
            string data = reader.ReadToEnd();
            VehicleModel vehicle = JsonConvert.DeserializeObject<List<VehicleModel>>(data).FirstOrDefault();

            VehicleDTO vehicleData = new VehicleDTO();
            vehicleData.Registration = vehicle.Registration;
            vehicleData.Make = vehicle.Make;
            vehicleData.Model = vehicle.Model;
            vehicleData.Colour = vehicle.PrimaryColour;
            vehicleData.MotExpiryDate = vehicle.MotTests[0].expiryDate;
            vehicleData.LastMotMileage = vehicle.MotTests[0].odometerValue + vehicle.MotTests[0].odometerUnit;

            return vehicleData;
        }
    }
}