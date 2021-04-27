using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AdsKitInXamarinForm
{
    public partial class MainPage : ContentPage
    {
        Image[] images;
        int score;
        IRewardAdService rewardAdService;
        CountDownTimer countDownTimer;
        bool IsRewarded = false;
        public MainPage()
        {
            
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            images  = new Image[] {imageOne,imageTwo,imageThree,imageFour,imageFive,imageSix,imageSeven,imageEight,imageNine,imageTen,imageEleven,imageTwelve,imageThirteen
                                            ,imageFourTeen,imageFifTeen};

            score = 0;

            rewardAdService = DependencyService.Get<IRewardAdService>();
            rewardAdService.OnRewarded += RewardAdService_OnRewarded;
            rewardAdService.OnRewardAdOpened += RewardAdService_OnRewardAdOpened;
            rewardAdService.OnRewardAdClosed += RewardAdService_OnRewardAdClosed;
            rewardAdService.OnRewardAdFailedToLoad += RewardAdService_OnRewardAdFailedToLoad;
            rewardAdService.OnRewardAdFailedToShow += RewardAdService_OnRewardAdFailedToShow;
            rewardAdService.OnRewardedLoaded += RewardAdService_OnRewardedLoaded;
            rewardAdService.LoadRewardAd();

            HideImages();

            countDownTimer = new CountDownTimer(this);
            countDownTimer.CountDownMinutes = 0;
            countDownTimer.CountDownSeconds = 60;
            countDownTimer.TimerStart();


        }
        
        private void RewardAdService_OnRewardedLoaded(object sender, string e)
        {
            Console.WriteLine("Reward ad loaded. "+ e);
            IsRewarded = false;
        }

        private void RewardAdService_OnRewardAdFailedToShow(object sender, int e)
        {
            Console.WriteLine("Reward ad failed to show. Error code: " + e);
        }

        private void RewardAdService_OnRewardAdFailedToLoad(object sender, int e)
        {
            Console.WriteLine("Reward ad failed to load. Error code: " + e);
        }

        private async void RewardAdService_OnRewardAdClosed(object sender, string e)
        {
            Console.WriteLine("Reward ad closed. " + e);
            if (IsRewarded)
            {
                HideImages();
                countDownTimer.CountDownMinutes = 0;
                countDownTimer.CountDownSeconds = 60;
                countDownTimer.TimerStart();
            }
            else
            {
                await Navigation.PushAsync(new ResultPage(score));
                Navigation.RemovePage(this);
            }
        }

        private void RewardAdService_OnRewardAdOpened(object sender, string e)
        {
            Console.WriteLine("Reward ad opened. " + e);
            rewardAdService.LoadRewardAd();
        }

        private void RewardAdService_OnRewarded(object sender, string e)
        {
            IsRewarded = true;
        }

        public void HideImages()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
             
            int TotalSec = 60;

            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    return false;
                }
                else
                {
                    if (TotalSec == 0)
                    {
                        ShowDialogMessage();
                        return false;
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        TotalSec = TotalSec - 1;
                        foreach (var item in images)
                        {
                            item.IsVisible = false;
                        }

                        Random random = new Random();
                        int randomNumber = random.Next(0, 14);

                        images[randomNumber].IsVisible = true;
                    });
                    return true;
                }
            });
            

        }
        async void ShowDialogMessage()
        {
            bool answer = await DisplayAlert("Game Over", "Would you like to continue with this score?" + " Score: " + score, "Yes", "No");
            if (answer)
            {
                Log.Warning("Dialog", "Open Ad Platform.");
                
                if (rewardAdService.IsLoaded())
                {
                    rewardAdService.ShowRewardAd();
                }
            }
            else
            {
                Log.Warning("Dialog", "Go to Result Page.");
                await Navigation.PushAsync(new ResultPage(score));
                Navigation.RemovePage(this);
            }
        }
        void OnImageNameTapped(object sender, EventArgs args)
        {
            try
            {
                score++;
                lblScore.Text = score.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        async void OnExitButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
            Navigation.RemovePage(this);
        }
        public class CountDownTimer : Label
        {
            CancellationTokenSource _CancellationTokenSource;
            MainPage mainPage;
            public CountDownTimer(MainPage mainPage)
            {
                this.mainPage = mainPage;
                _CancellationTokenSource = new CancellationTokenSource();
                FontFamily = "sans-serif-light";
                FontSize = 64;
                TextColor = Color.FromHex("#ffffff");
            }

            public void TimerStart()
            {
                int Min = CountDownMinutes;
                int Sec = CountDownSeconds;
                int TotalSec = (Min * 60) + Sec;

                CancellationTokenSource CTS = _CancellationTokenSource;

                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    if (CTS.IsCancellationRequested)
                    {
                        return false;
                    }
                    else
                    {
                        if (TotalSec == 0)
                        {
                            return false;
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            TotalSec = TotalSec - 1;
                            TimeSpan _TimeSpan = TimeSpan.FromSeconds(TotalSec);
                            mainPage.lblTime.Text = string.Format("{0:00}:{1:00}", _TimeSpan.Minutes, _TimeSpan.Seconds);
                        });
                        return true;
                    }
                });
            }

            private void TimerStop()
            {
                Interlocked.Exchange(ref _CancellationTokenSource, new CancellationTokenSource()).Cancel();
            }

            static void OnTimerCancelChanged(BindableObject bindable, object oldvalue, object newvalue)
            {
                ((CountDownTimer)bindable).TimerStop();
            }

            static void OnTimerTimeChanged(BindableObject bindable, object oldvalue, object newvalue)
            {
                ((CountDownTimer)bindable).TimerStop();
                ((CountDownTimer)bindable).TimerStart();
            }

            public static readonly BindableProperty CountDownMinutesProperty = BindableProperty.Create("CountDownMinutes", typeof(int), typeof(CountDownTimer), 0, BindingMode.TwoWay, null, OnTimerTimeChanged);
            public int CountDownMinutes
            {
                get { return (int)base.GetValue(CountDownMinutesProperty); }
                set { base.SetValue(CountDownMinutesProperty, value); }
            }

            public static readonly BindableProperty CountDownSecondsProperty = BindableProperty.Create("CountDownSeconds", typeof(int), typeof(CountDownTimer), 0, BindingMode.TwoWay, null, OnTimerTimeChanged);
            public int CountDownSeconds
            {
                get { return (int)base.GetValue(CountDownSecondsProperty); }
                set { base.SetValue(CountDownSecondsProperty, value); }
            }

            public static readonly BindableProperty TimerCancelProperty = BindableProperty.Create("TimerCancel", typeof(bool), typeof(CountDownTimer), false, BindingMode.TwoWay, null, OnTimerCancelChanged);
            public bool TimerCancel
            {
                get { return (bool)base.GetValue(TimerCancelProperty); }
                set { base.SetValue(TimerCancelProperty, value); }
            }

        }
    }
}
