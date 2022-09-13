using System;
using System.Globalization;
using System.Windows.Data;

namespace Weather.Converters
{
    public sealed class StringToSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Mapping all possible API responses to available icons
            string imageSource = "pack://application:,,,/Icons/sunny.png";

            switch (value)
            {
                case "Sunny":
                case "Mostly sunny":
                    // sunny
                    imageSource = "pack://application:,,,/Icons/sunny.png";
                    break;
                case "Clear":
                    // moon
                    imageSource = "pack://application:,,,/Icons/clear.png";
                    break;
                case "Partly cloudy":
                    // light clouds with sun
                    imageSource = "pack://application:,,,/Icons/partly_cloudy.png";
                    break;
                case "Cloudy":
                case "Overcast":
                    // light clouds
                    imageSource = "pack://application:,,,/Icons/cloudy.png";
                    break;
                case "Patchy snow possible":
                    // light snow with sun
                    imageSource = "pack://application:,,,/Icons/patchy_snow.png";
                    break;
                case "Thundery outbreaks possible":
                    // thunder with sun
                    imageSource = "pack://application:,,,/Icons/thundery.png";
                    break;
                case "Fog":
                case "Freezing fog":
                case "Mist":
                    // heavy clouds without sun
                    imageSource = "pack://application:,,,/Icons/mist.png";
                    break;
                case "Patchy light rain":
                case "Moderate rain at times":
                case "Light rain shower":
                case "Patchy rain possible":
                case "Patchy light drizzle":
                case "Patchy sleet possible":
                    // light rain with sun
                    imageSource = "pack://application:,,,/Icons/light_patchy_rain.png";
                    break;
                case "Light rain":
                case "Moderate rain":
                case "Light freezing rain":
                case "Light sleet":
                case "Light sleet showers":
                case "Light drizzle":
                case "Freezing drizzle":
                    // light rain without sun
                    imageSource = "pack://application:,,,/Icons/light_rain.png";
                    break;
                case "Heavy rain at times":
                case "Moderate or heavy rain shower":
                case "Torrential rain shower":
                    // heavy rain with sun
                    imageSource = "pack://application:,,,/Icons/heavy_patchy_rain.png";
                    break;
                case "Heavy rain":
                case "Moderate or heavy freezing rain":
                case "Moderate or heavy sleet":
                case "Moderate or heavy sleet showers":
                case "Heavy freezing drizzle":
                    // heavy rain without sun
                    imageSource = "pack://application:,,,/Icons/heavy_rain.png";
                    break;
                case "Patchy light snow":
                case "Patchy moderate snow":
                case "Light snow showers":
                case "Patchy freezing drizzle possible":
                    // light snow with sun
                    imageSource = "pack://application:,,,/Icons/patchy_snow.png";
                    break;
                case "Light snow":
                case "Moderate snow":
                case "Ice pellets":
                case "Light showers of ice pellets":
                    // light snow without sun
                    imageSource = "pack://application:,,,/Icons/snow.png";
                    break;
                case "Patchy heavy snow":
                case "Moderate or heavy snow showers":
                    break;
                case "Heavy snow":
                case "Moderate or heavy snow with thunder":
                case "Blowing snow":
                case "Blizzard":
                    break;
                case "Patchy light rain with thunder":
                    // light rain with sun and thunder
                    imageSource = "pack://application:,,,/Icons/light_patchy_rain_with_thunder.png";
                    break;
                case "Moderate or heavy rain with thunder":
                    // heavy rain with sun and thunder
                    imageSource = "pack://application:,,,/Icons/heavy_patchy_rain_with_thunder.png";
                    break;
                case "Patchy light snow with thunder":
                    // heavy rain without sun, but with thunder
                    imageSource = "pack://application:,,,/Icons/heavy_rain_with_thunder.png";
                    break;
            }
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
