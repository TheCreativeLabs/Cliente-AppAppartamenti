using System;
using Xamarin.Forms;
using Foundation;
using AppAppartamenti.iOS;
using AppAppartamenti.Notify;

[assembly: Dependency(typeof(ClearCookies))]  
namespace AppAppartamenti.iOS
{
        public class ClearCookies : IClearCookies
        {
            public void ClearAllCookies()
            {
                NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
                foreach (var cookie in CookieStorage.Cookies)
                    CookieStorage.DeleteCookie(cookie);
            }
        }
}
