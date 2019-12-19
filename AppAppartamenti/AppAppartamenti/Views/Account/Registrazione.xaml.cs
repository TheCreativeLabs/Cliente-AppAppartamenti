using AppAppartamenti.Api;
using AppAppartamenti.Utility;
using DependencyServiceDemos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using AppAppartamentiApiClient;

namespace AppAppartamenti.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registrazione : ContentPage
    {
        byte[] img = null;
        static Helpers.TranslateExtension translate = new Helpers.TranslateExtension();

        public Registrazione()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void btnRegistrati_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool formIsValid = true;

                //Controllo validità del nome.
                if (String.IsNullOrEmpty(entNome.Text))
                {
                    formIsValid = false;
                    //lblValidatorEntNome.IsVisible = true;
                }

                //Controllo validità del cognome.
                if (String.IsNullOrEmpty(entCognome.Text))
                {
                    formIsValid = false;
                    //lblValidatorEntCognome.IsVisible = true;
                }

                //Controllo validità della mail.
                if (String.IsNullOrEmpty(entEmail.Text) || !Regex.IsMatch(entEmail.Text, Utility.Utility.EmailRegex))
                {
                    formIsValid = false;
                    //entEmail.BackgroundColor = Color.FromRgb(255, 175, 173);
                    //lblValidatorEntEmail.IsVisible = true;
                }

                //Controllo validità della password.
                if (String.IsNullOrEmpty(entPassword.Text) ||  !Regex.IsMatch(entPassword.Text, Utility.Utility.PasswordRegex))
                {
                    formIsValid = false;
                    //entPassword.BackgroundColor = Color.FromRgb(255, 175, 173);
                    //lblValidatorEntPassword.IsVisible = true;
                }

                //Controllo che le due password siano uguali.
                if (String.IsNullOrEmpty(entConfermaPassword.Text) || !(entPassword.Text == entConfermaPassword.Text))
                {
                    formIsValid = false;
                    //entConfermaPassword.BackgroundColor = Color.FromRgb(255, 175, 173);
                    //lblValidatorEntConfermaPassword.IsVisible = true;
                }

                //Se la form è valida proseguo con la registrazione.
                if (formIsValid)
                {
                    HttpClient httpClient = new HttpClient();
                    AccountClient accountClient = new AccountClient(httpClient);

                    //Creo il modello dei dati per la registrazione
                    RegisterUserBindingModel registerBindingModel = new RegisterUserBindingModel()
                    {
                        Name = entNome.Text,
                        Surname = entCognome.Text,
                        BirthName = entNome.Text,
                        ImmagineProfilo = img,
                        DataNascita = dpDataNascita.Date,
                        Email = entEmail.Text,
                        Password = entPassword.Text,
                        ConfirmPassword = entConfermaPassword.Text
                    };

                    //TODO: gestire la data di nascita

                    var response = await accountClient.RegisterAsync(registerBindingModel);
                }
                else
                {
                    await DisplayAlert("Attenzione", "Compilare tutti i campi", "OK");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "USER_ALREADY_EXIST")
                {
                    string alertTitle = Helpers.TranslateExtension.ResMgr.Value.GetString("Login.Warning", translate.ci);
                    string alertContent = Helpers.TranslateExtension.ResMgr.Value.GetString("Login.AlertMailAlreadySigned", translate.ci);
                    string alertOk = Helpers.TranslateExtension.ResMgr.Value.GetString("Login.AlertOk", translate.ci);

                    await DisplayAlert(alertTitle,
                        alertContent,
                        alertOk);
                    Api.ApiHelper.DeleteToken();
                    Application.Current.MainPage = new Login();
                } else
                {
                    //TODO Gestire l'errore navigando alla pagina specifica.
                }
            }
        }
        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Login());
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void ent_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //Controllo che username e password siano valorizzati.
                if (!(String.IsNullOrEmpty(entNome.Text)) && !(String.IsNullOrEmpty(entCognome.Text))
                    && !(String.IsNullOrEmpty(entPassword.Text)) && !(String.IsNullOrEmpty(entConfermaPassword.Text)) && !(String.IsNullOrEmpty(entEmail.Text)))
                {
                    btnRegistrati.IsEnabled = true;
                }
                else
                {
                    btnRegistrati.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async void OnPickPhotoButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                imgFotoUtente.Source = ImageSource.FromStream(() => stream);
                img = stream.ReadAsBytes();
            }

            (sender as Button).IsEnabled = true;
        }


        async void ActionImmagini(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Carica una foto", "Cancel", null, "Scatta foto", "Galleria immagini");

            if (action == "Scatta foto")
            {
                TakePhoto();
            }
            else if (action == "Galleria immagini")
            {
                PickPhoto();
            }
        }

        async void PickPhoto()
        {
            await CrossMedia.Current.Initialize();

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();


            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                cameraStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
            }

            if (cameraStatus != PermissionStatus.Granted && storageStatus != PermissionStatus.Granted)
            {
                DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            //var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            //{
            //    Directory = "Sample",
            //    Name = "test.jpg"
            //});

            var listaImmagini = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });

            if (listaImmagini == null)
                return;


            imgFotoUtente.Source = ImageSource.FromStream(() => listaImmagini.GetStream());
            imgFotoUtente.IsVisible = true;
            lblFoto.IsVisible = false;

        }

        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();


            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                cameraStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
            }

            if (cameraStatus != PermissionStatus.Granted && storageStatus != PermissionStatus.Granted)
            {
                DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            //var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            //{
            //    PhotoSize = PhotoSize.Medium
            //});

            MediaFile file = null;

            Device.BeginInvokeOnMainThread(async () =>
            {
                file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    DefaultCamera = CameraDevice.Rear,
                    Directory = "Library",
                    Name = "Media.jpg"
                });
            });

            if (file == null)
                return;

            //cvImmagini.ItemsSource = mediaFileImages;

            imgFotoUtente.Source = ImageSource.FromStream(() => file.GetStream());
            imgFotoUtente.IsVisible = true;
            //lblFoto.IsVisible = false;
        }

        private async void btnAccediFacebook_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool answer = await DisplayAlert("Liberacasa.it vuole utilizzare Facebook.com per accedere", "Questo permette all'app e al sito web di accedere alle tue informazioni.", "Si", "No");
                if (answer)
                {
                    await Navigation.PushAsync(new NavigationPage(new Account.FacebookLogin()));
                }
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnGoogleAuth_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool answer = await DisplayAlert("Liberacasa.it vuole utilizzare Google.com per accedere", "Questo permette all'app e al sito web di accedere alle tue informazioni.", "Si", "No");
                if (answer)
                {
                    await Navigation.PushAsync(new NavigationPage(new Account.GoogleLogin()));
                }
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}