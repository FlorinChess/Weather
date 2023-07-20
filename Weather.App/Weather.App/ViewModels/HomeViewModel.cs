using MvvmHelpers.Commands;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.App.Helpers;
using Weather.App.Models;
using Weather.Core;
using Weather.Core.Exceptions;
using Weather.Core.Models;
using Xamarin.Forms;

namespace Weather.App.ViewModels
{
    public sealed class HomeViewModel : BaseViewModel
    {
        private readonly ApiCaller _apiCaller;

        #region Properties

        public ObservableCollection<WeatherHour> WeatherHours { get; set; }
        public string ChanceOfRain { get; set; }
        public string FullDate { get; set; }
        public string FullWeatherLocation { get; set; }
        public bool IsMetricSystemEnabled { get; set; } = Settings.IsMetricSystemEnabled;
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

        private string _weatherLocation = Settings.WeatherLocation;
        public string WeatherLocation
        {
            get => _weatherLocation;
            set
            {
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public AsyncCommand UpdateWeatherLocationCommand { get; }

        #endregion

        public HomeViewModel()
        {
            _apiCaller = new ApiCaller(new HttpClient());

            WeatherHours = new ObservableCollection<WeatherHour>();

            UpdateWeatherLocationCommand = new AsyncCommand(async () =>
            {
                await UpdateWeatherData();
            });

            UpdateWeatherLocationCommand.ExecuteAsync();
        }

        #region Private Methods

        private async Task UpdateWeatherData()
        {
            if (string.IsNullOrEmpty(WeatherLocation)) return;

            // Error handling
            try
            {
                await GetWeatherInformation();

                // pdateLocationSettings();
            }
            catch (LocationNotFoundException ex)
            {
                // MessageBox.Show(ex.Message, "Invalid location", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine(ex.Message);
                IsBusy = false;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"catch 2: {ex.Message}");

                // MessageBox.Show(ex.Message);
#else
                // MessageBox.Show("An error occured! Please enter a different weather location!", "An error occured!", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
                IsBusy = false;
            }
        }

        private async Task GetWeatherInformation()
        {
            // Disable the weather location TextBox to prevent conflicting calls
            IsBusy = true;

            // Get all the data here
            Weather = await _apiCaller.GetWeather(_weatherLocation);

            // Set current weather day
            CurrentWeatherDay = Weather.current;

            //Populate collections after data has been retrieved
            PopulateCollections();

            // Update the GUI
            UpdateUserInterface();

            IsBusy = false;
        }

        /// <summary>
        /// Populates the observable collections on the UI thread
        /// </summary>
        private void PopulateCollections()
        {
            // Reset collection
            WeatherHours.Clear();

            var weatherDay = Weather.forecast.forecastday[0];

            foreach (Hour weatherHour in weatherDay.hour)
            {
                DateTime time = DateTime.Parse(weatherHour.time);

                // Take into account the local time from the weather location
                DateTime weatherLocalTime = DateTime.Parse(Weather.location.localtime);

                // Don't display information from the past
                if (DateTime.Compare(time, weatherLocalTime) <= 0) continue;

                Device.BeginInvokeOnMainThread(() =>
                {
                    WeatherHours.Add(new WeatherHour()
                    {
                        Temperature =
                            (IsMetricSystemEnabled)
                            ? $"{weatherHour.temp_c}°C"
                            : $"{weatherHour.temp_f}°F",
                        Text = weatherHour.condition.text,
                        Time = time.ToShortTimeString()
                    });

                });
            }
        }

        /// <summary>
        /// Updates all the properties binded to the UI
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

            NotifyUserInterfaceChanges();
        }

        /// <summary>
        /// Fires off OnPropertyChanged() for all the properties binded to the UI
        /// </summary>
        private void NotifyUserInterfaceChanges()
        {
            OnPropertyChanged(nameof(FullWeatherLocation));
            OnPropertyChanged(nameof(FullDate));
            OnPropertyChanged(nameof(WeatherCondition));
            OnPropertyChanged(nameof(MaxTemperature));
            OnPropertyChanged(nameof(MinTemperature));
            OnPropertyChanged(nameof(Temperature));
            OnPropertyChanged(nameof(Wind));
            OnPropertyChanged(nameof(WindDirection));
            OnPropertyChanged(nameof(ChanceOfRain));
            OnPropertyChanged(nameof(Precipitation));
        }

        #endregion
    }
}
