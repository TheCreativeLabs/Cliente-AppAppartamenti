﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Text;
using Android.Views;
using Android.Widget;
using AppAppartamenti;
using CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

// this line directly ubleow usings, before namespace declaration
[assembly: ExportRenderer(typeof(WebViewUserAgent), typeof(DesktopWebViewRenderer))]
[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryRenderer))]
[assembly: ExportRenderer(typeof(SearchEntry), typeof(SearchEntryRenderer))]
[assembly: ExportRenderer(typeof(CustomDatePickerRenderer), typeof(DatePickerRenderer))]
[assembly: ExportRenderer(typeof(CustomPickerRenderer), typeof(PickerRenderer))]
[assembly: ExportRenderer(typeof(DatePickerRenderer), typeof(DatePickerCtrlRenderer))]

namespace CustomRenderer
{
    class MyEntryRenderer : EntryRenderer
    {
        public MyEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var nativeEditText = (global::Android.Widget.EditText)Control;

                RoundRectShape i = new RoundRectShape(
                         new float[] { 15, 15, 15, 15, 15, 15, 15, 15 },
                         null,
                         new float[] { 15, 15, 15, 15, 15, 15, 15, 15 });

                var shape = new ShapeDrawable(i);
                shape.Paint.Color = Xamarin.Forms.Color.FromRgb(184, 184, 184).ToAndroid();
                shape.Paint.SetStyle(Paint.Style.Stroke);
                nativeEditText.Background = shape;
                nativeEditText.SetPadding(25, 25, 25, 25);
            }
        }
    }

    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        public CustomDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            if (Control != null)
            {
                //var nativeEditText = (global::Android.Widget.EditText)Control;

                RoundRectShape i = new RoundRectShape(
                         new float[] { 15, 15, 15, 15, 15, 15, 15, 15 },
                         null,
                         new float[] { 15, 15, 15, 15, 15, 15, 15, 15 });

                var shape = new ShapeDrawable(i);
                shape.Paint.Color = Xamarin.Forms.Color.FromRgb(184, 184, 184).ToAndroid();
                shape.Paint.SetStyle(Paint.Style.Stroke);
                Control.Background = shape;
                Control.SetPadding(25, 25, 25, 25);
            }
        }
    }

    public class CustomPickerRenderer : PickerRenderer
    {
        public CustomPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Picker> e)
        {
            if (Control != null)
            {
                //var nativeEditText = (global::Android.Widget.)Control;

                RoundRectShape i = new RoundRectShape(
                         new float[] { 15, 15, 15, 15, 15, 15, 15, 15 },
                         null,
                         new float[] { 15, 15, 15, 15, 15, 15, 15, 15 });

                var shape = new ShapeDrawable(i);
                shape.Paint.Color = Xamarin.Forms.Color.FromRgb(184, 184, 184).ToAndroid();
                shape.Paint.SetStyle(Paint.Style.Stroke);
                Control.Background = shape;
                Control.SetPadding(25, 25, 25, 25);
            }
        }
    }

    class SearchEntryRenderer : EntryRenderer
    {
        public SearchEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var nativeEditText = (global::Android.Widget.EditText)Control;

                RoundRectShape i = new RoundRectShape(
                         new float[] { 15, 15, 15, 15, 15, 15, 15, 15 },
                         null,
                         new float[] { 15, 15, 15, 15, 15, 15, 15, 15 });

                var shape = new ShapeDrawable(i);
                shape.Paint.Color = Xamarin.Forms.Color.White.ToAndroid();
                shape.Paint.SetStyle(Paint.Style.Stroke);
                nativeEditText.Background = shape;
                nativeEditText.SetPadding(25, 25, 25, 25);
            }
        }
    }

    public class DatePickerCtrlRenderer : DatePickerRenderer
        {
        public DatePickerCtrlRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
            {
                base.OnElementChanged(e);

             this.Control.SetTextColor(Android.Graphics.Color.LightGray);
                this.Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                this.Control.SetPadding(20, 0, 0, 0);
                GradientDrawable gd = new GradientDrawable();
                gd.SetCornerRadius(25); //increase or decrease to changes the corner look
                gd.SetColor(Android.Graphics.Color.Transparent);
                gd.SetStroke(3, Android.Graphics.Color.LightGray);             
                this.Control.SetBackgroundDrawable(gd);

                DatePickerCtrl element = Element as DatePickerCtrl;

                if (!string.IsNullOrWhiteSpace(element.Placeholder))
                {
                    Control.Text = element.Placeholder;
                }
                this.Control.TextChanged += (sender, arg) => {
                    var selectedDate = arg.Text.ToString();
                    if (selectedDate == element.Placeholder)
                    {
                        Control.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                };
            }
        }

    // this in your namespace
    public class DesktopWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            Control.Settings.UserAgentString = "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.0.4) Gecko/20100101 Firefox/4.0";
        }
    }
}
