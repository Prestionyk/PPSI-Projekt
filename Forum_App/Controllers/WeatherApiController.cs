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
            string name = city.Substring(0, 1).ToUpper() + city.Substring(1, city.Length-1).ToLower();

            string link = $"https://api.openweathermap.org/data/2.5/weather?q={name}&APPID=53cbfca47c3fd27cc01e1ccc6f1ab1bb";
            var response = await _client.GetAsync(link);
            var result = await response.Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(result);
            double kelwin = double.Parse(obj["main"]["temp"]?.ToString());
            double temp = Math.Round(kelwin - 273.15, 2);

            Tuple<string, double> weather = new(name, temp);

            return View(nameof(ShowWeather), weather); 
        }

        public IActionResult ShowWeather(Tuple<string, double> weather)
        {
            return View(weather);
        }
    }
}
