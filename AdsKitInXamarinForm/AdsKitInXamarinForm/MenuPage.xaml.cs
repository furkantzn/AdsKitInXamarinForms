using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AdsKitInXamarinForm
{
    public partial class MenuPage : ContentPage
    {
        IRewardAdService rewardAdService;
        public MenuPage()
        {
            rewardAdService = DependencyService.Get<IRewardAdService>();
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
        async void OnPlayButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
        void OnExitButtonClicked(object sender, System.EventArgs e)
        {
            rewardAdService.ExitFromApp();
        }
    }
}