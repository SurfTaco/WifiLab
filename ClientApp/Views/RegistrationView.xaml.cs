using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ClientApp.Views
{
    public partial class RegistrationView : ContentView
    {
        public RegistrationView()
        {
            InitializeComponent();
            BindingContext = new RegistrationView();
        }
    }
}
