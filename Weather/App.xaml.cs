using System.Windows;
using Weather.Stores;
using Weather.ViewModels;

namespace Weather
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;
        public App()
        {
            _navigationStore = new NavigationStore();
            _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(this.MainWindow, _navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
