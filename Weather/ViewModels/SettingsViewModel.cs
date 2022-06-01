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
                // One of the options must be true at all times
                if (!value && !_isImperialSystemEnabled) return;
                
                _isMetricSystemEnabled = value;

                // Only one can be active at the same time
                if (value) IsImperialSystemEnabled = false;

                OnPropertyChanged();

            }
        }

        private bool _isImperialSystemEnabled;
        public bool IsImperialSystemEnabled
        {
            get => _isImperialSystemEnabled;
            set
            {
                // One of the options must be true at all times
                if (!value && !_isMetricSystemEnabled) return;

                _isImperialSystemEnabled = value;

                // Only one can be active at the same time
                if (value) IsMetricSystemEnabled = false;

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

            Properties.Settings.Default.WeatherLocation = WeatherLocation.Trim(); // This will be used in the URL so it's important to remove unnecessary white-space
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
