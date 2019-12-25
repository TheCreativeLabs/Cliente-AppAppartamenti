using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System;
using iOS.Renderers;
using AppAppartamenti.Utility;
using CoreGraphics;
using AppAppartamenti;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(ShadowNavigationBarRenderer))]
[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationRenderer))]
[assembly: ExportRenderer(typeof(DatePickerCtrl), typeof(DatePickerCtrlRenderer))]

namespace iOS.Renderers
{
    public class CustomNavigationRenderer : NavigationRenderer
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            UINavigationBar.Appearance.ShadowImage = new UIImage();
            UINavigationBar.Appearance.BackgroundColor = UIColor.Clear;
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = UIColor.Clear;
            UINavigationBar.Appearance.Translucent = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }
    }


        public class DatePickerCtrlRenderer : DatePickerRenderer
        {
            protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
            {
                base.OnElementChanged(e);
                if (this.Control == null)
                    return;
                var element = e.NewElement as DatePickerCtrl;
                if (!string.IsNullOrWhiteSpace(element.Placeholder))
                {
                    Control.Text = element.Placeholder;
                }
                Control.BorderStyle = UITextBorderStyle.RoundedRect;
                Control.Layer.BorderColor = UIColor.FromRGB(211, 211, 211).CGColor;
                //Control.Layer.CornerRadius = 10;
                Control.Layer.BorderWidth = 1f;
                Control.AdjustsFontSizeToFitWidth = true;
                Control.TextColor = UIColor.FromRGB(211, 211, 211);

            Control.ShouldEndEditing += (textField) => {
               var seletedDate = (UITextField)textField;
               var text = seletedDate.Text;
               if (text == element.Placeholder)
               {
                   Control.Text = DateTime.Now.ToString("dd/MM/yyyy");
               }
               return true;
           };
            }
            private void OnCanceled(object sender, EventArgs e)
            {
                Control.ResignFirstResponder();
            }
            private void OnDone(object sender, EventArgs e)
            {
                Control.ResignFirstResponder();
            }
        }


        public class ShadowNavigationBarRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (this.Element == null) return;

            NavigationBar.Layer.ShadowColor = UIColor.LightGray.CGColor;
            NavigationBar.Layer.ShadowOffset = new CGSize(0, 0);
            NavigationBar.Layer.ShadowOpacity = 1;

            // remove lower border and shadow of the navigation bar
            NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            NavigationBar.ShadowImage = new UIImage();
        }
    }

}