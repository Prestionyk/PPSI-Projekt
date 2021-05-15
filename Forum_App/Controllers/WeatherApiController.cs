using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace Forum_App.Controllers
{
    public class WeatherApiController : Controller
    {
        private readonly HttpClient _client;
        private readonly ILogger<WeatherApiController> _logger;
        public WeatherApiController(HttpClient client, ILogger<WeatherApiController> logger)
        {
            _client = client;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string city)
        {
            if(city != null)
            {
                string name = city.Substring(0, 1).ToUpper() + city.Substring(1, city.Length - 1).ToLower();

                string link = $"https://api.openweathermap.org/data/2.5/weather?q={name}&APPID=53cbfca47c3fd27cc01e1ccc6f1ab1bb";
                var response = await _client.GetAsync(link);
                var result = await response.Content.ReadAsStringAsync();

                JObject obj = JObject.Parse(result);
                if(obj["cod"].ToString() == "404")
                {
                    _logger.LogError("City not found");
                    return NotFound();
                }
                double kelwin = double.Parse(obj["main"]["temp"]?.ToString());
                double temp = Math.Round(kelwin - 273.15, 2);

                Tuple<string, double> weather = new(name, temp);

                return View(nameof(ShowWeather), weather);
            }
            else
            {
                _logger.LogError("Wrong city to check weather");
                return View();
            }
        }

        public IActionResult ShowWeather(Tuple<string, double> weather)
        {
            _logger.LogInformation("Weather checked correctly");
            return View(weather);
        }
    }
}
