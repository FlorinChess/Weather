using System.Threading.Tasks;
using Weather.Core.Models;

namespace Weather.Core
{
    public interface IWeatherClient
    {
        public ValueTask<WeatherInformation> GetWeather(string location, string lang = "en");
    }
}
