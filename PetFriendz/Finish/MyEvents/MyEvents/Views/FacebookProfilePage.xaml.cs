using FacebookLogin.ViewModels;
using MyEvents.Card;
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
        private Models.UsersManager usersManager;

        //Add Card Stack
        CardStackView cardStack;
        MainPageViewModel viewModel = new MainPageViewModel();
        public FacebookProfilePage()
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


            CardsStackView();
            //Add Card Stack
            CheckUserAvailability();
        }

        private async void CheckUserAvailability()
        {
            string email = this.lblfacebookProfileName.Text;

            var user = new Models.User { email = email };

            if (!string.IsNullOrEmpty(email))
            {

                Models.User userResponse = await usersManager.GetUserWhere(userSelect => userSelect.email == user.email);

                if (userResponse != null && userResponse.email.Equals(email, StringComparison.Ordinal))
                {
                    Application.Current.Properties["user"] = userResponse;


                    //  Application.Current.MainPage = new NavigationPage(new Dashboard());
                    await DisplayAlert("TEST", "UserFound", "ok", "ko");

                }
                else
                {
                    await DisplayAlert("TEST", "User Not Found", "ok", "ko");

                }
            }
        }

        private void CardsStackView()
        {
            this.BindingContext = viewModel;
            this.BackgroundColor = Color.Black;

            RelativeLayout view = new RelativeLayout();

            cardStack = new CardStackView();
            cardStack.SetBinding(CardStackView.ItemsSourceProperty, "ItemsList");
            cardStack.SwipedLeft += SwipedLeft;
            cardStack.SwipedRight += SwipedRight;

            view.Children.Add(cardStack,
                              Constraint.Constant(30),
                              Constraint.Constant(60),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Width - 60;
                              }),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Height - 140;
                              }));

            this.LayoutChanged += (object sender, System.EventArgs e) =>
            {
                cardStack.CardMoveDistance = (int)(this.Width * 0.60f);
            };

            this.Content = view;
        }

        void SwipedLeft(int index)
        {
            //DisplayAlert("TEST", "Elemento rechazado", "ok", "ko");
        }

        void SwipedRight(int index)
        {
            // DisplayAlert("TEST", "Elemento agregado", "ok", "ko");
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
        }

        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "");

                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                {
                    at = url.Replace("http://www.facebook.com/connect/login_success.html#access_token=", "");
                }

                var accessToken = at.Remove(at.IndexOf("&expires_in="));

                return accessToken;
            }

            return string.Empty;
        }

        async void SlideDownMenu_Clicked(object sender, EventArgs e)
        {

            var action = await DisplayActionSheet("Settings", "Cancel", null, "PetProfile", "LogOut");
            Debug.WriteLine("Action: " + action);
            switch (action)
            {
                case "PetProfile":
                    await Navigation.PushAsync(new ProfilePage());
                    break;
                case "LogOut":
                    await Navigation.PushAsync(new LoginMain());
                    break;

            }
        }
        private async void ProfileDetails_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        private void LogOut_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LoginMain());
        }



    }
}