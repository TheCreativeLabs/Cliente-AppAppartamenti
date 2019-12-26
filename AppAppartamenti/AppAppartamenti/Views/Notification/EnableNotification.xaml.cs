using System;
using System.Collections.Generic;
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

            Application.Current.MainPage = new MainPage();
            await AppCenter.SetEnabledAsync(true);
            SetDeviceInfo();

            button.IsEnabled = true;
        }


        private async void btnDisabled_ClickedAsync(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.IsEnabled = false;

            Application.Current.MainPage = new MainPage();
            await AppCenter.SetEnabledAsync(true);
            SetDeviceInfo();

            button.IsEnabled = true;
        }

        async void SetDeviceInfo()
        {
            NotificheClient notificheClient = new NotificheClient(Api.ApiHelper.GetApiClient());
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
