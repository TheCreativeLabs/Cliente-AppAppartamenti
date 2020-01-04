using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppAppartamenti.Api;
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
            var installationId = await AppCenter.GetInstallIdAsync();

            var os = "ANDROID";
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
            {
                os = "IOS";
            }

            NotificationInfoDto notificationInfoDto = new NotificationInfoDto()
            {
                InstallationId = installationId.Value,
                OsVersion = os
            };

            await notificheClient.UpdateInfoNotificationAsync(notificationInfoDto);
        }

    }
}
