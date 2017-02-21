
using MyEvents.Card;
using MyEvents.Models;
using MyEvents.ViewModels;
using MyEvents.ViewsSocial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class FacebookProfilePage : ContentPage
    {
        private string ClientId = "165942640479284";

        private User currentUser;
        private UsersManager usersManager;

        private List<PetPhoto> userList;
        private PetPhotoManager petphotoUserManager;

        public FacebookProfilePage()
        {
            InitializeComponent();

            // currentUser = (User)Application.Current.Properties["user"];

            // currentUser.email = lblfacebookProfileName.Text;

            string Owerneremail = lblfacebookProfileName.Text;

            userList = new List<PetPhoto>();

            petphotoUserManager = new PetPhotoManager();
            usersManager = new UsersManager();


            userListView.ItemTemplate = new DataTemplate(typeof(ImageRouteList));

            loadData();

            ////////////////////////////////////////////////////////////
            //facebook
            var apiRequest =
                "https://www.facebook.com/dialog/oauth?client_id="
                + ClientId
                + "&display=popup&response_type=token&redirect_uri=http://www.facebook.com/connect/login_success.html";

            var webView = new WebView
            {
                Source = apiRequest,
                HeightRequest = 1
            };

            webView.Navigated += WebViewOnNavigated;

            Content = webView;
    
        }
        //Photo
        public async void loadData()
        {
            try
            {
                userList = await petphotoUserManager.ListUserWhere(userSelect => userSelect.petname != lblfacebookProfileName.Text&& userSelect.datecreated <= DateTime.Now);

                if (userList.Count != 0)
                {
                    // userList.OrderByDescending(x => x.ID);
                    userListView.ItemsSource = userList.OrderByDescending(x => x.datecreated);
                    // userList.Sort();

                }
                else
                {
                    txtnorecord.Text = "No records";
                }
            }
            catch (Exception ex)
            {
                txtnorecord.Text = ex.Message;
            }
        }

        //facebook

        private async Task AddFacebookUserData(User newUser)
        {
            User userResponse = await usersManager.SaveGetUserAsync(newUser);
        }


        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {

            var accessToken = ExtractAccessTokenFromUrl(e.Url);

            if (accessToken != "")
            {
                var vm = BindingContext as FacebookViewModel;

                await vm.SetFacebookUserProfileAsync(accessToken);

                Content = MainStackLayout;

                // lblCheckName.Text = lblfacebookProfileName.Text;
                //Check and Compare

                User userResponse = await usersManager.GetUserWhere(userSelect => userSelect.email == lblfacebookProfileName.Text);
                if (!userResponse.email.Equals(lblfacebookProfileName.Text, StringComparison.Ordinal))
                {
                 //   await DisplayAlert("Incorrect", "Please Go to Profile enter your details", "Close");
                    string FacebookUserName = lblfacebookProfileName.Text;
                    var user = new User
                    {
                        email = FacebookUserName,
                        ownername = FacebookUserName

                    };
                    await AddUser(user);
                    await DisplayAlert("Alert", "Please Proceed to Profile enter your details", "Close");
                }
           

            }
           
        }

        private async Task AddUser(User user)
        {
            User userResponse = await usersManager.SaveGetUserAsync(user);
            Application.Current.Properties["user"] = userResponse;
        }

        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "");

                if (Xamarin.Forms.Device.OS == TargetPlatform.WinPhone || Xamarin.Forms.Device.OS == TargetPlatform.Windows)
                {
                    at = url.Replace("http://www.facebook.com/connect/login_success.html#access_token=", "");
                }

                var accessToken = at.Remove(at.IndexOf("&expires_in="));

                return accessToken;
            }

            return string.Empty;
        }

        private async void ProfileDetails_Clicked(object sender, EventArgs e)
        {
            string owneremail = lblfacebookProfileName.Text;
            await Navigation.PushAsync(new SocialProfile(owneremail));
        }

        private async void Friend_Clicked(object sender, EventArgs e)
        {
            string owneremail = lblfacebookProfileName.Text;
            await Navigation.PushAsync(new SocialFrenz(owneremail));
        }

        private async void Album_Clicked(object sender, EventArgs e)
        {
            string owneremail = lblfacebookProfileName.Text;
           
            await Navigation.PushAsync(new SocialOwnAlbum(owneremail));
        }

        private void LogOut_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LoginMain());
        }




    }
}