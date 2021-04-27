using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdsKitInXamarinForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        int lastScore;
        IRewardAdService rewardAdService;
        public ResultPage(int score)
        {
            rewardAdService = DependencyService.Get<IRewardAdService>();
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            lastScore = score;

            lblLastScore.Text = lastScore.ToString();
        }

        async void OnPlayAgainClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
        }

        void OnExitClicked(object sender, System.EventArgs e)
        {
            rewardAdService.ExitFromApp();
        }
    }
}