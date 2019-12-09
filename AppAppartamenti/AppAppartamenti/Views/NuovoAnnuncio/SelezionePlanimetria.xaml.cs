using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static AppAppartamenti.Views.SelezioneImmagini;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelezionePlanimetria : ContentPage
    {
        static Helpers.TranslateExtension translate = new Helpers.TranslateExtension();


        AnnuncioDtoInput annuncio;
        AnnuncioDetailViewModel dtoToModify;

        private List<ImageWithId> bytesImages = new List<ImageWithId>();

        public SelezionePlanimetria(AnnuncioDtoInput Annuncio, AnnuncioDetailViewModel dtoToModify)
        {
            this.dtoToModify = dtoToModify;
            InitializeComponent();
            annuncio = Annuncio;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (this.dtoToModify != null && this.dtoToModify.Immagini != null && this.dtoToModify.Immagini.Count != 0)
            {
                foreach (var item in dtoToModify.ImmaginiPlanimetria)
                {
                    int id = bytesImages.Count + 1;
                    ImageWithId imm = new ImageWithId() { Id = id, Image = item.Value };
                    bytesImages.Add(imm);
                }
                cvImmagini.ItemsSource = bytesImages.ToArray();
            }
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
            if (annuncio.ImmaginePlanimetria == null)
            {
                annuncio.ImmaginePlanimetria = new Collection<byte[]>();
            }
            foreach (var item in bytesImages)
            {
                annuncio.ImmaginePlanimetria.Add(item.Image);
            }

            await Navigation.PushAsync(new SelezioneDescrizione(annuncio, dtoToModify));
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

            //prendo solo 1 foto
            foreach (var item in listaImmagini)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    item.GetStream().CopyTo(memoryStream);
                    int id = bytesImages.Count + 1;
                    ImageWithId imm = new ImageWithId() { Id = id, Image = memoryStream.ToArray() };
                    bytesImages.Add(imm);
                    //mediaFileImages.Add(new MediaFileImage { File = item });
                }
            }

            cvImmagini.ItemsSource = bytesImages.ToArray();
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
            string scatta = Helpers.TranslateExtension.ResMgr.Value.GetString("SelezionePlanimetira.TakePicture", translate.ci);
            string galleria = Helpers.TranslateExtension.ResMgr.Value.GetString("SelezionePlanimetira.PhotoGallery", translate.ci);

            string action = await DisplayActionSheet(Helpers.TranslateExtension.ResMgr.Value.GetString("SelezionePlanimetira.UploadPhoto", translate.ci),
                Helpers.TranslateExtension.ResMgr.Value.GetString("SelezionePlanimetira.Cancel", translate.ci),
                null,
                scatta,
                galleria);

            if (action == scatta) {
                TakePhoto();
            }
            else if(action == galleria)
            {
                PickPhoto();
            }
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            var idImmagine = ((Button)sender).CommandParameter;
            bytesImages.RemoveAll(x => x.Id == (int)idImmagine);
            cvImmagini.ItemsSource = bytesImages.ToArray();
        }

    }
}