
using MyEvents.Models;
using MyEvents.Services;
using MyEvents.ViewModels;
using MyEvents.ViewsSocial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class GoogleB : ContentPage
    {
        private readonly GoogleViewModel _googleViewModel;

        private List<PetPhoto> userList;
        private PetPhotoManager petphotoUserManager;

        private UsersManager usersManager;
        public GoogleB()
        {
            InitializeComponent();
            string Owerneremail = lblfacebookProfileName.Text;


            userList = new List<PetPhoto>();
            usersManager = new UsersManager();

            petphotoUserManager = new PetPhotoManager();

            userListView.ItemTemplate = new DataTemplate(typeof(ImageRouteList));

            loadData();

            //////////////////////////////////////////////////////////////
            _googleViewModel = BindingContext as GoogleViewModel;

            var authRequest =
                  "https://accounts.google.com/o/oauth2/v2/auth"
                  + "?response_type=code"
                  + "&scope=openid"
                  + "&redirect_uri=" + GoogleServices.RedirectUri
                  + "&client_id=" + GoogleServices.ClientId;

            var webView = new WebView
            {
                Source = authRequest,
                HeightRequest = 1
            };

            webView.Navigated += WebViewOnNavigated;

            Content = webView;

            //Compare and Check


        }
        public async void loadData()
        {
            try
            {
                userList = await petphotoUserManager.ListUserWhere(userSelect => userSelect.petname != lblfacebookProfileName.Text && userSelect.datecreated <= DateTime.Now);

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

        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {

            var code = ExtractCodeFromUrl(e.Url);

            if (code != "")
            {

                var accessToken = await _googleViewModel.GetAccessTokenAsync(code);

                await _googleViewModel.SetGoogleUserProfileAsync(accessToken);

                Content = MainStackLayout;

                //Check And Compare
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

        private string ExtractCodeFromUrl(string url)
        {
            if (url.Contains("code="))
            {
                var attributes = url.Split('&');

                var code = attributes.FirstOrDefault(s => s.Contains("code=")).Split('=')[1];

                return code;
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