using MyEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class LoginMain : ContentPage
    {
        private User currentUser;
        private UsersManager usersManager;//=new UsersManager();
        public LoginMain()
        {
            InitializeComponent();
            usersManager = new UsersManager();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void SignIn_Clicked(object sender, EventArgs e)
        {
            string email = this.emailEntry.Text;
            string password = this.passwordEntry.Text;

            var user = new User { email = email, password = password };

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
              //  activityIndicator.IsRunning = true;

                User userResponse = await usersManager.GetUserWhere(userSelect => userSelect.email == user.email && userSelect.password == user.password);

             //   activityIndicator.IsRunning = false;

                if (userResponse != null && userResponse.email.Equals(email, StringComparison.Ordinal) && userResponse.password.Equals(password, StringComparison.Ordinal))
                {
                    Application.Current.Properties["user"] = userResponse;


                    Application.Current.MainPage = new NavigationPage(new ProfilePage());

                }
                else
                {
                    await DisplayAlert("Incorrect", "Your email or password is incorrect, please try again.", "Close");
                    this.emailEntry.Text = "";
                    this.passwordEntry.Text = "";
                }

            }
            else
            {
                await DisplayAlert("Incorrect", "The fields Email or Password can't be empty, please insert valid values.", "Close");
                this.emailEntry.Text = "";
                this.passwordEntry.Text = "";
            }
        }

        async void lblSignUp_Tapped(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new SignUp());

            //await NavigationPage(new SignUp());
            Application.Current.MainPage = new NavigationPage(new SignUp());

        }

    }
}
