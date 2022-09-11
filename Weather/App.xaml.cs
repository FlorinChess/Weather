using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Windows;
using Weather.Core;
using Weather.Stores;
using Weather.ViewModels;

namespace Weather
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<HttpClient>();
            services.AddSingleton<ApiCaller>();
            services.AddSingleton<NavigationStore>();

            services.AddTransient<HomeViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddSingleton<FeedbackViewModel>();

            _serviceProvider = services.BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
            navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(this.MainWindow, _serviceProvider.GetRequiredService<NavigationStore>())
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
