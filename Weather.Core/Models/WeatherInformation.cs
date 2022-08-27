namespace Weather.Core.Models
{
    public class WeatherInformation
    {
        public Location location { get; set; }
        public Current current { get; set; }
        public Forecast forecast { get; set; }
    }

    public class Location
    {
        public string country { get; set; }
        public string localtime { get; set; }
    }

    public class Current
    {
        public string last_updated { get; set; }
        public decimal temp_c { get; set; }
        public decimal temp_f { get; set; }
        public byte is_day { get; set; }
        public Condition condition { get; set; }
        public decimal wind_mph { get; set; }
        public decimal wind_kph { get; set; }
        public string wind_dir { get; set; }
        public decimal pressure_mb { get; set; }
        public decimal precip_mm { get; set; }
        public decimal precip_in { get; set; }
    }

    public class Condition
    {
        public string text { get; set; }
    }

    public class Forecast
    {
        public Forecastday[] forecastday { get; set; }
    }

    public class Forecastday
    {
        public Day day { get; set; }
        public Hour[] hour { get; set; }
    }

    public class Day
    {
        public decimal maxtemp_c { get; set; }
        public decimal maxtemp_f { get; set; }
        public decimal mintemp_c { get; set; }
        public decimal mintemp_f { get; set; }
        public decimal maxwind_mph { get; set; }
        public decimal maxwind_kph { get; set; }
        public decimal totalprecip_mm { get; set; }
        public decimal totalprecip_in { get; set; }
        public int daily_chance_of_rain { get; set; }
        public Condition condition { get; set; }
    }

    public class Hour
    {
        public string time { get; set; }
        public decimal temp_c { get; set; }
        public decimal temp_f { get; set; }
        public Condition condition { get; set; }
    }
}
