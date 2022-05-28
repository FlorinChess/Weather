using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Weather.Commands;
using Weather.Stores;

namespace Weather.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        private NavigationStore _navigationStore;

        private string _feedbackMessage;
        public string FeedbackMessage 
        { 
            get => _feedbackMessage;
            set 
            { 
                _feedbackMessage = value;
                FeedbackMessageLength = _feedbackMessage.Length.ToString();
                OnPropertyChanged();
            } 
        }

        private string _feedbackMessageLength = "0/300";

        public string FeedbackMessageLength
        {
            get => _feedbackMessageLength; 
            set 
            { 
                _feedbackMessageLength = $"{value}/300";
                OnPropertyChanged();
            }
        }


        #region Commands

        public ICommand SubmitFeedbackCommand { get; set; }

        public ICommand DonateCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        #endregion

        public FeedbackViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;


            DonateCommand = new RelayCommand(() =>
            {
                // Registry path for Google Chrome
                var path = Microsoft.Win32.Registry.GetValue(@"HKEY_CLASSES_ROOT\ChromeHTML\shell\open\command", null, null) as string;

                if (path != null)
                {
                    var split = path.Split('\"');
                    path = split.Length >= 2 ? split[1] : null;
                }
                // Starts the process from the given path (Google Chrome)
                System.Diagnostics.Process.Start(path, "https://ko-fi.com/florin_chess#paypalModal");

                _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
            });

            CloseCommand = new RelayCommand(() =>
            {
                _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
            });

            SubmitFeedbackCommand = new RelayCommand(() =>
            {
                _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
            });
        }
    }
}
