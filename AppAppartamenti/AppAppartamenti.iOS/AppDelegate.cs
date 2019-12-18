using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Syncfusion.SfCalendar.XForms.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Xamarin.RangeSlider.Forms.RangeSlider), typeof(Xamarin.RangeSlider.Forms.RangeSliderRenderer))]
namespace AppAppartamenti.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            // define useragent android like
            //string userAgent =  "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.13; rv:61.0) Gecko/20100101 Firefox/61.0";

            //// set default useragent
            //NSDictionary dictionary = NSDictionary.FromObjectAndKey(NSObject.FromObject(userAgent), NSObject.FromObject("UserAgent"));
            //NSUserDefaults.StandardUserDefaults.RegisterDefaults(dictionary);

            NSUserDefaults.StandardUserDefaults.RegisterDefaults(new NSDictionary("UserAgent",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.75.14 (KHTML, like Gecko) Version/7.0.3 Safari/7046A194A"));

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");

            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();
            Plugin.InputKit.Platforms.iOS.Config.Init();
            Xamarin.FormsMaps.Init();
            SfCalendarRenderer.Init();
            Syncfusion.XForms.iOS.PopupLayout.SfPopupLayoutRenderer.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

    }
}
