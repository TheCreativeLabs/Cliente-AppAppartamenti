using System;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using AppAppartamenti.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(MyTabsRenderer))]

namespace AppAppartamenti.Droid
{

    public class MyTabsRenderer : TabbedPageRenderer
		{
			public MyTabsRenderer()
			{

			}
			public MyTabsRenderer(Context context)
			{

			}

			protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TabbedPage> e)
			{
				base.OnElementChanged(e);

				if (!(GetChildAt(0) is ViewGroup layout))
					return;

				if (!(layout.GetChildAt(1) is BottomNavigationView bottomNavigationView))
					return;

				var topShadow = LayoutInflater.From(Context).Inflate(Resource.Layout.view_shadow, null);

				var layoutParams =
					new Android.Widget.RelativeLayout.LayoutParams(LayoutParams.MatchParent, 15);
				layoutParams.AddRule(LayoutRules.Above, bottomNavigationView.Id);

				layout.AddView(topShadow, 2, layoutParams);

			}
		}
	}
