using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Weather.App.Helpers;
using Xamarin.Essentials;

namespace Weather.App.ViewModels
{
    public class SettingsViewModel : BaseViewModel
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
