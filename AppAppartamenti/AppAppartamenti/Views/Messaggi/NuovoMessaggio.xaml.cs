using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppAppartamenti.Views.Messaggi
{
    public partial class NuovoMessaggio : ContentPage
    {

        public class Message
        {
            public string Text { get; set; }
            public string User { get; set; }
        }
        public static string User = "Rendy";


        public class ChatPageViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();
            public string TextToSend { get; set; }
            public ICommand OnSendCommand { get; set; }

            public ChatPageViewModel()
            {
                Messages.Add(new Message() { Text = "Hi" });
                Messages.Add(new Message() { Text = "How are you?" });

                OnSendCommand = new Command(() =>
                {
                    if (!string.IsNullOrEmpty(TextToSend))
                    {
                        Messages.Add(new Message() { Text = TextToSend, User = User });
                        TextToSend = string.Empty;
                    }

                });
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        public NuovoMessaggio()
        {
            InitializeComponent();

        }
    }
}
