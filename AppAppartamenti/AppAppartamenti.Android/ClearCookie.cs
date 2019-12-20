using System;
using Xamarin.Forms;
using Android.Webkit;
using AppAppartamenti.Notify;
using AppAppartamenti.Droid;

[assembly: Dependency(typeof(ClearCookies))]  
namespace AppAppartamenti.Droid
{
		public class ClearCookies : IClearCookies
		{
			public void ClearAllCookies()
			{
				var cookieManager = CookieManager.Instance;
				cookieManager.RemoveAllCookie();
			}
		}
	}
