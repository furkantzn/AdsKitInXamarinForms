using AdsKitInXamarinForm.Controls;
using AdsKitInXamarinForm.Droid.Renderer;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Huawei.Hms.Ads;
using Huawei.Hms.Ads.Banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomViewControl), typeof(CustomViewRenderer))]
namespace AdsKitInXamarinForm.Droid.Renderer
{
    public class CustomViewRenderer : ViewRenderer<CustomViewControl,BannerView>
    {
        protected BannerView NativeBanner => Control;

        private const int RefreshTime = 30;

        public CustomViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomViewControl> e)
        {
            base.OnElementChanged(e);
            AdParam adParam = new AdParam.Builder().Build();
            if (Control == null)
            {
                var bannerView = new BannerView(Context);
                bannerView.SetBannerRefresh(RefreshTime);
                bannerView.AdId = "testw6vs28auh3";


                bannerView.LoadAd(adParam);

                SetNativeControl(bannerView);
            }
            else
            {
                NativeBanner.SetBannerRefresh(RefreshTime);
                NativeBanner.LoadAd(adParam);
            }
            if (null != e.OldElement)
            {
                BannerView bannerView = Control;
            }

            if (null != e.NewElement)
            {
                Control.LoadAd(adParam);
            }
        }
    }
}