using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather.App.ViewModels
{
    public sealed class FeedbackViewModel : BaseViewModel
    {
        #region Commands

        public Command DonateCommand { get; }
        public Command SubmitFeedbackCommand { get; }

        #endregion

        public FeedbackViewModel()
        {
            DonateCommand = new Command(async () => await Browser.OpenAsync("https://ko-fi.com/florin_chess"));

            SubmitFeedbackCommand = new Command(async () => await Browser.OpenAsync("https://feedback-manager.azurewebsites.net/"));
        }
    }
}
