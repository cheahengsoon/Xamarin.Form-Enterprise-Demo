using MyEvents.Models;
using MyEvents.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class FacebookB : ContentPage
    {
        private string ClientId = "165942640479284";
        User currentUser;
        UsersManager usermanager;
        public FacebookB()
        {
            InitializeComponent();
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

        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {

            var accessToken = ExtractAccessTokenFromUrl(e.Url);

            if (accessToken != "")
            {
                var vm = BindingContext as FacebookViewModel;

                await vm.SetFacebookUserProfileAsync(accessToken);

                Content = MainStackLayout;
            }
            string facebookUserName;
            facebookUserName = lblFacebookUserName.Text;

            var newUser = new User
            {
                ownername = lblFacebookUserName.Text

            };
            await AddFacebookUser(newUser);

        }

        private async Task AddFacebookUser(User newUser)
        {
            User userResponse = await usermanager.SaveGetUserAsync(newUser);
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
    }
}