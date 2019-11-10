using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DettaglioAnnuncio : ContentPage
    {
        AnnuncioDetailViewModel viewModel;

        public DettaglioAnnuncio(AnnuncioDtoOutput dto)
        {
            InitializeComponent();

            BindingContext = viewModel  = new AnnuncioDetailViewModel(dto);
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}