using Forum_App.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Forum_App.Controllers
{
    public class WeatherApiController : Controller
    {
        private readonly HttpClient _client;
        public WeatherApiController(HttpClient client)
        {
            _client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string city)
        {
            CityWeatherModel weather = new();
            weather.Name = city.ToUpper();

            string link = $"https://api.openweathermap.org/data/2.5/weather?q={weather.Name}&APPID=53cbfca47c3fd27cc01e1ccc6f1ab1bb";
            var response = await _client.GetAsync(link);
            var result = await response.Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(result);
            double kelwin = double.Parse(obj["main"]["temp"]?.ToString());
            weather.CelciusTemperature = Math.Round(kelwin - 273.15, 2);

            return View(nameof(ShowWeather), weather); 
        }

        public IActionResult ShowWeather(CityWeatherModel weather)
        {
            return View(weather);
        }
    }
}
