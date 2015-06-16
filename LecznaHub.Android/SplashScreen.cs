using Android.App;
using Android.Content.PM;
using Cirrious.MvvmCross.Droid.Views;
using LecznaHub.Android;

namespace LecznaHub.Android
{
    [Activity(
		Label = "AndroidApplication1"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, NoHistory = true
		, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {

        }
    }
}