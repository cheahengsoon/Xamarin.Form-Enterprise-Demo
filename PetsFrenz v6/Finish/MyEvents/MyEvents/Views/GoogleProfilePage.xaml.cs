
using MyEvents.Card;
using MyEvents.Models;
using MyEvents.Services;
using MyEvents.ViewModels;
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

        User currentUser;
        UsersManager usersManager;

        private List<PetPhoto> userList;
        private PetPhotoManager petPhotoUserManager;
        public GoogleProfilePage()
        {

            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            usersManager = new UsersManager();
            //////////////////////////////////////////////////////////
            currentUser = (User)Application.Current.Properties["user"];
            userList = new List<PetPhoto>();

            petPhotoUserManager = new PetPhotoManager();

            userListView.ItemTemplate = new DataTemplate(typeof(ImageRouteList));
            userListView.Refreshing += UserListView_Refreshing;
            userListView.ItemTapped += userListView_ItemTapped;
            loadData();
            /////////////////////////////////////////////////////////
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

          //  CheckUserAvailability();
        }

        //Photo
        private async void userListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var user = e.Item as PetPhoto;
            var nolikeuser = new PetPhoto
            {
                ID = user.ID,
                nolike = (Convert.ToInt32(user.nolike) + 1).ToString()
            };
            await UpdateUser(nolikeuser);


        }

        private async Task UpdateUser(PetPhoto nolikeuser)
        {
            PetPhoto userResponse = await petPhotoUserManager.SaveGetUserAsync(nolikeuser);
        }
        private void UserListView_Refreshing(object sender, EventArgs e)
        {
            loadData();
        }
        public async void loadData()
        {
            try
            {
                userList = await petPhotoUserManager.ListUserWhere(userSelect => userSelect.petname != currentUser.petname && userSelect.datecreated <= DateTime.Now);

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

        //private async void CheckUserAvailability()
        //{
        //    string email = this.lblGoogleProfileName.Text;

        //    var user = new Models.User { email = email };

        //    if (!string.IsNullOrEmpty(email))
        //    {

        //        Models.User userResponse = await usersManager.GetUserWhere(userSelect => userSelect.email == user.email);

        //        if (userResponse != null && userResponse.email.Equals(email, StringComparison.Ordinal))
        //        {
        //            Application.Current.Properties["user"] = userResponse;


        //            //  Application.Current.MainPage = new NavigationPage(new Dashboard());
        //            await DisplayAlert("TEST", "UserFound", "ok", "ko");

        //        }
        //        else
        //        {
        //            await DisplayAlert("TEST", "User Not Found", "ok", "ko");

        //        }
        //    }
        //}
        //private void CardsStackView()
        //{
        //    this.BindingContext = viewModel;
        //    this.BackgroundColor = Color.Black;

        //    RelativeLayout view = new RelativeLayout();

        //    cardStack = new CardStackView();
        //    cardStack.SetBinding(CardStackView.ItemsSourceProperty, "ItemsList");
        //    cardStack.SwipedLeft += SwipedLeft;
        //    cardStack.SwipedRight += SwipedRight;

        //    view.Children.Add(cardStack,
        //                      Constraint.Constant(30),
        //                      Constraint.Constant(60),
        //                      Constraint.RelativeToParent((parent) =>
        //                      {
        //                          return parent.Width - 60;
        //                      }),
        //                      Constraint.RelativeToParent((parent) =>
        //                      {
        //                          return parent.Height - 140;
        //                      }));

        //    this.LayoutChanged += (object sender, System.EventArgs e) =>
        //    {
        //        cardStack.CardMoveDistance = (int)(this.Width * 0.60f);
        //    };

        //    this.Content = view;
        //}

        //void SwipedLeft(int index)
        //{
        //    //DisplayAlert("TEST", "Elemento rechazado", "ok", "ko");
        //}

        //void SwipedRight(int index)
        //{
        //    // DisplayAlert("TEST", "Elemento agregado", "ok", "ko");
        //}


        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {

            var code = ExtractCodeFromUrl(e.Url);

            if (code != "")
            {

                var accessToken = await _googleViewModel.GetAccessTokenAsync(code);

                await _googleViewModel.SetGoogleUserProfileAsync(accessToken);

                Content = MainStackLayout;

                string GoogleUserName = lblGoogleProfileName.Text;
                var user = new User
                {
                    email = GoogleUserName,
                    ownername = GoogleUserName

                };
                await AddUser(user);
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
            await Navigation.PushAsync(new Profile());
        }

        private void LogOut_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LoginMain());
        }

        private async void Settings_Clicked()
        {
            //Temporary
            await Navigation.PushAsync(new Profile());
        }

        private async void Friends_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new FriendList());
            await Navigation.PushAsync(new NotificationList());
        }

        private async void Notice_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationList());
        }

        private async void Album_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImageList());
        }

    }
}
