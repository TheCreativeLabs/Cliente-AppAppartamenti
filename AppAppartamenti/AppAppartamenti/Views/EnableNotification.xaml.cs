using System;
using System.Collections.Generic;
using AppAppartamenti.Api;
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
            ApiHelper.SetNotificationStatus(true);
           Application.Current.MainPage = new MainPage();
        }

        private async void btnDisabled_ClickedAsync(object sender, EventArgs e)
        {
            ApiHelper.SetNotificationStatus(true);
            Application.Current.MainPage = new MainPage();
        }
    }
}
