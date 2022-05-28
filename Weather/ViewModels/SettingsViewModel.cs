using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Weather.Commands;
using Weather.Stores;

namespace Weather.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {

        private NavigationStore _navigationStore;

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

        public ICommand SaveSettingsCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        #endregion

        public SettingsViewModel(NavigationStore navigationStore)
        {
            this._navigationStore = navigationStore;

            CancelCommand = new RelayCommand(() =>
            {
                _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
            });

            SaveSettingsCommand = new RelayCommand(() =>
            {
                UpdateSettings();

                _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
            });

            LoadSettings();
        }

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
        public void UpdateSettings()
        {
            UpdateWeatherLocation();
            UpdateMeasurementUnit();
            
            Properties.Settings.Default.Save();
        }

        public void UpdateWeatherLocation()
        {
            if (string.IsNullOrEmpty(WeatherLocation)) return;

            Properties.Settings.Default.WeatherLocation = WeatherLocation.Trim(); // This will be used in the URL so it's important to remove unnecessary white-space
        }

        public void UpdateMeasurementUnit()
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

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
