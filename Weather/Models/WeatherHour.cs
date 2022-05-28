using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Models
{
    public class WeatherHour
    {
        public string Time { get; set; }
        public string Temperature { get; set; }
        public string Text { get; set; }
        public string Wind { get; set; }
        public string Precipitation { get; set; }
        public int Humidity { get; set; }
    }
}
