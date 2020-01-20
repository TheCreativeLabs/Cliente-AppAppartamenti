using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Util;
using AppAppartamenti.Droid.PlattformEffects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.RangeSlider;

[assembly: ResolutionGroupName("EffectsSlider")]
[assembly: ExportEffect(typeof(RangeSliderEffect), "RangeSliderEffect")]
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
}