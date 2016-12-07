using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Demo.Views
{
    public partial class SignUp2 : ContentPage
    {
        UsersManager manager;
        public SignUp2()
        {
            InitializeComponent();

            manager = new UsersManager();
        }

        async Task AddUser(User user)
        {
            User userResponse = await manager.SaveGetUserAsync(user);
            Application.Current.Properties["user"] = userResponse;
        }

        async void OnSignUp_Clicked(object sender ,EventArgs e)
        {
            string email = this.emailEntry.Text;
            string password = this.passwordEntry.Text;

            if(!string.IsNullOrEmpty(email)&&!string.IsNullOrEmpty(password))
            {
                this.activityIndicator.IsRunning = true;
                this.signUpButton.IsEnabled = false;

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

                    ownername=string.Empty

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
    }
}
