using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.Api;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using DependencyServiceDemos;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RestSharp.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelezioneImmagini : ContentPage
    {

        private class MediaFileImage
        {
            public  MediaFile File { get; set; }
        }

        AnnuncioDtoInput annuncio;

        private List<MediaFileImage> mediaFileImages = new List<MediaFileImage>();

        public SelezioneImmagini(AnnuncioDtoInput Annuncio)
        {
            InitializeComponent();
            annuncio = Annuncio;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopAsync();
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }


        private async void BtnImmaginiProcedi_Clicked(object sender, EventArgs e)
        {
            foreach (var item in mediaFileImages)
            {
                MemoryStream memoryStream = new MemoryStream();
                item.File.GetStream().CopyTo(memoryStream);
                annuncio.Immagini.Add(memoryStream.ToArray());
            }


            Navigation.PushAsync(new SelezioneDescrizione(annuncio));
        }

        async void PickPhoto()
        {
            await CrossMedia.Current.Initialize();

            //if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
            //{
            //    DisplayAlert("No Camera", ":( No camera available.", "OK");
            //    return;
            //}

            //var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            //{
            //    Directory = "Sample",
            //    Name = "test.jpg"
            //});

            List<MediaFile> listaImmagini = await CrossMedia.Current.PickPhotosAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });

            if (listaImmagini == null)
                return;

            foreach (var item in listaImmagini)
            {
                mediaFileImages.Add(new MediaFileImage { File = item });
            }

            cvImmagini.ItemsSource = mediaFileImages;
        }

        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            //if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            //{
            //    DisplayAlert("No Camera", ":( No camera available.", "OK");
            //    return;
            //}

            //var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            //{
            //    Directory = "Sample",
            //    Name = "test.jpg"
            //});

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                PhotoSize = PhotoSize.Medium
            });

            if (file == null)
                return;

            //cvImmagini.ItemsSource = mediaFileImages;
        }

        async void ActionImmagini(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Carica una foto", "Cancel", null, "Scatta foto", "Galleria immagini");

            if (action == "Scatta foto") {
                TakePhoto();
            }
            else if(action == "Galleria immagini")
            {
                PickPhoto();
            }
        }

    }
}