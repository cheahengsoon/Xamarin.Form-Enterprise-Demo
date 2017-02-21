using MyEvents.Models;
using Plugin.Connectivity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Plugin.Connectivity.Abstractions;

namespace MyEvents.Views
{
    public partial class LoginMain : ContentPage
    {
        private User currentUser;
        private UsersManager usersManager;//=new UsersManager();

        public object Load { get; private set; }

        public LoginMain()
        {
            InitializeComponent();

            CrossConnectivity.Current.ConnectivityTypeChanged += Current_ConnectivityTypeChanged;
            //CheckConnection();
            usersManager = new UsersManager();

            NavigationPage.SetHasNavigationBar(this, false);
            
        }

        private async void Current_ConnectivityTypeChanged(object sender, ConnectivityTypeChangedEventArgs e)
        {
            if(!e.IsConnected)
            {
                await DisplayAlert("Error", "Please Check for your network connection.Thank You.", "OK");
            }
        }



        //install the Xam.Plugin.Connectivity
        private async void CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Incorrect", "Please Check your network connection. Thank You.", "Close");
            }
        
        }



        //async void KeepLoggedIn()
        //{
        //    string email = this.emailEntry.Text;
        //    string password = this.passwordEntry.Text;

        //    var user = new User { email = email, password = password };

        //    if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        //    {
        //        activityIndicator.IsRunning = true;

        //        User userResponse = await usersManager.GetUserWhere(userSelect => userSelect.email == user.email && userSelect.password == user.password);

        //        activityIndicator.IsRunning = false;

        //        if (userResponse != null && userResponse.email.Equals(email, StringComparison.Ordinal) && userResponse.password.Equals(password, StringComparison.Ordinal))
        //        {
        //            Application.Current.Properties["user"] = userResponse;


        //            Application.Current.MainPage = new NavigationPage(new Dashboard());

        //            //Test
        //            // Application.Current.MainPage = new NavigationPage(new FriendList());
        //        }
        //        else
        //        {
        //            await DisplayAlert("Incorrect", "Your email or password is incorrect, please try again.", "Close");
        //            this.emailEntry.Text = "";
        //            this.passwordEntry.Text = "";
        //        }

        //    }
        //    else
        //    {
        //        await DisplayAlert("Incorrect", "The fields Email or Password can't be empty, please insert valid values.", "Close");
        //        this.emailEntry.Text = "";
        //        this.passwordEntry.Text = "";
        //    }
        //}

        async void SignIn_Clicked(object sender, EventArgs e)
        {
            string email = this.emailEntry.Text;
            string password = this.passwordEntry.Text;

            var user = new User { email = email, password = password };

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                activityIndicator.IsRunning = true;

                User userResponse = await usersManager.GetUserWhere(userSelect => userSelect.email == user.email && userSelect.password == user.password);

                activityIndicator.IsRunning = false;

                if (userResponse != null && userResponse.email.Equals(email, StringComparison.Ordinal) && userResponse.password.Equals(password, StringComparison.Ordinal))
                {
                    Application.Current.Properties["user"] = userResponse;


                    Application.Current.MainPage = new NavigationPage(new DashboardB());

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

        async void lblForgetPassword_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgetPassword());



        }

        async void LoginWithFacebook_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FacebookProfilePage());
        }


        async void LoginWithGoogle_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GoogleB());
        }

        async void lblSignUp_Tapped(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new SignUp());

            //await NavigationPage(new SignUp());
            Application.Current.MainPage = new NavigationPage(new SignUp());

        }

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Temporary Storage for Phone Number from txtxPhoneNumber
            var email = emailEntry.Text;
            Application.Current.Properties["Email"] = email;
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Temporary Storage for Phone Number from txtxPhoneNumber
            var password = passwordEntry.Text;
            Application.Current.Properties["Password"] = password;
        }
    }
}