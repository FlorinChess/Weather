using Weather.App.Helpers;

namespace Weather.App.ViewModels
{
    public sealed class SettingsViewModel : BaseViewModel
    {
        private string _weatherLocation = Settings.WeatherLocation;
        public string WeatherLocation
        {
            get => _weatherLocation;
            set
            {
                _weatherLocation = value;
                Settings.WeatherLocation = value;
                OnPropertyChanged();
            }
        }

        private bool isMetricSystemEnabled = Settings.IsMetricSystemEnabled;
        public bool IsMetricSystemEnabled
        {
            get => isMetricSystemEnabled;
            set
            {
                isMetricSystemEnabled = value;
                Settings.IsMetricSystemEnabled = value;
                OnPropertyChanged();
            }
        }
    }
}
