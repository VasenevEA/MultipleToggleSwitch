using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MultiSwitchSample
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public string[] Toggles { get; set; } = new string[] { "On", "Auto", "Off", "Some" };

        private int _id = 1;
        public int SelectedId
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("SelectedId");
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}
