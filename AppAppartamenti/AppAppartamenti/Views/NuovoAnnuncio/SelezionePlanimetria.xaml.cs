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
            lbl_nuovoAnnuncio.IsVisible = dtoToModify == null;
            lbl_modificaAnnuncio.IsVisible = dtoToModify != null;
            annuncio = Annuncio;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (bytesImages.Count == 0 && annuncio.ImmaginePlanimetria != null && annuncio.ImmaginePlanimetria.Count > 0)
            {
                foreach (var item in annuncio.ImmaginePlanimetria)
                {
                    int id = bytesImages.Count + 1;
                    ImageWithId imm = new ImageWithId() { Id = id, Image = item };
                    bytesImages.Add(imm);
                }

            }
            else if (bytesImages.Count == 0 && this.dtoToModify != null && this.dtoToModify.Item.ImmaginiPlanimetria != null && this.dtoToModify.Item.ImmaginiPlanimetria.Count != 0)
            {
                foreach (var item in dtoToModify.Item.ImmaginiPlanimetria)
                {
                    int id = bytesImages.Count + 1;
                    ImageWithId imm = new ImageWithId() { Id = id, Image = item };
                    bytesImages.Add(imm);
                }
            }

            counter.Text = bytesImages.Count.ToString();
            cvImmagini.ItemsSource = bytesImages.ToArray();
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
            if (!bytesImages.Any())
            {
                await DisplayAlert("Campo obbligatorio", "Inserire almeno un'immagine", "Ok");
                return;
            }


            if (annuncio.ImmaginePlanimetria == null)
            {
                annuncio.ImmaginePlanimetria = new Collection<byte[]>();
            }
            annuncio.ImmaginePlanimetria.Clear();
            foreach (var item in bytesImages)
            {
                annuncio.ImmaginePlanimetria.Add(item.Image);
            }

            await Navigation.PushAsync(new SelezioneDescrizione(annuncio, dtoToModify));
        }

        async void PickPhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Attenzione", "Nonb è possibile collegarsi alla fotocamera", "OK");
                return;
            }

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

            if (listaImmagini.Count > 5 || bytesImages.Count > 5 || listaImmagini.Count + bytesImages.Count > 5)
            {
                await DisplayAlert("Attenzione", "È possibile caricare al massimo 5 immagini.", "OK");
                return;
            }

            //prendo solo 1 foto
            foreach (var item in listaImmagini)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    item.GetStream().CopyTo(memoryStream);
                    int id = bytesImages.Count + 1;
                    ImageWithId imm = new ImageWithId() { Id = id, Image = memoryStream.ToArray() };
                    bytesImages.Add(imm);
                    counter.Text = bytesImages.Count.ToString();
                    //mediaFileImages.Add(new MediaFileImage { File = item });
                }
            }

            cvImmagini.ItemsSource = bytesImages.ToArray();
        }

        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Attenzione", "Nonb è possibile collegarsi alla fotocamera", "OK");
                return;
            }

            //var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            //{
            //    Directory = "Sample",
            //    Name = "test.jpg"
            //});

            if (bytesImages.Count +1 >5)
            {
                await DisplayAlert("Attenzione", "È possibile caricare al massimo 5 immagini.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = false,
                PhotoSize = PhotoSize.Medium
            });

            if (file == null)
                return;

            //---INIZIO FIX CHIARA! HO AGGIUNTO QUESTO PEZZO, VERIFICARE SE MIGLIORA---
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                int id = bytesImages.Count + 1;
                ImageWithId imm = new ImageWithId() { Id = id, Image = memoryStream.ToArray() };
                bytesImages.Add(imm);
                cvImmagini.ItemsSource = bytesImages.ToArray();
                counter.Text = bytesImages.Count.ToString();
                //mediaFileImages.Add(new MediaFileImage { File = item });
            }
            //---FINE FIX CHIARA! HO AGGIUNTO QUESTO PEZZO, VERIFICARE SE MIGLIORA---

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

            if (annuncio.ImmaginePlanimetria == null)
            {
                annuncio.ImmaginePlanimetria = new Collection<byte[]>();
            }
            annuncio.ImmaginePlanimetria.Clear();
            foreach (var item in bytesImages)
            {
                annuncio.ImmaginePlanimetria.Add(item.Image);
            }

            counter.Text = bytesImages.Count.ToString();

        }

    }
}