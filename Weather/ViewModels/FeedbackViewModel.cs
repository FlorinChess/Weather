using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Input;
using Weather.Commands;
using Weather.Stores;

namespace Weather.ViewModels
{
    public sealed class FeedbackViewModel : BaseViewModel
    {
        private readonly string KoFiUrl = "https://ko-fi.com/florin_chess";
        private readonly string FeedbackManagerUrl = "https://feedback-manager.azurewebsites.net";
        private readonly NavigationStore _navigationStore;

        #region Commands

        public ICommand SubmitFeedbackCommand { get; }
        public ICommand DonateCommand { get; }
        public ICommand CloseCommand { get; }

        #endregion

        public FeedbackViewModel(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;

            DonateCommand = new RelayCommand(_ => OpenBrowser(KoFiUrl));

            SubmitFeedbackCommand = new RelayCommand(_ => OpenBrowser(FeedbackManagerUrl));

            CloseCommand = new RelayCommand(_ =>  NavigateHome(serviceProvider));
        }

        private void NavigateHome(IServiceProvider serviceProvider)
        {
            _navigationStore.CurrentViewModel = serviceProvider.GetRequiredService<HomeViewModel>();
        }

        private static void OpenBrowser(string url)
        {
            // Registry path for Google Chrome
            var path = Microsoft.Win32.Registry.GetValue(@"HKEY_CLASSES_ROOT\ChromeHTML\shell\open\command", null, null) as string;

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
        }
    }
}
