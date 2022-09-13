using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Input;
using Weather.Commands;
using Weather.Stores;

namespace Weather.ViewModels
{
    public sealed class SettingsViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        #region Properties

        private bool _isMetricSystemEnabled;
        public bool IsMetricSystemEnabled
        {
            get => _isMetricSystemEnabled;
            set
            {
                _isMetricSystemEnabled = value;

                // Only one can be active at the same time
                if (value)
                    IsImperialSystemEnabled = false;

                OnPropertyChanged();

            }
        }

        private bool _isImperialSystemEnabled;
        public bool IsImperialSystemEnabled
        {
            get => _isImperialSystemEnabled;
            set
            {
                _isImperialSystemEnabled = value;

                // Only one can be active at the same time
                if (value)
                    IsMetricSystemEnabled = false;

                OnPropertyChanged();
            }
        }

        private string _weatherLocation;
        public string WeatherLocation
        {
            get => _weatherLocation;
            set
            {
                _weatherLocation = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand SaveSettingsCommand { get; }
        public ICommand CancelCommand { get; }

        #endregion

        public SettingsViewModel(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;

            CancelCommand = new RelayCommand(_ =>
            {
                _navigationStore.CurrentViewModel = serviceProvider.GetRequiredService<HomeViewModel>();
            });

            SaveSettingsCommand = new RelayCommand(_ =>
            {
                UpdateSettings();

                _navigationStore.CurrentViewModel = serviceProvider.GetRequiredService<HomeViewModel>();
            });

            LoadSettings();
        }

        #region Private Methods

        private void LoadSettings()
        {
            WeatherLocation = Properties.Settings.Default.WeatherLocation;

            if (Properties.Settings.Default.IsMetricSystemEnabled)
            {
                IsMetricSystemEnabled = true;
                IsImperialSystemEnabled = false;
            }
            else
            {
                IsMetricSystemEnabled = false;
                IsImperialSystemEnabled = true;
            }
        }

        /// <summary>
        /// Updates and saves all application settings
        /// </summary>
        private void UpdateSettings()
        {
            UpdateWeatherLocation();
            UpdateMeasurementUnit();

            Properties.Settings.Default.Save();
        }

        private void UpdateWeatherLocation()
        {
            if (string.IsNullOrEmpty(WeatherLocation)) return;

            Properties.Settings.Default.WeatherLocation = WeatherLocation.Trim(); // This will be used in the API request so it's important to remove unnecessary white-space
        }

        private void UpdateMeasurementUnit()
        {
            if (IsMetricSystemEnabled)
            {
                Properties.Settings.Default.IsMetricSystemEnabled = true;
            }
            else
            {
                Properties.Settings.Default.IsMetricSystemEnabled = false;
            }
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
