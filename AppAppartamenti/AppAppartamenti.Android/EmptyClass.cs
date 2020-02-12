using System;
using Android.App;
using Android.OS;
using Android.Views;
using AppAppartamenti;
using AppAppartamenti.Droid;
using AppAppartamenti.Helpers;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBarStyleManager))]
namespace AppAppartamenti.Droid
{
    public class StatusBarStyleManager : IStatusBarStyleManager
    {
        public void SetColoredStatusBar(string hexColor)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
               
                    var currentWindow = GetCurrentWindow();
                    currentWindow.DecorView.SystemUiVisibility = 0;
                    currentWindow.SetStatusBarColor(Android.Graphics.Color.ParseColor(hexColor));
            }
        }

        public void SetWhiteStatusBar()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
               
                    var currentWindow = GetCurrentWindow();
                    currentWindow.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
                    currentWindow.SetStatusBarColor(Android.Graphics.Color.White);
            }
        }

        Window GetCurrentWindow()
        {
            var window = CrossCurrentActivity.Current.Activity.Window;

            // clear FLAG_TRANSLUCENT_STATUS flag:
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);

            // add FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS flag to the window
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

            return window;
        }
    }
}
