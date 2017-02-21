using MyEvents.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class SignUp : ContentPage
    {
        // private string Uri = "http://petdba.azurewebsites.net/api/EmployeesApi";
        UsersManager manager;
        public SignUp()
        {
            InitializeComponent();
            manager = new UsersManager();
        }

        private async Task AddUser(User user)
        {
            User userResponse = await manager.SaveGetUserAsync(user);
            Application.Current.Properties["user"] = userResponse;
        }

        private async void OnSignUp_Clicked(object sender, EventArgs e)
        {
            try
            {
                string email = this.emailEntry.Text;
                string password = this.passwordEntry.Text;

                // TODO: Improve email validation.
                // Password Length & numeric &so on.

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    // TODO: see whether can change the spinner colour.
                    this.activityIndicator.IsRunning = true;
                    this.signUpButton.IsEnabled = false;

                    // var accountController = new AccountController();
                    // accountController.CheckEmailExistence(email);

                    // await AddUserAsync();

                    var user = new User
                    {
                        email = email,
                        password = password,

                        petname = string.Empty,
                        petage = string.Empty,
                        petgender = string.Empty,
                        petimage = string.Empty,
                        pettype = string.Empty,

                        nolike = string.Empty,

                        ownername = string.Empty

                    };
                    await AddUser(user);
                    await Navigation.PushModalAsync(new Profile());
                    await Navigation.PopAsync();
                }
                else
                {
                    this.emailEntry.PlaceholderColor = this.emailEntry.TextColor = Color.FromHex("#f44336");
                    this.passwordEntry.PlaceholderColor = this.passwordEntry.TextColor = Color.FromHex("#f44336");
                    await DisplayAlert("Incorrect", "The fields Email or Password can't be empty, please insert valid values.", "Close");
                    this.emailEntry.Text = "";
                    this.passwordEntry.Text = "";
                }
            }
            catch (Exception ex)
            {
                //ex.Message;
                // await DisplayAlert("Incorrect", ex.Message, "Close");
            }
        }

        //private async Task AddUserAsync()
        //{

        //    var httpClient = new HttpClient();

        //    string email = this.emailEntry.Text;
        //    string password = this.passwordEntry.Text;

        //    var user = new User
        //    {
        //        email = email,
        //        password = password
        //    };

        //    var json = JsonConvert.SerializeObject(user);
        //    HttpContent httpContent = new StringContent(json);
        //    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    var result = await httpClient.PostAsync(Uri, httpContent);

        //}


        private async void lblPrivacyPolicy_Tapped(object sender, EventArgs e)
        {
            // Xamarin.Forms.Device.OpenUri(new Uri("PetFrenzPrivacyStatement.pdf"));
            await Navigation.PushAsync(new PrivacyPolicy());
            // Xamarin.Forms.Device.OpenUri(new Uri("http://petfrenz.azurewebsites.net/PetFrenzPrivacyStatement.pdf"));

        }

        private void PasswordTextChanged(object sender, EventArgs e)
        {
            string password = this.txtConfirmPassword.Text;
            if (txtConfirmPassword.Text != passwordEntry.Text)
            {
                lblPasswordMatch.IsVisible = true;
                signUpButton.IsEnabled = false;
            }
            else
            {
                lblPasswordMatch.IsVisible = false;
                signUpButton.IsEnabled = true;
            }

            if (!string.IsNullOrEmpty(password))
            {
                this.passwordEntry.PlaceholderColor = this.passwordEntry.TextColor = Color.FromHex("#00695C");
            }
            // signUpButton.IsEnabled = password.Length > 0 ? true : false;
        }

        async void EmailTextChanged(object sender, EventArgs e)
        {
            Entry emailEntry = (Entry)sender;

            string email = this.emailEntry.Text;

            if (!string.IsNullOrEmpty(email))
            {
                this.emailEntry.PlaceholderColor = this.emailEntry.TextColor = Color.FromHex("#00695C");
                this.activityIndicator.IsRunning = true;

                signUpButton.IsEnabled = false;
                User usersSelect = await manager.GetUserWhere(userSelect => userSelect.email == email);
                this.lblEmailExists.IsVisible = usersSelect != null ? true : false;
                signUpButton.IsEnabled = usersSelect != null ? true : false;
                this.signUpButton.IsEnabled = !lblEmailExists.IsVisible;

                this.activityIndicator.IsRunning = false;

            }
        }
    }
}

