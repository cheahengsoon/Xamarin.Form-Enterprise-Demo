using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Demo.TestFolder
{
    public partial class FF1 : ContentPage
    {
        private User currentUser;
        private UsersManager usersManager;

        public FF1()
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
                    Application.Current.MainPage = new NavigationPage(new FF2());
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
    }
}
