using System;
using Weather.App.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Weather.App
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
