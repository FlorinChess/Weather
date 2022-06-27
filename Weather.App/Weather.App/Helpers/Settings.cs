using Xamarin.Essentials;

namespace Weather.App.Helpers
{
    public static class Settings
    {
        const bool _isMetriSystemEnabled = true;
        public static bool IsMetricSystemEnabled
        {
            get => Preferences.Get(nameof(IsMetricSystemEnabled), _isMetriSystemEnabled);
            set => Preferences.Set(nameof(IsMetricSystemEnabled), value);
        }

        const string _weatherLocation = "Vienna";
        public static string WeatherLocation
        {
            get => Preferences.Get(nameof(WeatherLocation), _weatherLocation);
            set => Preferences.Set(nameof(WeatherLocation), value);
        }
    }
}
