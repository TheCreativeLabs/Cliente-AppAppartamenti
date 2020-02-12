using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AppAppartamenti.ViewModels
{
    public class TimeSlotViewModel : INotifyPropertyChanged

    {

        #region Public Properties

        /// <summary>

        /// Area is the actual DataSource for SfPicker control which will holds the collection of Country and State

        /// </summary>

        /// <value>The area.</value>
        ///
        public ObservableCollection<object> Time { get; set; }

        public ObservableCollection<object> Ore { get; set; }

        //Country is the collection of country names

        private ObservableCollection<object> Minuti { get; set; }

        public ObservableCollection<string> Header { get; set; }

        private object _SelectedTime;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        //Identify the selected area using property changed method

        public object SelectedTime

        {

            get { return _SelectedTime; }

            set { _SelectedTime = value; RaisePropertyChanged("SelectedTime"); }

        }

        public TimeSlotViewModel()

        {

            Ore = new ObservableCollection<object>();

            Header = new ObservableCollection<string>();

            Minuti = new ObservableCollection<object>();

            Time = new ObservableCollection<object>();

            //populate Countries

            Ore.Add("06");

            Ore.Add("07");

            Ore.Add("08");

            Ore.Add("09");

            Ore.Add("10");

            Ore.Add("11");

            Ore.Add("12");

            Ore.Add("13");

            Ore.Add("14");

            Ore.Add("15");

            Ore.Add("16");

            Ore.Add("17");

            Ore.Add("18");

            Ore.Add("19");

            Ore.Add("20");

            Ore.Add("21");

            Ore.Add("22");

            Ore.Add("23");

            //populate states

            Minuti.Add("00");

            Minuti.Add("30");

            Header.Add("Ore");

            Header.Add("Minuti");

            Time.Add(Ore);

            Time.Add(Minuti);
        }

        //Hooked when changes occurred 

        public void RaisePropertyChanged(string name)

        {

            if (PropertyChanged != null)

                PropertyChanged(this, new PropertyChangedEventArgs(name));

        }

    }
}
