using System;
using System.Collections.Generic;
using System.IO;
using AppAppartamenti.Api;
using AppAppartamenti.Views;
using AppAppartamentiApiClient;
using Xamarin.Forms;

namespace AppAppartamenti.ContentViews
{
    public partial class AssistenteVenditaButton : ContentView
    {
        public AssistenteVenditaButton()
        {
            InitializeComponent();
            OnAppearing();
        }

        protected async void OnAppearing()
        {
            //AccountClient accountClient = new AccountClient(await Api.ApiHelper.GetApiClient());
            byte[] ImmagineAssistente = await ApiHelper.GetVirtualAssistentImage();// accountClient.GetAvatarCurrentUserAsync();
            imgAssistente.Source = ImageSource.FromStream(() => new MemoryStream(ImmagineAssistente));
        }

        private async void BtnAssistente_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new AssistenteVirtuale(true));
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}
