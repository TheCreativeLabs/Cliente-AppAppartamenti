using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Widget;
using AppAppartamenti.Droid.Files;
using Xamarin.Forms;

[assembly: Dependency(typeof(CheckFilePermission))]
namespace AppAppartamenti.Droid.Files
{
    public class CheckFilePermission : AppAppartamenti.Files.ICheckFilePermission
    {
        MainActivity mainActivity;

        public async Task<bool> CheckPermission()
        {
            mainActivity = (AppAppartamenti.Droid.MainActivity)App.ParentWindow;
            if (mainActivity.CheckAppPermissions())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
