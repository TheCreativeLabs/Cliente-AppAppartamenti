﻿using AppAppartamenti.Views;
using AppAppartamenti.Api;
using AppAppartamenti.Views.Account;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                //Se ho il token allora vado direttamente alla home
                if (ApiHelper.GetToken() != null)
                {
                    ApiHelper.GetUserInfo();
                    ApiHelper.GetListaTipologiaProprieta();
                    ApiHelper.GetListaTipologiaAnnuncio();
                    Application.Current.MainPage = new MainPage();
                }
                else
                {
                    stkLogin.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("errore", "errore", "cancel");
            }
        }


        private async void btnAccedi_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                bool formIsValid = true;

                //Controllo che username e password siano valorizzati.
                if (String.IsNullOrEmpty(entUsername.Text))
                {
                    formIsValid = false;
                }

                if (String.IsNullOrEmpty(entPassword.Text))
                {
                    formIsValid = false;
                }

                //Se la form è valida allora setto il token
                if (formIsValid)
                {
                    await ApiHelper.SetTokenAsync(entUsername.Text, entPassword.Text);
                    Application.Current.MainPage = new MainPage();
                }
                else
                {
                    await DisplayAlert("Attenzione", "Compilare tutti i campi", "OK");
                }
            }
            catch (ApplicationException Ex)
            {
                //Se sono qui significa che non ho i diritti per accedere.
                await DisplayAlert("Attenzione", "L'indirizzo email o la password non sono validi.", "OK");
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new NavigationPage(new PasswordDimenticata()));
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void TapGestureRecognizer_lblRegistrati(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new NavigationPage(new Registrazione()));
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }
       
    }
}