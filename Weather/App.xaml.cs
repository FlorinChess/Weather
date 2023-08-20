using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using Weather.Core;
using Weather.Stores;
using Weather.ViewModels;

namespace Weather
{
    public sealed partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddHttpClient();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton<IWeatherClient, WeatherApiClient>();

            services.AddSingleton<WeatherApiClient>();
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
                DataContext = new MainViewModel(MainWindow, _serviceProvider.GetRequiredService<NavigationStore>())
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
