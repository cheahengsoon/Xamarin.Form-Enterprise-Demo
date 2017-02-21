using MyEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class ForgetPassword : ContentPage
    {
        private User currentUser;
        private UsersManager usersManager;
        public ForgetPassword()
        {
            InitializeComponent();
            usersManager = new UsersManager();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void Reset_Clicked(object sender, EventArgs e)
        {
            string email = this.txtEmail.Text;
            //  string password = this.passwordEntry.Text;

            var user = new User { email = email };

            if (!string.IsNullOrEmpty(email))
            {
                activityIndicator.IsRunning = true;

                User userResponse = await usersManager.GetUserWhere(userSelect => userSelect.email == user.email);

                activityIndicator.IsRunning = false;

                if (userResponse != null && userResponse.email.Equals(email, StringComparison.Ordinal))
                {
                    Application.Current.Properties["user"] = userResponse;
                    string femail = userResponse.email;
                    string fpassword = userResponse.password;
                    SendEmail(femail, fpassword);
                    // Application.Current.MainPage = new NavigationPage(new Reset());
                }
                else
                {
                    await DisplayAlert("Incorrect", "Your email or password is incorrect, please try again.", "Close");
                    this.txtEmail.Text = "";
                    // this.passwordEntry.Text = "";
                }

            }
            else
            {
                await DisplayAlert("Incorrect", "The fields Email or Password can't be empty, please insert valid values.", "Close");
                this.txtEmail.Text = "";

            }
        }
        private string Uri = "https://sendgridsamplefcf9.azurewebsites.net/api/SendMail?";
        private void SendEmail(string femail, string fpassword)
        {
            var httpClient = new HttpClient();
            var result = httpClient.GetAsync($"{Uri}email={femail}&content={"Your Password :" + fpassword}");

            DisplayAlert("Forget Password", "Your password had been sent to your email.", "OK");
        }



    }
}