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

        public DettaglioAnnuncio(Guid idAnnuncio)
        {
            InitializeComponent(); 
            
            BindingContext = viewModel = new AnnuncioDetailViewModel(idAnnuncio);
        }
    }
}