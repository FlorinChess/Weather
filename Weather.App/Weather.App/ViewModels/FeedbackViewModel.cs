using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather.App.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        #region Commands

        public Command DonateCommand { get; }
        public Command SubmitFeedbackCommand { get; set; }

        #endregion

        public FeedbackViewModel()
        {
            DonateCommand = new Command(async () => await Browser.OpenAsync("https://ko-fi.com/florin_chess"));

            SubmitFeedbackCommand = new Command(() => { });
        }
    }
}
