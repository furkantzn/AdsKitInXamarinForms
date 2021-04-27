using AdsKitInXamarinForm.Droid;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Huawei.Hms.Ads;
using Huawei.Hms.Ads.Reward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(RewardAdService))]
namespace AdsKitInXamarinForm.Droid
{
    public class RewardAdService : IRewardAdService
    {
        private RewardAd rewardedAd;
        private Activity activity;
        
        public RewardAdService()
        {
            this.activity = Platform.CurrentActivity;
        }

        public event EventHandler<string> OnRewarded;
        public event EventHandler<string> OnRewardAdClosed;
        public event EventHandler<int> OnRewardAdFailedToShow;
        public event EventHandler<string> OnRewardAdOpened;
        public event EventHandler<int> OnRewardAdFailedToLoad;
        public event EventHandler<string> OnRewardedLoaded;

        public void Destroy()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
            }
        }

        public void ExitFromApp()
        {
            if(activity != null)
            {
                activity.Finish();
            }
        }

        public bool IsLoaded()
        {
            if (rewardedAd != null)
            {
                return rewardedAd.IsLoaded;
            }
            else
            {
                return false;
            }
        }

        public void LoadRewardAd()
        {
            if (rewardedAd == null)
            {
                rewardedAd = new RewardAd(activity, "testx9dtjwj8hp");
            }
            rewardedAd.LoadAd(new AdParam.Builder().Build(), new RewardListener(activity,this));
        }
        public void Pause()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Pause();
            }
        }

        public void Resume()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Resume();
            }
        }

        public void ShowRewardAd()
        {
            if (rewardedAd.IsLoaded)
            {
                rewardedAd.Show(activity,new RewardStatusListener(activity,this));
            }
        }
        
        private class RewardStatusListener : RewardAdStatusListener
        {
            const string TAG = "RewardAdStatusListener";
            private Activity activity;
            private RewardAdService rewardAdService;
            public RewardStatusListener(Activity activity,RewardAdService rewardAdService)
            {
                this.activity = activity;
                this.rewardAdService = rewardAdService;
            }
            public override void OnRewardAdClosed()
            {
                Log.Info(TAG, "OnRewardAdClosed");
                rewardAdService.OnRewardAdClosed?.Invoke(null, "True");
            }
            public override void OnRewardAdFailedToShow(int errorCode)
            {
                Log.Info(TAG, "OnRewardAdFailedToShow Error code is " + errorCode);
                rewardAdService.OnRewardAdFailedToShow?.Invoke(null, errorCode);
            }
            public override void OnRewardAdOpened()
            {
                Log.Info(TAG, "OnRewardAdOpened");
                rewardAdService.OnRewardAdOpened?.Invoke(null, "True");
            }
            public override void OnRewarded(IReward reward)
            {
                Log.Info(TAG, "OnRewarded");
                rewardAdService.OnRewarded?.Invoke(reward, "True");
            }
        }
        private class RewardListener : RewardAdLoadListener
        {
            const string TAG = "RewardAdLoadListener";
            readonly Activity activity;
            private RewardAdService rewardAdService;
            public RewardListener(Activity currentActivity,RewardAdService rewardAdService)
            {
                this.rewardAdService = rewardAdService;
                this.activity = currentActivity;
            }
            public override void OnRewardAdFailedToLoad(int errorCode)
            {
                Log.Info(TAG, "OnRewardAdFailedToLoad Error code is " + errorCode);
                rewardAdService.OnRewardAdFailedToLoad?.Invoke(null, errorCode);
            }
            public override void OnRewardedLoaded()
            {
                Log.Info(TAG, "OnRewardedLoaded");
                rewardAdService.OnRewardedLoaded?.Invoke(null, "True");
            }
        }
    }
}