using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppAppartamenti.Api;
using AppAppartamenti.NotificationSample;
using AppAppartamentiApiClient;
using Microsoft.AppCenter;
using Xamarin.Forms;

namespace AppAppartamenti.Views
{
    public partial class EnableNotification : ContentPage
    {
        public EnableNotification()
        {
            InitializeComponent();

        }

        private async void btnEnable_ClickedAsync(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.IsEnabled = false;

            await AppCenter.SetEnabledAsync(true);
            await SetDeviceInfo();

            Application.Current.MainPage = new MainPage();


            button.IsEnabled = true;
        }


        private async void btnDisabled_ClickedAsync(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.IsEnabled = false;

            await AppCenter.SetEnabledAsync(false);
            await SetDeviceInfo();

            Application.Current.MainPage = new MainPage();


            button.IsEnabled = true;
        }

        async Task SetDeviceInfo()
        {
            NotificheClient notificheClient = new NotificheClient(await Api.ApiHelper.GetApiClient());

            var os = "ANDROID";
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
            {
                os = "IOS";
            }

            var installationId = "";

            if (os == "ANDROID")
            {
                installationId = await ApiHelper.GetFirebaseToken();
            }
            else
            {
                var guid = await AppCenter.GetInstallIdAsync();
                installationId = guid.Value.ToString();
            }

            NotificationInfoDto notificationInfoDto = new NotificationInfoDto()
            {
                InstallationId = installationId,
                OsVersion = os
            };

            await notificheClient.UpdateInfoNotificationAsync(notificationInfoDto);
        }

    }
}
