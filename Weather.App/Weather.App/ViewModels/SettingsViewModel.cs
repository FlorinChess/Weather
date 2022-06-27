using MvvmHelpers.Commands;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Weather.App.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Properties

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

        private bool isMetricSystemEnabled;
        public bool IsMetricSystemEnabled 
        { 
            get => isMetricSystemEnabled;
            set 
            { 
                isMetricSystemEnabled = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand UpdateSettingsCommand { get; set; }

        #endregion

        public SettingsViewModel()
        {
            UpdateSettingsCommand = new AsyncCommand(async () =>
            {
                await UpdateSettings();
            });
        }

        private async Task UpdateSettings()
        {

        }
    }
}
