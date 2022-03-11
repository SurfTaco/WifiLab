using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using ClientApp.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ClientApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties
        private string _mailAddress;

        public string MailAdress{
            get { return _mailAddress; }
            set {
                _mailAddress = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands

        public ICommand Login { get; }

        public ICommand Register { get; }
        #endregion

        #region Methods

        private async Task ExecuteLogin(object e)
        {
            if (e is string myPass)
            {
                //Überprüfe, ob eine gültige Email-Adresse eingegeben wurde
                if (!IsValidEmail(MailAdress))
                {
                    await Application.Current.MainPage.DisplayAlert("Login", "Ungültige Email-Adresse",
                        "OK"); // TODO: Alerts usw. auslagern
                }
                else // weiter zum Login
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress =
                            new Uri("https://dominik.upperdevelopment.at");

                        httpClient.DefaultRequestHeaders.Add("email", MailAdress);
                        httpClient.DefaultRequestHeaders.Add("pass", myPass);
                        httpClient.Timeout = TimeSpan.FromSeconds(5); // nicht länger als 5 Sekunden probieren, danach Fehler werfen

                       HttpResponseMessage response =
                            await httpClient.PostAsync(httpClient.BaseAddress +
                            "api/Login/LoginWithData", null);


                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            Tuple<string, DTO.DTOCustomer> deserializedContent = JsonConvert.DeserializeObject<Tuple<string, DTO.DTOCustomer>>(content);

                            string token = deserializedContent.Item1;
                            DTO.DTOCustomer loggedInCustomer = deserializedContent.Item2;
                            Application.Current.MainPage = new MainPage(); // oder AppShell, was auch immer --> setzt die Root-Page der App neu
                        }

                        
                        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            await Application.Current.MainPage.DisplayAlert("Login", "Benutzer nicht vorhanden oder PW falsch", "OK");
                        }

                        else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        {
                            await Application.Current.MainPage.DisplayAlert("Login", "Server Error",
                        "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Login", "Unbekannter Fehler", "OK");
                        }
                        
                        
                    }
                }

            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ExecuteRegister()
        {
            //Application.Current.MainPage.Navigation.PushAsync(new RegistrationView());
        }
        #endregion

        public LoginViewModel()
        {
            Login = new Command(async(object e) => ExecuteLogin(e));
            Register = new Command(() => ExecuteRegister());
        }
    }
}
