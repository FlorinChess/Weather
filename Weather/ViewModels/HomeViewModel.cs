using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Weather.Commands;
using Weather.Core;
using Weather.Core.Exceptions;
using Weather.Core.Models;
using Weather.Models;
using Weather.Stores;

namespace Weather.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        #region Properties

        public ObservableCollection<WeatherHour> WeatherHours { get; set; } = new ObservableCollection<WeatherHour>();
        public string ChanceOfRain { get; set; }
        public string FullDate { get; set; }
        public string FullWeatherLocation { get; set; }
        public static bool IsMetricSystemEnabled => Properties.Settings.Default.IsMetricSystemEnabled;
        public string MaxTemperature { get; set; }
        public string MinTemperature { get; set; }
        public string Precipitation { get; set; }
        public string Temperature { get; set; }
        public WeatherInformation Weather { get; set; }
        public string WeatherCondition { get; set; }
        public string Wind { get; set; }
        public string WindDirection { get; set; }

        private Current _currentWeatherDay;
        public Current CurrentWeatherDay
        {
            get => _currentWeatherDay;
            set
            {
                _currentWeatherDay = value;
                OnPropertyChanged();
            }
        }

        private bool _isApiCallFinished = true;
        public bool IsApiCallFinished 
        {
            get => _isApiCallFinished;
            set
            {
                _isApiCallFinished = value;
                OnPropertyChanged();
            }
        }

        private string _weatherLocation = Properties.Settings.Default.WeatherLocation;
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

        public ICommand OpenSettingsCommand { get; }
        public ICommand OpenFeedbackCommand { get; }
        public ICommand UpdateWeatherLocationCommand { get; }

        #endregion

        public HomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            OpenSettingsCommand = new RelayCommand(() =>
            {
                _navigationStore.CurrentViewModel = new SettingsViewModel(_navigationStore);
            });

            OpenFeedbackCommand = new RelayCommand(() =>
            {
                _navigationStore.CurrentViewModel = new FeedbackViewModel(_navigationStore);
            });

            UpdateWeatherLocationCommand = new RelayCommand(async () =>
            {
                await UpdateWeatherData();
            });

            UpdateWeatherLocationCommand.Execute(null);
        }

        #region Private Methods

        private async Task UpdateWeatherData()
        {
            if (string.IsNullOrEmpty(WeatherLocation)) return;

            // Error handling
            try
            {
                await GetWeatherInformation();

                UpdateLocationSettings();
            }
            catch (LocationNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Invalid location", MessageBoxButton.OK, MessageBoxImage.Error);

                IsApiCallFinished = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#else
                MessageBox.Show("An error occured! Please enter a different weather location!", "An error occured!", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
                IsApiCallFinished = true;
            }
        }

        private async Task GetWeatherInformation()
        {
            // Disable the weather location TextBox to prevent conflicting calls
            IsApiCallFinished = false;

            // Get all the data here
            Weather = await ApiCaller.GetWeather(_weatherLocation);

            // Populate collections after data has been retrieved
            PopulateCollections();

            // Update the GUI
            UpdateUserInterface();

            IsApiCallFinished = true;
        }

        /// <summary>
        /// Populates the observable collections on the UI thread
        /// </summary>
        private void PopulateCollections()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Reset collections
                WeatherHours.Clear();

                var weatherDay = Weather.forecast.forecastday[0];

                foreach (Hour weatherHour in weatherDay.hour)
                {
                    DateTime time = DateTime.Parse(weatherHour.time);

                    // Take into account the local time from the weather location
                    DateTime weatherLocalTime = DateTime.Parse(Weather.location.localtime);

                    // Don't display information from the past
                    if (DateTime.Compare(time, weatherLocalTime) <= 0) continue;

                    WeatherHours.Add(new WeatherHour()
                    {
                        Temperature =
                            (IsMetricSystemEnabled)
                            ? $"{weatherHour.temp_c}°C"
                            : $"{weatherHour.temp_f}°F",
                        Text = weatherHour.condition.text,
                        Time = time.ToShortTimeString()
                    });
                }

                CurrentWeatherDay = Weather.current;
            });
        }

        private void UpdateLocationSettings()
        {
            Properties.Settings.Default.WeatherLocation = WeatherLocation.Trim(); // This will be used in the URL so it's important to remove unnecessary white-space
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Updates all the properties binded to the GUI
        /// </summary>
        private void UpdateUserInterface()
        {
            FullWeatherLocation = $"{WeatherLocation}, {Weather.location.country}";

            DateTime date = DateTime.Parse(CurrentWeatherDay.last_updated);

            // Full date; e.g. Monday, 15 January
            FullDate = $"{date.DayOfWeek}, {date.Day} {date:MMMM}";

            WeatherCondition = CurrentWeatherDay.condition.text;

            MaxTemperature =
                (IsMetricSystemEnabled)
                ? $"{Weather.forecast.forecastday[0].day.maxtemp_c}°C"
                : $"{Weather.forecast.forecastday[0].day.maxtemp_f}°F";

            MinTemperature =
                (IsMetricSystemEnabled)
                ? $"{Weather.forecast.forecastday[0].day.mintemp_c}°C"
                : $"{Weather.forecast.forecastday[0].day.mintemp_f}°F";

            // Set current temperature
            Temperature =
                (IsMetricSystemEnabled)
                ? $"{CurrentWeatherDay.temp_c}°C"
                : $"{CurrentWeatherDay.temp_f}°F";

            // Set wind speed
            Wind =
                (IsMetricSystemEnabled)
                ? $"{CurrentWeatherDay.wind_kph}/kph"
                : $"{CurrentWeatherDay.wind_mph}/mph";

            // Set wind direction
            WindDirection = CurrentWeatherDay.wind_dir;

            // Set precipitation
            Precipitation =
                (IsMetricSystemEnabled)
                ? $"{CurrentWeatherDay.precip_mm} mm"
                : $"{CurrentWeatherDay.precip_in} in";

            // Set chance of rain
            ChanceOfRain = $"{Weather.forecast.forecastday[0].day.daily_chance_of_rain}%";
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
