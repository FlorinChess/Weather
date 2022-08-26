using System.Windows.Input;
using Weather.Commands;
using Weather.Stores;

namespace Weather.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        #region Properties

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

        #endregion

        #region Commands

        public ICommand SubmitFeedbackCommand { get; }
        public ICommand DonateCommand { get; }
        public ICommand CloseCommand { get; }

        #endregion

        public FeedbackViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            DonateCommand = new RelayCommand(() =>
            {
                // Registry path for Google Chrome
                var path = Microsoft.Win32.Registry.GetValue(@"HKEY_CLASSES_ROOT\ChromeHTML\shell\open\command", null, null) as string;

                string url = "https://ko-fi.com/florin_chess";

                if (!string.IsNullOrEmpty(path))
                {
                    var split = path.Split('\"');
                    path = split.Length >= 2 ? split[1] : null;

                    // Starts the process from the given path in Google Chrome
                    System.Diagnostics.Process.Start(path, url);
                }
                else
                {
                    // Start the process in Microsoft Edge
                    // NOTE: this is a workaround; /C to terminate cmd.exe after the command runs
                    // TODO: find a cleaner way to open a url with Edge
                    System.Diagnostics.Process.Start("CMD.exe", $"/C start msedge {url}");
                }

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
