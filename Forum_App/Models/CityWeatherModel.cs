using System.ComponentModel.DataAnnotations;

namespace Forum_App.Models
{
    public class CityWeatherModel
    {
        [Required]
        public string Name;

        public double CelciusTemperature;
    }
}
