using GoogleLogin.Services;
using GoogleLogin.ViewModels;
using MyEvents.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class GoogleProfilePage : ContentPage
    {
        private readonly GoogleViewModel _googleViewModel;

        //Add Card Stack
        CardStackView cardStack;
        MainPageViewModel viewModel = new MainPageViewModel();

        private Models.UsersManager usersManager;
        public GoogleProfilePage()
        {
            InitializeComponent();
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

            //Add Card Stack View
            // CardsStackView();

            CheckUserAvailability();
        }
        private async void CheckUserAvailability()
        {
            string email = this.lblGoogleProfileName.Text;

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

            var code = ExtractCodeFromUrl(e.Url);

            if (code != "")
            {

                var accessToken = await _googleViewModel.GetAccessTokenAsync(code);

                await _googleViewModel.SetGoogleUserProfileAsync(accessToken);

                Content = MainStackLayout;
            }
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
    }
}