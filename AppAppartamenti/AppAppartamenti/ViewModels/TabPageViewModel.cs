using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AppAppartamenti.Models;
using AppAppartamenti.Views;
using AppAppartamentiApiClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AppAppartamenti.ViewModels
{
    public class TabPageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public bool NewMessages { get; set; }
        public bool NewAppointement { get; set; }

        public Command LoadItemsCommand { get; set; }
        public Command ReloadItemsCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TabPageViewModel()
        {
            OnpropertyChanged("NewMessages");
        }

        void OnpropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}