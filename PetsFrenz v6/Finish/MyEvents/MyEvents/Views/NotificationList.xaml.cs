using MyEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class NotificationList : ContentPage
    {
        private User currentUser;

        private List<Friend> userList;
        // private UsersManager usersManager;

        private FriendManager frenzManager;
        public NotificationList()
        {
            InitializeComponent();

            currentUser = (User)Application.Current.Properties["user"];

            userList = new List<Friend>();
            //usersManager = new UsersManager();
            frenzManager = new FriendManager();

            userListView.ItemTemplate = new DataTemplate(typeof(RoutesCellFrienz));
            // userListView.ItemTapped += userListView_ItemTapped;
            userListView.Refreshing += userListView_Refreshing;

            LoadNotification();
        }

        private async void LoadNotification()
        {
            userListView.IsRefreshing = true;
            // userList = await usersManager.ListUserWhere(userSelect => userSelect.ID != currentUser.ID);
            // userList = await usersManager.ListUserWhere(userSelect => userSelect.petname != currentUser.petname);
            userList = await frenzManager.ListUserWhere(userSelect => userSelect.userpetname == currentUser.petname);
            if (userList.Count != 0)
            {
                userListView.ItemsSource = userList;
               
            }
            else
            {
                txtnorecord.Text = "No Friends Founded.";
            }
            userListView.IsRefreshing = false;
        }
        private void userListView_Refreshing(object sender, EventArgs e)
        {
            LoadNotification();
        }
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    LoadNotification();
        //}

        private async void friendlistProfile_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var user = e.Item as Friend;

            await Navigation.PushAsync(new FrenzChat(user));

        }

        private async void Friends_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FriendList());

        }
    }
}
