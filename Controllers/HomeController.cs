using Microsoft.AspNetCore.Mvc;
using MOTChecker.Models;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

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

        [HttpPost]
        public ActionResult Vehicle(string registration)
        {
            if (!string.IsNullOrEmpty(registration))
            {
                registration = registration.Replace(" ", string.Empty);

                if (ValidInput(registration))
                {
                    var GetVehicleData = GetRequest(registration);

                    VehicleDTO vehicleData = GetVehicleData.Result;

                    if (vehicleData != null)
                    {
                        return View(vehicleData);
                    }
                    return View("Error");
                }
            }
            return View("InputError");
        }

        public bool ValidInput(string registration)
        {
            var regex = "^[a-zA-Z0-9]*$";

            bool validLength = registration.Length > 1 & registration.Length < 8;
            bool validCharacters = Regex.IsMatch(registration, regex);

            return validLength && validCharacters;
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
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        //vehicle not found
                    }
                    return null;
                }
            }
        }

        public static VehicleDTO LoadJson(string results)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(results);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader reader = new StreamReader(stream);
            string data = reader.ReadToEnd();

            VehicleModel motResults = JsonConvert.DeserializeObject<List<VehicleModel>>(data).FirstOrDefault();
            VehicleDTO vehicleData = new VehicleDTO();

            vehicleData.Registration = motResults.Registration;
            vehicleData.Make = motResults.Make;
            vehicleData.Model = motResults.Model;
            vehicleData.Colour = motResults.PrimaryColour;
            if (motResults.MotTests == null)
            {
                vehicleData.MotExpiryDate = motResults.MotTestExpiryDate;
                vehicleData.LastMotMileage = "N/A";
            }
            else
            {
                vehicleData.MotExpiryDate = motResults.MotTests[0].expiryDate;
                vehicleData.LastMotMileage = motResults.MotTests[0].odometerValue + motResults.MotTests[0].odometerUnit;
            }

            return vehicleData;
        }

    }
}