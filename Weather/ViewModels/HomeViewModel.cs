using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Weather.Commands;
using Weather.Core;
using Weather.Core.Models;
using Weather.Models;
using Weather.Stores;

namespace Weather.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        #region Properties

        public ObservableCollection<Forecastday> WeatherDays { get; set; } = new ObservableCollection<Forecastday>();
        public ObservableCollection<WeatherHour> WeatherHours { get; set; } = new ObservableCollection<WeatherHour>();

        public bool IsMetricSystemEnabled => Properties.Settings.Default.IsMetricSystemEnabled;

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

        public string FullWeatherLocation { get; set; }
        public string FullDate { get; set; }

        private Forecastday _selectedWeatherDay;
        public Forecastday SelectedWeatherDay
        {
            get => _selectedWeatherDay;
            set 
            { 
                _selectedWeatherDay = value;
                OnPropertyChanged();
            }
        }

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

        public string Temperature { get; set; }
        public string MaxTemperature { get; set; }
        public string MinTemperature { get; set; }
        public string Wind { get; set; }
        public string WindDirection { get; set; }
        public string ChanceOfRain { get; set; }
        public string WeatherCondition { get; set; }
        public string LastUpdate { get; set; }
        public WeatherInformation Weather { get; set; }

        #endregion

        #region Commands

        public ICommand OpenSettingsCommand { get; set; }
        public ICommand OpenFeedbackCommand { get; set; }
        public ICommand UpdateWeatherLocationCommand { get; set; }

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

        private async Task UpdateWeatherData()
        {
            if (string.IsNullOrEmpty(WeatherLocation)) return;

            UpdateLocationSettings();

            await GetWeatherInformation();
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
                    DateTime.TryParse(weatherHour.time, out DateTime time);

                    // Take into account the local time from the weather location
                    DateTime weatherLocalTime = DateTime.Parse(Weather.location.localtime);

                    // Don't display information from the past
                    if (DateTime.Compare(time, weatherLocalTime) <= 0) continue;

                    WeatherHours.Add(new WeatherHour()
                    {
                        Time = time.ToShortTimeString(),
                        Temperature =
                            (IsMetricSystemEnabled) 
                            ? $"{weatherHour.temp_c}°C"
                            : $"{weatherHour.temp_f}°F",
                        Text = weatherHour.condition.text,
                        Wind =
                            (IsMetricSystemEnabled) 
                            ? $"{weatherHour.wind_kph}/kph" 
                            : $"{weatherHour.wind_mph}/mph",
                        Precipitation =
                            (IsMetricSystemEnabled)
                            ? $"{weatherHour.wind_kph} mm"
                            : $"{weatherHour.wind_mph} in",
                        Humidity = weatherHour.humidity
                    });
                }

                CurrentWeatherDay = Weather.current;
            });
        }

        /// <summary>
        /// Updates the weather location setting
        /// </summary>
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
            FullDate = $"{date.DayOfWeek}, {date.Day} {date.ToString("MMMM")}";

            WeatherCondition = CurrentWeatherDay.condition.text;

            LastUpdate = $"Last Update: { date.ToShortTimeString() }";

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
                ? $"{ CurrentWeatherDay.temp_c }°C" 
                : $"{ CurrentWeatherDay.temp_f }°F";

            // Set wind speed
            Wind =
                (IsMetricSystemEnabled)
                ? $"{ CurrentWeatherDay.wind_kph }/kph"
                : $"{ CurrentWeatherDay.wind_mph }/mph";

            // Set wind direction
            WindDirection = CurrentWeatherDay.wind_dir;

            OnPropertyChanged(nameof(LastUpdate));
            OnPropertyChanged(nameof(Temperature));
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
