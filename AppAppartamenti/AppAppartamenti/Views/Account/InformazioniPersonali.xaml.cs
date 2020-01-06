using AppAppartamenti.Api;
using AppAppartamentiApiClient;
using DependencyServiceDemos;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace AppAppartamenti.Views.Account
{
    public partial class InformazioniPersonali : ContentPage
    {
        UserInfoDto viewModel;

        public InformazioniPersonali()
        {
            InitializeComponent();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            UserInfoDto userInfo = await ApiHelper.GetUserInfo(false);
            viewModel = userInfo;

            BindingContext = viewModel;

            if (userInfo.DataDiNascita.HasValue)
                dpDataNascita.Date = userInfo.DataDiNascita.Value.Date;
                dpDataNascita.PlaceholderHidden = true;

            if (viewModel.FotoProfilo != null)
            {
                imgFotoUtente.Source = ImageSource.FromStream(() => { return new MemoryStream(userInfo.FotoProfilo); });
            }
            else if (viewModel.PhotoUrl != null)
            {
                imgFotoUtente.Source = ImageSource.FromUri(new Uri(viewModel.PhotoUrl));
            }
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            UpdateUserBindingModel updateUserModel = new UpdateUserBindingModel() { 
             BirthName = viewModel.Nome,
             Name = viewModel.Nome,
             DataNascita = viewModel.DataDiNascita,
             Surname = viewModel.Cognome,
             ImmagineProfilo = viewModel.FotoProfilo,
             Email = viewModel.Email //FIXME, IL BE NON AGGIORNA LA MAIL: LO FACCIAMO??
            };

            AccountClient accountClient = new AccountClient( await ApiHelper.GetApiClient());
            await accountClient.UpdateUserAsync(updateUserModel);
        }

        async void OnPickPhotoButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            PickPhoto();

            (sender as Button).IsEnabled = true;
        }

        async void PickPhoto()
        {
            await CrossMedia.Current.Initialize();

            MediaFile foto = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small
            });

            if (foto == null)
                return;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                foto.GetStream().CopyTo(memoryStream);
                viewModel.FotoProfilo = memoryStream.ToArray();
                imgFotoUtente.Source = ImageSource.FromStream(() => { return new MemoryStream(viewModel.FotoProfilo); });
            }
        }
    }
}
