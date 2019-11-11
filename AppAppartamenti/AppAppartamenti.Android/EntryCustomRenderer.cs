using System;
using System.Collections.Generic;
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

[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryRenderer))]
[assembly: ExportRenderer(typeof(SearchEntry), typeof(SearchEntryRenderer))]
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
}
