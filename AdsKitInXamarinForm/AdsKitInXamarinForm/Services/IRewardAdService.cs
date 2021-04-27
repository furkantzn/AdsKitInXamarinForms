using System;
using System.Collections.Generic;
using System.Text;

namespace AdsKitInXamarinForm
{
    public interface IRewardAdService
    {
        void ExitFromApp();
        void Destroy();
        void Pause();
        void Resume();
        bool IsLoaded();
        void LoadRewardAd();
        void ShowRewardAd();
        event EventHandler<string> OnRewarded;
        event EventHandler<string> OnRewardAdClosed;
        event EventHandler<int> OnRewardAdFailedToShow;
        event EventHandler<string> OnRewardAdOpened;
        event EventHandler<int> OnRewardAdFailedToLoad;
        event EventHandler<string> OnRewardedLoaded;
    }
}
