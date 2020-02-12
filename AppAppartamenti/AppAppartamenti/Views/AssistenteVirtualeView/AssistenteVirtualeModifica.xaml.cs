using System;
using System.Collections.Generic;
using System.Linq;
using AppAppartamenti.Api;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views.AssistenteVirtualeView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssistenteVirtualeModifica : ContentPage
    {

        AssistenteVirtualeListViewModel viewModel;

        public AssistenteVirtualeModifica()
        {
            InitializeComponent();

            BindingContext = viewModel = new AssistenteVirtualeListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!viewModel.Items.Any())
                viewModel.LoadItemsCommand.Execute(null);
        }

        async void OrariCollectionView_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            try
            {
                var item = e.CurrentSelection;

                if (item == null)
                    return;

                //aggiorno i dati
                AccountClient accountClient = new AccountClient(await Api.ApiHelper.GetApiClient());
                await accountClient.UpdateAvatarCurrentUserAsync(((AvatarDtoOutput)item[0]).Id.Value);

                //Rimuovo dalle preferences l'icona
                ApiHelper.DeleteAssistenteVirtualeImage();
            }
            catch (Exception ex)
            {
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}
