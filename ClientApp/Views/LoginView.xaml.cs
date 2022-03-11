using System;
using System.Collections.Generic;
using ClientApp.ViewModels;
using Xamarin.Forms;

namespace ClientApp.Views
{
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
    }
}
