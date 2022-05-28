using System;
using System.Globalization;
using System.Windows.Data;

namespace Weather.Converters
{
    public class StringToSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Mapping all possible API responses to available icons
            string imageSource = "pack://application:,,,/Icons/sunny.png";

            if ((string)value == "Sunny" || (string)value == "Mostly sunny")
            {
                // sunny
                imageSource = "pack://application:,,,/Icons/sunny.png";
            }
            else if ((string)value == "Clear")
            {
                // moon
                imageSource = "pack://application:,,,/Icons/clear.png";
            }
            else if ((string)value == "Partly cloudy")
            {
                // light clouds with sun
                imageSource = "pack://application:,,,/Icons/partly_cloudy.png";
            }
            else if ((string)value == "Cloudy" || (string)value == "Overcast")
            {
                // light clouds
                imageSource = "pack://application:,,,/Icons/cloudy.png";
            }
            else if ((string)value == "Patchy snow possible")
            {
                // light snow with sun
                imageSource = "pack://application:,,,/Icons/patchy_snow.png";
            }
            else if ((string)value == "Thundery outbreaks possible")
            {
                // thunder with sun
                imageSource = "pack://application:,,,/Icons/thundery.png";
            }
            else if ((string)value == "Fog" || (string)value == "Freezing fog" || (string)value == "Mist")
            {
                // heavy clouds without sun
                imageSource = "pack://application:,,,/Icons/mist.png";
            }
            else if ((string)value == "Patchy light rain" || (string)value == "Moderate rain at times" || (string)value == "Light rain shower" || (string)value == "Patchy rain possible" || (string)value == "Patchy light drizzle" || (string)value == "Patchy sleet possible")
            {
                // light rain with sun
                imageSource = "pack://application:,,,/Icons/light_patchy_rain.png";
            }
            else if ((string)value == "Light rain" || (string)value == "Moderate rain" || (string)value == "Light freezing rain" || (string)value == "Light sleet" || (string)value == "Light sleet showers" || (string)value == "Light drizzle" || (string)value == "Freezing drizzle")
            {
                // light rain without sun
                imageSource = "pack://application:,,,/Icons/light_rain.png";
            }
            else if ((string)value == "Heavy rain at times" || (string)value == "Moderate or heavy rain shower" || (string)value == "Torrential rain shower")
            {
                // heavy rain with sun
                imageSource = "pack://application:,,,/Icons/heavy_patchy_rain.png";
            }
            else if ((string)value == "Heavy rain" || (string)value == "Moderate or heavy freezing rain" || (string)value == "Moderate or heavy sleet" || (string)value == "Moderate or heavy sleet showers" || (string)value == "Heavy freezing drizzle")
            {
                // heavy rain without sun
                imageSource = "pack://application:,,,/Icons/heavy_rain.png";
            }
            else if ((string)value == "Patchy light snow" || (string)value == "Patchy moderate snow" || (string)value == "Light snow showers" || (string)value == "Patchy freezing drizzle possible")
            {
                // light snow with sun
                imageSource = "pack://application:,,,/Icons/patchy_snow.png";
            }
            else if ((string)value == "Light snow" || (string)value == "Moderate snow" || (string)value == "Ice pellets" || (string)value == "Light showers of ice pellets")
            {
                // light snow without sun
                imageSource = "pack://application:,,,/Icons/snow.png";
            }
            else if ((string)value == "Patchy heavy snow" || (string)value == "Moderate or heavy snow showers")
            {
                // heavy snow with sun
            }
            else if ((string)value == "Heavy snow" || (string)value == "Moderate or heavy snow with thunder" || (string)value == "Blowing snow" || (string)value == "Blizzard")
            {
                // heavy snow without sun
            }
            else if ((string)value == "Patchy light rain with thunder")
            {
                // light rain with sun and thunder
                imageSource = "pack://application:,,,/Icons/light_patchy_rain_with_thunder.png";
            }
            else if ((string)value == "Moderate or heavy rain with thunder")
            {
                // heavy rain with sun and thunder
                imageSource = "pack://application:,,,/Icons/heavy_patchy_rain_with_thunder.png";
            }
            else if ((string)value == "Patchy light snow with thunder")
            {
                // heavy rain without sun, but with thunder
                imageSource = "pack://application:,,,/Icons/heavy_rain_with_thunder.png";
            }
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
