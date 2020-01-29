using System;
using System.Linq;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Util;
using AppAppartamenti.Behaviors;
using AppAppartamenti.Droid.PlattformEffects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.RangeSlider;

[assembly: ResolutionGroupName("EffectsSlider")]
[assembly: ExportEffect(typeof(RangeSliderEffect), "RangeSliderEffect")]
[assembly: ExportEffect(typeof(LabelShadowEffect), "LabelShadowEffect")]
namespace AppAppartamenti.Droid.PlattformEffects
{
    public class RangeSliderEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var ctrl = (Xamarin.RangeSlider.RangeSliderControl)Control;

            //var thumbImage = new BitmapDrawable(ctrl.ThumbImage);
            //thumbImage.SetColorFilter(new PorterDuffColorFilter(themeColor, PorterDuff.Mode.SrcIn));
            Context context = Xamarin.Forms.Forms.Context;
            Bitmap icon = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.slider_small);

            //ctrl.ThumbImage = icon; //ConvertToBitmap(thumbImage, ctrl.ThumbImage.Width, ctrl.ThumbImage.Height);
            ctrl.SetCustomThumbImage(icon);

            //var thumbPressedImage = new BitmapDrawable(ctrl.ThumbPressedImage);
            //thumbPressedImage.SetColorFilter(new PorterDuffColorFilter(themeColor, PorterDuff.Mode.SrcIn));
            //ctrl.ThumbPressedImage = icon;// ConvertToBitmap(thumbPressedImage, ctrl.ThumbPressedImage.Width, ctrl.ThumbPressedImage.Height);
            ctrl.SetCustomThumbPressedImage(icon);
        }

        protected override void OnDetached()
        {
        }

        private static Bitmap ConvertToBitmap(Drawable drawable, int widthPixels, int heightPixels)
        {
            var mutableBitmap = Bitmap.CreateBitmap(widthPixels, heightPixels, Bitmap.Config.Argb8888);
            var canvas = new Canvas(mutableBitmap);
            drawable.SetBounds(0, 0, widthPixels, heightPixels);
            drawable.Draw(canvas);
            return mutableBitmap;
        }
    }

    public class LabelShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var control = Control as Android.Widget.TextView;
                var effect = (ShadowEffect)Element.Effects.FirstOrDefault(e => e is ShadowEffect);
                if (effect != null)
                {
                    float radius = effect.Radius;
                    float distanceX = effect.DistanceX;
                    float distanceY = effect.DistanceY;
                    Android.Graphics.Color color = effect.Color.ToAndroid();
                    control.SetShadowLayer(radius, distanceX, distanceY, color);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }
    }
}