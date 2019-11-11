using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AppAppartamenti;
using AppAppartamenti.iOS;
using CoreGraphics;

[assembly: ExportRenderer(typeof(SearchEntry), typeof(EntryCustomBorderRenderer))]
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
}