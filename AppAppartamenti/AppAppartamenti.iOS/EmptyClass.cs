﻿using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AppAppartamenti;
using AppAppartamenti.iOS;
using CoreGraphics;
using Foundation;
using System.ComponentModel;
using AppAppartamenti.ContentViews;

[assembly: ExportRenderer(typeof(SearchEntry), typeof(EntryCustomBorderRenderer))]
[assembly: ExportRenderer(typeof(ShadowFrame), typeof(ShadowFrameRenderer))]
[assembly: ExportRenderer(typeof(WebViewUserAgent), typeof(CustomWebView))]
[assembly: ExportRenderer(typeof(ChatInputBarView), typeof(ChatEntryRenderer))]
namespace AppAppartamenti.iOS
{
    public class EntryCustomBorderRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                return;

            Control.BorderStyle = UITextBorderStyle.None;
        }
    }

    public class EntryFocusEffect : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                return;

            Control.BorderStyle = UITextBorderStyle.None;
        }
    }

    public class CustomWebView : WebViewRenderer
    {
        public CustomWebView()
        {
            NSUserDefaults.StandardUserDefaults.RegisterDefaults(new NSDictionary("UserAgent",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.75.14 (KHTML, like Gecko) Version/7.0.3 Safari/7046A194A"));
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
           
            base.OnElementChanged(e);
        }
    }

    /// <summary>
    /// Renderer to update all frames with better shadows matching material design standards
    /// </summary>

    public class ShadowFrameRenderer : FrameRenderer
        {
            public override void Draw(CGRect rect)
            {
                base.Draw(rect);

                // Update shadow to match better material design standards of elevation
                Layer.ShadowRadius = 2.0f;
                Layer.ShadowColor = UIColor.Gray.CGColor;
                Layer.ShadowOffset = new CGSize(2, 2);
                Layer.ShadowOpacity = 0.40f;
                Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
                Layer.MasksToBounds = false;
            }
    }


    public class ChatEntryRenderer : ViewRenderer //Depending on your situation, you will need to inherit from a different renderer
    {
        NSObject _keyboardShowObserver;
        NSObject _keyboardHideObserver;
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                RegisterForKeyboardNotifications();
            }

            if (e.OldElement != null)
            {
                UnregisterForKeyboardNotifications();
            }
        }

        void RegisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver == null)
                _keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
            if (_keyboardHideObserver == null)
                _keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
        }

        void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
        {

            NSValue result = (NSValue)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));
            CGSize keyboardSize = result.RectangleFValue.Size;
            if (Element != null)
            {
                Element.Margin = new Thickness(0, 0, 0, keyboardSize.Height - 50); //push the entry up to keyboard height when keyboard is activated

            }
        }

        void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
            {
                Element.Margin = new Thickness(0); //set the margins to zero when keyboard is dismissed
            }

        }


        void UnregisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver != null)
            {
                _keyboardShowObserver.Dispose();
                _keyboardShowObserver = null;
            }

            if (_keyboardHideObserver != null)
            {
                _keyboardHideObserver.Dispose();
                _keyboardHideObserver = null;
            }
        }
    }
}
