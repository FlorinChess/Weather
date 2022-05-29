using Newtonsoft.Json;

namespace Weather.Core.Models
{
    public class WeatherInformation
    {
        [JsonProperty(PropertyName = "location")]
        public Location location { get; set; }

        [JsonProperty(PropertyName ="current")]
        public Current current { get; set; }

        [JsonProperty(PropertyName ="forecast")]
        public Forecast forecast { get; set; }
    }

    public class Location
    {
        [JsonProperty(PropertyName = "country")]
        public string country { get; set; }

        [JsonProperty(PropertyName = "localtime")]
        public string localtime { get; set; }
    }

    public class Current
    {
        [JsonProperty(PropertyName = "last_updated")]
        public string last_updated { get; set; }

        [JsonProperty(PropertyName = "temp_c")]
        public decimal temp_c { get; set; }

        [JsonProperty(PropertyName = "temp_f")]
        public decimal temp_f { get; set; }

        [JsonProperty(PropertyName = "is_day")]
        public byte is_day { get; set; }

        [JsonProperty(PropertyName = "condition")]
        public Condition condition { get; set; }

        [JsonProperty(PropertyName = "wind_mph")]
        public decimal wind_mph { get; set; }

        [JsonProperty(PropertyName = "wind_kph")]
        public decimal wind_kph { get; set; }

        [JsonProperty(PropertyName = "wind_dir")]
        public string wind_dir { get; set; }

        [JsonProperty(PropertyName = "pressure_mb")]
        public decimal pressure_mb { get; set; }

        [JsonProperty(PropertyName = "precip_mm")]
        public decimal precip_mm { get; set; }

        [JsonProperty(PropertyName = "precip_in")]
        public decimal precip_in { get; set; }
    }

    public class Condition
    {
        [JsonProperty(PropertyName = "text")]
        public string text { get; set; }
    }

    public class Forecast
    {
        [JsonProperty(PropertyName ="forecastday")]
        public Forecastday[] forecastday { get; set; }
    }

    public class Forecastday
    {
        [JsonProperty(PropertyName = "day")]
        public Day day { get; set; }

        [JsonProperty(PropertyName = "hour")]
        public Hour[] hour { get; set; }
    }

    public class Day
    {
        [JsonProperty(PropertyName = "maxtemp_c")]
        public decimal maxtemp_c { get; set; }

        [JsonProperty(PropertyName = "maxtemp_f")]
        public decimal maxtemp_f { get; set; }

        [JsonProperty(PropertyName = "mintemp_c")]
        public decimal mintemp_c { get; set; }

        [JsonProperty(PropertyName = "mintemp_f")]
        public decimal mintemp_f { get; set; }

        [JsonProperty(PropertyName = "maxwind_mph")]
        public decimal maxwind_mph { get; set; }

        [JsonProperty(PropertyName = "maxwind_kph")]
        public decimal maxwind_kph { get; set; }

        [JsonProperty(PropertyName = "totalprecip_mm")]
        public decimal totalprecip_mm { get; set; }

        [JsonProperty(PropertyName = "totalprecip_in")]
        public decimal totalprecip_in { get; set; }

        [JsonProperty(PropertyName = "daily_chance_of_rain")]
        public string daily_chance_of_rain { get; set; }

        [JsonProperty(PropertyName = "condition")]
        public Condition condition { get; set; }
    }

    public class Hour
    {
        [JsonProperty(PropertyName = "time")]
        public string time { get; set; }

        [JsonProperty(PropertyName = "temp_c")]
        public decimal temp_c { get; set; }

        [JsonProperty(PropertyName = "temp_f")]
        public decimal temp_f { get; set; }

        [JsonProperty(PropertyName = "condition")]
        public Condition condition { get; set; }
    }
}
